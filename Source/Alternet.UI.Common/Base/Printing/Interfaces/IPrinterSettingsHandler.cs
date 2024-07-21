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

        /// <inheritdoc cref="PrinterSettings.PrinterName"/>
        string? PrinterName { get; set; }

        /// <inheritdoc cref="PrinterSettings.IsValid"/>
        bool IsValid { get; }

        /// <inheritdoc cref="PrinterSettings.IsDefaultPrinter"/>
        bool IsDefaultPrinter { get; }

        /// <inheritdoc cref="PrinterSettings.PrintFileName"/>
        string? PrintFileName { get; set; }
    }
}
