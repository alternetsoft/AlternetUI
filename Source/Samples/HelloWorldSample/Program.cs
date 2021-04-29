using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

namespace HelloWorldSample
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}