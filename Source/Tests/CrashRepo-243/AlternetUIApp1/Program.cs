using System;
using Alternet.UI;

namespace AlternetUIApp1
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
