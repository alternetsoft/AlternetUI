using Alternet.UI;
using Alternet.Drawing;
using System;

namespace DrawingSample
{
    internal static class Resources
    {
        const string ResourceNamePrefix = "DrawingSample.Resources.";

        public static readonly Image LogoImage = new Image(typeof(TransformsPage).Assembly.GetManifestResourceStream(
            ResourceNamePrefix + "Logo.png") ?? throw new Exception());

        public static readonly Image LeavesImage = new Image(typeof(TransformsPage).Assembly.GetManifestResourceStream(
            ResourceNamePrefix + "Leaves.jpg") ?? throw new Exception());
    }
}