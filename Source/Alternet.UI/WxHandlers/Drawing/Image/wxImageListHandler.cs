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
                return PixelImageSize;
            }

            set
            {
                PixelImageSize = value;
            }
        }

        public bool Add(Alternet.Drawing.Image item)
        {
            AddImage((UI.Native.Image)item.Handler);
            return true;
        }
    }
}
