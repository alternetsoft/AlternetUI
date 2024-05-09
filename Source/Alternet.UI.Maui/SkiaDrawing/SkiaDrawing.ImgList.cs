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
        public override SizeI ImageSetGetPreferredBitmapSizeFor(ImageSet imageSet, IControl control)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI ImageSetGetPreferredBitmapSizeAtScale(ImageSet imageSet, double scale)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageSetAddImage(ImageSet imageSet, int index, Image item)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void ImageSetRemoveImage(ImageSet imageSet, int index, Image item)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateImageSet()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageSetFromSvgStream(
            Stream stream,
            int width,
            int height,
            Color? color = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageSetFromSvgString(
            string s,
            int width,
            int height,
            Color? color = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI ImageSetGetDefaultSize(ImageSet imageSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSetIsOk(ImageSet imageSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSetIsReadOnly(ImageSet imageSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageSetLoadFromStream(ImageSet imageSet, Stream stream)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override bool IconSetIsOk(IconSet iconSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI ImageListGetPixelImageSize(ImageList imageList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageListSetPixelImageSize(ImageList imageList, SizeI value)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override SizeD ImageListGetImageSize(ImageList imageList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageListSetImageSize(ImageList imageList, SizeD value)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateImageList()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageListAdd(ImageList imageList, int index, Image item)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void ImageListRemove(ImageList imageList, int index, Image item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void IconSetAdd(IconSet iconSet, Image image)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void IconSetAdd(IconSet iconSet, Stream stream)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void IconSetClear(IconSet iconSet)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateIconSet()
        {
            throw new NotImplementedException();
        }
    }
}
