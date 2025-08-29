using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Internal mouse handler not bound to the platform.
    /// </summary>
    public class PlessMouseHandler : DisposableObject, IMouseHandler
    {
        /// <inheritdoc/>
        public virtual bool? MousePresent => null;

        /// <inheritdoc/>
        public virtual MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return PlessMouse.GetButtonState(mouseButton);
        }

        /// <inheritdoc/>
        public virtual PointD GetPosition(Coord? scaleFactor)
        {
            var (position, control) = PlessMouse.LastMousePosition;

            if (control is null || position is null)
                return PointD.MinValue;

            var result = control.ClientToScreen(position.Value);

            return result;
        }

        /// <inheritdoc/>
        public virtual ICursorFactoryHandler CreateCursorFactoryHandler()
        {
            return new PlessCursorFactoryHandler();
        }
    }
}
