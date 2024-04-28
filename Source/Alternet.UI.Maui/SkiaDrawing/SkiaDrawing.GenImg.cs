using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing : NativeDrawing
    {
        public override object CreateGenericImage()
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(int width, int height, bool clear = false)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(SizeI size, bool clear = false)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(string fileName, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(string name, string mimetype, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(Stream stream, string mimeType, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(int width, int height, nint data, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(int width, int height, nint data, nint alpha, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override int GetGenericImageWidth(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override int GetGenericImageHeight(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GetGenericImageIsOk(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageCanRead(string filename)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageCanRead(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override string GetGenericImageExtWildcard()
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageRemoveHandler(string name)
        {
            throw new NotImplementedException();
        }

        public override int GetGenericImageCount(string filename, BitmapType bitmapType = BitmapType.Any)
        {
            throw new NotImplementedException();
        }

        public override int GetGenericImageCount(Stream stream, BitmapType bitmapType = BitmapType.Any)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageCleanUpHandlers()
        {
            throw new NotImplementedException();
        }

        public override GenericImageLoadFlags GetGenericImageDefaultLoadFlags()
        {
            throw new NotImplementedException();
        }

        public override void SetGenericImageDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetAlpha(object genericImage, int x, int y, byte alpha)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageClearAlpha(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetMask(object genericImage, bool hasMask = true)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetMaskColor(object genericImage, RGBValue rgb)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageSetMaskFromImage(object image1, object image2, RGBValue mask)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetOptionAsString(object genericImage, string name, string value)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetOptionAsInt(object genericImage, string name, int value)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetRGB(object genericImage, int x, int y, RGBValue rgb)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetRGBRect(object genericImage, RGBValue rgb, RectI? rect = null)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetImageType(object genericImage, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageCopy(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageReset(object genericImage, int width, int height, bool clear = false)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageClear(object genericImage, byte value = 0)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageReset(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override Color GenericImageFindFirstUnusedColor(object genericImage, RGBValue? startRGB = null)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageInitAlpha(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageBlur(object genericImage, int blurRadius)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageBlurHorizontal(object genericImage, int blurRadius)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageBlurVertical(object genericImage, int blurRadius)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageMirror(object genericImage, bool horizontally = true)
        {
            throw new NotImplementedException();
        }

        public override void GenericImagePaste(object genericImage1, object genericImage2, int x, int y, GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageReplace(object genericImage, RGBValue r1, RGBValue r2)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageRescale(object genericImage, int width, int height, GenericImageResizeQuality quality = GenericImageResizeQuality.Nearest)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageResizeNoScale(object genericImage, SizeI size, PointI pos, RGBValue? color = null)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageSizeNoScale(object genericImage, SizeI size, PointI pos = default, RGBValue? color = null)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageRotate90(object genericImage, bool clockwise = true)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageRotate180(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageRotateHue(object genericImage, double angle)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageChangeSaturation(object genericImage, double factor)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageChangeBrightness(object genericImage, double factor)
        {
            throw new NotImplementedException();
        }

        public override GenericImageLoadFlags GenericImageGetLoadFlags(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetLoadFlags(object genericImage, GenericImageLoadFlags flags)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageChangeHSV(object genericImage, double angleH, double factorS, double factorV)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageScale(object genericImage, int width, int height, GenericImageResizeQuality quality = GenericImageResizeQuality.Nearest)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageConvertAlphaToMask(object genericImage, byte threshold)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageConvertAlphaToMask(object genericImage, RGBValue rgb, byte threshold)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageConvertToGreyscale(object genericImage, double weightR, double weightG, double weightB)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageConvertToGreyscale(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageConvertToMono(object genericImage, RGBValue rgb)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageConvertToDisabled(object genericImage, byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageChangeLightness(object genericImage, int ialpha)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetAlpha(object genericImage, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override RGBValue GenericImageGetRGB(object genericImage, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override Color GenericImageGetPixel(object genericImage, int x, int y, bool withAlpha = false)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetPixel(object genericImage, int x, int y, Color color, bool withAlpha = false)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetRed(object genericImage, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetGreen(object genericImage, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetBlue(object genericImage, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override RGBValue GenericImageGetMaskRGB(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetMaskRed(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetMaskGreen(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override byte GenericImageGetMaskBlue(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override string GenericImageGetOptionAsString(object genericImage, string name)
        {
            throw new NotImplementedException();
        }

        public override int GenericImageGetOptionAsInt(object genericImage, string name)
        {
            throw new NotImplementedException();
        }

        public override object GenericImageGetSubImage(object genericImage, RectI rect)
        {
            throw new NotImplementedException();
        }

        public override int GenericImageGetImageType(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageHasAlpha(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageHasMask(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageHasOption(object genericImage, string name)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageIsTransparent(object genericImage, int x, int y, byte threshold)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageLoadFromStream(object genericImage, Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageLoadFromFile(object genericImage, string filename, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageLoadFromFile(object genericImage, string name, string mimetype, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageLoadFromStream(object genericImage, Stream stream, string mimetype, int index = -1)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageSaveToStream(object genericImage, Stream stream, string mimetype)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageSaveToFile(object genericImage, string filename, BitmapType bitmapType)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageSaveToFile(object genericImage, string filename, string mimetype)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageSaveToFile(object genericImage, string filename)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageSaveToStream(object genericImage, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetNativeData(object genericImage, nint data, int new_width, int new_height, bool static_data = false)
        {
            throw new NotImplementedException();
        }

        public override nint GenericImageGetNativeAlphaData(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override nint GenericImageGetNativeData(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageCreateNativeData(object genericImage, int width, int height, nint data, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageCreateNativeData(object genericImage, int width, int height, nint data, nint alpha, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetNativeAlphaData(object genericImage, nint alpha = 0, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetNativeData(object genericImage, nint data, bool staticData = false)
        {
            throw new NotImplementedException();
        }
    }
}
