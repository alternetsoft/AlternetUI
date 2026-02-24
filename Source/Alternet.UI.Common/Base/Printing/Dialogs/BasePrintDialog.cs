using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all print dialogs.
    /// </summary>
    public class BasePrintDialog : CommonDialog
    {
        private PrintDocument? document;

        /// <summary>
        /// Gets or sets <see cref="PrintDocument"/> used in the dialog.
        /// </summary>
        [Browsable(false)]
        public virtual PrintDocument? Document
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
        /// Gets attached <see cref="IBasePrintDialogHandler"/> object.
        /// </summary>
        [Browsable(false)]
        public new IBasePrintDialogHandler Handler => (IBasePrintDialogHandler)base.Handler;

        /// <inheritdoc/>
        protected override bool IsValidShowDialog()
        {
            if (PrinterUtils.HasPrinters() == false)
            {
                App.Alert("No printers are installed on the system.");
                return false;
            }

            if (Document == null)
            {
                App.Alert("Cannot show the dialog when the Document is null.");
                return false;
            }

            return true;
        }
    }
}
