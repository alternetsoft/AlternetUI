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
        /// Gets or sets whether to use 'dlOpen' on Linux in order to load native dll.
        /// </summary>
        public static bool UseDlOpenOnLinux = false;

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
        /// Subscribes to <see cref="AppDomain"/> and <see cref="TaskScheduler"/> events
        /// related to the exceptions handling.
        /// </summary>
        public static void RegisterExceptionsLogger()
        {
            static void LogException(string title, object e)
            {
                Debug.WriteLine(LogUtils.SectionSeparator);
                Debug.WriteLine(title);
                var s = e.ToString();
                Debug.WriteLine(s);
                Debug.WriteLine(LogUtils.SectionSeparator);
            }

            AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
            {
                DebugCall(() =>
                {
                    LogException("First Chance Exception", e.Exception);
                });
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                DebugCall(() =>
                {
                    LogException("CurrentDomain Unhandled exception", e.ExceptionObject);
                });
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                DebugCall(() =>
                {
                    LogException("Unobserved Task Exception", e.Exception);
                });
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
        /// Hooks exception events for the debug purposes.
        /// </summary>
        public static void HookExceptionEvents()
        {
            if (hookedExceptionEvents)
                return;
            var a = App.Current;
            App.ThreadException += Application_ThreadException;
            a.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
                CurrentDomain_UnhandledException;
            hookedExceptionEvents = true;
        }

        private static void HandleException(Exception e, string info)
        {
            LogUtils.LogException(e, info);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception, "Application.ThreadException");
        }

        private static void CurrentDomain_UnhandledException(
            object? sender,
            UnhandledExceptionEventArgs e)
        {
            if (!insideUnhandledException)
            {
                insideUnhandledException = true;
                try
                {
                    HandleException((e.ExceptionObject as Exception)!, "CurrentDomain.UnhandledException");
                }
                finally
                {
                    insideUnhandledException = false;
                }
            }
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
    }
}
