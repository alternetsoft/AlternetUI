using System;
using System.ComponentModel;

using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Enables users to change page-related print settings, including margins and paper
    /// orientation. This class cannot
    /// be inherited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="PageSetupDialog"/> dialog box modifies the <see cref="PageSettings"/> and
    /// <see cref="PrinterSettings"/> information for a given Document.
    /// The user can enable sections of the dialog box to manipulate printing and margins;
    /// paper orientation, size, and
    /// source; and to show Help button. The <see cref="MinMargins"/> property defines the
    /// minimum margins a user can
    /// select.
    /// </para>
    /// <para>
    /// When you create an instance of the <see cref="PageSetupDialog"/> class, the read/write
    /// properties are set to initial values.
    /// </para>
    /// <para>
    /// Because a <see cref="PageSetupDialog"/> needs page settings to display, you need to set
    /// the <see cref="Document"/>, <see cref="PrinterSettings"/>, or
    /// <see cref="PageSettings"/> property before calling <see cref="CommonDialog.ShowModal()"/>.
    /// </para>
    /// </remarks>
    public class PageSetupDialog : CommonDialog
    {
        private PrintDocument? document;

        /// <summary>
        /// Initializes a new instance of <see cref="PageSetupDialog"/>.
        /// </summary>
        public PageSetupDialog()
        {
            Handler = NativePlatform.Default.CreatePageSetupDialogHandler();
        }

        /// <summary>
        /// Gets or sets a value indicating the <see cref="PrintDocument"/> to get page settings from.
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
        /// Gets or sets a value indicating the minimum margins, in millimeters, the user is
        /// allowed to select.
        /// </summary>
        public virtual Thickness? MinMargins
        {
            get
            {
                return Handler.MinMargins;
            }

            set
            {
                Handler.MinMargins = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the margins section of the dialog box is enabled.
        /// </summary>
        public virtual bool AllowMargins
        {
            get
            {
                return Handler.AllowMargins;
            }

            set
            {
                Handler.AllowMargins = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the orientation section of the dialog box
        /// (landscape versus
        /// portrait) is enabled.
        /// </summary>
        public virtual bool AllowOrientation
        {
            get
            {
                return Handler.AllowOrientation;
            }

            set
            {
                Handler.AllowOrientation = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the paper section of the dialog box
        /// (paper size and paper source) is
        /// enabled.
        /// </summary>
        public virtual bool AllowPaper
        {
            get
            {
                return Handler.AllowPaper;
            }

            set
            {
                Handler.AllowPaper = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Printer button</b> is enabled.
        /// </summary>
        public virtual bool AllowPrinter
        {
            get
            {
                return Handler.AllowPrinter;
            }

            set
            {
                Handler.AllowPrinter = value;
            }
        }

        /// <inheritdoc/>
        public override string? Title { get; set; }

        internal IPageSetupDialogHandler Handler { get; private set; }

        /// <inheritdoc/>
        public override ModalResult ShowModal(Window? owner)
        {
            if (Document == null)
            {
                BaseApplication.Alert("Cannot show the PageSetup dialog when the Document is null.");
                return ModalResult.Canceled;
            }

            CheckDisposed();
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