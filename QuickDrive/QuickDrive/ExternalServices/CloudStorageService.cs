using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using QuickDrive.ExternalServices.ServiceClients;

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
            //Clients.Add(DriveServices.GoogleDrive, )
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

        public async static Task AuthenticateService(DriveServices service) => await Clients[service].Authenticate();
        public async static Task RemoveAuthentication(DriveServices service) => await Clients[service].RemoveAuthentication();
        public async static Task UploadFiles(DriveServices service, List<Byte[]> files) => await Clients[service].UploadFiles(files);
        public async static Task GetFolders(DriveServices service, string parent) => await Clients[service].GetFolders(parent);
        public async static Task CreateFolder(DriveServices service, string folderName, string parent) => await Clients[service].CreateFolder(folderName, parent);
        #endregion

        #region Private Methods

        #endregion
    }
}
