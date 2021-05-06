using Android.App;
using Android.Content;
using Android.OS;

using System;


namespace QuickDrive.Droid.OAuth
{
    [Activity(Label = "AuthInterceptor")]
    [IntentFilter(actions: new[] { Intent.ActionView },
                  Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
                  DataSchemes = new[]
                  {
                    "oauth2redirect"
                  },
                  DataPaths = new[]
                  {
                    "//com.teraflaredevelopment.quickdrive"
                  })]
    public class AuthInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;

            var uri_netfx = new Uri(uri_android.ToString());

            AccessTokenReader.Auth?.OnPageLoading(uri_netfx);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            Finish();
        }
    }
}
// CHECK THIS FOR MICROSOFT AUTHENTICATION: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet