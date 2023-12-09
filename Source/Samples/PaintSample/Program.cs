using Alternet.UI;
using System;
using System.ComponentModel;

namespace PaintSample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.CreateAndRun(() => new MainWindow());
        }
    }
}