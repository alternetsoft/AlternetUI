using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class FileDialog
        : Alternet.UI.IOpenFileDialogHandler, Alternet.UI.ISaveFileDialogHandler
    {
        public bool ShowHelp { get; set; }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            return ShowModal(GetNativeWindow(owner));
        }

        public void ShowAsync(Alternet.UI.Window? owner, Action<bool>? onClose)
        {
            ColorDialog.DefaultShowAsync(owner, onClose, ShowModal);
        }
    }
}