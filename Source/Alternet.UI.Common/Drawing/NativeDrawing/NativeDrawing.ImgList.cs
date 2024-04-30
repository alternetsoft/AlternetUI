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
        public abstract SizeI ImageSetGetPreferredBitmapSizeAtScale(object imageSet, double scale);

        public abstract void ImageSetAddImage(object imageSet, int index, Image item);

        public abstract void ImageSetRemoveImage(object imageSet, int index, Image item);

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

        public abstract SizeI ImageSetGetDefaultSize(object imageSet);

        public abstract bool ImageSetIsOk(object imageSet);

        public abstract bool ImageSetIsReadOnly(object imageSet);

        public abstract void ImageSetLoadFromStream(object imageSet, Stream stream);

        public abstract bool IconSetIsOk(object iconSet);

        public abstract void IconSetAdd(object iconSet, Image image);

        public abstract void IconSetAdd(object iconSet, Stream stream);

        public abstract void IconSetClear(object iconSet);

        public abstract object CreateIconSet();

        public abstract SizeI ImageListGetPixelImageSize(object imageList);

        public abstract void ImageListSetPixelImageSize(object imageList, SizeI value);

        public abstract SizeD ImageListGetImageSize(object imageList);

        public abstract void ImageListSetImageSize(object imageList, SizeD value);

        public abstract object CreateImageList();

        public abstract void ImageListAdd(object imageList, int index, Image item);

        public abstract void ImageListRemove(object imageList, int index, Image item);
    }
}
