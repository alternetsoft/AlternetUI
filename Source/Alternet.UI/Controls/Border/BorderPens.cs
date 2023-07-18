using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class BorderPens
    {
        private static readonly Color DefaultColor = Color.Gray;
        private Pen? left;
        private Pen? top;
        private Pen? right;
        private Pen? bottom;
        private Thickness width = new(1);
        private Color leftColor = DefaultColor;
        private Color topColor = DefaultColor;
        private Color bottomColor = DefaultColor;
        private Color rightColor = DefaultColor;

        public bool IsUniform => IsUniformColor && width.IsUniform;

        public Color Color
        {
            get
            {
                if (IsUniformColor)
                    return topColor;
                return Color.Empty;
            }

            set
            {
                if (leftColor != value)
                    left = null;
                if (rightColor != value)
                    right = null;
                if (topColor != value)
                    top = null;
                if (bottomColor != value)
                    bottom = null;
                leftColor = value;
                topColor = value;
                bottomColor = value;
                rightColor = value;
            }
        }

        public Thickness Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width == value)
                    return;
                if (width.Left != value.Left)
                    left = null;
                if (width.Right != value.Right)
                    right = null;
                if (width.Top != value.Top)
                    top = null;
                if (width.Bottom != value.Bottom)
                    bottom = null;
                width = value;
            }
        }

        public Pen Left
        {
            get
            {
                if (left == null)
                    left = CreatePen(leftColor, width.Left);
                return left;
            }
        }

        public Pen Top
        {
            get
            {
                if (top == null)
                    top = CreatePen(topColor, width.Top);
                return top;
            }
        }

        public Pen Right
        {
            get
            {
                if (right == null)
                    right = CreatePen(rightColor, width.Right);
                return right;
            }
        }

        public Pen Bottom
        {
            get
            {
                if (bottom == null)
                    bottom = CreatePen(bottomColor, width.Bottom);
                return bottom;
            }
        }

        private bool IsUniformVerticalColor => leftColor == rightColor;

        private bool IsUniformHorizontalColor => topColor == bottomColor;

        private bool IsUniformColor =>
            IsUniformVerticalColor && IsUniformHorizontalColor;

        public BorderPens Clone()
        {
            BorderPens result = new();

            result.width = width;
            result.leftColor = leftColor;
            result.topColor = topColor;
            result.bottomColor = bottomColor;
            result.rightColor = rightColor;
            return result;
        }

        private Pen CreatePen(Color color, double width)
        {
            return new Pen(color, Math.Max(1, width));
        }
    }
}
