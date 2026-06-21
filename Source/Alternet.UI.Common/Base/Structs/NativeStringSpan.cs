using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a span of characters in the native format used by the operating system.
    /// For example, on Windows, this would be a span of UTF-16 characters. On Linux or macOS,
    /// this would be a span of UTF-8 characters.
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NativeStringSpan
    {
        /// <summary>
        /// Gets a pointer to the native string data.
        /// </summary>
        public IntPtr Pointer;

        /// <summary>
        /// Gets the length of the native string in characters.
        /// </summary>
        public int Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeStringSpan"/>
        /// struct with the zero pointer and zero length.
        /// </summary>
        public NativeStringSpan()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeStringSpan"/>
        /// struct with the specified pointer and length.
        /// </summary>
        public NativeStringSpan(IntPtr pointer, int length)
        {
            Pointer = pointer;
            Length = length;
        }
    }
}
