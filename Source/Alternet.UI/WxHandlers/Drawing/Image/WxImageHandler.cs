using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class Image : Alternet.Drawing.IImageHandler
    {
        public bool SaveToStream(Stream stream, Alternet.Drawing.ImageFormat format, int quality)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return SaveToStream(outputStream, format.ToString());
        }

        public bool LoadFromStream(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            if (inputStream is null)
                return false;
            return LoadFromStream(inputStream);
        }

        public bool Load(string name, BitmapType type)
        {
            return LoadFile(name, (int)type);
        }

        public bool LoadFromStream(Stream stream, BitmapType type)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return LoadStream(inputStream, (int)type);
        }

        public bool SaveToFile(string name, BitmapType type, int quality)
        {
            return SaveToFile(name);
        }

        public bool SaveToStream(Stream stream, BitmapType type, int quality)
        {
            using var outputStream = new UI.Native.OutputStream(stream);
            return SaveStream(outputStream, (int)type);
        }

        public Alternet.Drawing.GenericImage ToGenericImage()
        {
            return Alternet.Drawing.WxGenericImageHandler.Create(ConvertToGenericImage());
        }

        Alternet.Drawing.IImageHandler Alternet.Drawing.IImageHandler.ConvertToDisabled(byte brightness)
        {
            return ConvertToDisabled(brightness);
        }

        Alternet.Drawing.IImageHandler Alternet.Drawing.IImageHandler.GetSubBitmap(Alternet.Drawing.RectI rect)
        {
            return GetSubBitmap(rect);
        }
    }
}
