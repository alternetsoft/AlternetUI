using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxClipboardHandler : DisposableObject, IClipboardHandler
    {
        public bool AsyncRequired => false;

        public bool OnlyText => false;

        public Task<IDataObject?> GetDataAsync()
        {
            var result = GetData();
            return Task.FromResult(result);
        }

        public void GetDataAsync(Action<IDataObject?> action)
        {
            var result = GetData();
            action(result);
        }

        public IDataObject? GetData()
        {
            var unmanagedDataObject =
                WxApplicationHandler.NativeClipboard.GetDataObject();
            if (unmanagedDataObject == null)
                return null;

            return new UnmanagedDataObjectAdapter(unmanagedDataObject);
        }

        public void SetData(IDataObject? value)
        {
            value ??= DataObject.Empty;
            WxApplicationHandler.NativeClipboard.SetDataObject(
                UnmanagedDataObjectService.GetUnmanagedDataObject(value));
        }

        public Task SetDataAsync(IDataObject? value)
        {
            SetData(value);
            return Task.CompletedTask;
        }
    }
}
