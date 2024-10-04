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
    /// the <see cref="BasePrintDialog.Document"/>, <see cref="PrinterSettings"/>, or
    /// <see cref="PageSettings"/> property before showing the dialog.
    /// </para>
    /// </remarks>
    public class PageSetupDialog : BasePrintDialog
    {
        /// <summary>
        /// Gets default <see cref="PageSetupDialog"/> instance.
        /// </summary>
        public static PageSetupDialog Default = defaultDialog ??= new PageSetupDialog();

        private static PageSetupDialog? defaultDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PageSetupDialog"/>.
        /// </summary>
        public PageSetupDialog()
        {
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

        /// <summary>
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new IPageSetupDialogHandler Handler => (IPageSetupDialogHandler)base.Handler;

        /// <inheritdoc/>
        protected override IDialogHandler CreateHandler()
        {
            return PrintingFactory.Handler.CreatePageSetupDialogHandler();
        }
    }
}