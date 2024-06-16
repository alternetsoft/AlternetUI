using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessMouseHandler : DisposableObject, IMouseHandler
    {
        public virtual MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return PlessMouse.GetButtonState(mouseButton);
        }

        public virtual PointD GetPosition()
        {
            var (position, control) = PlessMouse.LastMousePosition;

            if (control is null || position is null)
                return PointD.MinValue;

            return control.ClientToScreen(position.Value);
        }

        public virtual ICursorFactoryHandler CreateCursorFactoryHandler()
        {
            return new PlessCursorFactoryHandler();
        }
    }
}
