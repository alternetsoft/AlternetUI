using System;
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
        private static BaseDictionary<int, ExceptionInfo> exceptions = new();

        /// <summary>
        /// Gets the currently visible thread exception window, if one exists.
        /// </summary>
        public static ThreadExceptionWindow? ExceptionWindow
        {
            get
            {
                return App.FindVisibleWindow<ThreadExceptionWindow>();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the exception window is currently visible.
        /// </summary>
        public static bool IsExceptionWindowVisible
        {
            get
            {
                return App.IsWindowVisible<ThreadExceptionWindow>();
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the specified value
        /// is outside the valid range for a byte.
        /// </summary>
        /// <param name="v">The value to validate.</param>
        /// <param name="n">The name of the variable being validated,
        /// used in the exception message.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="v"/>
        /// is less than <see cref="byte.MinValue"/> or greater than
        /// <see cref="byte.MaxValue"/>.</exception>
        public static void ThrowOutOfByteRange(int v, string n)
        {
            throw new ArgumentException(
                string.Format(
                    "Variable '{0}' has invalid value {1}." + " " +
                    "Minimum allowed value is {2}, maximum is {3}.",
                    new object[] { n, v, byte.MinValue, byte.MaxValue }));
        }

        /// <summary>
        /// Generates a unique key string that identifies the specified exception based on its type, message, stack
        /// trace and runtime identifier. If the exception implements <see cref="IBaseObjectWithId"/>,
        /// its unique identifier will be used as the key instead.
        /// </summary>
        /// <remarks>This method can be used to group or track exceptions with identical characteristics.
        /// The returned key may contain sensitive information from the exception message or stack trace; use caution
        /// when logging or exposing it.</remarks>
        /// <param name="ex">The exception for which to generate a unique key. Cannot be null.</param>
        /// <returns>A string that uniquely represents the exception by combining its type, message, and stack trace.</returns>
        public static string GetExceptionKeyAsString(Exception ex)
        {
            if (ex is IBaseObjectWithId objectWithId)
            {
                var result = objectWithId.ToString();

                if (!string.IsNullOrEmpty(result))
                    return result;
            }

            var exceptionKey = $"{CommonUtils.GetRuntimeObjectKeyAndType(ex)}_{ex.Message}";
            return exceptionKey;
        }

        /// <summary>
        /// Rethrows the specified exception with the correct call stack information
        /// if the condition is true.
        /// </summary>
        /// <param name="e">The exception to rethrow.</param>
        /// <param name="condition">The condition to check.</param>
        public static void RethrowIf(Exception? e, bool condition)
        {
            if (condition)
                Rethrow(e);
        }

        /// <summary>
        /// Rethrows the specified exception with the correct call stack information.
        /// </summary>
        /// <param name="e">Previously thrown exception.</param>
        public static void Rethrow(Exception? e)
        {
            if (e is not null)
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
        /// Retrieves the associated ExceptionInfo for the specified exception instance.
        /// </summary>
        /// <remarks>If the exception is a BaseException, the ExceptionInfo is retrieved from its custom
        /// attributes. For other exception types, ExceptionInfo is managed in an internal cache. This method ensures
        /// that each exception instance is associated with a unique ExceptionInfo object.</remarks>
        /// <param name="ex">The exception for which to obtain exception information. Cannot be null.</param>
        /// <returns>An ExceptionInfo object associated with the specified exception. If no information exists, a new
        /// ExceptionInfo is created and returned.</returns>
        public static ExceptionInfo GetExceptionInfo(Exception ex)
        {
            if (ex is BaseException baseException)
            {
                var result = baseException.CustomAttr.GetAttributeOrAdd(KnownObjectAttributes.ExceptionInfo, () => new ExceptionInfo());
                return result;
            }
            else
            {
                var hashCode = GetExceptionKeyAsString(ex).GetHashCode();

                var result = exceptions.GetOrCreate(hashCode, () =>
                {
                    return new ExceptionInfo();
                });

                return result;
            }
        }

        /// <summary>
        /// Marks the specified exception as having been thrown during the application's main execution run.
        /// </summary>
        /// <param name="ex">The exception to mark as thrown in the application run. Cannot be null.</param>
        /// <returns>True if the exception was already marked as thrown in the application run before this call; otherwise, false.</returns>
        public static bool SetThrownInApplicationRun(Exception ex)
        {
            bool oldValue = false;

            try
            {
                var info = GetExceptionInfo(ex);
                oldValue = info.ThrownInApplicationRun;
                info.SetThrownInApplicationRun();
                return oldValue;
            }
            catch
            {
                return oldValue;
            }
        }

        /// <summary>
        /// Marks the specified exception as already logged and returns whether it was previously marked as such.
        /// </summary>
        /// <remarks>This method is typically used to prevent duplicate logging of the same exception
        /// instance. If the exception cannot be marked due to an error, the method returns false.</remarks>
        /// <param name="ex">The exception to mark as already logged. Cannot be null.</param>
        /// <returns>true if the exception was already marked as logged before this call; otherwise, false.</returns>
        public static bool SetAlreadyLogged(Exception ex)
        {
            bool oldValue = false;
            try
            {
                var info = GetExceptionInfo(ex);
                oldValue = info.AlreadyLogged;
                info.SetAlreadyLogged();
                return oldValue;
            }
            catch
            {
                return oldValue;
            }
        }

        /// <summary>
        /// Marks the specified exception as having been shown in a dialog and returns whether it was previously marked
        /// as shown.
        /// </summary>
        /// <param name="ex">The exception to mark as shown in a dialog. Cannot be null.</param>
        /// <returns>true if the exception was previously marked as shown in a dialog; otherwise, false.</returns>
        public static bool SetShownInDialog(Exception ex)
        {
            bool oldValue = false;

            try
            {
                var info = GetExceptionInfo(ex);
                oldValue = info.ShownInDialog;
                info.SetShownInDialog();
                return oldValue;
            }
            catch
            {
                return oldValue;
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

        /// <summary>
        /// Represents additional information about an exception.
        /// </summary>
        public class ExceptionInfo : BaseObjectWithAttr
        {
            /// <summary>
            /// Indicates whether the exception was thrown inside the <see cref="App.Run(Window)"/> method.
            /// </summary>
            public bool ThrownInApplicationRun { get; private set; }

            /// <summary>
            /// Indicates whether the exception was shown in the default error dialog.
            /// </summary>
            public bool ShownInDialog { get; private set; }

            /// <summary>
            /// Indicates whether the exception information has been logged. This can be used to prevent duplicate logging of the same exception.
            /// </summary>
            public bool AlreadyLogged { get; private set; }

            /// <summary>
            /// Marks that an exception was thrown during the application's run phase.
            /// </summary>
            public void SetThrownInApplicationRun()
            {
                ThrownInApplicationRun = true;
            }

            /// <summary>
            /// Marks the exception information as having been logged. This can be used to prevent duplicate logging of the same exception.
            /// </summary>
            public void SetAlreadyLogged()
            {
                AlreadyLogged = true;
            }

            /// <summary>
            /// Marks the item as having been shown in a dialog.
            /// </summary>
            public void SetShownInDialog()
            {
                ShownInDialog = true;
            }
        }
    }
}