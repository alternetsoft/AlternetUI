using System;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies information about how a document is printed,
    /// when printing from a AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// Typically, you access a <see cref="PrinterSettings"/>
    /// through <see cref="PrintDocument.PrinterSettings"/>
    /// property to modify printer settings.
    /// </remarks>
    public class PrinterSettings : HandledObject<IPrinterSettingsHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterSettings"/> class.
        /// </summary>
        public PrinterSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterSettings"/> class.
        /// </summary>
        public PrinterSettings(IPrinterSettingsHandler nativePrinterSettings)
        {
            Handler = nativePrinterSettings;
        }

        /// <summary>
        /// Gets or sets the printer setting for double-sided printing.
        /// </summary>
        public virtual Duplex Duplex
        {
            get
            {
                return Handler.Duplex;
            }

            set
            {
                Handler.Duplex = value;
            }
        }

        /// <summary>
        /// Gets or sets the page number of the first page to print.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used
        /// by the <see cref="PrintDialog"/>
        /// when the user selects a print range. The <see cref="PrintDialog.AllowSomePages"/>
        /// property must be set to
        /// true to enable the user to specify a print range. In addition, the
        /// <see cref="PrintDialog"/> requires the
        /// <see cref="MinimumPage"/> and <see cref="MaximumPage"/> to be specified
        /// and the <see cref="FromPage"/> value
        /// to be within that range.
        /// </para>
        /// <para>
        /// During the printing process, in the <see cref="PrintDocument.PrintPage"/> event,
        /// view the <see
        /// cref="PrintRange"/> to determine what should be printed. If <see cref="PrintRange"/>
        /// is <see
        /// cref="PrintRange.SomePages"/>, use the <see cref="FromPage"/> and <see cref="ToPage"/>
        /// properties to
        /// determine what pages should be printed. If <see cref="PrintRange"/>
        /// is <see cref="PrintRange.Selection"/>,
        /// then specify output only for the selected pages.
        /// </para>
        /// </remarks>
        public virtual int FromPage
        {
            get
            {
                return Handler.FromPage;
            }

            set
            {
                Handler.FromPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of the last page to print.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used
        /// by the <see cref="PrintDialog"/>
        /// when the user selects a print range. The <see cref="PrintDialog.AllowSomePages"/>
        /// property must be set to
        /// true to enable the user to specify a print range. In addition, the
        /// <see cref="PrintDialog"/> requires the
        /// <see cref="MinimumPage"/> and <see cref="MaximumPage"/> to be specified
        /// and the <see cref="FromPage"/> value
        /// to be within that range.
        /// </para>
        /// <para>
        /// During the printing process, in the <see cref="PrintDocument.PrintPage"/>
        /// event, view the <see
        /// cref="PrintRange"/> to determine what should be printed.
        /// If <see cref="PrintRange"/> is <see
        /// cref="PrintRange.SomePages"/>, use the <see cref="FromPage"/> and
        /// <see cref="ToPage"/> properties to
        /// determine what pages should be printed. If <see cref="PrintRange"/>
        /// is <see cref="PrintRange.Selection"/>,
        /// then specify output only for the selected pages.
        /// </para>
        /// </remarks>
        public virtual int ToPage
        {
            get
            {
                return Handler.ToPage;
            }

            set
            {
                Handler.ToPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum <see cref="FromPage"/> or <see cref="ToPage"/> that
        /// can be selected in a <see
        /// cref="PrintDialog"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used by
        /// the <see cref="PrintDialog"/> when the user selects a print range. The
        /// <see cref="PrintDialog.AllowSomePages"/> property must be set to true to
        /// enable the user to specify a print range.
        /// </remarks>
        public virtual int MinimumPage
        {
            get
            {
                return Handler.MinimumPage;
            }

            set
            {
                Handler.MinimumPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum <see cref="FromPage"/> or <see cref="ToPage"/>
        /// that can be selected in a <see cref="PrintDialog"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="FromPage"/> and <see cref="ToPage"/> properties are used
        /// by the <see cref="PrintDialog"/> when the user selects a print range. The
        /// <see cref="PrintDialog.AllowSomePages"/> property must be set to true
        /// to enable the user to specify a print range.
        /// </remarks>
        public virtual int MaximumPage
        {
            get
            {
                return Handler.MaximumPage;
            }

            set
            {
                Handler.MaximumPage = value;
            }
        }

        /// <summary>
        /// Gets or sets the page numbers that the user has specified to be printed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="PrintRange"/> property is used by the <see cref="PrintDialog"/>
        /// when the user selects a print
        /// range. The default <see cref="PrintRange"/> is <see cref="PrintRange.AllPages"/>.
        /// To enable the user to
        /// specify a range of pages to print, the <see cref="PrintDialog.AllowSomePages"/>
        /// property must be set to <see
        /// langword="true"/>. To enable the user to specify the selected pages to print,
        /// the <see
        /// cref="PrintDialog.AllowSelection"/> property must be set to <see langword="true"/>.
        /// </para>
        /// <para>
        /// During the printing process, in the <see cref="PrintDocument.PrintPage"/> event,
        /// view the <see cref="PrintRange"/> to determine what
        /// should be printed. If <see cref="PrintRange"/> is <see cref="PrintRange.SomePages"/>,
        /// use the <see cref="FromPage"/> and <see cref="ToPage"/> properties to
        /// determine what pages should be printed. If <see cref="PrintRange"/> is
        /// <see cref="PrintRange.Selection"/>, then specify output only for
        /// the selected pages.
        /// </para>
        /// </remarks>
        public virtual PrintRange PrintRange
        {
            get
            {
                return Handler.PrintRange;
            }

            set
            {
                Handler.PrintRange = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the printed document is collated.
        /// </summary>
        /// <value><see langword="true"/> if the printed document is collated; otherwise,
        /// <see langword="false"/>. The
        /// default is <see langword="false"/>.</value>
        /// <remarks>
        /// Collating is performed only when the number of copies is greater than 1.
        /// Set the <see cref="Copies"/> property to specify
        /// the number of copies to print. Setting <see cref="Collate"/> to <see langword="true"/>
        /// will print a complete copy of the document before the
        /// first page of the next copy is printed. false will print each page by the number
        /// of copies specified before
        /// printing the next page.
        /// </remarks>
        public virtual bool Collate
        {
            get
            {
                return Handler.Collate;
            }

            set
            {
                Handler.Collate = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of copies of the document to print.
        /// </summary>
        /// <value>
        /// The number of copies to print. The default is 1.
        /// </value>
        public virtual int Copies
        {
            get
            {
                return Handler.Copies;
            }

            set
            {
                Handler.Copies = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the printing output is sent to a file
        /// instead of a port.
        /// </summary>
        /// <value><see langword="true"/> if the printing output is sent to a file; otherwise,
        /// <see langword="false"/>. The default is <see langword="false"/>.</value>
        /// <remarks>
        /// The <see cref="PrintToFile"/> property is used by the <see cref="PrintDialog"/>
        /// when the user selects the <b>Print to file</b> option.
        /// </remarks>
        public virtual bool PrintToFile
        {
            get
            {
                return Handler.PrintToFile;
            }

            set
            {
                Handler.PrintToFile = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the printer to use.
        /// </summary>
        /// <value>The name of the printer to use. Use <see langword="null"/> value to specify
        /// the default printer.</value>
        /// <remarks>
        /// After setting the printer name, call <see cref="IsValid"/> to determine if the
        /// printer name is recognized as a valid printer on the system.
        /// </remarks>
        public virtual string? PrinterName
        {
            get
            {
                return Handler.PrinterName;
            }

            set
            {
                Handler.PrinterName = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterName"/> property designates
        /// a valid printer.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="PrinterName"/> property designates a
        /// valid printer; otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool IsValid
        {
            get => Handler.IsValid;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterName"/> property designates
        /// the default printer.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="PrinterName"/> designates the default
        /// printer; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// <see cref="IsDefaultPrinter"/> always returns <see langword="false"/> when you
        /// explicitly set the <see
        /// cref="PrinterName"/> property to a string value other than <see langword="null"/>.
        /// </remarks>
        public virtual bool IsDefaultPrinter
        {
            get => Handler.IsDefaultPrinter;
        }

        /// <summary>
        /// Gets or sets the file name, when printing to a file.
        /// </summary>
        public virtual string? PrintFileName
        {
            get
            {
                return Handler.PrintFileName;
            }

            set
            {
                Handler.PrintFileName = value;
            }
        }

        /// <summary>
        /// Writes the current printer settings and related properties to the specified log writer for diagnostic or
        /// informational purposes.
        /// </summary>
        /// <remarks>This method outputs a summary of key printer settings, including printer name,
        /// validity, default status, file printing options, page ranges, duplex mode, collation, and copy count. It is
        /// useful for troubleshooting or auditing printer configuration. The output format and destination depend on
        /// the implementation of <see cref="ILogWriter"/>.</remarks>
        /// <param name="log">The log writer to which the printer settings will be written. If <paramref name="log"/> is <see
        /// langword="null"/>, the default debug log writer is used.</param>
        public virtual void Log(ILogWriter? log = null)
        {
            if (log == null)
                log = LogWriter.Debug;
            log.BeginSection("PrinterSettings");
            log.WriteKeyValue("PrinterName", PrinterName);
            log.WriteKeyValue("IsValid", IsValid);
            log.WriteKeyValue("IsDefaultPrinter", IsDefaultPrinter);
            log.WriteKeyValue("PrintToFile", PrintToFile);
            log.WriteKeyValue("PrintFileName", PrintFileName);
            log.WriteKeyValue("Duplex", Duplex);
            log.WriteKeyValue("FromPage", FromPage);
            log.WriteKeyValue("ToPage", ToPage);
            log.WriteKeyValue("MinimumPage", MinimumPage);
            log.WriteKeyValue("MaximumPage", MaximumPage);
            log.WriteKeyValue("PrintRange", PrintRange);
            log.WriteKeyValue("Collate", Collate);
            log.WriteKeyValue("Copies", Copies);
            log.EndSection();
        }

        /// <inheritdoc/>
        protected override IPrinterSettingsHandler CreateHandler()
        {
            return PrintingFactory.Handler.CreatePrinterSettingsHandler();
        }
    }
}