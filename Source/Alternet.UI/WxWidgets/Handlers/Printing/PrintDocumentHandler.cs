using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    internal class PrintDocumentHandler : UI.Native.PrintDocument, IPrintDocumentHandler
    {
        RectD IPrintDocumentHandler.MarginBounds => PrintPage_MarginBounds;

        RectD IPrintDocumentHandler.PhysicalPageBounds => PrintPage_PhysicalPageBounds;

        RectD IPrintDocumentHandler.PageBounds => PrintPage_PageBounds;

        RectD IPrintDocumentHandler.PrintablePageBounds => PrintPage_PrintablePageBounds;

        int IPrintDocumentHandler.PrintedPageNumber => PrintPage_PageNumber;

        bool IPrintDocumentHandler.HasMorePages
        {
            get => PrintPage_HasMorePages;
            set => PrintPage_HasMorePages = value;
        }

        IPrinterSettingsHandler IPrintDocumentHandler.PrinterSettings => PrinterSettings;

        IPageSettingsHandler IPrintDocumentHandler.PageSettings => PageSettings;

        Graphics IPrintDocumentHandler.DrawingContext
        {
            get
            {
                return new WxGraphics(PrintPage_DrawingContext);
            }
        }
    }
}
