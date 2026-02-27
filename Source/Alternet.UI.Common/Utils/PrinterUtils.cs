using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;
using Alternet.Drawing.Printing;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for printer-related operations.
    /// </summary>
    public static class PrinterUtils
    {
        /// <summary>
        /// Gets or sets a function that returns a list of available printer names.
        /// </summary>
        /// <remarks>Set this property to provide a custom method for retrieving printer names. The
        /// assigned function should return a read-only list of printer names, or null if it is
        /// not possible to determine the printer names. If
        /// not set, the default printer name retrieval mechanism is used.</remarks>
        public static Func<IReadOnlyList<string>?>? GetPrinterNamesOverride { get; set; }

        /// <summary>
        /// Gets or sets a function that returns the name of the default printer, or null if no default printer is set.
        /// </summary>
        /// <remarks>Assign this property to customize how the default printer name is determined. If not
        /// set, the default printer name will not be available through this mechanism.</remarks>
        public static Func<string?>? GetDefaultPrinterNameOverride { get; set; }

        /// <summary>
        /// Gets or sets an override function that determines whether printers are available on the system.
        /// The function should return a nullable boolean value: true if printers are available, false if not,
        /// or null to indicate that it is not possible to determine the printer availability.
        /// </summary>
        /// <remarks>Assign a delegate to this property to provide custom logic for evaluating printer
        /// override status at runtime. This can be useful in scenarios where printer configuration may change based on
        /// user preferences, application state, or other dynamic conditions. If the function returns <see
        /// langword="null"/>, the default printer behavior is used.</remarks>
        public static Func<bool?>? HasPrintersOverride { get; set; }

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
                if (HasPrintersOverride != null)
                    return HasPrintersOverride();

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
                if (GetDefaultPrinterNameOverride != null)
                    return GetDefaultPrinterNameOverride();

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
                if (GetPrinterNamesOverride != null)
                    return GetPrinterNamesOverride();

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

        /// <summary>
        /// Calculates the pixel dimensions of a specified paper size for use with Skia documents,
        /// based on the given paper kind and optional raster DPI.
        /// </summary>
        /// <remarks>The calculated pixel dimensions are rounded to the nearest integer values. Use this
        /// method to obtain the appropriate pixel size for rendering or printing documents with Skia at a specific
        /// resolution.</remarks>
        /// <param name="paperSize">A value that specifies the standard paper size to use,
        /// as defined by the KnownPaperKind enumeration.</param>
        /// <param name="rasterDpi">The rasterization resolution, in dots per inch (DPI), to use for the calculation.
        /// If null, the default DPI for Skia documents is used.</param>
        /// <returns>A SizeI structure representing the width and height of the paper in pixels, calculated according to the
        /// specified paper size and DPI.</returns>
        public static SizeI GetPaperSizeForSkiaDocument(KnownPaperKind paperSize, float? rasterDpi = null)
        {
            rasterDpi ??= SKDocument.DefaultRasterDpi;

            var size = PaperSizes.SizeInchesRounded.GetSize(paperSize);
            var width = MathF.Round(size.Width * rasterDpi.Value);
            var height = MathF.Round(size.Height * rasterDpi.Value);

            return new SizeI((int)width, (int)height);
        }
    }
}
