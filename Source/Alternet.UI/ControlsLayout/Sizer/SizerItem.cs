using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class SizerItem : DisposableObject, ISizerItem
    {
        public SizerItem(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }
    }
}
