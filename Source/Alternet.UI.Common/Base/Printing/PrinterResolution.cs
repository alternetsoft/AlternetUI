using System;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Represents the resolution supported by a printer.
    /// </summary>
    public class PrinterResolution
    {
        private PrinterResolutionKind printerResolutionKind;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterResolution"/> class with
        /// the specified printer resolution kind.
        /// </summary>
        /// <param name="printerResolutionKind">The printer resolution kind.</param>
        public PrinterResolution(PrinterResolutionKind printerResolutionKind)
        {
            this.printerResolutionKind = printerResolutionKind;
        }

        /// <summary>
        /// Gets or sets the printer resolution kind.
        /// </summary>
        public virtual PrinterResolutionKind Kind
        {
            get => printerResolutionKind;
            set => printerResolutionKind = value;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{printerResolutionKind}";
        }
    }
}