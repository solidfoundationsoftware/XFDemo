using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFDemoApp.Platform.Droid.Effects
{
    internal class RoundedCornerOutlineProvider: ViewOutlineProvider
    {
        public float CornerRadius { get; }

        public ViewOutlineProvider Inner { get; }

        public RoundedCornerOutlineProvider(float cornerRadius, ViewOutlineProvider inner)
        {
            CornerRadius = cornerRadius;
            Inner = inner;
        }

        public override void GetOutline(View view, Outline outline)
        {
            Inner?.GetOutline(view, outline);
            int radius = (int)(CornerRadius * view.Resources.DisplayMetrics.Density);
            outline.SetRoundRect(new Android.Graphics.Rect(0, 0, view.Width, view.Height), radius);
        }
    }
}