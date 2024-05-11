using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing.Printing
{
    public interface IPrintDocumentHandler : IDisposable
    {
        event EventHandler<CancelEventArgs>? PrintPage;

        event EventHandler<CancelEventArgs>? BeginPrint;

        event EventHandler<CancelEventArgs>? EndPrint;

        bool OriginAtMargins { get; set; }

        string DocumentName { get; set; }

        IPrinterSettingsHandler PrinterSettings { get; }

        IPageSettingsHandler PageSettings { get; }

        Graphics DrawingContext { get; }

        bool HasMorePages { get; set; }

        RectD MarginBounds { get; }

        RectD PhysicalPageBounds { get; }

        RectD PageBounds { get; }

        RectD PrintablePageBounds { get; }

        int PrintedPageNumber { get; }

        void Print();
    }
}
