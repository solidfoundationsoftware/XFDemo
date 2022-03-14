using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XFDemoApp.Models;

namespace XFDemoApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemDetailViewModel()
        {
            savePromoPhotoCommand = new Command<byte[]>(async (image) => await SavePromoPhoto(image), (p) => !SavePromoPhotoInProgress);
        }

        private readonly Command savePromoPhotoCommand;
        public ICommand SavePromoPhotoCommand => savePromoPhotoCommand;

        private bool savePromoPhotoInProgress;
        public bool SavePromoPhotoInProgress
        {
            get { return savePromoPhotoInProgress; }
            set
            {
                SetProperty(ref savePromoPhotoInProgress, value);
                savePromoPhotoCommand.ChangeCanExecute();
            }
        }

        private async Task SavePromoPhoto(byte[] image)
        {
            try
            {
                SavePromoPhotoInProgress = true;

                var fileName = $"{(string.IsNullOrEmpty(SelectedListing?.PMListingId) ? "promo_photo" : SelectedListing.PMListingId)}.jpg";
                var savePhotoResult = await CrossPlatformService.SaveImageToPhotoAlbumAsync("XFDemoApp", fileName, image);

                if (!savePhotoResult.Success) throw new Exception(savePhotoResult.FailureMessage);

                await Application.Current.MainPage.DisplayAlert("SAVE PHOTO", "PROMO PHOTO WAS SAVED SUCCESSFULLY!", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("FAILURE", ex.Message, "OK");
            }
            finally
            {
                SavePromoPhotoInProgress = false;
            }
        }


        private string itemId;
        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                _ = LoadItemId(value);
            }
        }

        private Listing selectedListing;
        public Listing SelectedListing { get => selectedListing; set => SetProperty(ref selectedListing, value); }

        public async Task LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                SelectedListing = item;

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
