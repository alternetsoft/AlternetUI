using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public virtual bool AsyncRequired => false;

        /// <inheritdoc/>
        public virtual bool OnlyText => false;

        /// <inheritdoc/>
        public virtual bool Flush()
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual bool HasFormat(ClipboardDataFormatId format)
        {
            return data?.HasFormat(format) ?? false;
        }

        /// <inheritdoc/>
        public virtual bool HasFormat(string format)
        {
            return data?.HasFormat(format) ?? false;
        }

        /// <inheritdoc/>
        public virtual IDataObject? GetData()
        {
            return data;
        }

        /// <inheritdoc/>
        public virtual Task<IDataObject?> GetDataAsync()
        {
            var result = GetData();
            return Task.FromResult(result);
        }

        /// <inheritdoc/>
        public virtual void GetDataAsync(Action<IDataObject?> action)
        {
            var result = GetData();
            action(result);
        }

        /// <inheritdoc/>
        public virtual void SetData(IDataObject? value)
        {
            data = value;
        }

        /// <inheritdoc/>
        public virtual Task SetDataAsync(IDataObject? value)
        {
            SetData(value);
            return Task.CompletedTask;
        }
    }
}
