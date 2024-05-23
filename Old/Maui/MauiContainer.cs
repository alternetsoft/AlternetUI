using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;

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

            /*
            canvas.FontColor = Colors.White; //App.SecondaryColor
            canvas.FontSize = 18;

            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.StrokeColor = Colors.LawnGreen;
            canvas.FillColor = Colors.LawnGreen;
            canvas.DrawRectangle(20, 20, 380, 100);
            var size = canvas.GetStringSize(
                "Text is left aligned.",
                Microsoft.Maui.Graphics.Font.Default,
                18);
            canvas.FillRectangle(20, 20, size.Width * 1.2f, size.Height * 1.5f);
            canvas.DrawString("Text is left aligned.", 20, 20, 380, 100, Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top);

            canvas.DrawRectangle(20, 60, 380, 100);
            canvas.DrawString("Text is centered.", 20, 60, 380, 100, Microsoft.Maui.Graphics.HorizontalAlignment.Center, Microsoft.Maui.Graphics.VerticalAlignment.Top);

            canvas.DrawRectangle(20, 100, 380, 100);
            canvas.DrawString("Text is right aligned.", 20, 100, 380, 100, Microsoft.Maui.Graphics.HorizontalAlignment.Right, Microsoft.Maui.Graphics.VerticalAlignment.Top);

            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
            canvas.DrawRectangle(20, 140, 350, 100);
            canvas.DrawString("This text is displayed using the bold system font.", 20, 140, 350, 100, Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top);

            canvas.Font = new Microsoft.Maui.Graphics.Font("Arial");
            canvas.FontColor = Colors.Black;
            canvas.SetShadow(new SizeF(6, 6), 4, Colors.Gray);
            canvas.DrawString("This text has a shadow.", 20, 200, 300, 100, Microsoft.Maui.Graphics.HorizontalAlignment.Left, Microsoft.Maui.Graphics.VerticalAlignment.Top);
            */
            /*
            canvas.FontColor = Colors.Red;
            canvas.StrokeSize = 1;
            canvas.StrokeColor = Colors.Blue;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;
            canvas.FontSize = 12f;

            string markdownText =
                @"This is *italic text*, **bold text**, __underline text__, and ***bold italic text***.";
            IAttributedText attributedText = MarkdownAttributedTextReader.Read(markdownText);
            canvas.DrawText(attributedText, 10, 10, 400, 400);
            */
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