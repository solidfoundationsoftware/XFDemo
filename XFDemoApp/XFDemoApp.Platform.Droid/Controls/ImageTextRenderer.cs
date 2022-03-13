using Android.Content;
using Android.Graphics;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFDemoApp.Platform.Controls;
using FastRenderers = Xamarin.Forms.Platform.Android.FastRenderers;
using XFColor = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(XFDemoApp.Platform.Controls.ImageText), typeof(XFDemoApp.Platform.Droid.Controls.ImageTextRenderer))]

namespace XFDemoApp.Platform.Droid.Controls
{
    public class ImageTextRenderer : FastRenderers.ImageRenderer
    {
        bool disposed;

        Paint textBackgroundPaint;
        Paint dropShadowPaint;

        Bitmap bitmapText;
        Bitmap bitmapSourceImage;
        Bitmap bitmapCompositeImage;

        public ImageTextRenderer(Context context) : base(context) { }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine($"\nELEMENT DISPOSE");

            if (disposing && !disposed)
            {
                disposed = true;

                textBackgroundPaint?.Dispose();
                textBackgroundPaint = null;

                dropShadowPaint?.Dispose();
                dropShadowPaint = null;

                bitmapText?.Dispose();
                bitmapText = null;

                bitmapSourceImage?.Dispose();
                bitmapSourceImage = null;

                bitmapCompositeImage?.Dispose();
                bitmapCompositeImage = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                System.Diagnostics.Debug.WriteLine($"\nELEMENT DESTROYED");
            }

            if (e.NewElement != null)
            {
                System.Diagnostics.Debug.WriteLine($"\nELEMENT CREATED");

                textBackgroundPaint = new Paint { Color = XFColor.FromHex("#FFC51C5D").ToAndroid() };
                dropShadowPaint = new Paint { Color = XFColor.Black.ToAndroid(), AntiAlias = true };
                dropShadowPaint.SetStyle(Paint.Style.Fill);
                dropShadowPaint.SetShadowLayer(10, -10, 10, XFColor.Black.MultiplyAlpha(0.7).ToAndroid());

                UpdateText();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            System.Diagnostics.Debug.WriteLine($"\nPROPERTY CHANGED: {e.PropertyName}");
            if (e.PropertyName == Image.IsLoadingProperty.PropertyName)
            {
                if (!Element.IsLoading)
                {
                    UpdateSourceImage();
                }
            }
            else if (e.PropertyName == ImageText.TextProperty.PropertyName)
            {
                UpdateText();
                UpdateCompositeImage();
            }
        }

        private void UpdateText()
        {
            System.Diagnostics.Debug.WriteLine($"\nUPDATE TEXT");

            var text = (string)Element.GetValue(ImageText.TextProperty);
            text = string.IsNullOrEmpty(text) ? "" : text;

            var padding = (int)Element.GetValue(ImageText.TextPaddingProperty);

            using (Paint textPaint = new Paint { TextSize = 70, Color = XFColor.White.ToAndroid(), AntiAlias = true })
            {
                var bounds = new Android.Graphics.Rect();
                var textToMeasure = string.IsNullOrEmpty(text) ? "A" : text;
                textPaint.GetTextBounds(textToMeasure, 0, textToMeasure.Length, bounds);

                bitmapText = Bitmap.CreateBitmap(bounds.Right + padding, bounds.Height() + padding, Bitmap.Config.Argb8888);

                using (Canvas canvas = new Canvas(bitmapText))
                {
                    canvas.DrawColor(XFColor.FromHex("#FFC51C5D").ToAndroid());
                    canvas.DrawText(text, padding / 2, -bounds.Top + (padding / 2), textPaint);
                }
            }

            Invalidate();
        }

        private async Task UpdateSourceImage()
        {
            System.Diagnostics.Debug.WriteLine($"\nUPDATE IMAGE");

            var source = Element.Source;
            IImageSourceHandler handler = null;

            if (source is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                handler = new StreamImagesourceHandler();
            }

            if (handler != null)
            {
                bitmapSourceImage = await handler.LoadImageAsync(source, base.Context);
                System.Diagnostics.Debug.WriteLine($"\nIMAGE: {bitmapSourceImage.Width}/{bitmapSourceImage.Height}");

                UpdateCompositeImage();
            }
        }

        private void UpdateCompositeImage()
        {
            if (bitmapSourceImage == null) return;
            if (bitmapText == null) return;

            bitmapCompositeImage = bitmapSourceImage.Copy(bitmapSourceImage.GetConfig(), true);

            using (Canvas canvas = new Canvas(bitmapCompositeImage))
            {
                int left = canvas.Width - bitmapText.Width;
                canvas.DrawRect(new Android.Graphics.Rect(left, 0, bitmapText.Width + left, bitmapText.Height), dropShadowPaint);
                canvas.DrawBitmap(bitmapText, canvas.Width - bitmapText.Width, 0, null);
            }

            using (var stream = new MemoryStream(bitmapCompositeImage.ByteCount))
            {
                bitmapCompositeImage.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                Element.SetValue(ImageText.CompositeImageProperty, stream.ToArray());
            }
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            System.Diagnostics.Debug.WriteLine($"\nDRAW: {canvas.Width}/{canvas.Height}, {bitmapText.Width}/{bitmapText.Height}");

            int left = canvas.Width - bitmapText.Width;
            canvas.DrawRect(new Android.Graphics.Rect(left, 0, bitmapText.Width + left, bitmapText.Height), dropShadowPaint);
            canvas.DrawBitmap(bitmapText, canvas.Width - bitmapText.Width, 0, null);
        }
    }
}