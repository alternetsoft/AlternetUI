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
        /// Represents a method that performs an action on a read-only span
        /// of elements of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>This delegate is commonly used to define operations that work on spans of data
        /// without modifying the underlying memory.</remarks>
        /// <typeparam name="T">The type of the elements in the read-only span.</typeparam>
        /// <param name="span">The read-only span of elements to process.</param>
        public delegate void ReadOnlySpanAction<T>(ReadOnlySpan<T> span);

        /// <summary>
        /// Allocates a <see cref="Span{Char}"/> of the specified length,
        /// fills it with the given character,
        /// and invokes the provided action with the resulting <see cref="ReadOnlySpan{Char}"/>.
        /// Uses stack allocation if the span size is below the threshold; otherwise falls back to heap.
        /// </summary>
        /// <param name="count">The number of characters to allocate and fill.</param>
        /// <param name="ch">The character to fill the span with.</param>
        /// <param name="action">The callback to invoke with the filled span.</param>
        /// <param name="invokeOnEmpty">
        /// If <c>true</c>, the action will be invoked with an empty span
        /// when <paramref name="count"/> is zero or negative.
        /// If <c>false</c>, the action will be skipped in that case.
        /// </param>
        public static void InvokeWithFilledSpan(
            int count,
            char ch,
            ReadOnlySpanAction<char> action,
            bool invokeOnEmpty = false)
        {
            if (count <= 0)
            {
                if (invokeOnEmpty)
                    action(ReadOnlySpan<char>.Empty);
                return;
            }

            if (count <= SpanStackLimit)
            {
                Span<char> buffer = stackalloc char[count];
                buffer.Fill(ch);
                action(buffer);
            }
            else
            {
                char[] buffer = new char[count];
                Array.Fill(buffer, ch);
                action(buffer);
            }
        }

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