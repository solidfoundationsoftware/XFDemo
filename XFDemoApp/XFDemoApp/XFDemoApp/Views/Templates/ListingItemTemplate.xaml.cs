using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFDemoApp.Platform;
using XFDemoApp.Platform.Effects;

namespace XFDemoApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListingItemTemplate : ContentView
    {
        public ListingItemTemplate()
        {
            InitializeComponent();
        }
    }
}