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
        public override SizeI ImageSetGetPreferredBitmapSizeFor(ImageSet imageSet, IControl control)
        {
            return ((UI.Native.ImageSet)imageSet.Handler)
                .GetPreferredBitmapSizeFor(WxPlatform.WxWidget(control));
        }

        /// <inheritdoc/>
        public override SizeI ImageSetGetPreferredBitmapSizeAtScale(ImageSet imgSet, double scale)
        {
            return ((UI.Native.ImageSet)imgSet.Handler).GetPreferredBitmapSizeAtScale(scale);
        }

        /// <inheritdoc/>
        public override void ImageSetAddImage(ImageSet imgSet, int index, Image item)
        {
            ((UI.Native.ImageSet)imgSet.Handler).AddImage((UI.Native.Image)item.NativeObject);
        }

        /// <inheritdoc/>
        public override void ImageSetRemoveImage(ImageSet imgSet, int index, Image item)
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
        public override SizeI ImageSetGetDefaultSize(ImageSet imgSet)
        {
            return ((UI.Native.ImageSet)imgSet.Handler).DefaultSize;
        }

        /// <inheritdoc/>
        public override bool ImageSetIsOk(ImageSet imgSet)
            => ((UI.Native.ImageSet)imgSet.Handler).IsOk;

        /// <inheritdoc/>
        public override bool ImageSetIsReadOnly(ImageSet imgSet)
            => ((UI.Native.ImageSet)imgSet.Handler).IsReadOnly;

        /// <inheritdoc/>
        public override void ImageSetLoadFromStream(ImageSet imgSet, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.ImageSet)imgSet.Handler).LoadFromStream(inputStream);
        }

        /// <inheritdoc/>
        public override bool IconSetIsOk(IconSet icnSet) => ((UI.Native.IconSet)icnSet.Handler).IsOk();

        /// <inheritdoc/>
        public override SizeI ImageListGetPixelImageSize(ImageList imgList)
        {
            return ((UI.Native.ImageList)imgList.Handler).PixelImageSize;
        }

        /// <inheritdoc/>
        public override void ImageListSetPixelImageSize(ImageList imgList, SizeI value)
        {
            ((UI.Native.ImageList)imgList.Handler).PixelImageSize = value;
        }

        /// <inheritdoc/>
        public override SizeD ImageListGetImageSize(ImageList imgList)
        {
            return ((UI.Native.ImageList)imgList.Handler).ImageSize;
        }

        /// <inheritdoc/>
        public override void ImageListSetImageSize(ImageList imgList, SizeD value)
        {
            ((UI.Native.ImageList)imgList.Handler).ImageSize = value;
        }

        /// <inheritdoc/>
        public override object CreateImageList()
        {
            return new UI.Native.ImageList();
        }

        /// <inheritdoc/>
        public override void ImageListAdd(ImageList imgList, int index, Image item)
        {
            ((UI.Native.ImageList)imgList.Handler).AddImage((UI.Native.Image)item.NativeObject);
        }

        /// <inheritdoc/>
        public override void ImageListRemove(ImageList imgList, int index, Image item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void IconSetAdd(IconSet icnSet, Image image)
        {
            ((UI.Native.IconSet)icnSet.Handler).AddImage((UI.Native.Image)image.NativeObject);
        }

        /// <inheritdoc/>
        public override void IconSetAdd(IconSet icnSet, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.IconSet)icnSet.Handler).LoadFromStream(inputStream);
        }

        /// <inheritdoc/>
        public override void IconSetClear(IconSet icnSet)
        {
            ((UI.Native.IconSet)icnSet.Handler).Clear();
        }

        /// <inheritdoc/>
        public override object CreateIconSet()
        {
            return new UI.Native.IconSet();
        }
    }
}