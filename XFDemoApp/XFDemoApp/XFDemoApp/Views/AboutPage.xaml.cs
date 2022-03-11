using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFDemoApp.Platform;
using XFDemoApp.Views.Templates;

namespace XFDemoApp.Views
{
    public partial class AboutPage : ContentPage
    {
        string text = "3/$25";

        SKBitmap photoBitmap;
        SKBitmap textBitmap;
        SKBitmap finalBitmap;

        SKPaint paintBackground = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = Color.FromHex("#FFC51C5D").ToSKColor()
        };

        public AboutPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<Behavior<VisualElement>, VisualElement>(this, "OK", (e, sender) =>
            {
                Debug.WriteLine($"\nSUBSCRIBE: {e.BindingContext}/{sender}");
            });
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (photoBitmap == null)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("XFDemoApp.Resources.sample.jpg"))
                {
                    photoBitmap = SKBitmap.Decode(stream);
                    //Debug.WriteLine($"w: {photoBitmap.Width}, h: {photoBitmap.Height}");

                    using (SKPaint textPaint = new SKPaint { TextSize = 70, Color = SKColors.White, IsAntialias = true })
                    {
                        SKRect bounds = new SKRect();
                        textPaint.MeasureText(text, ref bounds);

                        textBitmap = new SKBitmap((int)bounds.Right + 20, (int)bounds.Height + 20);

                        using (SKCanvas bitmapCanvas = new SKCanvas(textBitmap))
                        {
                            bitmapCanvas.Clear();
                            bitmapCanvas.DrawText(text, 10, -bounds.Top + 10, textPaint);
                        }
                    }

                    finalBitmap = new SKBitmap(photoBitmap.Width, photoBitmap.Height);

                    using (SKCanvas canvas = new SKCanvas(finalBitmap))
                    {
                        canvas.Clear();
                        canvas.DrawBitmap(photoBitmap, 0, 0);

                        float textX = (float)(photoBitmap.Width - textBitmap.Width);
                        SKRect textRect = new SKRect(textX, 0, textBitmap.Width + textX, textBitmap.Height);

                        paintBackground.ImageFilter = SKImageFilter.CreateDropShadow(-10, 10, 10, 10, SKColors.Black);

                        canvas.DrawRect(textRect, paintBackground);
                        canvas.DrawBitmap(textBitmap, textRect);
                    }
                }
            }
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            float x = 0;
            float y = 0;
            float scale = Math.Min((float)info.Width / finalBitmap.Width, (float)info.Height / finalBitmap.Height);

            canvas.Clear();
            //canvas.DrawPaint(paintBackground);

            SKRect rect = new SKRect(x, y, finalBitmap.Width * scale, finalBitmap.Height * scale);
            canvas.DrawBitmap(finalBitmap, rect);
            //canvas.DrawBitmap(bitmap, rect);

            //canvas.DrawRect(0, 0, 80, 80, paintBackground);
            //float scaleText = Math.Min((float)info.Width / textBitmap.Width, (float)info.Height / textBitmap.Height);
            //float textX = (float)(photoBitmap.Width - textBitmap.Width);
            //SKRect textRect = new SKRect(textX, y, textBitmap.Width + textX, textBitmap.Height);
            //canvas.DrawRect(textRect, paintBackground);
            //canvas.DrawBitmap(textBitmap, textRect);

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            using (SKImage image = SKImage.FromBitmap(finalBitmap))
            {
                SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
                DateTime dt = DateTime.Now;
                string filename = String.Format("photo.jpg",
                                                dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);

                var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = System.IO.Path.Combine(documentsFolder, filename);
                Debug.WriteLine($"\n{path}");

                System.IO.File.WriteAllBytes(path, data.ToArray());
            }
        }

        private void ToggleEffect_Clicked(object sender, EventArgs e)
        {
            if (cardView.Effects.Count > 0)
            {
                cardView.Effects.Clear();
            }
            else
            {
                cardView.Effects.Add(new DropShadowColorEffect());
            }
        }

        private void ToggleShadow_Clicked(object sender, EventArgs e)
        {
            cardView.HasShadow = !cardView.HasShadow;

            
        }
    }
}