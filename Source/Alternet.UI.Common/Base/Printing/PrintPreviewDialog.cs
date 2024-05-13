using System;
using System.ComponentModel;

using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a dialog box form that contains a preview for printing
    /// from an Alternet.UI application.
    /// </summary>
    [ControlCategory("Printing")]
    public class PrintPreviewDialog : CommonDialog, IDisposable
    {
        private PrintDocument? document;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPreviewDialog"/> class.
        /// </summary>
        public PrintPreviewDialog()
        {
            Handler = NativePlatform.Default.CreatePrintPreviewDialogHandler();
        }

        /// <summary>
        /// Gets or sets the document to preview.
        /// </summary>
        [Browsable(false)]
        public PrintDocument? Document
        {
            get
            {
                return document;
            }

            set
            {
                if (document == value)
                    return;
                document = value;
                Handler.SetDocument(value?.Handler);
            }
        }

        /// <summary>
        /// Gets or sets the title of this <see cref="PrintPreviewDialog"/>.
        /// </summary>
        public override string? Title
        {
            get => Handler.Title;
            set => Handler.Title = value;
        }

        internal IPrintPreviewDialogHandler Handler { get; private set; }

        /// <summary>
        /// Shows the <see cref="PrintPreviewDialog"/> with the specified optional owner window.
        /// </summary>
        /// <param name="owner">The owner window for the dialog.</param>
        /// <exception cref="InvalidOperationException">The <see cref="Document"/> property
        /// value is <see langword="null"/>.</exception>
        public override ModalResult ShowModal(Window? owner = null)
        {
            if (Document == null)
            {
                BaseApplication.Alert("Cannot show the print preview dialog when the Document is null.");
                return ModalResult.Canceled;
            }

            return Handler.ShowModal(owner);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            Handler?.Dispose();
            Handler = null!;
        }
    }
}