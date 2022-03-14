using SkiaSharp;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using XFDemoApp.ViewModels;

namespace XFDemoApp.Views
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