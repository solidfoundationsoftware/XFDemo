using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using XFDemoApp.Models;
using XFDemoApp.Views;

namespace XFDemoApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        protected const string NO_DATA_MESSAGE = "You have no listings in your inventory";
        private Listing _selectedItem;

        public ObservableCollection<Listing> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Listing> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Inventory";
            Items = new ObservableCollection<Listing>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Listing>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task OnAppearing()
        {
            if (SelectedItem == null)
            {
                await ExecuteLoadItemsCommand();
            }
        }

        public Listing SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Listing item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.PMListingId}");
        }

        string noDataMessage = NO_DATA_MESSAGE;
        public string NoDataMessage
        {
            get => noDataMessage;
            set
            {
                SetProperty(ref noDataMessage, value);
            }
        }
    }
}