using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    internal class PrintDialogHandler : UI.Native.PrintDialog, IPrintDialogHandler
    {
        string? ICustomPrintDialogHandler.Title { get; set; }

        void ICustomPrintDialogHandler.SetDocument(IPrintDocumentHandler? value)
        {
            Document = value as PrintDocumentHandler;
        }

        ModalResult ICustomPrintDialogHandler.ShowModal(Window? owner)
        {
            var nativeOwner = owner == null
                ? null : ((WindowHandler)owner.Handler).NativeControl;
            return (ModalResult)ShowModal(nativeOwner);
        }
    }
}
