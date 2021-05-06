using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using QuickDrive.ExternalServices.ServiceClients;
using QuickDrive.ExternalServices.ServiceClients.Models;

namespace QuickDrive.ExternalServices
{
    public static class CloudStorageService
    {
        #region Properties and Private Fields
        private static Dictionary<DriveServices, ServiceClientBase> Clients { get; set; }
        #endregion

        #region Constructor
        static CloudStorageService()
        {
            Clients = new Dictionary<DriveServices, ServiceClientBase>();
            Clients.Add(DriveServices.GoogleDrive, new GoogleClient());
            Clients.Add(DriveServices.OneDrive, new MicrosoftClient());
        }
        #endregion

        #region Public Methods
        public async static Task<List<DriveServices>> GetAuthenticatedServices()
        {
            var services = new List<DriveServices>();
            foreach(var service in Clients.Keys)
            {
                if (await Clients[service].IsAuthenticated())
                    services.Add(service);
            }
            return services;
        }

        public async static Task<bool> IsAuthenticated(DriveServices service) => await Clients[service].IsAuthenticated();
        public async static Task AuthenticateService(DriveServices service) => await Clients[service].Authenticate();
        public static void RemoveAuthentication(DriveServices service) => Clients[service].RemoveAuthentication();
        public async static Task UploadFiles(DriveServices service, List<UploadFile> files, string parentFolderId) => await Clients[service].UploadFiles(files, parentFolderId);
        public async static Task<Folder> GetFolders(DriveServices service, string parent) => await Clients[service].GetFolders(parent);
        public async static Task CreateFolder(DriveServices service, string folderName, string parent) => await Clients[service].CreateFolder(folderName, parent);
        #endregion

        #region Private Methods

        #endregion
    }
}
