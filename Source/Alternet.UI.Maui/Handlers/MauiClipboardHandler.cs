using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace Alternet.UI
{
    internal class MauiClipboardHandler : DisposableObject, IClipboardHandler
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
