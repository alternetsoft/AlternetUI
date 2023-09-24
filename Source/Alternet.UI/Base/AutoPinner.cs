using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class AutoPinner : IDisposable
    {
        private GCHandle pinnedObject;

        public AutoPinner(object obj)
        {
            pinnedObject = GCHandle.Alloc(obj, GCHandleType.Pinned);
        }

        public static implicit operator IntPtr(AutoPinner ap)
        {
            return ap.pinnedObject.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            pinnedObject.Free();
        }
    }
}
