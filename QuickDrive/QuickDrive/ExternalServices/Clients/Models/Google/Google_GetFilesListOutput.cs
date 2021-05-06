using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuickDrive.ExternalServices.ServiceClients.Models
{
    public class Google_GetFilesListOutput
    {
        public string nextPageToken { get; set; }
        public List<Google_GetFilesListOutput_Files> Files { get; set; }
    }

    public class Google_GetFilesListOutput_Files
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Parents { get; set; }
    }

}
