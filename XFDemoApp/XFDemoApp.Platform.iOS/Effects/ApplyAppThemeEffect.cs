using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(XFDemoApp.Platform.iOS.Effects.ApplyAppThemeEffect), nameof(XFDemoApp.Platform.iOS.Effects.ApplyAppThemeEffect))]

namespace XFDemoApp.Platform.iOS.Effects
{
    public class ApplyAppThemeEffect : PlatformEffect
    {
        const float CORNER_RADIUS = 4f;

        protected override void OnAttached()
        {
            if (Control != null)
            {
                ApplyStyles();
            }
        }

        private void ApplyStyles()
        {
            if (Element is SearchBar)
            {
                var textField = GetSearchBarTextField();

                if (textField != null)
                {
                    textField.BackgroundColor = Color.Transparent.ToUIColor();
                }
            }

            if (Container != null)
            {
                Container.Layer.CornerRadius = CORNER_RADIUS;

                if (base.Control != null)
                {
                    base.Control.ClipsToBounds = true;
                    Control.Layer.CornerRadius = CORNER_RADIUS;
                }
                else
                {
                    base.Container.ClipsToBounds = true;
                }
            }
        }

        private UITextField GetSearchBarTextField()
        {
            UIView[] subviews = Control.Subviews;
            for (int i = 0; i < subviews.Length; i++)
            {
                UIView[] subviews2 = subviews[i].Subviews;
                foreach (UIView uIView in subviews2)
                {
                    if (uIView is UITextField uITextField)
                    {
                        return uITextField;
                    }
                    UIView[] subviews3 = uIView.Subviews;
                    for (int j = 0; j < subviews3.Length; j++)
                    {
                        if (subviews3[j] is UITextField uITextField2)
                        {
                            return uITextField2;
                        }
                    }
                }
            }

            return null;
        }

        protected override void OnDetached()
        {

        }
    }
}