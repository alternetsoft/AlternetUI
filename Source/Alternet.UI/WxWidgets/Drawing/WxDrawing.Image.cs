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
        public override object CreateImage(ImageSet imageSet, SizeI size)
        {
            var image = new UI.Native.Image();
            ((UI.Native.ImageSet)imageSet.Handler).InitImage(
                image,
                size.Width,
                size.Height);
            return image;
        }

        public override object ImageConvertToGenericImage(Image image)
        {
            return ((UI.Native.Image)image.NativeObject).ConvertToGenericImage();
        }

        /// <inheritdoc/>
        public override object CreateImage()
        {
            return new UI.Native.Image();
        }

        /// <inheritdoc/>
        public override bool ImageSave(Image img, string fileName)
        {
            return ((UI.Native.Image)img.NativeObject).SaveToFile(fileName);
        }

        /// <inheritdoc/>
        public override bool ImageSave(Image img, Stream stream, ImageFormat format)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return ((UI.Native.Image)img.NativeObject).SaveToStream(outputStream, format.ToString());
        }

        /// <inheritdoc/>
        public override SizeI GetImagePixelSize(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).PixelSize;
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(Image img, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            if (inputStream is null)
                return false;

            return ((UI.Native.Image)img.NativeObject).LoadFromStream(inputStream);
        }

        /// <inheritdoc/>
        public override bool GetImageIsOk(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).IsOk;
        }

        /// <inheritdoc/>
        public override object CreateImageFromGenericImage(GenericImage genericImage, int depth = -1)
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).LoadFromGenericImage((IntPtr)genericImage.Handler, depth);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphics(int width, int height, Graphics dc)
        {
            var nativeImage = CreateImage();
            UI.Native.DrawingContext.ImageFromDrawingContext(
                (UI.Native.Image)nativeImage,
                width,
                height,
                (UI.Native.DrawingContext)dc.NativeObject);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImage(ImageSet imageSet, IControl control)
        {
            var nativeObject = NativeDrawing.Default.CreateImage();
            ((UI.Native.ImageSet)imageSet.Handler).InitImageFor(
                (UI.Native.Image)nativeObject,
                WxPlatform.WxWidget(control));
            return nativeObject;
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphicsAndGenericImage(
            GenericImage genericImage,
            Graphics dc)
        {
            var nativeImage = CreateImage();
            UI.Native.DrawingContext.ImageFromGenericImageDC(
                (UI.Native.Image)nativeImage,
                (IntPtr)genericImage.Handler,
                (UI.Native.DrawingContext)dc.NativeObject);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromImage(Image image)
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).CopyFrom((UI.Native.Image)image.NativeObject);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageFromImage(Image original, SizeI newSize)
        {
            var nativeImage = CreateImage();
            ((UI.Native.Image)nativeImage).InitializeFromImage(
                (UI.Native.Image)original.NativeObject,
                newSize);
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
        public override int GetImageDepth(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).Depth;
        }

        /// <inheritdoc/>
        public override SizeI GetImageDipSize(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).DipSize;
        }

        /// <inheritdoc/>
        public override bool GetImageHasAlpha(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).HasAlpha;
        }

        /// <inheritdoc/>
        public override double GetImageScaledHeight(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).ScaledHeight;
        }

        /// <inheritdoc/>
        public override SizeI GetImageScaledSize(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).ScaledSize;
        }

        /// <inheritdoc/>
        public override double GetImageScaledWidth(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).ScaledWidth;
        }

        /// <inheritdoc/>
        public override double GetImageScaleFactor(Image img)
        {
            return ((UI.Native.Image)img.NativeObject).ScaleFactor;
        }

        /// <inheritdoc/>
        public override object ImageConvertToDisabled(Image img, byte brightness = 255)
        {
            var converted = ((UI.Native.Image)img.NativeObject).ConvertToDisabled(brightness);
            return converted;
        }

        /// <inheritdoc/>
        public override object ImageGetSubBitmap(Image img, RectI rect)
        {
            var converted = ((UI.Native.Image)img.NativeObject).GetSubBitmap(rect);
            return converted;
        }

        /// <inheritdoc/>
        public override bool ImageLoad(Image img, string name, BitmapType type)
        {
            return ((UI.Native.Image)img.NativeObject).LoadFile(name, (int)type);
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(Image img, Stream stream, BitmapType type)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return ((UI.Native.Image)img.NativeObject).LoadStream(inputStream, (int)type);
        }

        /// <inheritdoc/>
        public override void ImageRescale(Image img, SizeI sizeNeeded)
        {
            ((UI.Native.Image)img.NativeObject).Rescale(sizeNeeded);
        }

        /// <inheritdoc/>
        public override bool ImageSaveToFile(Image img, string name, BitmapType type)
        {
            return ((UI.Native.Image)img.NativeObject).SaveFile(name, (int)type);
        }

        /// <inheritdoc/>
        public override bool ImageSaveToStream(Image img, Stream stream, BitmapType type)
        {
            using var outputStream = new UI.Native.OutputStream(stream);
            return ((UI.Native.Image)img.NativeObject).SaveStream(outputStream, (int)type);
        }

        /// <inheritdoc/>
        public override void ImageResetAlpha(Image img)
        {
            ((UI.Native.Image)img.NativeObject).ResetAlpha();
        }

        /// <inheritdoc/>
        public override void SetImageHasAlpha(Image img, bool hasAlpha)
        {
            ((UI.Native.Image)img.NativeObject).HasAlpha = hasAlpha;
        }

        /// <inheritdoc/>
        public override void SetImageScaleFactor(Image img, double value)
        {
            ((UI.Native.Image)img.NativeObject).ScaleFactor = value;
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
