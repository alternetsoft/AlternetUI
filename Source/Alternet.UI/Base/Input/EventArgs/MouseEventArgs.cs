using System;
using System.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    ///     The delegate to use for handlers that receive MouseEventArgs.
    /// </summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    ///     The MouseEventArgs class provides access to the logical
    ///     Mouse device for all derived event args.
    /// </summary>
    public class MouseEventArgs : HandledEventArgs
    {
        private readonly Control currentTarget;
        private readonly Control originalTarget;
        private PointD location;

        /// <summary>
        ///     Initializes a new instance of the MouseEventArgs class.
        /// </summary>
        internal MouseEventArgs(
            Control currentTarget,
            Control originalTarget,
            MouseButton button,
            long timestamp)
            : this(currentTarget, originalTarget, timestamp)
        {
            ChangedButton = button;
            ClickCount = 1;
        }

        /// <summary>
        ///     Initializes a new instance of the MouseEventArgs class.
        /// </summary>
        internal MouseEventArgs(Control currentTarget, Control originalTarget, long timestamp)
        {
            this.currentTarget = currentTarget;
            this.originalTarget = originalTarget;
            MouseDevice = Mouse.PrimaryDevice;
            Timestamp = timestamp;
            location = GetPosition(currentTarget);
        }

        /// <summary>
        /// Gets current target control for the event.
        /// </summary>
        public Control CurrentTarget => currentTarget;

        /// <summary>
        /// Gets original target control for the event.
        /// </summary>
        public Control OriginalTarget => originalTarget;

        /// <summary>
        /// Gets timestamp of the event.
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        ///     Read-only access to the logical mouse device associated with
        ///     this event.
        /// </summary>
        public MouseDevice MouseDevice { get; }

        /// <summary>
        ///     Read-only access to the button being described.
        /// </summary>
        public MouseButton ChangedButton
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets which mouse button was pressed.
        /// </summary>
        /// <returns>One of the <see cref="MouseButtons" /> values.</returns>
        public MouseButtons Button
        {
            get
            {
                switch (ChangedButton)
                {
                    case MouseButton.Left:
                        return MouseButtons.Left;
                    case MouseButton.Middle:
                        return MouseButtons.Middle;
                    case MouseButton.Right:
                        return MouseButtons.Right;
                    case MouseButton.XButton1:
                        return MouseButtons.XButton1;
                    case MouseButton.XButton2:
                        return MouseButtons.XButton2;
                    default:
                        return MouseButtons.None;
                }
            }
        }

        /// <summary>
        /// Gets the x-coordinate of the mouse during the generating mouse event.
        /// </summary>
        /// <returns>The x-coordinate of the mouse, in dips.</returns>
        public double X
        {
            get
            {
                return location.X;
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the mouse during the generating mouse event.
        /// </summary>
        /// <returns>
        /// The y-coordinate of the mouse, in pixels.
        /// </returns>
        public double Y
        {
            get
            {
                return location.Y;
            }
        }

        /// <summary>
        ///     Read-only access to the button state.
        /// </summary>
        public MouseButtonState ButtonState
        {
            get
            {
                MouseButtonState state = MouseButtonState.Released;

                switch (ChangedButton)
                {
                    case MouseButton.Left:
                        state = this.MouseDevice.LeftButton;
                        break;

                    case MouseButton.Right:
                        state = this.MouseDevice.RightButton;
                        break;

                    case MouseButton.Middle:
                        state = this.MouseDevice.MiddleButton;
                        break;

                    case MouseButton.XButton1:
                        state = this.MouseDevice.XButton1;
                        break;

                    case MouseButton.XButton2:
                        state = this.MouseDevice.XButton2;
                        break;
                }

                return state;
            }
        }

        /// <summary>
        /// Gets the number of times the mouse button was pressed and released.
        /// </summary>
        /// <returns>
        /// An <see cref="int" /> that contains the number of times the mouse button
        /// was pressed and released.</returns>
        public int Clicks => ClickCount;

        /// <summary>
        /// Same as <see cref="Clicks"/>.
        /// </summary>
        public int ClickCount
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a signed count of the number of detents the mouse wheel has rotated,
        /// multiplied by the WHEEL_DELTA constant. A detent is one notch of the
        /// mouse wheel.</summary>
        /// <returns>
        /// A signed count of the number of detents the mouse wheel has rotated,
        /// multiplied by the WHEEL_DELTA constant.
        /// </returns>
        public int Delta
        {
            get;
            internal set;
        }

        /// <summary>
        ///     The state of the left button.
        /// </summary>
        public MouseButtonState LeftButton
        {
            get
            {
                return this.MouseDevice.LeftButton;
            }
        }

        /// <summary>
        ///     The state of the right button.
        /// </summary>
        public MouseButtonState RightButton
        {
            get
            {
                return this.MouseDevice.RightButton;
            }
        }

        /// <summary>
        ///     The state of the middle button.
        /// </summary>
        public MouseButtonState MiddleButton
        {
            get
            {
                return this.MouseDevice.MiddleButton;
            }
        }

        /// <summary>
        ///     The state of the first extended button.
        /// </summary>
        public MouseButtonState XButton1
        {
            get
            {
                return this.MouseDevice.XButton1;
            }
        }

        /// <summary>
        ///     The state of the second extended button.
        /// </summary>
        public MouseButtonState XButton2
        {
            get
            {
                return this.MouseDevice.XButton2;
            }
        }

        /// <summary>
        /// Gets the location of the mouse during the generating mouse event.
        /// </summary>
        /// <returns>A <see cref="PointD" /> that contains the x- and y- mouse
        /// coordinates, in dips, relative to the upper-left corner of the control.</returns>
        public PointD Location => location;

        /// <summary>
        ///     Calculates the position of the mouse relative to
        ///     a particular element.
        /// </summary>
        public PointD GetPosition(Control? relativeTo)
        {
            if (relativeTo is null)
                return this.MouseDevice.GetScreenPosition();
            else
                return this.MouseDevice.GetPosition(relativeTo);
        }

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"MouseEventArgs ({ChangedButton}, {Location}, ClickCount: {ClickCount}, Delta: {Delta})";
        }
    }
}
