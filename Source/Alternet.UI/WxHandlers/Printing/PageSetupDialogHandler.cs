using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.UI.Native
{
    internal partial class PageSetupDialog : Alternet.Drawing.Printing.IPageSetupDialogHandler
    {
        Thickness? Alternet.Drawing.Printing.IPageSetupDialogHandler.MinMargins
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

        public bool ShowHelp { get; set; }

        public string? Title { get; set; }

        public void ShowAsync(Alternet.UI.Window? owner, Action<bool>? onClose)
        {
            ColorDialog.DefaultShowAsync(owner, onClose, ShowModal);
        }

        public void SetDocument(Alternet.Drawing.Printing.IPrintDocumentHandler? value)
        {
            Document = value as Alternet.Drawing.Printing.PrintDocumentHandler;
        }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            return ShowModal(GetNativeWindow(owner));
        }
    }
}
