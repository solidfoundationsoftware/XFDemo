using Android.Content;
using Android.Graphics;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFDemoApp.Platform;
using FastRenderers = Xamarin.Forms.Platform.Android.FastRenderers;
using XFColor = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(ImageText), typeof(XFDemoApp.Platform.Droid.ImageTextRenderer))]

namespace XFDemoApp.Platform.Droid
{
    public class ImageTextRenderer : FastRenderers.ImageRenderer
    {
        ImageText visualElement;

        Paint paint = new Paint
        {
            Color = XFColor.FromHex("#FFC51C5D").ToAndroid()
        };

        Paint dropShadowPaint = new Paint { Color = XFColor.Black.ToAndroid(), AntiAlias = true };

        Bitmap textBitmap;

        public ImageTextRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            visualElement = Element as ImageText;

            if (textBitmap == null)
            {
                UpdateText();                

                dropShadowPaint.SetStyle(Paint.Style.Fill);
                dropShadowPaint.SetShadowLayer(10, -10, 10, XFColor.Black.MultiplyAlpha(0.7).ToAndroid());
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            System.Diagnostics.Debug.WriteLine($"\nPROPERTY CHANGED: {e.PropertyName}");
            if (e.PropertyName == Image.IsLoadingProperty.PropertyName)
            {
                if (!Element.IsLoading) SaveImage();
            }
            else if (e.PropertyName == ImageText.TextProperty.PropertyName)
            {
                UpdateText();
            }
        }

        private void UpdateText()
        {
            System.Diagnostics.Debug.WriteLine($"\nUPDATE TEXT");

            var text = visualElement.Text;
            var padding = visualElement.TextPadding;

            using (Paint textPaint = new Paint { TextSize = 70, Color = XFColor.White.ToAndroid(), AntiAlias = true })
            {
                var bounds = new Android.Graphics.Rect();
                var textToMeasure = string.IsNullOrEmpty(text) ? "A" : text;
                textPaint.GetTextBounds(textToMeasure, 0, textToMeasure.Length, bounds);

                textBitmap = Bitmap.CreateBitmap(bounds.Right + padding, bounds.Height() + padding, Bitmap.Config.Argb8888);

                using (Canvas bitmapCanvas = new Canvas(textBitmap))
                {
                    bitmapCanvas.DrawColor(XFColor.FromHex("#FFC51C5D").ToAndroid());
                    bitmapCanvas.DrawText(text, padding / 2, -bounds.Top + (padding / 2), textPaint);
                }
            }

            Invalidate();
        }

        private async Task SaveImage()
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
                var image = await handler.LoadImageAsync(source, base.Context);
                System.Diagnostics.Debug.WriteLine($"\nIMAGE: {image.Width}/{image.Height}");
            }
        }


        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            System.Diagnostics.Debug.WriteLine($"\nDRAW: {canvas.Width}/{canvas.Height}, {textBitmap.Width}/{textBitmap.Height}");

            int left = canvas.Width - textBitmap.Width;
            canvas.DrawRect(new Android.Graphics.Rect(left, 0, textBitmap.Width + left, textBitmap.Height), dropShadowPaint);
            canvas.DrawBitmap(textBitmap, canvas.Width - textBitmap.Width, 0, null);
        }
    }
}