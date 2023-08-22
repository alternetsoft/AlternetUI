using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PropertyGridItem : IPropertyGridItem
    {
        private IntPtr handle;

        public PropertyGridItem(IntPtr handle)
        {
            this.handle = handle;
        }

        public IntPtr Handle => handle;
    }
}
