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
        public Alternet.Drawing.SizeI GetPreferredBitmapSizeFor(IControl control)
        {
            return GetPreferredBitmapSizeFor(WxApplicationHandler.WxWidget(control));
        }

        void Alternet.Drawing.IImageSetHandler.Add(Alternet.Drawing.Image item)
        {
            AddImage((UI.Native.Image)item.Handler);
        }

        public void LoadFromStream(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            LoadFromStream(inputStream);
        }
    }
}
