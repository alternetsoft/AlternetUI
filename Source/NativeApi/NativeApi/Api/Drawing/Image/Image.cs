#pragma warning disable
using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_bitmap.html
    public class Image
    {
        // Gets or sets the scale factor of this bitmap.
        // Scale factor is 1 by default, but can be greater to indicate that the size of
        // bitmap in logical, DPI-independent pixels is smaller than its actual size in
        // physical pixels. Bitmaps with scale factor greater than 1 must be used in high DPI
        // to appear sharp on the screen.
        // Note that the scale factor is only used in the ports where logical pixels are not the same
        // as physical ones, such as MacOs or Linux, and this function always returns 1 under
        // the other platforms.
        // Setting scale to 2 means that the bitmap will be twice smaller (in each direction) when
        // drawn on screen in the ports in which logical and physical pixels differ
        // (i.e. MacOs and Linux, but not Windows). This doesn't change the bitmap actual size
        // or its contents, but changes its scale factor, so that it appears in a smaller
        // size when it is drawn on screen:
        public double AAAScaleFactor { get; set; }

        // Returns the size of bitmap in DPI-independent units.
        // This assumes that the bitmap was created using the value of scale factor corresponding
        // to the current DPI and returns its physical size divided by this scale factor.
        // Unlike LogicalSize, this function returns the same value under all platforms
        // and so its result should not be used as window or device context coordinates.
        Int32Size AAADipSize { get; }

        // Returns the height of the bitmap in logical pixels.
        public double AAAScaledHeight { get; }

        // Returns the size of the bitmap in logical pixels.        
        public Int32Size AAAScaledSize { get; }

        // Returns the width of the bitmap in logical pixels.
        public double AAAScaledWidth { get; }

        // Create a bitmap compatible with the given DC, inheriting its magnification factor.
        // width-The width of the bitmap in pixels, must be strictly positive.
        // height-The height of the bitmap in pixels, must be strictly positive.
        // dc-DC from which the scaling factor is inherited
        public void AAAInitializeFromDrawingContext(int width, int height, DrawingContext dc) { }

        // Create a bitmap specifying its size in DPI-independent pixels and the scale factor to use.
        // The physical size of the bitmap is obtained by multiplying the given size by scale and
        // rounding it to the closest integer.
        // After using this function the following postconditions are true:
        // GetSize() returns size multiplied by scale
        // GetDIPSize() returns size
        // GetScaleFactor() returns scale
        // size - The size of the bitmap in DPI-independent pixels. Both width and height must
        // be strictly positive.
        // scale - Scale factor used by the bitmap, see SetScaleFactor().
        // depth - The number of bits used to represent each bitmap pixel.
        // true - if the creation was successful.          
        public bool AAAInitializeFromDipSize(int width, int height,
            double scale, int depth = -1) => default;

        // Creates a bitmap compatible with the given DC from the given image.
        // This constructor initializes the bitmap with the data of the given image, which
        // must be valid, but inherits the scaling factor from the given device context
        // instead of simply using the default factor of 1.
        // img Platform-independent wxImage object.
        // dc DC from which the scaling factor is inherited
        public void AAAInitializeFromGenericImageDC(Image source, DrawingContext dc) { }

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