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
        public ICursorFactoryHandler CreateCursorFactoryHandler()
        {
            return new WxCursorFactoryHandler();
        }

        /// <inheritdoc/>
        public MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            return WxApplicationHandler.NativeMouse.GetButtonState(mouseButton);
        }

        /// <inheritdoc/>
        public PointD GetPosition()
        {
            var resultI = WxApplicationHandler.NativeMouse.GetPosition();
            var result = GraphicsFactory.PixelToDip(resultI);
            return result;
        }
    }
}
