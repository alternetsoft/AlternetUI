using System;
using System.Security;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    ///     The Mouse class represents the mouse device to the
    ///     members of a context.
    /// </summary>
    /// <remarks>
    ///     The static members of this class simply delegate to the primary
    ///     mouse device of the calling thread's input manager.
    /// </remarks>
    public static class Mouse
    {
        /// <summary>
        ///     The number of units the mouse wheel should be rotated to scroll one line.
        /// </summary>
        /// <remarks>
        ///     The delta was set to 120 to allow Microsoft or other vendors to
        ///     build finer-resolution wheels in the future, including perhaps
        ///     a freely-rotating wheel with no notches. The expectation is
        ///     that such a device would send more messages per rotation, but
        ///     with a smaller value in each message. To support this
        ///     possibility, you should either add the incoming delta values
        ///     until MouseWheelDeltaForOneLine amount is reached (so for a
        ///     delta-rotation you get the same response), or scroll partial
        ///     lines in response to the more frequent messages. You could also
        ///     choose your scroll granularity and accumulate deltas until it
        ///     is reached.
        /// </remarks>
        public static int MouseWheelDeltaForOneLine = 120;

        private static IMouseHandler? handler;

        /// <summary>
        /// Gets or sets <see cref="IMouseHandler"/> used to access mouse.
        /// </summary>
        public static IMouseHandler Handler
        {
            get
            {
                return handler ??= App.Handler.CreateMouseHandler();
            }

            set
            {
                handler = value;
            }
        }

        /// <summary>
        ///     The state of the left button.
        /// </summary>
        public static MouseButtonState LeftButton
        {
            get
            {
                return GetButtonState(MouseButton.Left);
            }
        }

        /// <summary>
        ///     The state of the right button.
        /// </summary>
        public static MouseButtonState RightButton
        {
            get
            {
                return GetButtonState(MouseButton.Right);
            }
        }

        /// <summary>
        ///     The state of the middle button.
        /// </summary>
        public static MouseButtonState MiddleButton
        {
            get
            {
                return GetButtonState(MouseButton.Middle);
            }
        }

        /// <summary>
        ///     The state of the first extended button.
        /// </summary>
        public static MouseButtonState XButton1
        {
            get
            {
                return GetButtonState(MouseButton.XButton1);
            }
        }

        /// <summary>
        ///     The state of the second extended button.
        /// </summary>
        public static MouseButtonState XButton2
        {
            get
            {
                return GetButtonState(MouseButton.XButton2);
            }
        }

        /// <summary>
        /// Gets mouse button state.
        /// </summary>
        /// <param name="mouseButton">Mouse button.</param>
        /// <returns></returns>
        public static MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return Handler.GetButtonState(mouseButton);
        }

        /// <summary>
        /// Gets mouse button position.
        /// </summary>
        /// <param name="scaleFactor">Scale factor to use when converting pixels to dips.</param>
        /// <returns></returns>
        public static PointD GetPosition(Coord scaleFactor)
        {
            return Handler.GetPosition(scaleFactor);
        }

        /// <summary>
        /// Gets mouse button position.
        /// </summary>
        /// <returns></returns>
        public static PointD GetPosition()
        {
            return Handler.GetPosition(null);
        }

        /// <summary>
        ///     Calculates the position of the mouse relative to
        ///     a particular element.
        /// </summary>
        public static PointD GetPosition(Control? relativeTo)
        {
            var position = Handler.GetPosition(relativeTo?.ScaleFactor);
            if (relativeTo is not null)
            {
                var clientPosition = relativeTo.ScreenToClient(position);
                return clientPosition;
            }

            return position;
        }
    }
}