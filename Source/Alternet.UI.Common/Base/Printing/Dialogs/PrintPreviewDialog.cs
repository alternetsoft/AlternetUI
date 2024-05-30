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
    public class PrintPreviewDialog : BasePrintDialog
    {
        /// <summary>
        /// Gets default <see cref="PrintPreviewDialog"/> instance.
        /// </summary>
        public static PrintPreviewDialog Default = defaultDialog ??= new();

        private static PrintPreviewDialog? defaultDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPreviewDialog"/> class.
        /// </summary>
        public PrintPreviewDialog()
        {
        }

        [Browsable(false)]
        public new IPrintPreviewDialogHandler Handler => (IPrintPreviewDialogHandler)base.Handler;

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return PrintingFactory.Handler.CreatePrintPreviewDialogHandler();
        }
    }
}