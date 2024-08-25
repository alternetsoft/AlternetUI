using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements scroll bar drawing.
    /// </summary>
    public class ScrollBarDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets background element.
        /// </summary>
        public RectangleDrawable? Background;

        /// <summary>
        /// Gets or sets scroll thumb element.
        /// </summary>
        public RectangleDrawable? Thumb;

        /// <summary>
        /// Gets or sets up button element.
        /// </summary>
        public RectangleDrawable? UpButton;

        /// <summary>
        /// Gets or sets down button element.
        /// </summary>
        public RectangleDrawable? DownButton;

        /// <summary>
        /// Gets or sets left button element.
        /// </summary>
        public RectangleDrawable? LeftButton;

        /// <summary>
        /// Gets or sets right button element.
        /// </summary>
        public RectangleDrawable? RightButton;

        /// <summary>
        /// Gets or sets up arrow element.
        /// </summary>
        public RectangleDrawable? UpArrow;

        /// <summary>
        /// Gets or sets down arrow element.
        /// </summary>
        public RectangleDrawable? DownArrowPainter;

        /// <summary>
        /// Gets or sets primitive painter for the left arrow.
        /// </summary>
        public RectangleDrawable? LeftArrowPainter;

        /// <summary>
        /// Gets or sets primitive painter for the right arrow.
        /// </summary>
        public RectangleDrawable? RightArrowPainter;

        /// <summary>
        /// Gets of sets whether scroll bar is vertical.
        /// </summary>
        public bool IsVertical = true;

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            Background?.Draw(control, dc);

            var startButton = GetStartButton();
            var endButton = GetEndButton();
            var startArrow = GetStartArrow();
            var endArrow = GetEndArrow();

            startButton?.Draw(control, dc);
            startArrow?.Draw(control, dc);
            endButton?.Draw(control, dc);
            endArrow?.Draw(control, dc);
        }

        private RectangleDrawable? GetEndArrow()
        {
            if (IsVertical)
                return DownArrowPainter;
            return RightArrowPainter;
        }

        private RectangleDrawable? GetStartArrow()
        {
            if (IsVertical)
                return UpArrow;
            return LeftArrowPainter;
        }

        private RectangleDrawable? GetStartButton()
        {
            if (IsVertical)
                return UpButton;
            return LeftButton;
        }

        private RectangleDrawable? GetEndButton()
        {
            if (IsVertical)
                return DownButton;
            return RightButton;
        }
    }
}
