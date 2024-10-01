using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to control the mouse behavior.
    /// </summary>
    public interface IMouseHandler : IDisposable
    {
        /// <summary>
        /// Gets whether mouse is present.
        /// </summary>
        bool? MousePresent { get; }

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

        /// <summary>
        /// Gets mouse position.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used to convert pixels to dips.</param>
        /// <returns></returns>
        PointD GetPosition(Coord? scaleFactor);

        /// <summary>
        /// Creates <see cref="ICursorFactoryHandler"/> object.
        /// </summary>
        /// <returns></returns>
        ICursorFactoryHandler CreateCursorFactoryHandler();
    }
}
