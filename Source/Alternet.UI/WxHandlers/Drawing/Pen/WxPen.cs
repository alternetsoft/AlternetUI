using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class Pen : Alternet.Drawing.IPenHandler
    {
        public void Update(Alternet.Drawing.Pen pen)
        {
            Initialize(
                pen.DashStyle,
                pen.Color,
                pen.Width,
                pen.LineCap,
                pen.LineJoin);
        }
    }
}