using System;
using Alternet.UI;

namespace test_netcoreapp6_0
{
    class Program
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
