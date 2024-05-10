using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing.Printing
{
    public interface IPrinterSettingsHandler : IDisposable
    {
        Duplex Duplex { get; set; }

        int FromPage { get; set; }

        int ToPage { get; set; }

        int MinimumPage { get; set; }

        int MaximumPage { get; set; }

        PrintRange PrintRange { get; set; }

        bool Collate { get; set; }

        int Copies { get; set; }

        bool PrintToFile { get; set; }

        string? PrinterName { get; set; }

        bool IsValid { get; }

        bool IsDefaultPrinter { get; }

        string? PrintFileName { get; set; }
    }
}
