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
            if (ForceX11OnLinux && App.IsLinuxOS)
            {
                Environment.SetEnvironmentVariable("GDK_BACKEND", "x11,*");
            }
        }
    }
}