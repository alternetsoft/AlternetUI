using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements interior scroll handling for controls.
    /// </summary>
    public class InteriorScrollActivity : BaseControlActivity
    {
        private AbstractControl? control;
        private bool subscribedClickRepeated;
        private bool isDragging = false;
        private int hitTestsMouseDown = -1;
        private PointD mouseDownLocation;
        private PointD mouseMoveLocation;
        private bool raiseOnMouseMove;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteriorScrollActivity"/> class.
        /// </summary>
        public InteriorScrollActivity()
        {
        }

        /// <summary>
        /// Specifies the available methods for performing scroll actions in response to user input.
        /// </summary>
        /// <remarks>Use this enumeration to select how scrolling behavior is triggered in controls that
        /// support user-driven scrolling. The selected method determines whether scrolling occurs automatically or only
        /// in response to specific user actions.</remarks>
        public enum ScrollMethodKind
        {
            /// <summary>
            /// No scrolling is performed.
            /// </summary>
            None,

            /// <summary>
            /// Scrolling is performed repeatedly while left mouse button or finger is pressed.
            /// Direction depends on the position of the pointer from the initial touch point.
            /// </summary>
            RepeatWhilePressed,

            /// <summary>
            /// Scrolling is performed using the difference between the most recent pointer-down position
            /// (mouse click or touch) and the current pointer position.
            /// </summary>
            DeltaWhilePressed,
        }

        /// <summary>
        /// Defines a delegate for hit testing an element to determine which part was clicked.
        /// </summary>
        /// <param name="sender">The control that is being hit tested.</param>
        /// <param name="clickLocation">The location of the click.</param>
        /// <returns></returns>
        public delegate int HitTestDelegate(AbstractControl sender, PointD clickLocation);

        /// <summary>
        /// Gets or sets the hit test delegate to determine which part of the element was hit.
        /// </summary>
        public virtual HitTestDelegate? HitTest { get; set; }

        /// <summary>
        /// Occurs when a scroll action is performed.
        /// </summary>
        public event EventHandler<ScrollEventArgs>? Scroll;

        /// <summary>
        /// Occurs when an delta scroll action is performed.
        /// </summary>
        public event EventHandler? DeltaScroll;

        /// <summary>
        /// Gets or sets the default scroll method for all instances of <see cref="InteriorScrollActivity"/>.
        /// </summary>
        public static ScrollMethodKind DefaultScrollMethod { get; set; } = ScrollMethodKind.DeltaWhilePressed;

        /// <summary>
        /// Gets or sets the default minimum gesture distance for all instances of <see cref="InteriorScrollActivity"/>.
        /// If not set, a system-defined default value is used.
        /// </summary>
        public static float? DefaultMinGestureDistance { get; set; }

        /// <summary>
        /// Gets or sets the minimum gesture distance for the current instance of <see cref="InteriorScrollActivity"/>.
        /// If not set, <see cref="DefaultMinGestureDistance"/> is used.
        /// </summary>
        public virtual float? MinGestureDistance { get; set; }

        /// <summary>
        /// Gets or sets whether to send scroll events to the attached control. Default is <c>true</c>.
        /// </summary>
        public virtual bool SendScrollToControl { get; set; } = true;

        /// <summary>
        /// Gets or sets the scroll method for the current instance of <see cref="InteriorScrollActivity"/>.
        /// Default is null. If not specified, <see cref="DefaultScrollMethod"/> is used.
        /// </summary>
        public virtual ScrollMethodKind? ScrollMethod { get; set; }

        /// <summary>
        /// Gets the location of the last mouse down event.
        /// </summary>
        public virtual PointD LastMouseDownLocation
        {
            get
            {
                return mouseDownLocation;
            }

            set
            {
                mouseDownLocation = value;
            }
        }

        /// <summary>
        /// Gets the location of the last mouse move event.
        /// </summary>
        public virtual PointD LastMouseMoveLocation
        {
            get
            {
                return mouseMoveLocation;
            }

            set
            {
                mouseMoveLocation = value;
            }
        }

        /// <inheritdoc/>
        public override void BeforeMouseMove(AbstractControl sender, MouseEventArgs e)
        {
            mouseMoveLocation = e.Location;

            if (isDragging && hitTestsMouseDown >= 0)
            {
                switch (RealScrollMethod)
                {
                    case ScrollMethodKind.DeltaWhilePressed:
                        var distance = DrawingUtils.GetDistance(mouseDownLocation, e.Location);
                        if (distance < GetMinGestureDistance(sender))
                            return;
                        RaiseDeltaScroll(sender);
                        mouseDownLocation = e.Location;
                        break;
                    case ScrollMethodKind.RepeatWhilePressed:
                        if (raiseOnMouseMove)
                        {
                            RaiseScroll(sender, e.Location);
                            raiseOnMouseMove = false;
                        }

                        break;
                }
            }
            else
            {
            }
        }

        /// <inheritdoc/>
        public override void AfterMouseLeave(AbstractControl sender, EventArgs e)
        {
            ResetDragging(sender);
            UnsubscribeClickRepeated();
        }

        /// <summary>
        /// Resets the dragging state to its default value.
        /// </summary>
        public void ResetDragging(AbstractControl sender)
        {
            isDragging = false;
            hitTestsMouseDown = -1;
        }

        /// <inheritdoc/>
        public override void AfterVisualStateChanged(AbstractControl sender, EventArgs e)
        {
            var visualState = sender.VisualState;

            if (visualState != VisualControlState.Pressed)
            {
                UnsubscribeClickRepeated();
            }
        }

        /// <inheritdoc/>
        public override void AfterVisibleChanged(AbstractControl sender, EventArgs e)
        {
            ResetDragging(sender);
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterLostFocus(AbstractControl sender, LostFocusEventArgs e)
        {
            ResetDragging(sender);
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void BeforeMouseDown(AbstractControl sender, MouseEventArgs e)
        {
            ResetDragging(sender);
            mouseDownLocation = e.Location;

            if (e.Button != MouseButtons.Left)
                return;
            if (RealScrollMethod == ScrollMethodKind.None)
                return;

            hitTestsMouseDown = HitTest?.Invoke(sender, e.Location) ?? -1;

            if (hitTestsMouseDown >= 0)
            {
                raiseOnMouseMove = true;
                isDragging = true;

                switch (RealScrollMethod)
                {
                    case ScrollMethodKind.DeltaWhilePressed:
                        break;
                    case ScrollMethodKind.RepeatWhilePressed:
                        SubscribeClickRepeated(sender);
                        break;
                }
            }
        }

        /// <inheritdoc/>
        public override void BeforeMouseUp(AbstractControl sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (isDragging)
            {
                ResetDragging(sender);
            }

            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnsubscribeClickRepeated();
            base.DisposeManaged();
        }

        /// <summary>
        /// Gets the minimum distance required to recognize a gesture.
        /// </summary>
        /// <param name="sender">The control that is sending the gesture.</param>
        /// <returns></returns>
        public virtual float GetMinGestureDistance(AbstractControl sender)
        {
            return MinGestureDistance ?? DefaultMinGestureDistance ?? DragStartEventArgs.MinDragStartDistance;
        }

        /// <summary>
        /// Raises a delta scroll event for the specified control.
        /// </summary>
        /// <param name="sender">The control that is the source of the delta scroll event.</param>
        public virtual void RaiseDeltaScroll(AbstractControl sender)
        {
            DeltaScroll?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises a scroll event for the specified control with the given orientation, event type, and value changes.
        /// </summary>
        /// <remarks>If the event is not marked as handled and the SendScrollToControl property is set to
        /// true, the scroll event is propagated to the sender control. This method is typically used to notify controls
        /// of scroll actions and to allow for custom handling or propagation of scroll events.</remarks>
        /// <param name="sender">The control that is the source of the scroll event.</param>
        /// <param name="orientation">The orientation of the scroll bar associated with the event.</param>
        /// <param name="evType">The type of scroll event that occurred.</param>
        /// <param name="oldValue">The previous value of the scroll position. Defaults to 0.</param>
        /// <param name="newValue">The new value of the scroll position. Defaults to 0.</param>
        public virtual void RaiseScroll(
            AbstractControl sender,
            ScrollBarOrientation orientation,
            ScrollEventType evType,
            int oldValue = 0,
            int newValue = 0)
        {
            if (RealScrollMethod != ScrollMethodKind.RepeatWhilePressed)
                return;

            ScrollEventArgs scrollArgs = new();

            scrollArgs.ScrollOrientation = orientation;
            scrollArgs.Type = evType;
            scrollArgs.NewValue = newValue;
            scrollArgs.OldValue = oldValue;

            Scroll?.Invoke(sender, scrollArgs);

            if (scrollArgs.Handled)
                return;

            if (SendScrollToControl)
                sender.RaiseScroll(scrollArgs);
        }

        /// <summary>
        /// Raises a scroll event based on the movement from the initial mouse down location to the specified touch
        /// location or the current mouse position.
        /// </summary>
        /// <remarks>This method determines the scroll direction and magnitude based on the distance and
        /// direction between the initial mouse down location and the current pointer or touch location. If the movement
        /// does not exceed the minimum scroll distance, no scroll event is raised.</remarks>
        /// <param name="sender">The control that is the source of the scroll event.</param>
        /// <param name="touchLocation">The location of the touch or pointer, in device-independent coordinates.
        /// If null, the current mouse position is used.</param>
        public virtual void RaiseScroll(AbstractControl sender, PointD? touchLocation)
        {
            if (RealScrollMethod != ScrollMethodKind.RepeatWhilePressed)
                return;

            var newLocation = touchLocation ?? Mouse.GetPosition(sender);

            var distance = DrawingUtils.GetDistance(mouseDownLocation, newLocation);
            if (distance < GetMinGestureDistance(sender))
                return;

            var swipeDirection = SwipeDirectionHelper.GetSwipeDirection(mouseDownLocation, newLocation);

            if (swipeDirection.HasFlag(SwipeDirection.Up))
            {
                RaiseScroll(sender, ScrollBarOrientation.Vertical, ScrollEventType.SmallIncrement);
            }
            else
            if (swipeDirection.HasFlag(SwipeDirection.Down))
            {
                RaiseScroll(sender, ScrollBarOrientation.Vertical, ScrollEventType.SmallDecrement);
            }

            if (swipeDirection.HasFlag(SwipeDirection.Left))
            {
                RaiseScroll(sender, ScrollBarOrientation.Horizontal, ScrollEventType.SmallIncrement);
            }
            else
            if (swipeDirection.HasFlag(SwipeDirection.Right))
            {
                RaiseScroll(sender, ScrollBarOrientation.Horizontal, ScrollEventType.SmallDecrement);
            }
        }

        /// <summary>
        /// Gets the effective scroll method for the current instance.
        /// </summary>
        protected virtual ScrollMethodKind RealScrollMethod
        {
            get
            {
                return ScrollMethod ?? DefaultScrollMethod;
            }
        }

        /// <summary>
        /// Handles the timer event for repeating click actions, typically used to trigger repeated scrolling or similar
        /// behavior when a control is held down.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes to customize the behavior
        /// of repeated click events. It is commonly used in scenarios where holding down a control should result in
        /// repeated actions, such as scrolling or incrementing values.</remarks>
        /// <param name="sender">The source of the event, usually the timer or control initiating the repeat action.</param>
        /// <param name="e">An object that contains the event data.</param>
        protected virtual void OnClickRepeatTimerEvent(object? sender, EventArgs e)
        {
            if (control is null)
                return;

            if (TimerUtils.LastClickLessThanRepeatInterval(control))
                return;
            RaiseScroll(control, null);
        }

        /// <summary>
        /// Unsubscribes the current instance from the repeated click timer event.
        /// </summary>
        /// <remarks>Call this method to detach the event handler for repeated click events and release
        /// associated resources. This method is intended to be overridden in derived classes to customize
        /// unsubscription behavior.</remarks>
        protected virtual void UnsubscribeClickRepeated()
        {
            this.control = null;
            if (subscribedClickRepeated)
            {
                TimerUtils.ClickRepeated -= OnClickRepeatTimerEvent;
                subscribedClickRepeated = false;
            }
        }

        /// <summary>
        /// Subscribes the specified control to receive repeated click events if not already subscribed.
        /// </summary>
        /// <param name="control">The control to associate with repeated click event handling. Cannot be null.</param>
        protected virtual void SubscribeClickRepeated(AbstractControl control)
        {
            if (!subscribedClickRepeated)
            {
                this.control = control;
                TimerUtils.ClickRepeated += OnClickRepeatTimerEvent;
                subscribedClickRepeated = true;
            }
        }
    }
}
