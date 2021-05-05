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
        private static string ServiceName(ExternalServices.DriveServices service) => Enum.GetName(typeof(ExternalServices.DriveServices), service);

        // Refresh Token (Per Service)
        private static string RefreshTokenKey(ExternalServices.DriveServices service) => ServiceName(service) + "RefreshToken";
        public  static string GetRefreshToken(ExternalServices.DriveServices service) => Preferences.Get(RefreshTokenKey(service), null);
        public  static void   SetRefreshToken(ExternalServices.DriveServices service, string value) => Preferences.Set(RefreshTokenKey(service), value);
    }
}
