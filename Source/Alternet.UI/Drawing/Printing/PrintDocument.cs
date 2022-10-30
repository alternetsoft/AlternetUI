using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Native;
using System;
using System.ComponentModel;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Defines a reusable object that sends output to a printer, when printing from an AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// Typically, you create an instance of the <see cref="PrintDocument"/> class, set properties such as the
    /// <see cref="DocumentName"/> and <see cref="PrinterSettings"/>, and call the <see cref="Print"/> method to start the
    /// printing process. Handle the <see cref="PrintPage"/> event where you specify the output to print, by using the
    /// <see cref="PrintPageEventArgs.DrawingContext"/> property of the <see cref="PrintPageEventArgs"/>.
    /// </remarks>
    public class PrintDocument : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDocument"/> class.
        /// </summary>
        public PrintDocument() : this(new UI.Native.PrintDocument())
        {
        }

        internal PrintDocument(UI.Native.PrintDocument nativePrintDocument)
        {
            NativePrintDocument = nativePrintDocument;

            nativePrintDocument.PrintPage += NativePrintDocument_PrintPage;
            nativePrintDocument.BeginPrint += NativePrintDocument_BeginPrint;
            nativePrintDocument.EndPrint += NativePrintDocument_EndPrint;
        }

        private void NativePrintDocument_EndPrint(object? sender, CancelEventArgs e)
        {
            var ea = new PrintEventArgs();
            OnEndPrint(ea);
            e.Cancel = ea.Cancel;

            currentDrawingContext = null;
        }

        private void NativePrintDocument_BeginPrint(object? sender, CancelEventArgs e)
        {
            if (currentDrawingContext != null)
                throw new InvalidOperationException();

            currentDrawingContext = new DrawingContext(NativePrintDocument.PrintPage_DrawingContext);
            
            var ea = new PrintEventArgs();
            OnBeginPrint(ea);
            e.Cancel = ea.Cancel;
        }

        DrawingContext? currentDrawingContext;

        private void NativePrintDocument_PrintPage(object? sender, CancelEventArgs e)
        {
            if (currentDrawingContext == null)
                throw new InvalidOperationException();

            var ea = new PrintPageEventArgs(NativePrintDocument, currentDrawingContext);
            OnPrintPage(ea);
            e.Cancel = ea.Cancel;
        }

        internal UI.Native.PrintDocument NativePrintDocument { get; private set; }

        /// <summary>
        /// Gets or sets the document name to display (for example, in a print status dialog box or printer queue) while printing the document.
        /// </summary>
        /// <value>
        /// The document name to display while printing the document.
        /// </value>
        public string DocumentName
        {
            get
            {
                return NativePrintDocument.DocumentName;
            }

            set
            {
                NativePrintDocument.DocumentName = value;
            }
        }

        /// <summary>
        /// Gets or sets the printer that prints the document.
        /// </summary>
        /// <value>
        /// A <see cref="PrinterSettings"/> that specifies where and how the document is printed.
        /// The default is a <see cref="PrinterSettings"/> with its properties set to their default values.
        /// </value>
        public PrinterSettings PrinterSettings
        {
            get
            {
                return new PrinterSettings(NativePrintDocument.PrinterSettings);
            }

            set
            {
                NativePrintDocument.PrinterSettings = value.NativePrinterSettings;
            }
        }

        /// <summary>
        /// Gets or sets page settings that are used as defaults for all pages to be printed.
        /// </summary>
        /// <value>
        /// A <see cref="PageSettings"/> that specifies the default page settings for the document.
        /// </value>
        /// <remarks>
        /// You can specify several default page settings through the <see cref="DefaultPageSettings"/> property. For example, the
        /// <see cref="PageSettings.Color"/> property specifies whether the page prints in color, the <see cref="PageSettings.Landscape"/> property
        /// specifies landscape or portrait orientation, and the <see cref="PageSettings.Margins"/> property specifies the margins of
        /// the page.
        /// </remarks>
        public PageSettings DefaultPageSettings
        {
            get
            {
                return new PageSettings(NativePrintDocument.DefaultPageSettings);
            }

            set
            {
                NativePrintDocument.DefaultPageSettings = value.NativePageSettings;
            }
        }

        /// <summary>
        /// Occurs when the output to print for the current page is needed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// To specify the output to print, use the <see cref="PrintPageEventArgs.DrawingContext"/> property of the <see
        /// cref="PrintPageEventArgs"/>. For example, to specify a line of text that should be printed, draw the text
        /// using the <see cref="DrawingContext.DrawText(string, Font, Brush, Point)"/> method.
        /// </para>
        /// <para>
        /// In addition to specifying the output, you can indicate if there are additional pages to print by setting the
        /// <see cref="PrintPageEventArgs.HasMorePages"/> property to <see langword="true"/>. The default is <see
        /// langword="false"/>, which indicates that there are no more pages to print. Individual page settings can also
        /// be modified through the <see cref="PageSettings"/> and the print job can be canceled by setting the
        /// <see cref="CancelEventArgs.Cancel"/> property of the <see cref="PrintPageEventArgs"/> object to
        /// <see langword="true"/>.
        /// </para>
        /// </remarks>
        public event EventHandler<PrintPageEventArgs>? PrintPage;

        /// <summary>
        /// Occurs when the <see cref="Print()"/> method is called and before the first page of the document prints.
        /// </summary>
        /// <remarks>
        /// Typically, you handle the <see cref="BeginPrint"/> event to initialize fonts, file streams,
        /// and other resources used during the printing process.
        /// </remarks>
        public event EventHandler<PrintEventArgs>? BeginPrint;

        /// <summary>
        /// Occurs when the last page of the document has printed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Typically, you handle the <see cref="EndPrint"/> event to release fonts, file streams, and other resources
        /// used during the printing process, like fonts.
        /// </para>
        /// <para>
        /// You indicate that there are no more pages to print by setting the <see cref="PrintPageEventArgs.HasMorePages"/> property
        /// to false in the <see cref="PrintPage"/> event. The <see cref="EndPrint"/> event also occurs if the printing process is canceled or an
        /// exception occurs during the printing process.
        /// </para>
        /// </remarks>
        public event EventHandler<PrintEventArgs>? EndPrint;

        /// <summary>
        /// Raises the <see cref="BeginPrint"/> event. It is called after the <see cref="Print()"/> method is called and
        /// before the first page of the document prints.
        /// </summary>
        /// <param name="e">A <see cref="PrintEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// The <see cref="OnBeginPrint"/> method allows derived classes to handle the event without attaching a
        /// delegate. This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        protected virtual void OnBeginPrint(PrintEventArgs e)
        {
            BeginPrint?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="EndPrint"/> event. It is called when the last page of the document has printed.
        /// </summary>
        /// <param name="e">A <see cref="PrintEventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// The <see cref="OnEndPrint"/> method allows derived classes to handle the event without attaching a delegate. This is the
        /// preferred technique for handling the event in a derived class. The <see cref="OnEndPrint"/> method is also called if the
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
        /// The <see cref="OnPrintPage"/> method allows derived classes to handle the event without attaching a delegate.
        /// This is the preferred technique for handling the event in a derived class.
        /// </remarks>
        protected virtual void OnPrintPage(PrintPageEventArgs e)
        {
            PrintPage?.Invoke(this, e);
        }

        /// <summary>
        /// Starts the document's printing process.
        /// </summary>
        /// <remarks>
        /// Specify the output to print by handling the <see cref="PrintPage"/> event and by using the
        /// <see cref="PrintPageEventArgs.DrawingContext"/> included in the <see cref="PrintPageEventArgs"/>.
        /// </remarks>
        public void Print()
        {
            if (currentDrawingContext != null)
                throw new InvalidOperationException("Another printing operation is in progress.");

            NativePrintDocument.Print();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="PrintDocument"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="PrintDocument"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativePrintDocument.PrintPage -= NativePrintDocument_PrintPage;
                    NativePrintDocument.BeginPrint -= NativePrintDocument_BeginPrint;
                    NativePrintDocument.EndPrint -= NativePrintDocument_EndPrint;

                    NativePrintDocument.Dispose();
                    NativePrintDocument = null!;
                }

                isDisposed = true;
            }
        }
    }
}