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
