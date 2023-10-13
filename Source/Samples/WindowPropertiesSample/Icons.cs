using Alternet.UI;
using System;
using System.IO;

namespace WindowPropertiesSample
{
    internal static class Icons
    {
        public static ImageSet Icon1 = new(LoadImage("TestIcon1.ico"));
        public static ImageSet Icon2 = new(LoadImage("TestIcon2.ico"));

        private static Stream LoadImage(string name)
        {
            return typeof(Icons).Assembly.GetManifestResourceStream("WindowPropertiesSample.Resources." + name) ?? throw new Exception();
        }
    }
}