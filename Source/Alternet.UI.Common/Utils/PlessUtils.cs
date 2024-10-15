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
        /// <summary>
        /// Converts screen coordinates to client coordinates.
        /// </summary>
        /// <param name="position">Point in screen coordinates.</param>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static PointD ScreenToClient(PointD position, AbstractControl control)
        {
            var topLeft = ClientToScreen(PointD.Empty, control);
            var result = position - topLeft;
            return result;
        }

        /// <summary>
        /// Converts client coordinates to screen coordinates.
        /// </summary>
        /// <param name="position">Point in client coordinates.</param>
        /// <param name="control">Control.</param>
        /// <returns></returns>
        public static PointD ClientToScreen(PointD position, AbstractControl control)
        {
            PointD absolutePos = control.AbsolutePosition;
            var result = absolutePos + position;
            return result;
        }
    }
}
