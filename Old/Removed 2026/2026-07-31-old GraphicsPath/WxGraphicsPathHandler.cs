using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI.Native
{
    internal partial class GraphicsPath : Alternet.Drawing.IGraphicsPathHandler
    {
        public void AddLines(ReadOnlySpan<PointD> points)
        {
            unsafe
            {
                fixed (PointD* p = points)
                {
                    AddLines(p, points.Length);
                }
            }
        }
    }
}
