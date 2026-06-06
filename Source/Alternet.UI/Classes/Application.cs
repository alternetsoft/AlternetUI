using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties to manage an application.
    /// </summary>
    public partial class Application : App
    {
        static Application()
        {

            if (!App.IsMaui)
            {
                if (App.IsWindowsOS)
                {
                    try
                    {
                        InitDpiAwareness();
                    }
                    catch
                    {
                    }

                    if (!App.Is64BitProcess)
                    {
                        if (!IsNetOrCoreApp)
                        {
                            var s = $"Critical error\n\n";
                            s += $"Application: [{CommonUtils.GetAppExePath()}]\n\n";
                            s += $"Your software configuration is not supported:\n";
                            s += $"Windows 32 bit and Net Framework {Environment.Version}\n\n";
                            s += $"Use Windows 64 bit or newer Net Framework version.\n";

                            DialogFactory.ShowCriticalMessage(s);
                        }
                    }
                }
            }

            if (App.IsLinuxOS)
            {
                if (WxGlobalSettings.Linux.InjectGtkCss)
                {
                    if (WxGlobalSettings.Linux.LoadGtkCss)
                    {
                        WxGlobalSettings.Linux.GtkCss = LoadOptionalGtkCss();
                    }

                    var s = WxGlobalSettings.Linux.GtkCss;
                    if (s != null)
                    {
                        Native.Application.SetGtkCss(true, s);
                    }
                }
            }

            Handler = CreateDefaultHandler();

            DebugUtils.DebugCall(() =>
            {
                WebBrowser.CrtSetDbgFlag(0);
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class
        /// with the specified handler.
        /// </summary>
        public Application(IApplicationHandler? handler)
            : base(handler ?? Handler ?? CreateDefaultHandler())
        {
        }

        /// <summary>
        /// Shows critical warning message.
        /// </summary>
        /// <param name="warning">The warning message to display.</param>
        public static void ShowCriticalWarning(string warning)
        {
            try
            {
                var s = $"Warning\n\n";
                s += $"Application: [{CommonUtils.GetAppExePath()}]\n\n";
                s += $"Warning: {warning}\n\n";
                DialogFactory.ShowCriticalMessage(s, null, true);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Attempts to load a GTK CSS file named '&lt;ApplicationFileName&gt;.gtk.css' from
        /// the application directory.
        /// Returns the contents as a string, or <c>null</c> if the file doesn't exist or can't be read.
        /// </summary>
        public static string? LoadOptionalGtkCss()
        {
            try
            {
                string appFileName = Path.GetFileNameWithoutExtension(
                    Assembly.GetEntryAssembly()?.Location ?? "App");

                string cssFileName = appFileName + ".gtk.css";

                var appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? "");

                if (string.IsNullOrEmpty(appDirectory))
                    return null;

                string fullPath = Path.Combine(appDirectory, cssFileName);

                if (File.Exists(fullPath))
                    return File.ReadAllText(fullPath);

                return null;
            }
            catch (Exception ex)
            {
                BaseObject.Nop(new Exception($"Failed to load GTK CSS: {ex.Message}"));
                return null;
            }
        }

        /// <summary>
        /// Initializes dpi awareness for the application. This is called automatically.
        /// </summary>
        private static void InitDpiAwareness()
        {
            bool setProcessDpiAware = false;

            if (MswUtils.IsWindows81OrLater())
            {
                IntPtr hProcess = System.Diagnostics.Process.GetCurrentProcess().Handle;

                int v = MswUtils.NativeMethods.GetProcessDpiAwareness(hProcess, out MswProcessDpiAwareness awareness);
                if (v == 0)
                {
                    switch (awareness)
                    {
                        case MswProcessDpiAwareness.Unaware:
                            setProcessDpiAware = true;
                            break;
                        case MswProcessDpiAwareness.SystemAware:
                            break;
                        case MswProcessDpiAwareness.PerMonitorAware:
                            if (DebugUtils.IsDebugDefined)
                            {
                                ShowCriticalWarning(
"""
Per-monitor DPI awareness is not supported.
Please edit app.manifest and set the DPI awareness to 'SystemAware'. Example:

  <application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    </windowsSettings>
  </application>
  <application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    </windowsSettings>
  </application>

"""
    );
                            }
                            break;
                    }
                }
                else
                {
                    setProcessDpiAware = true;
                }
            }

            if (setProcessDpiAware && MswUtils.IsWindowsVistaOrLater())
            {
                var result = MswUtils.NativeMethods.SetProcessDPIAware();

                if (result)
                {
                }
                else
                {
                }
            }
        }
    }
}