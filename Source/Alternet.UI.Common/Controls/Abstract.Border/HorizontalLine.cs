using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a horizontal line control.
    /// </summary>
    public partial class HorizontalLine : GenericControl
    {
        /// <summary>
        /// Gets or sets default height of the horizontal line.
        /// </summary>
        public static Coord DefaultHeight = 1;

        private Coord lineHeight = DefaultHeight;

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
        public HorizontalLine(AbstractControl parent)
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
                return (result.Width, lineHeight + Padding.Vertical);
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
            var rect = ClientRectangle;
            var brush = DefaultColors.GetControlBorderBrush(this);
            e.Graphics.DrawHorzLine(brush, (rect.Left, rect.Top + Padding.Top), rect.Width, LineHeight);
        }
    }
}
