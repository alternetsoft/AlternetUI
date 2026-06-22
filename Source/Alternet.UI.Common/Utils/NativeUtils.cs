using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods for working with strings, spans and other native resources.
    /// </summary>
    public static class NativeUtils
    {
        /// <summary>
        /// Calls the specified function with a <see cref="NativeStringSpan"/> representation
        /// of the given string and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the callback.</typeparam>
        /// <param name="s">The string to convert to a native string span.</param>
        /// <param name="func">The function to invoke with the native string span.</param>
        /// <returns>The result returned by the callback.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Invoke<T>(string s, Func<NativeStringSpan, T> func)
        {
            return NativeStringSpan.InvokeWithResult(s, func);
        }

        /// <summary>
        /// Calls the specified action with a <see cref="NativeStringSpan"/> representation of the given string.
        /// </summary>
        /// <param name="s">The string to convert to a native string span.</param>
        /// <param name="action">The action to invoke with the native string span.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(string s, Action<NativeStringSpan>? action)
        {
            NativeStringSpan.Invoke(s, action);
        }
    }
}
