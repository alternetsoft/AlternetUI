using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Scroll bar primitive painter.
    /// </summary>
    public class ScrollBarPrimitivePainter : PrimitivePainter
    {
        /// <summary>
        /// Gets or sets primitive painter for the background.
        /// </summary>
        public BorderPrimitivePainter? BackgroundPainter;

        /// <summary>
        /// Gets or sets primitive painter for the scroll thumb.
        /// </summary>
        public BorderPrimitivePainter? ThumbPainter;

        /// <summary>
        /// Gets or sets primitive painter for the scroll up button.
        /// </summary>
        public BorderPrimitivePainter? UpButtonPainter;

        /// <summary>
        /// Gets or sets primitive painter for the scroll down button.
        /// </summary>
        public BorderPrimitivePainter? DownButtonPainter;

        /// <summary>
        /// Gets or sets primitive painter for the scroll left button.
        /// </summary>
        public BorderPrimitivePainter? LeftButtonPainter;

        /// <summary>
        /// Gets or sets primitive painter for the scroll right button.
        /// </summary>
        public BorderPrimitivePainter? RightButtonPainter;

        /// <summary>
        /// Gets or sets primitive painter for the up arrow.
        /// </summary>
        public BorderPrimitivePainter? UpArrowPainter;

        /// <summary>
        /// Gets or sets primitive painter for the down arrow.
        /// </summary>
        public BorderPrimitivePainter? DownArrowPainter;

        /// <summary>
        /// Gets or sets primitive painter for the left arrow.
        /// </summary>
        public BorderPrimitivePainter? LeftArrowPainter;

        /// <summary>
        /// Gets or sets primitive painter for the right arrow.
        /// </summary>
        public BorderPrimitivePainter? RightArrowPainter;

        /// <summary>
        /// Gets of sets whether scroll bar is vertical.
        /// </summary>
        public bool IsVertical = true;

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            BackgroundPainter?.Draw(control, dc);

            var startButton = GetStartButton();
            var endButton = GetEndButton();
            var startArrow = GetStartArrow();
            var endArrow = GetEndArrow();

            startButton?.Draw(control, dc);
            startArrow?.Draw(control, dc);
            endButton?.Draw(control, dc);
            endArrow?.Draw(control, dc);
        }

        private BorderPrimitivePainter? GetEndArrow()
        {
            if (IsVertical)
                return DownArrowPainter;
            return RightArrowPainter;
        }

        private BorderPrimitivePainter? GetStartArrow()
        {
            if (IsVertical)
                return UpArrowPainter;
            return LeftArrowPainter;
        }

        private BorderPrimitivePainter? GetStartButton()
        {
            if (IsVertical)
                return UpButtonPainter;
            return LeftButtonPainter;
        }

        private BorderPrimitivePainter? GetEndButton()
        {
            if (IsVertical)
                return DownButtonPainter;
            return RightButtonPainter;
        }
    }
}
