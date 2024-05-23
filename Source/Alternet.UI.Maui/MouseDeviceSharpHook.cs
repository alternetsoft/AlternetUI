using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI.Maui
{
    public class MouseDeviceSharpHook : MouseDevice
    {
        public static MouseDevice Default = new MouseDeviceSharpHook();

        public static MouseButtonState ButtonState;

        public static PointD ScreenPosition;

        protected override MouseButtonState GetButtonStateFromSystem(MouseButton mouseButton)
        {
            return ButtonState;
        }

        protected override PointD GetScreenPositionFromSystem()
        {
            return ScreenPosition;
        }
    }
}
