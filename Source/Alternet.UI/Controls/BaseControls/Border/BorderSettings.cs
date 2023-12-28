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
    public class BorderSettings : BaseObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Default border settings.
        /// </summary>
        public static readonly BorderSettings Default = new();

        private readonly BorderSideSettings left = new();
        private readonly BorderSideSettings top = new();
        private readonly BorderSideSettings right = new();
        private readonly BorderSideSettings bottom = new();
        private double? uniformCornerRadius;
        private bool? uniformRadiusIsPercent = true;
        private BorderCornerRadius? topLeftRadius;
        private BorderCornerRadius? topRightRadius;
        private BorderCornerRadius? bottomRightRadius;
        private BorderCornerRadius? bottomLeftRadius;

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderSettings"/> class.
        /// </summary>
        public BorderSettings()
        {
            left.PropertyChanged += Left_PropertyChanged;
            right.PropertyChanged += Right_PropertyChanged;
            top.PropertyChanged += Top_PropertyChanged;
            bottom.PropertyChanged += Bottom_PropertyChanged;
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
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the uniform corner radius for the <see cref="Border"/> control.
        /// </summary>
        /// <remarks>
        /// This value is applied to all the corners. If returned value is not null, all border corners
        /// have the same settings.
        /// </remarks>
        public double? UniformCornerRadius
        {
            get
            {
                return uniformCornerRadius;
            }

            set
            {
                if (uniformCornerRadius == value)
                    return;
                uniformCornerRadius = value;
                PropertyChanged?.Invoke(this, new(nameof(UniformCornerRadius)));
            }
        }

        /// <summary>
        /// Gets or sets whether the uniform corner radius for the <see cref="Border"/> control
        /// is specified in percents.
        /// </summary>
        /// <remarks>
        /// This value is applied to all the corners. If returned value is not null, all border corners
        /// have the same settings.
        /// </remarks>
        public bool? UniformRadiusIsPercent
        {
            get
            {
                return uniformRadiusIsPercent;
            }

            set
            {
                if (uniformRadiusIsPercent == value)
                    return;
                uniformRadiusIsPercent = value;
                PropertyChanged?.Invoke(this, new(nameof(UniformCornerRadius)));
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
        public bool DrawDefaultBorder { get; set; } = true;

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
        public Color? Color
        {
            get
            {
                if (IsUniformColor)
                    return top.Color;
                return null;
            }

            set
            {
                value ??= Default.Color ?? BorderSideSettings.DefaultColor;
                SetColors(value.Value, value.Value, value.Value, value.Value);
            }
        }

        /// <summary>
        /// Gets or sets the border widths.
        /// </summary>
        /// <remarks>
        /// You can specify different widths for the left, top, bottom and right
        /// edges of the border.
        /// </remarks>
        public Thickness Width
        {
            get
            {
                return new Thickness(left.Width, top.Width, right.Width, bottom.Width);
            }

            set
            {
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

        internal BorderCornerRadius? TopLeftRadius
        {
            get
            {
                return topLeftRadius;
            }

            set
            {
                if (topLeftRadius == value)
                    return;
                topLeftRadius = value;
                PropertyChanged?.Invoke(this, new(nameof(TopLeftRadius)));
            }
        }

        internal BorderCornerRadius? TopRightRadius
        {
            get
            {
                return topRightRadius;
            }

            set
            {
                if (topRightRadius == value)
                    return;
                topRightRadius = value;
                PropertyChanged?.Invoke(this, new(nameof(TopRightRadius)));
            }
        }

        internal BorderCornerRadius? BottomRightRadius
        {
            get
            {
                return bottomRightRadius;
            }

            set
            {
                if (bottomRightRadius == value)
                    return;
                bottomRightRadius = value;
                PropertyChanged?.Invoke(this, new(nameof(BottomRightRadius)));
            }
        }

        internal BorderCornerRadius? BottomLeftRadius
        {
            get
            {
                return bottomLeftRadius;
            }

            set
            {
                if (bottomLeftRadius == value)
                    return;
                bottomLeftRadius = value;
                PropertyChanged?.Invoke(this, new(nameof(BottomLeftRadius)));
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
            const double cornerSize = 5;

            void DrawHorizontal(Graphics dc, Brush brush, RectD rect)
            {
                var rect1 = rect;
                var rect2 = rect;
                rect1.Width = cornerSize;
                rect2.Location = rect2.TopRight - new SizeD(cornerSize, 0);
                rect2.Width = cornerSize;
                dc.FillRectangle(brush, rect1);
                dc.FillRectangle(brush, rect2);
            }

            void DrawVertical(Graphics dc, Brush brush, RectD rect)
            {
                var rect1 = rect;
                var rect2 = rect;
                rect1.Height = cornerSize;
                rect2.Location = rect2.BottomLeft - new SizeD(0, cornerSize);
                rect2.Height = cornerSize;
                dc.FillRectangle(brush, rect1);
                dc.FillRectangle(brush, rect2);
            }

            if (sender is not BorderSettings border)
                return;
            var dc = args.DrawingContext;
            var rect = args.Bounds;
            if (border.Top.Width > 0)
            {
                DrawHorizontal(dc, border.Top.Brush, border.GetTopRectangle(rect));
            }

            if (border.Bottom.Width > 0)
            {
                DrawHorizontal(dc, border.Bottom.Brush, border.GetBottomRectangle(rect));
            }

            if (border.Left.Width > 0)
            {
                DrawVertical(dc, border.Left.Brush, border.GetLeftRectangle(rect));
            }

            if (border.Right.Width > 0)
            {
                DrawVertical(dc, border.Right.Brush, border.GetRightRectangle(rect));
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
        public RectD GetTopRectangle(RectD rect)
        {
            return DrawingUtils.GetTopLineRect(rect, Top.Width);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public RectD GetBottomRectangle(RectD rect)
        {
            return DrawingUtils.GetBottomLineRect(rect, Bottom.Width);
        }

        /// <summary>
        /// Gets rectangle of the left border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public RectD GetLeftRectangle(RectD rect)
        {
            return DrawingUtils.GetLeftLineRect(rect, Left.Width);
        }

        /// <summary>
        /// Gets rectangle of the right border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public RectD GetRightRectangle(RectD rect)
        {
            return DrawingUtils.GetRightLineRect(rect, Right.Width);
        }

        /// <summary>
        /// Gets real uniform corner radius using <see cref="UniformCornerRadius"/>,
        /// <see cref="UniformRadiusIsPercent"/> and <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect">Ractangle for percentage calculation.</param>
        /// <returns></returns>
        public double? GetUniformCornerRadius(RectD rect)
        {
            var radius = UniformCornerRadius;
            if (radius is null)
                return null;
            var isPercent = UniformRadiusIsPercent;
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
        /// Draws border in the specified rectangle of the drawing context.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle.</param>
        public virtual void Draw(Graphics dc, RectD rect)
        {
            Paint?.Invoke(this, new PaintEventArgs(dc, rect));

            if (!DrawDefaultBorder)
                return;

            var radius = GetUniformCornerRadius(rect);

            if (radius != null)
            {
                dc.DrawRoundedRectangle(Top.Pen, rect.InflatedBy(-1, -1), radius.Value);
                return;
            }

            if (Top.Width > 0)
            {
                dc.FillRectangle(Top.Brush, GetTopRectangle(rect));
            }

            if (Bottom.Width > 0)
            {
                dc.FillRectangle(Bottom.Brush, GetBottomRectangle(rect));
            }

            if (Left.Width > 0)
            {
                dc.FillRectangle(Left.Brush, GetLeftRectangle(rect));
            }

            if (Right.Width > 0)
            {
                dc.FillRectangle(Right.Brush, GetRightRectangle(rect));
            }
        }

        /// <summary>
        /// Sets colors of the individual border edges.
        /// </summary>
        /// <param name="leftColor">Color of the left edge.</param>
        /// <param name="topColor">Color of the top edge.</param>
        /// <param name="rightColor">Color of the right edge.</param>
        /// <param name="bottomColor">Color of the bottom edge.</param>
        public bool SetColors(Color leftColor, Color topColor, Color rightColor, Color bottomColor)
        {
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
        /// Creates clone of the <see cref="BorderSettings"/>
        /// </summary>
        public BorderSettings Clone()
        {
            BorderSettings result = new();
            result.Assign(this);
            return result;
        }

        /// <summary>
        /// Assign properties from another object.
        /// </summary>
        /// <param name="value">Source of the properties to assign.</param>
        public void Assign(BorderSettings value)
        {
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

        private void Left_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new(nameof(Left)));
        }

        private void Right_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new(nameof(Right)));
        }

        private void Top_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new(nameof(Top)));
        }

        private void Bottom_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new(nameof(Bottom)));
        }
    }
}
