using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;

using QuickDrive.ExternalServices.ServiceClients.Models;
using QuickDrive.Exceptions;

namespace QuickDrive.ExternalServices.ServiceClients
{
    public class GoogleClient : ServiceClientBase
    {
        private const int MAX_ATTEMPTS = 2;
        private const string API_FILES = "https://www.googleapis.com/drive/v3/files";
        private const string API_GET_ROOT_FILE = "https://www.googleapis.com/drive/v3/files/root";
        private const string API_FILE_UPLOAD = "https://www.googleapis.com/upload/drive/v3/files?uploadType=resumable";

        public GoogleClient()
        {
            Token = new TokenClient(DriveServices.GoogleDrive);
        }

        #region Upload Files
        public async override Task UploadFiles(List<UploadFile> files, string parentFolderId = "root")
        {
            // Get access token
            var token = await Token.GetAccessToken();

            // For each set of files
            foreach(var file in files)
            {
                // STEP 1: SEND INITIAL REQUEST
                // Generate post data and headers
                var data = new Google_CreateFileInput(file.Name, parentFolderId);

                // Start intial POST request with resumable upload
                var response = await ApiClient.QueryService(API_FILE_UPLOAD, HttpMethod.Post, null, data, token);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.ReasonPhrase);

                // Get the location URL
                var locationUrl = response.Headers.GetValues("Location").First();

                // STEP 2: UPLOAD CONTENT
                // Generate post data and headers
                HttpResponseMessage newResponse = null;
                newResponse = await ApiClient.QueryService(locationUrl, HttpMethod.Put, null, null, token, file.Content);

                // If succeeded, move on
                if (newResponse != null && newResponse.IsSuccessStatusCode)
                    continue;

                // If 308, resume
                while (newResponse == null || (int)newResponse.StatusCode == 308)
                {
                    var check_header = new ContentRangeHeaderValue(file.FileSize());
                    newResponse = await ApiClient.QueryService(locationUrl, HttpMethod.Put, auth_token: token, contentRange:check_header, Expect308: true);
                    int start;
                    try
                    {
                        var rangeStr = newResponse.Headers.GetValues("Range").First().Replace("bytes=0-", "");
                        var range = Convert.ToInt32(rangeStr);
                        start = range + 1;
                    } 
                    catch(Exception e)
                    {
                        throw new FileUploadFailedException("Range could not be aquired on HTTP PUT Google Drive", e);
                    }

                    var fileContent = file.Content.Skip(start).ToArray();
                    var contentRange = new ContentRangeHeaderValue(start, file.FileSize() - 1, file.FileSize());
                    newResponse = await ApiClient.QueryService(locationUrl, HttpMethod.Put, null, null, token, fileContent, contentRange);
                }

                if (!newResponse.IsSuccessStatusCode)
                    throw new FileUploadFailedException(await newResponse.Content.ReadAsStringAsync(), new HttpRequestException());
            }
        }
        #endregion

        #region Get Folders
        public async override Task<Folder> GetFolders(string parentFolderId)
        {
            // Get auth token
            var token = await Token.GetAccessToken();

            // Get root ID
            var rootIdResponse = await ApiClient.QueryService<Dictionary<string, string>>(API_GET_ROOT_FILE, HttpMethod.Get, null, null, token);
            var rootId = rootIdResponse["id"];

            // Get files list
            List<Google_GetFilesListOutput_Files> filesList = new List<Google_GetFilesListOutput_Files>();
            Google_GetFilesListOutput filesListResponse = new Google_GetFilesListOutput() { nextPageToken = "" };

            var attempts = 0;
            bool success = false;
            while (attempts < MAX_ATTEMPTS && !success)
            {
                try
                {
                    var now = DateTime.Now;
                    do
                    {
                        var query = new Dictionary<string, string>();
                        query.Add("corpora", "user");
                        query.Add("q", "mimeType = 'application/vnd.google-apps.folder'");
                        query.Add("fields", "files/id, files/parents/*, files/name, nextPageToken");
                        query.Add("pageToken", filesListResponse.nextPageToken);
                        query.Add("pageSize", "1000");

                        filesListResponse = await ApiClient.QueryService<Google_GetFilesListOutput>(API_FILES, HttpMethod.Get, query, null, token);
                        filesList.AddRange(filesListResponse.Files);
                    } while (!String.IsNullOrEmpty(filesListResponse.nextPageToken));
                    var time = (DateTime.Now - now).TotalMilliseconds;
                    success = true;
                } 
                catch (HttpRequestException e)
                {
                    attempts++;
                    filesList = new List<Google_GetFilesListOutput_Files>();
                    filesListResponse = new Google_GetFilesListOutput() { nextPageToken = "" };

                    if (attempts >= MAX_ATTEMPTS)
                        throw e;
                }
            }
            

            // 1) Create dictionary
            var nodeRelations = new Dictionary<string, List<Folder>>();
            foreach (var file in filesList)
            {
                if (file == null || file.Id == null || file.Name == null || file.Parents == null)
                    continue;

                var node = new Folder(file);
                foreach (var parent in file.Parents)
                {
                    if (!nodeRelations.ContainsKey(parent))
                        nodeRelations.Add(parent, new List<Folder>());

                    nodeRelations[parent].Add(node);
                }
            }

            // 2) Recursively add children to their parents
            var rootNode = new Folder(rootId, "root");
            AddChildNodes(rootNode, nodeRelations);

            return rootNode;
        }

        private void AddChildNodes(Folder node, Dictionary<string, List<Folder>> relations)
        {
            // If node already has children, return
            if (node.Children != null)
                return;

            // If no children, set as empty list 
            if (!relations.ContainsKey(node.ID))
                node.Children = new List<Folder>();
            else // Add children to node
                node.Children = relations[node.ID];

            // Foreach child, recurse AddChildNodes
            foreach(var child in node.Children)
                AddChildNodes(child, relations);
        }
        #endregion

        #region Create Folder
        public async override Task CreateFolder(string folderName, string parentFolderId = "root") 
        {
            // Get access token
            var token = await Token.GetAccessToken();

            // Create request body
            var data = new Google_CreateFolderInput(folderName, "application/vnd.google-apps.folder", parentFolderId);

            // Request
            var result = await ApiClient.QueryService<Dictionary<string, string>>(API_FILES, HttpMethod.Post, null, data, token);
        }
        #endregion
    }
}
