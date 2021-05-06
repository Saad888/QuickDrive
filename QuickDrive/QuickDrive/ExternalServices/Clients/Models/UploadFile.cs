using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace QuickDrive.ExternalServices.ServiceClients.Models
{
    public class UploadFile
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }

        public long FileSize() => Content.Length;

        public UploadFile(string name, Stream stream)
        {
            Name = name;
            using (var streamReader = new MemoryStream())
            {
                stream.CopyTo(streamReader);
                Content = streamReader.ToArray();
            }
        }

        public UploadFile(string name, byte[] content)
        {
            Name = name;
            Content = content;
        }
    }
}
