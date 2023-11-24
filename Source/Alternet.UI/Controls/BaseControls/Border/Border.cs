using System;
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
        private readonly ControlStateBorders borders = new();
        private bool hasBorder = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border()
        {
            borders.Normal = CreateBorderSettings(BorderSettings.Default);
            UpdatePadding();
            borders.Normal.PropertyChanged += Settings_PropertyChanged;
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

        /// <summary>
        /// Gets or sets the default border color for
        /// the <see cref="Border"/> control.
        /// </summary>
        public static Color DefaultBorderColor
        {
            get
            {
                return BorderSettings.Default.Color ?? BorderSideSettings.DefaultColor;
            }

            set
            {
                BorderSettings.Default.Color = value;
            }
        }

        /// <summary>
        /// Gets border for all states of the control.
        /// </summary>
        [Browsable(false)]
        public ControlStateBorders Borders => borders;

        /// <inheritdoc/>
        public override Rect ChildrenLayoutBounds
        {
            get
            {
                var bounds = base.ChildrenLayoutBounds;
                bounds.X += BorderWidth.Left;
                bounds.Y += BorderWidth.Top;
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
        public Thickness BorderWidth
        {
            get => Normal.Width;
            set
            {
                value.ApplyMin(0);
                if (Normal.Width == value)
                    return;
                Normal.SetWidth(value);
                UpdatePadding();
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (hasBorder == value)
                    return;
                hasBorder = value;
                Refresh();
            }
        }

        /// <inheritdoc cref="BorderSettings.UniformCornerRadius"/>
        public double? UniformCornerRadius
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
        public bool? UniformRadiusIsPercent
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
        /// This value is applied to all the sides. If returned value is not null, all border sides
        /// have the same settings.
        /// </remarks>
#if DEBUG
        [Browsable(true)]
#else
        [Browsable(false)]
#endif
        public double? UniformBorderWidth
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
        /// If this property is null, <see cref="DefaultBorderColor"/> is used
        /// for the border color.
        /// </remarks>
        public Color? BorderColor
        {
            get
            {
                return Normal.Color;
            }

            set
            {
                if (value == null)
                    Normal.Color = BorderSettings.Default.Color;
                else
                    Normal.Color = (Color)value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets individual border side settings.
        /// </summary>
        [Browsable(false)]
        public BorderSettings Normal
        {
            get
            {
                return borders.Normal ??= new(BorderSettings.Default);
            }

            set
            {
                if (value == null)
                    borders.Normal?.Assign(BorderSettings.Default);
                else
                    borders.Normal?.Assign(value);
            }
        }

        /// <summary>
        /// Gets <see cref="BorderSettings"/> for the specified control state.
        /// </summary>
        /// <param name="state">Control state</param>
        public BorderSettings GetSettings(GenericControlState state)
        {
            return borders.GetObjectOrDefault(state, Normal);
        }

        /// <inheritdoc cref="BorderSettings.SetColors"/>
        public void SetColors(Color left, Color top, Color right, Color bottom)
        {
            if (Normal.SetColors(left, top, right, bottom))
                Refresh();
        }

        /// <inheritdoc/>
        public override Size GetPreferredSize(Size availableSize)
        {
            var width = Normal.Width;
            return base.GetPreferredSize(availableSize) + (width.Horizontal, width.Vertical);
        }

        /// <summary>
        /// Creates used <see cref="BorderSettings"/> instance. Override to use have border
        /// painting or non-default behavior.
        /// </summary>
        protected virtual BorderSettings CreateBorderSettings(BorderSettings defaultSettings)
        {
            return new(defaultSettings);
        }

        /// <inheritdoc/>
        protected override void OnCurrentStateChanged()
        {
            base.OnCurrentStateChanged();
            if (borders.HasOtherStates || (Backgrounds?.HasOtherStates ?? false))
                Refresh();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateBorderHandler(this);
        }

        private void UpdatePadding()
        {
            Thickness result = Normal.Width;
            Padding = result;
        }

        private void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }
    }
}