using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Draws a border, background, or both around another control.
    /// </summary>
    public class Border : UserPaintControl
    {
        private static readonly BorderPens DefaultPens = new();
        private readonly BorderPens pens = DefaultPens.Clone();

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            UpdatePadding();
        }

        /// <summary>
        /// Gets or sets the default border width for
        /// the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// You can specify different widths for the left, top, bottom and right
        /// edges of the border.
        /// </remarks>
        public static Thickness DefaultBorderWidth
        {
            get => DefaultPens.Width;
            set => DefaultPens.Width = value;
        }

        /// <summary>
        /// Gets or sets the default border color for
        /// the <see cref="Border"/> control.
        /// </summary>
        public static Color DefaultBorderColor
        {
            get
            {
                return DefaultPens.Color;
            }

            set
            {
                DefaultPens.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the border width for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// You can specify different widths for the left, top, bottom and right
        /// edges of the border.
        /// </remarks>
        public Thickness BorderWidth
        {
            get => pens.Width;
            set
            {
                value.ApplyMinMax(0, 1);
                if (pens.Width == value)
                    return;
                pens.Width = value;
                UpdatePadding();
                Refresh();
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Border;

        /// <summary>
        /// Gets or sets the border color for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="DefaultBorderColor"/> is used
        /// for the border color.
        /// </remarks>
        public Color? BorderColor
        {
            get
            {
                return pens.Color;
            }

            set
            {
                if (value == null)
                    pens.Color = DefaultPens.Color;
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
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateBorderHandler(this);
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