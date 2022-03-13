using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFDemoApp.Platform.Effects
{
    public class DropShadowColorEffect : RoutingEffect
    {
        public DropShadowColorEffect() : base($"{nameof(XFDemoApp)}.{nameof(DropShadowColorEffect)}") { }

        public float Radius { get; set; } = 20f;

        public Color Color { get; set; } = Color.Gray;
    }
}
