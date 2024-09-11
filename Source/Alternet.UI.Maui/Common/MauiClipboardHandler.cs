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

        public void SetData(IDataObject? value)
        {
            var data = value?.GetData(DataFormats.Text);
            SystemClipboard.SetTextAsync(data?.ToString());
        }
    }
}
