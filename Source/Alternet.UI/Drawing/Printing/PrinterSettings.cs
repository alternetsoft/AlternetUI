using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Native;
using System;

namespace Alternet.Drawing.Printing
{
    public class PrinterSettings : IDisposable
    {
        private bool isDisposed;

        internal PrinterSettings(UI.Native.PrinterSettings nativePrinterSettings)
        {
            NativePrinterSettings = nativePrinterSettings;
        }

        internal UI.Native.PrinterSettings NativePrinterSettings { get; private set; }

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