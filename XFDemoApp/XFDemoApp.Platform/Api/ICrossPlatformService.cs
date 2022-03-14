using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XFDemoApp.Platform.Api
{
    public interface ICrossPlatformService
    {
        Task<APIResult> SaveImageToPhotoAlbumAsync(string albumName, string imageFileName, byte[] image);
    }
}
