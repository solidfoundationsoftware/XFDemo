using Android.Graphics.Drawables;
using AndroidX.CardView.Widget;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(XFDemoApp.Platform.Droid.DropShadowColorEffect), nameof(XFDemoApp.Platform.DropShadowColorEffect))]

namespace XFDemoApp.Platform.Droid
{
    public class DropShadowColorEffect : PlatformEffect
    {
        Android.Graphics.Color originalAmbientShadowColor;
        Android.Graphics.Color originalSpotShadowColor;

        float originalRadius = 0;
        float radius = 0;

        Android.Graphics.Color dropShadowColor = Color.Black.ToAndroid(); //Color.FromHex("#FF659AEA").ToAndroid();

        protected override void OnAttached()
        {
            if (Control is CardView cardView)
            {
                var background = cardView.Background;

                System.Diagnostics.Debug.WriteLine(background);
                System.Diagnostics.Debug.WriteLine(Resource.Color.cardview_shadow_start_color);
                System.Diagnostics.Debug.WriteLine(Resource.Attribute.cardElevation);

                var effect = (XFDemoApp.Platform.DropShadowColorEffect)Element.Effects.FirstOrDefault(e => e is XFDemoApp.Platform.DropShadowColorEffect);
                if (effect != null)
                {
                    radius = effect.Radius;
                    dropShadowColor = effect.Color.ToAndroid();                    
                }

                originalRadius = cardView.Elevation;
                originalAmbientShadowColor = new Android.Graphics.Color(cardView.OutlineAmbientShadowColor);
                originalSpotShadowColor = new Android.Graphics.Color(cardView.OutlineSpotShadowColor);
            }

            UpdateEffect();
        }

        private void UpdateEffect()
        {
            if (Element is Frame frame && Control is CardView cardView)
            {
                frame.HasShadow = true;
                cardView.SetElevation(radius);
                cardView.SetOutlineAmbientShadowColor(dropShadowColor);
                cardView.SetOutlineSpotShadowColor(dropShadowColor);

                //if (frame.HasShadow)
                //{
                //    cardView.SetElevation(radius);
                //    cardView.SetOutlineAmbientShadowColor(dropShadowColor);
                //    cardView.SetOutlineSpotShadowColor(dropShadowColor);
                //}
                //else
                //{
                //    RemoveEffect();
                //}
            }
        }

        private void RemoveEffect()
        {
            if (Control is CardView cardView)
            {
                cardView.SetElevation(originalRadius);
                cardView.SetOutlineAmbientShadowColor(originalAmbientShadowColor);
                cardView.SetOutlineSpotShadowColor(originalSpotShadowColor);
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            //if (args.PropertyName == Frame.HasShadowProperty.PropertyName)
            //{
            //    UpdateEffect();
            //}
        }

        protected override void OnDetached()
        {
            RemoveEffect();
        }
    }
}