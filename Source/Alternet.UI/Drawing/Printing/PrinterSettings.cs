using Alternet.UI.Native;
using System;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies information about how a document is printed, when printing from a AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// Typically, you access a <see cref="PrinterSettings"/> through <see cref="PrintDocument.PrinterSettings"/>
    /// property to modify printer settings.
    /// </remarks>
    public class PrinterSettings : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterSettings"/> class.
        /// </summary>
        public PrinterSettings()
            : this(new UI.Native.PrinterSettings())
        {
        }

        internal PrinterSettings(UI.Native.PrinterSettings nativePrinterSettings)
        {
            NativePrinterSettings = nativePrinterSettings;
        }

        internal UI.Native.PrinterSettings NativePrinterSettings { get; private set; }

        /// <summary>
        /// Gets or sets the printer setting for double-sided printing.
        /// </summary>
        public Duplex Duplex
        {
            get
            {
                return (Duplex)NativePrinterSettings.Duplex;
            }

            set
            {
                NativePrinterSettings.Duplex = (UI.Native.Duplex)value;
            }
        }

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
        public int FromPage
        {
            get
            {
                return NativePrinterSettings.FromPage;
            }

            set
            {
                NativePrinterSettings.FromPage = value;
            }
        }

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
        public int ToPage
        {
            get
            {
                return NativePrinterSettings.ToPage;
            }

            set
            {
                NativePrinterSettings.ToPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum <see cref="FromPage"/> or <see cref="ToPage"/> that can be selected in a <see
        /// cref="PrintDialog"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by the <see cref="PrintDialog"/> when the user selects a print range. The
        /// <see cref="PrintDialog.AllowSomePages"/> property must be set to true to enable the user to specify a print range.
        /// </remarks>
        public int MinimumPage
        {
            get
            {
                return NativePrinterSettings.MinimumPage;
            }

            set
            {
                NativePrinterSettings.MinimumPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum <see cref="FromPage"/> or <see cref="ToPage"/> that can be selected in a <see cref="PrintDialog"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by the <see cref="PrintDialog"/> when the user selects a print range. The
        /// <see cref="PrintDialog.AllowSomePages"/> property must be set to true to enable the user to specify a print range.
        /// </remarks>
        public int MaximumPage
        {
            get
            {
                return NativePrinterSettings.MaximumPage;
            }

            set
            {
                NativePrinterSettings.MaximumPage = value;
            }
        }

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
        public PrintRange PrintRange
        {
            get
            {
                return (PrintRange)NativePrinterSettings.PrintRange;
            }

            set
            {
                NativePrinterSettings.PrintRange = (UI.Native.PrintRange)value;
            }
        }

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
        public bool Collate
        {
            get
            {
                return NativePrinterSettings.Collate;
            }

            set
            {
                NativePrinterSettings.Collate = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of copies of the document to print.
        /// </summary>
        /// <value>
        /// The number of copies to print. The default is 1.
        /// </value>
        public int Copies
        {
            get
            {
                return NativePrinterSettings.Copies;
            }

            set
            {
                NativePrinterSettings.Copies = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the printing output is sent to a file instead of a port.
        /// </summary>
        /// <value><see langword="true"/> if the printing output is sent to a file; otherwise, <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// The <see cref="PrintToFile"/> property is used by the <see cref="PrintDialog"/> when the user selects the <b>Print to file</b> option.
        /// </remarks>
        public bool PrintToFile
        {
            get
            {
                return NativePrinterSettings.PrintToFile;
            }

            set
            {
                NativePrinterSettings.PrintToFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the printer to use.
        /// </summary>
        /// <value>The name of the printer to use. Use <see langword="null"/> value to specify the default printer.</value>
        /// <remarks>
        /// After setting the printer name, call <see cref="IsValid"/> to determine if the printer name is recognized as a valid printer on the system.
        /// </remarks>
        public string? PrinterName
        {
            get
            {
                return NativePrinterSettings.PrinterName;
            }

            set
            {
                NativePrinterSettings.PrinterName = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterName"/> property designates a valid printer.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="PrinterName"/> property designates a valid printer; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsValid { get => NativePrinterSettings.IsValid; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterName"/> property designates the default printer.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="PrinterName"/> designates the default printer; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <see cref="IsDefaultPrinter"/> always returns <see langword="false"/> when you explicitly set the <see
        /// cref="PrinterName"/> property to a string value other than <see langword="null"/>.
        /// </remarks>
        public bool IsDefaultPrinter { get => NativePrinterSettings.IsDefaultPrinter; }

        /// <summary>
        /// Gets or sets the file name, when printing to a file.
        /// </summary>
        public string? PrintFileName
        {
            get
            {
                return NativePrinterSettings.PrintFileName;
            }

            set
            {
                NativePrinterSettings.PrintFileName = value;
            }
        }

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