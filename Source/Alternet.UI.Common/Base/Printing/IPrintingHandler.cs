using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing.Printing
{
    public interface IPrintingHandler : IDisposable
    {
        IPrintDocumentHandler CreatePrintDocumentHandler();

        IPrinterSettingsHandler CreatePrinterSettingsHandler();

        IPrintDialogHandler CreatePrintDialogHandler();

        IPageSettingsHandler CreatePageSettingsHandler();

        IPageSetupDialogHandler CreatePageSetupDialogHandler();

        IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler();
    }
}
