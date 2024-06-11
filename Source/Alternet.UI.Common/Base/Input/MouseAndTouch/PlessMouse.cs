using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static class PlessMouse
    {
        private static bool[] buttons = new bool[(int)MouseButton.Unknown + 1];

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
