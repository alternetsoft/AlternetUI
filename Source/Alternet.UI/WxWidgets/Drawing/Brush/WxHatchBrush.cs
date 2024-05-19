using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class HatchBrush : Alternet.Drawing.IHatchBrushHandler
    {
        public void Update(Alternet.Drawing.HatchBrush brush)
        {
            Initialize(
                (UI.Native.BrushHatchStyle)brush.HatchStyle,
                brush.Color);
        }
    }
}