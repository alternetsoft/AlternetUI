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
        
        string[] IOpenFileDialogHandler.FileNames
        {
            get
            {
                var nativeFileNames = NativeUtils.ToStringArray(FileNames);
                return nativeFileNames;
            }
        }

        public Alternet.UI.ModalResult ShowModal(Alternet.UI.Window? owner)
        {
            return ShowModal(GetNativeWindow(owner));
        }

        public void ShowAsync(Alternet.UI.Window? owner, Action<bool>? onClose)
        {
            ColorDialog.DefaultShowAsync(owner, onClose, ShowModal);
        }

        string? IFileDialogHandler.GetInitialDirectory()
        {
            return GetInitialDirectory();
        }

        void IFileDialogHandler.SetInitialDirectory(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetInitialDirectory(s));
        }

        string? IFileDialogHandler.GetFilter()
        {
            return GetFilter();
        }

        void IFileDialogHandler.SetFilter(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetFilter(s));
        }

        string? IFileDialogHandler.GetFileName()
        {
            return GetFileName();
        }

        void IFileDialogHandler.SetFileName(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetFileName(s));
        }

        string? IDialogHandler.GetTitle()
        {
            return GetTitle();
        }

        void IDialogHandler.SetTitle(string? value)
        {
            NativeStringSpan.Invoke(value, s => SetTitle(s));
        }
    }
}