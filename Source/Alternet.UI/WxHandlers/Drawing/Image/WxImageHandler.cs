using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.UI.Native
{
    internal partial class Image : Alternet.Drawing.IImageHandler
    {
        public int Width => PixelWidth;

        public int Height => PixelHeight;

        public SKAlphaType AlphaType
        {
            get
            {
                if(!HasAlpha)
                    return SKAlphaType.Opaque;

                if (App.IsWindowsOS)
                {
                    return SKAlphaType.Premul;
                }
                else
                if (App.IsLinuxOS)
                {
                    return SKAlphaType.Unpremul;
                }
                else
                if (App.IsMacOS)
                {
                    return SKAlphaType.Premul;
                }
                else
                {
                    return SKAlphaType.Premul;
                }
            }
        }

        public Alternet.Drawing.ImageBitsFormatKind BitsFormat
        {
            get
            {
                if(Depth != 24 && Depth != 32)
                    return Alternet.Drawing.ImageBitsFormatKind.Unknown;
                if (HasMask)
                    return Alternet.Drawing.ImageBitsFormatKind.Unknown;
                if (HasAlpha)
                    return Alternet.Drawing.ImageBitsFormatKind.Alpha;
                else
                    return Alternet.Drawing.ImageBitsFormatKind.Native;
            }
        }

        public void Assign(SKBitmap bitmap)
        {
            Alternet.Drawing.GenericImage image = (Alternet.Drawing.GenericImage)bitmap;
            Assign(image);
        }

        public void Assign(Alternet.Drawing.GenericImage image)
        {
            var depth = image.HasAlpha ? 32 : 24;
            LoadFromGenericImage(Alternet.Drawing.WxGenericImageHandler.GetPtr(image), depth);
        }

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

        public void SetImmutable()
        {
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

        Alternet.Drawing.IImageHandler Alternet.Drawing.IImageHandler.GetSubBitmap(Alternet.Drawing.RectI rect)
        {
            return GetSubBitmap(rect);
        }
    }
}
