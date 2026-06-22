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
        /// Calls the specified function with a <see cref="NativeStringSpan"/> representation of the
        /// string parameters and returns the result.
        /// </summary>
        /// <param name="s1">The first string to convert to a native string span.</param>
        /// <param name="s2">The second string to convert to a native string span.</param>
        /// <param name="callback">The function to invoke with the native string spans.</param>
        /// <returns>The result returned by the callback.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Invoke<T>(
                  string? s1,
                  string? s2,
                  Func<NativeStringSpan, NativeStringSpan, T> callback)
        {
            return NativeStringSpan.InvokeWithResult(s1, s2, callback);
        }

        /// <summary>
        /// Calls the specified action with a <see cref="NativeStringSpan"/> representation of the
        /// string parameters.
        /// </summary>
        /// <param name="s1">The first string to convert to a native string span.</param>
        /// <param name="s2">The second string to convert to a native string span.</param>
        /// <param name="callback">The action to invoke with the native string spans.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(
                  string? s1,
                  string? s2,
                  Action<NativeStringSpan, NativeStringSpan> callback)
        {
            NativeStringSpan.Invoke(s1, s2, callback);
        }

        /// <summary>
        /// Calls the specified action with a <see cref="NativeStringSpan"/> representation of the
        /// string parameters.
        /// </summary>
        /// <param name="s1">The first string to convert to a native string span.</param>
        /// <param name="s2">The second string to convert to a native string span.</param>
        /// <param name="s3">The third string to convert to a native string span.</param>
        /// <param name="callback">The action to invoke with the native string spans.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invoke(
                  string? s1,
                  string? s2,
                  string? s3,
                  Action<NativeStringSpan, NativeStringSpan, NativeStringSpan> callback)
        {
            StringUtils.InvokeWithNativeText(s1 ?? string.Empty, span =>
            {
                StringUtils.InvokeWithNativeText(s2 ?? string.Empty, span2 =>
                {
                    StringUtils.InvokeWithNativeText(s3 ?? string.Empty, span3 =>
                    {
                        callback(span, span2, span3);
                    });
                });
            });
        }

        /// <summary>
        /// Calls the specified function with a <see cref="NativeStringSpan"/> representation of the
        /// string parameters and returns the result.
        /// </summary>
        /// <param name="s1">The first string to convert to a native string span.</param>
        /// <param name="s2">The second string to convert to a native string span.</param>
        /// <param name="s3">The third string to convert to a native string span.</param>
        /// <param name="callback">The function to invoke with the native string spans.</param>
        /// <returns>The result returned by the callback.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Invoke<T>(
                  string? s1,
                  string? s2,
                  string? s3,
                  Func<NativeStringSpan, NativeStringSpan, NativeStringSpan, T> callback)
        {
            return StringUtils.InvokeWithResult(s1 ?? string.Empty, span =>
            {
                return StringUtils.InvokeWithResult(s2 ?? string.Empty, span2 =>
                {
                    return StringUtils.InvokeWithResult(s3 ?? string.Empty, span3 =>
                    {
                        return callback(span, span2, span3);
                    });
                });
            });
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
