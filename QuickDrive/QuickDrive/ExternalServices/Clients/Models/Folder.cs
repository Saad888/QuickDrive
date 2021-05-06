using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDrive.ExternalServices.ServiceClients.Models
{
    public class Folder
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Folder> Children { get; set; }

        public Folder(Google_GetFilesListOutput_Files file)
        {
            ID = file.Id;
            Name = file.Name;
            Children = null;
        }

        public Folder(string id, string name)
        {
            ID = id;
            Name = name;
            Children = null;
        }
    }
}
