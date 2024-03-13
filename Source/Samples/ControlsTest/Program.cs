using System;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            LogUtils.ShowDebugWelcomeMessage = true;

            var application = new Application();

            var window = new MainTestWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}