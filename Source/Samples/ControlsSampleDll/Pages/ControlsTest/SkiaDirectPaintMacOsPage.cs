using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;
using Alternet.Drawing;

namespace ControlsSampleDll
{
    internal class SkiaDirectPaintMacOsPage : Panel
    {
        public SkiaDirectPaintMacOsPage()
        {
            if (!App.IsMacOS)
            {
                var label = new Label();
                label.Text = "This page is available only on macOs.";
                label.Parent = this;
                label.Dock = DockStyle.Fill;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.VerticalAlignment = VerticalAlignment.Center;
            }
            else
            {
                var skiaControl = new SkiaDirectPaintMacOs();
                skiaControl.Parent = this;
                skiaControl.Dock = DockStyle.Fill;
            }
        }

        private class SkiaDirectPaintMacOs : UserControl        
        {
            public override void DefaultPaint(PaintEventArgs e)
            {
                if (!App.IsMacOS)
                    return;
                IntPtr nsViewPtr = Handler.GetHandle();
                if (nsViewPtr == IntPtr.Zero)
                    return;
            }
        }
    }
}