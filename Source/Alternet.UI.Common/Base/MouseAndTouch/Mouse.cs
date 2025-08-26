using System;
using System.Runtime.CompilerServices;
using System.Security;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the mouse device.
    /// </summary>
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
        /// Occurs when any of the controls receives mouse move event.
        /// </summary>
        public static event MouseEventHandler? Moved;

        /// <summary>
        /// Gets whether mouse is present.
        /// </summary>
        public static bool? IsMousePresent
        {
            get
            {
                return Handler.MousePresent;
            }
        }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return Handler.GetButtonState(mouseButton);
        }

        /// <summary>
        /// Gets mouse button position.
        /// </summary>
        /// <param name="scaleFactor">Scale factor to use when converting pixels to dips.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointD GetPosition(Coord scaleFactor)
        {
            return Handler.GetPosition(scaleFactor);
        }

        /// <summary>
        /// Determines the effective position based on the specified position and the relative control.
        /// </summary>
        /// <param name="position">The position to evaluate.
        /// If <see langword="null"/> or equal to <see cref="PointD.MinusOne"/>, the current
        /// mouse position relative to <paramref name="relativeTo"/> is returned.</param>
        /// <param name="relativeTo">The control relative to which the mouse position
        /// is calculated if <paramref name="position"/> is <see langword="null"/> or
        /// <see cref="PointD.MinusOne"/>. Can be <see langword="null"/>.</param>
        /// <returns>The coerced position. If <paramref name="position"/>
        /// is not <see langword="null"/> and not <see cref="PointD.MinusOne"/>,
        /// the value of <paramref name="position"/> is returned.</returns>
        public static PointD CoercePosition(PointD? position, AbstractControl? relativeTo)
        {
            if(position is null || position == PointD.MinusOne)
                return Mouse.GetPosition(relativeTo);

            return position.Value;
        }

        /// <summary>
        ///     Calculates the position of the mouse relative to
        ///     a particular element.
        /// </summary>
        public static PointD GetPosition(AbstractControl? relativeTo)
        {
            var position = Handler.GetPosition(relativeTo?.ScaleFactor);
            if (relativeTo is not null)
            {
                var clientPosition = relativeTo.ScreenToClient(position);
                return clientPosition;
            }

            return position;
        }

        /// <summary>
        /// Raises <see cref="Moved"/> event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        public static void RaiseMoved(object sender, MouseEventArgs e)
        {
            Moved?.Invoke(sender, e);
        }
    }
}