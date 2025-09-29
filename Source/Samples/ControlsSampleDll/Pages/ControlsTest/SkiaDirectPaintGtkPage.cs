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
                if (!App.IsLinuxOS)
                    return;

                IntPtr gtkWidgetPtr = Handler.GetHandle();
                if (gtkWidgetPtr == IntPtr.Zero)
                    return;

                Gtk.Widget widget = new Gtk.Widget(gtkWidgetPtr);

                int width = widget.AllocatedWidth;
                int height = widget.AllocatedHeight;

                Gdk.Window gdkWindow = widget.Window;

                var cairoPtr = Handler.NativeGraphicsContext;
                using var cairoContext = new Context(cairoPtr, false);

                var info = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
                using var bitmap = new SKBitmap(info);
                using var surface = SKSurface.Create(bitmap.Info, bitmap.GetPixels(), bitmap.RowBytes);

                var canvas = surface.Canvas;
                canvas.Clear(SKColors.DarkOliveGreen);
                SkiaUtils.DrawHelloText(canvas);

                var data = bitmap.Bytes;
                using var imageSurface = new ImageSurface(data, Format.Argb32, width, height, bitmap.RowBytes);

                cairoContext.SetSourceSurface(imageSurface, 0, 0);
                cairoContext.Paint();
            }
        }
    }
}