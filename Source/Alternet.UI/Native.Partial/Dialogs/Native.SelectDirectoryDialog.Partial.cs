using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class SelectDirectoryDialog : Alternet.UI.ISelectDirectoryDialogHandler
    {
        public bool ShowHelp { get; set; }

        public void ShowAsync(Alternet.UI.Window? owner, Action<bool>? onClose)
        {
            ColorDialog.DefaultShowAsync(owner, onClose, ShowModal);
        }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            return ShowModal(GetNativeWindow(owner));
        }

        string? ISelectDirectoryDialogHandler.GetDirectoryName()
        {
            return GetDirectoryName();
        }

        string? ISelectDirectoryDialogHandler.GetInitialDirectory()
        {
            return GetInitialDirectory();
        }

        string? IDialogHandler.GetTitle()
        {
            return GetTitle();
        }

        void ISelectDirectoryDialogHandler.SetDirectoryName(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetDirectoryName(s));

        }

        void ISelectDirectoryDialogHandler.SetInitialDirectory(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetInitialDirectory(s));
        }

        void IDialogHandler.SetTitle(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetTitle(s));
        }
    }
}