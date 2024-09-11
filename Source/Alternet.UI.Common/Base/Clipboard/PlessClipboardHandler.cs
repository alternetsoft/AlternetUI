using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements internal <see cref="IClipboardHandler"/> which is not connected to the
    /// operating system clipboard.
    /// </summary>
    public class PlessClipboardHandler : DisposableObject, IClipboardHandler
    {
        private static IDataObject? data;

        /// <inheritdoc/>
        public virtual IDataObject? GetData()
        {
            return data;
        }

        /// <inheritdoc/>
        public virtual void SetData(IDataObject? value)
        {
            data = value;
        }
    }
}
