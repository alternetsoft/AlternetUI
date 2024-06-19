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
        public static bool ShowTestMouseInControl = false;

        public static Color TestMouseColor = Color.Red;

        public static SizeD TestMouseSize = 5;

        private static readonly bool[] Buttons = new bool[(int)MouseButton.Unknown + 1];

        private static (PointD? Position, Control? Control) lastMousePosition;

        /// <summary>
        /// Occurs when <see cref="LastMousePosition"/> property is changed.
        /// </summary>
        public static event EventHandler? LastMousePositionChanged;

        public static RectD GetTestMouseRect(Control control)
        {
            var mouseLocation = Mouse.GetPosition(control);
            return (mouseLocation, PlessMouse.TestMouseSize);
        }

        public static PointD UpdateMousePosition(PointD? position, Control control)
        {
            position ??= Mouse.GetPosition(control);
            PlessMouse.LastMousePosition = (position, control);
            return position.Value;
        }

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

        public static bool IsButtonPressed(MouseButton mouseButton)
        {
            if (mouseButton >= MouseButton.Unknown || mouseButton < 0)
                return false;
            return Buttons[(int)mouseButton];
        }

        public static MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return IsButtonPressed(mouseButton) ? MouseButtonState.Pressed : MouseButtonState.Released;
        }

        public static void SetButtonPressed(MouseButton button, bool value = true)
        {
            Buttons[(int)button] = value;
        }
    }
}
