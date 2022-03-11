using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFDemoApp.Platform;

namespace XFDemoApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListingItemTemplate : ContentView
    {
        public ListingItemTemplate()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create
            (
                nameof(ItemTappedCommand),
                typeof(ICommand),
                typeof(ListingItemTemplate),
                null
            );

        public ICommand ItemTappedCommand
        {
            get => (ICommand)GetValue(ItemTappedCommandProperty);
            set => SetValue(ItemTappedCommandProperty, value);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (frameContainer.Effects.Count == 0)
            {
                frameContainer.Effects.Add(new DropShadowColorEffect { Color = (Color)App.Current.Resources["AccentColor"] });
            }
        }
    }
}