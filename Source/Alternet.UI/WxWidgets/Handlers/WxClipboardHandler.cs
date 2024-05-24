using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxClipboardHandler : DisposableObject, IClipboardHandler
    {
        public IDataObject? GetData()
        {
            var unmanagedDataObject =
                WxApplicationHandler.NativeClipboard.GetDataObject();
            if (unmanagedDataObject == null)
                return null;

            return new UnmanagedDataObjectAdapter(unmanagedDataObject);
        }

        public void SetData(IDataObject value)
        {
            WxApplicationHandler.NativeClipboard.SetDataObject(
                UnmanagedDataObjectService.GetUnmanagedDataObject(value));
        }
    }
}
