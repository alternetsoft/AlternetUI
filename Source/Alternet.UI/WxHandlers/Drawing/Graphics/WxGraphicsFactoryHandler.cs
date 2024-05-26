using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class WxGraphicsFactoryHandler : DisposableObject, IGraphicsFactoryHandler
    {
        public Graphics CreateGraphicsFromScreen()
        {
            return new WxGraphics(UI.Native.DrawingContext.FromScreen());
        }

        public Graphics CreateGraphicsFromImage(Image image)
        {
            return new WxGraphics(
                UI.Native.DrawingContext.FromImage(
                    (UI.Native.Image)image.NativeObject));
        }
    }
}
