using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using QuickDrive.ExternalServices.ServiceClients.Models;

namespace QuickDrive.ExternalServices.ServiceClients
{
    public abstract class ServiceClientBase
    {
        internal TokenClient Token { get; set; }

        public async Task<bool> IsAuthenticated() => await Token.IsAuthenticated();
        public async Task Authenticate() => await Token.GetAccessToken();
        public void RemoveAuthentication() => Token.RemoveAuthentication();

        public abstract Task UploadFiles(List<UploadFile> files, string parentFolderId);
        public abstract Task<Folder> GetFolders(string parentFolderId);
        public abstract Task CreateFolder(string folderName, string parentFolderId);
    }
}
