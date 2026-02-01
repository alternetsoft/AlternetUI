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
    public class PrintDialog : BasePrintDialog
    {
        /// <summary>
        /// Gets default <see cref="PrintDialog"/> instance.
        /// </summary>
        public static PrintDialog Default = defaultDialog ??= new();

        private static PrintDialog? defaultDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PrintDialog"/>.
        /// </summary>
        public PrintDialog()
        {
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
        /// Gets dialog handler.
        /// </summary>
        [Browsable(false)]
        public new IPrintDialogHandler Handler => (IPrintDialogHandler)base.Handler;

        /// <summary>
        /// Writes the current print dialog settings to the specified log writer.
        /// </summary>
        /// <param name="logWriter">The log writer to which the print dialog settings are written.
        /// If null, a default debug log writer is used.</param>
        public virtual void Log(ILogWriter? logWriter = null)
        {
            logWriter ??= LogWriter.Debug;

            logWriter.BeginSection("PrintDialog");

            logWriter.WriteKeyValue("AllowSomePages", AllowSomePages);
            logWriter.WriteKeyValue("AllowSelection", AllowSelection);
            logWriter.WriteKeyValue("AllowPrintToFile", AllowPrintToFile);

            Document?.Log(logWriter);

            logWriter.EndSection();
        }

        /// <summary>
        /// Creates <see cref="IDialogHandler"/> object used in this dialog.
        /// </summary>
        /// <returns></returns>
        protected override IDialogHandler CreateHandler()
        {
            return PrintingFactory.Handler.CreatePrintDialogHandler();
        }
    }
}