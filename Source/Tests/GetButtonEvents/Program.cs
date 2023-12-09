using System;
using Alternet.UI;

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

            var events = AssemblyUtils.EnumEvents(typeof(Button), true);

            foreach (var item in events)
                Application.Log(item.Name);
        }
    }
}
