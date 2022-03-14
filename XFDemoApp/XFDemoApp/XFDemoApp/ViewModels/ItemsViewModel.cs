using System;
using System.Collections.Generic;
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

        public Command<string> SearchCommand { get; }

        public ItemsViewModel()
        {
            Title = "Inventory";
            Items = new ObservableCollection<Listing>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            SearchCommand = new Command<string>(async p => await ExecuteSearchCommand(p));

            SearchOptions.Add(new PickerOption<ListingSortOrder>(ListingSortOrder.None, "None"));
            SearchOptions.Add(new PickerOption<ListingSortOrder>(ListingSortOrder.TitleAscending, "Title Asc"));
            SearchOptions.Add(new PickerOption<ListingSortOrder>(ListingSortOrder.TitleDescending, "Title Desc"));
            SearchOptions.Add(new PickerOption<ListingSortOrder>(ListingSortOrder.PriceAscending, "Price Asc"));
            SearchOptions.Add(new PickerOption<ListingSortOrder>(ListingSortOrder.PriceDescending, "Price Desc"));

            selectedSearchOption = SearchOptions[0];
        }

        private void PopulateListings(IEnumerable<Listing> listings)
        {
            Items.Clear();

            if (listings != null)
            {
                foreach (Listing listing in listings)
                {
                    Items.Add(listing);
                }
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                PopulateListings(await DataStore.GetItemsAsync(SelectedSearchOption?.Key ?? ListingSortOrder.None));
                SearchText = String.Empty;
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

        async Task ExecuteSearchCommand(string searchTerm)
        {
            IsBusy = true;

            try
            {
                SearchText = searchTerm;
                PopulateListings(await DataStore.SearchItemsAsync(searchTerm, SelectedSearchOption?.Key ?? ListingSortOrder.None));
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
            else
            {
                SelectedItem = null;
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

        async void OnItemSelected(Listing item)
        {
            if (item == null)
                return;

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

        private string searchText;
        public string SearchText { get => searchText; set => SetProperty(ref searchText, value); }

        public ObservableCollection<PickerOption<ListingSortOrder>> SearchOptions { get; } = new ObservableCollection<PickerOption<ListingSortOrder>>();

        private PickerOption<ListingSortOrder> selectedSearchOption;
        public PickerOption<ListingSortOrder> SelectedSearchOption 
        { 
            get => selectedSearchOption;
            set
            {
                if (SetProperty(ref selectedSearchOption, value))
                {
                    _ = ExecuteSearchCommand(SearchText);
                }
            }
        }
    }
}