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
        public override void GenericImageSetAlpha(GenericImage img, int x, int y, byte alpha)
        {
            UI.Native.GenericImage.SetAlpha((IntPtr)img.Handler, x, y, alpha);
        }

        /// <inheritdoc/>
        public override void GenericImageClearAlpha(GenericImage img)
        {
            UI.Native.GenericImage.ClearAlpha((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override void GenericImageSetMask(GenericImage img, bool hasMask = true)
        {
            UI.Native.GenericImage.SetMask((IntPtr)img.Handler, hasMask);
        }

        /// <inheritdoc/>
        public override void GenericImageSetMaskColor(GenericImage img, RGBValue rgb)
        {
            UI.Native.GenericImage.SetMaskColor((IntPtr)img.Handler, rgb.R, rgb.G, rgb.B);
        }

        /// <inheritdoc/>
        public override int GetGenericImageWidth(GenericImage img)
        {
            return UI.Native.GenericImage.GetWidth((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override int GetGenericImageHeight(GenericImage img)
        {
            return UI.Native.GenericImage.GetHeight((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override bool GetGenericImageIsOk(GenericImage img)
        {
            return UI.Native.GenericImage.IsOk((IntPtr)img.Handler);
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
        public override bool GenericImageSetMaskFromImage(
            GenericImage image1,
            GenericImage image2,
            RGBValue mask)
        {
            return UI.Native.GenericImage.SetMaskFromImage(
                (IntPtr)image1.Handler,
                (IntPtr)image2.Handler,
                mask.R,
                mask.G,
                mask.B);
        }

        /// <inheritdoc/>
        public override void GenericImageSetOptionAsString(GenericImage img, string name, string value)
        {
            UI.Native.GenericImage.SetOptionString((IntPtr)img.Handler, name, value);
        }

        /// <inheritdoc/>
        public override void GenericImageSetOptionAsInt(GenericImage img, string name, int value)
        {
            UI.Native.GenericImage.SetOptionInt((IntPtr)img.Handler, name, value);
        }

        /// <inheritdoc/>
        public override void GenericImageSetRGB(GenericImage img, int x, int y, RGBValue rgb)
        {
            UI.Native.GenericImage.SetRGB((IntPtr)img.Handler, x, y, rgb.R, rgb.G, rgb.B);
        }

        /// <inheritdoc/>
        public override void GenericImageSetRGBRect(GenericImage img, RGBValue rgb, RectI? rect = null)
        {
            if(rect is null)
            {
                var height = GetGenericImageHeight(img);
                var width = GetGenericImageWidth(img);
                rect = (0, 0, width, height);
            }

            UI.Native.GenericImage.SetRGBRect((IntPtr)img.Handler, rect.Value, rgb.R, rgb.G, rgb.B);
        }

        /// <inheritdoc/>
        public override void GenericImageSetImageType(GenericImage img, BitmapType type)
        {
            UI.Native.GenericImage.SetImageType((IntPtr)img.Handler, (int)type);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageCopy(GenericImage img)
        {
            return new(UI.Native.GenericImage.Copy((IntPtr)img.Handler));
        }

        /// <inheritdoc/>
        public override bool GenericImageReset(GenericImage img, int width, int height, bool clear = false)
        {
            return UI.Native.GenericImage.CreateFreshImage((IntPtr)img.Handler, width, height, clear);
        }

        /// <inheritdoc/>
        public override void GenericImageClear(GenericImage img, byte value = 0)
        {
            UI.Native.GenericImage.Clear((IntPtr)img.Handler, value);
        }

        /// <inheritdoc/>
        public override void GenericImageReset(GenericImage img)
        {
            UI.Native.GenericImage.DestroyImageData((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override Color GenericImageFindFirstUnusedColor(
            GenericImage img,
            RGBValue? startRGB = null)
        {
            var value = startRGB ?? new(1, 0, 0);

            return UI.Native.GenericImage.FindFirstUnusedColor(
                (IntPtr)img.Handler,
                value.R,
                value.G,
                value.B);
        }

        /// <inheritdoc/>
        public override void GenericImageInitAlpha(GenericImage img)
        {
            if (GenericImageHasAlpha(img))
                return;
            UI.Native.GenericImage.InitAlpha((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageBlur(GenericImage img, int blurRadius)
        {
            return new(UI.Native.GenericImage.Blur((IntPtr)img.Handler, blurRadius));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageBlurHorizontal(GenericImage img, int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurHorizontal((IntPtr)img.Handler, blurRadius));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageBlurVertical(GenericImage img, int blurRadius)
        {
            return new(UI.Native.GenericImage.BlurVertical((IntPtr)img.Handler, blurRadius));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageMirror(GenericImage img, bool horizontally = true)
        {
            return new(UI.Native.GenericImage.Mirror((IntPtr)img.Handler, horizontally));
        }

        /// <inheritdoc/>
        public override void GenericImagePaste(
            GenericImage img1,
            GenericImage img2,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite)
        {
            UI.Native.GenericImage.Paste(
                (IntPtr)img1.Handler,
                (IntPtr)img2.Handler,
                x,
                y,
                (int)alphaBlend);
        }

        /// <inheritdoc/>
        public override void GenericImageReplace(GenericImage img, RGBValue r1, RGBValue r2)
        {
            UI.Native.GenericImage.Replace((IntPtr)img.Handler, r1.R, r1.G, r1.B, r2.R, r2.G, r2.B);
        }

        /// <inheritdoc/>
        public override void GenericImageRescale(
            GenericImage img,
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            UI.Native.GenericImage.Rescale((IntPtr)img.Handler, width, height, (int)quality);
        }

        /// <inheritdoc/>
        public override void GenericImageResizeNoScale(
            GenericImage img,
            SizeI size,
            PointI pos,
            RGBValue? color = null)
        {
            if (color is null)
            {
                UI.Native.GenericImage.Resize((IntPtr)img.Handler, size, pos, -1, -1, -1);
                return;
            }

            var red = color.Value.R;
            var green = color.Value.G;
            var blue = color.Value.G;

            UI.Native.GenericImage.Resize((IntPtr)img.Handler, size, pos, red, green, blue);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageSizeNoScale(
            GenericImage img,
            SizeI size,
            PointI pos = default,
            RGBValue? color = null)
        {
            var red = color?.R ?? -1;
            var green = color?.G ?? -1;
            var blue = color?.G ?? -1;

            var image = UI.Native.GenericImage.Size((IntPtr)img.Handler, size, pos, red, green, blue);
            return new(image);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageRotate90(GenericImage img, bool clockwise = true)
        {
            return new(UI.Native.GenericImage.Rotate90((IntPtr)img.Handler, clockwise));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageRotate180(GenericImage img)
        {
            return new(UI.Native.GenericImage.Rotate180((IntPtr)img.Handler));
        }

        /// <inheritdoc/>
        public override void GenericImageRotateHue(GenericImage img, double angle)
        {
            UI.Native.GenericImage.RotateHue((IntPtr)img.Handler, angle);
        }

        /// <inheritdoc/>
        public override void GenericImageChangeSaturation(GenericImage img, double factor)
        {
            UI.Native.GenericImage.ChangeSaturation((IntPtr)img.Handler, factor);
        }

        /// <inheritdoc/>
        public override void GenericImageChangeBrightness(GenericImage img, double factor)
        {
            UI.Native.GenericImage.ChangeBrightness((IntPtr)img.Handler, factor);
        }

        /// <inheritdoc/>
        public override GenericImageLoadFlags GenericImageGetLoadFlags(GenericImage img)
        {
            return (GenericImageLoadFlags)UI.Native.GenericImage.GetLoadFlags((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override void GenericImageSetLoadFlags(GenericImage img, GenericImageLoadFlags flags)
        {
            UI.Native.GenericImage.SetLoadFlags((IntPtr)img.Handler, (int)flags);
        }

        /// <inheritdoc/>
        public override void GenericImageChangeHSV(
            GenericImage img,
            double angleH,
            double factorS,
            double factorV)
        {
            UI.Native.GenericImage.ChangeHSV((IntPtr)img.Handler, angleH, factorS, factorV);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageScale(
            GenericImage img,
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal)
        {
            return new(UI.Native.GenericImage.Scale((IntPtr)img.Handler, width, height, (int)quality));
        }

        /// <inheritdoc/>
        public override bool GenericImageConvertAlphaToMask(GenericImage img, byte threshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMask((IntPtr)img.Handler, threshold);
        }

        /// <inheritdoc/>
        public override bool GenericImageConvertAlphaToMask(
            GenericImage img,
            RGBValue rgb,
            byte threshold)
        {
            return UI.Native.GenericImage.ConvertAlphaToMaskUseColor(
                (IntPtr)img.Handler,
                rgb.R,
                rgb.G,
                rgb.B,
                threshold);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageConvertToGreyscale(
            GenericImage img,
            double weightR,
            double weightG,
            double weightB)
        {
            return new(UI.Native.GenericImage.ConvertToGreyscaleEx(
                (IntPtr)img.Handler,
                weightR,
                weightG,
                weightB));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageConvertToGreyscale(GenericImage img)
        {
            return new(UI.Native.GenericImage.ConvertToGreyscale((IntPtr)img.Handler));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageConvertToMono(GenericImage img, RGBValue rgb)
        {
            return new(UI.Native.GenericImage.ConvertToMono((IntPtr)img.Handler, rgb.R, rgb.G, rgb.B));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageConvertToDisabled(GenericImage img, byte brightness = 255)
        {
            return new(UI.Native.GenericImage.ConvertToDisabled((IntPtr)img.Handler, brightness));
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageChangeLightness(GenericImage img, int ialpha)
        {
            return new(UI.Native.GenericImage.ChangeLightness((IntPtr)img.Handler, ialpha));
        }

        /// <inheritdoc/>
        public override byte GenericImageGetAlpha(GenericImage img, int x, int y)
        {
            return UI.Native.GenericImage.GetAlpha((IntPtr)img.Handler, x, y);
        }

        /// <inheritdoc/>
        public override RGBValue GenericImageGetRGB(GenericImage img, int x, int y)
        {
            var r = GenericImageGetRed(img, x, y);
            var g = GenericImageGetGreen(img, x, y);
            var b = GenericImageGetBlue(img, x, y);
            return new(r, g, b);
        }

        /// <inheritdoc/>
        public override Color GenericImageGetPixel(
            GenericImage img,
            int x,
            int y,
            bool withAlpha = false)
        {
            var r = GenericImageGetRed(img, x, y);
            var g = GenericImageGetGreen(img, x, y);
            var b = GenericImageGetBlue(img, x, y);
            var a = (withAlpha
                && GenericImageHasAlpha(img))
                ? GenericImageGetAlpha(img, x, y) : 255;
            return Color.FromArgb(a, r, g, b);
        }

        /// <inheritdoc/>
        public override void GenericImageSetPixel(
            GenericImage img,
            int x,
            int y,
            Color color,
            bool withAlpha = false)
        {
            color.GetArgbValues(out var a, out var r, out var g, out var b);
            GenericImageSetRGB(img, x, y, new RGBValue(r, g, b));
            if (withAlpha && GenericImageHasAlpha(img))
                GenericImageSetAlpha(img, x, y, a);
        }

        /// <inheritdoc/>
        public override byte GenericImageGetRed(GenericImage img, int x, int y)
        {
            return UI.Native.GenericImage.GetRed((IntPtr)img.Handler, x, y);
        }

        /// <inheritdoc/>
        public override byte GenericImageGetGreen(GenericImage img, int x, int y)
        {
            return UI.Native.GenericImage.GetGreen((IntPtr)img.Handler, x, y);
        }

        /// <inheritdoc/>
        public override byte GenericImageGetBlue(GenericImage img, int x, int y)
        {
            return UI.Native.GenericImage.GetBlue((IntPtr)img.Handler, x, y);
        }

        /// <inheritdoc/>
        public override RGBValue GenericImageGetMaskRGB(GenericImage img)
        {
            var r = GenericImageGetMaskRed(img);
            var g = GenericImageGetMaskGreen(img);
            var b = GenericImageGetMaskBlue(img);
            return new(r, g, b);
        }

        /// <inheritdoc/>
        public override byte GenericImageGetMaskRed(GenericImage img)
        {
            return UI.Native.GenericImage.GetMaskRed((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override byte GenericImageGetMaskGreen(GenericImage img)
        {
            return UI.Native.GenericImage.GetMaskGreen((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override byte GenericImageGetMaskBlue(GenericImage img)
        {
            return UI.Native.GenericImage.GetMaskBlue((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override string GenericImageGetOptionAsString(GenericImage img, string name)
        {
            return UI.Native.GenericImage.GetOptionString((IntPtr)img.Handler, name);
        }

        /// <inheritdoc/>
        public override int GenericImageGetOptionAsInt(GenericImage img, string name)
        {
            return UI.Native.GenericImage.GetOptionInt((IntPtr)img.Handler, name);
        }

        /// <inheritdoc/>
        public override GenericImage GenericImageGetSubImage(GenericImage img, RectI rect)
        {
            return new(UI.Native.GenericImage.GetSubImage((IntPtr)img.Handler, rect));
        }

        /// <inheritdoc/>
        public override BitmapType GenericImageGetImageType(GenericImage img)
        {
            return (BitmapType)UI.Native.GenericImage.GetImageType((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override void DisposeGenericImage(GenericImage img)
        {
            UI.Native.GenericImage.DeleteImage((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override bool GenericImageHasAlpha(GenericImage img)
        {
            return UI.Native.GenericImage.HasAlpha((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override bool GenericImageHasMask(GenericImage img)
        {
            return UI.Native.GenericImage.HasMask((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override bool GenericImageHasOption(GenericImage img, string name)
        {
            return UI.Native.GenericImage.HasOption((IntPtr)img.Handler, name);
        }

        /// <inheritdoc/>
        public override bool GenericImageIsTransparent(GenericImage img, int x, int y, byte threshold)
        {
            return UI.Native.GenericImage.IsTransparent((IntPtr)img.Handler, x, y, threshold);
        }

        /// <inheritdoc/>
        public override bool GenericImageLoadFromStream(
            GenericImage img,
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithBitmapType(
                (IntPtr)img.Handler,
                inputStream,
                (int)bitmapType,
                index);
        }

        /// <inheritdoc/>
        public override bool GenericImageLoadFromFile(
            GenericImage img,
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithBitmapType(
                (IntPtr)img.Handler,
                filename,
                (int)bitmapType,
                index);
        }

        /// <inheritdoc/>
        public override bool GenericImageLoadFromFile(
            GenericImage img,
            string name,
            string mimetype,
            int index = -1)
        {
            return UI.Native.GenericImage.LoadFileWithMimeType((IntPtr)img.Handler, name, mimetype, index);
        }

        /// <inheritdoc/>
        public override bool GenericImageLoadFromStream(
            GenericImage img,
            Stream stream,
            string mimetype,
            int index = -1)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.LoadStreamWithMimeType((IntPtr)img.Handler, inputStream, mimetype, index);
        }

        /// <inheritdoc/>
        public override bool GenericImageSaveToStream(GenericImage img, Stream stream, string mimetype)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithMimeType((IntPtr)img.Handler, outputStream, mimetype);
        }

        /// <inheritdoc/>
        public override bool GenericImageSaveToFile(GenericImage img, string filename, BitmapType bitmapType)
        {
            return UI.Native.GenericImage.SaveFileWithBitmapType((IntPtr)img.Handler, filename, (int)bitmapType);
        }

        /// <inheritdoc/>
        public override bool GenericImageSaveToFile(GenericImage img, string filename, string mimetype)
        {
            return UI.Native.GenericImage.SaveFileWithMimeType((IntPtr)img.Handler, filename, mimetype);
        }

        /// <inheritdoc/>
        public override bool GenericImageSaveToFile(GenericImage img, string filename)
        {
            return UI.Native.GenericImage.SaveFile((IntPtr)img.Handler, filename);
        }

        /// <inheritdoc/>
        public override bool GenericImageSaveToStream(GenericImage img, Stream stream, BitmapType type)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return UI.Native.GenericImage.SaveStreamWithBitmapType((IntPtr)img.Handler, outputStream, (int)type);
        }

        /// <inheritdoc/>
        public override void GenericImageSetNativeData(
            GenericImage img,
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false)
        {
            UI.Native.GenericImage.SetDataWithSize((IntPtr)img.Handler, data, new_width, new_height, static_data);
        }

        /// <inheritdoc/>
        public override IntPtr GenericImageGetNativeAlphaData(GenericImage img)
        {
            return UI.Native.GenericImage.GetAlphaData((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override IntPtr GenericImageGetNativeData(GenericImage img)
        {
            return UI.Native.GenericImage.GetData((IntPtr)img.Handler);
        }

        /// <inheritdoc/>
        public override bool GenericImageCreateNativeData(
            GenericImage img,
            int width,
            int height,
            IntPtr data,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateData((IntPtr)img.Handler, width, height, data, staticData);
        }

        /// <inheritdoc/>
        public override bool GenericImageCreateNativeData(
            GenericImage img,
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false)
        {
            return UI.Native.GenericImage.CreateAlphaData((IntPtr)img.Handler, width, height, data, alpha, staticData);
        }

        /// <inheritdoc/>
        public override void GenericImageSetNativeAlphaData(
            GenericImage img,
            IntPtr alpha = default,
            bool staticData = false)
        {
            UI.Native.GenericImage.SetAlphaData((IntPtr)img.Handler, alpha, staticData);
        }

        public override void GenericImageSetNativeData(GenericImage img, IntPtr data, bool staticData = false)
        {
            UI.Native.GenericImage.SetData((IntPtr)img.Handler, data, staticData);
        }
    }
}
