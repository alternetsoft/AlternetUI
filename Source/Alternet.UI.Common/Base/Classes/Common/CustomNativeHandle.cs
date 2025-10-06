using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a custom native handle wrapper for platform-specific objects.
    /// </summary>
    public class CustomNativeHandle : DisposableObject, IEquatable<CustomNativeHandle>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomNativeHandle"/> class.
        /// </summary>
        public CustomNativeHandle()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomNativeHandle"/> class with the specified handle.
        /// </summary>
        /// <param name="handle">The native handle object to wrap.</param>
        public CustomNativeHandle(object handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Gets or sets the underlying native handle object.
        /// </summary>
        public virtual object? Handle { get; set; }

        /// <summary>
        /// Gets the handle as a pointer.
        /// </summary>
        public IntPtr AsPointer
        {
            get
            {
                return (IntPtr?)Handle ?? IntPtr.Zero;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Handle?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not CustomNativeHandle other)
                return false;
            return Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(CustomNativeHandle? other)
        {
            if (other == null)
                return false;
            return Equals(Handle, other.Handle) && GetType() == other.GetType();
        }
    }
}
