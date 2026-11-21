using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    internal class WxPrintingHandler : DisposableObject, IPrintingHandler
    {
        public IPrinterSettingsHandler CreatePrinterSettingsHandler()
        {
            return new UI.Native.PrinterSettings();
        }

        public IPageSettingsHandler CreatePageSettingsHandler()
        {
            return new UI.Native.PageSettings();
        }

        public IPrintDocumentHandler CreatePrintDocumentHandler()
        {
            return new PrintDocumentHandler();
        }

        public IPrintDialogHandler CreatePrintDialogHandler()
        {
            return new UI.Native.PrintDialog();
        }

        public IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler()
        {
            return new UI.Native.PrintPreviewDialog();
        }

        public IPageSetupDialogHandler CreatePageSetupDialogHandler()
        {
            return new UI.Native.PageSetupDialog();
        }
    }
}
