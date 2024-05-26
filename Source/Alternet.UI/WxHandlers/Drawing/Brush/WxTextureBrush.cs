using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class TextureBrush : Alternet.Drawing.ITextureBrushHandler
    {
        public void Update(Alternet.Drawing.TextureBrush brush)
        {
            Initialize((UI.Native.Image)brush.Image.Handler);
        }
    }
}
