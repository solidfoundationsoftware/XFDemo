using SkiaSharp;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using XFDemoApp.ViewModels;

namespace XFDemoApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }

        private void buttonSaveImages_Clicked(object sender, System.EventArgs e)
        {
            if (imagePromoText.CompositeImage == null || imagePromoText.CompositeImage.Length == 0) return;
          
            using (SKImage image = SKImage.FromBitmap(SKBitmap.Decode(imagePromoText.CompositeImage)))
            {
                SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
                string filename = "photo.jpg";

                var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var path = System.IO.Path.Combine(documentsFolder, filename);
                Debug.WriteLine($"\n{path}");

                System.IO.File.WriteAllBytes(path, data.ToArray());
            }
        }
    }
}