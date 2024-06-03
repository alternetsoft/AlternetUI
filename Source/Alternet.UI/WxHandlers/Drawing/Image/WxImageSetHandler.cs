using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class ImageSet : Alternet.Drawing.IImageSetHandler
    {
        public bool IsDummy => false;

        bool Alternet.Drawing.IImageContainer.Clear()
        {
            Clear();
            return true;
        }

        bool Alternet.Drawing.IImageContainer.Remove(int imageIndex)
        {
            return false;    
        }

        public Alternet.Drawing.SizeI GetPreferredBitmapSizeFor(IControl control)
        {
            return GetPreferredBitmapSizeFor(WxApplicationHandler.WxWidget(control));
        }

        bool Alternet.Drawing.IImageContainer.Add(Alternet.Drawing.Image item)
        {
            AddImage((UI.Native.Image)item.Handler);
            return true;
        }

        public bool LoadFromStream(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            LoadFromStream(inputStream);
            return true;
        }
    }
}
