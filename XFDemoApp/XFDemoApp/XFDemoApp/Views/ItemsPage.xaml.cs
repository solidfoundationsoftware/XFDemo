using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFDemoApp.Models;
using XFDemoApp.Platform;
using XFDemoApp.Platform.Effects;
using XFDemoApp.ViewModels;
using XFDemoApp.Views;
using XFDemoApp.Views.Templates;

namespace XFDemoApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }

        private void ItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine($"\nITEM SELECTED: {e.CurrentSelection}/{e.PreviousSelection}");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Debug.WriteLine($"\nLIST ITEM SELECTED: {sender}");
            if (sender is ListingItemTemplate listingItem)
            {
                if (listingItem.Effects.Count == 0)
                {
                    //(Color)App.Current.Resources["AccentColor"]
                    listingItem.Effects.Add(new DropShadowColorEffect { Color = Color.Red });
                }
            }
        }

        private void pickerSearchOptions_Unfocused(object sender, FocusEventArgs e)
        {

        }
    }
}