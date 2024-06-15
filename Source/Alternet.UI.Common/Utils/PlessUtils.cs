using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Static methods related to platformless control and application implementation.
    /// </summary>
    public static class PlessUtils
    {
        public static PointD ScreenToClient(PointD position, Control control)
        {
            var topLeft = ClientToScreen(PointD.Empty, control);
            var result = position - topLeft;
            return result;
        }

        public static PointD ClientToScreen(PointD position, Control control)
        {
            PointD absolutePos = control.AbsolutePosition;
            var result = absolutePos + position;
            return result;
        }
    }
}
