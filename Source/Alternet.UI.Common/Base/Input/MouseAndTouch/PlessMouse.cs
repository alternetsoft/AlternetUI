using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public static class PlessMouse
    {
        private static (PointD? Position, Control? Control) lastMousePosition;
        private static readonly bool[] buttons = new bool[(int)MouseButton.Unknown + 1];

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
            }
        }

        public static bool IsButtonPressed(MouseButton mouseButton)
        {
            if (mouseButton >= MouseButton.Unknown || mouseButton < 0)
                return false;
            return buttons[(int)mouseButton];
        }

        public static MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return IsButtonPressed(mouseButton) ? MouseButtonState.Pressed : MouseButtonState.Released;
        }

        public static void SetButtonPressed(MouseButton button, bool value = true)
        {
            buttons[(int)button] = value;
        }
    }
}
