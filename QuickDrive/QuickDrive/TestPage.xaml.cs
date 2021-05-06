using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Media;
using Plugin.Media.Abstractions;

using QuickDrive.ExternalServices;
using QuickDrive.ExternalServices.ServiceClients.Models;

namespace QuickDrive
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            CloudStorageService.AuthenticateService(DriveServices.OneDrive);

        }
    }
}