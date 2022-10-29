using Alternet.UI;
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
        /// <summary>
        /// Gets the <see cref="DrawingContext"/> used to paint the page.
        /// </summary>
        /// <value>
        /// The <see cref="DrawingContext"/> used to paint the page.
        /// </value>
        public DrawingContext DrawingContext { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether an additional page should be printed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if an additional page should be printed; otherwise, <see langword="false"/>. The
        /// default is <see langword="false"/>.
        /// </value>
        public bool HasMorePages { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets the page settings for the current page.
        /// </summary>
        /// <value>
        /// The page settings for the current page.
        /// </value>
        public PageSettings PageSettings { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page inside the margins, in millimeters.
        /// </summary>
        public Thickness PhysicalMarginBounds { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page inside the margins, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        public Rect MarginBounds { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets the rectangular area that represents the total area of the page, in millimeters.
        /// </summary>
        public Rect PhysicalPageBounds { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets the rectangular area that represents the portion of the page inside the margins, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        /// <value>
        /// The rectangular area, measured in device-independent units (1/96th inch per unit) that represents the total area of the page.
        /// </value>
        public Rect PageBounds { get => throw new Exception(); set => throw new Exception(); }
    }
}