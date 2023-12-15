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

            var type = typeof(Program);
            var resources = type.Module.Assembly.GetManifestResourceNames();
            LogUtils.Log(resources);

            var stream = type.Module.Assembly.GetManifestResourceStream(
                "CustomCursor.CUR.HideWhiteSpace.cur");
            if (stream is null)
            {
                Application.Log("Error loading sample cursor");
                return;
            }

            var form = Application.FirstWindow<WindowLogListBox>();
            if (form is null)
                return;
            form.ListBox.Cursor = new(stream);
        }
    }
}
