using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies <see cref="Border"/> drawing settings.
    /// </summary>
    public class BorderSettings : BaseObject, INotifyPropertyChanged
    {
        internal static readonly BorderSettings Default = new();

        private readonly BorderSideSettings left = new();
        private readonly BorderSideSettings top = new();
        private readonly BorderSideSettings right = new();
        private readonly BorderSideSettings bottom = new();

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
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

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
        /// Gets rectangle of the top border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public Rect GetTopRectangle(Rect rect)
        {
            var point = rect.TopLeft;
            var size = new Size(rect.Width, Top.Width);
            return new Rect(point, size);
        }

        /// <summary>
        /// Gets rectangle of the bottom border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public Rect GetBottomRectangle(Rect rect)
        {
            var point = new Point(rect.Left, rect.Bottom - Bottom.Width);
            var size = new Size(rect.Width, Bottom.Width);
            return new Rect(point, size);
        }

        /// <summary>
        /// Gets rectangle of the left border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public Rect GetLeftRectangle(Rect rect)
        {
            var point = rect.TopLeft;
            var size = new Size(Left.Width, rect.Height);
            return new Rect(point, size);
        }

        /// <summary>
        /// Gets rectangle of the right border edge.
        /// </summary>
        /// <param name="rect">Border rectangle.</param>
        public Rect GetRightRectangle(Rect rect)
        {
            var point = new Point(rect.Right - Right.Width, rect.Top);
            var size = new Size(Right.Width, rect.Height);
            return new Rect(point, size);
        }

        /// <summary>
        /// Draws border in the specified rectangle of the drawing context.
        /// </summary>
        /// <param name="dc">Drawing context.</param>
        /// <param name="rect">Rectangle.</param>
        public void Draw(DrawingContext dc, Rect rect)
        {
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
