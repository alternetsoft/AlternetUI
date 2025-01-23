using System;
using System.IO;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal static class UnmanagedDataObjectService
    {
        public static Native.UnmanagedDataObject GetUnmanagedDataObject(object input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            if (input is IDataObject obj)
                return GetUnmanagedDataObject(obj);

            var output = new Native.UnmanagedDataObject();
            var adapter = new UnmanagedDataObjectAdapter(output);
            adapter.SetData(input);

            return output;
        }

        public static Native.UnmanagedDataObject GetUnmanagedDataObject(IDataObject input)
        {
            var output = new Native.UnmanagedDataObject();
            foreach (var format in input.GetFormats())
                CopyData(input, output, format);

            return output;
        }

        public static void SetData(Native.UnmanagedDataObject dataObject, string format, object data)
        {
            if (data == null)
                return;

            if(format == DataFormats.Rtf && data is string stringData)
            {
                using var stream = new MemoryStream();
                StreamUtils.StringToStream(stream, stringData, Encoding.ASCII);
                stream.Position = 0;
                dataObject.SetStreamData(format, new Native.InputStream(stream));
                return;
            }

            if (format == DataFormats.Text || data is string)
            {
                dataObject.SetStringData(
                    format,
                    (string)ClipboardUtils.SetDataTransform(format, data));
                return;
            }

            if (format == DataFormats.Files || data is FileInfo[])
            {
                dataObject.SetFileNamesData(format, string.Join("|", GetFileNames(data)));
                return;
            }

            if (format == DataFormats.Bitmap || data is Image)
            {
                var image = (Image)data;
                using var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                dataObject.SetStreamData(format, new Native.InputStream(stream));
                return;
            }

            if(format == DataFormats.Serializable)
            {
                using var stream = new MemoryStream();
                DataObject.SerializeDataObject(stream, data);
                stream.Position = 0;
                dataObject.SetStreamData(
                    DataFormats.AlternetUISerializable,
                    new Native.InputStream(stream));
                return;
            }

            if(data is Stream streamData)
            {
                if (streamData.CanSeek)
                {
                    streamData.Position = 0;
                }
                else
                {
                    App.LogWarning(
                        new NotSupportedException(
                            "UnmanagedDataObjectService.SetData: Stream doesn't support seeking"));
                }

                dataObject.SetStreamData(format, new Native.InputStream(streamData));
                return;
            }

            App.LogError(
                new NotSupportedException("This type of data is not supported: " + data.GetType()));
        }

        private static void CopyData(
            IDataObject input,
            Native.UnmanagedDataObject output,
            string format)
        {
            var data = input.GetData(format);
            if (data == null)
                return;

            SetData(output, format, data);
        }

        private static string[] GetFileNames(object data)
        {
            return data switch
            {
                FileInfo[] x => x.Select(x => x.FullName).ToArray(),
                string[] x => x,
                _ => throw new Exception()
            };
        }
    }
}