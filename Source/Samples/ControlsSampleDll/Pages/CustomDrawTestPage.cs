﻿using System;
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
        private ScrollBar.KnownTheme currentTheme = ScrollBar.KnownTheme.WindowsDark;

        /* private static readonly WxControlPainterHandler Painter = new(); */

        private readonly PaintActionsControl customDrawControl = new()
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

        private readonly ControlTemplate controlTemplate = new()
        {
            Visible = false,
        };

        private readonly InteriorDrawable interiorDrawable;

        static CustomDrawTestPage()
        {
        }

        public CustomDrawTestPage()
        {
            controlTemplate.Parent = this;
            controlTemplate.SetSizeToContent(WindowSizeToContentMode.WidthAndHeight);
            interiorDrawable = CreateInteriorDrawable(false);

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

            AddDrawScrollBarAction(ScrollBar.KnownTheme.WindowsDark);
            AddDrawScrollBarAction(ScrollBar.KnownTheme.WindowsLight);
            AddDrawScrollBarAction(ScrollBar.KnownTheme.VisualStudioDark);
            AddDrawScrollBarAction(ScrollBar.KnownTheme.VisualStudioLight);

            void AddDrawScrollBarAction(ScrollBar.KnownTheme theme)
            {
                panel.AddAction($"Draw ScrollBar ({theme})",
                    () => {
                        DrawScrollBar(theme);
                    });
            }

            panel.AddAction("Draw Control Template", DrawControlTemplate);

            horzScrollBar.ValueChanged += HorzScrollBar_ValueChanged;
        }

        public static InteriorDrawable CreateInteriorDrawable(bool isDarkBackground)
        {
            InteriorDrawable result = new();

            var metrics = ScrollBar.DefaultMetrics;
            result.Metrics = metrics;

            result.Border = new();
            result.Border.Border = new();
            result.Border.Border.Color = ColorUtils.GetDefaultBorderColor(isDarkBackground);

            return result;
        }

        public static void PaintInteriorDrawable(
            InteriorDrawable drawable,
            ScrollBar.KnownTheme theme,
            Control control,
            Graphics canvas,
            RectD rect,
            ScrollBar.AltPositionInfo position)
        {
            rect.Inflate(-20);

            drawable.SetThemeMetrics(theme);
            drawable.Background ??= new();
            drawable.Background.Brush = Color.GreenYellow.AsBrush;

            drawable.Bounds = rect;
            drawable.VertPosition = position.AsPositionInfo();
            drawable.HorzPosition = position.AsPositionInfo();
            drawable.Draw(control, canvas);
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

        public void DrawScrollBar(ScrollBar.KnownTheme theme)
        {
            customDrawControl.SetPaintAction((control, canvas, rect) =>
            {
                currentTheme = theme;

                PaintInteriorDrawable(
                            interiorDrawable,
                            theme,
                            control,
                            canvas,
                            rect,
                            horzScrollBar.AltPosInfo);
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

        public void DrawControlTemplate()
        {
            customDrawControl.SetPaintAction(DrawTemplate);

            void DrawTemplate(Control container, Graphics canvas, RectD rect)
            {
                RectD clipRect = (0, 0, controlTemplate.Width, controlTemplate.Height);

                PaintEventArgs e = new(canvas, clipRect);

                TransformMatrix transform = new();
                transform.Translate(100, 50);
                e.Graphics.PushTransform(transform);
                controlTemplate.RaisePaintRecursive(e);
                e.Graphics.Pop();
            }
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
            DrawScrollBar(currentTheme);
        }

        internal class ControlTemplate : Control
        {
            public ControlTemplate()
            {
                Layout = LayoutStyle.Horizontal;
                GenericLabel label1 = new("This text has ");
                GenericLabel label2 = new("bold");
                GenericLabel label3 = new(" fragment");
                label2.IsBold = true;
                DoInsideLayout(() =>
                {
                    label1.Parent = this;
                    label2.Parent = this;
                    label3.Parent = this;
                });
            }
        }
    }
}