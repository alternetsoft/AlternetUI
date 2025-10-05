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
        public static Color? DefaultColor;

        /// <summary>
        /// Default border color.
        /// </summary>
        public static Color DefaultCommonBorderColor = SystemColors.GrayText;

        private static BorderSettings? debugBorder;
        private static BorderSettings? debugBorderBlue;
        private static BorderSettings? debugBorderGreen;

        private readonly BorderSideSettings left = new();
        private readonly BorderSideSettings top = new();
        private readonly BorderSideSettings right = new();
        private readonly BorderSideSettings bottom = new();
        private Coord? uniformCornerRadius;
        private bool? uniformRadiusIsPercent = false;
        private BorderCornerRadius? topLeftRadius;
        private BorderCornerRadius? topRightRadius;
        private BorderCornerRadius? bottomRightRadius;
        private BorderCornerRadius? bottomLeftRadius;

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
                return debugBorder
                    ??= Default.WithColor(DefaultDebugBorderColor ?? LightDarkColors.Red);
            }
        }

        /// <summary>
        /// Get border settings for the green debug border.
        /// </summary>
        public static BorderSettings DebugBorderGreen
        {
            get
            {
                return debugBorderGreen
                    ??= Default.WithColor(DefaultDebugBorderColor ?? LightDarkColors.Green);
            }
        }

        /// <summary>
        /// Get border settings for the blue debug border.
        /// </summary>
        public static BorderSettings DebugBorderBlue
        {
            get
            {
                return debugBorderBlue
                    ??= Default.WithColor(DefaultDebugBorderColor ?? LightDarkColors.Blue);
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
                return uniformCornerRadius;
            }

            set
            {
                if (uniformCornerRadius == value || Immutable)
                    return;
                uniformCornerRadius = value;
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
                return uniformRadiusIsPercent;
            }

            set
            {
                if (uniformRadiusIsPercent == value || Immutable)
                    return;
                uniformRadiusIsPercent = value;
                RaisePropertyChanged(nameof(UniformCornerRadius));
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

        internal virtual BorderCornerRadius? TopLeftRadius
        {
            get
            {
                return topLeftRadius;
            }

            set
            {
                if (topLeftRadius == value || Immutable)
                    return;
                topLeftRadius = value;
                RaisePropertyChanged(nameof(TopLeftRadius));
            }
        }

        internal virtual BorderCornerRadius? TopRightRadius
        {
            get
            {
                return topRightRadius;
            }

            set
            {
                if (topRightRadius == value || Immutable)
                    return;
                topRightRadius = value;
                RaisePropertyChanged(nameof(TopRightRadius));
            }
        }

        internal virtual BorderCornerRadius? BottomRightRadius
        {
            get
            {
                return bottomRightRadius;
            }

            set
            {
                if (bottomRightRadius == value || Immutable)
                    return;
                bottomRightRadius = value;
                RaisePropertyChanged(nameof(BottomRightRadius));
            }
        }

        internal virtual BorderCornerRadius? BottomLeftRadius
        {
            get
            {
                return bottomLeftRadius;
            }

            set
            {
                if (bottomLeftRadius == value || Immutable)
                    return;
                bottomLeftRadius = value;
                RaisePropertyChanged(nameof(BottomLeftRadius));
            }
        }

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
        public static void DrawDesignCorners(object? sender, PaintEventArgs args)
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

            var defaultColor = DefaultCommonBorderColor;

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
        /// Same as <see cref="Width"/> but implemented as method.
        /// </summary>
        /// <param name="value">New width.</param>
        public void SetWidth(Thickness value)
        {
            Width = value;
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

            uniformCornerRadius = value.uniformCornerRadius;
            uniformRadiusIsPercent = value.uniformRadiusIsPercent;
            topLeftRadius = value.topLeftRadius;
            topRightRadius = value.topRightRadius;
            bottomRightRadius = value.bottomRightRadius;
            bottomLeftRadius = value.bottomLeftRadius;
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
