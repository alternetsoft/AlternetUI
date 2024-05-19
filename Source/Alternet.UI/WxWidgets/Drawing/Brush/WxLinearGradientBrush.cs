using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class LinearGradientBrush : Alternet.Drawing.ILinearGradientBrushHandler
    {
        public void Update(Alternet.Drawing.LinearGradientBrush brush)
        {
            Initialize(
                brush.StartPoint,
                brush.EndPoint,
                brush.GradientStops.Select(x => x.Color).ToArray(),
                brush.GradientStops.Select(x => x.Offset).ToArray());
        }
    }
}
