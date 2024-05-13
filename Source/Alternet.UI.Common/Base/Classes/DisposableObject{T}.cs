using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class DisposableObject<T> : DisposableObject
    {
        private T handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        /// <param name="handle">Handle to unmanaged resources.</param>
        /// <param name="disposeHandle">Specifies whether to dispose handle using
        /// <see cref="DisposeUnmanagedResources"/>.</param>
        public DisposableObject(T handle, bool disposeHandle)
            : base(disposeHandle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Gets handle to unmanaged resources.
        /// </summary>
        [Browsable(false)]
        public T Handle
        {
            get => handle;
            protected set => handle = value;
        }
    }
}
