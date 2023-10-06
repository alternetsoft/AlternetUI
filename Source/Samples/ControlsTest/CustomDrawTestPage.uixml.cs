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
        private readonly CustomDrawControl customDrawControl = new()
        {
            SuggestedWidth = 500,
            SuggestedHeight = 400,
            Background = Brushes.White,
        };

        static CustomDrawTestPage()
        {
        }

        public CustomDrawTestPage()
        {
            InitializeComponent();

            customDrawControl.Parent = mainPanel;
        }
    }
}