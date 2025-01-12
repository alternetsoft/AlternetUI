using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxClipboardHandler : DisposableObject, IClipboardHandler, IDoCommand
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

            var unmanaged = UnmanagedDataObjectService.GetUnmanagedDataObject(value);
            
            WxApplicationHandler.NativeClipboard.SetDataObject(unmanaged);
        }

        public Task SetDataAsync(IDataObject? value)
        {
            SetData(value);
            return Task.CompletedTask;
        }

        public object? DoCommand(string cmdName, params object?[] args)
        {
            if(cmdName == "log")
            {
                Log();
            }

            return null;
        }

        public void Log()
        {
            App.LogBeginSection();

            for (int i = 1; i < (int)ClipboardDataFormatId.Max; i++)
            {
                var format = (ClipboardDataFormatId)i;
                var present = WxApplicationHandler.NativeClipboard.IsIntFormatSupported(i);

                if (present)
                {
                    App.Log($"Clipboard contains format: {format}");
                }
            }

            App.LogEndSection();
        }
    }
}
