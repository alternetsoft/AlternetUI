using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for printer-related operations.
    /// </summary>
    public static class PrinterUtils
    {
        /// <summary>
        /// Determines whether any printers are installed on the system.
        /// </summary>
        /// <remarks>This method checks for the presence of printers. If an error occurs
        /// during the check, the method returns false.</remarks>
        /// <returns>true if printers are available; otherwise, false.
        /// Returns null if the operating system is not Windows.
        /// </returns>
        public static bool? HasPrinters()
        {
            try
            {
                if (App.IsWindowsOS)
                {
                    return MswPrinterUtils.HasPrinters();
                }

                return null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name of the default printer for the current operating system, if available.
        /// </summary>
        /// <remarks>This method checks if the application is running on a Windows operating system before
        /// attempting to retrieve the default printer name. If the operating system is not Windows, it returns
        /// null.</remarks>
        /// <returns>The name of the default printer as a string, or null if the operation fails or if the operating system is
        /// not Windows.</returns>
        public static string? GetDefaultPrinterName()
        {
            try
            {
                if (App.IsWindowsOS)
                {
                    return MswPrinterUtils.GetDefaultPrinterName();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves a read-only list of printer names available on the system.
        /// </summary>
        /// <remarks>If called on a non-Windows
        /// platform or if an error occurs during retrieval, the method returns null.</remarks>
        /// <returns>A read-only list of printer names if available; otherwise, null.</returns>
        public static IReadOnlyList<string>? GetPrinterNames()
        {
            try
            {
                if (App.IsWindowsOS)
                {
                    return MswPrinterUtils.GetPrinterNames();
                }

                return null;
            }
            catch
            {
                return [];
            }
        }
    }
}
