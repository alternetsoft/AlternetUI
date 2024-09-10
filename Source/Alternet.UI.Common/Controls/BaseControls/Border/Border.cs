using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Draws a border, background, or both around another control.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class Border : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public Border(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            RefreshOptions = ControlRefreshOptions.RefreshOnBorder
                | ControlRefreshOptions.RefreshOnBackground;
            Borders ??= new();
            var settings = CreateBorderSettings(BorderSettings.Default);
            Borders.SetAll(settings);
            UpdatePadding();
            Borders.Normal!.PropertyChanged += OnSettingsPropertyChanged;
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
            get => BorderSettings.Default.Width;
            set => BorderSettings.Default.Width = value;
        }

        /// <inheritdoc/>
        public override RectD ChildrenLayoutBounds
        {
            get
            {
                var bounds = base.ChildrenLayoutBounds;
                bounds.X += BorderWidth.Left;
                bounds.Y += BorderWidth.Top;
                if (bounds.Size == 0)
                    return bounds;
                bounds.Width -= BorderWidth.Horizontal;
                bounds.Height -= BorderWidth.Vertical;
                return bounds;
            }
        }

        /// <summary>
        /// Gets or sets the border width for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// You can specify different widths for the left, top, bottom and right
        /// edges of the border.
        /// </remarks>
        public virtual Thickness BorderWidth
        {
            get => Normal.Width;
            set
            {
                value.ApplyMin(0);
                if (Normal.Width == value)
                    return;
                Normal.SetWidth(value);
                UpdatePadding();
                PerformLayout();
                Refresh();
            }
        }

        /// <inheritdoc cref="BorderSettings.UniformCornerRadius"/>
        public virtual Coord? UniformCornerRadius
        {
            get
            {
                return Normal.UniformCornerRadius;
            }

            set
            {
                if (Normal.UniformCornerRadius == value)
                    return;
                Normal.UniformCornerRadius = value;
                Refresh();
            }
        }

        /// <inheritdoc cref="BorderSettings.UniformRadiusIsPercent"/>
        public virtual bool? UniformRadiusIsPercent
        {
            get
            {
                return Normal.UniformRadiusIsPercent;
            }

            set
            {
                if (Normal.UniformRadiusIsPercent == value)
                    return;
                Normal.UniformRadiusIsPercent = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the uniform border width for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// This value is applied to all the sides. If returned value is not null,
        /// all border sides
        /// have the same settings.
        /// </remarks>
#if DEBUG
        [Browsable(true)]
#else
        [Browsable(false)]
#endif
        public virtual Coord? UniformBorderWidth
        {
            get
            {
                if (Normal.Width.IsUniform)
                    return Normal.Width.Left;
                else
                    return null;
            }

            set
            {
                value ??= 0;
                var w = new Thickness(value.Value);
                BorderWidth = w;
            }
        }

        /// <inheritdoc cref="Control.Background"/>
        [Browsable(true)]
        public override Brush? Background
        {
            get => base.Background;
            set
            {
                base.Background = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Border;

        /// <summary>
        /// Gets or sets the border color for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// If this property is null, <see cref="BorderSettings.DefaultColor"/> is used
        /// for the border color.
        /// </remarks>
        public virtual Color? BorderColor
        {
            get
            {
                return Normal.Color ?? ColorUtils.GetDefaultBorderColor(IsDarkBackground);
            }

            set
            {
                if (value == null)
                {
                    Normal.Color = ColorUtils.GetDefaultBorderColor(IsDarkBackground);
                }
                else
                    Normal.Color = (Color)value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets individual border side settings.
        /// </summary>
        [Browsable(false)]
        public virtual BorderSettings Normal
        {
            get
            {
                Borders ??= new();
                return Borders.Normal ??= new(BorderSettings.Default);
            }

            set
            {
                Borders ??= new();
                if (value == null)
                    Borders.Normal?.Assign(BorderSettings.Default);
                else
                    Borders.Normal?.Assign(value);
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        /// <summary>
        /// Creates border filled with default settings.
        /// </summary>
        /// <returns></returns>
        public static BorderSettings CreateDefault(Color? color = null)
        {
            BorderSettings result = new(BorderSettings.Default);
            if (color is not null)
                result.Color = color;
            return result;
        }

        /// <inheritdoc cref="BorderSettings.SetColors"/>
        public void SetColors(Color left, Color top, Color right, Color bottom)
        {
            if (Normal.SetColors(left, top, right, bottom))
                Refresh();
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var width = Normal.Width;
            return base.GetPreferredSize(availableSize) + (width.Horizontal, width.Vertical);
        }

        /// <inheritdoc/>
        public override void DefaultPaint(Graphics dc, RectD rect)
        {
            DrawDefaultBackground(dc, rect);
        }

        /// <summary>
        /// Creates used <see cref="BorderSettings"/> instance. Override to use have border
        /// painting or non-default behavior.
        /// </summary>
        protected virtual BorderSettings CreateBorderSettings(BorderSettings defaultSettings)
        {
            return new(defaultSettings);
        }

        private void UpdatePadding()
        {
            Thickness result = Normal.Width;
            Padding = result;
        }

        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }
    }
}