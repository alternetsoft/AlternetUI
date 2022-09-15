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
            if (format == DataFormats.Text)
                dataObject.SetStringData(format, (string)DataObject.SetDataTransform(format, data));

            if (format == DataFormats.Files)
                dataObject.SetFileNamesData(format, string.Join("|", (string)DataObject.SetDataTransform(format, data)));

            if (format == DataFormats.Bitmap)
            {
                var image = (Image)DataObject.SetDataTransform(format, data);
                using (var stream = new MemoryStream())
                {
                    image.Save(stream);
                    dataObject.SetStreamData(format, new InputStream(stream));
                }
            }

            throw new NotSupportedException();
        }

        public void SetData(object data)
        {
            SetData(DataObject.DetectFormatFromData(data), data);
        }
    }
}