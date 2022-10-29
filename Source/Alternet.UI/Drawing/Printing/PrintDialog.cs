using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Lets users select a printer and choose which sections of the document to print from an AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// When you create an instance of <see cref="PrintDialog"/>, the read/write properties are set to initial values.
    /// To get printer settings that are modified by the user with the
    /// <see cref="PrintDialog"/>, use the <see cref="PrinterSettings"/> property.
    /// </remarks>
    public sealed class PrintDialog : CommonDialog
    {
        private Native.PrintDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PrintDialog"/>.
        /// </summary>
        public PrintDialog()
        {
            nativeDialog = new Native.PrintDialog();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Pages</b> option button is enabled.
        /// </summary>
        public bool AllowSomePages
        {
            get
            {
                return nativeDialog.AllowSomePages;
            }

            set
            {
                nativeDialog.AllowSomePages = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Selection</b> option button is enabled.
        /// </summary>
        public bool AllowSelection
        {
            get
            {
                return nativeDialog.AllowSelection;
            }

            set
            {
                nativeDialog.AllowSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Print to file</b> option button is enabled.
        /// </summary>
        public bool AllowPrintToFile
        {
            get
            {
                return nativeDialog.AllowPrintToFile;
            }

            set
            {
                nativeDialog.AllowPrintToFile = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the the <b>Help</b> button is displayed.
        /// </summary>
        public bool ShowHelp
        {
            get
            {
                return nativeDialog.ShowHelp;
            }

            set
            {
                nativeDialog.ShowHelp = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the PrintDocument used to obtain <see cref="PrinterSettings"/>.
        /// </summary>
        /// <value>The <see cref="PrintDocument"/> used to obtain <see cref="PrinterSettings"/>. The default is <see langword="null"/>.</value>
        public PrintDocument? Document
        {
            get
            {
                return nativeDialog.Document == null ? null : new PrintDocument(nativeDialog.Document);
            }

            set
            {
                nativeDialog.Document = value == null ? null : value.NativePrintDocument;
            }
        }

        /// <summary>
        /// Gets or sets the printer settings the dialog box modifies.
        /// </summary>
        public PrinterSettings PrinterSettings
        {
            get
            {
                return new PrinterSettings(nativeDialog.PrinterSettings);
            }

            set
            {
                nativeDialog.PrinterSettings = value.NativePrinterSettings;
            }
        }

        private protected override string? TitleCore { get; set; }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            var nativeOwner = owner == null ? null : ((NativeWindowHandler)owner.Handler).NativeControl;
            return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }
    }
}