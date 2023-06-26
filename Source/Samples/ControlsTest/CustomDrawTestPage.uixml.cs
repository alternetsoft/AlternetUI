using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    internal partial class CustomDrawTestPage : Control
    {
        private ITestPageSite? site;

        static CustomDrawTestPage()
        {
        }

        public CustomDrawTestPage()
        {
            InitializeComponent();
        }

        public ITestPageSite? Site
        {
            get => site;

            set
            {
                site = value;
                CustomDrawControl.Background = new SolidBrush(Color.Yellow);
            }
        }
    }
}