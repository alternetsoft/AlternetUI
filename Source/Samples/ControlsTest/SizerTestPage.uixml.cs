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
    internal partial class SizerTestPage : Control
    {
        private readonly LayoutPanel panel = new();
        private readonly Button button1 = new("Button 1");
        private readonly Button button2 = new("Button 2");
        private readonly Button button3 = new("Button 3");

        static SizerTestPage()
        {
        }

        public SizerTestPage()
        {
            InitializeComponent();

            Size size = new(100, 100);

            button1.Size = size;
            button2.Size = size;
            button3.Size = size;

            button2.Location = new(150, 150);
            button3.Location = new(300, 300);

            button1.Parent = panel;
            button2.Parent = panel;
            button3.Parent = panel;

            panel.Parent = mainControl;

            panel.PerformLayout();

            void ApplySizer(ISizer sizer)
            {
                panel.SetSizerAndFit(sizer, false);
            }

            MainTestWindow? mainWindow = Application.FirstWindow<MainTestWindow>();
            if (mainWindow is null)
                return;

            mainWindow.MainPanel.AddAction("WrapSizer(Vertical)", () =>
            {
                var sizer = SizerFactory.Default.CreateWrapSizer(true);
                ApplySizer(sizer);
            });

            mainWindow.MainPanel.AddAction("WrapSizer(Horizontal)", () =>
            {
                var sizer = SizerFactory.Default.CreateWrapSizer(false);
                ApplySizer(sizer);
            });

            mainWindow.MainPanel.AddAction("FlexGridSizer(2,10,10)", () =>
            {
                var sizer = SizerFactory.Default.CreateFlexGridSizer(2, 10, 10);
                ApplySizer(sizer);
            });

            mainWindow.MainPanel.AddAction("GridBagSizer(10, 10)", () =>
            {
                var sizer = SizerFactory.Default.CreateGridBagSizer(10, 10);
                ApplySizer(sizer);
            });

            mainWindow.MainPanel.AddAction("BoxSizer(Vertical)", () =>
            {
                var sizer = SizerFactory.Default.CreateBoxSizer(true);
                ApplySizer(sizer);
            });

            mainWindow.MainPanel.AddAction("BoxSizer(Horizontal)", () =>
            {
                var sizer = SizerFactory.Default.CreateBoxSizer(false);
                ApplySizer(sizer);
            });

            mainWindow.MainPanel.AddAction("GridSizer(2, 10, 10)", () =>
            {
                var sizer = SizerFactory.Default.CreateGridSizer(2, 10, 10);
                ApplySizer(sizer);
            });
        }
    }
}