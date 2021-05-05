using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Auth;
using Xamarin.Forms;

using Plugin.CurrentActivity;

using QuickDrive.Globals;
using QuickDrive.Exceptions;
using QuickDrive.ExternalServices;
using QuickDrive.ExternalServices.Authentication;

[assembly: Dependency(typeof(QuickDrive.Droid.OAuth.AccessTokenReader))]
namespace QuickDrive.Droid.OAuth
{
    public class AccessTokenReader : IAccessTokenReader
    {
        private static Token Token { get; set; }
        private static DriveServices CurrentService { get; set; }
        public static OAuth2Authenticator Auth;

        public async Task<Token> GetAccessToken(AuthCredentialsModel credentials, DriveServices currentService)
        {
            CurrentService = currentService;
            Token = null;

            Auth = new OAuth2Authenticator(
                credentials.ClientId,
                credentials.ClientSecret,
                String.Join(" ", credentials.Scopes),
                new Uri(credentials.AuthUrl),
                GlobalConstants.REDIRECT_URI,
                new Uri(credentials.TokenUrl),
                isUsingNativeUI: true
            );
            Auth.Completed += OnAuthenticationCompleted;

            CustomTabsConfiguration.CustomTabsClosingMessage = null;

            var intent = Auth.GetUI(CrossCurrentActivity.Current.AppContext);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);

            while (!Auth.HasCompleted)
            {
                await Task.Delay(500);
            }

            if (!Auth.IsAuthenticated() || Token == null)
            {
                throw new OAuthRefusedException();
            }

            return Token;
        }

        private void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                Token = new Token(e.Account.Properties, CurrentService);
            }
        }
    }
}