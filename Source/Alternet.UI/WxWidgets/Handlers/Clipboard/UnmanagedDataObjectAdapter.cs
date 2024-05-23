using System;
using Alternet.Drawing;
using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class UnmanagedDataObjectAdapter : IDataObject
    {
        private readonly UnmanagedDataObject dataObject;

        internal UnmanagedDataObjectAdapter(UnmanagedDataObject dataObject)
        {
            this.dataObject = dataObject;
        }

        public object? GetData(string format)
        {
            if (format == DataFormats.Text)
                return dataObject.GetStringData(format);

            if (format == DataFormats.Files)
                return dataObject.GetFileNamesData(format).Split('|');

            if (format == DataFormats.Bitmap)
                return new Bitmap(new UnmanagedStreamAdapter(dataObject.GetStreamData(format)));

            throw new NotSupportedException();
        }

        public bool GetDataPresent(string format)
        {
            return dataObject.GetDataPresent(format);
        }

        public string[] GetFormats()
        {
            return dataObject.Formats;
        }

        public void SetData(string format, object data)
        {
            UnmanagedDataObjectService.SetData(dataObject, format, data);
        }

        public void SetData(object data)
        {
            SetData(ClipboardUtils.DetectFormatFromData(data), data);
        }
    }
}