using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFDemoApp.Platform.Effects
{
    public class ApplyAppThemeEffect : RoutingEffect
    {
        public ApplyAppThemeEffect() : base($"{nameof(XFDemoApp)}.{nameof(ApplyAppThemeEffect)}") { }

        
    }
}
