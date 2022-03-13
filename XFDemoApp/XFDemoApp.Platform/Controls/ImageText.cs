using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFDemoApp.Platform.Controls
{
    public class ImageText : Image
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ImageText), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        public static readonly BindableProperty TextPaddingProperty = BindableProperty.Create(nameof(TextPadding), typeof(int), typeof(ImageText), 50);

        public int TextPadding
        {
            get { return (int)GetValue(TextPaddingProperty); }
            set { SetValue(TextPaddingProperty, value); }
        }

        public static readonly BindableProperty CompositeImageProperty = BindableProperty.Create(nameof(CompositeImage), typeof(byte[]), typeof(ImageText), null);

        public byte[] CompositeImage
        {
            get { return (byte[])GetValue(CompositeImageProperty); }
            set { SetValue(CompositeImageProperty, value); }
        }
    }
}
