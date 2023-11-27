using Alternet.UI;
using System;
using System.IO;
using Alternet.Drawing;

namespace WindowPropertiesSample
{
    internal static class Icons
    {
        public static IconSet Icon1 = new(LoadImage("TestIcon1.ico"));
        public static IconSet Icon2 = new(LoadImage("TestIcon2.ico"));

        private static Stream LoadImage(string name)
        {
            return typeof(Icons).Assembly.GetManifestResourceStream("WindowPropertiesSample.Resources." + name) ?? throw new Exception();
        }
    }
}