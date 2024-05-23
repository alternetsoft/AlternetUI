using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class RadialGradientBrush : Alternet.Drawing.IRadialGradientBrushHandler
    {
        public void Update(Alternet.Drawing.RadialGradientBrush brush)
        {
            Initialize(
                brush.Center,
                brush.Radius,
                brush.GradientOrigin,
                brush.GradientStops.Select(x => x.Color).ToArray(),
                brush.GradientStops.Select(x => x.Offset).ToArray());
        }
    }
}