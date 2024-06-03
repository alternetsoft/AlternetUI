using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class DummyImageContainer : DisposableObject, IImageContainer
    {
        public virtual bool IsOk => true;

        public virtual bool IsReadOnly => false;

        public virtual bool IsDummy => true;

        public virtual bool Add(Image image)
        {
            return true;
        }

        public virtual bool Clear()
        {
            return true;
        }

        public virtual bool Remove(int imageIndex)
        {
            return true;
        }

        public virtual bool LoadFromStream(Stream stream)
        {
            return true;
        }
    }
}
