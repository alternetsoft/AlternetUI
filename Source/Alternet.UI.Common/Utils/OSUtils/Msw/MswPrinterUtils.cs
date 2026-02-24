using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI;

internal static class MswPrinterUtils
{
    // PRINTER_INFO_4 structure (simpler and enough for many purposes)
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PRINTER_INFO_4
    {
        public string pPrinterName;
        public string pServerName;
        public uint Attributes;
    }

    // EnumPrinters function
    [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool EnumPrinters(
        uint flags,
        string name,
        uint level,
        IntPtr pPrinterEnum,
        uint cbBuf,
        out uint pcbNeeded,
        out uint pcReturned);

    private const uint PRINTER_ENUM_LOCAL = 2;
    private const uint PRINTER_ENUM_CONNECTIONS = 4;

    public static bool HasPrinters()
    {
        return GetPrinterNames().Count > 0;
    }

    public static List<string> GetPrinterNames()
    {
        List<string> printers = new List<string>();
        uint flags = PRINTER_ENUM_LOCAL | PRINTER_ENUM_CONNECTIONS;
        uint level = 4;
        uint cbNeeded = 0;
        uint cReturned = 0;

        // Step 1: Query size needed
        EnumPrinters(flags, null!, level, IntPtr.Zero, 0, out cbNeeded, out cReturned);

        if (cbNeeded == 0)
            return printers; // No printers

        // Step 2: Allocate buffer and retrieve
        IntPtr pPrinters = Marshal.AllocHGlobal((int)cbNeeded);

        try
        {
            if (EnumPrinters(flags, null!, level, pPrinters, cbNeeded, out cbNeeded, out cReturned))
            {
                int offset = pPrinters.ToInt32();
                int structSize = Marshal.SizeOf(typeof(PRINTER_INFO_4));

                for (int i = 0; i < cReturned; i++)
                {
                    IntPtr currentPtr = new IntPtr(offset + i * structSize);
                    PRINTER_INFO_4 pi4 = (PRINTER_INFO_4)Marshal.PtrToStructure(currentPtr, typeof(PRINTER_INFO_4))!;
                    printers.Add(pi4.pPrinterName);
                }
            }
        }
        finally
        {
            Marshal.FreeHGlobal(pPrinters);
        }
        return printers;
    }

    // Get Default Printer using WinAPI
    [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int pcchBuffer);

    public static string? GetDefaultPrinterName()
    {
        int size = 0;
        GetDefaultPrinter(null!, ref size); // Get required size
        if (size == 0)
            return null;

        StringBuilder buffer = new StringBuilder(size);
        if (GetDefaultPrinter(buffer, ref size))
            return buffer.ToString();

        return null;
    }
}
