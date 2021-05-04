using System;

using Xamarin.Essentials;

using QuickDrive.ExternalServices;

namespace QuickDrive.Globals
{
    public static class GlobalPreferences
    {
        /// <summary>
        /// Trigger to Save Refresh Token from google API
        /// </summary>
        public static bool Setting_SaveRefreshToken
        {
            get { return Preferences.Get("Setting_SaveRefreshToken", false); }
            set { Preferences.Set("Setting_SaveRefreshToken", value); }
        }

        // -----------------------------------
        // Per service preferences
        private static string ServiceName(ExternalServices.Services service) => Enum.GetName(typeof(ExternalServices.Services), service);

        // Refresh Token (Per Service)
        private static string RefreshTokenKey(ExternalServices.Services service) => ServiceName(service) + "RefreshToken";
        public  static string GetRefreshToken(ExternalServices.Services service) => Preferences.Get(RefreshTokenKey(service), null);
        public  static void   SetRefreshToken(ExternalServices.Services service, string value) => Preferences.Set(RefreshTokenKey(service), value);
    }
}
