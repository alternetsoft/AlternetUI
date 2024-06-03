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
        public bool IsDummy => false;

        public bool IsReadOnly => false;

        bool Alternet.Drawing.IImageContainer.IsOk => IsOk();

        public bool Remove(int index) => false;

        bool Alternet.Drawing.IImageContainer.Clear()
        {
            Clear();
            return true;
        }

        public bool Add(Alternet.Drawing.Image image)
        {
            AddImage((UI.Native.Image)image.Handler);
            return true;
        }

        public bool Add(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            LoadFromStream(inputStream);
            return true;
        }
    }
}
