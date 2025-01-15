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

        public bool Flush()
        {
            return WxApplicationHandler.NativeClipboard.Flush();
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
                var present = HasFormat(format);

                if (present)
                {
                    App.Log($"Clipboard contains format: {format}");
                }
            }

            var s = "WindowsForms10PersistentObject";

            var persistentSupported = WxApplicationHandler.NativeClipboard
                .IsStrFormatSupported(s);
            if(persistentSupported)
                App.Log($"Clipboard contains format: {s}");

            App.LogEndSection();
        }

        public bool HasFormat(ClipboardDataFormatId format)
        {
            if(!DataFormats.IsValidOnWindowsMacLinux(format))
                return false;
            var present = WxApplicationHandler.NativeClipboard.IsIntFormatSupported((int)format);
            return present;
        }

        public bool HasFormat(string format)
        {
            var result = WxApplicationHandler.NativeClipboard.IsStrFormatSupported(format);
            return result;
        }
    }
}
