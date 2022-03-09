using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XFDemoApp.Models;
using XFDemoApp.ViewModels;

namespace XFDemoApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Listing Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}