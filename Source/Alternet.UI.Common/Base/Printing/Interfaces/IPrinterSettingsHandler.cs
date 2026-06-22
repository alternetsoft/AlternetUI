using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains methods and properties which allow to change printer settings.
    /// </summary>
    public interface IPrinterSettingsHandler : IDisposable
    {
        /// <inheritdoc cref="PrinterSettings.Duplex"/>
        Duplex Duplex { get; set; }

        /// <inheritdoc cref="PrinterSettings.FromPage"/>
        int FromPage { get; set; }

        /// <inheritdoc cref="PrinterSettings.ToPage"/>
        int ToPage { get; set; }

        /// <inheritdoc cref="PrinterSettings.MinimumPage"/>
        int MinimumPage { get; set; }

        /// <inheritdoc cref="PrinterSettings.MaximumPage"/>
        int MaximumPage { get; set; }

        /// <inheritdoc cref="PrinterSettings.PrintRange"/>
        PrintRange PrintRange { get; set; }

        /// <inheritdoc cref="PrinterSettings.Collate"/>
        bool Collate { get; set; }

        /// <inheritdoc cref="PrinterSettings.Copies"/>
        int Copies { get; set; }

        /// <inheritdoc cref="PrinterSettings.PrintToFile"/>
        bool PrintToFile { get; set; }

        /// <summary>
        /// Gets the name of the printer.
        /// </summary>
        /// <returns>The name of the printer.</returns>
        string? GetPrinterName();

        /// <summary>
        /// Sets the name of the printer.
        /// </summary>
        /// <param name="value">The name of the printer.</param>
        void SetPrinterName(string? value);

        /// <inheritdoc cref="PrinterSettings.IsValid"/>
        bool IsValid { get; }

        /// <inheritdoc cref="PrinterSettings.IsDefaultPrinter"/>
        bool IsDefaultPrinter { get; }

        /// <summary>
        /// Gets the file name of the print file.
        /// </summary>
        /// <returns>The file name of the print file.</returns>
        string? GetPrintFileName();

        /// <summary>
        /// Sets the file name of the print file.
        /// </summary>
        /// <param name="value">The file name of the print file.</param>
        void SetPrintFileName(string? value);
    }
}
