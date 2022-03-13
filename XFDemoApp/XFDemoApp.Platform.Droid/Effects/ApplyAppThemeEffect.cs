using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(XFDemoApp.Platform.Droid.Effects.ApplyAppThemeEffect), nameof(XFDemoApp.Platform.Droid.Effects.ApplyAppThemeEffect))]

namespace XFDemoApp.Platform.Droid.Effects
{
    public class ApplyAppThemeEffect : PlatformEffect
    {
        //TODO define the value as a property so it can be passed in.
        const float CORNER_RADIUS = 4f;

        protected override void OnAttached()
        {
            if (Container != null)
            {
                Container.OutlineProvider = new RoundedCornerOutlineProvider(CORNER_RADIUS, base.Container.OutlineProvider);
                Container.ClipToOutline = true;
            }
            if (Control != null)
            {
                ApplyStyles();
            }
        }

        private void ApplyStyles()
        {
            if (Control is SearchView searchView)
            {
                int identifier = searchView.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
                if (identifier != 0)
                {
                    Android.Views.View view = searchView.FindViewById(identifier);
                    if (view != null && view.Background != null)
                    {
                        view.Background.SetTint(Color.Transparent.ToAndroid());
                    }
                }
            }
            else if (Control is EditText editText)
            {
                editText.SetBackgroundColor(Color.Transparent.ToAndroid());
            }
        }

        protected override void OnDetached()
        {
            
        }
    }
}