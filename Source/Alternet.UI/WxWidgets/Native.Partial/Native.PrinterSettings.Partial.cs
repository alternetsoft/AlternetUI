#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class PrinterSettings : Alternet.Drawing.Printing.IPrinterSettingsHandler
    {
        string? Alternet.Drawing.Printing.IPrinterSettingsHandler.GetPrinterName()
        {
            return GetPrinterName().ToString();
        }

        void Alternet.Drawing.Printing.IPrinterSettingsHandler.SetPrinterName(string? value)
        {
            NativeUtils.Invoke(value, SetPrinterName);
        }

        string? Alternet.Drawing.Printing.IPrinterSettingsHandler.GetPrintFileName()
        {
            return GetPrintFileName().ToString();
        }

        void Alternet.Drawing.Printing.IPrinterSettingsHandler.SetPrintFileName(string? value)
        {
            NativeUtils.Invoke(value, SetPrintFileName);
        }
    }
}
