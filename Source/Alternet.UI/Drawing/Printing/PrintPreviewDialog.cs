using Alternet.Drawing.Printing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a dialog box form that contains a preview for printing from an AlterNET UI application.
    /// </summary>
    public class PrintPreviewDialog : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPreviewDialog"/> class.
        /// </summary>
        public PrintPreviewDialog() : this(new Native.PrintPreviewDialog())
        {
        }

        internal PrintPreviewDialog(Native.PrintPreviewDialog nativePrintPreviewDialog)
        {
            NativePrintPreviewDialog = nativePrintPreviewDialog;
        }

        /// <summary>
        /// Gets or sets the document to preview.
        /// </summary>
        public PrintDocument? Document
        {
            get
            {
                return NativePrintPreviewDialog.Document == null ? null : new PrintDocument(NativePrintPreviewDialog.Document);
            }

            set
            {
                NativePrintPreviewDialog.Document = value == null ? null : value.NativePrintDocument;
            }
        }

        /// <summary>
        /// Gets or sets the title of this <see cref="PrintPreviewDialog"/>.
        /// </summary>
        public string? Title
        {
            get => NativePrintPreviewDialog.Title;
            set => NativePrintPreviewDialog.Title = value;
        }

        internal Native.PrintPreviewDialog NativePrintPreviewDialog { get; private set; }

        /// <summary>
        /// Releases all resources used by the <see cref="PrintDocument"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Shows the <see cref="PrintPreviewDialog"/> with the specified optional owner window.
        /// </summary>
        /// <param name="owner">The owner window for the dialog.</param>
        /// <exception cref="InvalidOperationException">The <see cref="Document"/> property value is <see langword="null"/>.</exception>
        public void Show(Window? owner = null)
        {
            if (NativePrintPreviewDialog.Document == null)
                throw new InvalidOperationException("Cannot show the print preview dialog when the Document property value is null.");

            var nativeOwner = owner == null ? null : ((NativeWindowHandler)owner.Handler).NativeControl;
            NativePrintPreviewDialog.Show(nativeOwner);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="PrintPreviewDialog"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativePrintPreviewDialog.Dispose();
                    NativePrintPreviewDialog = null!;
                }

                isDisposed = true;
            }
        }
    }
}