using System;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal class Program
    {
        [STAThread]
#pragma warning disable
        public static void Main(string[] args)
#pragma warning restore
        {
            var application = new Application();

            // Makes SVG images a little bit bigger on High dpi displays.
            // This is an example, this call is not needed as SVG images are scaled automatically.
            if (Display.Primary.DPI.Width > 96)
                Toolbar.DefaultImageSize96dpi = 24;

            var window = new MainTestWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}