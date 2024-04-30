using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
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
    }
}