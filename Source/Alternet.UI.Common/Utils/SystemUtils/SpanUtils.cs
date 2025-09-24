using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods and constants for working with spans.
    /// </summary>
    /// <remarks>This class contains helper methods and constants designed to facilitate
    /// operations involving spans.
    /// It is intended for use in scenarios where efficient memory management
    /// and performance are critical.</remarks>
    public static class SpanUtils
    {
        /// <summary>
        /// Represents the maximum number of spans that can be stored in a stack.
        /// </summary>
        /// <remarks>This constant defines the upper limit for the size of a span stack. It is used to
        /// prevent excessive memory usage or stack overflow scenarios when working with spans.</remarks>
        public static int SpanStackLimit = 256;

        /// <summary>
        /// Converts the specified UTF-16 character span to UTF-8 and invokes
        /// the provided action with a pointer to the
        /// UTF-8 data and its length.
        /// </summary>
        /// <remarks>This method uses stack allocation for small UTF-8 buffers
        /// and heap allocation for
        /// larger ones. Callers should ensure that the action does not store
        /// or use the pointer outside the scope of
        /// the action.</remarks>
        /// <param name="input">The UTF-16 character span to be converted to UTF-8.
        /// If the span is empty, the action is invoked with a null
        /// pointer and a length of 0.</param>
        /// <param name="action">The action to invoke with the UTF-8 data.
        /// The action receives a pointer to the UTF-8-encoded bytes and the
        /// number of bytes in the UTF-8 data. The pointer is valid only
        /// for the duration of the action's execution.</param>
        public static unsafe void InvokeWithUTF8Span(ReadOnlySpan<char> input, Action<IntPtr, int> action)
        {
            if (input.IsEmpty)
            {
                action(IntPtr.Zero, 0);
                return;
            }

            var utf8 = Encoding.UTF8;
            int byteCount = utf8.GetByteCount(input);

            if (byteCount <= SpanStackLimit)
            {
                Span<byte> buffer = stackalloc byte[byteCount];
                utf8.GetBytes(input, buffer);
                fixed (byte* ptr = buffer)
                {
                    action((IntPtr)ptr, byteCount);
                }
            }
            else
            {
                byte[] buffer = new byte[byteCount];
                utf8.GetBytes(input, buffer);
                fixed (byte* ptr = buffer)
                {
                    action((IntPtr)ptr, byteCount);
                }
            }
        }
    }
}