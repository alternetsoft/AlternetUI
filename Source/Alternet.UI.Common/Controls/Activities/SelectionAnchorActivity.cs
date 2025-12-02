using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements selection anchors handling for the controls with selection capabilities.
    /// </summary>
    public class SelectionAnchorActivity : BaseControlActivity
    {
        private bool isDragging = false;
        private PointD clickOffset;
        private int hitTestMouseDown = -1;
        private int hitTestMouseMove = -1;
        private PointD mouseDownLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionAnchorActivity"/> class.
        /// </summary>
        public SelectionAnchorActivity()
        {
        }

        /// <summary>
        /// Defines a delegate for hit testing an element to determine which part was clicked.
        /// </summary>
        /// <param name="sender">The control that is being hit tested.</param>
        /// <param name="clickLocation">The location of the click.</param>
        /// <returns></returns>
        public delegate int HitTestDelegate(AbstractControl sender, PointD clickLocation);

        /// <summary>
        /// Defines a delegate for obtaining the rectangle of a specific part of an element.
        /// </summary>
        /// <param name="sender">The control that is being hit tested.</param>
        /// <param name="hitTest">The result of the hit test.</param>
        /// <returns></returns>
        public delegate RectD? HitTestRectangleDelegate(AbstractControl sender, int hitTest);

        /// <summary>
        /// Gets or sets the hit test delegate to determine which part of the element was hit.
        /// </summary>
        public HitTestDelegate? HitTest { get; set; }

        /// <summary>
        /// Occurs when an anchor is being moved during a drag operation.
        /// </summary>
        public event EventHandler<AnchorMoveEventArgs>? AnchorMove;

        /// <summary>
        /// Gets or sets the delegate used to obtain the rectangle that corresponds to a specified hit test result.
        /// </summary>
        /// <remarks>
        /// If this property is <c>null</c>, the activity will not be able to resolve the hit-test rectangle for a given
        /// hit test and related operations that depend on the rectangle will be skipped.
        /// </remarks>
        public HitTestRectangleDelegate? HitTestRectangle { get; set; }

        /// <summary>
        /// Gets a value indicating whether a drag operation is currently in progress for this activity.
        /// </summary>
        /// <value><c>true</c> when dragging is active; otherwise, <c>false</c>.</value>
        public bool IsDragging => isDragging;

        /// <summary>
        /// Gets the offset between the mouse-down location and the origin of the hit-test rectangle when dragging began.
        /// </summary>
        /// <remarks>
        /// This offset is computed on mouse down and is used to preserve the relative cursor position during a drag.
        /// </remarks>
        public PointD ClickOffset => clickOffset;

        /// <summary>
        /// Gets the hit test result that was recorded when the last mouse-down event occurred.
        /// </summary>
        /// <value>
        /// The recorded hit test result from the mouse-down event, or <c>null</c> if no hit was recorded.
        /// </value>
        public int HitTestMouseDown => hitTestMouseDown;

        /// <summary>
        /// Gets the hit test result for the most recent mouse move event.
        /// </summary>
        public int HitTestMouseMove => hitTestMouseMove;

        /// <inheritdoc/>
        public override void AfterMouseCaptureLost(AbstractControl sender, EventArgs e)
        {
            ResetDragging(sender);
            sender.ReleaseMouseCapture();
        }

        /// <inheritdoc/>
        public override void BeforeMouseMove(AbstractControl sender, MouseEventArgs e)
        {
            if (isDragging && hitTestMouseDown != -1)
            {
                if (AnchorMove is not null)
                {
                    AnchorMoveEventArgs args = new AnchorMoveEventArgs(e, hitTestMouseDown);
                    AnchorMove(sender, args);
                }

                e.Handled = true;
            }
            else
            {
            }
        }

        /// <inheritdoc/>
        public override void AfterMouseLeave(AbstractControl sender, EventArgs e)
        {
        }

        /// <summary>
        /// Resets the dragging state to its default value.
        /// </summary>
        public void ResetDragging(AbstractControl sender)
        {
            isDragging = false;
            hitTestMouseDown = -1;
        }

        /// <inheritdoc/>
        public override void AfterVisibleChanged(AbstractControl sender, EventArgs e)
        {
            ResetDragging(sender);
        }

        /// <inheritdoc/>
        public override void AfterLostFocus(AbstractControl sender, LostFocusEventArgs e)
        {
            ResetDragging(sender);
            sender.ReleaseMouseCapture();
        }

        /// <inheritdoc/>
        public override void BeforeMouseDown(AbstractControl sender, MouseEventArgs e)
        {
            ResetDragging(sender);

            if (e.Button != MouseButtons.Left)
                return;

            var hitTest = GetHitTest(sender, e.Location);
            hitTestMouseDown = hitTest;
            mouseDownLocation = e.Location;

            if (hitTest == -1)
            {
            }
            else
            {
                var rect = GetHitTestRect(sender, hitTest);

                if(rect is not null)
                {
                    isDragging = true;

                    clickOffset = e.Location - rect.Value.Location;

                    sender.CaptureMouse();

                    e.Handled = true;
                }
            }
        }

        /// <inheritdoc/>
        public override void BeforeMouseUp(AbstractControl sender, MouseEventArgs e)
        {
            sender.ReleaseMouseCapture();
            if (e.Button != MouseButtons.Left)
                return;

            if (isDragging)
            {
                ResetDragging(sender);
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        /// <summary>
        /// Returns the hit test rectangle for the specified control and hit test result.
        /// </summary>
        /// <param name="sender">The control for which the hit test rectangle is being requested.</param>
        /// <param name="hitTest">The hit test result that determines the area to be returned.</param>
        /// <returns>A <see cref="RectD"/> representing the hit test area. Returns null if no hit test
        /// rectangle is available.</returns>
        protected virtual RectD? GetHitTestRect(AbstractControl sender, int hitTest)
        {
            if (HitTestRectangle is null)
                return null;

            return HitTestRectangle(sender, hitTest);
        }

        /// <summary>
        /// Performs a hit test on the specified control at the given click location.
        /// </summary>
        /// <param name="sender">The control to hit test.</param>
        /// <param name="clickLocation">The location of the click.</param>
        /// <returns></returns>
        protected virtual int GetHitTest(AbstractControl sender, PointD clickLocation)
        {
            if (HitTest is null)
                return -1;
            return HitTest(sender, clickLocation);
        }

        /// <summary>
        /// Provides data for the AnchorMove event.
        /// </summary>
        public class AnchorMoveEventArgs : EventArgs
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="AnchorMoveEventArgs"/> class.
            /// </summary>
            /// <param name="mouseArgs">The mouse event arguments.</param>
            /// <param name="hitTest">The hit test result.</param>
            public AnchorMoveEventArgs(MouseEventArgs mouseArgs, int hitTest)
            {
                MouseArgs = mouseArgs;
                HitTest = hitTest;
            }

            /// <summary>
            /// Gets the mouse event arguments.
            /// </summary>
            public MouseEventArgs MouseArgs { get; }

            /// <summary>
            /// Gets the hit test result.
            /// </summary>
            public int HitTest { get; }
        }
    }
}
