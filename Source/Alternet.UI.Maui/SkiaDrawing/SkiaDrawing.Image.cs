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
        /// <inheritdoc/>
        public override bool GetImageIsOk(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(object image, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromGenericImage(object genericImage, int depth = -1)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphics(int width, int height, object dc)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphicsAndGenericImage(object genericImage, object dc)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromImage(object original, SizeI newSize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromScreen()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromSvgStream(
            Stream stream,
            int width,
            int height,
            Color? color = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromSvgString(
            string s,
            int width,
            int height,
            Color? color = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageWithSizeAndDepth(SizeI size, int depth = 32)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override BitmapType GetDefaultBitmapType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override int GetImageDepth(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImageDipSize(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool GetImageHasAlpha(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override double GetImageScaledHeight(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImageScaledSize(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override double GetImageScaledWidth(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override double GetImageScaleFactor(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object ImageConvertToDisabled(object image, byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object ImageGetSubBitmap(object image, RectI rect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageLoad(object image, string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(object image, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageRescale(object image, SizeI sizeNeeded)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSaveToFile(object image, string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSaveToStream(object image, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageResetAlpha(object image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetImageHasAlpha(object image, bool hasAlpha)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetImageScaleFactor(object image, double value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImage()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, string fileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, Stream stream, ImageFormat format)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImagePixelSize(object image)
        {
            throw new NotImplementedException();
        }
    }
}
