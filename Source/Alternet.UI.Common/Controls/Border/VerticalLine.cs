using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a vertical line control.
    /// </summary>
    public partial class VerticalLine : GraphicControl
    {
        private Coord lineWidth = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalLine"/> class.
        /// </summary>
        public VerticalLine()
        {
            ParentBackColor = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalLine"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public VerticalLine(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <inheritdoc/>
        public override SizeD SuggestedSize
        {
            get
            {
                var result = base.SuggestedSize;
                return (lineWidth, result.Height);
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the width of the vertical line.
        /// </summary>
        public virtual Coord LineWidth
        {
            get => lineWidth;

            set
            {
                if (lineWidth == value)
                    return;
                lineWidth = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var rect = e.ClientRectangle;
            var brush = DefaultColors.GetControlBorderBrush(this);
            e.Graphics.DrawVertLine(brush, rect.TopLeft, rect.Height, LineWidth);
        }
    }
}
