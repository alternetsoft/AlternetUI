using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        /// <inheritdoc/>
        public override bool GenericImageCanRead(string filename)
        {
            return UI.Native.GenericImage.CanRead(filename);
        }

        /// <inheritdoc/>
        public override bool GenericImageCanRead(Stream stream)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.CanReadStream(inputStream);
        }

        /// <inheritdoc/>
        public override string GetGenericImageExtWildcard()
        {
            return UI.Native.GenericImage.GetImageExtWildcard();
        }

        /// <inheritdoc/>
        public override bool GenericImageRemoveHandler(string name)
        {
            return UI.Native.GenericImage.RemoveHandler(name);
        }

        /// <inheritdoc/>
        public override int GetGenericImageCount(
            string filename,
            BitmapType bitmapType = BitmapType.Any)
        {
            return UI.Native.GenericImage.GetImageCountInFile(filename, (int)bitmapType);
        }

        /// <inheritdoc/>
        public override int GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.GetImageCountInStream(inputStream, (int)bitmapType);
        }

        /// <inheritdoc/>
        public override void GenericImageCleanUpHandlers()
        {
            UI.Native.GenericImage.CleanUpHandlers();
        }

        /// <inheritdoc/>
        public override GenericImageLoadFlags GetGenericImageDefaultLoadFlags()
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetDefaultLoadFlags();
        }

        /// <inheritdoc/>
        public override void SetGenericImageDefaultLoadFlags(GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetDefaultLoadFlags((int)flags);
        }

        /// <inheritdoc/>
        public override void GenericImageSetAlpha(object genericImage, int x, int y, byte alpha)
        {
            UI.Native.GenericImage.SetAlpha((IntPtr)genericImage, x, y, alpha);
        }

        /// <inheritdoc/>
        public override void GenericImageClearAlpha(object genericImage)
        {
            UI.Native.GenericImage.ClearAlpha((IntPtr)genericImage);
        }

        /// <inheritdoc/>
        public override void GenericImageSetMask(object genericImage, bool hasMask = true)
        {
            UI.Native.GenericImage.SetMask((IntPtr)genericImage, hasMask);
        }

        /// <inheritdoc/>
        public override void GenericImageSetMaskColor(object genericImage, RGBValue rgb)
        {
            UI.Native.GenericImage.SetMaskColor((IntPtr)genericImage, rgb.R, rgb.G, rgb.B);
        }

        /// <inheritdoc/>
        public override int GetGenericImageWidth(object genericImage)
        {
            return UI.Native.GenericImage.GetWidth((IntPtr)genericImage);
        }

        /// <inheritdoc/>
        public override int GetGenericImageHeight(object genericImage)
        {
            return UI.Native.GenericImage.GetHeight((IntPtr)genericImage);
        }

        /// <inheritdoc/>
        public override bool GetGenericImageIsOk(object genericImage)
        {
            return UI.Native.GenericImage.IsOk((IntPtr)genericImage);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage()
        {
            return UI.Native.GenericImage.CreateImage();
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(int width, int height, bool clear = false)
        {
            return UI.Native.GenericImage.CreateImageWithSize(width, height, clear);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(SizeI size, bool clear = false)
        {
            return UI.Native.GenericImage.CreateImageWithSize(size.Width, size.Height, clear);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(
            string fileName,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return UI.Native.GenericImage.CreateImageFromFileWithBitmapType(
                    fileName,
                    (int)bitmapType,
                    index);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(string name, string mimetype, int index = -1)
        {
            return UI.Native.GenericImage.CreateImageFromFileWithMimeType(name, mimetype, index);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return UI.Native.GenericImage.CreateImageFromStreamWithBitmapData(
                      new UI.Native.InputStream(stream),
                      (int)bitmapType,
                      index);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(
            Stream stream,
            string mimeType,
            int index = -1)
        {
            return UI.Native.GenericImage.CreateImageFromStreamWithMimeType(
                      new UI.Native.InputStream(stream),
                      mimeType,
                      index);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(
            int width,
            int height,
            IntPtr data,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateImageWithSizeAndData(width, height, data, staticData);
        }

        /// <inheritdoc/>
        public override object CreateGenericImage(
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateImageWithAlpha(width, height, data, alpha, staticData);
        }

        /// <inheritdoc/>
        public override bool GenericImageSetMaskFromImage(object image1, object image2, RGBValue mask)
        {
            return UI.Native.GenericImage.SetMaskFromImage(
                (IntPtr)image1,
                (IntPtr)image2,
                mask.R,
                mask.G,
                mask.B);
        }

        /// <inheritdoc/>
        public override void GenericImageSetOptionAsString(object genericImage, string name, string value)
        {
            UI.Native.GenericImage.SetOptionString((IntPtr)genericImage, name, value);
        }

        /// <inheritdoc/>
        public override void GenericImageSetOptionAsInt(object genericImage, string name, int value)
        {
            UI.Native.GenericImage.SetOptionInt((IntPtr)genericImage, name, value);
        }

        public override void GenericImageSetRGB(object genericImage, int x, int y, RGBValue rgb)
        {
            UI.Native.GenericImage.SetRGB((IntPtr)genericImage, x, y, rgb.R, rgb.G, rgb.B);
        }

        public override void GenericImageSetRGBRect(object genericImage, RGBValue rgb, RectI? rect = null)
        {
            if(rect is null)
            {
                var height = GetGenericImageHeight((IntPtr)genericImage);
                var width = GetGenericImageWidth((IntPtr)genericImage);
                rect = (0, 0, width, height);
            }

            UI.Native.GenericImage.SetRGBRect((IntPtr)genericImage, rect.Value, rgb.R, rgb.G, rgb.B);
        }

        public override void GenericImageSetImageType(object genericImage, BitmapType type)
        {
            UI.Native.GenericImage.SetImageType((IntPtr)genericImage, (int)type);
        }

        public override object GenericImageCopy(object genericImage)
        {
            return UI.Native.GenericImage.Copy((IntPtr)genericImage);
        }

        public override bool GenericImageReset(object genericImage, int width, int height, bool clear = false)
        {
            return UI.Native.GenericImage.CreateFreshImage((IntPtr)genericImage, width, height, clear);
        }

        public override void GenericImageClear(object genericImage, byte value = 0)
        {
            UI.Native.GenericImage.Clear((IntPtr)genericImage, value);
        }

        public override void GenericImageReset(object genericImage)
        {
            UI.Native.GenericImage.DestroyImageData((IntPtr)genericImage);
        }

        public override Color GenericImageFindFirstUnusedColor(object genericImage, RGBValue? startRGB = null)
        {
            var value = startRGB ?? new(1, 0, 0);

            return UI.Native.GenericImage.FindFirstUnusedColor(
                (IntPtr)genericImage,
                value.R,
                value.G,
                value.B);
        }

        public override void GenericImageInitAlpha(object genericImage)
        {
            if (GenericImageHasAlpha((IntPtr)genericImage))
                return;
            UI.Native.GenericImage.InitAlpha((IntPtr)genericImage);
        }

        public override object GenericImageBlur(object genericImage, int blurRadius)
        {
            return UI.Native.GenericImage.Blur((IntPtr)genericImage, blurRadius);
        }

        public override object GenericImageBlurHorizontal(object genericImage, int blurRadius)
        {
            return UI.Native.GenericImage.BlurHorizontal((IntPtr)genericImage, blurRadius);
        }

        public override object GenericImageBlurVertical(object genericImage, int blurRadius)
        {
            return UI.Native.GenericImage.BlurVertical((IntPtr)genericImage, blurRadius);
        }

        public override object GenericImageMirror(object genericImage, bool horizontally = true)
        {
            return UI.Native.GenericImage.Mirror((IntPtr)genericImage, horizontally);
        }

        public override void GenericImagePaste(
            object genericImage1,
            object genericImage2,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite)
        {
            UI.Native.GenericImage.Paste(
                (IntPtr)genericImage1,
                (IntPtr)genericImage2,
                x,
                y,
                (int)alphaBlend);
        }

        public override void GenericImageReplace(object genericImage, RGBValue r1, RGBValue r2)
        {
            UI.Native.GenericImage.Replace((IntPtr)genericImage, r1.R, r1.G, r1.B, r2.R, r2.G, r2.B);
        }

        public override void GenericImageRescale(
            object genericImage,
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            UI.Native.GenericImage.Rescale((IntPtr)genericImage, width, height, (int)quality);
        }

        public override void GenericImageResizeNoScale(
            object genericImage,
            SizeI size,
            PointI pos,
            RGBValue? color = null)
        {
            if (color is null)
            {
                UI.Native.GenericImage.Resize((IntPtr)genericImage, size, pos, -1, -1, -1);
                return;
            }

            var red = color.Value.R;
            var green = color.Value.G;
            var blue = color.Value.G;

            UI.Native.GenericImage.Resize((IntPtr)genericImage, size, pos, red, green, blue);
        }

        public override object GenericImageSizeNoScale(
            object genericImage,
            SizeI size,
            PointI pos = default,
            RGBValue? color = null)
        {
            var red = color?.R ?? -1;
            var green = color?.G ?? -1;
            var blue = color?.G ?? -1;

            var image = UI.Native.GenericImage.Size((IntPtr)genericImage, size, pos, red, green, blue);
            return image;
        }

        public override object GenericImageRotate90(object genericImage, bool clockwise = true)
        {
            return UI.Native.GenericImage.Rotate90((IntPtr)genericImage, clockwise);
        }

        public override object GenericImageRotate180(object genericImage)
        {
            return UI.Native.GenericImage.Rotate180((IntPtr)genericImage);
        }

        public override void GenericImageRotateHue(object genericImage, double angle)
        {
            UI.Native.GenericImage.RotateHue((IntPtr)genericImage, angle);
        }

        public override void GenericImageChangeSaturation(object genericImage, double factor)
        {
            UI.Native.GenericImage.ChangeSaturation((IntPtr)genericImage, factor);
        }

        public override void GenericImageChangeBrightness(object genericImage, double factor)
        {
            UI.Native.GenericImage.ChangeBrightness((IntPtr)genericImage, factor);
        }

        public override GenericImageLoadFlags GenericImageGetLoadFlags(object genericImage)
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetLoadFlags((IntPtr)genericImage);
        }

        public override void GenericImageSetLoadFlags(object genericImage, GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetLoadFlags((IntPtr)genericImage, (int)flags);
        }

        public override void GenericImageChangeHSV(
            object genericImage,
            double angleH,
            double factorS,
            double factorV)
        {
            UI.Native.GenericImage.ChangeHSV((IntPtr)genericImage, angleH, factorS, factorV);
        }

        public override object GenericImageScale(
            object genericImage,
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            return UI.Native.GenericImage.Scale((IntPtr)genericImage, width, height, (int)quality);
        }

        public override bool GenericImageConvertAlphaToMask(object genericImage, byte threshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMask((IntPtr)genericImage, threshold);
        }

        public override bool GenericImageConvertAlphaToMask(
            object genericImage,
            RGBValue rgb,
            byte threshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMaskUseColor(
                (IntPtr)genericImage,
                rgb.R,
                rgb.G,
                rgb.B,
                threshold);
        }

        public override object GenericImageConvertToGreyscale(
            object genericImage,
            double weightR,
            double weightG,
            double weightB)
        {
            return UI.Native.GenericImage.ConvertToGreyscaleEx((IntPtr)genericImage, weightR, weightG, weightB);
        }

        public override object GenericImageConvertToGreyscale(object genericImage)
        {
            return UI.Native.GenericImage.ConvertToGreyscale((IntPtr)genericImage);
        }

        public override object GenericImageConvertToMono(object genericImage, RGBValue rgb)
        {
            return UI.Native.GenericImage.ConvertToMono((IntPtr)genericImage, rgb.R, rgb.G, rgb.B);
        }

        public override object GenericImageConvertToDisabled(object genericImage, byte brightness = 255)
        {
            return UI.Native.GenericImage.ConvertToDisabled((IntPtr)genericImage, brightness);
        }

        public override object GenericImageChangeLightness(object genericImage, int ialpha)
        {
            return UI.Native.GenericImage.ChangeLightness((IntPtr)genericImage, ialpha);
        }

        public override byte GenericImageGetAlpha(object genericImage, int x, int y)
        {
            return UI.Native.GenericImage.GetAlpha((IntPtr)genericImage, x, y);
        }

        public override RGBValue GenericImageGetRGB(object genericImage, int x, int y)
        {
            var r = GenericImageGetRed((IntPtr)genericImage, x, y);
            var g = GenericImageGetGreen((IntPtr)genericImage, x, y);
            var b = GenericImageGetBlue((IntPtr)genericImage, x, y);
            return new(r, g, b);
        }

        public override Color GenericImageGetPixel(object genericImage, int x, int y, bool withAlpha = false)
        {
            var r = GenericImageGetRed((IntPtr)genericImage, x, y);
            var g = GenericImageGetGreen((IntPtr)genericImage, x, y);
            var b = GenericImageGetBlue((IntPtr)genericImage, x, y);
            var a = (withAlpha
                && GenericImageHasAlpha((IntPtr)genericImage))
                ? GenericImageGetAlpha((IntPtr)genericImage, x, y) : 255;
            return Color.FromArgb(a, r, g, b);
        }

        public override void GenericImageSetPixel(
            object genericImage,
            int x,
            int y,
            Color color,
            bool withAlpha = false)
        {
            color.GetArgbValues(out var a, out var r, out var g, out var b);
            GenericImageSetRGB((IntPtr)genericImage, x, y, new RGBValue(r, g, b));
            if (withAlpha && GenericImageHasAlpha((IntPtr)genericImage))
                GenericImageSetAlpha((IntPtr)genericImage, x, y, a);
        }

        public override byte GenericImageGetRed(object genericImage, int x, int y)
        {
            return UI.Native.GenericImage.GetRed((IntPtr)genericImage, x, y);
        }

        public override byte GenericImageGetGreen(object genericImage, int x, int y)
        {
            return UI.Native.GenericImage.GetGreen((IntPtr)genericImage, x, y);
        }

        public override byte GenericImageGetBlue(object genericImage, int x, int y)
        {
            return UI.Native.GenericImage.GetBlue((IntPtr)genericImage, x, y);
        }

        public override RGBValue GenericImageGetMaskRGB(object genericImage)
        {
            var r = GenericImageGetMaskRed((IntPtr)genericImage);
            var g = GenericImageGetMaskGreen((IntPtr)genericImage);
            var b = GenericImageGetMaskBlue((IntPtr)genericImage);
            return new(r, g, b);
        }

        public override byte GenericImageGetMaskRed(object genericImage)
        {
            return UI.Native.GenericImage.GetMaskRed((IntPtr)genericImage);
        }

        public override byte GenericImageGetMaskGreen(object genericImage)
        {
            return UI.Native.GenericImage.GetMaskGreen((IntPtr)genericImage);
        }

        public override byte GenericImageGetMaskBlue(object genericImage)
        {
            return UI.Native.GenericImage.GetMaskBlue((IntPtr)genericImage);
        }

        public override string GenericImageGetOptionAsString(object genericImage, string name)
        {
            return UI.Native.GenericImage.GetOptionString((IntPtr)genericImage, name);
        }

        public override int GenericImageGetOptionAsInt(object genericImage, string name)
        {
            return UI.Native.GenericImage.GetOptionInt((IntPtr)genericImage, name);
        }

        public override object GenericImageGetSubImage(object genericImage, RectI rect)
        {
            return UI.Native.GenericImage.GetSubImage((IntPtr)genericImage, rect);
        }

        public override int GenericImageGetImageType(object genericImage)
        {
            return UI.Native.GenericImage.GetImageType((IntPtr)genericImage);
        }

        public override bool GenericImageHasAlpha(object genericImage)
        {
            return UI.Native.GenericImage.HasAlpha((IntPtr)genericImage);
        }

        public override bool GenericImageHasMask(object genericImage)
        {
            return UI.Native.GenericImage.HasMask((IntPtr)genericImage);
        }

        public override bool GenericImageHasOption(object genericImage, string name)
        {
            return UI.Native.GenericImage.HasOption((IntPtr)genericImage, name);
        }

        public override bool GenericImageIsTransparent(object genericImage, int x, int y, byte threshold)
        {
            return UI.Native.GenericImage.IsTransparent((IntPtr)genericImage, x, y, threshold);
        }

        public override bool GenericImageLoadFromStream(
            object genericImage,
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithBitmapType(
                (IntPtr)genericImage,
                inputStream,
                (int)bitmapType,
                index);
        }

        public override bool GenericImageLoadFromFile(
            object genericImage,
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithBitmapType(
                (IntPtr)genericImage,
                filename,
                (int)bitmapType,
                index);
        }

        public override bool GenericImageLoadFromFile(
            object genericImage,
            string name,
            string mimetype,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithMimeType((IntPtr)genericImage, name, mimetype, index);
        }

        public override bool GenericImageLoadFromStream(
            object genericImage,
            Stream stream,
            string mimetype,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithMimeType((IntPtr)genericImage, inputStream, mimetype, index);
        }

        public override bool GenericImageSaveToStream(object genericImage, Stream stream, string mimetype)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithMimeType((IntPtr)genericImage, outputStream, mimetype);
        }

        public override bool GenericImageSaveToFile(object genericImage, string filename, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.SaveFileWithBitmapType((IntPtr)genericImage, filename, (int)bitmapType);
        }

        public override bool GenericImageSaveToFile(object genericImage, string filename, string mimetype)
        {
            return UI.Native.GenericImage.SaveFileWithMimeType((IntPtr)genericImage, filename, mimetype);
        }

        public override bool GenericImageSaveToFile(object genericImage, string filename)
        {
            return UI.Native.GenericImage.SaveFile((IntPtr)genericImage, filename);
        }

        public override bool GenericImageSaveToStream(object genericImage, Stream stream, BitmapType type)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithBitmapType((IntPtr)genericImage, outputStream, (int)type);
        }

        public override void GenericImageSetNativeData(
            object genericImage,
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false)
        {
            UI.Native.GenericImage.SetDataWithSize((IntPtr)genericImage, data, new_width, new_height, static_data);
        }

        public override IntPtr GenericImageGetNativeAlphaData(object genericImage)
        {
            return UI.Native.GenericImage.GetAlphaData((IntPtr)genericImage);
        }

        public override IntPtr GenericImageGetNativeData(object genericImage)
        {
            return UI.Native.GenericImage.GetData((IntPtr)genericImage);
        }

        public override bool GenericImageCreateNativeData(
            object genericImage,
            int width,
            int height,
            IntPtr data,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateData((IntPtr)genericImage, width, height, data, staticData);
        }

        public override bool GenericImageCreateNativeData(
            object genericImage,
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateAlphaData((IntPtr)genericImage, width, height, data, alpha, staticData);
        }

        public override void GenericImageSetNativeAlphaData(
            object genericImage,
            IntPtr alpha = default,
            bool staticData = false)
        {
            UI.Native.GenericImage.SetAlphaData((IntPtr)genericImage, alpha, staticData);
        }

        public override void GenericImageSetNativeData(object genericImage, IntPtr data, bool staticData = false)
        {
            UI.Native.GenericImage.SetData((IntPtr)genericImage, data, staticData);
        }
    }
}
