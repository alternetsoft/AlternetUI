using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    internal class PrintPreviewDialogHandler : UI.Native.PrintPreviewDialog, IPrintPreviewDialogHandler
    {
        bool ICustomPrintDialogHandler.ShowHelp { get; set; }

        void ICustomPrintDialogHandler.SetDocument(IPrintDocumentHandler? value)
        {
            Document = value as PrintDocumentHandler;
        }

        ModalResult ICustomPrintDialogHandler.ShowModal(Window? owner)
        {
            var nativeOwner = owner == null
                ? null : ((WindowHandler)owner.Handler).NativeControl;
            ShowModal(nativeOwner);
            return ModalResult.Accepted;
        }
    }
}
