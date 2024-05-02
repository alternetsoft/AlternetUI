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
        public override SizeI ImageSetGetPreferredBitmapSizeAtScale(object imageSet, double scale)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageSetAddImage(object imageSet, int index, Image item)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void ImageSetRemoveImage(object imageSet, int index, Image item)
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
        public override SizeI ImageSetGetDefaultSize(object imageSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSetIsOk(object imageSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSetIsReadOnly(object imageSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageSetLoadFromStream(object imageSet, Stream stream)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override bool IconSetIsOk(object iconSet)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI ImageListGetPixelImageSize(object imageList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageListSetPixelImageSize(object imageList, SizeI value)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override SizeD ImageListGetImageSize(object imageList)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageListSetImageSize(object imageList, SizeD value)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override object CreateImageList()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void ImageListAdd(object imageList, int index, Image item)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void ImageListRemove(object imageList, int index, Image item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void IconSetAdd(object iconSet, Image image)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void IconSetAdd(object iconSet, Stream stream)
        {
            NotImplemented();
        }

        /// <inheritdoc/>
        public override void IconSetClear(object iconSet)
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
