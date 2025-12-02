using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements swipe gesture handling for controls.
    /// </summary>
    public class SwipeGestureActivity : BaseControlActivity
    {
        private bool isDragging = false;
        private PointD mouseDownLocation;
        private int hitTestMouseDown = -1;
        private bool isEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwipeGestureActivity"/> class.
        /// </summary>
        public SwipeGestureActivity()
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
        /// Gets or sets the default minimum swipe distance for all instances of <see cref="SwipeGestureActivity"/>.
        /// If not set, a system-defined default value is used.
        /// </summary>
        public static float? DefaultMinSwipeDistance { get; set; }

        /// <summary>
        /// Gets or sets the hit test delegate to determine which part of the element was hit.
        /// </summary>
        public HitTestDelegate? HitTest { get; set; }

        /// <summary>
        /// Gets the hit test result for the most recent mouse down event.
        /// </summary>
        public int HitTestMouseDown => hitTestMouseDown;

        /// <summary>
        /// Occurs when a swipe gesture is detected.
        /// </summary>
        public event EventHandler<SwipeGestureEventArgs>? SwipeDetected;

        /// <summary>
        /// Gets a value indicating whether a drag operation is currently in progress for this activity.
        /// </summary>
        /// <value><c>true</c> when dragging is active; otherwise, <c>false</c>.</value>
        public bool IsDragging => isDragging;

        /// <summary>
        /// Gets or sets the minimum swipe distance required to recognize a swipe gesture.
        /// </summary>
        /// <remarks>Set this property to define the threshold distance, in device-independent units, that
        /// a user's gesture must exceed to be considered a swipe. If set to <see langword="null"/>,
        /// the <see cref="DefaultMinSwipeDistance"/> is used.</remarks>
        public float? MinSwipeDistance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
        }

        /// <summary>
        /// Resets the dragging state to its default value.
        /// </summary>
        public virtual void ResetDragging(AbstractControl sender)
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
        }

        /// <inheritdoc/>
        public override void BeforeMouseDown(AbstractControl sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            ResetDragging(sender);

            if (e.Button != MouseButtons.Left)
                return;

            var hitTest = GetHitTest(sender, e.Location);
            mouseDownLocation = e.Location;
            hitTestMouseDown = hitTest;

            if (hitTest == -1)
            {
            }
            else
            {
                isDragging = true;
            }
        }

        /// <summary>
        /// Gets the minimum distance required to recognize a swipe gesture.
        /// </summary>
        /// <param name="sender">The control that is sending the swipe gesture.</param>
        /// <returns></returns>
        public virtual float GetMinSwipeDistance(AbstractControl sender)
        {
            return MinSwipeDistance ?? DefaultMinSwipeDistance ?? DragStartEventArgs.MinDragStartDistance;
        }

        /// <inheritdoc/>
        public override void BeforeMouseUp(AbstractControl sender, MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (e.Button != MouseButtons.Left)
                return;

            if (isDragging)
            {
                ResetDragging(sender);

                if (SwipeDetected != null)
                {
                    var distance = DrawingUtils.GetDistance(mouseDownLocation, e.Location);

                    if (distance < GetMinSwipeDistance(sender))
                        return;

                    var args = new SwipeGestureEventArgs(e, mouseDownLocation, e.Location);

                    SwipeDetected(this, args);
                }
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
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
        /// Enables or disables the activity.
        /// </summary>
        /// <param name="sender">The control for which this activity is being enabled or disabled.</param>
        /// <param name="value">The new value indicating whether the activity is enabled.</param>
        private void SetEnabled(AbstractControl sender, bool value)
        {
            if (isEnabled == value)
                return;
            isEnabled = value;
            ResetDragging(sender);
        }

        /// <summary>
        /// Provides data for the SwipeGesture event.
        /// </summary>
        public class SwipeGestureEventArgs : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the SwipeGestureEventArgs class with the specified mouse event data and
            /// swipe coordinates.
            /// </summary>
            /// <remarks>The swipe direction and angle are calculated based on the provided initial
            /// and end points. This constructor is typically used when handling swipe gestures in custom input
            /// scenarios.</remarks>
            /// <param name="mouseArgs">The mouse event data associated with the swipe gesture. Cannot be null.</param>
            /// <param name="initialPoint">The starting point of the swipe gesture, in device-independent coordinates.</param>
            /// <param name="endPoint">The ending point of the swipe gesture, in device-independent coordinates.</param>
            public SwipeGestureEventArgs(MouseEventArgs mouseArgs, PointD initialPoint, PointD endPoint)
            {
                MouseArgs = mouseArgs;
                Angle = SwipeDirectionHelper.GetAngleFromPoints(initialPoint.X, initialPoint.Y, endPoint.X, endPoint.Y);
                Direction = SwipeDirectionHelper.GetSwipeDirectionFromAngle(Angle);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SwipeGestureEventArgs"/> class.
            /// </summary>
            /// <param name="mouseArgs">The mouse event arguments.</param>
            /// <param name="direction">The direction of the swipe gesture.</param>
            /// <param name="angle">The angle of the swipe gesture.</param>
            public SwipeGestureEventArgs(MouseEventArgs mouseArgs, SwipeDirection direction, float angle)
            {
                MouseArgs = mouseArgs;
                Direction = direction;
                Angle = angle;
            }

            /// <summary>
            /// Gets or sets the mouse event arguments.
            /// </summary>
            public MouseEventArgs MouseArgs { get; set; }

            /// <summary>
            /// Gets or sets the angle, in degrees, represented by this instance.
            /// </summary>
            public float Angle { get; set; }

            /// <summary>
            /// Gets or sets the direction of the swipe gesture.
            /// </summary>
            public SwipeDirection Direction { get; set; }
        }
    }
}
