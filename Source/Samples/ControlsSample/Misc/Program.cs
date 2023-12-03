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

            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}