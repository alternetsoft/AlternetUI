using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{

    /// <summary>
    /// Represents the method that will handle a DPI changed event of a form or control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="DpiChangedEventArgs" /> that contains the event data.</param>
    public delegate void DpiChangedEventHandler(object? sender, DpiChangedEventArgs e);

    /// <summary>
    /// Provides data for the DPI changed events of a form or control.
    /// </summary>
    public sealed class DpiChangedEventArgs : BaseCancelEventArgs
    {
        /// <summary>
        /// Gets the DPI value for the display device where the control or form was previously
        /// displayed.
        /// </summary>
        /// <returns>A DPI value.</returns>
        public int DeviceDpiOld { get; set; }

        /// <summary>
        /// Gets the DPI value for the new display device where the control or form is
        /// currently being displayed.
        /// </summary>
        /// <returns>The DPI value.</returns>
        public int DeviceDpiNew { get; set; }

        public DpiChangedEventArgs(int oldDpi, int newDpi)
        {
            DeviceDpiOld = oldDpi;
            DeviceDpiNew = newDpi;
        }

        /// <summary>
        /// Creates and returns a string representation of the current
        /// <see cref="DpiChangedEventArgs" />.</summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return $"was: {DeviceDpiOld}, now: {DeviceDpiNew}";
        }
    }
}
