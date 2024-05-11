using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Logs environment versions.
        /// </summary>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogVersion()
        {
            if (!LogUtils.ShowDebugWelcomeMessage)
                return;
            if (LogUtils.Flags.HasFlag(LogUtils.LogFlags.VersionLogged))
                return;
            LogUtils.Flags |= LogUtils.LogFlags.VersionLogged;
            var wxWidgets = WebBrowser.GetLibraryVersionString();
            var bitsOS = BaseApplication.Is64BitOS ? "x64" : "x86";
            var bitsApp = BaseApplication.Is64BitProcess ? "x64" : "x86";
            var net = $"Net: {Environment.Version}, OS: {bitsOS}, App: {bitsApp}";
            var dpi = $"DPI: {Application.FirstWindow()?.GetDPI().Width}";
            var ui = $"UI: {WebBrowser.DoCommandGlobal("UIVersion")}";
            var counterStr = $"Counter: {Application.BuildCounter}";
            var s = $"{ui}, {net}, {wxWidgets}, {dpi}, {counterStr}";
            BaseApplication.Log(s);
            if (BaseApplication.LogFileIsEnabled)
                BaseApplication.DebugLog($"Log File = {BaseApplication.LogFilePath}");
        }

        /// <summary>
        /// Logs <see cref="SystemSettings"/>.
        /// </summary>
        public static void LogSystemSettings()
        {
            BaseApplication.LogBeginSection();
            BaseApplication.Log($"IsDark = {SystemSettings.AppearanceIsDark}");
            BaseApplication.Log($"IsUsingDarkBackground = {SystemSettings.IsUsingDarkBackground}");
            BaseApplication.Log($"AppearanceName = {SystemSettings.AppearanceName}");

            var defaultColors = Control.GetStaticDefaultFontAndColor(ControlTypeId.TextBox);
            LogUtils.LogColor("TextBox.ForegroundColor (defaults)", defaultColors.ForegroundColor);
            LogUtils.LogColor("TextBox.BackgroundColor (defaults)", defaultColors.BackgroundColor);

            BaseApplication.Log($"CPP.SizeOfLong = {WebBrowser.DoCommandGlobal("SizeOfLong")}");
            BaseApplication.Log($"CPP.IsDebug = {WebBrowser.DoCommandGlobal("IsDebug")}");

            BaseApplication.LogSeparator();

            foreach (SystemSettingsFeature item in Enum.GetValues(typeof(SystemSettingsFeature)))
            {
                BaseApplication.Log($"HasFeature({item}) = {SystemSettings.HasFeature(item)}");
            }

            BaseApplication.LogSeparator();

            foreach (SystemSettingsMetric item in Enum.GetValues(typeof(SystemSettingsMetric)))
            {
                BaseApplication.Log($"GetMetric({item}) = {SystemSettings.GetMetric(item)}");
            }

            BaseApplication.LogSeparator();

            foreach (SystemSettingsFont item in Enum.GetValues(typeof(SystemSettingsFont)))
            {
                BaseApplication.Log($"GetFont({item}) = {SystemSettings.GetFont(item)}");
            }

            BaseApplication.LogEndSection();
        }

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
