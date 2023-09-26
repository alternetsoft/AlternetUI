using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Draws a border, background, or both around another control.
    /// </summary>
    [ControlCategory("Containers")]
    public class Border : UserPaintControl
    {
        private static readonly BorderSettings DefaultPens = new();
        private readonly BorderSettings pens = DefaultPens.Clone();

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
                value.ApplyMin(0);
                if (pens.Width == value)
                    return;
                pens.Width = value;
                UpdatePadding();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the border width for the <see cref="Border"/> control.
        /// </summary>
        public double? UniformBorderWidth
        {
            get
            {
                if (pens.Width.IsUniform)
                    return pens.Width.Left;
                else
                    return null;
            }

            set
            {
                if (value is null)
                    value = 0;
                var w = new Thickness(value.Value);
                BorderWidth = w;
            }
        }

        /// <inheritdoc cref="Control.Background"/>
        public new Brush? Background
        {
            get => base.Background;
            set
            {
                if (Background == value)
                    return;
                base.Background = value;
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

        /// <summary>
        /// Gets individual border side settings.
        /// </summary>
        internal BorderSettings Pens => pens;

        /// <inheritdoc/>
        public override Size GetPreferredSize(Size availableSize)
        {
            return base.GetPreferredSize(availableSize)/* +
                new Size(pens.Width.Horizontal, pens.Width.Vertical)*/;
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateBorderHandler(this);
        }

        private void UpdatePadding()
        {
            Thickness result = pens.Width;
            Padding = result;
        }
    }
}