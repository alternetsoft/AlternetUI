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
    internal class BorderSettings

    {
        private static readonly Color DefaultColor = Color.Gray;
        private Pen? leftPen;
        private Pen? topPen;
        private Pen? rightPen;
        private Pen? bottomPen;
        private Brush? leftBrush;
        private Brush? topBrush;
        private Brush? rightBrush;
        private Brush? bottomBrush;
        private Thickness width = new(1);
        private Color leftColor = DefaultColor;
        private Color topColor = DefaultColor;
        private Color bottomColor = DefaultColor;
        private Color rightColor = DefaultColor;

        /// <summary>
        /// Gets whether all sides have same color and width.
        /// </summary>
        [Browsable(false)]
        public bool IsUniform => IsUniformColor && width.IsUniform;

        /// <summary>
        /// Gets or sets uniform color of the border lines.
        /// </summary>
        [Browsable(false)]
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
                SetColors(value, value, value, value);
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
                    leftPen = null;
                if (width.Right != value.Right)
                    rightPen = null;
                if (width.Top != value.Top)
                    topPen = null;
                if (width.Bottom != value.Bottom)
                    bottomPen = null;
                width = value;
            }
        }

        public Pen LeftPen
        {
            get
            {
                if (leftPen == null)
                    leftPen = BorderSettings.CreatePen(leftColor, width.Left);
                return leftPen;
            }
        }

        public Brush LeftBrush
        {
            get
            {
                if (leftBrush == null)
                    leftBrush = new SolidBrush(leftColor);
                return leftBrush;
            }
        }

        public Pen TopPen
        {
            get
            {
                if (topPen == null)
                    topPen = BorderSettings.CreatePen(topColor, width.Top);
                return topPen;
            }
        }

        public Brush TopBrush
        {
            get
            {
                if (topBrush == null)
                    topBrush = new SolidBrush(topColor);
                return topBrush;
            }
        }

        public Pen RightPen
        {
            get
            {
                if (rightPen == null)
                    rightPen = BorderSettings.CreatePen(rightColor, width.Right);
                return rightPen;
            }
        }

        public Brush RightBrush
        {
            get
            {
                if (rightBrush == null)
                    rightBrush = new SolidBrush(rightColor);
                return rightBrush;
            }
        }

        public Pen BottomPen
        {
            get
            {
                if (bottomPen == null)
                    bottomPen = BorderSettings.CreatePen(bottomColor, width.Bottom);
                return bottomPen;
            }
        }

        public Brush BottomBrush
        {
            get
            {
                if (bottomBrush == null)
                    bottomBrush = new SolidBrush(bottomColor);
                return bottomBrush;
            }
        }

        public Color LeftColor
        {
            get => leftColor;
        }

        public Color TopColor
        {
            get => topColor;
        }

        public Color BottomColor
        {
            get => bottomColor;
        }

        public Color RightColor
        {
            get => rightColor;
        }

        private bool IsUniformVerticalColor => leftColor == rightColor;

        private bool IsUniformHorizontalColor => topColor == bottomColor;

        private bool IsUniformColor =>
            IsUniformVerticalColor && IsUniformHorizontalColor;

        public bool SetColors(Color left, Color top, Color right, Color bottom)
        {
            var result = false;

            if (leftColor != left)
            {
                leftPen = null;
                leftBrush = null;
                leftColor = left;
                result = true;
            }

            if (rightColor != right)
            {
                rightPen = null;
                rightBrush = null;
                rightColor = right;
                result = true;
            }

            if (topColor != top)
            {
                topPen = null;
                topBrush = null;
                topColor = top;
                result = true;
            }

            if (bottomColor != bottom)
            {
                bottomPen = null;
                bottomBrush = null;
                bottomColor = bottom;
                result = true;
            }

            return result;
        }

        public BorderSettings Clone()
        {
            BorderSettings result = new()
            {
                width = width,
                leftColor = leftColor,
                topColor = topColor,
                bottomColor = bottomColor,
                rightColor = rightColor,
            };
            return result;
        }

        private static Pen CreatePen(Color color, double width)
        {
            return new Pen(color, Math.Max(1, width));
        }
    }
}
