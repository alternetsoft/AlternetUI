using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Alternet.UI
{
    public class MauiContainer : GraphicsView, IDrawable
    {
        private MauiGraphics? graphics;
        private Alternet.UI.Control? control;

        public MauiContainer()
        {
            Drawable = this;
        }

        public Alternet.UI.Control? Control
        {
            get => control;

            set
            {
                if (control == value)
                    return;
                control = value;
            }
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (graphics is null)
                graphics = new(this, canvas, dirtyRect);
            else
            {
                graphics.Canvas = canvas;
                graphics.DirtyRect = dirtyRect;
            }

            control?.RaisePaint(new PaintEventArgs(graphics, dirtyRect.ToRectD()));
        }
    }
}