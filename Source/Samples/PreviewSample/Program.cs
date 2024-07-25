using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace PreviewSample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var window = new PreviewSampleWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}