using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.UI.Native
{
    internal partial class PrintDialog : Alternet.Drawing.Printing.IPrintDialogHandler
    {
        private string? title;

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

        string? IDialogHandler.GetTitle()
        {
            return title;
        }

        void IDialogHandler.SetTitle(string? value)
        {
            title = value;
        }
    }
}
