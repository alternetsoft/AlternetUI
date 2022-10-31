using System;
using System.ComponentModel;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Provides data for the <see cref="PrintDocument.PrintPage"/> event.
    /// </summary>
    /// <remarks>
    /// The MarginBounds property retrieves the rectangular area that represents the portion of the page between the
    /// margins. The PageBounds property retrieves the rectangular area that represents the total area of the page. The
    /// Graphics property defines the graphics object with which to do the painting. The PageSettings property retrieves
    /// the printer settings for the current page. The remaining properties indicate whether a print job should be
    /// canceled or whether a print job has more pages.
    /// </remarks>
    public class PrintPageEventArgs : CancelEventArgs
    {
        private UI.Native.PrintDocument nativePrintDocument;
        private readonly DrawingContext drawingContext;

        internal PrintPageEventArgs(UI.Native.PrintDocument nativePrintDocument, DrawingContext drawingContext)
        {
            this.nativePrintDocument = nativePrintDocument;
            this.drawingContext = drawingContext;
        }

        /// <summary>
        /// Gets the <see cref="DrawingContext"/> used to paint the page.
        /// </summary>
        /// <value>
        /// The <see cref="DrawingContext"/> used to paint the page.
        /// </value>
        public DrawingContext DrawingContext => drawingContext;

        /// <summary>
        /// Gets or sets a value indicating whether an additional page should be printed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if an additional page should be printed; otherwise, <see langword="false"/>. The
        /// default is <see langword="false"/>.
        /// </value>
        public bool HasMorePages { get => nativePrintDocument.PrintPage_HasMorePages; set => nativePrintDocument.PrintPage_HasMorePages = value; }

        /// <summary>
        /// Gets the page settings for the current page.
        /// </summary>
        /// <value>
        /// The page settings for the current page.
        /// </value>
        public PageSettings PageSettings { get => new PageSettings(nativePrintDocument.PrintPage_PageSettings); }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page inside the margins, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        public Rect MarginBounds { get => nativePrintDocument.PrintPage_MarginBounds; }

        /// <summary>
        /// Gets the rectangular area that represents the total area of the page, in millimeters.
        /// </summary>
        public Rect PhysicalPageBounds { get => nativePrintDocument.PrintPage_PhysicalPageBounds; }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page inside the margins, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        /// <value>
        /// The rectangular area, measured in device-independent units (1/96th inch per unit) that represents the total area of the page.
        /// </value>
        public Rect PageBounds { get => nativePrintDocument.PrintPage_PageBounds; }
        
        /// <summary>
        /// Gets the rectangular area that represents the printable portion of the page, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        /// <value>
        /// The rectangular area, measured in device-independent units (1/96th inch per unit) that represents the printable area of the page.
        /// </value>
        public Rect PrintablePageBounds { get => nativePrintDocument.PrintPage_PrintablePageBounds; }
    }
}