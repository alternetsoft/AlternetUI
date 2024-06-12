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

        private static MouseDevice mouseDevice = MouseDevice.Default;
        private static long? mouseWheelTimestamp;

        /// <summary>
        ///     The state of the left button.
        /// </summary>
        public static MouseButtonState LeftButton
        {
            get
            {
                return Mouse.PrimaryDevice.LeftButton;
            }
        }

        /// <summary>
        ///     The state of the right button.
        /// </summary>
        public static MouseButtonState RightButton
        {
            get
            {
                return Mouse.PrimaryDevice.RightButton;
            }
        }

        /// <summary>
        ///     The primary mouse device.
        /// </summary>
        public static MouseDevice PrimaryDevice
        {
            get
            {
                return mouseDevice;
            }

            set
            {
                mouseDevice = value;
            }
        }

        /// <summary>
        ///     The state of the middle button.
        /// </summary>
        public static MouseButtonState MiddleButton
        {
            get
            {
                return Mouse.PrimaryDevice.MiddleButton;
            }
        }

        /// <summary>
        ///     The state of the first extended button.
        /// </summary>
        public static MouseButtonState XButton1
        {
            get
            {
                return Mouse.PrimaryDevice.XButton1;
            }
        }

        /// <summary>
        ///     The state of the second extended button.
        /// </summary>
        public static MouseButtonState XButton2
        {
            get
            {
                return Mouse.PrimaryDevice.XButton2;
            }
        }

        /// <summary>
        /// </summary>
        public static PointD GetPosition()
        {
            return App.Handler.GetMousePositionFromSystem();
        }

        /// <summary>
        ///     Calculates the position of the mouse relative to
        ///     a particular element.
        /// </summary>
        public static PointD GetPosition(Control? relativeTo)
        {
            if(relativeTo is not null)
                return relativeTo.ScreenToClient(GetPosition());
            return GetPosition();
        }

        public static void ReportMouseMove(
            Control originalTarget,
            long timestamp,
            PointD? position,
            out bool handled)
        {
            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = UpdateMousePosition(position, currentTarget);

            var eventArgs = new MouseEventArgs(
                currentTarget,
                originalTarget,
                timestamp,
                position.Value);
            currentTarget.RaiseMouseMove(eventArgs);
        }

        public static void ReportMouseDown(
            Control originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            PlessMouse.SetButtonPressed(changedButton);

            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = UpdateMousePosition(position, currentTarget);

            var eventArgs = new MouseEventArgs(
                currentTarget,
                originalTarget,
                changedButton,
                timestamp,
                position.Value);

            currentTarget.RaiseMouseDown(eventArgs);
        }

        public static void ReportMouseDoubleClick(
            Control originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = UpdateMousePosition(position, currentTarget);

            var eventArgs =
                new MouseEventArgs(
                    currentTarget,
                    originalTarget,
                    changedButton,
                    timestamp,
                    position.Value);
            currentTarget.RaiseMouseDoubleClick(eventArgs);
        }

        public static void ReportMouseUp(
            Control originalTarget,
            long timestamp,
            MouseButton changedButton,
            PointD? position,
            out bool handled)
        {
            PlessMouse.SetButtonPressed(changedButton, false);

            handled = false;
            var currentTarget = Control.GetMouseTargetControl(originalTarget);
            if (currentTarget == null)
                return;

            position = UpdateMousePosition(position, currentTarget);

            var eventArgs
                = new MouseEventArgs(
                    currentTarget,
                    originalTarget,
                    changedButton,
                    timestamp,
                    position.Value);
            currentTarget.RaiseMouseUp(eventArgs);
        }

        private static PointD UpdateMousePosition(PointD? position, Control control)
        {
            position ??= Mouse.GetPosition(control);
            PlessMouse.LastMousePosition = (position, control);
            return position.Value;
        }

        public static void ReportMouseWheel(
            Control originalTarget,
            long timestamp,
            int delta,
            PointD? position,
            out bool handled)
        {
            handled = false;

            if (mouseWheelTimestamp == timestamp)
                return;
            mouseWheelTimestamp = timestamp;

            var currentTarget = Control.GetMouseTargetControl(originalTarget);            
            if (currentTarget == null)
                return;

            position = UpdateMousePosition(position, currentTarget);

            var eventArgs
                = new MouseEventArgs(
                    currentTarget,
                    originalTarget,
                    timestamp,
                    position.Value);
            eventArgs.Delta = delta;
            currentTarget.RaiseMouseWheel(eventArgs);
        }
    }
}