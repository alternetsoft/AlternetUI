using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace Alternet.UI
{
    internal partial class MauiClipboardHandler : DisposableObject, IClipboardHandler
    {
        private IDataObject? lastData;

        public MauiClipboardHandler()
        {
            SystemClipboard.ClipboardContentChanged += (s, e) =>
            {
                lastData = null;
            };
        }

        public bool AsyncRequired => true;

        public bool OnlyText => true;

        private Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard SystemClipboard =>
            Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.Default;

        public bool Flush()
        {
            return false;
        }

        public IDataObject? GetData()
        {
            var text = SystemClipboard.GetTextAsync().Result;

            if (lastData is not null)
            {
                var lastText = lastData.GetData(DataFormats.Text)?.ToString();
                if (lastText == text)
                    return lastData;
            }

            if (text is null)
                return null;

            var data = new DataObject(text);
            return data;
        }

        public Task<IDataObject?> GetDataAsync()
        {
            if (!SystemClipboard.HasText)
                return Task.FromResult<IDataObject?>(null);

            var stringTask = SystemClipboard.GetTextAsync();

            var resultTask = stringTask.ContinueWith(Fn);

            IDataObject? Fn(Task<string?> t)
            {
                var result = t.Result;
                if(result is null)
                    return null;
                var data = new DataObject(result);
                return data;
            }

            return resultTask;
        }

        public void GetDataAsync(Action<IDataObject?> action)
        {
            var task = GetDataAsync();
            task.ContinueWith((t) =>
            {
                var result = t.Result;
                action(result);
            });
        }

        /// <inheritdoc/>
        public virtual bool HasFormat(ClipboardDataFormatId format)
        {
            var result = lastData?.HasFormat(format) ?? false;

            if (result)
                return true;

            switch (format)
            {
                case ClipboardDataFormatId.UnicodeText:
                case ClipboardDataFormatId.Text:
                case ClipboardDataFormatId.OemText:
                    return SystemClipboard.HasText;
                case ClipboardDataFormatId.Filename:
                case ClipboardDataFormatId.Dib:
                case ClipboardDataFormatId.Bitmap:
                case ClipboardDataFormatId.Invalid:
                case ClipboardDataFormatId.MetaFile:
                case ClipboardDataFormatId.Sylk:
                case ClipboardDataFormatId.Dif:
                case ClipboardDataFormatId.Tiff:
                case ClipboardDataFormatId.Palette:
                case ClipboardDataFormatId.PenData:
                case ClipboardDataFormatId.Riff:
                case ClipboardDataFormatId.Wave:
                case ClipboardDataFormatId.EnhMetaFile:
                case ClipboardDataFormatId.Locale:
                case ClipboardDataFormatId.Private:
                case ClipboardDataFormatId.Html:
                case ClipboardDataFormatId.Png:
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public virtual bool HasFormat(string format)
        {
            if (format == DataFormats.Text || format == DataFormats.UnicodeText)
                return SystemClipboard.HasText;

            var result = lastData?.HasFormat(format) ?? false;

            return result;
        }

        public void SetData(IDataObject? value)
        {
            SetDataAsync(value);
        }

        public Task SetDataAsync(IDataObject? value)
        {
            var data = value?.GetData(DataFormats.Text);
            return SystemClipboard
                .SetTextAsync(data?.ToString())
                .ContinueWith(
                (t) =>
                {
                    Invoke(() =>
                    {
                        lastData = value;
                    });
                });
        }
    }
}
