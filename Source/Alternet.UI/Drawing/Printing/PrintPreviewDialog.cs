using Alternet.Drawing.Printing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a dialog box form that contains a preview for printing from an AlterNET UI application.
    /// </summary>
    public sealed class PrintPreviewDialog : CommonDialog
    {
        /// <summary>
        /// Gets or sets the document to preview.
        /// </summary>
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }

        private Native.PrintPreviewDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PrintPreviewDialog"/>.
        /// </summary>
        public PrintPreviewDialog()
        {
            nativeDialog = new Native.PrintPreviewDialog();
        }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            return ModalResult.Canceled;
            //var nativeOwner = owner == null ? null : ((NativeWindowHandler)owner.Handler).NativeControl;
            //return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }

        private protected override string? TitleCore
        {
            get
            {
                CheckDisposed();
                return null;
                //return nativeDialog.Title;
            }

            set
            {
                CheckDisposed();
                //nativeDialog.Title = value;
            }
        }
    }
}