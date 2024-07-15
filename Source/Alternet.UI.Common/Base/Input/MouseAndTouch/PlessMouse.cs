using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the platformless mouse implementation.
    /// </summary>
    public static class PlessMouse
    {
        /// <summary>
        /// Gets or sets whether to draw test mouse pointer inside the control.
        /// </summary>
        public static bool ShowTestMouseInControl = false;

        /// <summary>
        /// Gets or sets color of the test mouse pointer.
        /// </summary>
        public static Color TestMouseColor = Color.Red;

        /// <summary>
        /// Gets or sets size of the test mouse pointer.
        /// </summary>
        public static SizeD TestMouseSize = 5;

        private static readonly bool[] Buttons = new bool[(int)MouseButton.Unknown + 1];

        private static (PointD? Position, Control? Control) lastMousePosition;

        /// <summary>
        /// Occurs when <see cref="LastMousePosition"/> property is changed.
        /// </summary>
        public static event EventHandler? LastMousePositionChanged;

        /// <summary>
        /// Gets last mouse position passed to mouse event handlers.
        /// </summary>
        public static (PointD? Position, Control? Control) LastMousePosition
        {
            get
            {
                return lastMousePosition;
            }

            set
            {
                if (lastMousePosition == value)
                    return;

                lastMousePosition = value;

                LastMousePositionChanged?.Invoke(null, EventArgs.Empty);

                if (ShowTestMouseInControl)
                {
                    Control.HoveredControl?.Refresh();
                }
            }
        }

        /// <summary>
        /// Gets rectangle for the test mouse pointer.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static RectD GetTestMouseRect(Control control)
        {
            var mouseLocation = Mouse.GetPosition(control);
            return (mouseLocation, PlessMouse.TestMouseSize);
        }

        /// <summary>
        /// Updates <see cref="LastMousePosition"/>
        /// </summary>
        /// <param name="position">Mouse position. If <c>null</c>, <see cref="Mouse.GetPosition(Control)"/>
        /// is used to get mouse position.</param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static PointD UpdateMousePosition(PointD? position, Control control)
        {
            position ??= Mouse.GetPosition(control);
            PlessMouse.LastMousePosition = (position, control);
            return position.Value;
        }

        /// <summary>
        /// Draws test mouse pointer inside the control.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <param name="dc">Drawing context.</param>
        public static void DrawTestMouseRect(Control control, Graphics dc)
        {
            if (control != Control.HoveredControl)
                return;

            if (control.UserPaint && ShowTestMouseInControl)
            {
                dc.FillRectangle(TestMouseColor.AsBrush, GetTestMouseRect(control));
            }
        }

        /// <summary>
        /// Gets mouse button pressed state which was changed using
        /// <see cref="SetButtonPressed"/> method.
        /// </summary>
        /// <param name="mouseButton">Mouse button.</param>
        /// <returns></returns>
        public static bool IsButtonPressed(MouseButton mouseButton)
        {
            if (mouseButton >= MouseButton.Unknown || mouseButton < 0)
                return false;
            return Buttons[(int)mouseButton];
        }

        /// <summary>
        /// Gets mouse button pressed state which was changed using
        /// <see cref="SetButtonPressed"/> method.
        /// </summary>
        /// <param name="mouseButton">Mouse button.</param>
        /// <returns></returns>
        public static MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return IsButtonPressed(mouseButton) ? MouseButtonState.Pressed : MouseButtonState.Released;
        }

        /// <summary>
        /// Sets internal mouse button pressed state. Do not call directly.
        /// </summary>
        /// <param name="button">Mouse button.</param>
        /// <param name="value">Pressed state.</param>
        public static void SetButtonPressed(MouseButton button, bool value = true)
        {
            Buttons[(int)button] = value;
        }
    }
}
