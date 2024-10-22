using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="DisposableObject"/> with <see cref="Handle"/> property
    /// of the <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Handle"/> property.</typeparam>
    public class DisposableObject<T> : DisposableObject
    {
        private T handle = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject{T}"/> class.
        /// </summary>
        /// <param name="handle">Handle to unmanaged resources.</param>
        /// <param name="disposeHandle">Specifies whether to dispose handle using
        /// <see cref="DisposableObject.DisposeUnmanaged"/>.</param>
        public DisposableObject(T handle, bool disposeHandle)
            : base(disposeHandle)
        {
            this.handle = handle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject{T}"/> class.
        /// </summary>
        /// <param name="disposeHandle">Specifies whether to dispose handle using
        /// <see cref="DisposableObject.DisposeUnmanaged"/>.</param>
        public DisposableObject(bool disposeHandle)
            : base(disposeHandle)
        {
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
