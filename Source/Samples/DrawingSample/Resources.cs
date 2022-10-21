using Alternet.UI;
using Alternet.Drawing;
using System;

namespace DrawingSample
{
    internal static class Resources
    {
        public static readonly Image LogoImage = new Image(typeof(TransformsPage).Assembly.GetManifestResourceStream(
            "DrawingSample.Resources.Logo.png") ?? throw new Exception());
    }
}