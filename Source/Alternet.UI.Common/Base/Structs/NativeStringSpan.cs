using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Operator to convert a <see cref="NativeStringSpan"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="span">The value to convert.</param>
        public static implicit operator string(NativeStringSpan span)
        {
            return ToManagedString(span);
        }

        /// <summary>
        /// Converts the native string span to a managed string.
        /// </summary>
        /// <returns>The managed string representation of the native string span.</returns>
        public static string ToManagedString(NativeStringSpan span)
        {
            if (span.Pointer == IntPtr.Zero || span.Length == 0)
                return string.Empty;

            if (App.IsWindowsOS)
            {
                // Windows: UTF-16
                return Marshal.PtrToStringUni(span.Pointer, span.Length);
            }
            else
            {
                // Linux/macOS: UTF-8
                return Marshal.PtrToStringUTF8(span.Pointer, span.Length);
            }
        }

        /// <summary>
        /// Calls the specified action with a <see cref="NativeStringSpan"/> representation of the given string.
        /// </summary>
        /// <param name="s">The string to convert to a native string span.</param>
        /// <param name="action">The action to invoke with the native string span.</param>
        public static void Invoke(string s, Action<NativeStringSpan> action)
        {
            StringUtils.InvokeWithNativeText(s, action);
        }

        /// <summary>
        /// Calls the specified function with a <see cref="NativeStringSpan"/> representation
        /// of the given string and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the callback.</typeparam>
        /// <param name="s">The string to convert to a native string span.</param>
        /// <param name="callback">The function to invoke with the native string span.</param>
        /// <returns>The result returned by the callback.</returns>
        public static T InvokeWithResult<T>(string s, Func<NativeStringSpan, T> callback)
        {
            return StringUtils.InvokeWithResult(s, callback);
        }

        /// <inheritdoc/>
        public readonly override string ToString()
        {
            return ToManagedString(this);
        }
    }
}
