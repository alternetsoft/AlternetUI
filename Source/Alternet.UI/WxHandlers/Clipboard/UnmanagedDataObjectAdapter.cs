using System;
using System.IO;

using Alternet.Drawing;
using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class UnmanagedDataObjectAdapter : IDataObject, IDoCommand
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

            if (format == DataFormats.Serializable)
            {
                var nativeStream = dataObject.GetStreamData(DataFormats.AlternetUISerializable);
                if (nativeStream is null)
                    return null;
                var stream = new UnmanagedStreamAdapter(nativeStream);
                var data = DataObject.DeserializeDataObject(stream);
                return data;
            }

            return null;
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

            for(int i = 1; i < (int)ClipboardDataFormatId.Max; i++)
            {
                var format = (ClipboardDataFormatId)i;
                var present = GetNativeDataPresent(format);

                if(present)
                {
                    App.Log($"Clipboard contains format: {format}");
                }
            }

            App.LogEndSection();
        }

        internal bool GetNativeDataPresent(ClipboardDataFormatId format)
        {
            if (!DataFormats.IsValidOnWindowsMacLinux(format))
                return false;
            return dataObject.GetNativeDataPresent((int)format);
        }

        public bool GetDataPresent(string format)
        {
            if (format == DataFormats.Serializable)
                format = DataFormats.AlternetUISerializable;

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

        public bool HasFormat(ClipboardDataFormatId format)
        {
            if (!DataFormats.IsValidOnWindowsMacLinux(format))
                return false;
            return dataObject.GetNativeDataPresent((int)format);
        }

        public bool HasFormat(string format)
        {
            return dataObject.GetDataPresent(format);
        }
    }
}