using System;
using System.Collections.Generic;
using System.Text;

using QuickDrive.Globals;

namespace QuickDrive.ExternalServices.Authentication
{
    public class Token
    {
        private string AccessToken { get; set; }
        private string RefreshToken { get; set; }
        private string TokenType { get; set; }
        private int ExpireTime { get; set; } = 50;
        private DateTime ExpiresAt { get; set; }

        public Token(Dictionary<string, string> properties, DriveServices service)
        {
            AccessToken = properties["access_token"];
            RefreshToken = properties["refresh_token"];
            TokenType = properties["token_type"];

            ExpiresAt = DateTime.Now.AddMinutes(ExpireTime);
            if (GlobalPreferences.Setting_SaveRefreshToken)
            {
                GlobalPreferences.SetRefreshToken(service, RefreshToken);
            }
        }

        public Token(string token, string refreshToken, string tokenType)
        {
            AccessToken = token;
            RefreshToken = refreshToken;
            TokenType = tokenType;
            ExpiresAt = DateTime.Now.AddMinutes(ExpireTime);
        }

        public string GetAccessToken()
        {
            if (AccessToken == null) return null;
            if (DateTime.Now > ExpiresAt) return null;
            return AccessToken;
        }
        public static string GetStoredRefreshToken(DriveServices service)
        {
            return GlobalPreferences.GetRefreshToken(service);
        }

        public string GetRefreshToken() => RefreshToken;

        public string GetTokenType() => TokenType;
    }
}
