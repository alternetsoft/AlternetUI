using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alternet.Winforms
{
    /// <summary>
    /// Provides utility methods for working with WinForms.
    /// </summary>
    public static class WinformsUtils
    {
        /// <summary>
        /// Gets the scale factor of the specified WinForms control.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <returns>The scale factor of the control.</returns>
        public static float GetScaleFactor(Control control)
        {
#if NET471_OR_GREATER || NET8_0_OR_GREATER
            float dpi = control.DeviceDpi;
            return dpi / 96f;
#else
            using var g = control.CreateGraphics();
            return g.DpiX / 96f;
#endif
        }
    }
}
