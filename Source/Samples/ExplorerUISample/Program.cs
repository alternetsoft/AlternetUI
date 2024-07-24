﻿using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace ExplorerUISample
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var window = new ExplorerMainWindow();

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
    }
}