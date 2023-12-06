#pragma warning disable
using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class Image
    {
        public bool LoadFromStream(InputStream stream) => default;
        public bool LoadSvgFromStream(InputStream stream, int width, int height, Color color) => default;
        public void Initialize(Int32Size size) { }
        public void InitializeFromImage(Image source, Int32Size size) { }
        public void CopyFrom(Image otherImage) { }

        public bool SaveToStream(OutputStream stream, string format) => default;
        public bool SaveToFile(string fileName) => default;

        public IntPtr ConvertToGenericImage() => default;
        public void LoadFromGenericImage(IntPtr image, int depth = -1) { }

        public Int32Size PixelSize { get; }

        public bool IsOk { get; }

        public bool GrayScale() => throw new Exception();

        public bool HasAlpha { get;set; }

        public void ResetAlpha() { }

        public int PixelWidth { get; }
        public int PixelHeight { get; }

        public bool LoadFile(string name, int type /*= wxBITMAP_DEFAULT_TYPE*/) => default;

        public bool SaveFile(string name, int type) => default;

        public bool SaveStream(OutputStream stream, int type) => default;

        public bool LoadStream(InputStream stream, int type) => default;

        // Returns a sub bitmap of the current one as long as the rect belongs entirely to the bitmap.
        public Image GetSubBitmap(Int32Rect rect) => default;
             
        // Returns disabled (dimmed) version of the bitmap.
        public Image ConvertToDisabled(byte brightness = 255) => default;

        // Gets the color depth of the bitmap.
        public int Depth { get; }

        // Rescale the given bitmap to the requested size.
        // This function is just a convenient wrapper for wxImage::Rescale() used to
        // resize the given bmp to the requested size.If you need more control over
        // resizing, e.g.to specify the quality option different from
        // wxIMAGE_QUALITY_NEAREST used by this function, please use the wxImage function
        // directly instead. Both the bitmap itself and size must be valid.
        public void Rescale(Int32Size sizeNeeded) { }

        public static int GetDefaultBitmapType() => default;
    }
}