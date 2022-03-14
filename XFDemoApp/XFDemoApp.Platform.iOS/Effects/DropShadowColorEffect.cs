using CoreGraphics;
using System;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(XFDemoApp.Platform.iOS.Effects.DropShadowColorEffect), nameof(XFDemoApp.Platform.iOS.Effects.DropShadowColorEffect))]

namespace XFDemoApp.Platform.iOS.Effects
{
    public class DropShadowColorEffect : PlatformEffect
    {
        CGColor originalShadowColor;
        CGColor dropShadowColor = UIColor.Black.CGColor;

        UIView View => base.Control ?? base.Container;

        protected override void OnAttached()
        {
            if (View is UIView cardView)
            {
                var effect = (XFDemoApp.Platform.Effects.DropShadowColorEffect)Element.Effects.FirstOrDefault(e => e is XFDemoApp.Platform.Effects.DropShadowColorEffect);
                if (effect != null)
                {
                    dropShadowColor = effect.Color.ToCGColor();
                }

                originalShadowColor = cardView.Layer.ShadowColor;
            }

            UpdateEffect();
        }

        private void UpdateEffect()
        {
            if (Element is Frame frame && View is UIView cardView)
            {
                frame.HasShadow = true;
                cardView.Layer.ShadowColor = dropShadowColor;
            }
        }

        private void RemoveEffect()
        {
            if (Element is Frame frame && View is UIView cardView)
            {
                frame.HasShadow = false;
                cardView.Layer.ShadowColor = originalShadowColor;
            }
        }

        protected override void OnDetached()
        {
            RemoveEffect();
        }
    }
}