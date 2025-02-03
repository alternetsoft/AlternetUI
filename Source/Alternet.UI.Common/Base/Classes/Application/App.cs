using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties to manage an application.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class App : DisposableObject
    {
        /// <summary>
        /// Gets id of the application thread.
        /// </summary>
        public static readonly int AppThreadId = Thread.CurrentThread.ManagedThreadId;

        /// <summary>
        /// Returns true if operating system is Windows.
        /// </summary>
        public static readonly bool IsWindowsOS;

        /// <summary>
        /// Returns true if operating system is Linux.
        /// </summary>
        public static readonly bool IsLinuxOS;

        /// <summary>
        /// Returns true if operating system is Apple macOS.
        /// </summary>
        public static readonly bool IsMacOS;

        /// <summary>
        /// Gets a value that indicates whether the current operating system is
        /// a 64-bit operating system.
        /// </summary>
        public static readonly bool Is64BitOS;

        /// <summary>
        /// Gets a value that indicates whether the current process is a 64-bit process.
        /// </summary>
        public static readonly bool Is64BitProcess;

        /// <summary>
        /// Gets operating system as <see cref="OperatingSystems"/> enumeration.
        /// </summary>
        public static readonly OperatingSystems BackendOS;

        /// <summary>
        /// Indicates whether the current application is running on Android.
        /// </summary>
        public static readonly bool IsAndroidOS;

        /// <summary>
        /// Indicates whether the current application is running on unknown OS.
        /// </summary>
        public static readonly bool IsUnknownOS;

        /// <summary>
        /// Indicates whether the current application is running on Apple iOS.
        /// </summary>
        public static readonly bool IsIOS;

        /// <summary>
        /// Gets device the app is running on, such as a desktop computer or a tablet.
        /// </summary>
        public static readonly GenericDeviceType DeviceType;

        /// <summary>
        /// Gets or sets application exit code used when application terminates
        /// due to unhandled exception.
        /// </summary>
        public static int ThreadExceptionExitCode = 0;

        /// <summary>
        /// Gets or sets whether to log unhandled thread exception.
        /// </summary>
        public static bool LogUnhandledThreadException = true;

        /// <summary>
        /// Gets or sets whether calls to and from native code are wrapped in "try catch".
        /// </summary>
        /// <remarks>
        /// Under Windows default value is <c>true</c> and such wrapping is not needed.
        /// Under other systems default value is <c>false</c> and all calls are wrapped.
        /// </remarks>
        public static bool FastThreadExceptions;

        /// <summary>
        /// Gets application handler.
        /// </summary>
        public static IApplicationHandler Handler = null!;

        /// <summary>
        /// Gets invariant <see cref="CultureInfo"/>.
        /// </summary>
        public static CultureInfo InvariantEnglishUS = CultureInfo.InvariantCulture;

        internal static readonly Destructor MyDestructor = new();

        private static readonly
            ConcurrentQueue<(Action<object?> Action, object? Data)> IdleTasks = new();

        private static readonly ConcurrentQueue<LogUtils.LogItem> LogQueue = new();

        private static bool? isMono;

        private static UnhandledExceptionMode unhandledExceptionMode
            = UnhandledExceptionMode.CatchWithDialog;

        private static UnhandledExceptionMode unhandledExceptionModeDebug
            = UnhandledExceptionMode.CatchWithDialogAndThrow;

        private static bool? isMaui;
        private static bool inOnThreadException;
        private static IconSet? icon;
        private static bool terminating = false;
        private static int logUpdateCount;
        private static bool logFileIsEnabled;
        private static App? current;
        private static string? logFilePath;
        private static Window? mainWindow;
        private static bool wakeUpIdleWithTimer = true;
        private static Timer? wakeUpIdleTimer;
        private static bool? isNetOrCoreApp;

        private readonly List<Window> windows = new();

        static App()
        {
            Is64BitOS = Environment.Is64BitOperatingSystem;
            Is64BitProcess = Environment.Is64BitProcess;

            DeviceType = AssemblyUtils.InvokeMauiUtilsGetDeviceType();

            IsWindowsOS = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (IsWindowsOS)
            {
                if (!DebugUtils.IsDebugDefined)
                {
                    FastThreadExceptions = true;
                }

                BackendOS = OperatingSystems.Windows;
                return;
            }

            IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (IsMacOS)
            {
                BackendOS = OperatingSystems.MacOs;
                return;
            }

            IsLinuxOS = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (IsLinuxOS)
            {
                BackendOS = OperatingSystems.Linux;
                return;
            }

            if (AssemblyUtils.InvokeIsAndroid() == true)
            {
                App.IsAndroidOS = true;
                App.IsUnknownOS = false;
                App.BackendOS = OperatingSystems.Android;
                return;
            }

            if (AssemblyUtils.InvokeIsIOS() == true)
            {
                var isCatalyst = AssemblyUtils.InvokeMauiUtilsIsMacCatalyst();

                if (isCatalyst == true)
                {
                    App.IsMacOS = true;
                    BackendOS = OperatingSystems.MacOs;
                }
                else
                {
                    App.IsIOS = true;
                    App.BackendOS = OperatingSystems.IOS;
                }

                App.IsUnknownOS = false;
                return;
            }

            BackendOS = OperatingSystems.Unknown;
            IsUnknownOS = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="handler">Application handler.</param>
        public App(IApplicationHandler? handler = null)
        {
            if (handler is not null)
                Handler = handler;
            Handler ??= CreateDefaultHandler();
            SynchronizationContext.InstallIfNeeded();
            App.Current = this;

            Initialized = true;
            Window.UpdateDefaultFont();

            DebugUtils.DebugCall(() =>
            {
                WebBrowser.CrtSetDbgFlag(0);
            });

            UpdateWakeUpIdleTimer();
        }

        /// <summary>
        /// Occurs before native debug message needs to be displayed.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs>? BeforeNativeLogMessage;

        /// <summary>
        /// Occurs when an untrapped thread exception is thrown.
        /// </summary>
        /// <remarks>
        /// This event allows your application to handle otherwise
        /// unhandled exceptions that occur in UI threads. Attach
        /// your event handler to the <see cref="ThreadException"/> event
        /// to deal with these exceptions, which will
        /// leave your application in an unknown state. Where possible,
        /// exceptions should be handled by a structured
        /// exception handling block. You can change whether this callback
        /// is used for unhandled thread
        /// exceptions by setting <see cref="App.SetUnhandledExceptionMode"/>
        /// and <see cref="App.SetUnhandledExceptionModeIfDebugger"/>.
        /// </remarks>
        public static event BaseThreadExceptionEventHandler? ThreadException;

        /// <summary>
        /// Occurs when controls which display log messages need to be refreshed.
        /// </summary>
        public static event EventHandler? LogRefresh;

        /// <summary>
        /// Occurs when debug message needs to be displayed.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs>? LogMessage;

        /// <summary>
        /// Occurs when the application finishes processing events and is
        /// about to enter the idle state.
        /// </summary>
        public static event EventHandler? Idle;

        /// <summary>
        /// Gets whether application is running on the desktop operating system.
        /// </summary>
        public static bool IsDesktopOs => IsWindowsOS || IsMacOS || IsLinuxOS;

        /// <summary>
        /// Gets whether device the app is running on is desktop.
        /// </summary>
        public static bool IsDesktopDevice => DeviceType == GenericDeviceType.Desktop;

        /// <summary>
        /// Gets whether the process architecture of the currently running app is Arm.
        /// </summary>
        public static bool IsArmProcess
        {
            get
            {
                var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
                return arch == Architecture.Arm || arch == Architecture.Arm64;
            }
        }

        /// <summary>
        /// Gets whether the operating system architecture is Arm.
        /// </summary>
        public static bool IsArmOS
        {
            get
            {
                var arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture;
                return arch == Architecture.Arm || arch == Architecture.Arm64;
            }
        }

        /// <summary>
        /// Gets whether device the app is running on is tablet.
        /// </summary>
        public static bool IsTabletDevice => DeviceType == GenericDeviceType.Tablet;

        /// <summary>
        /// Gets whether device the app is running on is phone.
        /// </summary>
        public static bool IsPhoneDevice => DeviceType == GenericDeviceType.Phone;

        /// <summary>
        /// Gets whether current thread is the thread which was used to initialize the application.
        /// It is better to use platform specific ways to get whether current thread is UI thread.
        /// </summary>
        public static bool IsAppThread => Thread.CurrentThread.ManagedThreadId == AppThreadId;

        /// <summary>
        /// Gets top-most modal dialog or Null if no modal dialogs are shown.
        /// </summary>
        public static Window? TopModalDialog
        {
            get
            {
                return ModalDialogs?.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets collection of the visible modal dialogs.
        /// </summary>
        public static IEnumerable<Window> ModalDialogs
        {
            get
            {
                if (current is null)
                    yield break;

                var windows = Current.VisibleWindows.OrderByDescending(x => x.LastShownAsDialogTime);

                foreach (var window in windows)
                {
                    if (window.Modal)
                        yield return window;
                }
            }
        }

        /// <summary>
        /// Gets last unhandled exception.
        /// </summary>
        public static Exception? LastUnhandledException
        {
            get => lastUnhandledException;

            internal set => lastUnhandledException = value;
        }

        /// <summary>
        /// Gets whether last unhandled exception was thrown.
        /// </summary>
        public static bool LastUnhandledExceptionThrown
        {
            get => lastUnhandledExceptionThrown;

            internal set => lastUnhandledExceptionThrown = value;
        }

        /// <summary>
        /// Gets whether <see cref="LogMessage"/> event has any handlers.
        /// </summary>
        public static bool HasLogMessageHandler => LogMessage is not null;

        /// <summary>
        /// Gets the path for the executable file that started the application, not including
        /// the executable name.</summary>
        /// <returns>
        /// The path for the executable file that started the application.
        /// </returns>
        public static string StartupPath
        {
            get
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string s = Path.GetDirectoryName(location)!;
                return s;
            }
        }

        /// <summary>
        /// Gets or sets whether to call <see cref="Debug.WriteLine(string)"/> when
        /// <see cref="App.Log"/> is called. Default is <c>false</c>.
        /// </summary>
        public static LogItemKindFlags DebugWriteLine { get; set; } = LogItemKindFlags.Error;

        /// <summary>
        /// Gets whether <see cref="ThreadException"/> event is assigned.
        /// </summary>
        public static bool ThreadExceptionAssigned => ThreadException is not null;

        /// <summary>
        /// Gets currently used platform.
        /// </summary>
        public static UIPlatformKind PlatformKind => SystemSettings.Handler.GetPlatformKind();

        /// <summary>
        /// Returns True if application runs on Net or NetCore platform (Net 5 or higher).
        /// Returns False if application runs on Net Framework.
        /// </summary>
        public static bool IsNetOrCoreApp
        {
            get
            {
                isNetOrCoreApp ??=
                    AppUtils.FrameworkIdentifier == NetFrameworkIdentifier.Net ||
                    AppUtils.FrameworkIdentifier == NetFrameworkIdentifier.NetCore;
                return isNetOrCoreApp.Value;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Handler"/> is initialized.
        /// </summary>
        public static bool HasHandler
        {
            get
            {
                return Handler is not null;
            }
        }

        /// <summary>
        /// Gets the <see cref="App"/> object for the currently
        /// runnning application.
        /// </summary>
        public static App Current
        {
            get
            {
                return current ??= new App();
            }

            protected set
            {
                current = value;
            }
        }

        /// <summary>
        /// Indicates whether the current application is running on Mono runtime.
        /// </summary>
        public static bool IsMono => isMono ??= Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Gets how the application responds to unhandled exceptions.
        /// Use <see cref="SetUnhandledExceptionMode"/> to change this property.
        /// </summary>
        public static UnhandledExceptionMode UnhandledExceptionMode => unhandledExceptionMode;

        /// <summary>
        /// Gets how the application responds to unhandled exceptions (if debugger is attached).
        /// Use <see cref="SetUnhandledExceptionModeIfDebugger"/> to change this property.
        /// </summary>
        public static UnhandledExceptionMode UnhandledExceptionModeIfDebugger
            => unhandledExceptionModeDebug;

        /// <summary>
        /// Gets a value that indicates whether a debugger is attached to the process.
        /// </summary>
        /// <value>
        /// true if a debugger is attached; otherwise, false.
        /// </value>
        public static bool IsDebuggerAttached
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Run"/> method execution is finished.
        /// </summary>
        public static bool Terminating
        {
            get => terminating;
            protected set => terminating = value;
        }

        /// <summary>
        /// Gets or sets application log file path.
        /// </summary>
        /// <remarks>
        /// Default value is exe file path and "Alternet.UI.log" file name.
        /// </remarks>
        public static string LogFilePath
        {
            get
            {
                if (logFilePath is null)
                {
                    string location;

                    if (App.IsWindowsOS && File.Exists(@"e:\logToFile.on"))
                        location = @"e:\alternet-ui";
                    else
                        location = Assembly.GetExecutingAssembly().Location;
                    logFilePath = Path.ChangeExtension(location, ".log");
                }

                return logFilePath;
            }

            set
            {
                logFilePath = value;
            }
        }

        /// <summary>
        /// Gets or sets root tooltip provider.
        /// </summary>
        [Browsable(false)]
        public static IToolTipProvider? ToolTipProvider { get; set; }

        /// <summary>
        /// Gets or sets whether to write all log messages to file.
        /// </summary>
        /// <remarks>
        ///  Default value is <c>false</c>.
        /// </remarks>
        public static bool LogFileIsEnabled
        {
            get
            {
                return logFileIsEnabled;
            }

            set
            {
                if (logFileIsEnabled == value)
                    return;
                logFileIsEnabled = value;

                if (logFileIsEnabled)
                    LogUtils.LogAppStartedToFile();
            }
        }

        /// <summary>
        /// Gets whether application is executed on Maui platform.
        /// </summary>
        public static bool IsMaui => isMaui ??= KnownAssemblies.LibraryMaui.Value is not null;

        /// <summary>
        /// Uses timer to call <see cref="WakeUpIdle"/> periodically.
        /// Default value is <c>true</c>. Default timeout value is
        /// <see cref="TimerUtils.DefaultWakeUpIdleTimeout"/>.
        /// </summary>
        public static bool WakeUpIdleWithTimer
        {
            get
            {
                return wakeUpIdleWithTimer;
            }

            set
            {
                wakeUpIdleWithTimer = value;
                UpdateWakeUpIdleTimer();
            }
        }

        /// <summary>
        /// Returns true if between two <see cref="BeginBusyCursor"/> and
        /// <see cref="EndBusyCursor"/> calls.
        /// </summary>
        public static bool IsBusyCursor
        {
            get
            {
                return Cursor.Factory.IsBusyCursor();
            }
        }

        /// <summary>
        /// Allows to suppress some debug messages.
        /// </summary>
        /// <remarks>
        ///  Currently used to suppress GTK messages under Linux. Default
        ///  value is true.
        /// </remarks>
        public static bool SupressDiagnostics { get; set; } = true;

        /// <summary>
        /// Gets whether application was created and <see cref="Current"/> property is assigned.
        /// </summary>
        public static bool HasApplication => current is not null;

        /// <summary>
        /// Gets whether application was initialized;
        /// </summary>
        public static bool Initialized
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets default icon for the application.
        /// </summary>
        /// <remarks>
        /// By default it returns icon of the the first <see cref="Window"/>.
        /// You can assing <see cref="IconSet"/> here to override default behavior.
        /// If you assing <c>null</c>, this property will again return icon of
        /// the the first <see cref="Window"/>. Change to this property doesn't
        /// update the icon of the the first <see cref="Window"/>.
        /// </remarks>
        public static IconSet? DefaultIcon
        {
            get
            {
                return icon ?? FirstWindow()?.Icon;
            }

            set
            {
                icon = value;
            }
        }

        /// <summary>
        /// Gets main window.
        /// </summary>
        public static Window? MainWindow
        {
            get => mainWindow;
            internal set => mainWindow = value;
        }

        /// <summary>
        /// Gets <see cref="Window.ActiveWindow"/> or <see cref="MainWindow"/> or
        /// <see cref="FirstWindow()"/>. The first not null value of these is returned.
        /// </summary>
        public static Window SafeWindow
        {
            get
            {
                return Window.Default;
            }
        }

        /// <summary>
        /// Gets whether execution is inside the <see cref="Run"/> method.
        /// </summary>
        public static bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets whether application has forms.
        /// </summary>
        public static bool HasForms => HasApplication && Current.Windows.Count > 0;

        /// <summary>
        /// Gets whether application has visible forms.
        /// </summary>
        public static bool HasVisibleForms => HasForms
            && Current.VisibleWindows.FirstOrDefault() != null;

        /// <summary>
        /// Gets the instantiated windows in the application.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{Window}"/> that contains
        /// references to all window objects in the application.</value>
        public IReadOnlyList<Window> Windows => windows;

        /// <summary>
        /// Gets all visible windows in the application.
        /// </summary>
        /// <value>A <see cref="IEnumerable{Window}"/> that contains
        /// references to all visible <see cref="Window"/> objects in the application.</value>
        public virtual IEnumerable<Window> VisibleWindows
        {
            get
            {
                var result = Windows.Where(x => x.Visible);
                return result;
            }
        }

        /// <summary>
        /// Gets all visible windows in the application ordered by the last activated time.
        /// Last activated window will be the first one in the result.
        /// </summary>
        /// <value>A <see cref="IEnumerable{Window}"/> that contains
        /// references to all visible <see cref="Window"/> objects in the application
        /// ordered by the last activated time.</value>
        public virtual IEnumerable<Window> LastActivatedWindows
        {
            get
            {
                var result = VisibleWindows.OrderByDescending(x => x.LastActivateTime);
                return result;
            }
        }

        /// <summary>
        /// Allows the programmer to specify whether the application will exit when the
        /// top-level frame is deleted.
        /// Returns true if the application will exit when the top-level frame is deleted.
        /// </summary>
        public virtual bool ExitOnFrameDelete
        {
            get => Handler.ExitOnFrameDelete;
            set => Handler.ExitOnFrameDelete = value;
        }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        public virtual bool IsActive => Handler.IsActive;

        /// <summary>
        /// Gets whether application in Uixml previewer mode.
        /// </summary>
        public virtual bool InUixmlPreviewerMode
        {
            get => Handler.InUixmlPreviewerMode;
            set => Handler.InUixmlPreviewerMode = value;
        }

        /// <summary>
        /// Gets or sets whether application will use the best visual on systems that
        /// support different visuals.
        /// </summary>
        public virtual bool UseBestVisual
        {
            get => SystemSettings.Handler.UseBestVisual;
            set => SystemSettings.Handler.UseBestVisual = value;
        }

        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        /// <remarks>
        /// It is used for paths, config, and other places the user doesn't see.
        /// By default it is set to the executable program name.
        /// </remarks>
        public virtual string Name
        {
            get => SystemSettings.Handler.AppName;
            set => SystemSettings.Handler.AppName = value;
        }

        /// <summary>
        /// Gets or sets the application display name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The display name is the name shown to the user in titles, reports, etc.
        /// while the application name is used for paths, config, and other
        /// places the user doesn't see.
        /// </para>
        /// <para>
        /// By default the application display name is the same as application
        /// name or a capitalized version of the program if the application
        /// name was not set either.
        /// It's usually better to set it explicitly to something nicer.
        /// </para>
        /// </remarks>
        public virtual string DisplayName
        {
            get => SystemSettings.Handler.AppDisplayName;
            set => SystemSettings.Handler.AppDisplayName = value;
        }

        /// <summary>
        /// Gets or sets the application class name.
        /// </summary>
        /// <remarks>
        /// It should be set by the application itself, there are
        /// no reasonable defaults.
        /// </remarks>
        public virtual string AppClassName
        {
            get => SystemSettings.Handler.AppClassName;
            set => SystemSettings.Handler.AppClassName = value;
        }

        /// <summary>
        /// Gets or sets the vendor name.
        /// </summary>
        /// <remarks>
        /// It is used in some areas such as configuration, standard paths, etc.
        /// It should be set by the application itself, there are
        /// no reasonable defaults.
        /// </remarks>
        public virtual string VendorName
        {
            get => SystemSettings.Handler.VendorName;
            set => SystemSettings.Handler.VendorName = value;
        }

        /// <summary>
        /// Gets or sets the vendor display name.
        /// </summary>
        /// <remarks>
        /// It is shown in titles, reports, dialogs to the user, while
        /// the vendor name is used in some areas such as configuration,
        /// standard paths, etc.
        /// It should be set by the application itself, there are
        /// no reasonable defaults.
        /// </remarks>
        public virtual string VendorDisplayName
        {
            get => SystemSettings.Handler.VendorDisplayName;
            set => SystemSettings.Handler.VendorDisplayName = value;
        }

        /// <summary>
        /// Gets the layout direction for the current locale or <see cref="LangDirection.Default"/>
        /// if it's unknown.
        /// </summary>
        public virtual LangDirection LangDirection
        {
            get
            {
                return SystemSettings.Handler.GetLangDirection();
            }
        }

        /// <summary>
        /// Informs all message pumps that they must terminate, and then closes
        /// all application windows after the messages have been processed.
        /// </summary>
        public static void Exit()
        {
            if (HasApplication)
                Handler.Exit();
        }

        /// <summary>
        /// Calls <see cref="Exit"/> and after that terminates this process and returns an
        /// exit code to the operating system.
        /// </summary>
        /// <param name="exitCode">
        /// The exit code to return to the operating system. Use 0 (zero) to indicate that
        /// the process completed successfully.
        /// </param>
        public static void ExitAndTerminate(int exitCode = 0)
        {
            Exit();
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Executes <see cref="App.IdleLog"/> using <see cref="BaseObject.Invoke"/>.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind.</param>
        public static void InvokeIdleLog(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            Invoke(() =>
            {
                App.IdleLog(obj, kind);
            });
        }

        /// <summary>
        /// Executes action in the application idle state using <see cref="BaseObject.Invoke"/>.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void InvokeIdle(Action? action)
        {
            Invoke(() =>
            {
                AddIdleTask(action);
            });
        }

        /// <summary>
        /// This function wakes up the (internal and platform dependent) idle system, i.e.
        /// it will force the system to send an idle event even if the system currently is
        /// idle and thus would not send any idle event until after some other event would get sent.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WakeUpIdle()
        {
            if (HasApplication)
                Handler?.WakeUpIdle();
        }

        /// <summary>
        /// Instructs the application to display a dialog with an optional
        /// <paramref name="message"/>,
        /// and to wait until the user dismisses the dialog.
        /// </summary>
        /// <param name="message">A string you want to display in the alert dialog, or,
        /// alternatively, an object that is converted into a string and displayed.</param>
        public static void Alert(object? message = null)
        {
            MessageBox.Show(
                App.SafeWindow,
                message,
                CommonStrings.Default.WindowTitleApplicationAlert,
                MessageBoxButtons.OK,
                MessageBoxIcon.None);
        }

        /// <summary>
        /// Logs name, value and hint as "{name} = {value} ({hint})".
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="kind">Item kind.</param>
        /// <param name="hint">Hint string. Optinal. If present, it is
        /// shown in () after the value.</param>
        public static void LogNameValue(
            object name,
            object? value,
            LogItemKind? kind = null,
            string? hint = null)
        {
            if (hint is not null)
            {
                hint = $" ({hint})";
            }

            Log($"{name} = {value}{hint}", kind ?? LogItemKind.Information);
        }

        /// <summary>
        /// Logs name and value as "{name} = {value}". If last logged item starts from "{name} = ",
        /// it is repaced with the new item.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="kind">Item kind.</param>
        public static void LogNameValueReplace(
            object name,
            object? value,
            LogItemKind? kind = null)
        {
            var prefix = $"{name} = ";
            LogReplace($"{prefix}{value}", prefix, kind ?? LogItemKind.Information);
        }

        /// <summary>
        /// Logs name and value pair as "{name} = {value}" if <paramref name="condition"/>
        /// is <c>true</c>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="condition">Log if <c>true</c>.</param>
        /// <param name="kind">Item kind.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogNameValueIf(
            string name,
            object? value,
            bool condition,
            LogItemKind kind = LogItemKind.Information)
        {
            if (condition)
                LogNameValue(name, value, kind);
        }

        /// <summary>
        /// Calls <see cref="LogMessage"/> event if <paramref name="condition"/>
        /// is <c>true</c>.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="condition">Log if <c>true</c>.</param>
        /// <param name="kind">Item kind.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogIf(
            object? obj,
            bool condition,
            LogItemKind kind = LogItemKind.Information)
        {
            if (condition)
                Log(obj, kind);
        }

        /// <summary>
        /// Creates application and main form, runs and disposes them.
        /// </summary>
        /// <param name="createFunc">Function which creates main form.</param>
        /// <param name="runAction">Runs action after main form is created.</param>
        /// <exception cref="Exception">If application is already created.</exception>
        public static void CreateAndRun(Func<Window> createFunc, Action? runAction = null)
        {
            if (!Initialized)
            {
                App? application;

                var appType = Type.GetType("Alternet.UI.Application, Alternet.UI");

                if (appType is not null)
                {
                    application = (App?)Activator.CreateInstance(appType);
                }
                else
                {
                    throw new Exception("Alternet.UI library not loaded.");
                }

                if (App.Handler is null)
                {
                    throw new Exception("Application handler is not assigned.");
                }

                application ??= new App(App.Handler);

                var window = createFunc();
                AddIdleTask(runAction);

                application.Run(window);

                window.Dispose();
                application.Dispose();
            }
            else
            {
                var window = createFunc();
                runAction?.Invoke();
                window.ShowAndFocus();
            }
        }

        /// <summary>
        /// Calls <paramref name="func"/> inside try-catch block if specified by the exception handling
        /// settings <see cref="FastThreadExceptions"/> and <see cref="GetUnhandledExceptionMode()"/>.
        /// </summary>
        /// <typeparam name="T">Type of the function result.</typeparam>
        /// <param name="func">Function to call.</param>
        /// <returns></returns>
        [return: MaybeNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T HandleThreadExceptions<T>(Func<T> func)
        {
            if (FastThreadExceptions)
                return func();

            return HandleThreadExceptionsCore(func);
        }

        /// <summary>
        /// Calls <paramref name="action"/> inside try-catch block if specified by the exception handling
        /// settings <see cref="FastThreadExceptions"/> and <see cref="GetUnhandledExceptionMode()"/>.
        /// </summary>
        /// <param name="action">Action to call.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HandleThreadExceptions(Action action)
        {
            if (FastThreadExceptions)
                action();
            else
                HandleThreadExceptionsCore(action);
        }

        /// <summary>
        /// Adds <paramref name="task"/> which will be executed one time
        /// when the application finished processing events and is
        /// about to enter the idle state.
        /// </summary>
        /// <param name="task">Task action.</param>
        /// <param name="param">Task parameter.</param>
        /// <remarks>
        /// This is thread-safe method and it can be called from non-UI threads.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIdleTask(Action<object?> task, object? param = null)
        {
            IdleTasks.Enqueue((task, param));
        }

        /// <summary>
        /// Adds <paramref name="task"/> which will be executed one time
        /// when the application finished processing events and is
        /// about to enter the idle state.
        /// </summary>
        /// <param name="task">Task action.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIdleTask(Action? task)
        {
            if (task is null)
                return;
            AddIdleTask((object? param) => task());
        }

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(object? obj)
        {
            Log($"Warning: {obj}", LogItemKind.Warning);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind. Optional.
        /// Default is <see cref="LogItemKind.Error"/>.</param>
        public static void LogError(object? obj, LogItemKind kind = LogItemKind.Error)
        {
            try
            {
                if (obj is Exception e)
                    LogUtils.LogException(e, kind);
                else
                {
                    if (obj is null)
                        return;

                    var prefix = "Error";
                    if (kind == LogItemKind.Information)
                        prefix = "Warning";

                    Log($"{prefix}: {obj}", kind);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Logs error message if DEBUG is defined.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogErrorIfDebug(object? obj)
        {
            LogError(obj);
        }

        /// <summary>
        /// Calls <see cref="LogError"/> if <paramref name="condition"/>
        /// is <c>true</c>.
        /// </summary>
        /// <param name="obj">Error text or object to log.</param>
        /// <param name="condition">Log if <c>true</c>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogErrorIf(
            object? obj,
            bool condition)
        {
            if (condition)
                LogError(obj);
        }

        /// <summary>
        /// Calls <see cref="LogMessage"/> event.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind.</param>
        public static void Log(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            try
            {
                IdleLog(obj, kind);

                if (LogMessage is null || LogInUpdates())
                    return;
                ProcessLogQueue(true);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Calls <see cref="Log"/> after calling <see cref="BeforeNativeLogMessage"/> event.
        /// </summary>
        /// <param name="s">Message to log.</param>
        public static void LogNativeMessage(string s)
        {
            if (BeforeNativeLogMessage is not null)
            {
                LogMessageEventArgs e = new(s);
                BeforeNativeLogMessage(current, e);
                if (e.Cancel)
                    return;
            }

            Log(s);
            Debug.WriteLine(s);
        }

        /// <summary>
        /// Sets system option value by name.
        /// </summary>
        /// <param name="name">Option name.</param>
        /// <param name="value">Option value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSystemOption(string name, int value)
        {
            SystemSettings.Handler.SetSystemOption(name, value);
        }

        /// <summary>
        /// Logs an empty string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogEmptyLine()
        {
            Log(" ");
        }

        /// <inheritdoc cref="Log"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugLog(object? msg)
        {
            Log(msg);
        }

        /// <inheritdoc cref="LogError"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugLogError(object? msg)
        {
            LogError(msg);
        }

        /// <inheritdoc cref="LogIf"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugLogIf(object? obj, bool condition)
        {
            if (condition)
                Log(obj);
        }

        /// <summary>
        /// Finds first visible window of the specified type. If no window is found, returns Null.
        /// </summary>
        /// <typeparam name="T">Type of the window to find.</typeparam>
        /// <returns></returns>
        public static T? FindVisibleWindow<T>()
            where T : Window
        {
            var windows = Current.LastActivatedWindows;

            foreach (var window in windows)
            {
                if (window is T t)
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Finds first window of the specified type. If no window is found, returns Null.
        /// </summary>
        /// <typeparam name="T">Type of the window to find.</typeparam>
        /// <returns></returns>
        public static T? FindWindow<T>()
            where T : Window
        {
            var windows = Current.Windows;

            foreach(var window in windows)
            {
                if (window is T t)
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Returns first window or <c>null</c> if there are no windows or window is not
        /// of the <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the window to return.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? FirstWindow<T>()
            where T : Window
        {
            var result = FindVisibleWindow<T>() ?? FindWindow<T>();
            return result;
        }

        /// <summary>
        /// Returns first window or <c>null</c> if there are no windows.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Window? FirstWindow()
        {
            return FirstWindow<Window>();
        }

        /// <summary>
        /// Calls <see cref="LogMessage"/> event to add or replace log message.
        /// </summary>
        /// <param name="obj">Message text.</param>
        /// <param name="prefix">Message text prefix.</param>
        /// <param name="kind">Message kind.</param>
        /// <remarks>
        /// If last logged message
        /// contains <paramref name="prefix"/>, last log item is replaced with
        /// <paramref name="obj"/> instead of adding new log item.
        /// </remarks>
        public static void LogReplace(
            object? obj,
            object? prefix = null,
            LogItemKind kind = LogItemKind.Information)
        {
            if (Terminating)
                return;

            try
            {
                var msg = obj?.ToString();
                if (msg is null || msg.Length == 0)
                    return;
                WriteToLogFileIfAllowed(msg);
                if (DebugWriteLine.HasKind(kind))
                    Debug.WriteLine(msg);
                var prefixStr = prefix?.ToString() ?? msg;

                var args = new LogMessageEventArgs(msg, prefixStr, true);
                args.Kind = kind;

                LogMessage?.Invoke(null, args);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets whether massive log outputs are performed and controls attached to log
        /// should not refresh.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LogInUpdates()
        {
            return logUpdateCount > 0;
        }

        /// <summary>
        /// Runs single idle task if it is present.
        /// </summary>
        public static bool ProcessIdleTask()
        {
            try
            {
                if (App.Terminating)
                    return false;

                if (IdleTasks.TryDequeue(out var task))
                {
                    task.Action(task.Data);
                    return true;
                }

                return false;
            }
            catch(Exception e)
            {
                TryCatchSilent(() => App.LogError(e));

                if (DebugUtils.IsDebugDefined)
                {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        /// Runs idle tasks if they are present.
        /// </summary>
        public static void ProcessIdleTasks()
        {
            int count = 0;

            while (count < 10)
            {
                count++;
                if (!ProcessIdleTask())
                    break;
            }
        }

        /// <summary>
        /// Changes the cursor to the given cursor for all windows in the application.
        /// </summary>
        /// <remarks>
        /// Use <see cref="IsBusyCursor"/> to get current busy cursor state.
        /// Use <see cref="EndBusyCursor"/> to revert the cursor back to its previous state.
        /// These two calls can be nested, and a counter ensures that only the outer calls take effect.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BeginBusyCursor() => Cursor.Factory.BeginBusyCursor();

        /// <summary>
        /// Changes the cursor back to the original cursor, for all windows in the application.
        /// </summary>
        /// <remarks>
        /// Use with <see cref="BeginBusyCursor"/> and <see cref="IsBusyCursor"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndBusyCursor() => Cursor.Factory.EndBusyCursor();

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginBusyCursor"/>
        /// and <see cref="EndBusyCursor"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public static void DoInsideBusyCursor(Action action)
        {
            BeginBusyCursor();
            try
            {
                action();
            }
            finally
            {
                EndBusyCursor();
            }
        }

        /// <summary>
        /// Logs section using <see cref="LogBeginSection"/>, <see cref="LogEndSection"/>
        /// and calling <paramref name="logAction"/> between them.
        /// </summary>
        /// <param name="logAction">Log action.</param>
        /// <param name="title">Section title (optional).</param>
        public static void LogSection(Action logAction, string? title = null)
        {
            LogBeginSection(title);
            try
            {
                logAction?.Invoke();
            }
            finally
            {
                LogEndSection();
            }
        }

        /// <summary>
        /// Begins log section.
        /// </summary>
        public static void LogBeginSection(string? title = null, LogItemKind kind = LogItemKind.Information)
        {
            Log(LogUtils.SectionSeparator, kind);

            if (title is not null)
            {
                Log(title, kind);
                Log(LogUtils.SectionSeparator, kind);
            }
        }

        /// <summary>
        /// Logs separator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogSeparator(LogItemKind kind = LogItemKind.Information)
        {
            Log(LogUtils.SectionSeparator, kind);
        }

        /// <summary>
        /// Ends log section.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogEndSection(LogItemKind kind = LogItemKind.Information)
        {
            Log(LogUtils.SectionSeparator, kind);
        }

        /// <summary>
        /// Raises <see cref="ThreadException"/> event.
        /// </summary>
        /// <param name="sender">Sender parameter of the event.</param>
        /// <param name="args">Event arguments.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RaiseThreadException(object sender, BaseThreadExceptionEventArgs args)
        {
            ThreadException?.Invoke(sender, args);
        }

        /// <summary>
        /// Calls <see cref="Log"/> method with <paramref name="obj"/> parameter
        /// when application becomes idle. This method works only if DEBUG conditional is defined
        /// and <paramref name="condition"/> is True.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="condition">The flag which specifies whether to
        /// call <see cref="Log"/> method.</param>
        /// <param name="kind">Message kind.</param>
        /// <remarks>
        /// This method is thread safe and can be called from non-ui threads.
        /// </remarks>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugIdleLogIf(
            object? obj,
            bool condition,
            LogItemKind kind = LogItemKind.Information)
        {
            if(condition)
                IdleLog(obj, kind);
        }

        /// <summary>
        /// Calls <see cref="Log"/> method with <paramref name="obj"/> parameter
        /// when application becomes idle. This method works only if DEBUG conditional is defined.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind.</param>
        /// <remarks>
        /// This method is thread safe and can be called from non-ui threads.
        /// </remarks>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugIdleLog(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            IdleLog(obj, kind);
        }

        /// <summary>
        /// Calls <see cref="Log"/> method with <paramref name="obj"/> parameter
        /// when application becomes idle.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind.</param>
        /// <remarks>
        /// This method is thread safe and can be called from non-ui threads.
        /// </remarks>
        public static void IdleLog(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            if (Terminating)
                return;

            DebugUtils.DebugCall(() =>
            {
                if (kind == LogItemKind.Error)
                {
                }

                if (kind == LogItemKind.Information)
                {
                }

                if (kind == LogItemKind.Warning)
                {
                }
            });

            var msg = obj?.ToString();

            if (msg is null)
                return;

            string[] result = LogUtils.LogToExternalIfAllowed(msg, kind);

            foreach (string s2 in result)
                LogQueue.Enqueue(new(s2, kind));
        }

        /// <summary>
        /// Adds log item using the specified <see cref="ListControlItem"/>.
        /// <see cref="LogListBox"/> controls binded to the log will paint added item
        /// with image, font and color properties specified in the <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <param name="kind">Item kind.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddLogItem(
            ListControlItem item,
            LogItemKind kind = LogItemKind.Information)
        {
            LogQueue.Enqueue(new(item, kind));
        }

        /// <inheritdoc cref="LogReplace"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugLogReplace(object? obj, string? prefix = null)
        {
            LogReplace(obj, prefix);
        }

        /// <summary>
        /// Shows <see cref="ThreadExceptionWindow"/> on the screen.
        /// </summary>
        /// <param name="onClose">Action to call when dialog is closed.</param>
        /// <param name="exception">Exception information.</param>
        /// <param name="additionalInfo">Additional information.</param>
        /// <param name="canContinue">Whether 'Continue' button is visible.</param>
        /// <param name="canQuit">Whether 'Quit' button is visible.</param>
        /// <returns><c>true</c> if continue pressed, <c>false</c> otherwise.</returns>
        public static void ShowExceptionWindow(
            Exception exception,
            string? additionalInfo = null,
            bool canContinue = true,
            bool canQuit = true,
            Action<bool>? onClose = null)
        {
            var errorWindow =
                new ThreadExceptionWindow(exception, additionalInfo, canContinue, canQuit);
            if (App.IsRunning)
            {
                errorWindow.ShowDialogAsync(null, (result) =>
                {
                    errorWindow.Dispose();
                    onClose?.Invoke(result);
                });
            }
            else
            {
                if (App.current is null)
                    return;

                errorWindow.CanContinue = false;
                errorWindow.CanQuit = true;
                App.Current.Run(errorWindow);
            }
        }

        /// <summary>
        /// Shows <see cref="ThreadExceptionWindow"/> on the screen.
        /// </summary>
        /// <param name="onClose">Action to call when dialog is closed.</param>
        /// <param name="exception">Exception information.</param>
        public static void ShowExceptionWindow(
            Exception exception,
            Action<bool>? onClose)
        {
            ShowExceptionWindow(
                exception,
                additionalInfo: null,
                canContinue: true,
                canQuit: true,
                onClose);
        }

        /// <summary>
        /// Instructs the application how to respond to unhandled exceptions in debug mode.
        /// </summary>
        /// <param name="mode">An <see cref="UnhandledExceptionMode"/>
        /// value describing how the application should
        /// behave if an exception is thrown without being caught.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode mode)
        {
            unhandledExceptionModeDebug = mode;
        }

        /// <summary>
        /// Can be called before massive outputs to log. Pairs with <see cref="LogEndUpdate"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogBeginUpdate()
        {
            logUpdateCount++;
        }

        /// <summary>
        /// Must be called after massive outputs to log,
        /// if <see cref="LogBeginUpdate"/> was previously called.
        /// </summary>
        public static void LogEndUpdate()
        {
            logUpdateCount--;
            if (logUpdateCount == 0)
                OnLogRefresh();
        }

        /// <summary>
        /// Handles exception showing <see cref="ThreadExceptionWindow"/>
        /// dialog and calling exception
        /// related events if specified in the exception handling settings.
        /// </summary>
        /// <param name="exception">Exception to process.</param>
        public static void OnThreadException(Exception exception)
        {
            bool HandleWithEvent()
            {
                if (ThreadExceptionAssigned)
                {
                    var args = new BaseThreadExceptionEventArgs(exception);
                    RaiseThreadException(Thread.CurrentThread, args);

                    if(args.Handled)

                    return args.Handled;
                }

                return false;
            }

            void HandleWithDialog(Action<bool>? onResult = null)
            {
                var td = new ThreadExceptionWindow(exception);

                td.ShowDialogAsync(null, (result) =>
                {
                    td.Dispose();

                    if (!result)
                    {
                        ExitAndTerminate(ThreadExceptionExitCode);
                        onResult?.Invoke(true);
                    }

                    onResult?.Invoke(false);
                });
            }

            if (inOnThreadException)
                return;

            inOnThreadException = true;

            try
            {
                if (LastUnhandledException == exception)
                    return;

                LastUnhandledException = exception;
                LastUnhandledExceptionThrown = false;

                if (LogUnhandledThreadException)
                {
                    LogUtils.LogException(exception);
                }

                var mode = GetUnhandledExceptionMode();

                switch (mode)
                {
                    case UnhandledExceptionMode.CatchException:
                        if(ThreadExceptionAssigned)
                            HandleWithEvent();
                        else
                            HandleWithDialog();
                        break;
                    case UnhandledExceptionMode.ThrowException:
                        LastUnhandledExceptionThrown = true;
                        ExceptionUtils.Rethrow(exception);
                        break;
                    case UnhandledExceptionMode.CatchWithDialog:
                        if (HandleWithEvent())
                            return;
                        HandleWithDialog();
                        break;
                    case UnhandledExceptionMode.CatchWithDialogAndThrow:
                        if (HandleWithEvent())
                            return;
                        HandleWithDialog((result) =>
                        {
                            if (!result)
                            {
                                LastUnhandledExceptionThrown = true;
                                ExceptionUtils.Rethrow(exception);
                            }
                        });

                        break;
                    case UnhandledExceptionMode.CatchWithThrow:
                        if (HandleWithEvent())
                            return;
                        LastUnhandledExceptionThrown = true;
                        ExceptionUtils.Rethrow(exception);
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                inOnThreadException = false;
            }
        }

        /// <summary>
        /// Gets current unhandled exception mode.
        /// </summary>
        /// <returns></returns>
        public static UnhandledExceptionMode GetUnhandledExceptionMode()
        {
            if (IsDebuggerAttached)
                return unhandledExceptionModeDebug;
            else
                return unhandledExceptionMode;
        }

        /// <summary>
        /// Processes all messages currently in the message queue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DoEvents()
        {
            if (App.Terminating)
                return;

            Current?.ProcessPendingEvents();
        }

        /// <summary>
        /// Instructs the application how to respond to unhandled exceptions.
        /// </summary>
        /// <param name="mode">An <see cref="UnhandledExceptionMode"/>
        /// value describing how the application should
        /// behave if an exception is thrown without being caught.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUnhandledExceptionMode(UnhandledExceptionMode mode)
        {
            unhandledExceptionMode = mode;
        }

        /// <summary>
        /// Gets all visible windows in the application except the specified window.
        /// </summary>
        /// <value>A <see cref="IEnumerable{Window}"/> that contains
        /// references to all visible <see cref="Window"/> objects in the application
        /// except the specified window.</value>
        public static IEnumerable<Window> VisibleWindowsWithExcept(Window? exceptWindow = null)
        {
            if (exceptWindow is null)
                return Current.VisibleWindows;
            var result = Current?.Windows.Where(x => (x.Visible && x != exceptWindow)) ?? [];
            return result;
        }

        /// <summary>
        /// Gets all windows in the application except the specified window.
        /// </summary>
        /// <value>A <see cref="IEnumerable{Window}"/> that contains
        /// references to all <see cref="Window"/> objects in the application
        /// except the specified window.</value>
        public static IEnumerable<Window> WindowsWithExcept(Window? exceptWindow = null)
        {
            if (exceptWindow is null)
                return Current.Windows;
            var result = Current?.Windows.Where(x => (x != exceptWindow)) ?? [];
            return result;
        }

        /// <summary>
        /// Raises <see cref="Idle"/> event.
        /// </summary>
        public static void RaiseIdle()
        {
            if (App.Terminating)
                return;

            if (HasForms)
            {
                ProcessLogQueue(true);
                ProcessIdleTasks();
            }

            Idle?.Invoke(current, EventArgs.Empty);
        }

        /// <summary>
        /// Instructs the application how to respond to unhandled exceptions.
        /// </summary>
        /// <param name="value">An <see cref="UnhandledExceptionMode"/>
        /// value describing how the application should
        /// behave if an exception is thrown without being caught.</param>
        public static void SetUnhandledExceptionModes(UnhandledExceptionMode value)
        {
            SetUnhandledExceptionModeIfDebugger(value);
            SetUnhandledExceptionMode(value);
        }

        /// <summary>
        /// Processes all pending events.
        /// </summary>
        public virtual void ProcessPendingEvents()
        {
            App.Handler.ProcessPendingEvents();
        }

        /// <summary>
        /// Checks whether there are any pending events in the queue.
        /// </summary>
        /// <returns><c>true</c> if there are any pending events in the queue,
        /// <c>false</c> otherwise.</returns>
        public virtual bool HasPendingEvents()
        {
            return Handler.HasPendingEvents();
        }

        /// <summary>
        /// Starts an application UI event loop and makes the specified window
        /// visible.
        /// Begins running a UI event processing loop on the current thread.
        /// </summary>
        /// <param name="window">A <see cref="Window"/> that opens automatically
        /// when an application starts.</param>
        /// <remarks>Typically, the main function of an application calls this
        /// method and passes to it the main window of the application.</remarks>
        public virtual void Run(Window window)
        {
            IsRunning = true;

            try
            {
                MainWindow = window ?? throw new ArgumentNullException(nameof(window));
                CheckDisposed();
                window.Show();

                while (true)
                {
                    try
                    {
                        Handler.Run(window);
                        break;
                    }
                    catch (Exception e)
                    {
                        OnThreadException(e);

                        if(Debugger.IsAttached && DebugUtils.IsDebugDefined
                            && LastUnhandledExceptionThrown)
                        {
                            throw;
                        }
                    }
                }

                SynchronizationContext.Uninstall();
            }
            finally
            {
                MainWindow = null;
                Terminating = true;
                IsRunning = false;
            }
        }

        /// <summary>
        /// Allows runtime switching of the UI environment theme.
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool SetNativeTheme(string theme)
        {
            return SystemSettings.Handler.SetNativeTheme(theme);
        }

        /// <summary>
        /// Allows the programmer to specify whether the application will use the best
        /// visual on systems that support several visual on the same display.
        /// </summary>
        public virtual void SetUseBestVisual(bool flag, bool forceTrueColour = false)
        {
            SystemSettings.Handler.SetUseBestVisual(flag, forceTrueColour);
        }

        internal static void WriteToLogFileIfAllowed(string? msg)
        {
            if (!LogFileIsEnabled || msg is null)
                return;
            try
            {
                LogUtils.LogToFile(msg);
            }
            catch
            {
            }
        }

        internal void RegisterWindow(Window window)
        {
            windows.Add(window);
        }

        internal void UnregisterWindow(Window window)
        {
            if (MainWindow == window)
                MainWindow = null;
            windows.Remove(window);
        }

        /// <summary>
        /// Sets the 'top' window.
        /// </summary>
        /// <param name="window">New 'top' window.</param>
        internal virtual void SetTopWindow(Window window)
        {
            Handler.SetTopWindow(window);
        }

        internal void RecreateAllHandlers()
        {
            foreach (var window in Windows)
                window.RecreateAllHandlers();
        }

        /// <summary>
        /// Processes log queue.
        /// </summary>
        /// <param name="refresh">Specifies whether to refresh attached log controls.</param>
        protected static void ProcessLogQueue(bool refresh)
        {
            if (LogQueue.IsEmpty || LogInUpdates())
                return;

            LogBeginUpdate();
            try
            {
                while (true)
                {
                    if (LogQueue.TryDequeue(out var queueItem))
                        LogToEvent(queueItem);
                    else
                        break;
                }
            }
            finally
            {
                LogEndUpdate();
            }
        }

        /// <summary>
        /// Creates default application handler.
        /// </summary>
        /// <returns></returns>
        protected static IApplicationHandler CreateDefaultHandler()
        {
            IApplicationHandler? result = null;
            Type? type;

            if (IsMaui && !IsLinuxOS)
                type = Type.GetType("Alternet.UI.MauiApplicationHandler, Alternet.UI.Maui");
            else
                type = Type.GetType("Alternet.UI.WxApplicationHandler, Alternet.UI");

            if (type is not null)
            {
                result = (IApplicationHandler?)Activator.CreateInstance(type);
            }

            return result ?? throw new Exception("Application handler not found.");
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            App.Current = null!;
        }

        [return: MaybeNull]
        private static T HandleThreadExceptionsCore<T>(Func<T> func)
        {
            if (GetUnhandledExceptionMode() == UnhandledExceptionMode.ThrowException)
                return func();

            try
            {
                return func();
            }
            catch (Exception e)
            {
                OnThreadException(e);
                return default!;
            }
        }

        private static void HandleThreadExceptionsCore(Action action)
        {
            if (GetUnhandledExceptionMode() == UnhandledExceptionMode.ThrowException)
            {
                action();
                return;
            }

            try
            {
                action();
            }
            catch (Exception e)
            {
                OnThreadException(e);
            }
        }

        private static void OnLogRefresh()
        {
            LogRefresh?.Invoke(null, EventArgs.Empty);
        }

        private static void LogToEvent(LogUtils.LogItem item)
        {
            if (LogMessage is null)
                return;

            LogMessageEventArgs? args = new();
            args.Item = item;
            args.Kind = item.Kind;
            args.Message = item.Msg;

            LogMessage(null, args);
        }

        private static void UpdateWakeUpIdleTimer()
        {
            if (wakeUpIdleWithTimer)
            {
                wakeUpIdleTimer ??= new();
                wakeUpIdleTimer.Stop();
                wakeUpIdleTimer.Interval = TimerUtils.DefaultWakeUpIdleTimeout;
                wakeUpIdleTimer.TickAction = () =>
                {
                    WakeUpIdle();
                };
                wakeUpIdleTimer.Start();
            }
            else
            {
                SafeDispose(ref wakeUpIdleTimer);
            }
        }

        internal sealed class Destructor
        {
            ~Destructor()
            {
                if (LogFileIsEnabled)
                    LogUtils.LogAppFinishedToFile();
            }
        }

        /// <summary>
        /// Build counter for the test purposes.
        /// </summary>
#pragma warning disable
        public static int BuildCounter = 6;
        private static Exception? lastUnhandledException;
        private static bool lastUnhandledExceptionThrown;
#pragma warning restore
    }
}