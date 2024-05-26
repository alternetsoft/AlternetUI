using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class IconSet : Alternet.Drawing.IIconSetHandler
    {
        bool Alternet.Drawing.IIconSetHandler.IsOk => IsOk();

        public void Add(Alternet.Drawing.Image image)
        {
            AddImage((UI.Native.Image)image.Handler);
        }

        public void Add(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            LoadFromStream(inputStream);
        }
    }
}
