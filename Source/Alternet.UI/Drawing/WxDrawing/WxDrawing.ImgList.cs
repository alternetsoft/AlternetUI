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
        public override SizeI ImageSetGetPreferredBitmapSizeAtScale(object imageSet, double scale)
        {
            return ((UI.Native.ImageSet)imageSet).GetPreferredBitmapSizeAtScale(scale);
        }

        /// <inheritdoc/>
        public override void ImageSetAddImage(object imageSet, int index, Image item)
        {
            ((UI.Native.ImageSet)imageSet).AddImage((UI.Native.Image)item.NativeObject);
        }

        /// <inheritdoc/>
        public override void ImageSetRemoveImage(object imageSet, int index, Image item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override object CreateImageSet()
        {
            return new UI.Native.ImageSet();
        }

        /// <inheritdoc/>
        public override object CreateImageSetFromSvgStream(
            Stream stream,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = new UI.Native.ImageSet();
            using var inputStream = new UI.Native.InputStream(stream, false);
            nativeImage.LoadSvgFromStream(inputStream, width, height, color ?? Color.Black);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override object CreateImageSetFromSvgString(
            string s,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = new UI.Native.ImageSet();
            nativeImage.LoadSvgFromString(s, width, height, color ?? Color.Black);
            return nativeImage;
        }

        /// <inheritdoc/>
        public override SizeI ImageSetGetDefaultSize(object imageSet)
        {
            return ((UI.Native.ImageSet)imageSet).DefaultSize;
        }

        /// <inheritdoc/>
        public override bool ImageSetIsOk(object imageSet)
            => ((UI.Native.ImageSet)imageSet).IsOk;

        /// <inheritdoc/>
        public override bool ImageSetIsReadOnly(object imageSet)
            => ((UI.Native.ImageSet)imageSet).IsReadOnly;

        /// <inheritdoc/>
        public override void ImageSetLoadFromStream(object imageSet, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.ImageSet)imageSet).LoadFromStream(inputStream);
        }

        /// <inheritdoc/>
        public override bool IconSetIsOk(object iconSet) => ((UI.Native.IconSet)iconSet).IsOk();

        /// <inheritdoc/>
        public override SizeI ImageListGetPixelImageSize(object imageList)
        {
            return ((UI.Native.ImageList)imageList).PixelImageSize;
        }

        /// <inheritdoc/>
        public override void ImageListSetPixelImageSize(object imageList, SizeI value)
        {
            ((UI.Native.ImageList)imageList).PixelImageSize = value;
        }

        /// <inheritdoc/>
        public override SizeD ImageListGetImageSize(object imageList)
        {
            return ((UI.Native.ImageList)imageList).ImageSize;
        }

        /// <inheritdoc/>
        public override void ImageListSetImageSize(object imageList, SizeD value)
        {
            ((UI.Native.ImageList)imageList).ImageSize = value;
        }

        /// <inheritdoc/>
        public override object CreateImageList()
        {
            return new UI.Native.ImageList();
        }

        /// <inheritdoc/>
        public override void ImageListAdd(object imageList, int index, Image item)
        {
            ((UI.Native.ImageList)imageList).AddImage((UI.Native.Image)item.NativeObject);
        }

        /// <inheritdoc/>
        public override void ImageListRemove(object imageList, int index, Image item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void IconSetAdd(object iconSet, Image image)
        {
            ((UI.Native.IconSet)iconSet).AddImage((UI.Native.Image)image.NativeObject);
        }

        /// <inheritdoc/>
        public override void IconSetAdd(object iconSet, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.IconSet)iconSet).LoadFromStream(inputStream);
        }

        /// <inheritdoc/>
        public override void IconSetClear(object iconSet)
        {
            ((UI.Native.IconSet)iconSet).Clear();
        }

        /// <inheritdoc/>
        public override object CreateIconSet()
        {
            return new UI.Native.IconSet();
        }
    }
}