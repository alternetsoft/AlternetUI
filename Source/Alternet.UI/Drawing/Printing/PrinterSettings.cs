using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Native;
using System;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies information about how a document is printed, when printing from a AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// Typically, you access a <see cref="PrinterSettings"/> through <see cref="PrintDocument.PrinterSettings"/>
    /// or <see cref="PageSettings.PrinterSettings"/> properties to modify printer settings.
    /// </remarks>
    public class PrinterSettings : IDisposable
    {
        private bool isDisposed;

        internal PrinterSettings(UI.Native.PrinterSettings nativePrinterSettings)
        {
            NativePrinterSettings = nativePrinterSettings;
        }

        internal UI.Native.PrinterSettings NativePrinterSettings { get; private set; }

        /// <summary>
        /// Gets or sets the printer setting for double-sided printing.
        /// </summary>
        public Duplex Duplex { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the page number of the first page to print.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by the <see cref="PrintDialog"/>
        /// when the user selects a print range. The <see cref="PrintDialog.AllowSomePages"/> property must be set to
        /// true to enable the user to specify a print range. In addition, the <see cref="PrintDialog"/> requires the
        /// <see cref="MinimumPage"/> and <see cref="MaximumPage"/> to be specified and the <see cref="FromPage"/> value
        /// to be within that range.
        /// </para>
        /// <para>
        /// During the printing process, in the <see cref="PrintDocument.PrintPage"/> event, view the <see
        /// cref="PrintRange"/> to determine what should be printed. If <see cref="PrintRange"/> is <see
        /// cref="PrintRange.SomePages"/>, use the <see cref="FromPage"/> and <see cref="ToPage"/> properties to
        /// determine what pages should be printed. If <see cref="PrintRange"/> is <see cref="PrintRange.Selection"/>,
        /// then specify output only for the selected pages.
        /// </para>
        /// </remarks>
        public int FromPage { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the number of the last page to print.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by the <see cref="PrintDialog"/>
        /// when the user selects a print range. The <see cref="PrintDialog.AllowSomePages"/> property must be set to
        /// true to enable the user to specify a print range. In addition, the <see cref="PrintDialog"/> requires the
        /// <see cref="MinimumPage"/> and <see cref="MaximumPage"/> to be specified and the <see cref="FromPage"/> value
        /// to be within that range.
        /// </para>
        /// <para>
        /// During the printing process, in the <see cref="PrintDocument.PrintPage"/> event, view the <see
        /// cref="PrintRange"/> to determine what should be printed. If <see cref="PrintRange"/> is <see
        /// cref="PrintRange.SomePages"/>, use the <see cref="FromPage"/> and <see cref="ToPage"/> properties to
        /// determine what pages should be printed. If <see cref="PrintRange"/> is <see cref="PrintRange.Selection"/>,
        /// then specify output only for the selected pages.
        /// </para>
        /// </remarks>
        public int ToPage { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the minimum <see cref="FromPage"/> or <see cref="ToPage"/> that can be selected in a <see
        /// cref="PrintDialog"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by the <see cref="PrintDialog"/> when the user selects a print range. The
        /// <see cref="PrintDialog.AllowSomePages"/> property must be set to true to enable the user to specify a print range.
        /// </remarks>
        public int MinimumPage { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the maximum <see cref="FromPage"/> or <see cref="ToPage"/> that can be selected in a <see cref="PrintDialog"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by the <see cref="PrintDialog"/> when the user selects a print range. The
        /// <see cref="PrintDialog.AllowSomePages"/> property must be set to true to enable the user to specify a print range.
        /// </remarks>
        public int MaximumPage { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the page numbers that the user has specified to be printed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="PrintRange"/> property is used by the <see cref="PrintDialog"/> when the user selects a print
        /// range. The default <see cref="PrintRange"/> is <see cref="PrintRange.AllPages"/>. To enable the user to
        /// specify a range of pages to print, the <see cref="PrintDialog.AllowSomePages"/> property must be set to <see
        /// langword="true"/>. To enable the user to specify the selected pages to print, the <see
        /// cref="PrintDialog.AllowSelection"/> property must be set to <see langword="true"/>.
        /// </para>
        /// <para>
        /// During the printing process, in the <see cref="PrintDocument.PrintPage"/> event, view the <see cref="PrintRange"/> to determine what
        /// should be printed. If <see cref="PrintRange"/> is <see cref="PrintRange.SomePages"/>, use the <see cref="FromPage"/> and <see cref="ToPage"/> properties to
        /// determine what pages should be printed. If <see cref="PrintRange"/> is <see cref="PrintRange.Selection"/>, then specify output only for
        /// the selected pages.
        /// </para>
        /// </remarks>
        public PrintRange PrintRange { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the printed document is collated.
        /// </summary>
        /// <value><see langword="true"/> if the printed document is collated; otherwise, <see langword="false"/>. The
        /// default is <see langword="false"/>.</value>
        /// <remarks>
        /// Collating is performed only when the number of copies is greater than 1. Set the <see cref="Copies"/> property to specify
        /// the number of copies to print. Setting <see cref="Collate"/> to <see langword="true"/> will print a complete copy of the document before the
        /// first page of the next copy is printed. false will print each page by the number of copies specified before
        /// printing the next page.
        /// </remarks>
        public bool Collate { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the number of copies of the document to print.
        /// </summary>
        /// <value>
        /// The number of copies to print. The default is 1.
        /// </value>
        public int Copies { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets the default page settings for this printer.
        /// </summary>
        /// <value>A <see cref="PageSettings"/> that represents the default page settings for this printer.</value>
        /// <remarks>
        /// Page settings include the size of the margins on the page, the size of paper to use, and whether to print in
        /// color. For more information about page settings, see the <see cref="PageSettings"/> class.
        /// </remarks>
        public PageSettings DefaultPageSettings { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the printing output is sent to a file instead of a port.
        /// </summary>
        /// <value><see langword="true"/> if the printing output is sent to a file; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// The <see cref="PrintToFile"/> property is used by the <see cref="PrintDialog"/> when the user selects the <b>Print to file</b> option.
        /// </remarks>
        public bool PrintToFile { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the name of the printer to use.
        /// </summary>
        /// <value>The name of the printer to use. Use <see langword="null"/> value to specify the default printer.</value>
        /// <remarks>
        /// After setting the printer name, call <see cref="IsValid"/> to determine if the printer name is recognized as a valid printer on the system.
        /// </remarks>
        public string? PrinterName { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterName"/> property designates a valid printer.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="PrinterName"/> property designates a valid printer; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsValid { get => throw new Exception(); }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterName"/> property designates the default printer.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="PrinterName"/> designates the default printer; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <see cref="IsDefaultPrinter"/> always returns <see langword="false"/> when you explicitly set the <see
        /// cref="PrinterName"/> property to a string value other than <see langword="null"/>.
        /// </remarks>
        public bool IsDefaultPrinter { get => throw new Exception(); }

        /// <summary>
        /// Gets or sets the file name, when printing to a file.
        /// </summary>
        public string? PrintFileName { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativePrinterSettings.Dispose();
                    NativePrinterSettings = null!;
                }

                isDisposed = true;
            }
        }
    }
}