using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxMouseHandler : DisposableObject, IMouseHandler
    {
        /// <inheritdoc/>
        public bool? MousePresent => null;

        /// <inheritdoc/>
        public ICursorFactoryHandler CreateCursorFactoryHandler()
        {
            return new WxCursorFactoryHandler();
        }

        /// <inheritdoc/>
        public MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return Native.Mouse.GetButtonState(mouseButton);
        }

        /// <inheritdoc/>
        public PointD GetPosition(Coord? scaleFactor)
        {
            var resultI = Native.Mouse.GetPosition();
            var result = GraphicsFactory.PixelToDip(resultI, scaleFactor);
            return result;
        }
    }
}
