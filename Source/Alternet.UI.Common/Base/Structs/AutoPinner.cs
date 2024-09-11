using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to pin object so it will not be moved in the memory.
    /// </summary>
    public struct AutoPinner : IDisposable
    {
        private GCHandle pinnedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPinner"/> class.
        /// </summary>
        /// <param name="obj">Object to pin.</param>
        public AutoPinner(object obj)
        {
            pinnedObject = GCHandle.Alloc(obj, GCHandleType.Pinned);
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from <see cref="AutoPinner"/> to
        /// <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="ap">Value to convert.</param>
        public static implicit operator IntPtr(AutoPinner ap)
        {
            return ap.pinnedObject.AddrOfPinnedObject();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            pinnedObject.Free();
        }
    }
}
