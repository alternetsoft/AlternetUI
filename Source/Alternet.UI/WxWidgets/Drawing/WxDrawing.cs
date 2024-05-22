using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing : NativeDrawing
    {
        public override Graphics CreateGraphicsFromScreen()
        {
            return new WxGraphics(UI.Native.DrawingContext.FromScreen());
        }

        public override Graphics CreateGraphicsFromImage(Image image)
        {
            return new WxGraphics(
                UI.Native.DrawingContext.FromImage(
                    (UI.Native.Image)image.NativeObject));
        }
    }
}