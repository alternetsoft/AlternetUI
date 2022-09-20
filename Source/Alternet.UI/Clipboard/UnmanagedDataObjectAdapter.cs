using Alternet.UI.Native;
using System;
using System.IO;
using Image = Alternet.Drawing.Image;

namespace Alternet.UI
{
    internal class UnmanagedDataObjectAdapter : IDataObject
    {
        private UnmanagedDataObject dataObject;

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
                return new Image(new UnmanagedStreamAdapter(dataObject.GetStreamData(format)));

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
            SetData(DataObjectHelpers.DetectFormatFromData(data), data);
        }
    }
}