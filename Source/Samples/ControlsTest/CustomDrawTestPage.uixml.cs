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

            panel.AddAction("Bad Image (test assert)", () =>
            {
                customDrawControl.SetPaintAction((control, canvas, rect) =>
                {
                    var image = new Bitmap();
                    canvas.DrawImage(image, PointD.Empty);
                });
            });

            panel.AddAction("Draw native checkbox", () =>
            {
                customDrawControl.SetPaintAction((control, canvas, rect) =>
                {
                    var painter = NativeControlPainter.Default;

                    Fn((50, 50), CheckState.Unchecked, GenericControlState.Normal, "unchecked");

                    Fn((150, 50), CheckState.Checked, GenericControlState.Normal, "checked");

                    Fn(
                        (250, 50),
                        CheckState.Checked,
                        GenericControlState.Hovered,
                        "checked, current");

                    Fn(
                        (50, 100),
                        CheckState.Checked,
                        GenericControlState.Disabled,
                        "disabled");

                    Fn(
                        (150, 100),
                        CheckState.Indeterminate,
                        GenericControlState.Normal,
                        "undetermined");

                    void Fn(
                        PointD location,
                        CheckState checkState,
                        GenericControlState controlState,
                        string title)
                    {
                        var size = DrawingUtils.GetCheckBoxSize(control, checkState, controlState);
                        var rect = (location, size);
                        canvas.DrawCheckBox(
                            control,
                            rect,
                            checkState,
                            controlState);
                        location.Y += size.Height;
                        canvas.DrawText(title, location);
                    }
                });
            });
        }
    }
}