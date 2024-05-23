using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing.Printing;

namespace Alternet.UI
{
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

        [Browsable(false)]
        public new IBasePrintDialogHandler Handler => (IBasePrintDialogHandler)base.Handler;

        /// <inheritdoc/>
        public override ModalResult ShowModal(Window? owner)
        {
            if (Document == null)
            {
                BaseApplication.Alert("Cannot show the dialog when the Document is null.");
                return ModalResult.Canceled;
            }

            CheckDisposed();
            return Handler.ShowModal(owner);
        }
    }
}
