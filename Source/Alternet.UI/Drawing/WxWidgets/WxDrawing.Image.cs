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
        public override object CreateImage()
        {
            return new UI.Native.Image();
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, string fileName)
        {
            return ((UI.Native.Image)image).SaveToFile(fileName);
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, Stream stream, ImageFormat format)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return ((UI.Native.Image)image).SaveToStream(outputStream, format.ToString());
        }

        /// <inheritdoc/>
        public override SizeI GetImagePixelSize(object image)
        {
            return ((UI.Native.Image)image).PixelSize;
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(object image, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            if (inputStream is null)
                return false;

            return ((UI.Native.Image)image).LoadFromStream(inputStream);
        }

        /// <inheritdoc/>
        public override bool GetImageIsOk(object image)
        {
            return ((UI.Native.Image)image).IsOk;
        }

        /// <inheritdoc/>
        public override object CreateImageFromGenericImage(object genericImage, int depth = -1)
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).LoadFromGenericImage((IntPtr)genericImage, depth);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphics(int width, int height, object dc)
        {
            var nativeImage = CreateImage();
            UI.Native.DrawingContext.ImageFromDrawingContext(
                (UI.Native.Image)nativeImage,
                width,
                height,
                (UI.Native.DrawingContext)dc);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphicsAndGenericImage(object genericImage, object dc)
        {
            var nativeImage = CreateImage();
            UI.Native.DrawingContext.ImageFromGenericImageDC(
                (UI.Native.Image)nativeImage,
                (IntPtr)genericImage,
                (UI.Native.DrawingContext)dc);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromImage(object original, SizeI newSize)
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).InitializeFromImage((UI.Native.Image)original, newSize);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageWithSizeAndDepth(SizeI size, int depth = 32)
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).Initialize(size, depth);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override BitmapType GetDefaultBitmapType()
        {
            return (BitmapType)UI.Native.Image.GetDefaultBitmapType();
        }

        /// <inheritdoc/>
        public override int GetImageDepth(object image)
        {
            return ((UI.Native.Image)image).Depth;
        }

        /// <inheritdoc/>
        public override SizeI GetImageDipSize(object image)
        {
            return ((UI.Native.Image)image).DipSize;
        }

        /// <inheritdoc/>
        public override bool GetImageHasAlpha(object image)
        {
            return ((UI.Native.Image)image).HasAlpha;
        }

        /// <inheritdoc/>
        public override double GetImageScaledHeight(object image)
        {
            return ((UI.Native.Image)image).ScaledHeight;
        }

        /// <inheritdoc/>
        public override SizeI GetImageScaledSize(object image)
        {
            return ((UI.Native.Image)image).ScaledSize;
        }

        /// <inheritdoc/>
        public override double GetImageScaledWidth(object image)
        {
            return ((UI.Native.Image)image).ScaledWidth;
        }

        /// <inheritdoc/>
        public override double GetImageScaleFactor(object image)
        {
            return ((UI.Native.Image)image).ScaleFactor;
        }

        /// <inheritdoc/>
        public override object ImageConvertToDisabled(object image, byte brightness = 255)
        {
            var converted = ((UI.Native.Image)image).ConvertToDisabled(brightness);
            return converted;
        }

        /// <inheritdoc/>
        public override object ImageGetSubBitmap(object image, RectI rect)
        {
            var converted = ((UI.Native.Image)image).GetSubBitmap(rect);
            return converted;
        }

        /// <inheritdoc/>
        public override bool ImageLoad(object image, string name, BitmapType type)
        {
            return ((UI.Native.Image)image).LoadFile(name, (int)type);
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(object image, Stream stream, BitmapType type)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return ((UI.Native.Image)image).LoadStream(inputStream, (int)type);
        }

        /// <inheritdoc/>
        public override void ImageRescale(object image, SizeI sizeNeeded)
        {
            ((UI.Native.Image)image).Rescale(sizeNeeded);
        }

        /// <inheritdoc/>
        public override bool ImageSaveToFile(object image, string name, BitmapType type)
        {
            return ((UI.Native.Image)image).SaveFile(name, (int)type);
        }

        /// <inheritdoc/>
        public override bool ImageSaveToStream(object image, Stream stream, BitmapType type)
        {
            using var outputStream = new UI.Native.OutputStream(stream);
            return ((UI.Native.Image)image).SaveStream(outputStream, (int)type);
        }

        /// <inheritdoc/>
        public override void ImageResetAlpha(object image)
        {
            ((UI.Native.Image)image).ResetAlpha();
        }

        /// <inheritdoc/>
        public override void SetImageHasAlpha(object image, bool hasAlpha)
        {
            ((UI.Native.Image)image).HasAlpha = hasAlpha;
        }

        /// <inheritdoc/>
        public override void SetImageScaleFactor(object image, double value)
        {
            ((UI.Native.Image)image).ScaleFactor = value;
        }

        /// <inheritdoc/>
        public override object CreateImageFromScreen()
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).InitializeFromScreen();
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromSvgStream(
            Stream stream,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = CreateImage();
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.Image)nativeImage).LoadSvgFromStream(
                inputStream,
                width,
                height,
                color ?? Color.Black);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromSvgString(
            string s,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = new UI.Native.Image();
            nativeImage.LoadSvgFromString(s, width, height, color ?? Color.Black);
            return nativeImage;
        }
    }
}
