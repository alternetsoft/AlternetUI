using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var control = new MainControl();

            var window = new Window
            {
                Title = "PropertyGrid Sample",
            };

            control.Parent = window;

            window.Size = new SizeD(1024, 900);

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}