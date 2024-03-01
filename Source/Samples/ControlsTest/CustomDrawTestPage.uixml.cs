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
            VerticalAlignment = VerticalAlignment.Fill,
            Background = Brushes.White,
        };

        private readonly SplittedControlsPanel panel = new()
        {
            LeftVisible = false,
            TopVisible = false,
            BottomVisible = false,
        };

        static CustomDrawTestPage()
        {
        }

        public CustomDrawTestPage()
        {
            InitializeComponent();

            panel.ActionsControl.Required();
            panel.VerticalAlignment = VerticalAlignment.Fill;
            panel.Parent = mainPanel;

            customDrawControl.Parent = panel.FillPanel;

            panel.AddAction("Draw native ComboBox", () =>
            {
                customDrawControl.SetPaintAction((control, canvas, rect) =>
                {
                    NativeControlPainter.Default.DrawComboBox(
                        control,
                        canvas,
                        (50, 50, 150, 100),
                        NativeControlPainter.DrawFlags.None);
                });
            });

            panel.AddAction("Draw bad Image", () =>
            {
                customDrawControl.SetPaintAction((control, canvas, rect) =>
                {
                    var image = new Bitmap();
                    canvas.DrawImage(image, PointD.Empty);
                });
            });
        }
    }
}