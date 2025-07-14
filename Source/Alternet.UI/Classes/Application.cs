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
                if (!App.Is64BitProcess && App.IsWindowsOS)
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

                string appDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? "");

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
    }
}