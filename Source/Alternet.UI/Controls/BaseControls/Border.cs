using System.ComponentModel;
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
        private readonly BorderSettings settings = DefaultPens.Clone();

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
            get => settings.Width;
            set
            {
                value.ApplyMin(0);
                if (settings.Width == value)
                    return;
                settings.Width = value;
                UpdatePadding();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the border width for the <see cref="Border"/> control.
        /// </summary>
#if DEBUG
        [Browsable(true)]
#else
        [Browsable(false)]
#endif
        public double? UniformBorderWidth
        {
            get
            {
                if (settings.Width.IsUniform)
                    return settings.Width.Left;
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
                return settings.Color;
            }

            set
            {
                if (value == null)
                    settings.Color = DefaultPens.Color;
                else
                    settings.Color = (Color)value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets individual border side settings.
        /// </summary>
        internal BorderSettings Settings => settings;

        /// <summary>
        /// Sets colors of the individual border sides.
        /// </summary>
        /// <param name="left">Color of the left side.</param>
        /// <param name="top">Color of the top side.</param>
        /// <param name="right">Color of the right side.</param>
        /// <param name="bottom">Color of the bottom side.</param>
        public void SetColors(Color left, Color top, Color right, Color bottom)
        {
            if (settings.SetColors(left, top, right, bottom))
                Refresh();
        }

        /// <inheritdoc/>
        public override Size GetPreferredSize(Size availableSize)
        {
            return base.GetPreferredSize(availableSize) +
                new Size(settings.Width.Horizontal, settings.Width.Vertical);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateBorderHandler(this);
        }

        private void UpdatePadding()
        {
            Thickness result = settings.Width;
            Padding = result;
        }
    }
}