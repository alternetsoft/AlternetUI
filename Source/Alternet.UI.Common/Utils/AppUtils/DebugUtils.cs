﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to the debug process.
    /// </summary>
    public static class DebugUtils
    {
        /// <summary>
        /// Gets a value that indicates whether DEBUG conditional is defined.
        /// </summary>
        public static readonly bool IsDebugDefined;

        /// <summary>
        /// Gets or sets whether to log application loading process.
        /// </summary>
        public static bool DebugLoading = false;

        /// <summary>
        /// Gets or sets whether exception logger (registered with
        /// <see cref="RegisterExceptionsLogger"/>)
        /// outputs messages to <see cref="Debug.WriteLine(string)"/>.
        /// </summary>
        public static bool ExceptionsLoggerDebugWriteLine = true;

        /// <summary>
        /// Gets or sets whether exception logger (registered with
        /// <see cref="RegisterExceptionsLogger"/>)
        /// outputs messages to <see cref="App.Log"/>.
        /// </summary>
        public static bool ExceptionsLoggerAppLog = false;

        /// <summary>
        /// Gets or sets whether to use 'dlOpen' on Linux in order to load native dll.
        /// </summary>
        public static bool UseDlOpenOnLinux = false;

        private static readonly IndexedValues<Type, ExceptionItem> exceptionRegister = new();

        private static bool insideUnhandledException;
        private static bool hookedExceptionEvents;

        static DebugUtils()
        {
#if DEBUG
            IsDebugDefined = true;
#else
            IsDebugDefined = false;
#endif

            DebugLoading = DebugUtils.IsDebugDefined && false;

            ExceptionLoggerSetIgnore<ThreadInterruptedException>();
            ExceptionLoggerSetIgnore<ThreadAbortException>();
            ExceptionLoggerSetIgnore<OperationCanceledException>();
        }

        /// <summary>
        /// Alias for <see cref="Debugger.IsAttached"/>.
        /// </summary>
        public static bool IsDebuggerAttached => Debugger.IsAttached;

        /// <summary>
        /// Gets whether "DEBUG" is defined and debugger is attached.
        /// </summary>
        public static bool IsDebugDefinedAndAttached => IsDebugDefined && IsDebuggerAttached;

        /// <summary>
        /// Sets whether exceptions of the specified type are ignored in the exception logger.
        /// </summary>
        /// <typeparam name="T">Type of the exception to ignore.</typeparam>
        /// <param name="value">Whether to ignore the exceptions of the specified type.</param>
        public static void ExceptionLoggerSetIgnore<T>(bool value = true)
        {
            var item = exceptionRegister.GetValue(typeof(T), () => new ExceptionItem());
            if (item is null)
                return;
            item.IgnoredInLogger = value;
        }

        /// <summary>
        /// Gets whether exceptions of the specified type are ignored in the exception logger.
        /// </summary>
        /// <param name="t">Type of the exception.</param>
        /// <returns>True if exception is ignored in the exception logger;
        /// False if it is processed.</returns>
        public static bool ExceptionLoggerIgnored(Type t)
        {
            var item = exceptionRegister.GetValue(t);
            var result = item?.IgnoredInLogger ?? false;
            return result;
        }

        /// <summary>
        /// Calls the specified action if condition and <see cref="IsDebugDefined"/> are <c>true</c>.
        /// </summary>
        /// <param name="condition">Condition to check.</param>
        /// <param name="actionToCall">Action to call.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        public static void DebugCallIf(bool condition, Action actionToCall)
        {
            if (condition)
                actionToCall();
        }

        /// <summary>
        /// Calls the specified action <see cref="IsDebugDefined"/> is <c>true</c>.
        /// </summary>
        /// <param name="actionToCall">Action to call.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Conditional("DEBUG")]
        public static void DebugCall(Action actionToCall)
        {
            actionToCall();
        }

        /// <summary>
        /// Calls <see cref="RegisterExceptionsLogger"/> if DEBUG conditional is defined.
        /// </summary>
        [Conditional("DEBUG")]
        public static void RegisterExceptionsLoggerIfDebug(Action<Exception>? callback = null)
        {
            RegisterExceptionsLogger(callback);
        }

        /// <summary>
        /// Subscribes to events related to the exception handling and logs exceptions.
        /// </summary>
        /// <seealso cref="ExceptionsLoggerDebugWriteLine"/>.
        /// <seealso cref="ExceptionsLoggerAppLog"/>.
        public static void RegisterExceptionsLogger(Action<Exception>? callback = null)
        {
            if (hookedExceptionEvents)
                return;
            hookedExceptionEvents = true;

            void LogException(string title, object e)
            {
                if (insideUnhandledException)
                    return;

                insideUnhandledException = true;

                try
                {
                    if (e is not Exception exception)
                        exception = new(e.ToString());

                    if (ExceptionLoggerIgnored(exception.GetType()))
                        return;

                    if (ExceptionsLoggerDebugWriteLine)
                        LogExceptionToAction(title, e, (s) => Debug.WriteLine(s));
                    if (ExceptionsLoggerAppLog)
                        LogExceptionToAction(title, e, (s) => App.Log(s));
                    callback?.Invoke(exception);
                }
                finally
                {
                    insideUnhandledException = false;
                }
            }

            static void LogExceptionToAction(string title, object e, Action<string> writeLine)
            {
                try
                {
                    writeLine(LogUtils.SectionSeparator);
                    writeLine(title);
                    var s = e.ToString();
                    writeLine(s);
                    writeLine(LogUtils.SectionSeparator);
                }
                catch
                {
                }
            }

            App.ThreadException += (s, e) =>
            {
                LogException("Application.ThreadException", e.Exception);
            };

            AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
            {
                LogException("First Chance Exception", e.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogException("CurrentDomain Unhandled exception", e.ExceptionObject);
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogException("Unobserved Task Exception", e.Exception);
            };
        }

        /// <summary>
        /// Waits until debugger is attached. Uses <paramref name="debugOptionFileName"/>
        /// file existance in order to get "wait" setting on/off.
        /// </summary>
        /// <param name="debugOptionFileName">Path to file name which existance specifies
        /// whether wait for debugger is on. Optional. When not specified, file
        /// "e:\debugserver.on" is used.</param>
        public static void WaitDebugger(string? debugOptionFileName = null)
        {
            try
            {
                bool waitDebug = File.Exists(debugOptionFileName ?? @"e:\debugserver.on");
                if (waitDebug)
                {
                    while (!Debugger.IsAttached)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Same as <see cref="RegisterExceptionsLogger"/>.
        /// </summary>
        public static void HookExceptionEvents()
        {
            RegisterExceptionsLogger();
        }

        /// <summary>
        /// <see cref="TraceListener"/> descendant for the debug purposes.
        /// </summary>
        public class DebugTraceListener : TraceListener
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DebugTraceListener"/> class.
            /// </summary>
            public DebugTraceListener()
            {
            }

            /// <inheritdoc/>
            public override void Write(string message)
            {
                CommonUtils.Nop();
            }

            /// <inheritdoc/>
            public override void WriteLine(string message)
            {
                CommonUtils.Nop();
            }
        }

        private class ExceptionItem
        {
            public bool IgnoredInLogger;
        }
    }
}
