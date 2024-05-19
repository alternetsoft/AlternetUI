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

            StartHoverInteraction += OnStartHoverInteraction;
            MoveHoverInteraction += OnMoveHoverInteraction;
            EndHoverInteraction += OnEndHoverInteraction;
            StartInteraction += OnStartInteraction;
            DragInteraction += OnDragInteraction;
            EndInteraction += OnEndInteraction;
            CancelInteraction += OnCancelInteraction;
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

        /// <param name="Touches"></param>
        /// <param name="IsInsideBounds">This is only used for EndInteraction.</param>
        protected virtual void OnTouchInteraction(
            TouchInteractionKind kind,
            PointF[]? touches = null,
            bool isInsideBounds = true)
        {
        }

        protected virtual void OnCancelInteraction(object? sender, EventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.Cancel);
        }

        protected virtual void OnEndInteraction(object? sender, Microsoft.Maui.Controls.TouchEventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.End, e.Touches, e.IsInsideBounds);
        }

        protected virtual void OnDragInteraction(object? sender, Microsoft.Maui.Controls.TouchEventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.Drag, e.Touches, e.IsInsideBounds);
        }

        protected virtual void OnStartInteraction(object? sender, Microsoft.Maui.Controls.TouchEventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.Start, e.Touches, e.IsInsideBounds);
        }

        protected virtual void OnEndHoverInteraction(object? sender, EventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.EndHover);
        }

        protected virtual void OnMoveHoverInteraction(object? sender, Microsoft.Maui.Controls.TouchEventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.MoveHover, e.Touches, e.IsInsideBounds);
        }

        protected virtual void OnStartHoverInteraction(object? sender, Microsoft.Maui.Controls.TouchEventArgs e)
        {
            OnTouchInteraction(TouchInteractionKind.Drag, e.Touches, e.IsInsideBounds);
        }
    }
}