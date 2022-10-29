using Alternet.UI.Internal.ComponentModel;
using Alternet.UI.Native;
using System;

namespace Alternet.Drawing.Printing
{
    public class PageSettings : IDisposable
    {
        private bool isDisposed;

        internal PageSettings(UI.Native.PageSettings nativePageSettings)
        {
            NativePageSettings = nativePageSettings;
        }

        internal UI.Native.PageSettings NativePageSettings { get; private set; }

        public bool Color { get; set; }

        public bool Landscape { get; set; }

        public Margins Margins { get; set; }

        public string DocumentName { get; set; }

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
                    NativePageSettings.Dispose();
                    NativePageSettings = null!;
                }

                isDisposed = true;
            }
        }
    }
}