using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements mouse handling for the <see cref="InteriorDrawable"/>.
    /// </summary>
    public class InteriorNotification : ControlNotification
    {
        private InteriorDrawable interior;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteriorNotification"/> class.
        /// </summary>
        /// <param name="interior">Interior drawable.</param>
        public InteriorNotification(InteriorDrawable interior)
        {
            this.interior = interior;
        }

        /// <inheritdoc/>
        public override void AfterMouseMove(Control sender, MouseEventArgs e)
        {
            var hitTest = interior.HitTest(sender.ScaleFactor, e.Location);
            if (hitTest != InteriorDrawable.HitTestResult.None)
            {
                App.Log(hitTest);
            }
        }

        /// <inheritdoc/>
        public override void AfterSetScrollBarInfo(
            Control sender,
            bool isVertical,
            ScrollBarInfo value)
        {
        }

        /// <inheritdoc/>
        public override void AfterScroll(Control sender, ScrollEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterDpiChanged(Control sender, DpiChangedEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterClick(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterDragDrop(Control sender, DragEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterDragEnter(Control sender, DragEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterDragLeave(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterDragOver(Control sender, DragEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterDragStart(Control sender, DragStartEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterEnabledChanged(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterIsMouseOverChanged(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseCaptureLost(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseDown(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseEnter(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeave(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonDown(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonUp(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseRightButtonDown(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseRightButtonUp(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseUp(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseWheel(Control sender, MouseEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterPaint(Control sender, PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterQueryContinueDrag(Control sender, QueryContinueDragEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterResize(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterSizeChanged(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterSystemColorsChanged(Control sender)
        {
        }

        /// <inheritdoc/>
        public override void AfterTouch(Control sender, TouchEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterVisualStateChanged(Control sender)
        {
        }
    }
}
