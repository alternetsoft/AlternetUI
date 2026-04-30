using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class ImageList : Alternet.Drawing.IImageListHandler
    {
        public bool IsDummy => false;

        public bool IsOk => true;

        public bool IsReadOnly => false;

        Alternet.Drawing.SizeI Alternet.Drawing.IImageListHandler.Size
        {
            get
            {
                return (PixelImageSizeX, PixelImageSizeY);
            }

            set
            {
                SetPixelImageSize(value.Width, value.Height);
            }
        }

        public bool Add(Alternet.Drawing.Image item)
        {
            AddImage((UI.Native.Image)item.Handler);
            return true;
        }
    }
}
