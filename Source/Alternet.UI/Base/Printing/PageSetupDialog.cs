using System;
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
    public sealed class PageSetupDialog : CommonDialog
    {
        private readonly Native.PageSetupDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PageSetupDialog"/>.
        /// </summary>
        public PageSetupDialog()
        {
            nativeDialog = new Native.PageSetupDialog();
        }

        /// <summary>
        /// Gets or sets a value indicating the <see cref="PrintDocument"/> to get page settings from.
        /// </summary>
        public PrintDocument? Document
        {
            get
            {
                return nativeDialog.Document == null ? null : new PrintDocument(nativeDialog.Document);
            }

            set
            {
                nativeDialog.Document = value?.NativePrintDocument;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the minimum margins, in millimeters, the user is
        /// allowed to select.
        /// </summary>
        public Thickness? MinMargins
        {
            get
            {
                return nativeDialog.MinMarginsValueSet ? nativeDialog.MinMargins : null;
            }

            set
            {
                nativeDialog.MinMarginsValueSet = value != null;

                if (value != null)
                    nativeDialog.MinMargins = value.Value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the margins section of the dialog box is enabled.
        /// </summary>
        public bool AllowMargins
        {
            get
            {
                return nativeDialog.AllowMargins;
            }

            set
            {
                nativeDialog.AllowMargins = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the orientation section of the dialog box
        /// (landscape versus
        /// portrait) is enabled.
        /// </summary>
        public bool AllowOrientation
        {
            get
            {
                return nativeDialog.AllowOrientation;
            }

            set
            {
                nativeDialog.AllowOrientation = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the paper section of the dialog box
        /// (paper size and paper source) is
        /// enabled.
        /// </summary>
        public bool AllowPaper
        {
            get
            {
                return nativeDialog.AllowPaper;
            }

            set
            {
                nativeDialog.AllowPaper = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Printer button</b> is enabled.
        /// </summary>
        public bool AllowPrinter
        {
            get
            {
                return nativeDialog.AllowPrinter;
            }

            set
            {
                nativeDialog.AllowPrinter = value;
            }
        }

        private protected override string? TitleCore { get; set; }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            var nativeOwner = owner == null ? null
                : ((WindowHandler)owner.Handler).NativeControl;
            return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }
    }
}