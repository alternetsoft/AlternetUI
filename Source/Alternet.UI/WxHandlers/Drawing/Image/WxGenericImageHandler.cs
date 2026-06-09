using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    internal class WxGenericImageHandler : DisposableObject<IntPtr>, IGenericImageHandler
    {
        public SKAlphaType AlphaType => HasAlpha ? SKAlphaType.Unpremul : SKAlphaType.Opaque;

        public ImageBitsFormatKind BitsFormat
        {
            get
            {
                return ImageBitsFormatKind.Generic;
            }
        }

        public unsafe SKColor[] Pixels
        {
            get
            {
                if (!IsOk)
                    return Array.Empty<SKColor>();
                var pixels = DrawingUtils.CreatePixels(Width, Height);

                var rgb = UI.Native.GenericImage.GetData(Handle);
                DrawingUtils.SetRgbValuesFromPtr(pixels, (RGBValue*)rgb);

                if (HasAlpha)
                {
                    var alpha = UI.Native.GenericImage.GetAlphaData(Handle);
                    DrawingUtils.SetAlphaValuesFromPtr(pixels, (byte*)alpha);
                }
                else
                {
                    DrawingUtils.FillAlphaData(pixels, 255);
                }

                return pixels;
            }

            set
            {
                if (!IsOk)
                    return;
                DrawingUtils.SeparateAlphaData(value, out var rgb, out var alpha);
                var alphaPtr = AllocAlphaData(Width, Height, alpha);
                var dataPtr = AllocData(Width, Height, rgb);
                UI.Native.GenericImage.SetData(Handle, dataPtr, false);
                UI.Native.GenericImage.SetAlphaData(Handle, alphaPtr, false);
            }
        }

        public RGBValue[] RgbData
        {
            get
            {
                if (!IsOk)
                    return Array.Empty<RGBValue>();

                var dataPtr = UI.Native.GenericImage.GetData(Handle);
                var result = DrawingUtils.CreateRgbDataFromPtr(Width, Height, dataPtr);
                return result;
            }

            set
            {
                if (!IsOk)
                    return;
                var dataPtr = AllocData(Width, Height, value);
                UI.Native.GenericImage.SetData(Handle, dataPtr, false);
            }
        }

        public byte[] AlphaData
        {
            get
            {
                if (!IsOk)
                    return Array.Empty<byte>();

                byte[] result;

                if (!HasAlpha)
                {
                    result = DrawingUtils.CreateAlphaData(Width, Height, 255);
                    return result;
                }

                var alphaPtr = UI.Native.GenericImage.GetAlphaData(Handle);
                result = DrawingUtils.CreateAlphaDataFromPtr(Width, Height, alphaPtr);
                return result;
            }

            set
            {
                if (!IsOk)
                    return;
                if(!HasAlpha)
                    InitAlpha();
                var alphaPtr = AllocAlphaData(Width, Height, value);
                UI.Native.GenericImage.SetAlphaData(Handle, alphaPtr, false);
            }
        }

        public IntPtr LockBits() => UI.Native.GenericImage.LockBits(Handle);

        public void UnlockBits() => UI.Native.GenericImage.UnlockBits(Handle);

        public int GetStride() => UI.Native.GenericImage.GetStride(Handle);

        public int Width => UI.Native.GenericImage.GetWidth(Handle);

        public int Height => UI.Native.GenericImage.GetHeight(Handle);

        public bool IsOk => UI.Native.GenericImage.IsOk(Handle);

        public bool HasAlpha => UI.Native.GenericImage.HasAlpha(Handle);

        public bool HasMask => UI.Native.GenericImage.HasMask(Handle);

        public WxGenericImageHandler(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public WxGenericImageHandler()
            : base(UI.Native.GenericImage.CreateImage(), true)
        {
        }

        public WxGenericImageHandler(int width, int height, bool clear = false)
            : base(
                  UI.Native.GenericImage.CreateImageWithSize(width, height, clear),
                  true)
        {
        }

        public WxGenericImageHandler(SizeI size, bool clear = false)
            : base(
                  UI.Native.GenericImage.CreateImageWithSize(size.Width, size.Height, clear),
                  true)
        {
        }

        public WxGenericImageHandler(
            string fileName,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromFileWithBitmapType(
                    fileName,
                    (int)bitmapType,
                    index),
                  true)
        {
        }

        public WxGenericImageHandler(string name, string mimetype, int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromFileWithMimeType(name, mimetype, index),
                  true)
        {
        }

        public WxGenericImageHandler(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromStreamWithBitmapData(
                      new UI.Native.InputStream(stream),
                      (int)bitmapType,
                      index),
                  true)
        {
        }

        public WxGenericImageHandler(
            Stream stream,
            string mimeType,
            int index = -1)
            : base(
                  UI.Native.GenericImage.CreateImageFromStreamWithMimeType(
                      new UI.Native.InputStream(stream),
                      mimeType,
                      index),
                  true)
        {
        }

        public WxGenericImageHandler(int width, int height, SKColor[] data)
            : base(true)
        {
            Handle = UI.Native.GenericImage.CreateImageWithSize(width, height, false);
            InitAlpha();
            Pixels = data;
        }

        public WxGenericImageHandler(int width, int height, RGBValue[] data)
            : base(true)
        {
            var dataPtr = AllocData(width, height, data);
            Handle = UI.Native.GenericImage.CreateImageWithSizeAndData(width, height, dataPtr, false);
        }

        public unsafe WxGenericImageHandler(int width, int height, RGBValue[] data, byte[] alpha)
            : base(true)
        {
            var dataPtr = AllocData(width, height, data);
            var alphaPtr = AllocAlphaData(width, height, alpha);

            Handle = UI.Native.GenericImage.CreateImageWithAlpha(width, height, dataPtr, alphaPtr, false);
        }

        public static GenericImage Create(IntPtr ptr)
        {
            var handler = new WxGenericImageHandler(ptr, true);
            return new GenericImage(handler);
        }

        public static IntPtr GetPtr(GenericImage image)
        {
            var handler = image?.Handler as WxGenericImageHandler;
            return handler?.Handle ?? default;
        }

        public void SetAlpha(int x, int y, byte alpha)
        {
            UI.Native.GenericImage.SetAlpha(Handle, x, y, alpha);
        }

        public void ClearAlpha()
        {
            UI.Native.GenericImage.ClearAlpha(Handle);
        }

        public void SetMask(bool hasMask = true)
        {
            UI.Native.GenericImage.SetMask(Handle, hasMask);
        }

        public void SetMaskColor(RGBValue rgb)
        {
            UI.Native.GenericImage.SetMaskColor(Handle, rgb.R, rgb.G, rgb.B);
        }

        /// <inheritdoc/>
        public void SetOptionAsString(string name, string value)
        {
            UI.Native.GenericImage.SetOptionString(Handle, name, value);
        }

        public void SetOptionAsInt(string name, int value)
        {
            UI.Native.GenericImage.SetOptionInt(Handle, name, value);
        }

        public void SetRGB(int x, int y, RGBValue rgb)
        {
            UI.Native.GenericImage.SetRGB(Handle, x, y, rgb.R, rgb.G, rgb.B);
        }

        public void SetRGBRect(RGBValue rgb, RectI? rect = null)
        {
            rect ??= RectI.Create(Width, Height);
            UI.Native.GenericImage.SetRGBRect(Handle, rect.Value, rgb.R, rgb.G, rgb.B);
        }

        public void SetImageType(BitmapType type)
        {
            UI.Native.GenericImage.SetImageType(Handle, (int)type);
        }

        public bool Reset(int width, int height, bool clear = false)
        {
            return UI.Native.GenericImage.CreateFreshImage(Handle, width, height, clear);
        }

        public void Clear(byte value = 0)
        {
            UI.Native.GenericImage.Clear(Handle, value);
        }

        public void Reset()
        {
            UI.Native.GenericImage.DestroyImageData(Handle);
        }

        public Color FindFirstUnusedColor(RGBValue? startRGB = null)
        {
            var value = startRGB ?? new(1, 0, 0);

            return UI.Native.GenericImage.FindFirstUnusedColor(
                Handle,
                value.R,
                value.G,
                value.B);
        }

        public void InitAlpha()
        {
            if (HasAlpha)
                return;
            UI.Native.GenericImage.InitAlpha(Handle);
        }

        public void ResizeNoScale(
            SizeI size,
            PointI pos,
            RGBValue? color = null)
        {
            if (color is null)
            {
                UI.Native.GenericImage.Resize(Handle, size, pos, -1, -1, -1);
                return;
            }

            var red = color.Value.R;
            var green = color.Value.G;
            var blue = color.Value.B;

            UI.Native.GenericImage.Resize(Handle, size, pos, red, green, blue);
        }

        public void RotateHue(double angle)
        {
            UI.Native.GenericImage.RotateHue(Handle, angle);
        }

        public void ChangeSaturation(double factor)
        {
            UI.Native.GenericImage.ChangeSaturation(Handle, factor);
        }

        public void ChangeBrightness(double factor)
        {
            UI.Native.GenericImage.ChangeBrightness(Handle, factor);
        }

        public bool ConvertAlphaToMask(byte threshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMask(Handle, threshold);
        }

        public bool ConvertAlphaToMask(
            RGBValue rgb,
            byte threshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMaskUseColor(
                Handle,
                rgb.R,
                rgb.G,
                rgb.B,
                threshold);
        }

        public byte GetAlpha(int x, int y)
        {
            return UI.Native.GenericImage.GetAlpha(Handle, x, y);
        }

        public RGBValue GetRGB(int x, int y)
        {
            var r = GetRed(x, y);
            var g = GetGreen(x, y);
            var b = GetBlue(x, y);
            return new(r, g, b);
        }

        public Color GetPixel(
            int x,
            int y,
            bool withAlpha = false)
        {
            var r = GetRed(x, y);
            var g = GetGreen(x, y);
            var b = GetBlue(x, y);
            var a = (withAlpha && HasAlpha) ? GetAlpha(x, y) : (byte)255;
            return Color.FromArgb(a, r, g, b);
        }

        public void SetPixel(
            int x,
            int y,
            Color color,
            bool withAlpha = false)
        {
            color.GetArgbValues(out var a, out var r, out var g, out var b);
            SetRGB(x, y, new RGBValue(r, g, b));
            if (withAlpha && HasAlpha)
                SetAlpha(x, y, a);
        }

        public byte GetRed(int x, int y)
        {
            return UI.Native.GenericImage.GetRed(Handle, x, y);
        }

        public byte GetGreen(int x, int y)
        {
            return UI.Native.GenericImage.GetGreen(Handle, x, y);
        }

        public byte GetBlue(int x, int y)
        {
            return UI.Native.GenericImage.GetBlue(Handle, x, y);
        }

        public RGBValue GetMaskRGB()
        {
            var r = GetMaskRed();
            var g = GetMaskGreen();
            var b = GetMaskBlue();
            return new(r, g, b);
        }

        public byte GetMaskRed()
        {
            return UI.Native.GenericImage.GetMaskRed(Handle);
        }

        public byte GetMaskGreen()
        {
            return UI.Native.GenericImage.GetMaskGreen(Handle);
        }

        public byte GetMaskBlue()
        {
            return UI.Native.GenericImage.GetMaskBlue(Handle);
        }

        public string GetOptionAsString(string name)
        {
            return UI.Native.GenericImage.GetOptionString(Handle, name);
        }

        public int GetOptionAsInt(string name)
        {
            return UI.Native.GenericImage.GetOptionInt(Handle, name);
        }

        public BitmapType GetImageType()
        {
            return (BitmapType)UI.Native.GenericImage.GetImageType(Handle);
        }

        public bool HasOption(string name)
        {
            return UI.Native.GenericImage.HasOption(Handle, name);
        }

        public bool IsTransparent(int x, int y, byte threshold)
        {
            return UI.Native.GenericImage.IsTransparent(Handle, x, y, threshold);
        }

        public bool LoadFromStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithBitmapType(
                Handle,
                inputStream,
                (int)bitmapType,
                index);
        }

        public bool LoadFromFile(
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithBitmapType(
                Handle,
                filename,
                (int)bitmapType,
                index);
        }

        public bool LoadFromFile(
            string name,
            string mimetype,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithMimeType(Handle, name, mimetype, index);
        }

        public bool LoadFromStream(
            Stream stream,
            string mimetype,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithMimeType(Handle, inputStream, mimetype, index);
        }

        public bool SaveToStream(Stream stream, string mimetype)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithMimeType(Handle, outputStream, mimetype);
        }

        public bool SaveToFile(string filename, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.SaveFileWithBitmapType(Handle, filename, (int)bitmapType);
        }

        public bool SaveToFile(string filename, string mimetype)
        {
            return UI.Native.GenericImage.SaveFileWithMimeType(Handle, filename, mimetype);
        }

        public bool SaveToFile(string filename)
        {
            return UI.Native.GenericImage.SaveFile(Handle, filename);
        }

        public bool SaveToStream(Stream stream, BitmapType type)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithBitmapType(Handle, outputStream, (int)type);
        }

        public void SetNativeData(
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false)
        {
            UI.Native.GenericImage.SetDataWithSize(Handle, data, new_width, new_height, static_data);
        }

        public IntPtr GetNativeAlphaData()
        {
            return UI.Native.GenericImage.GetAlphaData(Handle);
        }

        public IntPtr GetNativeData()
        {
            return UI.Native.GenericImage.GetData(Handle);
        }

        public bool CreateNativeData(
            int width,
            int height,
            IntPtr data,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateData(Handle, width, height, data, staticData);
        }

        public bool CreateNativeData(
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateAlphaData(Handle, width, height, data, alpha, staticData);
        }

        public void SetNativeAlphaData(
            IntPtr alpha = default,
            bool staticData = false)
        {
            UI.Native.GenericImage.SetAlphaData(Handle, alpha, staticData);
        }

        public void SetNativeData(IntPtr data, bool staticData = false)
        {
            UI.Native.GenericImage.SetData(Handle, data, staticData);
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            UI.Native.GenericImage.DeleteImage(Handle);
        }

        private unsafe IntPtr AllocAlphaData(int width, int height, byte[] alpha)
        {
            var size = width * height;

            if (size != alpha.Length)
                throw new ArgumentException("Invalid alpha array length.");

            var alphaPtr = BaseMemory.Alloc(size);

            fixed (byte* sourceAlphaPtr = alpha)
            {
                BaseMemory.Move(alphaPtr, (IntPtr)sourceAlphaPtr, size);
            }

            return alphaPtr;
        }

        private unsafe IntPtr AllocData(int width, int height, RGBValue[] data)
        {
            var size = width * height;
            var rgbSize = size * 3;

            if (size != data.Length)
                throw new ArgumentException("Invalid RGB array length.");

            var dataPtr = BaseMemory.Alloc(rgbSize);

            fixed (RGBValue* sourcePtr = data)
            {
                BaseMemory.Move(dataPtr, (IntPtr)sourcePtr, rgbSize);
            }

            return dataPtr;
        }
    }
}
