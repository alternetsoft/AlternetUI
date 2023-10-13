using Alternet.UI;
using System;
using System.ComponentModel;

namespace MenuSample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}