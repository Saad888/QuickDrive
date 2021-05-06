using System;
using System.Collections.Generic;
using System.Text;

using QuickDrive.Globals;
using QuickDrive.ExternalServices.ServiceClients.Models;

namespace QuickDrive.ExternalServices.Authentication
{
    public class Token
    {
        private string AccessToken { get; set; }
        private string RefreshToken { get; set; }
        private int ExpireTimeShift { get; set; } = 60;
        private DateTime ExpiresAt { get; set; }

        public Token(Dictionary<string, string> properties, DriveServices service)
        {
            AccessToken = properties["token_type"] + " " + properties["access_token"];
            RefreshToken = properties["refresh_token"];
            var expireTime = Int32.Parse(properties["expires_in"]);

            ExpiresAt = DateTime.Now.AddMinutes(expireTime - ExpireTimeShift);
            if (GlobalPreferences.Setting_SaveRefreshToken)
            {
                GlobalPreferences.SetRefreshToken(service, RefreshToken);
            }
        }

        public Token(RefreshTokenOutput output, string refreshToken)
        {
            AccessToken = output.TokenType + " " + output.AccessToken;
            RefreshToken = refreshToken;
            ExpiresAt = DateTime.Now.AddSeconds(output.ExpiresIn - ExpireTimeShift);
        }

        public string GetAccessToken()
        {
            if (AccessToken == null) return null;
            if (DateTime.Now > ExpiresAt) return null;
            return AccessToken;
        }
        public static string GetStoredRefreshToken(DriveServices service)
        {
            return GlobalPreferences.Setting_SaveRefreshToken ? GlobalPreferences.GetRefreshToken(service) : null;
        }

        public string GetRefreshToken() => RefreshToken;
    }
}
