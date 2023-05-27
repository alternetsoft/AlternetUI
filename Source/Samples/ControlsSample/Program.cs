using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace ControlsSample
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            var window = new MainWindow();

            WebBrowserPage.HookExceptionEvents(application);
            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}