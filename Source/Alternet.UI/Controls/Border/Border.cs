using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Draws a border, background, or both around another control.
    /// </summary>
    public class Border : UserPaintControl
    {
        private static BorderPens defaultPens = new();
        private BorderPens pens = defaultPens.Clone();

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            UpdatePadding();
        }

        public static Thickness DefaultBorderWidth
        {
            get => defaultPens.Width;
            set => defaultPens.Width = value;
        }

        public static Color DefaultBorderColor
        {
            get
            {
                return defaultPens.Color;
            }

            set
            {
                defaultPens.Color = value;
            }
        }

        public Thickness BorderWidth
        {
            get => pens.Width;
            set
            {
                if (pens.Width == value)
                    return;
                if (value == null)
                    pens.Width = defaultPens.Width;
                else
                {
                    var v = (Thickness)value;
                    if (v.Top > 1) v.Top = 1;
                    if (v.Bottom > 1) v.Bottom = 1;
                    if (v.Left > 1) v.Left = 1;
                    if (v.Right > 1) v.Right = 1;
                    pens.Width = v;
                }
                UpdatePadding();
                Refresh();
            }
        }

        public Color? BorderColor
        {
            get
            {
                return pens.Color;
            }

            set
            {
                if (value == null)
                    pens.Color = defaultPens.Color;
                else
                    pens.Color = (Color)value;
                Refresh();
            }
        }

        /// <inheritdoc/>
        public override Size GetPreferredSize(Size availableSize)
        {
            return base.GetPreferredSize(availableSize) +
                new Size(pens.Width.Horizontal, pens.Width.Vertical);
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var dc = e.DrawingContext;

            if (pens.IsUniform && pens.Width.Top > 0)
            {
                dc.DrawRectangle(pens.Top, DrawClientRectangle);
                return;
            }

            var r = DrawClientRectangle;

            if(pens.Width.Top > 0)
                dc.DrawLine(pens.Top, r.TopLeft, r.TopRight);
            if (pens.Width.Bottom > 0)
                dc.DrawLine(pens.Bottom, r.BottomLeft, r.BottomRight);
            if (pens.Width.Left > 0)
                dc.DrawLine(pens.Left, r.TopLeft, r.BottomLeft);
            if (pens.Width.Right > 0)
                dc.DrawLine(pens.Right, r.TopRight, r.BottomRight);
        }

        private void UpdatePadding()
        {
            Thickness result = pens.Width;
            Padding = result;
        }
    }
}