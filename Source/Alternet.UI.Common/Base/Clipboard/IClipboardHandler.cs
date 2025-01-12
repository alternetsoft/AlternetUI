using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties and methods which allow to work with clipboard.
    /// </summary>
    public interface IClipboardHandler : IDisposable
    {
        /// <inheritdoc cref="Clipboard.AsyncRequired"/>
        bool AsyncRequired { get; }

        /// <inheritdoc cref="Clipboard.OnlyText"/>
        bool OnlyText { get; }

        /// <inheritdoc cref="Clipboard.GetDataObject"/>
        IDataObject? GetData();

        /// <inheritdoc cref="Clipboard.GetDataObjectAsync()"/>
        Task<IDataObject?> GetDataAsync();

        /// <inheritdoc cref="Clipboard.GetDataObjectAsync(Action{IDataObject?})"/>
        void GetDataAsync(Action<IDataObject?> action);

        /// <inheritdoc cref="Clipboard.SetDataObject(IDataObject?)"/>
        void SetData(IDataObject? value);

        /// <inheritdoc cref="Clipboard.SetDataObjectAsync(IDataObject?)"/>
        Task SetDataAsync(IDataObject? value);

        /// <inheritdoc cref="Clipboard.HasFormat(ClipboardDataFormatId)"/>
        bool HasFormat(ClipboardDataFormatId format);

        /// <inheritdoc cref="Clipboard.HasFormat(string)"/>
        bool HasFormat(string format);

        /// <inheritdoc cref="Clipboard.Flush()"/>
        bool Flush();
    }
}
