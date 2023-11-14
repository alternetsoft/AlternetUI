using System;
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
            a.ThreadException += Application_ThreadException;
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
                (sender as Window)?.Hide();
                e.Cancel = true;
            }

            void DevToolsWindow_Disposed(object? sender, EventArgs e)
            {
            }
        }

        private static void HandleException(Exception e)
        {
            LogUtils.LogException(e);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException(e.Exception);
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
                    HandleException((e.ExceptionObject as Exception)!);
                }
                finally
                {
                    insideUnhandledException = false;
                }
            }
        }
    }
}
