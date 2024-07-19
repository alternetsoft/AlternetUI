using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains methods and properties which allow to work with print document.
    /// </summary>
    public interface IPrintDocumentHandler : IDisposable
    {
        /// <inheritdoc cref="PrintDocument.PrintPage"/>
        event EventHandler<CancelEventArgs>? PrintPage;

        /// <inheritdoc cref="PrintDocument.BeginPrint"/>
        event EventHandler<CancelEventArgs>? BeginPrint;

        /// <inheritdoc cref="PrintDocument.EndPrint"/>
        event EventHandler<CancelEventArgs>? EndPrint;

        /// <inheritdoc cref="PrintDocument.OriginAtMargins"/>
        bool OriginAtMargins { get; set; }

        /// <inheritdoc cref="PrintDocument.DocumentName"/>
        string DocumentName { get; set; }

        /// <inheritdoc cref="PrintDocument.PrinterSettings"/>
        IPrinterSettingsHandler PrinterSettings { get; }

        /// <inheritdoc cref="PrintDocument.PageSettings"/>
        IPageSettingsHandler PageSettings { get; }

        /// <inheritdoc cref="PrintPageEventArgs.DrawingContext"/>
        Graphics DrawingContext { get; }

        /// <inheritdoc cref="PrintPageEventArgs.HasMorePages"/>
        bool HasMorePages { get; set; }

        /// <inheritdoc cref="PrintPageEventArgs.MarginBounds"/>
        RectD MarginBounds { get; }

        /// <inheritdoc cref="PrintPageEventArgs.PhysicalPageBounds"/>
        RectD PhysicalPageBounds { get; }

        /// <inheritdoc cref="PrintPageEventArgs.PageBounds"/>
        RectD PageBounds { get; }

        /// <inheritdoc cref="PrintPageEventArgs.PrintablePageBounds"/>
        RectD PrintablePageBounds { get; }

        /// <inheritdoc cref="PrintPageEventArgs.PageNumber"/>
        int PrintedPageNumber { get; }

        /// <inheritdoc cref="PrintDocument.Print"/>
        void Print();
    }
}
