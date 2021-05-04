using QuickDrive.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace QuickDrive.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}