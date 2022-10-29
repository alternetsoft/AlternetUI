using Alternet.Drawing.Printing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Enables users to change page-related print settings, including margins and paper orientation. This class cannot
    /// be inherited.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="PageSetupDialog"/> dialog box modifies the <see cref="PageSettings"/> and
    /// <see cref="PrinterSettings"/> information for a given Document.
    /// The user can enable sections of the dialog box to manipulate printing and margins; paper orientation, size, and
    /// source; and to show Help button. The <see cref="MinMargins"/> property defines the minimum margins a user can
    /// select.
    /// </para>
    /// <para>
    /// When you create an instance of the <see cref="PageSetupDialog"/> class, the read/write properties are set to initial values.
    /// </para>
    /// <para>
    /// Because a <see cref="PageSetupDialog"/> needs page settings to display, you need to set the <see cref="Document"/>, <see cref="PrinterSettings"/>, or
    /// <see cref="PageSettings"/> property before calling <see cref="CommonDialog.ShowModal"/>.
    /// </para>
    /// </remarks>
    public sealed class PageSetupDialog : CommonDialog
    {
        /// <summary>
        /// Gets or sets the printer settings that are modified in the dialog.
        /// </summary>
        public PrinterSettings PrinterSettings { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating the page settings to modify.
        /// </summary>
        public PageSettings PageSettings { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating the <see cref="PrintDocument"/> to get page settings from.
        /// </summary>
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating the minimum margins, in millimeters, the user is allowed to select.
        /// </summary>
        public Margins? MinMargins { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the margins section of the dialog box is enabled.
        /// </summary>
        public bool AllowMargins { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the orientation section of the dialog box (landscape versus
        /// portrait) is enabled.
        /// </summary>
        public bool AllowOrientation { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the paper section of the dialog box (paper size and paper source) is
        /// enabled.
        /// </summary>
        public bool AllowPaper { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Printer button</b> is enabled.
        /// </summary>
        public bool AllowPrinter { get => throw new Exception(); set => throw new Exception(); }


        private Native.PageSetupDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PageSetupDialog"/>.
        /// </summary>
        public PageSetupDialog()
        {
            nativeDialog = new Native.PageSetupDialog();
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