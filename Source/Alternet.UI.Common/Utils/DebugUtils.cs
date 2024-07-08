using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private static bool insideUnhandledException;
        private static bool hookedExceptionEvents;

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
    }
}
