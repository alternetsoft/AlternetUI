using System;
using System.ComponentModel;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
           /*
            var minCheckBoxMargin = new Thickness(3);

            DefaultPropsPlatforms.PlatformAny.Controls.RadioButton.
                MinMargin = minCheckBoxMargin;
            DefaultPropsPlatforms.PlatformAny.Controls.CheckBox.
                MinMargin = minCheckBoxMargin;
            */
            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}