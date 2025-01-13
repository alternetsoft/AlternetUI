﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties related to the exceptions.
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Rethrows the specified exception with the correct call stack information.
        /// </summary>
        /// <param name="e">Previously thrown exception.</param>
        public static void Rethrow(Exception? e)
        {
            if(e is not null)
                ExceptionDispatchInfo.Capture(e).Throw();
        }

        /// <summary>
        /// Configure unhandled exception mode
        /// so default error dialog will be shown.
        /// </summary>
        public static void ForceUnhandledExceptionToUseDialog()
        {
            if (!UnhandledExceptionUsesDialog())
            {
                App.SetUnhandledExceptionModes(UnhandledExceptionMode.CatchWithDialog);
                App.Log("Execute App.SetUnhandledExceptionModes(CatchWithDialog)");
            }
        }

        /// <summary>
        /// Gets whether unhandled exception mode is configured
        /// so default error dialog will be shown.
        /// </summary>
        /// <returns></returns>
        public static bool UnhandledExceptionUsesDialog()
        {
            var mode = App.GetUnhandledExceptionMode();
            switch (mode)
            {
                default:
                case UnhandledExceptionMode.CatchException:
                case UnhandledExceptionMode.ThrowException:
                    return false;
                case UnhandledExceptionMode.CatchWithDialog:
                case UnhandledExceptionMode.CatchWithDialogAndThrow:
                case UnhandledExceptionMode.CatchWithThrow:
                    return true;
            }
        }

        /// <summary>
        /// Gets whether the specified exception is considered critical.
        /// </summary>
        /// <param name="ex">Exception to test.</param>
        /// <returns></returns>
        public static bool IsCriticalException(Exception ex)
        {
            ex = UnwrapTargetInvocationException(ex);

            return ex is NullReferenceException ||
                   ex is StackOverflowException ||
                   ex is OutOfMemoryException ||
                   ex is System.Threading.ThreadAbortException ||
                   ex is System.Runtime.InteropServices.SEHException ||
                   ex is System.Security.SecurityException;
        }

        /// <summary>
        /// Gets whether the specified exception is considered critical
        /// when it arise during callbacks into application code.
        /// </summary>
        /// <param name="ex">Exception to test.</param>
        /// <returns></returns>
        public static bool IsCriticalApplicationException(Exception ex)
        {
            ex = UnwrapTargetInvocationException(ex);

            return ex is StackOverflowException ||
                   ex is OutOfMemoryException ||
                   ex is System.Threading.ThreadAbortException ||
                   ex is System.Security.SecurityException;
        }

        /// <summary>
        /// Gets inner exception.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <returns></returns>
        public static Exception UnwrapTargetInvocationException(Exception ex)
        {
            while (ex.InnerException != null &&
                    (ex is System.Reflection.TargetInvocationException))
            {
                ex = ex.InnerException;
            }

            return ex;
        }

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
