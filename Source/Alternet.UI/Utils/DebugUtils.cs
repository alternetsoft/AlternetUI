﻿using System;
using System.Collections.Generic;
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
        private static WindowDeveloperTools? devToolsWindow;

        /// <summary>
        /// Hooks exception events for the debug purposes.
        /// </summary>
        public static void HookExceptionEvents()
        {
            if (hookedExceptionEvents)
                return;
            var a = Application.Current;
            Application.ThreadException += Application_ThreadException;
            a.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
                CurrentDomain_UnhandledException;
            hookedExceptionEvents = true;
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            if(devToolsWindow is null)
            {
                devToolsWindow = new();
                devToolsWindow.Closing += DevToolsWindow_Closing;
                devToolsWindow.Disposed += DevToolsWindow_Disposed;
            }

            devToolsWindow.Show();

            static void DevToolsWindow_Closing(object? sender, WindowClosingEventArgs e)
            {
            }

            static void DevToolsWindow_Disposed(object? sender, EventArgs e)
            {
                devToolsWindow = null;
            }
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
