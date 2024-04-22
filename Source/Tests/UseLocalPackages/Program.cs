using System;
using Alternet.UI;
using Alternet.Drawing;

namespace MinMaster
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.CreateAndRun(() => new WindowLogListBox(), Init);
        }

        private static void Init()
        {
            Application.Log($"The application started at {DateTime.Now:HH:mm:ss.fff}");
            var form = Application.FirstWindow<WindowLogListBox>();
            if (form is null)
                return;
        }
    }
}
