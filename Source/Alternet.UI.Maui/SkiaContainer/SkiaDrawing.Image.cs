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
    public partial class SkiaDrawing
    {
        /// <inheritdoc/>
        public override object CreateImage(ImageSet imageSet, IControl control)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromImage(Image image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImage(ImageSet imageSet, SizeI size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object ImageConvertToGenericImage(Image image)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool GetImageIsOk(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(Image img, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromGenericImage(GenericImage genericImage, int depth = -1)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphics(int width, int height, Graphics dc)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromGraphicsAndGenericImage(
            GenericImage genericImage,
            Graphics dc)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageFromImage(Image original, SizeI newSize)
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
        public override int GetImageDepth(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImageDipSize(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool GetImageHasAlpha(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override double GetImageScaledHeight(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImageScaledSize(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override double GetImageScaledWidth(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override double GetImageScaleFactor(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object ImageConvertToDisabled(Image img, byte brightness = 255)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object ImageGetSubBitmap(Image img, RectI rect)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageLoad(Image img, string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(Image img, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageRescale(Image img, SizeI sizeNeeded)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSaveToFile(Image img, string name, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSaveToStream(Image img, Stream stream, BitmapType type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageResetAlpha(Image img)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetImageHasAlpha(Image img, bool hasAlpha)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void SetImageScaleFactor(Image img, double value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImage()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSave(Image img, string fileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSave(Image img, Stream stream, ImageFormat format)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImagePixelSize(Image img)
        {
            throw new NotImplementedException();
        }
    }
}
