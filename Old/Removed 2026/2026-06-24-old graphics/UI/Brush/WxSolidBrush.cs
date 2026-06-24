using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class SolidBrush : Alternet.Drawing.ISolidBrushHandler
    {
        public void Update(Alternet.Drawing.SolidBrush brush)
        {
            Initialize(brush.Color);
        }
    }
}
