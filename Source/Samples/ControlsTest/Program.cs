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
            var window = new MainTestWindow();

            WebBrowserTestPage.HookExceptionEvents(application);
            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}