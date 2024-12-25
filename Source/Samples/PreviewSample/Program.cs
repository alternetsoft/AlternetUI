using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace PreviewSample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.CreateAndRun(() => new PreviewSampleWindow());
        }
    }
}