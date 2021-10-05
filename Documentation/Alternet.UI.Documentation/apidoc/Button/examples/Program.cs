using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var application = new Application();
            var window = new ExampleWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}
