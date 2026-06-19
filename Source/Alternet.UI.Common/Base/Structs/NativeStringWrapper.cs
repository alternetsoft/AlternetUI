using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a wrapper for a native string pointer.
    /// </summary>
    public sealed class NativeStringWrapper : IDisposable
    {
        /// <summary>
        /// Gets the pointer to the native string.
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeStringWrapper"/> class with the specified string.
        /// </summary>
        /// <param name="managedString">The managed string to be wrapped.</param>
        /// <exception cref="PlatformNotSupportedException">Thrown when
        /// the current platform is not supported.</exception>
        public NativeStringWrapper(string managedString)
        {
            if (App.IsWindowsOS)
            {
                // Windows expects UTF-16
                Pointer = Marshal.StringToHGlobalUni(managedString);
            }
            else if (App.IsLinuxOS || App.IsMacOS)
            {
                // Linux/macOS expect UTF-8
                Pointer = Marshal.StringToCoTaskMemUTF8(managedString);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (Pointer != IntPtr.Zero)
            {
                if (App.IsWindowsOS)
                    Marshal.FreeHGlobal(Pointer);
                else
                    Marshal.FreeCoTaskMem(Pointer);

                Pointer = IntPtr.Zero;
            }
        }
    }
}
