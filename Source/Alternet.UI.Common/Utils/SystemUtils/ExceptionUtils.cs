using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties related to the exceptions.
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if argument is null.
        /// </summary>
        /// <param name="argument">Argument to check.</param>
        /// <param name="paramName">Parameter name. Optional.</param>
        [Conditional("DEBUG")]
        public static void DebugThrowIfNull(
            [NotNull] object? argument,
            string? paramName = null)
        {
            if (argument == null)
            {
                ThrowArgumentNull(paramName);
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if argument is null.
        /// </summary>
        /// <param name="argument">Argument to check.</param>
        /// <param name="paramName">Parameter name. Optional.</param>
        [Conditional("DEBUG")]
        public static void DebugThrowIfNull(
            nint argument,
            string? paramName = null)
        {
            if (argument == IntPtr.Zero)
            {
                ThrowArgumentNull(paramName);
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if argument is null.
        /// </summary>
        /// <param name="argument">Argument to check.</param>
        /// <param name="paramName">Parameter name. Optional.</param>
        [Conditional("DEBUG")]
        public static unsafe void DebugThrowIfNull(
            [NotNull] void* argument,
            string? paramName = null)
        {
            if (argument == null)
            {
                ThrowArgumentNull(paramName);
            }
        }

        [DoesNotReturn]
        internal static void ThrowArgumentNull(string? paramName)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}
