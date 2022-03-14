using AndroidX.CardView.Widget;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(XFDemoApp.Platform.Droid.Effects.DropShadowColorEffect), nameof(XFDemoApp.Platform.Droid.Effects.DropShadowColorEffect))]

namespace XFDemoApp.Platform.Droid.Effects
{
    public class DropShadowColorEffect : PlatformEffect
    {
        Android.Graphics.Color originalAmbientShadowColor;
        Android.Graphics.Color originalSpotShadowColor;

        float originalRadius = 0;
        float radius = 20f;

        Android.Graphics.Color dropShadowColor = Color.Black.ToAndroid(); 

        protected override void OnAttached()
        {
            if (Control is CardView cardView)
            {
                var background = cardView.Background;

                var effect = (XFDemoApp.Platform.Effects.DropShadowColorEffect)Element.Effects.FirstOrDefault(e => e is XFDemoApp.Platform.Effects.DropShadowColorEffect);
                if (effect != null)
                {
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
            }
        }

        private void RemoveEffect()
        {
            if (Element is Frame frame && Control is CardView cardView)
            {
                frame.HasShadow = false;
                cardView.SetElevation(originalRadius);
                cardView.SetOutlineAmbientShadowColor(originalAmbientShadowColor);
                cardView.SetOutlineSpotShadowColor(originalSpotShadowColor);
            }
        }

        protected override void OnDetached()
        {
            RemoveEffect();
        }
    }
}