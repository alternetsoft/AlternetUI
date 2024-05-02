using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Overrides <see cref="AbstractDrawing"/> methods throwing
    /// <see cref="NotImplementedException"/>.
    /// </summary>
    internal class NotImplementedDrawing : NativeDrawing
    {
        public override object CreateFont() => NotImplemented();

        public override object CreateDefaultFont() => NotImplemented();

        public override Font CreateSystemFont(SystemSettingsFont systemFont) => NotImplemented<Font>();

        public override object CreateFont(object font) => NotImplemented();

        public override object CreateDefaultMonoFont() => NotImplemented();

        public override object CreatePen() => NotImplemented();

        public override object CreateTransparentBrush() => NotImplemented();

        public override object CreateHatchBrush() => NotImplemented();

        public override object CreateLinearGradientBrush() => NotImplemented();

        public override object CreateRadialGradientBrush() => NotImplemented();

        public override object CreateSolidBrush() => NotImplemented();

        public override object CreateTextureBrush() => NotImplemented();

        public override void UpdatePen(Pen pen) => NotImplemented();

        public override void UpdateSolidBrush(SolidBrush brush) => NotImplemented();

        public override void UpdateFont(object font, FontParams prm) => NotImplemented();

        public override void UpdateHatchBrush(HatchBrush brush) => NotImplemented();

        public override void UpdateLinearGradientBrush(LinearGradientBrush brush) => NotImplemented();

        public override void UpdateRadialGradientBrush(RadialGradientBrush brush) => NotImplemented();

        public override Color GetColor(SystemSettingsColor index) => NotImplemented<Color>();

        public override string[] GetFontFamiliesNames() => NotImplemented<string[]>();

        public override bool IsFontFamilyValid(string name) => NotImplemented<bool>();

        public override string GetFontFamilyName(GenericFontFamily genericFamily)
             => NotImplemented<string>();

        public override int GetDefaultFontEncoding() => NotImplemented<int>();

        public override void SetDefaultFontEncoding(int value) => NotImplemented();

        public override string GetFontName(object font) => NotImplemented<string>();

        public override int GetFontEncoding(object font) => NotImplemented<int>();

        public override SizeI GetFontSizeInPixels(object font) => NotImplemented<SizeI>();

        public override bool GetFontIsUsingSizeInPixels(object font) => NotImplemented<bool>();

        public override int GetFontNumericWeight(object font) => NotImplemented<int>();

        public override bool GetFontIsFixedWidth(object font) => NotImplemented<bool>();

        public override FontWeight GetFontWeight(object font) => NotImplemented<FontWeight>();

        public override FontStyle GetFontStyle(object font) => NotImplemented<FontStyle>();

        public override bool GetFontStrikethrough(object font) => NotImplemented<bool>();

        public override bool GetFontUnderlined(object font) => NotImplemented<bool>();

        public override double GetFontSizeInPoints(object font) => NotImplemented<double>();

        public override string GetFontInfoDesc(object font) => NotImplemented<string>();

        public override bool FontEquals(object font1, object font2) => NotImplemented<bool>();

        public override string FontToString(object font) => NotImplemented<string>();

        public override void UpdateTransformMatrix(object matrix, double m11, double m12, double m21, double m22, double dx, double dy)
        {
            throw new NotImplementedException();
        }

        public override double GetTransformMatrixM11(object matrix)
        {
            throw new NotImplementedException();
        }

        public override double GetTransformMatrixM12(object matrix)
        {
            throw new NotImplementedException();
        }

        public override double GetTransformMatrixM21(object matrix)
        {
            throw new NotImplementedException();
        }

        public override double GetTransformMatrixM22(object matrix)
        {
            throw new NotImplementedException();
        }

        public override double GetTransformMatrixDX(object matrix)
        {
            throw new NotImplementedException();
        }

        public override double GetTransformMatrixDY(object matrix)
        {
            throw new NotImplementedException();
        }

        public override bool GetTransformMatrixIsIdentity(object matrix)
        {
            throw new NotImplementedException();
        }

        public override void ResetTransformMatrix(object matrix)
        {
            throw new NotImplementedException();
        }

        public override void MultiplyTransformMatrix(object matrix1, object matrix2)
        {
            throw new NotImplementedException();
        }

        public override void TranslateTransformMatrix(object matrix, double offsetX, double offsetY)
        {
            throw new NotImplementedException();
        }

        public override void ScaleTransformMatrix(object matrix, double scaleX, double scaleY)
        {
            throw new NotImplementedException();
        }

        public override void RotateTransformMatrix(object matrix, double angle)
        {
            throw new NotImplementedException();
        }

        public override void InvertTransformMatrix(object matrix)
        {
            throw new NotImplementedException();
        }

        public override void SetTransformMatrixM11(object matrix, double value)
        {
            throw new NotImplementedException();
        }

        public override void SetTransformMatrixM12(object matrix, double value)
        {
            throw new NotImplementedException();
        }

        public override void SetTransformMatrixM21(object matrix, double value)
        {
            throw new NotImplementedException();
        }

        public override void SetTransformMatrixM22(object matrix, double value)
        {
            throw new NotImplementedException();
        }

        public override void SetTransformMatrixDX(object matrix, double value)
        {
            throw new NotImplementedException();
        }

        public override void SetTransformMatrixDY(object matrix, double value)
        {
            throw new NotImplementedException();
        }

        public override PointD TransformMatrixOnPoint(object matrix, PointD point)
        {
            throw new NotImplementedException();
        }

        public override SizeD TransformMatrixOnSize(object matrix, SizeD size)
        {
            throw new NotImplementedException();
        }

        public override bool TransformMatrixEquals(object matrix1, object matrix2)
        {
            throw new NotImplementedException();
        }

        public override int TransformMatrixGetHashCode(object matrix)
        {
            throw new NotImplementedException();
        }

        public override object CreateTransformMatrix()
        {
            throw new NotImplementedException();
        }

        public override object CreateImage()
        {
            throw new NotImplementedException();
        }

        public override bool ImageSave(object image, string fileName)
        {
            throw new NotImplementedException();
        }

        public override bool ImageSave(object image, Stream stream, ImageFormat format)
        {
            throw new NotImplementedException();
        }

        public override SizeI GetImagePixelSize(object image)
        {
            throw new NotImplementedException();
        }

        public override bool ImageLoadFromStream(object image, Stream stream)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromImage(Image original, SizeI newSize)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromGenericImage(GenericImage genericImage, int depth = -1)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromGraphics(int width, int height, Graphics dc)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromGraphicsAndGenericImage(GenericImage genericImage, Graphics dc)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageWithSizeAndDepth(SizeI size, int depth = 32)
        {
            throw new NotImplementedException();
        }

        public override BitmapType GetDefaultBitmapType()
        {
            throw new NotImplementedException();
        }

        public override bool GetImageHasAlpha(object image)
        {
            throw new NotImplementedException();
        }

        public override void SetImageHasAlpha(object image, bool hasAlpha)
        {
            throw new NotImplementedException();
        }

        public override double GetImageScaleFactor(object image)
        {
            throw new NotImplementedException();
        }

        public override void SetImageScaleFactor(object image, double value)
        {
            throw new NotImplementedException();
        }

        public override SizeI GetImageDipSize(object image)
        {
            throw new NotImplementedException();
        }

        public override double GetImageScaledHeight(object image)
        {
            throw new NotImplementedException();
        }

        public override SizeI GetImageScaledSize(object image)
        {
            throw new NotImplementedException();
        }

        public override double GetImageScaledWidth(object image)
        {
            throw new NotImplementedException();
        }

        public override int GetImageDepth(object image)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromScreen()
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromSvgStream(Stream stream, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromSvgString(string s, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public override bool ImageLoad(object image, string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public override bool ImageSaveToFile(object image, string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public override bool ImageSaveToStream(object image, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public override bool ImageLoadFromStream(object image, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        public override object ImageGetSubBitmap(object image, RectI rect)
        {
            throw new NotImplementedException();
        }

        public override object ImageConvertToDisabled(object image, byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        public override void ImageRescale(object image, SizeI sizeNeeded)
        {
            throw new NotImplementedException();
        }

        public override void ImageResetAlpha(object image)
        {
            throw new NotImplementedException();
        }

        public override bool GetImageIsOk(object image)
        {
            throw new NotImplementedException();
        }

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

        public override object CreateGenericImage(int width, int height, IntPtr data, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override object CreateGenericImage(int width, int height, IntPtr data, IntPtr alpha, bool staticData = false)
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

        public override void DisposeGenericImage(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override BitmapType GenericImageGetImageType(object genericImage)
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

        public override void GenericImageSetNativeData(object genericImage, IntPtr data, int new_width, int new_height, bool static_data = false)
        {
            throw new NotImplementedException();
        }

        public override IntPtr GenericImageGetNativeAlphaData(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override IntPtr GenericImageGetNativeData(object genericImage)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageCreateNativeData(object genericImage, int width, int height, IntPtr data, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override bool GenericImageCreateNativeData(object genericImage, int width, int height, IntPtr data, IntPtr alpha, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetNativeAlphaData(object genericImage, IntPtr alpha = default, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override void GenericImageSetNativeData(object genericImage, IntPtr data, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public override FillMode GraphicsPathGetFillMode(object graphicsPath)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathSetFillMode(object graphicsPath, FillMode value)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddLines(object graphicsPath, PointD[] points)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddLine(object graphicsPath, PointD pt1, PointD pt2)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddLineTo(object graphicsPath, PointD pt)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddEllipse(object graphicsPath, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddBezier(object graphicsPath, PointD startPoint, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddBezierTo(object graphicsPath, PointD controlPoint1, PointD controlPoint2, PointD endPoint)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddArc(object graphicsPath, PointD center, double radius, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddRectangle(object graphicsPath, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathAddRoundedRectangle(object graphicsPath, RectD rect, double cornerRadius)
        {
            throw new NotImplementedException();
        }

        public override RectD GraphicsPathGetBounds(object graphicsPath)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathStartFigure(object graphicsPath, PointD point)
        {
            throw new NotImplementedException();
        }

        public override void GraphicsPathCloseFigure(object graphicsPath)
        {
            throw new NotImplementedException();
        }

        public override object CreateGraphicsPath()
        {
            throw new NotImplementedException();
        }

        public override object CreateRegion()
        {
            throw new NotImplementedException();
        }

        public override object CreateRegion(RectD rect)
        {
            throw new NotImplementedException();
        }

        public override object CreateRegion(object region)
        {
            throw new NotImplementedException();
        }

        public override object CreateRegion(PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            throw new NotImplementedException();
        }

        public override bool RegionIsEmpty(object nativeRegion)
        {
            throw new NotImplementedException();
        }

        public override bool RegionIsOk(object nativeRegion)
        {
            throw new NotImplementedException();
        }

        public override void RegionClear(object nativeRegion)
        {
            throw new NotImplementedException();
        }

        public override RegionContain RegionContains(object nativeRegion, PointD pt)
        {
            throw new NotImplementedException();
        }

        public override RegionContain RegionContains(object nativeRegion, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void RegionIntersect(object nativeRegion, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void RegionIntersect(object nativeRegion1, object nativeRegion2)
        {
            throw new NotImplementedException();
        }

        public override void RegionUnion(object nativeRegion, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void RegionUnion(object nativeRegion1, object nativeRegion2)
        {
            throw new NotImplementedException();
        }

        public override void RegionXor(object nativeRegion1, object nativeRegion2)
        {
            throw new NotImplementedException();
        }

        public override void RegionXor(object nativeRegion, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void RegionSubtract(object nativeRegion, RectD rect)
        {
            throw new NotImplementedException();
        }

        public override void RegionSubtract(object nativeRegion1, object nativeRegion2)
        {
            throw new NotImplementedException();
        }

        public override void RegionTranslate(object nativeRegion, double dx, double dy)
        {
            throw new NotImplementedException();
        }

        public override RectD RegionGetBounds(object nativeRegion)
        {
            throw new NotImplementedException();
        }

        public override bool RegionEquals(object nativeRegion1, object nativeRegion2)
        {
            throw new NotImplementedException();
        }

        public override int RegionGetHashCode(object nativeRegion)
        {
            throw new NotImplementedException();
        }

        public override object ImageConvertToGenericImage(Image image)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTextureBrush(TextureBrush brush)
        {
            throw new NotImplementedException();
        }

        public override object CreateGraphicsPath(object nativeGraphics)
        {
            throw new NotImplementedException();
        }

        public override SizeI ImageListGetPixelImageSize(object imageList)
        {
            throw new NotImplementedException();
        }

        public override void ImageListSetPixelImageSize(object imageList, SizeI value)
        {
            throw new NotImplementedException();
        }

        public override SizeD ImageListGetImageSize(object imageList)
        {
            throw new NotImplementedException();
        }

        public override void ImageListSetImageSize(object imageList, SizeD value)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageList()
        {
            throw new NotImplementedException();
        }

        public override void ImageListAdd(object imageList, int index, Image item)
        {
            throw new NotImplementedException();
        }

        public override void ImageListRemove(object imageList, int index, Image item)
        {
            throw new NotImplementedException();
        }

        public override void IconSetAdd(object iconSet, Image image)
        {
            throw new NotImplementedException();
        }

        public override void IconSetAdd(object iconSet, Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void IconSetClear(object iconSet)
        {
            throw new NotImplementedException();
        }

        public override object CreateIconSet()
        {
            throw new NotImplementedException();
        }

        public override bool IconSetIsOk(object iconSet)
        {
            throw new NotImplementedException();
        }

        public override SizeI ImageSetGetPreferredBitmapSizeAtScale(object imageSet, double scale)
        {
            throw new NotImplementedException();
        }

        public override void ImageSetAddImage(object imageSet, int index, Image item)
        {
            throw new NotImplementedException();
        }

        public override void ImageSetRemoveImage(object imageSet, int index, Image item)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageSet()
        {
            throw new NotImplementedException();
        }

        public override object CreateImageSetFromSvgStream(Stream stream, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageSetFromSvgString(string s, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public override SizeI ImageSetGetDefaultSize(object imageSet)
        {
            throw new NotImplementedException();
        }

        public override bool ImageSetIsOk(object imageSet)
        {
            throw new NotImplementedException();
        }

        public override bool ImageSetIsReadOnly(object imageSet)
        {
            throw new NotImplementedException();
        }

        public override void ImageSetLoadFromStream(object imageSet, Stream stream)
        {
            throw new NotImplementedException();
        }

        public override object CreateImage(ImageSet imageSet, SizeI size)
        {
            throw new NotImplementedException();
        }

        public override object CreateCaret()
        {
            throw new NotImplementedException();
        }

        public override object CreateCaret(IControl control, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override int CaretGetBlinkTime()
        {
            throw new NotImplementedException();
        }

        public override void CaretSetBlinkTime(int value)
        {
            throw new NotImplementedException();
        }

        public override SizeI CaretGetSize(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetSize(object nativeCaret, SizeI value)
        {
            throw new NotImplementedException();
        }

        public override PointI CaretGetPosition(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetPosition(object nativeCaret, PointI value)
        {
            throw new NotImplementedException();
        }

        public override bool CaretIsOk(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override bool CaretGetVisible(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override void CaretSetVisible(object nativeCaret, bool value)
        {
            throw new NotImplementedException();
        }

        public override void DisposeCaret(object nativeCaret)
        {
            throw new NotImplementedException();
        }

        public override Graphics CreateGraphicsFromScreen()
        {
            throw new NotImplementedException();
        }

        public override Graphics CreateGraphicsFromImage(Image image)
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor()
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor(CursorType cursor)
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor(string cursorName, BitmapType type, int hotSpotX = 0, int hotSpotY = 0)
        {
            throw new NotImplementedException();
        }

        public override object CreateCursor(Image image)
        {
            throw new NotImplementedException();
        }

        public override bool CursorIsOk(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override PointI CursorGetHotSpot(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override void CursorSetGlobal(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override void DisposeCursor(object nativeCursor)
        {
            throw new NotImplementedException();
        }

        public override object CreateDisplay()
        {
            throw new NotImplementedException();
        }

        public override object CreateDisplay(int index)
        {
            throw new NotImplementedException();
        }

        public override int DisplayGetCount()
        {
            throw new NotImplementedException();
        }

        public override int DisplayGetDefaultDPIValue()
        {
            throw new NotImplementedException();
        }

        public override SizeI DisplayGetDefaultDPI()
        {
            throw new NotImplementedException();
        }

        public override string DisplayGetName(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override SizeI DisplayGetDPI(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override double DisplayGetScaleFactor(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override bool DisplayGetIsPrimary(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override RectI DisplayGetClientArea(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override RectI DisplayGetGeometry(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override int DisplayGetFromPoint(PointI pt)
        {
            throw new NotImplementedException();
        }

        public override void DisposeDisplay(object nativeDisplay)
        {
            throw new NotImplementedException();
        }

        public override int DisplayGetFromControl(IControl control)
        {
            throw new NotImplementedException();
        }

        public override object CreateImage(ImageSet imageSet, IControl control)
        {
            throw new NotImplementedException();
        }

        public override object CreateImageFromImage(Image image)
        {
            throw new NotImplementedException();
        }
    }
}