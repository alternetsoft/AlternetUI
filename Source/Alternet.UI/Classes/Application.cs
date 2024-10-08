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
    public partial class Application : App, IDisposable
    {
        /// <summary>
        /// Gets whether 'NETCOREAPP' is defined.
        /// </summary>
        public static readonly bool IsNetCoreApp;

        static Application()
        {
#if NETCOREAPP
            IsNetCoreApp = true;
#else
            IsNetCoreApp = false;
#endif

            if (!App.Is64BitProcess && App.IsWindowsOS)
            {
                if (!IsNetCoreApp)
                {
                    var s = $"Critical error\n\n";
                    s += $"Application: [{CommonUtils.GetAppExePath()}]\n\n";
                    s += $"Your software configuration is not supported:\n";
                    s += $"Windows 32 bit and Net Framework {Environment.Version}\n\n";
                    s += $"Use Windows 64 bit or newer Net Framework version.\n";

                    DialogFactory.ShowCriticalMessage(s);
                }
            }

            Handler = new WxApplicationHandler();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application(IApplicationHandler? handler = null)
            : base(handler ?? Handler ?? new WxApplicationHandler())
        {
        }

        /// <summary>
        /// Creates application and main form, runs and disposes them.
        /// </summary>
        /// <param name="createFunc">Function which creates main form.</param>
        /// <param name="runAction">Runs action after main form is created.</param>
        /// <exception cref="Exception">If application is already created.</exception>
        public static void CreateAndRun(Func<Window> createFunc, Action? runAction = null)
        {
            if (Initialized)
                throw new Exception("The application has already been created.");

            var application = new Application();
            var window = createFunc();

            void Task(object? userData)
            {
                runAction();
            }

            if (runAction is not null)
                AddIdleTask(Task);

            application.Run(window);

            window.Dispose();
            application.Dispose();
        }
   }
}