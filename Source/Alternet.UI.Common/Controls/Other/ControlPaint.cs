using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods used to paint common controls and their elements.
    /// </summary>
    public static class ControlPaint
    {
        private static IControlPainterHandler? handler;

        /// <summary>
        /// Gets or sets current control painter handler.
        /// </summary>
        public static IControlPainterHandler Handler
        {
            get => handler ??= App.Handler.CreateControlPainterHandler();
            set => handler = value;
        }

        /// <summary>
        /// Draws a border with the specified style and color, on the specified canvas,
        /// and within the specified bounds.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics" /> to draw on.</param>
        /// <param name="bounds">The <see cref="RectD" /> that represents the dimensions
        /// of the border.</param>
        /// <param name="color">The <see cref="Color" /> of the border.</param>
        /// <param name="style">One of the <see cref="ButtonBorderStyle" /> values
        /// that specifies the style of the border.</param>
        public static void DrawBorder(Graphics graphics, RectD bounds, Color color, ButtonBorderStyle style)
        {
            switch (style)
            {
                case ButtonBorderStyle.Dotted:
                case ButtonBorderStyle.Dashed:
                case ButtonBorderStyle.Solid:
                    DrawBorderSimple(graphics, bounds, color, style);
                    break;
                case ButtonBorderStyle.None:
                    break;
            }
        }

        internal static void DrawBorderSimple(
            Graphics graphics,
            RectD bounds,
            Color color,
            ButtonBorderStyle style)
        {
            if (style == ButtonBorderStyle.Solid)
            {
                Draw(color.AsPen);
            }
            else
            {
                var dashStyle = BorderStyleToDashStyle(style);
                using Pen pen = new(color, 1, dashStyle);
                Draw(pen);
            }

            void Draw(Pen pen)
            {
                graphics.DrawRectangle(pen, (bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1));
            }
        }

        internal static void LogPartSize(AbstractControl control)
        {
            App.Log($"CheckMarkSize: {Handler.GetCheckMarkSize(control)}");
            App.Log($"CheckBoxSize(0): {Handler.GetCheckBoxSize(control)}");
            App.Log($"GetExpanderSize: {Handler.GetExpanderSize(control)}");
            App.Log($"GetHeaderButtonHeight: {Handler.GetHeaderButtonHeight(control)}");
            App.Log($"GetHeaderButtonMargin: {Handler.GetHeaderButtonMargin(control)}");
        }

        private static DashStyle BorderStyleToDashStyle(ButtonBorderStyle borderStyle)
        {
            return borderStyle switch
            {
                ButtonBorderStyle.Dotted => DashStyle.Dot,
                ButtonBorderStyle.Dashed => DashStyle.Dash,
                ButtonBorderStyle.Solid => DashStyle.Solid,
                _ => DashStyle.Solid,
            };
        }
    }
}
