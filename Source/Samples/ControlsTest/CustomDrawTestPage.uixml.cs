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
        private readonly CustomDrawControl customDrawControl;
        private ITestPageSite? site;

        static CustomDrawTestPage()
        {
        }

        public CustomDrawTestPage()
        {
            InitializeComponent();

            customDrawControl = new ()
            {
                SuggestedWidth = 500,
                SuggestedHeight = 400,
            };

            mainPanel.Children.Add(customDrawControl);
        }

        public ITestPageSite? Site
        {
            get => site;

            set
            {
                site = value;
                customDrawControl.Site = value;
                customDrawControl!.Background = new SolidBrush(Color.Yellow);
            }
        }
    }
}