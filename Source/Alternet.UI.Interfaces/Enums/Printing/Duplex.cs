using System.ComponentModel;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies the printer's duplex setting.
    /// </summary>
    public enum Duplex
    {
        /// <summary>
        /// Single-sided printing.
        /// </summary>
        Simplex,

        /// <summary>
        /// Double-sided, vertical printing.
        /// </summary>
        Vertical,

        /// <summary>
        /// Double-sided, horizontal printing.
        /// </summary>
        Horizontal,
    }
}