using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        public abstract SizeI ImageSetGetPreferredBitmapSizeFor(ImageSet imageSet, IControl control);

        public abstract SizeI ImageSetGetPreferredBitmapSizeAtScale(ImageSet imageSet, double scale);

        public abstract void ImageSetAddImage(ImageSet imageSet, int index, Image item);

        public abstract void ImageSetRemoveImage(ImageSet imageSet, int index, Image item);

        public abstract object CreateImageSet();

        public abstract object CreateImageSetFromSvgStream(
            Stream stream,
            int width,
            int height,
            Color? color = null);

        public abstract object CreateImageSetFromSvgString(
            string s,
            int width,
            int height,
            Color? color = null);

        public abstract SizeI ImageSetGetDefaultSize(ImageSet imageSet);

        public abstract bool ImageSetIsOk(ImageSet imageSet);

        public abstract bool ImageSetIsReadOnly(ImageSet imageSet);

        public abstract void ImageSetLoadFromStream(ImageSet imageSet, Stream stream);

        public abstract bool IconSetIsOk(IconSet iconSet);

        public abstract void IconSetAdd(IconSet iconSet, Image image);

        public abstract void IconSetAdd(IconSet iconSet, Stream stream);

        public abstract void IconSetClear(IconSet iconSet);

        public abstract object CreateIconSet();

        public abstract SizeI ImageListGetPixelImageSize(ImageList imgList);

        public abstract void ImageListSetPixelImageSize(ImageList imgList, SizeI value);

        public abstract SizeD ImageListGetImageSize(ImageList imgList);

        public abstract void ImageListSetImageSize(ImageList imgList, SizeD value);

        public abstract object CreateImageList();

        public abstract void ImageListAdd(ImageList imgList, int index, Image item);

        public abstract void ImageListRemove(ImageList imgList, int index, Image item);
    }
}
