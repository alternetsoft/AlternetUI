using System;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Represents the resolution supported by a printer.
    /// </summary>
    public class PrinterResolution
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterResolution"/> class with the specified printer resolution kind.
        /// </summary>
        /// <param name="printerResolutionKind">The printer resolution kind.</param>
        public PrinterResolution(PrinterResolutionKind printerResolutionKind)
        {
        }

        /// <summary>
        /// Gets the printer resolution kind.
        /// </summary>
        public PrinterResolutionKind Kind { get => throw new Exception(); set => throw new Exception(); }
    }
}