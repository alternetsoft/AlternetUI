using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    // https://developer.mozilla.org/en-US/docs/Web/CSS/border-radius
    // https://developer.mozilla.org/en-US/docs/Web/CSS/border-top-left-radius

    /// <summary>
    /// Specifies <see cref="Border"/> drawing settings.
    /// </summary>
    public class BorderSettings : ImmutableObject
    {
        /// <summary>
        /// Default border settings.
        /// </summary>
        public static readonly BorderSettings Default = new();

        /// <summary>
        /// Temporary border used for calculations.
        /// </summary>
        public static readonly BorderSettings Temp = new();

        /// <summary>
        /// Gets or sets color of the debug border. Default value is Null.
        /// In this case red color is used.
        /// </summary>
        public static Color? DefaultDebugBorderColor;

        /// <summary>
        /// Gets or sets size of the design corners.
        /// </summary>
        public static Coord DesignCornerSize = 5;

        /// <summary>
        /// Gets or sets default border color.
        /// </summary>
        [Obsolete("Use DefaultColors.BorderColor instead.")]
        public static readonly Color? DefaultColor;

        /// <summary>
        /// Default border color.
        /// </summary>
        [Obsolete("Use DefaultColors.BorderColor instead.")]
        public static readonly Color DefaultCommonBorderColor = SystemColors.GrayText;

        private static BorderSettings? debugBorder;
        private static BorderSettings? debugBorderBlue;
        private static BorderSettings? debugBorderGreen;
        private static BorderSettings? accentBorder;
        private static BorderSettings? transparentBorder;
        private static BorderSettings? emptyBorder;
        private static BorderSettings? bottomLineBorder;

        private readonly BorderSideSettings left = new();
        private readonly BorderSideSettings top = new();
        private readonly BorderSideSettings right = new();
        private readonly BorderSideSettings bottom = new();

        private BorderCornerRadius cornerRadius;
        private BaseCollection<BorderSettings>? innerBorders;
        private Thickness innerBorderMargin = Thickness.One;
        private bool innerBorderVisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderSettings"/> class.
        /// </summary>
        public BorderSettings()
        {
            left.PropertyChanged += OnLeftPropertyChanged;
            right.PropertyChanged += OnRightPropertyChanged;
            top.PropertyChanged += OnTopPropertyChanged;
            bottom.PropertyChanged += OnBottomPropertyChanged;
        }

        /// <summary>
        /// Initializes a new instance of the BorderSettings class with the specified border width and color.
        /// </summary>
        /// <param name="width">The thickness of the border to apply. Specifies the width for each side.</param>
        /// <param name="color">The color of the border. If null, the default color is used.</param>
        public BorderSettings(Thickness width, Color? color = null)
            : this()
        {
            Width = width;

            if (color != null)
                Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the BorderSettings class with the specified border thickness and optional color.
        /// </summary>
        /// <param name="left">The thickness of the left border, in device-independent units.</param>
        /// <param name="top">The thickness of the top border, in device-independent units.</param>
        /// <param name="right">The thickness of the right border, in device-independent units.</param>
        /// <param name="bottom">The thickness of the bottom border, in device-independent units.</param>
        /// <param name="color">The color of the border. If null, the default border color is used.</param>
        public BorderSettings(float left, float top, float right, float bottom, Color? color = null)
            : this(new Thickness(left, top, right, bottom), color)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderSettings"/> class
        /// and assign its properties from <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        public BorderSettings(BorderSettings source)
            : this()
        {
            Assign(source);
        }

        /// <summary>
        /// Occurs when the border is redrawn.
        /// </summary>
        public event EventHandler<PaintEventArgs>? Paint;

        /// <summary>
        /// Get border settings for the default debug border.
        /// </summary>
        public static BorderSettings DebugBorder
        {
            get
            {
                return debugBorder ??= Default.WithColor(DefaultDebugBorderColor ?? LightDarkColors.Red);
            }

            set
            {
                debugBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets the border settings for the bottom line.
        /// </summary>
        /// <remarks>The default border has a width of 1 on the bottom edge and 0 on the other sides.</remarks>
        public static BorderSettings BottomLineBorder
        {
            get
            {
                if (bottomLineBorder == null)
                {
                    bottomLineBorder = new();
                    bottomLineBorder.Width = new Thickness(0, 0, 0, 1);
                }

                return bottomLineBorder;
            }
            set
            {
                bottomLineBorder = value;
            }
        }

        /// <summary>
        /// Gets a border settings instance configured with a transparent color.
        /// </summary>
        /// <remarks>Use this property when a border is required but should not be visible, allowing the
        /// underlying background to show through. The returned instance can be used in scenarios where a non-intrusive
        /// or invisible border is desired.</remarks>
        public static BorderSettings TransparentBorder
        {
            get
            {
                var result = transparentBorder ??= Default.WithColor(Color.Transparent);
                result.SetImmutable();
                return result;
            }
        }

        /// <summary>
        /// Gets a static instance of border settings configured with an empty color value.
        /// </summary>
        /// <remarks>This property returns a shared, read-only instance of the BorderSettings class with
        /// its color set to <see cref="Color.Empty"/>. The same instance is reused throughout the application, which
        /// can help reduce memory usage when a border with no color is needed.</remarks>
        public static BorderSettings EmptyColorBorder
        {
            get
            {
                var result = emptyBorder ??= Default.WithColor(Color.Empty);
                result.SetImmutable();
                return result;
            }
        }

        /// <summary>
        /// Gets or sets border settings for the accent border. Accent border is used to indicate active or focused elements.
        /// It uses <see cref="DefaultColors.AccentColor"/> as default color. If you want to have the same accent border on all platforms,
        /// set <see cref="DefaultColors.AccentColor"/> to the desired color.
        /// </summary>
        public static BorderSettings AccentBorder
        {
            get
            {
                return accentBorder ??= Default.WithColor(DefaultColors.AccentColor);
            }

            set
            {
                accentBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets border settings for the green debug border.
        /// </summary>
        public static BorderSettings DebugBorderGreen
        {
            get
            {
                return debugBorderGreen ??= Default.WithColor(LightDarkColors.Green);
            }

            set
            {
                debugBorderGreen = value;
            }
        }

        /// <summary>
        /// Gets or sets border settings for the blue debug border.
        /// </summary>
        public static BorderSettings DebugBorderBlue
        {
            get
            {
                return debugBorderBlue ??= Default.WithColor(LightDarkColors.Blue);
            }

            set
            {
                debugBorderBlue = value;
            }
        }

        /// <summary>
        /// Gets or sets the uniform corner radius for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// This value is applied to all the corners. If returned value is not null, all border corners
        /// have the same settings.
        /// </remarks>
        public virtual Coord? UniformCornerRadius
        {
            get
            {
                return cornerRadius.Value;
            }

            set
            {
                if (cornerRadius.Value == value || Immutable)
                    return;
                cornerRadius.SetValue(value);
                RaisePropertyChanged(nameof(UniformCornerRadius));
            }
        }

        /// <summary>
        /// Gets or sets whether the uniform corner radius for the <see cref="Border"/> control
        /// is specified in percent.
        /// </summary>
        /// <remarks>
        /// This value is applied to all the corners. If returned value is not null, all border corners
        /// have the same settings.
        /// </remarks>
        public virtual bool? UniformRadiusIsPercent
        {
            get
            {
                return cornerRadius.IsPercent;
            }

            set
            {
                if (cornerRadius.IsPercent == value || Immutable)
                    return;
                cornerRadius.IsPercent = value;
                RaisePropertyChanged(nameof(UniformCornerRadius));
            }
        }

        /// <summary>
        /// Gets whether this border has inner borders. Inner border is drawn inside the main border
        /// and can be used to create layered border effects.
        /// </summary>
        public virtual bool HasInnerBorders => innerBorders != null && innerBorders.Count > 0;

        /// <summary>
        /// Gets the collection of inner border settings.
        /// </summary>
        /// <remarks>The collection is initialized on first access. Use this property to configure
        /// multiple inner borders for advanced styling or layout scenarios. Modifying the collection affects the
        /// appearance of the element's inner borders.</remarks>
        public virtual BaseCollection<BorderSettings> InnerBorders
        {
            get
            {
                return innerBorders ??= new();
            }
        }

        /// <summary>
        /// Gets or sets whether the inner borders are visible.
        /// </summary>
        public virtual bool InnerBorderVisible
        {
            get => innerBorderVisible;
            set
            {
                if (innerBorderVisible == value || Immutable)
                    return;
                innerBorderVisible = value;
                RaisePropertyChanged(nameof(InnerBorderVisible));
            }
        }

        /// <summary>
        /// Gets or sets the inner border margin. It determines the spacing between this border and the
        /// first inner border.
        /// </summary>
        /// <remarks>Changing this property raises the PropertyChanged event, allowing the user interface
        /// to update in response to margin changes. </remarks>
        public virtual Thickness InnerBorderMargin
        {
            get => innerBorderMargin;
            set
            {
                if (innerBorderMargin == value || Immutable)
                    return;
                innerBorderMargin = value;
                RaisePropertyChanged(nameof(InnerBorderMargin));
            }
        }

        /// <summary>
        /// Gets or sets whether to draw default border.
        /// </summary>
        /// <remarks>
        /// If <see cref="DrawDefaultBorder"/> is <c>false</c>, border can be painted
        /// using <see cref="Paint"/> event handler.
        /// </remarks>
        [Browsable(false)]
        public virtual bool DrawDefaultBorder { get; set; } = true;

        /// <summary>
        /// Gets whether all sides have same color and width.
        /// </summary>
        [Browsable(false)]
        public bool IsUniform => IsUniformColor && Width.IsUniform;

        /// <summary>
        /// Gets settings for the left edge of the border.
        /// </summary>
        public BorderSideSettings Left => left;

        /// <summary>
        /// Gets settings for the top edge of the border.
        /// </summary>
        public BorderSideSettings Top => top;

        /// <summary>
        /// Gets settings for the right edge of the border.
        /// </summary>
        public BorderSideSettings Right => right;

        /// <summary>
        /// Gets settings for the bottom edge of the border.
        /// </summary>
        public BorderSideSettings Bottom => bottom;

        /// <summary>
        /// Gets or sets uniform color of the border lines.
        /// </summary>
        [Browsable(false)]
        public virtual Color? Color
        {
            get
            {
                if (IsUniformColor)
                    return top.Color;
                return null;
            }

            set
            {
                if (Color == value || Immutable)
                    return;
                SetColors(value, value, value, value);
            }
        }

        /// <summary>
        /// Gets or sets the border widths.
        /// </summary>
        /// <remarks>
        /// You can specify different widths for the left, top, bottom and right
        /// edges of the border.
        /// </remarks>
        public virtual Thickness Width
        {
            get
            {
                return new Thickness(left.Width, top.Width, right.Width, bottom.Width);
            }

            set
            {
                if (Width == value || Immutable)
                    return;
                value.ApplyMin(0);
                left.Width = value.Left;
                right.Width = value.Right;
                top.Width = value.Top;
                bottom.Width = value.Bottom;
            }
        }

        /// <summary>
        /// Gets whether colors of the left and right edges of the border are equal.
        /// </summary>
        public bool IsUniformVerticalColor => left.Color == right.Color;

        /// <summary>
        /// Gets whether colors of the top and bottom edges of the border are equal.
        /// </summary>
        private bool IsUniformHorizontalColor => top.Color == bottom.Color;

        /// <summary>
        /// Gets whether colors of the vertical and horizontal edges of the border are equal.
        /// </summary>
        private bool IsUniformColor =>
            IsUniformVerticalColor && IsUniformHorizontalColor;

        /// <summary>
        /// <see cref="Paint"/> event handler implementation which draws design corners used
        /// to indicate control's bounds.
        /// </summary>
        /// <param name="sender"><see cref="BorderSettings"/> instance.</param>
        /// <param name="args">Event arguments.</param>
        public static void DrawDesignCornersHandler(object? sender, PaintEventArgs args)
        {
            if (sender is not BorderSettings border)
                return;
            DrawDesignCorners(args.Graphics, args.ClientRectangle, border);
        }

        /// <summary>
        /// Calculates a uniform corner radius for a rectangle, optionally
        /// treating the radius as a percentage of the rectangle's minimum dimension.
        /// </summary>
        /// <param name="rect">The rectangle for which the corner radius is being calculated.</param>
        /// <param name="cornerRadius">The uniform corner radius to apply.
        /// If <see langword="null"/>, the method returns <see langword="null"/>.</param>
        /// <param name="radiusIsPercent">A value indicating whether
        /// the <paramref name="cornerRadius"/> is expressed as a percentage of the
        /// rectangle's minimum dimension. If <see langword="null"/>, the method returns
        /// <see langword="null"/>.</param>
        /// <returns>The calculated corner radius as a <see cref="Coord"/> value.
        /// If <paramref name="cornerRadius"/> or
        /// <paramref name="radiusIsPercent"/> is <see langword="null"/>, the method
        /// returns <see langword="null"/>. If <paramref name="radiusIsPercent"/>
        /// is <see langword="true"/>, the radius is
        /// calculated as a percentage of the rectangle's minimum dimension.
        /// Otherwise, the method returns the value of <paramref name="cornerRadius"/>.</returns>
        public static Coord? GetUniformCornerRadius(
            RectD rect,
            Coord? cornerRadius,
            bool? radiusIsPercent)
        {
            var radius = cornerRadius;
            if (radius is null)
                return null;
            var isPercent = radiusIsPercent;
            if (isPercent is null)
                return null;
            if (isPercent.Value)
            {
                return rect.PercentOfMinSize(radius.Value);
            }
            else
                return radius.Value;
        }

        /// <summary>
        /// Draws design corners used to indicate element bounds.
        /// </summary>
        public static void DrawDesignCorners(Graphics dc, RectD rect, BorderSettings? border = null)
        {
            border ??= BorderSettings.DebugBorder;

            void DrawHorizontal(Graphics dc, Brush brush, RectD rect)
            {
                var rect1 = rect;
                var rect2 = rect;
                rect1.Width = DesignCornerSize;
                rect2.Location = rect2.TopRight - new SizeD(DesignCornerSize, 0);
                rect2.Width = DesignCornerSize;
                dc.FillRectangle(brush, rect1);
                dc.FillRectangle(brush, rect2);
            }

            void DrawVertical(Graphics dc, Brush brush, RectD rect)
            {
                var rect1 = rect;
                var rect2 = rect;
                rect1.Height = DesignCornerSize;
                rect2.Location = rect2.BottomLeft - new SizeD(0, DesignCornerSize);
                rect2.Height = DesignCornerSize;
                dc.FillRectangle(brush, rect1);
                dc.FillRectangle(brush, rect2);
            }

            var defaultColor = SystemColors.GrayText;

            if (border.Top.Width > 0)
            {
                DrawHorizontal(dc, border.Top.GetBrush(defaultColor), border.GetTopRectangle(rect));
            }

            if (border.Bottom.Width > 0)
            {
                DrawHorizontal(
                    dc,
                    border.Bottom.GetBrush(defaultColor),
                    border.GetBottomRectangle(rect));
            }

            if (border.Left.Width > 0)
            {
                DrawVertical(dc, border.Left.GetBrush(defaultColor), border.GetLeftRectangle(rect));
            }

            if (border.Right.Width > 0)
            {
                DrawVertical(dc, border.Right.GetBrush(defaultColor), border.GetRightRectangle(rect));
            }
        }

        /// <summary>
        /// Sets the corner radius for the border.
        /// </summary>
        /// <param name="cornerRadius">The corner radius to set.</param>
        public virtual void SetCornerRadius(BorderCornerRadius? cornerRadius)
        {
            if (Immutable)
                return;
            this.cornerRadius = cornerRadius ?? new();
            RaisePropertyChanged(nameof(UniformCornerRadius));
        }

        /// <summary>
        /// Same as <see cref="Width"/> but implemented as method.
        /// </summary>
        /// <param name="value">New width.</param>
        public void SetWidth(Thickness value)
        {
            Width = value;
        }

        /// <summary>
        /// Sets the visibility of the inner border.
        /// </summary>
        /// <param name="value">A boolean value indicating whether the inner border should be visible. Pass <see langword="true"/> to show
        /// the inner border; otherwise, pass <see langword="false"/> to hide it.</param>
        public void SetInnerBorderVisible(bool value)
        {
            InnerBorderVisible = value;
        }

        /// <summary>
        /// Gets rectangle of the top border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public virtual RectD GetTopRectangle(RectD rect)
        {
            return DrawingUtils.GetTopLineRect(rect, Top.Width);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public virtual RectD GetBottomRectangle(RectD rect)
        {
            return DrawingUtils.GetBottomLineRect(rect, Bottom.Width);
        }

        /// <summary>
        /// Gets rectangle of the left border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public virtual RectD GetLeftRectangle(RectD rect)
        {
            return DrawingUtils.GetLeftLineRect(rect, Left.Width);
        }

        /// <summary>
        /// Gets rectangle of the right border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public virtual RectD GetRightRectangle(RectD rect)
        {
            return DrawingUtils.GetRightLineRect(rect, Right.Width);
        }

        /// <summary>
        /// Gets the first non-null pen from the left, top, right, or bottom sides.
        /// This method uses <see cref="BorderSideSettings.Pen"/> properties of each side.
        /// </summary>
        /// <remarks>The order of precedence is left, then top, then right, then bottom. This method is
        /// useful when a default pen is needed from any side.</remarks>
        /// <returns>A <see cref="Pen"/> instance from the first non-null side, or <see langword="null"/> if all sides have null
        /// pens.</returns>
        public virtual Pen? GetPen()
        {
            return Left.Pen ?? Top.Pen ?? Right.Pen ?? Bottom.Pen;
        }

        /// <summary>
        /// Gets real uniform corner radius using <see cref="UniformCornerRadius"/>,
        /// <see cref="UniformRadiusIsPercent"/> and <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect">Rectangle for percentage calculation.</param>
        /// <returns></returns>
        public virtual Coord? GetUniformCornerRadius(RectD rect)
        {
            return GetUniformCornerRadius(
                        rect,
                        UniformCornerRadius,
                        UniformRadiusIsPercent);
        }

        /// <summary>
        /// Calls paint event with the specified rectangles.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="clipRect">The invalidated rectangle.</param>
        /// <param name="clientRect">The client rectangle.</param>
        public void InvokePaint(Graphics dc, RectD clipRect, RectD clientRect)
        {
            Paint?.Invoke(this, new PaintEventArgs(() => dc, clipRect, clientRect));
        }

        /// <summary>
        /// Calls paint event with the specified rectangle.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="clientRect">The client rectangle.</param>
        public void InvokePaint(Graphics dc, RectD clientRect)
        {
            Paint?.Invoke(this, new PaintEventArgs(() => dc, clientRect, clientRect));
        }

        /// <summary>
        /// Gets whether color is ok for painting the border.
        /// </summary>
        /// <param name="color">Color to check.</param>
        /// <returns></returns>
        public virtual bool ColorIsOk(Color? color)
        {
            return color is not null && color.IsOk && color != Color.Transparent && !color.IsEmpty;
        }

        /// <summary>
        /// Same as using <see cref="Color"/> property.
        /// </summary>
        /// <param name="value">New uniform border color.</param>
        public void SetColor(Color? value)
        {
            Color = value;
        }

        /// <summary>
        /// Sets colors of the individual border edges.
        /// </summary>
        /// <param name="leftColor">Color of the left edge.</param>
        /// <param name="topColor">Color of the top edge.</param>
        /// <param name="rightColor">Color of the right edge.</param>
        /// <param name="bottomColor">Color of the bottom edge.</param>
        public virtual bool SetColors(
            Color? leftColor,
            Color? topColor,
            Color? rightColor,
            Color? bottomColor)
        {
            if (Immutable)
                return false;

            var result = false;

            if (left.Color != leftColor)
            {
                left.Color = leftColor;
                result = true;
            }

            if (right.Color != rightColor)
            {
                right.Color = rightColor;
                result = true;
            }

            if (top.Color != topColor)
            {
                top.Color = topColor;
                result = true;
            }

            if (bottom.Color != bottomColor)
            {
                bottom.Color = bottomColor;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Gets this border with all valid side colors set to the specified value.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Border side color is changed only if <see cref="ColorIsOk"/> returns <c>true</c>
        /// for this color.
        /// </remarks>
        public virtual BorderSettings ToColor(Color value)
        {
            var result = Clone();
            var leftColor = ColorIsOk(Left.Color) ? value : Left.Color;
            var topColor = ColorIsOk(Top.Color) ? value : Top.Color;
            var rightColor = ColorIsOk(Right.Color) ? value : Right.Color;
            var bottomColor = ColorIsOk(Bottom.Color) ? value : Bottom.Color;
            result.SetColors(leftColor, topColor, rightColor, bottomColor);
            return result;
        }

        /// <summary>
        /// Gets this border with all colors set to <see cref="SystemColors.GrayText"/>
        /// </summary>
        /// <returns></returns>
        public virtual BorderSettings ToGrayScale()
        {
            var result = ToColor(SystemColors.GrayText);
            return result;
        }

        /// <summary>
        /// Creates clone of the <see cref="BorderSettings"/>
        /// </summary>
        public virtual BorderSettings Clone()
        {
            BorderSettings result = new();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Creates clone of this object with changed border color.
        /// </summary>
        /// <param name="color">Border color of the new <see cref="BorderSettings"/>.</param>
        /// <returns></returns>
        public virtual BorderSettings WithColor(Color color)
        {
            var result = Clone();
            result.Color = color;
            return result;
        }

        /// <summary>
        /// Assign properties from another object.
        /// </summary>
        /// <param name="value">Source of the properties to assign.</param>
        public virtual void Assign(BorderSettings value)
        {
            if (Immutable)
                return;

            left.Assign(value.left);
            right.Assign(value.right);
            top.Assign(value.top);
            bottom.Assign(value.bottom);

            cornerRadius = value.cornerRadius;
        }

        /// <summary>
        /// Sets a uniform corner radius for all corners of the element.
        /// </summary>
        /// <remarks>Use this method to ensure consistent rounding on all corners of the element. Passing
        /// <see langword="null"/> will clear any previously set corner radius.</remarks>
        /// <param name="value">The corner radius to apply to all corners, specified as a <see langword="Coord"/>. Specify <see
        /// langword="null"/> to remove any applied corner radius and revert to the default.</param>
        public void SetUniformCornerRadius(Coord? value)
        {
            UniformCornerRadius = value;
        }

        private void OnLeftPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Left));
        }

        private void OnRightPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Right));
        }

        private void OnTopPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Top));
        }

        private void OnBottomPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Bottom));
        }
    }
}
