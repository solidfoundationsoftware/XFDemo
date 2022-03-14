using CoreGraphics;
using CoreText;
using Foundation;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFDemoApp.Platform.Controls;
using XFColor = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(XFDemoApp.Platform.Controls.ImageText), typeof(XFDemoApp.Platform.iOS.Controls.ImageTextRenderer))]

namespace XFDemoApp.Platform.iOS.Controls
{
    public class ImageTextRenderer : Xamarin.Forms.Platform.iOS.ViewRenderer<ImageText, UIView>, IImageVisualElementRenderer
    {
        bool disposed;

        FormsUIImageView imageView;
        UIImage sourceImage;

        readonly nfloat screenDensity;

        public bool IsDisposed => disposed;

        public ImageTextRenderer() : base()
        {
            screenDensity = (nfloat)DeviceDisplay.MainDisplayInfo.Density;
            ImageElementManager.Init(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;

                ImageElementManager.Dispose(this);
                imageView?.Dispose();
                sourceImage?.Dispose();
            }

            base.Dispose(disposing);
        }

        protected async override void OnElementChanged(ElementChangedEventArgs<ImageText> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var control = CreateNativeControl();
                    control.ClipsToBounds = true;
                    SetNativeControl(control);
                }

                imageView = new FormsUIImageView();

                await UpdateImage().ConfigureAwait(false);
            }
        }

        protected override UIView CreateNativeControl()
        {
            return new UIView();
        }

        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Image.SourceProperty.PropertyName)
            {
                await ImageElementManager.SetImage(this, Element).ConfigureAwait(false);
            }
            else if (e.PropertyName == Image.IsLoadingProperty.PropertyName)
            {
                if (!Element.IsLoading)
                {
                    UpdateCompositeImage();
                }
            }
            else if (e.PropertyName == ImageText.TextProperty.PropertyName)
            {
                UpdateCompositeImage();
            }
        }

        private void UpdateCompositeImage()
        {
            if (sourceImage == null) return;

            var text = (string)Element.GetValue(ImageText.TextProperty) ?? "";

            var rect = new CGRect(0, 0, sourceImage.Size.Width, sourceImage.Size.Height);
            var size = new CGSize(rect.Width, rect.Height);

            UIGraphics.BeginImageContextWithOptions(size, true, sourceImage.CurrentScale);

            //get graphics context
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                DrawImage(g, rect, sourceImage);
                DrawText(g, text, rect.Width);

                //get a UIImage from the context
                var compositeImage = UIGraphics.GetImageFromCurrentImageContext();

                if (compositeImage != null)
                {
                    Element.SetValue(ImageText.CompositeImageProperty, compositeImage.AsJPEG(1).ToArray());
                }
            }

            UIGraphics.EndImageContext();

            SetNeedsDisplay();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var text = (string)Element.GetValue(ImageText.TextProperty) ?? "";

            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                DrawImage(g, rect, sourceImage);
                DrawText(g, text, rect.Width);
            }
        }

        void DrawImage(CGContext context, CGRect rect, UIImage image)
        {
            context.SaveState();
            context.ScaleCTM(1, -1);
            context.TranslateCTM(0, -rect.Height);

            if (image != null)
            {
                context.DrawImage(rect, image.CGImage);
            }

            context.RestoreState();
        }

        void DrawText(CGContext context, string text, nfloat containerWidth)
        {
            context.SaveState();
            context.ScaleCTM(1, -1);

            var attributedString = new NSAttributedString(text,
                new CTStringAttributes
                {
                    ForegroundColor = UIColor.White.CGColor,
                    Font = new CTFont("ArialMT", 75 / screenDensity),
                });

            using (var textLine = new CTLine(attributedString))
            {
                var padding = (nint)((int)Element.GetValue(ImageText.TextPaddingProperty) / screenDensity);
                var shadowSize = 10 / screenDensity;

                var sizeOfText = textLine.GetBounds(CTLineBoundsOptions.UseOpticalBounds);

                var background = new CGRect(0, 0, sizeOfText.Width + padding, sizeOfText.Height + padding);

                var x = containerWidth - background.Width;
                var y = background.Height;

                context.SaveState();
                context.TranslateCTM(x, -y);

                context.SetFillColor(XFColor.FromHex("#FFC51C5D").ToCGColor());
                context.SetShadow(new CGSize(-shadowSize, shadowSize), shadowSize, UIColor.Black.ColorWithAlpha((nfloat)0.7).CGColor);
                context.AddRect(background);
                context.DrawPath(CGPathDrawingMode.Fill);
                context.RestoreState();

                var textPadding = padding / 2;
                x = x + textPadding;
                y = sizeOfText.Y + sizeOfText.Height + textPadding;
                context.TranslateCTM(x, -y);
                textLine.Draw(context);
            }

            context.RestoreState();
        }

        public void SetImage(UIImage image)
        {
            sourceImage = image;
        }

        public UIImageView GetImage()
        {
            return imageView;
        }

        async Task UpdateImage()
        {
            if (Element == null) return;

            try
            {
                await ImageElementManager.SetImage(this, Element).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(nameof(ImageTextRenderer), "Error loading image: {0}", ex);
            }
        }
    }
}