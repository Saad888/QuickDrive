using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuickDrive.ExternalServices.ServiceClients
{
    public abstract class ServiceClientBase
    {
        TokenClient Token;

        public async Task<bool> IsAuthenticated() => await Token.IsAuthenticated();
        public async Task Authenticate() => await Token.GetAccessToken();
        public void RemoveAuthentication() => Token.RemoveAuthentication();

        public abstract Task UploadFiles(List<byte[]> files);
        public abstract Task GetFolders(string parentFolderId);
        public abstract Task CreateFolder(string folderName, string parentFolderId);
    }
}
