using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    internal class PageSetupDialogHandler : UI.Native.PageSetupDialog, IPageSetupDialogHandler
    {
        Thickness? IPageSetupDialogHandler.MinMargins
        {
            get
            {
                return MinMarginsValueSet ? MinMargins : null;
            }

            set
            {
                MinMarginsValueSet = value != null;

                if (value != null)
                    MinMargins = value.Value;
            }
        }

        bool ICustomPrintDialogHandler.ShowHelp { get; set; }

        string? ICustomPrintDialogHandler.Title { get; set; }

        void ICustomPrintDialogHandler.SetDocument(IPrintDocumentHandler? value)
        {
            Document = value as PrintDocumentHandler;
        }

        ModalResult ICustomPrintDialogHandler.ShowModal(Window? owner)
        {
            var nativeOwner = owner == null ? null
                : ((WindowHandler)owner.Handler).NativeControl;
            return (ModalResult)ShowModal(nativeOwner);
        }
    }
}
