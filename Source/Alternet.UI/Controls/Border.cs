using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Draws a border, background, or both around another control.
    /// </summary>
    public class Border : UserPaintControl
    {
        private static Pen? defaultBorderPen = null;
        private static double defaultBorderWidth = 1;
        private static Color defaultBorderColor = Color.Gray;

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            Padding = new Thickness(defaultBorderWidth);
        }

        public static Pen? DefaultBorderPen
        {
            get => defaultBorderPen;
            set => defaultBorderPen = value;
        }

        public static double DefaultBorderWidth
        {
            get => defaultBorderWidth;
            set
            {
                defaultBorderWidth = value;
                if (defaultBorderPen != null)
                    defaultBorderPen.Width = value;
            }
        }

        public static Color DefaultBorderColor
        {
            get => defaultBorderColor;
            set
            {
                defaultBorderColor = value;
                if (defaultBorderPen != null)
                    defaultBorderPen.Color = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (defaultBorderPen == null)
                defaultBorderPen = new Pen(defaultBorderColor, defaultBorderWidth);

            base.OnPaint(e);
            var dc = e.DrawingContext;

            dc.DrawRectangle(defaultBorderPen, DrawClientRectangle);
        }
    }
}