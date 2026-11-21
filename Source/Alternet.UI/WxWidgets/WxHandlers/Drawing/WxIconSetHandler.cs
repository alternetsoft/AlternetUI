using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Native
{
    internal partial class IconSet : Alternet.Drawing.IImageContainer
    {
        public bool IsDummy => false;

        public bool IsReadOnly => false;

        bool Alternet.Drawing.IImageContainer.IsOk => IsOk();

        public bool RemoveAt(int index) => false;

        bool Alternet.Drawing.IImageContainer.Clear()
        {
            Clear();
            return true;
        }

        public bool Add(Alternet.Drawing.Image image)
        {
            var img = UI.Application.ToNative(image);

            if (img != null)
                AddImage(img);
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
