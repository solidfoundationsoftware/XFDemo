using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFDemoApp.Platform
{
    public class DropShadowColorEffect : RoutingEffect
    {
        public DropShadowColorEffect() : base($"{nameof(XFDemoApp)}.{nameof(DropShadowColorEffect)}") { }

        public float Radius { get; set; } = 20f;

        public Color Color { get; set; } = Color.Gray;

        //public static readonly BindableProperty DropShadowRadiusProperty = BindableProperty.Create(nameof(DropShadowRadius), typeof(float), typeof(DropShadowColorEffect), 10);

        //public float DropShadowRadius
        //{
        //    get { return (float)GetValue(DropShadowRadiusProperty); }
        //    set { SetValue(DropShadowRadiusProperty, value); }
        //}
    }
}
