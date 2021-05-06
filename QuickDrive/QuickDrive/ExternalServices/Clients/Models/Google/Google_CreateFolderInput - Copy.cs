using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuickDrive.ExternalServices.ServiceClients.Models
{
    public class Google_CreateFileInput
    {
        public string name { get; set; }
        public List<string> parents { get; set; }

        public Google_CreateFileInput(string n, string p)
        {
            name = n;
            parents = new List<string>() { p };
        }
    }
}
