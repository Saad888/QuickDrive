using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;

namespace QuickDrive.ExternalServices.Authentication
{
    public static class AuthCredentialsReader
    {
        private static Dictionary<string, AuthCredentialsModel> Credentials { get; set; }

        static AuthCredentialsReader()
        {
            // Get credentials from Embdeded Reader
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "QuickDrive.credentials.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                Credentials = JsonConvert.DeserializeObject<Dictionary<string, AuthCredentialsModel>>(streamReader.ReadToEnd());
            }
        }

        public static AuthCredentialsModel GetCredentials(Services service)
        {
            return Credentials?[Enum.GetName(typeof(Services), service)];
        }
    }
}
