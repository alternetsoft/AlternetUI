using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        public abstract SizeI ImageListGetPixelImageSize(object imageList);

        public abstract void ImageListSetPixelImageSize(object imageList, SizeI value);

        public abstract SizeD ImageListGetImageSize(object imageList);

        public abstract void ImageListSetImageSize(object imageList, SizeD value);

        public abstract object CreateImageList();

        public abstract void ImageListAdd(object imageList, int index, Image item);

        public abstract void ImageListRemove(object imageList, int index, Image item);
    }
}
