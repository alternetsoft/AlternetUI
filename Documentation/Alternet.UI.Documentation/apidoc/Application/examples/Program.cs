using System;
using Alternet.UI;

namespace Alternet.UI.Documentation.Examples.Application
{
    #region class
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Alternet.UI.Application();
            var window = new MainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
    #endregion
}
