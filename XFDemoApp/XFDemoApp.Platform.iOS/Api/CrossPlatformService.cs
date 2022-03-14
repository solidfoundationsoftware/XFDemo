using Foundation;
using Photos;
using System;
using System.Threading.Tasks;
using XFDemoApp.Platform.Api;

[assembly: Xamarin.Forms.Dependency(typeof(XFDemoApp.Platform.iOS.Api.CrossPlatformService))]

namespace XFDemoApp.Platform.iOS.Api
{
    public class CrossPlatformService : ICrossPlatformService
    {
        public async Task<APIResult> SaveImageToPhotoAlbumAsync(string albumName, string imageFileName, byte[] image)
        {
            if (string.IsNullOrEmpty(albumName)) return await Task.FromResult(new APIResult(false, APIConstants.ERROR_ALBUM_NAME_MISSING));
            if (string.IsNullOrEmpty(imageFileName)) return await Task.FromResult(new APIResult(false, APIConstants.ERROR_IMAGE_FILE_NAME_MISSING));
            if (image == null || image.Length == 0) return await Task.FromResult(new APIResult(false, APIConstants.ERROR_IMAGE_DATA_MISSING));

            bool savingAllowed = true;
            var source = new TaskCompletionSource<APIResult>();

            var tempAlbum = GetPhotoAlbum(albumName);

            if (tempAlbum == null)
            {
                var createPhotoAlbumResult = await CreatePhotoAlbum(albumName);

                if (createPhotoAlbumResult.Success)
                {
                    tempAlbum = GetPhotoAlbum(albumName);

                    if (tempAlbum == null) savingAllowed = false;
                }
                else
                {
                    savingAllowed = false;
                    source.TrySetResult(new APIResult(false, APIConstants.ERROR_ACCESS_PHOTO_DENIED));
                }
            }

            if (savingAllowed)
            {
                PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(() =>
                {
                    var saveImageRequest = PHAssetCreationRequest.CreationRequestForAsset();

                    var creationOptions = new PHAssetResourceCreationOptions { };
                    saveImageRequest.AddResource(PHAssetResourceType.Photo, NSData.FromArray(image), creationOptions);

                    var albumToChangeRequest = PHAssetCollectionChangeRequest.ChangeRequest(tempAlbum);

                    var photo = saveImageRequest.PlaceholderForCreatedAsset;

                    albumToChangeRequest.AddAssets(new PHObject[] { photo });

                }, (savePhotoSuccess, savePhotoError) =>
                    {
                        var apiResult = new APIResult(savePhotoSuccess, savePhotoError?.LocalizedDescription);
                        source.TrySetResult(apiResult);
                    });
            }

            return await source.Task;
        }

        private async Task<APIResult> CreatePhotoAlbum(string albumName)
        {
            if (string.IsNullOrEmpty(albumName)) return await Task.FromResult(new APIResult(false, APIConstants.ERROR_ALBUM_NAME_MISSING));

            var source = new TaskCompletionSource<APIResult>();

            //create the album
            PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(() =>
            {
                var createAlbumRequest = PHAssetCollectionChangeRequest.CreateAssetCollection(albumName);
            }, (success, error) =>
            {
                source.TrySetResult(new APIResult(success, error?.LocalizedDescription));
            });

            return await source.Task;
        }

        private PHAssetCollection GetPhotoAlbum(string albumName)
        {
            if (string.IsNullOrEmpty(albumName)) return null;

            var userCreatedAlbums = PHCollectionList.FetchTopLevelUserCollections(null);

            if (userCreatedAlbums != null && userCreatedAlbums.Count > 0)
            {
                foreach (var item in userCreatedAlbums)
                {
                    var album = item as PHAssetCollection;

                    if (album != null && album.LocalizedTitle.Equals(albumName, StringComparison.OrdinalIgnoreCase))
                    {
                        return album;
                    }
                }
            }

            return null;
        }
    }
}