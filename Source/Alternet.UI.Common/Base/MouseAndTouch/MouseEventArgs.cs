using System;
using System.Collections;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// The delegate to use for handlers that receive <see cref="MouseEventArgs"/>.
    /// </summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// Provides data for the mouse events.
    /// </summary>
    public class MouseEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Gets initialized <see cref="MouseEventArgs"/> object.
        /// </summary>
        public static new readonly MouseEventArgs Empty = new();

        private object currentTarget;
        private object originalTarget;
        private PointD location;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class
        /// with the specified parameters.
        /// </summary>
        public MouseEventArgs(
            object currentTarget,
            object originalTarget,
            MouseButton button,
            long timestamp,
            PointD location)
            : this(currentTarget, originalTarget, timestamp, location)
        {
            ChangedButton = button;
            ClickCount = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class
        /// with the specified parameters.
        /// </summary>
        public MouseEventArgs(
            object currentTarget,
            object originalTarget,
            long timestamp,
            PointD location)
        {
            this.currentTarget = currentTarget;
            this.originalTarget = originalTarget;
            Timestamp = timestamp;
            this.location = location;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        public MouseEventArgs()
        {
            currentTarget = AssemblyUtils.Default;
            originalTarget = AssemblyUtils.Default;
        }

        /// <summary>
        /// Gets or sets device type which raised the event.
        /// </summary>
        public TouchDeviceType DeviceType { get; set; } = TouchDeviceType.Mouse;

        /// <summary>
        /// Gets or sets current target control for the event. Same as <see cref="CurrentTarget"/>.
        /// </summary>
        public object Source
        {
            get => currentTarget;
            set => currentTarget = value;
        }

        /// <summary>
        /// Gets or sets current target control for the event. Same as <see cref="Source"/>.
        /// </summary>
        public object CurrentTarget
        {
            get => currentTarget;
            set => currentTarget = value;
        }

        /// <summary>
        /// Gets or sets original target control for the event.
        /// </summary>
        public object OriginalTarget
        {
            get => originalTarget;
            set => originalTarget = value;
        }

        /// <summary>
        /// Gets or sets timestamp of the event.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Gets or sets changed button.
        /// </summary>
        public MouseButton ChangedButton
        {
            get;
            set;
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
        /// Value is relative to the left corner of the control.
        /// </summary>
        /// <returns>The x-coordinate of the mouse, in dips.</returns>
        public Coord X
        {
            get
            {
                return location.X;
            }
        }

        /// <summary>
        /// Gets the y-coordinate of the mouse during the generating mouse event.
        /// Value is relative to the upper corner of the control.
        /// </summary>
        /// <returns>
        /// The y-coordinate of the mouse, in dips.
        /// </returns>
        public Coord Y
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
                return Mouse.GetButtonState(ChangedButton);
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
            set;
        }

        /// <summary>
        /// Gets or sets a signed count of the number of detents the mouse wheel has rotated.</summary>
        /// <returns>
        /// A signed count of the number of detents the mouse wheel has rotated.
        /// </returns>
        public int Delta
        {
            get;
            set;
        }

        /// <summary>
        /// The state of the left button.
        /// </summary>
        public MouseButtonState LeftButton
        {
            get
            {
                return Mouse.LeftButton;
            }
        }

        /// <summary>
        /// The state of the right button.
        /// </summary>
        public MouseButtonState RightButton
        {
            get
            {
                return Mouse.RightButton;
            }
        }

        /// <summary>
        /// The state of the middle button.
        /// </summary>
        public MouseButtonState MiddleButton
        {
            get
            {
                return Mouse.MiddleButton;
            }
        }

        /// <summary>
        /// The state of the first extended button.
        /// </summary>
        public MouseButtonState XButton1
        {
            get
            {
                return Mouse.XButton1;
            }
        }

        /// <summary>
        /// The state of the second extended button.
        /// </summary>
        public MouseButtonState XButton2
        {
            get
            {
                return Mouse.XButton2;
            }
        }

        /// <summary>
        /// Gets or sets the location of the mouse during the generating mouse event.
        /// </summary>
        /// <returns>A <see cref="PointD" /> that contains mouse
        /// coordinates, in dips, relative to the upper-left corner of the control.</returns>
        public PointD Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
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
