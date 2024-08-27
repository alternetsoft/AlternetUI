using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using Alternet.Drawing;
using Alternet.UI;

namespace ControlsSample
{
    public partial class CustomDrawTestPage : Window
    {
        /* private static readonly WxControlPainterHandler Painter = new(); */

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

        private readonly VerticalStackPanel mainPanel = new()
        {
            Margin = (5, 5, 5, 5),
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        private readonly Label headerLabel = new()
        {
            Margin = (5, 5, 5, 5),
            Text = "Custom Draw Test",
        };

        private readonly ScrollBar horzScrollBar = new()
        {
        };

        private readonly ScrollBarsDrawable scrollBarsDrawable = new()
        {
        };

        static CustomDrawTestPage()
        {
        }

        public CustomDrawTestPage()
        {
            Size = (900, 700);
            State = WindowState.Maximized;

            mainPanel.Parent = this;
            headerLabel.Parent = mainPanel;
            horzScrollBar.Parent = mainPanel;

            panel.ActionsControl.Required();
            panel.VerticalAlignment = VerticalAlignment.Fill;
            panel.Parent = mainPanel;

            customDrawControl.Parent = panel.FillPanel;

            /* panel.AddAction("Draw Native ComboBox", DrawNativeComboBox); */
            panel.AddAction("Draw Native Checkbox", DrawNativeCheckbox);
            panel.AddAction("Test Bad Image Assert", TestBadImageAssert);
            panel.AddAction("Draw ScrollBar", DrawScrollBar);

            horzScrollBar.ValueChanged += HorzScrollBar_ValueChanged;
        }

        public void DrawNativeComboBox()
        {
            customDrawControl.SetPaintAction((control, canvas, rect) =>
            {
                /*
                Painter.DrawComboBox(
                    control,
                    canvas,
                    (50, 50, 150, 100),
                    WxControlPainterHandler.DrawFlags.None);
                */
            });
        }

        public void DrawScrollBar()
        {
            customDrawControl.SetPaintAction((control, canvas, rect) =>
            {
                var metrics = ScrollBar.DefaultMetrics;

                scrollBarsDrawable.Bounds = rect;
                scrollBarsDrawable.Metrics = metrics;
                scrollBarsDrawable.VertScrollBar?.SetAltPosInfo(horzScrollBar.AltPosInfo);
                scrollBarsDrawable.HorzScrollBar?.SetAltPosInfo(horzScrollBar.AltPosInfo);
                scrollBarsDrawable.Draw(control, canvas);

                var downSvgImage = KnownSvgImages.GetImgTriangleArrow(ArrowDirection.Down);
                var upSvgImage = KnownSvgImages.GetImgTriangleArrow(ArrowDirection.Up);
                var leftSvgImage = KnownSvgImages.GetImgTriangleArrow(ArrowDirection.Left);
                var rightSvgImage = KnownSvgImages.GetImgTriangleArrow(ArrowDirection.Right);

                DrawImage((50, 50), upSvgImage);
                DrawImage((50, 90), downSvgImage);
                DrawImage((20, 70), leftSvgImage);
                DrawImage((90, 70), rightSvgImage);

                void DrawImage(PointD coord, SvgImage svg)
                {
                    var img = svg.AsImage(32);
                    if (img is null)
                        return;
                    canvas.DrawImage(img, coord);
                }
            });
        }

        public void TestBadImageAssert()
        {
            customDrawControl.SetPaintAction((control, canvas, rect) =>
            {
                var image = new Bitmap();
                canvas.DrawImage(image, PointD.Empty);
            });
        }

        public void DrawNativeCheckbox()
        {
            customDrawControl.SetPaintAction((control, canvas, rect) =>
            {
                Fn((50, 50), CheckState.Unchecked, VisualControlState.Normal, "unchecked");

                Fn((150, 50), CheckState.Checked, VisualControlState.Normal, "checked");

                Fn(
                    (250, 50),
                    CheckState.Checked,
                    VisualControlState.Hovered,
                    "checked, current");

                Fn(
                    (50, 100),
                    CheckState.Checked,
                    VisualControlState.Disabled,
                    "disabled");

                Fn(
                    (150, 100),
                    CheckState.Indeterminate,
                    VisualControlState.Normal,
                    "undetermined");

                void Fn(
                    PointD location,
                    CheckState checkState,
                    VisualControlState controlState,
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
        }

        private void HorzScrollBar_ValueChanged(object? sender, EventArgs e)
        {
            DrawScrollBar();
        }
    }
}