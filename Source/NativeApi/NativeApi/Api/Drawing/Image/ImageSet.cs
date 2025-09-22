#pragma warning disable
using NativeApi.Api.ManagedServers;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class ImageSet
    {
        public void LoadFromStream(InputStream stream) { }
        public void AddImage(Image image) { }
        public void Clear() { }
        public bool IsOk { get; }
        public bool IsReadOnly { get; }
        public void LoadSvgFromStream(InputStream stream, int width, int height, Color color) { }
        public void LoadSvgFromString(string s, int width, int height, Color color) { }

        public void InitImage(Image image, int width, int height) { }

        /*
        Get the size of the bitmap represented by this bundle in default resolution
            or, equivalently, at 100% scaling.

        When creating the bundle from a number of bitmaps, this will be just the
        size of the smallest bitmap in it.

        Note that this function is mostly used by wxWidgets itself and not
        the application. 
         */
        public SizeI DefaultSize { get; }

        /*
        Get bitmap of the size appropriate for the DPI scaling used by the given window.

        This helper function simply combines GetPreferredBitmapSizeFor() and
        GetBitmap(), i.e. it returns a (normally unscaled) bitmap from the bundle
        of the closest size to the size that should be used at the DPI scaling of
        the provided window.                 
         */
        public void InitImageFor(Image image, IntPtr window) { }

        /*
        Get the size that would be best to use for this bundle at the given DPI scaling factor.

        For bundles containing some number of the fixed-size bitmaps, this function
        returns the size of an existing bitmap closest to the ideal size at the given
        scale, i.e.GetDefaultSize() multiplied by scale.

        Passing a size returned by this function to GetBitmap() ensures that bitmap
        doesn't need to be rescaled, which typically significantly lowers its quality.         
         */
        public SizeI GetPreferredBitmapSizeAtScale(float scale) => default;

        /*
        Get the size that would be best to use for this bundle at the DPI
        scaling factor used by the given window.

        This is just a convenient wrapper for GetPreferredBitmapSizeAtScale() calling
        that function with the result of wxWindow::GetDPIScaleFactor()         
         */
        public SizeI GetPreferredBitmapSizeFor(IntPtr window) => default;
    }
}


