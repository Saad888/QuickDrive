using System.Collections.Generic;

namespace QuickDrive.ExternalServices.Authentication
{
    public class AuthCredentialsModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthUrl { get; set; }
        public string TokenUrl { get; set; }
        public string RefreshUrl { get; set; }
        public List<string> Scopes { get; set; }
    }
}
