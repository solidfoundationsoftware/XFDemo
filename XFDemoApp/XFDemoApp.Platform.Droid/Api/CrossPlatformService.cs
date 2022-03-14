using Android.App;
using Android.Content;
using Android.Provider;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XFDemoApp.Platform.Api;

[assembly: Xamarin.Forms.Dependency(typeof(XFDemoApp.Platform.Droid.Api.CrossPlatformService))]

namespace XFDemoApp.Platform.Droid.Api
{
    public class CrossPlatformService : ICrossPlatformService
    {
        public async Task<APIResult> SaveImageToPhotoAlbumAsync(string albumName, string imageFileName, byte[] image)
        {
            if (string.IsNullOrEmpty(albumName)) return new APIResult(false, APIConstants.ERROR_ALBUM_NAME_MISSING);
            if (string.IsNullOrEmpty(imageFileName)) return new APIResult(false, APIConstants.ERROR_IMAGE_FILE_NAME_MISSING);
            if (image == null || image.Length == 0) return new APIResult(false, APIConstants.ERROR_IMAGE_DATA_MISSING);

            var saveImageError = string.Empty;
            var saveImageResult = false;

            try
            {
                // We only need the permission when accessing the file, but it's more natural
                // to ask the user first, then show the picker.
                if (await new Permissions.StorageWrite().CheckAndRequestPermissionAsync() != PermissionStatus.Granted) 
                    return new APIResult(false, APIConstants.ERROR_ACCESS_PHOTO_DENIED);

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                {
                    var values = new ContentValues();
                    values.Put(MediaStore.Images.Media.InterfaceConsts.Title, imageFileName);
                    values.Put(MediaStore.Images.Media.InterfaceConsts.RelativePath, "Pictures/" + albumName);
                    values.Put(MediaStore.Images.Media.InterfaceConsts.IsPending, true);

                    var uri = Application.Context.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, values);

                    if (uri != null)
                    {
                        using (var fileStream = Application.Context.ContentResolver.OpenOutputStream(uri))
                        {
                            fileStream.Write(image);
                        }

                        values.Clear();
                        values.Put(MediaStore.Images.Media.InterfaceConsts.IsPending, false);
                        Application.Context.ContentResolver.Update(uri, values, null, null);

                        saveImageResult = true;
                    }
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var jFolder = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), albumName);
                    if (!jFolder.Exists())
                        jFolder.Mkdirs();

                    var destinationPath = System.IO.Path.Combine(jFolder.AbsolutePath, imageFileName);

                    System.IO.File.WriteAllBytes(destinationPath, image);

                    saveImageResult = true;

                    //refresh the media database
                    var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(destinationPath)));
                    Application.Context.SendBroadcast(mediaScanIntent);
                }
            }
            catch (Exception ex)
            {
                saveImageError = ex.Message;
            }

            return new APIResult(saveImageResult, saveImageError);
        }
    }
}