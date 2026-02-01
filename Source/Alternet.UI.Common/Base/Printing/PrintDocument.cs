using System;
using System.ComponentModel;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Defines a reusable object that sends output to a printer, when printing from
    /// an AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// Typically, you create an instance of the <see cref="PrintDocument"/> class,
    /// set properties such as the
    /// <see cref="DocumentName"/> and <see cref="PrinterSettings"/>, and call
    /// the <see cref="Print"/> method to start the
    /// printing process. Handle the <see cref="PrintPage"/> event where you
    /// specify the output to print, by using the
    /// <see cref="PrintPageEventArgs.DrawingContext"/> property of the
    /// <see cref="PrintPageEventArgs"/>.
    /// </remarks>
    public class PrintDocument : HandledObject<IPrintDocumentHandler>
    {
        private Graphics? currentDrawingContext;
        private PrinterSettings? printerSettings;
        private PageSettings? pageSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDocument"/> class.
        /// </summary>
        public PrintDocument()
        {
        }

        /// <summary>
        /// Occurs when the output to print for the current page is needed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// To specify the output to print, use the <see cref="PrintPageEventArgs.DrawingContext"/>
        /// property of the <see
        /// cref="PrintPageEventArgs"/>. For example, to specify a line of text that should be
        /// printed, draw the text
        /// using the <see cref="Graphics.DrawText(ReadOnlySpan{char}, Font, Brush, PointD)"/> method.
        /// </para>
        /// <para>
        /// In addition to specifying the output, you can indicate if there are additional pages
        /// to print by setting the
        /// <see cref="PrintPageEventArgs.HasMorePages"/> property to <see langword="true"/>.
        /// The default is <see
        /// langword="false"/>, which indicates that there are no more pages to print. Individual
        /// page settings can also
        /// be modified through the <see cref="Printing.PageSettings"/> and the print job can be
        /// canceled by setting the
        /// <see cref="CancelEventArgs.Cancel"/> property of the <see cref="PrintPageEventArgs"/>
        /// object to
        /// <see langword="true"/>.
        /// </para>
        /// </remarks>
        public event EventHandler<PrintPageEventArgs>? PrintPage;

        /// <summary>
        /// Occurs when the <see cref="Print()"/> method is called and before the first page of
        /// the document prints.
        /// </summary>
        /// <remarks>
        /// Typically, you handle the <see cref="BeginPrint"/> event to initialize fonts, file
        /// streams,
        /// and other resources used during the printing process.
        /// </remarks>
        public event EventHandler<PrintEventArgs>? BeginPrint;

        /// <summary>
        /// Occurs when the last page of the document has printed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Typically, you handle the <see cref="EndPrint"/> event to release fonts, file
        /// streams, and other resources
        /// used during the printing process, like fonts.
        /// </para>
        /// <para>
        /// You indicate that there are no more pages to print by setting the
        /// <see cref="PrintPageEventArgs.HasMorePages"/> property
        /// to false in the <see cref="PrintPage"/> event. The <see cref="EndPrint"/>
        /// event also occurs if the printing process is canceled or an
        /// exception occurs during the printing process.
        /// </para>
        /// </remarks>
        public event EventHandler<PrintEventArgs>? EndPrint;

        /// <summary>
        /// Gets or sets the document name to display (for example, in a print status dialog
        /// box or printer queue) while printing the document.
        /// </summary>
        /// <value>
        /// The document name to display while printing the document.
        /// </value>
        public virtual string DocumentName
        {
            get
            {
                return Handler.DocumentName;
            }

            set
            {
                Handler.DocumentName = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the position of a graphics object
        /// associated with a page is located
        /// just inside the user-specified margins or at the top-left corner of the
        /// printable area of the page.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the graphics origin starts at the page margins;
        /// <see langword="false"/> if the graphics origin is at the top-left
        /// corner of the printable page. The default is false.
        /// </value>
        public virtual bool OriginAtMargins
        {
            get
            {
                return Handler.OriginAtMargins;
            }

            set
            {
                Handler.OriginAtMargins = value;
            }
        }

        /// <summary>
        /// Gets the printer settings for this document.
        /// </summary>
        /// <value>
        /// A <see cref="PrinterSettings"/> that specifies where and how the document
        /// is printed.
        /// The default is a <see cref="PrinterSettings"/> with its properties set to
        /// their default values.
        /// </value>
        public virtual PrinterSettings PrinterSettings
        {
            get
            {
                return printerSettings ??= new PrinterSettings(Handler.PrinterSettings);
            }
        }

        /// <summary>
        /// Gets or sets page settings that are used for all pages to be printed.
        /// </summary>
        /// <value>
        /// A <see cref="Printing.PageSettings"/> that specifies the page settings for the document.
        /// </value>
        /// <remarks>
        /// You can specify several page settings through the <see cref="PageSettings"/> property.
        /// For example, the
        /// <see cref="PageSettings.Color"/> property specifies whether the page prints in color,
        /// the <see cref="PageSettings.Landscape"/> property
        /// specifies landscape or portrait orientation, and the <see cref="PageSettings.Margins"/>
        /// property specifies the margins of
        /// the page.
        /// </remarks>
        public virtual PageSettings PageSettings
        {
            get
            {
                return pageSettings ??= new PageSettings(Handler.PageSettings);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a print operation is currently in progress.
        /// </summary>
        [Browsable(false)]
        public bool IsCurrentPrinting
        {
            get
            {
                return currentDrawingContext != null;
            }
        }

        /// <summary>
        /// Gets the current graphics drawing context used for printing operations.
        /// </summary>
        /// <remarks>The returned <see cref="Graphics"/> instance may be <see langword="null"/> if no
        /// drawing context is currently available. This property is typically used to perform custom drawing within the
        /// rendering surface.</remarks>
        [Browsable(false)]
        public Graphics? CurrentDrawingContext
        {
            get
            {
                return currentDrawingContext;
            }
        }

        /// <summary>
        /// Throws an exception if a printing operation is currently in progress.
        /// </summary>
        /// <remarks>Call this method before starting a new printing operation to ensure that no other
        /// printing process is currently running. This helps prevent conflicts or resource contention caused by
        /// overlapping print jobs.</remarks>
        /// <exception cref="InvalidOperationException">Thrown if a printing operation is active when this method is called.</exception>
        public virtual void ThrowIfPrintingInProgress()
        {
            if (IsCurrentPrinting)
                throw new InvalidOperationException("A printing operation is in progress.");
        }

        /// <summary>
        /// Starts the document's printing process.
        /// </summary>
        /// <remarks>
        /// Specify the output to print by handling the <see cref="PrintPage"/> event and by
        /// using the
        /// <see cref="PrintPageEventArgs.DrawingContext"/> included in the
        /// <see cref="PrintPageEventArgs"/>.
        /// </remarks>
        public virtual void Print()
        {
            ThrowIfPrintingInProgress();
            Handler.Print();
        }

        /// <summary>
        /// Logs the document, page and printer settings using the specified log writer.
        /// </summary>
        /// <param name="log">The log writer to which settings information will be written.
        /// If not specified, debug output will be used.
        /// </param>
        public virtual void Log(ILogWriter? log = null)
        {
            log ??= LogWriter.Debug;

            log.BeginSection("PrintDocument");

            log.WriteKeyValue("DocumentName", DocumentName);
            log.WriteKeyValue("OriginAtMargins", OriginAtMargins);

            PageSettings.Log(log);
            PrinterSettings.Log(log);

            log.EndSection();
        }

        /// <summary>
        /// Raises the <see cref="BeginPrint"/> event. It is called after the
        /// <see cref="Print()"/> method is called and
        /// before the first page of the document prints.
        /// </summary>
        /// <param name="e">A <see cref="PrintEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// The <see cref="OnBeginPrint"/> method allows derived classes to handle the
        /// event without attaching a
        /// delegate. This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        protected virtual void OnBeginPrint(PrintEventArgs e)
        {
            BeginPrint?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="EndPrint"/> event. It is called when the last page of the
        /// document has printed.
        /// </summary>
        /// <param name="e">A <see cref="PrintEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// The <see cref="OnEndPrint"/> method allows derived classes to handle the event
        /// without attaching a delegate. This is the
        /// preferred technique for handling the event in a derived class. The
        /// <see cref="OnEndPrint"/> method is also called if the
        /// printing process is canceled or an exception occurs during the printing process.
        /// </remarks>
        protected virtual void OnEndPrint(PrintEventArgs e)
        {
            EndPrint?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PrintPage"/> event. It is called before a page prints.
        /// </summary>
        /// <param name="e">A <see cref="PrintPageEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// The <see cref="OnPrintPage"/> method allows derived classes to handle the event
        /// without attaching a delegate.
        /// This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        protected virtual void OnPrintPage(PrintPageEventArgs e)
        {
            PrintPage?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (!IsHandlerCreated)
                return;

            Handler.PrintPage -= OnNativePrintDocumentPrintPage;
            Handler.BeginPrint -= OnNativePrintDocumentBeginPrint;
            Handler.EndPrint -= OnNativePrintDocumentEndPrint;

            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override IPrintDocumentHandler CreateHandler()
        {
            var result = PrintingFactory.Handler.CreatePrintDocumentHandler();

            result.PrintPage += OnNativePrintDocumentPrintPage;
            result.BeginPrint += OnNativePrintDocumentBeginPrint;
            result.EndPrint += OnNativePrintDocumentEndPrint;

            return result;
        }

        /// <summary>
        /// Handles the completion of a native print document operation and updates the cancellation status based on the
        /// print event arguments.
        /// </summary>
        /// <remarks>Override this method to customize behavior when a native print document finishes
        /// printing. The cancellation status in <paramref name="e"/> will be updated according to the result of the
        /// print event.</remarks>
        /// <param name="sender">The source of the event, typically the print document or related component.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data, including the cancellation status for the
        /// print operation.</param>
        protected virtual void OnNativePrintDocumentEndPrint(object? sender, CancelEventArgs e)
        {
            var ea = new PrintEventArgs();
            OnEndPrint(ea);
            e.Cancel = ea.Cancel;

            currentDrawingContext = null;
        }

        /// <summary>
        /// Raises the event signaling the start of a native print document operation.
        /// </summary>
        /// <remarks>Override this method to perform custom actions when a native print document begins
        /// printing. Setting <paramref name="e"/>.Cancel to <see langword="true"/> will abort the print
        /// operation.</remarks>
        /// <param name="sender">The source of the event, typically the print document initiating the operation.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> instance that can be used to cancel the print operation.</param>
        protected virtual void OnNativePrintDocumentBeginPrint(object? sender, CancelEventArgs e)
        {
            ThrowIfPrintingInProgress();

            currentDrawingContext = Handler.DrawingContext;

            var ea = new PrintEventArgs();
            OnBeginPrint(ea);
            e.Cancel = ea.Cancel;
        }

        /// <summary>
        /// Handles the print page event for the native print document, raising the print page logic and updating the
        /// cancellation status as needed.
        /// </summary>
        /// <remarks>Override this method to customize how print pages are processed during native
        /// printing. The cancellation status of the event is updated based on the print page logic.</remarks>
        /// <param name="sender">The source of the event, typically the print document object that initiated the print page request.</param>
        /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data, including the cancellation flag that can be
        /// set to abort printing.</param>
        /// <exception cref="InvalidOperationException">Thrown if the drawing context required for printing is not available.</exception>
        protected virtual void OnNativePrintDocumentPrintPage(object? sender, CancelEventArgs e)
        {
            if (currentDrawingContext == null)
                throw new InvalidOperationException();

            var ea = new PrintPageEventArgs(this, currentDrawingContext);
            OnPrintPage(ea);
            e.Cancel = ea.Cancel;
        }
    }
}