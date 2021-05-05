using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

using QuickDrive.Globals;
using QuickDrive.Exceptions;
using QuickDrive.ExternalServices.Authentication;
using QuickDrive.ExternalServices.ServiceClients.RequestModels;

using Xamarin.Forms;

namespace QuickDrive.ExternalServices.ServiceClients
{
    public class TokenClient
    {
        private Token Token { get; set; }
        private DriveServices Service { get; set; }

        public TokenClient(DriveServices service)
        {
            Service = service;
        }

        public async Task<string> GetAccessToken()
        {
            // Get access token
            var accessToken = Token?.GetAccessToken();
            if (!String.IsNullOrEmpty(accessToken))
                return accessToken;

            // If token is null, get refresh token
            var refreshToken = Token != null ? Token.GetRefreshToken() : Token.GetStoredRefreshToken(Service);
            if (String.IsNullOrEmpty(refreshToken))
            {
                await GetAccessTokenFromOAuth();
            }
            else
            {
                await RefreshAccessToken(refreshToken);
            }

            return Token.GetAccessToken();
        }

        private async Task GetAccessTokenFromOAuth()
        {
            // If refresh token is null, get access token from OAuth
            var credentials = AuthCredentialsReader.GetCredentials(Service);
            var reader = DependencyService.Get<IAccessTokenReader>();
            Token = await reader.GetAccessToken(credentials, Service);
        }

        private async Task RefreshAccessToken(string refreshToken)
        {
            // If refresh token is not null, query refresh endpoint
            var credentials = AuthCredentialsReader.GetCredentials(Service);
            var reqObj = new Dictionary<string, string>();
            reqObj["client_id"] = credentials.ClientId;
            reqObj["client_secret"] = credentials.ClientSecret;
            reqObj["grant_type"] = "refresh_token";
            reqObj["refresh_token"] = refreshToken;

            try
            {
                var output = await ApiClient.QueryService<GoogleRefreshTokenOutput>(credentials.RefreshUrl, reqObj);
                Token = new Token(output.AccessToken, refreshToken, output.TokenType);
            }
            catch (HttpResponseFailedException e)
            {
                throw new RefreshTokenFailedException("Refresh token query failed", e);
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            // Check if access token
            var accessToken = Token?.GetAccessToken();
            if (!String.IsNullOrEmpty(accessToken))
                return true;

            // Get refresh token
            var refreshToken = Token != null ? Token.GetRefreshToken() : Token.GetStoredRefreshToken(Service);
            if (!String.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    await RefreshAccessToken(refreshToken);
                    return !String.IsNullOrEmpty(Token?.GetAccessToken());
                }
                catch (RefreshTokenFailedException)
                {
                    return false;
                }
            }

            return false;
        }

        public void RemoveAuthentication()
        {
            // Revoke token and remove saved refresh token
            Token = null;
            GlobalPreferences.SetRefreshToken(Service, null);
        }
    }
}
