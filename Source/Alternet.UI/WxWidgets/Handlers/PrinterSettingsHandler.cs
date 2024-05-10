using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    internal class PrinterSettingsHandler : IPrinterSettingsHandler
    {
        private UI.Native.PrinterSettings nativeObject;

        public PrinterSettingsHandler()
        {
            nativeObject = new UI.Native.PrinterSettings();
        }

        public PrinterSettingsHandler(UI.Native.PrinterSettings nativeObject)
        {
            this.nativeObject = nativeObject;
        }

        Duplex IPrinterSettingsHandler.Duplex
        {
            get => (Duplex)nativeObject.Duplex;
            set => nativeObject.Duplex = (UI.Native.Duplex)value;
        }

        PrintRange IPrinterSettingsHandler.PrintRange
        {
            get => (PrintRange)nativeObject.PrintRange;
            set => nativeObject.PrintRange = (UI.Native.PrintRange)value;
        }

        int IPrinterSettingsHandler.FromPage
        {
            get => nativeObject.FromPage;
            set => nativeObject.FromPage = value;
        }

        int IPrinterSettingsHandler.ToPage
        {
            get => nativeObject.ToPage;
            set => nativeObject.ToPage = value;
        }

        int IPrinterSettingsHandler.MinimumPage
        {
            get => nativeObject.MinimumPage;
            set => nativeObject.MinimumPage = value;
        }

        int IPrinterSettingsHandler.MaximumPage
        {
            get => nativeObject.MaximumPage;
            set => nativeObject.MaximumPage = value;
        }

        bool IPrinterSettingsHandler.Collate
        {
            get => nativeObject.Collate;
            set => nativeObject.Collate = value;
        }

        int IPrinterSettingsHandler.Copies
        {
            get => nativeObject.Copies;
            set => nativeObject.Copies = value;
        }

        bool IPrinterSettingsHandler.PrintToFile
        {
            get => nativeObject.PrintToFile;
            set => nativeObject.PrintToFile = value;
        }

        string? IPrinterSettingsHandler.PrinterName
        {
            get => nativeObject.PrinterName;
            set => nativeObject.PrinterName = value;
        }

        string? IPrinterSettingsHandler.PrintFileName
        {
            get => nativeObject.PrintFileName;
            set => nativeObject.PrintFileName = value;
        }

        bool IPrinterSettingsHandler.IsDefaultPrinter
        {
            get => nativeObject.IsDefaultPrinter;
        }

        bool IPrinterSettingsHandler.IsValid
        {
            get => nativeObject.IsValid;
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
