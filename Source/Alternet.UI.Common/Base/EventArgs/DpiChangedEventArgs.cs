using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

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
        private SizeI dpiOld;
        private SizeI dpiNew;

        /// <summary>
        /// Gets the DPI value for the display device where the control or form was previously
        /// displayed.
        /// </summary>
        /// <returns>A DPI value.</returns>
        public int DeviceDpiOld
        {
            get => dpiOld.Width;
            set => dpiOld.Width = value;
        }

        /// <summary>
        /// Gets the DPI value for the new display device where the control or form is
        /// currently being displayed.
        /// </summary>
        /// <returns>The DPI value.</returns>
        public int DeviceDpiNew
        {
            get => dpiNew.Width;
            set => dpiNew.Width = value;
        }

        public SizeI DpiOld
        {
            get => dpiOld;
            set => dpiOld = value;
        }

        public SizeI DpiNew
        {
            get => dpiNew;
            set => dpiNew = value;
        }

        public DpiChangedEventArgs(SizeI oldDpi, SizeI newDpi)
        {
            DpiOld = oldDpi;
            DpiNew = newDpi;
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
