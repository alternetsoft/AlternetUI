using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace ControlsTest
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            var window = new MainTestWindow();

            WebBrowserTestPage.HookExceptionEvents(application);
            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}