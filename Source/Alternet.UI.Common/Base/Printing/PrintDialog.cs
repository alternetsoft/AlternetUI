using System.ComponentModel;

using Alternet.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Lets users select a printer and choose which sections of the document to print from
    /// an Alternet.UI application.
    /// </summary>
    /// <remarks>
    /// When you create an instance of <see cref="PrintDialog"/>, the read/write properties are
    /// set to initial values.
    /// To get printer settings that are modified by the user with the
    /// <see cref="PrintDialog"/>, use the <see cref="PrinterSettings"/> property.
    /// </remarks>
    [ControlCategory("Printing")]
    public class PrintDialog : CommonDialog
    {
        private PrintDocument? document;

        /// <summary>
        /// Initializes a new instance of <see cref="PrintDialog"/>.
        /// </summary>
        public PrintDialog()
        {
            Handler = NativePlatform.Default.CreatePrintDialogHandler();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Pages</b> option button is enabled.
        /// </summary>
        public virtual bool AllowSomePages
        {
            get
            {
                return Handler.AllowSomePages;
            }

            set
            {
                Handler.AllowSomePages = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Selection</b> option button is enabled.
        /// </summary>
        public virtual bool AllowSelection
        {
            get
            {
                return Handler.AllowSelection;
            }

            set
            {
                Handler.AllowSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Print to file</b> option button is enabled.
        /// </summary>
        public virtual bool AllowPrintToFile
        {
            get
            {
                return Handler.AllowPrintToFile;
            }

            set
            {
                Handler.AllowPrintToFile = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the the <b>Help</b> button is displayed.
        /// </summary>
        public virtual bool ShowHelp
        {
            get
            {
                return Handler.ShowHelp;
            }

            set
            {
                Handler.ShowHelp = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the PrintDocument used to obtain
        /// <see cref="PrinterSettings"/>.
        /// </summary>
        /// <value>The <see cref="PrintDocument"/> used to obtain <see cref="PrinterSettings"/>.
        /// The default is <see langword="null"/>.</value>
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

        /// <inheritdoc/>
        public override string? Title
        {
            get => Handler.Title;
            set => Handler.Title = value;
        }

        internal IPrintDialogHandler Handler { get; private set; }

        /// <inheritdoc/>
        public override ModalResult ShowModal(Window? owner)
        {
            if (Document == null)
            {
                BaseApplication.Alert("Cannot show the print dialog when the Document is null.");
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