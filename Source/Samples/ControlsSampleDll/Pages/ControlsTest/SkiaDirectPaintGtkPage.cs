using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;
using Alternet.Drawing;
using SkiaSharp;
using Cairo;

namespace ControlsSample
{
    internal class SkiaDirectPaintGtkPage : Panel
    {
        public SkiaDirectPaintGtkPage()
        {
            if (!App.IsLinuxOS)
            {
                var label = new Label();
                label.Text = "This page is available only on Linux.";
                label.Parent = this;
                label.Dock = DockStyle.Fill;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                var skiaControl = new SkiaDirectPaintLinux();
                skiaControl.Parent = this;
                skiaControl.Dock = DockStyle.Fill;
            }
        }

        private class SkiaDirectPaintLinux : UserControl        
        {
            public override void DefaultPaint(PaintEventArgs e)
            {
                DrawingUtils.DrawOnLinuxCairoSurface(this, (canvas) =>
                {
                    canvas.Clear(SKColors.DarkOliveGreen);
                    SkiaUtils.DrawHelloText(canvas);
                });
            }
        }
    }
}