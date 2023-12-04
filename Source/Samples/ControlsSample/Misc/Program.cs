using System;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var testBadFont = false;

            var application = new Application();

            if (testBadFont)
                Control.DefaultFont = new Font("abrakadabra", 12);

            // Makes SVG images a little bit bigger on High dpi displays.
            // This is an example, this call is not needed as SVG images are scaled automatically.
            if(Display.Primary.DPI.Width > 96)
                Toolbar.DefaultImageSize96dpi = 24;

            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}