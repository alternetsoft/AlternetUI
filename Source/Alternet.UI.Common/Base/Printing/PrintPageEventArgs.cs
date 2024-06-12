using System;
using System.ComponentModel;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Provides data for the <see cref="PrintDocument.PrintPage"/> event.
    /// </summary>
    /// <remarks>
    /// The MarginBounds property retrieves the rectangular area that represents the portion
    /// of the page between the
    /// margins. The PageBounds property retrieves the rectangular area that represents
    /// the total area of the page. The
    /// Graphics property defines the graphics object with which to do the painting.
    /// The PageSettings property retrieves
    /// the printer settings for the current page. The remaining properties indicate
    /// whether a print job should be
    /// canceled or whether a print job has more pages.
    /// </remarks>
    public class PrintPageEventArgs : CancelEventArgs
    {
        private readonly Graphics drawingContext;
        private readonly PrintDocument printDocument;

        internal PrintPageEventArgs(PrintDocument printDocument, Graphics drawingContext)
        {
            this.printDocument = printDocument;
            this.drawingContext = drawingContext;
        }

        /// <summary>
        /// Gets the <see cref="DrawingContext"/> used to paint the page.
        /// </summary>
        /// <value>
        /// The <see cref="DrawingContext"/> used to paint the page.
        /// </value>
        public Graphics DrawingContext => drawingContext;

        /// <summary>
        /// Gets or sets a value indicating whether an additional page should be printed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if an additional page should be printed;
        /// otherwise, <see langword="false"/>. The
        /// default is <see langword="false"/>.
        /// </value>
        public bool HasMorePages
        {
            get => printDocument.Handler.HasMorePages;
            set => printDocument.Handler.HasMorePages = value;
        }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page
        /// inside the margins, in device-independent units.
        /// </summary>
        public RectD MarginBounds
        {
            get => printDocument.Handler.MarginBounds;
        }

        /// <summary>
        /// Gets the rectangular area that represents the total area of the page, in millimeters.
        /// </summary>
        public RectD PhysicalPageBounds
        {
            get => printDocument.Handler.PhysicalPageBounds;
        }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page inside
        /// the margins, in device-independent units.
        /// </summary>
        /// <value>
        /// The rectangular area, measured in device-independent units
        /// that represents the total area of the page.
        /// </value>
        public RectD PageBounds
        {
            get => printDocument.Handler.PageBounds;
        }

        /// <summary>
        /// Gets the rectangular area that represents the printable portion of the page,
        /// in device-independent units.
        /// </summary>
        /// <value>
        /// The rectangular area, measured in device-independent units
        /// that represents the printable area of the page.
        /// </value>
        public RectD PrintablePageBounds
        {
            get => printDocument.Handler.PrintablePageBounds;
        }

        /// <summary>
        /// Gets the 1-based number of the page being printed.
        /// </summary>
        public int PageNumber
        {
            get => printDocument.Handler.PrintedPageNumber;
        }
    }
}