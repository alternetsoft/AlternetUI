using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a horizontal line control.
    /// </summary>
    public partial class HorizontalLine : GraphicControl
    {
        private Coord lineHeight = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalLine"/> class.
        /// </summary>
        public HorizontalLine()
        {
            ParentBackColor = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalLine"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public HorizontalLine(Control parent)
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
                return (result.Width, lineHeight);
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the height of the horizontal line.
        /// </summary>
        public virtual Coord LineHeight
        {
            get => lineHeight;

            set
            {
                if (lineHeight == value)
                    return;
                lineHeight = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            var rect = e.ClipRectangle;
            var brush = DefaultColors.GetBorderBrush(this);
            e.Graphics.DrawHorzLine(brush, rect.TopLeft, rect.Width, LineHeight);
        }
    }
}
