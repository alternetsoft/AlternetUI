using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class ImageList : Alternet.Drawing.IImageListHandler
    {
        public void Add(Alternet.Drawing.Image item)
        {
            AddImage((UI.Native.Image)item.Handler);
        }
    }
}
