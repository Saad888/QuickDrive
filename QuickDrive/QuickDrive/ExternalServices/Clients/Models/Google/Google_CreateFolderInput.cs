using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuickDrive.ExternalServices.ServiceClients.Models
{
    public class Google_CreateFolderInput
    {
        public string name { get; set; }
        public string mimeType { get; set; }
        public List<string> parents { get; set; }

        public Google_CreateFolderInput(string n, string m, string p)
        {
            name = n;
            mimeType = m;
            parents = new List<string>() { p };
        }
    }
}
