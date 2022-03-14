using System;
using System.Collections.Generic;
using System.Text;

namespace XFDemoApp.Platform.Api
{
    public static class APIConstants
    {
        public const string ERROR_ALBUM_NAME_MISSING = "Album name cannot be empty.";
        public const string ERROR_IMAGE_FILE_NAME_MISSING = "Image file name cannot be empty.";
        public const string ERROR_IMAGE_DATA_MISSING = "Valid image needs to be provided.";
        public const string ERROR_ACCESS_PHOTO_DENIED = "We need full access to your photos in order to create an album.";
    }
}
