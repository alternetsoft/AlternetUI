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
        public bool AsyncRequired => true;

        public bool OnlyText => true;

        private Microsoft.Maui.ApplicationModel.DataTransfer.IClipboard SystemClipboard =>
            Microsoft.Maui.ApplicationModel.DataTransfer.Clipboard.Default;

        public IDataObject? GetData()
        {
            if(!SystemClipboard.HasText)
                return null;
            var text = SystemClipboard.GetTextAsync().Result;
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
            return SystemClipboard.SetTextAsync(data?.ToString());
        }
    }
}
