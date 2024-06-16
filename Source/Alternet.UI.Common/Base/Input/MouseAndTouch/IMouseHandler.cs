using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IMouseHandler : IDisposable
    {
        /// <summary>
        /// Gets the current state of the specified button from
        /// the device from the underlying system.
        /// </summary>
        /// <param name="mouseButton">
        /// The mouse button to get the state of.
        /// </param>
        /// <returns>
        /// The state of the specified mouse button.
        /// </returns>
        MouseButtonState GetButtonState(MouseButton mouseButton);

        PointD GetPosition();

        ICursorFactoryHandler CreateCursorFactoryHandler();
    }
}
