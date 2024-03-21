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
    /// Provides methods and properties to manage an application, such as
    /// methods to start and stop an application,
    /// and properties to get information about an application.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : BaseObject, IDisposable
    {
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

        internal const int BuildCounter = 6;
        internal static readonly Destructor MyDestructor = new();

        private static readonly ConcurrentQueue<(Action<object?> Action, object? Data)> IdleTasks = new();
        private static readonly ConcurrentQueue<(string Msg, LogItemKind Kind)> LogQueue = new();
        private static bool terminating = false;
        private static bool logFileIsEnabled;
        private static Application? current;
        private static IconSet? icon;
        private static bool inOnThreadException;
        private static int logUpdateCount;

        private static UnhandledExceptionMode unhandledExceptionMode
            = UnhandledExceptionMode.CatchException;

        private static UnhandledExceptionMode unhandledExceptionModeDebug
            = UnhandledExceptionMode.ThrowException;

        private readonly List<Window> windows = new();
        private readonly KeyboardInputProvider keyboardInputProvider;
        private readonly MouseInputProvider mouseInputProvider;
        private volatile bool isDisposed;
        private Native.Application nativeApplication;
        private VisualTheme visualTheme = StockVisualThemes.Native;
        private Window? window;

        static Application()
        {
            Is64BitOS = Environment.Is64BitOperatingSystem;
            Is64BitProcess = Environment.Is64BitProcess;

            var backend = WebBrowser.GetBackendOS();

            IsWindowsOS = backend == WebBrowserBackendOS.Windows;

            if (IsWindowsOS)
            {
                FastThreadExceptions = true;

                BackendOS = OperatingSystems.Windows;
                goto exit;
            }

            IsMacOS = backend == WebBrowserBackendOS.MacOS;

            if (IsMacOS)
            {
                BackendOS = OperatingSystems.MacOs;
                goto exit;
            }

            IsLinuxOS = backend == WebBrowserBackendOS.Unix;

            if (IsLinuxOS)
            {
                BackendOS = OperatingSystems.Linux;
                goto exit;
            }

#if NET5_0_OR_GREATER
            IsAndroidOS = OperatingSystem.IsAndroid();

            if (IsAndroidOS)
            {
                BackendOS = OperatingSystems.Android;
                goto exit;
            }

            IsIOS = OperatingSystem.IsIOS();

            if (IsIOS)
            {
                BackendOS = OperatingSystems.IOS;
                goto exit;
            }
#endif
            BackendOS = OperatingSystems.Unknown;
            IsUnknownOS = true;
            goto exit;

        exit:
            ;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
            nativeApplication = new Native.Application();

            SynchronizationContext.InstallIfNeeded();

            nativeApplication.Idle += NativeApplication_Idle;
            nativeApplication.LogMessage += NativeApplication_LogMessage;
            nativeApplication.Name = Path.GetFileNameWithoutExtension(
                Process.GetCurrentProcess()?.MainModule?.FileName!);
            current = this;

            keyboardInputProvider = new KeyboardInputProvider(
                nativeApplication.Keyboard);
            mouseInputProvider = new MouseInputProvider(nativeApplication.Mouse);

            if (SupressDiagnostics)
                Native.Application.SuppressDiagnostics(-1);

#if DEBUG
            WebBrowser.CrtSetDbgFlag(0);
#endif
            Initialized = true;
            Window.UpdateDefaultFont();
        }

        /// <summary>
        /// Occurs before native debug message needs to be displayed.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs>? BeforeNativeLogMessage;

        /// <summary>
        /// Occurs when controls which display log messages need to be refreshed.
        /// </summary>
        public static event EventHandler? LogRefresh;

        /// <summary>
        /// Occurs when the <see cref="VisualTheme"/> property changes.
        /// </summary>
        public static event EventHandler? VisualThemeChanged;

        /// <summary>
        /// Occurs when debug message needs to be displayed.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs>? LogMessage;

        /// <summary>
        ///  Occurs when an untrapped thread exception is thrown.
        /// </summary>
        /// <remarks>
        /// This event allows your application to handle otherwise
        /// unhandled exceptions that occur in UI threads. Attach
        /// your event handler to the <see cref="ThreadException"/> event
        /// to deal with these exceptions, which will
        /// leave your application in an unknown state. Where possible,
        /// exceptions should be handled by a structured
        /// exception handling block. You can change whether this callback
        /// is used for unhandled Windows Forms thread
        /// exceptions by setting <see cref="SetUnhandledExceptionMode"/>.
        /// </remarks>
        public static event ThreadExceptionEventHandler? ThreadException;

        /// <summary>
        /// Occurs when the application finishes processing events and is
        /// about to enter the idle state.
        /// </summary>
        public event EventHandler? Idle;

        /// <summary>
        /// Gets or sets whether to call <see cref="Debug.WriteLine(string)"/> when\
        /// <see cref="Application.Log"/> is called. Default is <c>false</c>.
        /// </summary>
        public static bool DebugWriteLine { get; set; } = false;

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
                return icon ?? Application.FirstWindow()?.Icon;
            }

            set
            {
                icon = value;
            }
        }

        /// <summary>
        /// Gets whether application has forms.
        /// </summary>
        public static bool HasForms => Application.current?.Windows.Count > 0;

        /// <summary>
        /// Gets whether application was initialized;
        /// </summary>
        public static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets application log file path.
        /// </summary>
        /// <remarks>
        /// Default value is exe file path and "Alternet.UI.log" file name.
        /// </remarks>
        public static string LogFilePath { get; set; } =
            Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");

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
                    LogUtils.LogToFileAppStarted();
            }
        }

        /// <summary>
        /// Returns true if between two <see cref="BeginBusyCursor"/> and
        /// <see cref="EndBusyCursor"/> calls.
        /// </summary>
        public static bool IsBusyCursor => Native.WxOtherFactory.IsBusyCursor();

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
        /// Allows to suppress some debug messages.
        /// </summary>
        /// <remarks>
        ///  Currently used to suppress GTK messages under Linux. Default
        ///  value is true.
        /// </remarks>
        public static bool SupressDiagnostics { get; set; } = true;

        /// <summary>
        /// Gets whether <see cref="Run"/> method execution is finished.
        /// </summary>
        public static bool Terminating { get => terminating; }

        /// <summary>
        /// Gets whether execution is inside the <see cref="Run"/> method.
        /// </summary>
        public static bool IsRunning { get; internal set; }

        /// <summary>
        /// Gets the <see cref="Application"/> object for the currently
        /// runnning application.
        /// </summary>
        public static Application Current
        {
            get
            {
                // maybe make it thread static?
                // maybe move this to native?
                return current ?? throw new InvalidOperationException(
                    ErrorMessages.Default.CurrentApplicationIsNotSet);
            }
        }

        /// <summary>
        /// Gets the instantiated windows in an application.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{Window}"/> that contains
        /// references to all window objects in the current application.</value>
        public IReadOnlyList<Window> Windows => windows;

        /// <summary>
        /// Gets all visible windows in an application.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{Window}"/> that contains
        /// references to all visible window objects in the current application.</value>
        public virtual IEnumerable<Window> VisibleWindows
        {
            get
            {
                var result = Windows.Where(x => x.Visible);
                return result;
            }
        }

        /// <summary>
        /// Gets whether <see cref="Dispose(bool)"/> has been called.
        /// </summary>
        public virtual bool IsDisposed
        {
            get => isDisposed;
            private set => isDisposed = value;
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
            get => nativeApplication.Name;
            set => nativeApplication.Name = value;
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
            get => nativeApplication.DisplayName;
            set => nativeApplication.DisplayName = value;
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
            get => nativeApplication.AppClassName;
            set => nativeApplication.AppClassName = value;
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
            get => nativeApplication.VendorName;
            set => nativeApplication.VendorName = value;
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
            get => nativeApplication.VendorDisplayName;
            set => nativeApplication.VendorDisplayName = value;
        }

        /// <summary>
        /// Allows the programmer to specify whether the application will exit when the
        /// top-level frame is deleted.
        /// Returns true if the application will exit when the top-level frame is deleted.
        /// </summary>
        public virtual bool ExitOnFrameDelete
        {
            get => nativeApplication.GetExitOnFrameDelete();
            set => nativeApplication.SetExitOnFrameDelete(value);
        }

        /// <summary>
        /// Gets the layout direction for the current locale or <see cref="LangDirection.Default"/>
        /// if it's unknown.
        /// </summary>
        public virtual LangDirection LangDirection
        {
            get
            {
                return (LangDirection)nativeApplication.GetLayoutDirection();
            }
        }

        /// <summary>
        /// Gets or sets whether application will use the best visual on systems that
        /// support different visuals.
        /// </summary>
        public virtual bool UseBestVisual
        {
            get => nativeApplication.GetUseBestVisual();
            set => nativeApplication.SetUseBestVisual(value, false);
        }

        /// <summary>
        /// Gets whether the application is active, i.e. if one of its windows is currently in
        /// the foreground.
        /// </summary>
        public virtual bool IsActive => nativeApplication.IsActive();

        /// <summary>
        /// Gets or sets a <see cref="UI.VisualTheme"/> that is used by
        /// UI controls in the application.
        /// </summary>
        internal virtual VisualTheme VisualTheme
        {
            get => visualTheme;
            set
            {
                if (visualTheme == value)
                    return;

                visualTheme = value;

                OnVisualThemeChanged();
                VisualThemeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal Window? MainWindow
        {
            get => window;
            set => window = value;
        }

        internal Native.Clipboard NativeClipboard => nativeApplication.Clipboard;

        internal Native.Keyboard NativeKeyboard => nativeApplication.Keyboard;

        internal Native.Application NativeApplication => nativeApplication;

        internal Native.Mouse NativeMouse => nativeApplication.Mouse;

        internal bool InUixmlPreviewerMode
        {
            get => nativeApplication.InUixmlPreviewerMode;
            set => nativeApplication.InUixmlPreviewerMode = value;
        }

        internal bool InvokeRequired => nativeApplication.InvokeRequired;

        internal string EventArgString => NativeApplication.EventArgString;

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
                FirstWindow(),
                message,
                CommonStrings.Default.WindowTitleApplicationAlert,
                MessageBoxButtons.OK,
                MessageBoxIcon.None);
        }

        /// <summary>
        /// Changes the cursor to the given cursor for all windows in the application.
        /// </summary>
        /// <remarks>
        /// Use <see cref="IsBusyCursor"/> to get current busy cursor state.
        /// Use <see cref="EndBusyCursor"/> to revert the cursor back to its previous state.
        /// These two calls can be nested, and a counter ensures that only the outer calls take effect.
        /// </remarks>
        public static void BeginBusyCursor() => Native.WxOtherFactory.BeginBusyCursor();

        /// <summary>
        /// Changes the cursor back to the original cursor, for all windows in the application.
        /// </summary>
        /// <remarks>
        /// Use with <see cref="BeginBusyCursor"/> and <see cref="IsBusyCursor"/>.
        /// </remarks>
        public static void EndBusyCursor() => Native.WxOtherFactory.EndBusyCursor();

        /// <summary>
        /// Adds <paramref name="task"/> which will be executed one time
        /// when the application finished processing events and is
        /// about to enter the idle state.
        /// </summary>
        /// <param name="task">Task action.</param>
        /// <param name="param">Task parameter.</param>
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
        public static void AddIdleTask(Action? task)
        {
            if (task is null)
                return;
            AddIdleTask((object? param) => task());
        }

        /// <summary>
        /// Sets wxSystemOptions value.
        /// </summary>
        /// <param name="name">Option name.</param>
        /// <param name="value">Option value.</param>
        public static void SetSystemOption(string name, int value)
        {
            Native.Application.SetSystemOptionInt(name, value);
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

        /// <summary>
        /// Logs name and value pair as "{name} = {value}".
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="kind">Item kind.</param>
        public static void LogNameValue(
            string name,
            object? value,
            LogItemKind kind = LogItemKind.Information)
        {
            Application.Log($"{name} = {value}", kind);
        }

        /// <summary>
        /// Logs name and value pair as "{name} = {value}" if <paramref name="condition"/>
        /// is <c>true</c>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="condition">Log if <c>true</c>.</param>
        /// <param name="kind">Item kind.</param>
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
        public static void LogIf(
            object? obj,
            bool condition,
            LogItemKind kind = LogItemKind.Information)
        {
            if (condition)
                Log(obj, kind);
        }

        /// <summary>
        /// Logs warning message.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        public static void LogWarning(object? obj)
        {
            Log($"Warning: {obj}", LogItemKind.Warning);
        }

        /// <summary>
        /// Logs error message.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        public static void LogError(object? obj)
        {
            Log($"Error: {obj}", LogItemKind.Error);
        }

        /// <summary>
        /// Calls <see cref="LogMessage"/> event.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind.</param>
        public static void Log(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            IdleLog(obj, kind);

            if (LogMessage is null || LogInUpdates())
                return;
            ProcessLogQueue(true);
        }

        /// <summary>
        /// Executes the specified delegate on the thread that owns the application.
        /// </summary>
        /// <param name="method">A delegate that contains a method to be called
        /// in the control's thread context.</param>
        /// <returns>An <see cref="object"/> that contains the return value from
        /// the delegate being invoked, or <c>null</c> if the delegate has no
        /// return value.</returns>
        public static object? Invoke(Delegate? method)
        {
            if (method == null)
                return null;
            return Invoke(method, Array.Empty<object?>());
        }

        /// <summary>
        /// Executes the specified action on the thread that owns the application.
        /// </summary>
        /// <param name="action">An action to be called in the control's
        /// thread context.</param>
        public static void Invoke(Action? action)
        {
            if (action == null)
                return;
            Invoke(action, Array.Empty<object?>());
        }

        /// <summary>
        /// Executes the specified delegate, on the thread that owns the application,
        /// with the specified list of arguments.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters of
        /// the same number and type that are contained in the
        /// <c>args</c> parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to
        /// the specified method. This parameter can be <c>null</c> if the
        /// method takes no arguments.</param>
        /// <returns>An <see cref="object"/> that contains the return value
        /// from the delegate being invoked, or <c>null</c> if the delegate has
        /// no return value.</returns>
        public static object? Invoke(Delegate method, object?[] args)
            => SynchronizationService.Invoke(method, args);

        /// <summary>
        /// Executes <see cref="Application.IdleLog"/> using <see cref="Invoke(Action?)"/>.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        /// <param name="kind">Message kind.</param>
        public static void InvokeIdleLog(object? obj, LogItemKind kind = LogItemKind.Information)
        {
            Invoke(() =>
            {
                Application.IdleLog(obj, kind);
            });
        }

        /// <summary>
        /// Executes action in the application idle state using <see cref="Invoke(Action?)"/>.
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
            if (Application.Terminating)
                return;

            var msg = obj?.ToString();

            if (msg is null)
                return;

            WriteToLogFileIfAllowed(msg);

            string[] result = msg.Split(
                StringUtils.StringSplitToArrayChars,
                StringSplitOptions.RemoveEmptyEntries);

            if (DebugWriteLine || LogMessage is null)
            {
                foreach (string s2 in result)
                {
                    Debug.WriteLine(s2);

                    try
                    {
                        Console.WriteLine(s2);
                    }
                    catch
                    {
                    }
                }
            }

            foreach (string s2 in result)
                LogQueue.Enqueue((s2, kind));
        }

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
        /// Checks if the Android version (returned by the Linux command uname) is greater than
        /// or equal to the specified version. This method can be used to guard APIs that were
        /// added in the specified version.
        /// </summary>
        /// <param name="major">The major release number.</param>
        /// <param name="minor">The minor release number.</param>
        /// <param name="build">The build release number.</param>
        /// <param name="revision">The revision release number.</param>
        /// <returns><c>true</c> if the current application is running on an Android version that
        /// is at least what was specified in the parameters; <c>false</c> otherwise.</returns>
        public static bool IsAndroidVersionAtLeast(
            int major,
            int minor = 0,
            int build = 0,
            int revision = 0)
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsAndroidVersionAtLeast(major, minor, build, revision);
#else
            return false;
#endif
        }

        /// <summary>
        /// Logs an empty string.
        /// </summary>
        public static void LogEmptyLine()
        {
            Log(" ");
        }

        /// <inheritdoc cref="Log"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLog(object? msg)
        {
            Log(msg);
        }

        /// <inheritdoc cref="LogIf"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogIf(object? obj, bool condition)
        {
            if (condition)
                Log(obj);
        }

        /// <inheritdoc cref="LogReplace"/>
        /// <remarks>
        /// Works only if DEBUG conditional is defined.
        /// </remarks>
        [Conditional("DEBUG")]
        public static void DebugLogReplace(object? obj, string? prefix = null)
        {
            LogReplace(obj, prefix);
        }

        /// <summary>
        /// Returns first window or <c>null</c> if there are no windows or window is not
        /// of the <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the window to return.</typeparam>
        public static T? FirstWindow<T>()
            where T : class
        {
            var windows = Current.Windows;

            if (windows.Count == 0)
                return null;
            return windows[0] as T;
        }

        /// <summary>
        /// Can be called before massive outputs to log. Pairs with <see cref="LogEndUpdate"/>.
        /// </summary>
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
        /// Gets whether massive log outputs are performed and controls attached to log
        /// should not refresh.
        /// </summary>
        /// <returns></returns>
        public static bool LogInUpdates()
        {
            return logUpdateCount > 0;
        }

        /// <summary>
        /// Runs idle tasks if they are present.
        /// </summary>
        public static void ProcessIdleTasks()
        {
            while (true)
            {
                if (IdleTasks.TryDequeue(out var task))
                    task.Action(task.Data);
                else
                    break;
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
        public static void LogSeparator(LogItemKind kind = LogItemKind.Information)
        {
            Log(LogUtils.SectionSeparator, kind);
        }

        /// <summary>
        /// Ends log section.
        /// </summary>
        public static void LogEndSection(LogItemKind kind = LogItemKind.Information)
        {
            Log(LogUtils.SectionSeparator, kind);
        }

        /// <summary>
        /// Returns first window or <c>null</c> if there are no windows.
        /// </summary>
        public static Window? FirstWindow()
        {
            return FirstWindow<Window>();
        }

        /// <summary>
        /// Informs all message pumps that they must terminate, and then closes
        /// all application windows after the messages have been processed.
        /// </summary>
        public static void Exit()
        {
            current?.nativeApplication.Exit();
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
            string? prefix = null,
            LogItemKind kind = LogItemKind.Information)
        {
            var msg = obj?.ToString();
            if (msg is null || msg.Length == 0)
                return;
            WriteToLogFileIfAllowed(msg);
            if (DebugWriteLine)
                Debug.WriteLine(msg);
            prefix ??= msg;

            var args = new LogMessageEventArgs(msg, prefix, true);
            args.Kind = kind;

            LogMessage?.Invoke(Current, args);
        }

        /// <summary>Processes all messages currently in the message queue.</summary>
        public static void DoEvents()
        {
            Current?.ProcessPendingEvents();
        }

        /// <summary>
        /// Processes all pending events.
        /// </summary>
        public virtual void ProcessPendingEvents()
        {
            nativeApplication.ProcessPendingEvents();
        }

        /// <summary>
        /// Checks whether there are any pending events in the queue.
        /// </summary>
        /// <returns><c>true</c> if there are any pending events in the queue,
        /// <c>false</c> otherwise.</returns>
        public bool HasPendingEvents()
        {
            return nativeApplication.HasPendingEvents();
        }

        /// <summary>
        /// Instructs the application how to respond to unhandled exceptions.
        /// </summary>
        /// <param name="mode">An <see cref="UnhandledExceptionMode"/>
        /// value describing how the application should
        /// behave if an exception is thrown without being caught.</param>
        public virtual void SetUnhandledExceptionMode(UnhandledExceptionMode mode)
        {
            unhandledExceptionMode = mode;
        }

        /// <summary>
        /// Instructs the application how to respond to unhandled exceptions in debug mode.
        /// </summary>
        /// <param name="mode">An <see cref="UnhandledExceptionMode"/>
        /// value describing how the application should
        /// behave if an exception is thrown without being caught.</param>
        public virtual void SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode mode)
        {
            unhandledExceptionModeDebug = mode;
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
        public void Run(Window window)
        {
            IsRunning = true;

            try
            {
                this.window = window ?? throw new ArgumentNullException(nameof(window));
                CheckDisposed();
                window.Show();

                while (true)
                {
                    try
                    {
                        nativeApplication.Run(
                            ((WindowHandler)window.Handler).NativeControl);
                        break;
                    }
                    catch (Exception e)
                    {
                        OnThreadException(e);
                    }
                }

                SynchronizationContext.Uninstall();
                this.window = null;
            }
            finally
            {
                terminating = true;
                IsRunning = false;
            }
        }

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Allows runtime switching of the UI environment theme.
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
        public virtual bool SetNativeTheme(string theme)
        {
            return nativeApplication.SetNativeTheme(theme);
        }

        /// <summary>
        /// Allows the programmer to specify whether the application will use the best
        /// visual on systems that support several visual on the same display.
        /// </summary>
        public virtual void SetUseBestVisual(bool flag, bool forceTrueColour = false)
        {
            nativeApplication.SetUseBestVisual(flag, forceTrueColour);
        }

        [return: MaybeNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T HandleThreadExceptions<T>(Func<T> func)
        {
            if (FastThreadExceptions)
                return func();

            return HandleThreadExceptionsCore(func);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void HandleThreadExceptions(Action action)
        {
            if (FastThreadExceptions)
                action();
            else
                HandleThreadExceptionsCore(action);
        }

        internal static void OnThreadException(Exception exception)
        {
            if (inOnThreadException)
                return;

            inOnThreadException = true;
            try
            {
                if(LogUnhandledThreadException)
                {
                    LogUtils.LogException(exception, "Application.OnThreadException");
                }

                if (GetUnhandledExceptionMode() == UnhandledExceptionMode.ThrowException)
                    throw exception;

                if (ThreadException is not null)
                {
                    var args = new ThreadExceptionEventArgs(exception);
                    ThreadException(Thread.CurrentThread, args);
                }
                else
                {
                    var td = new ThreadExceptionWindow(exception);
                    var result = ModalResult.Accepted;

                    try
                    {
                        result = td.ShowModal();
                    }
                    finally
                    {
                        td.Dispose();
                    }

                    if (result == ModalResult.Canceled)
                    {
                        ExitAndTerminate(ThreadExceptionExitCode);
                    }
                }
            }
            finally
            {
                inOnThreadException = false;
            }
        }

        /// <summary>
        /// Sets the 'top' window.
        /// </summary>
        /// <param name="window">New 'top' window.</param>
        internal virtual void SetTopWindow(Window window)
        {
            nativeApplication.SetTopWindow(window.WxWidget);
        }

        internal void WakeUpIdle()
        {
            nativeApplication.WakeUpIdle();
        }

        internal void RegisterWindow(Window window)
        {
            windows.Add(window);
        }

        internal void UnregisterWindow(Window window)
        {
            if (this.window == window)
                this.window = null;
            windows.Remove(window);
        }

        internal void BeginInvoke(Action action)
        {
            nativeApplication.BeginInvoke(action);
        }

        /// <summary>
        /// Gets current unhandled exception mode.
        /// </summary>
        /// <returns></returns>
        protected static UnhandledExceptionMode GetUnhandledExceptionMode()
        {
            if (Application.IsDebuggerAttached)
                return unhandledExceptionModeDebug;
            else
                return unhandledExceptionMode;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the object and
        /// optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and
        /// unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    nativeApplication.Idle -= NativeApplication_Idle;
                    nativeApplication.LogMessage -= NativeApplication_LogMessage;
                    keyboardInputProvider.Dispose();
                    mouseInputProvider.Dispose();
                    nativeApplication.Dispose();
                    nativeApplication = null!;

                    current = null;
                }

                IsDisposed = true;
            }
        }

        private static void OnLogRefresh()
        {
            LogRefresh?.Invoke(Current, EventArgs.Empty);
        }

        private static void ProcessLogQueue(bool refresh)
        {
            if (LogQueue.IsEmpty || LogInUpdates())
                return;

            LogBeginUpdate();
            try
            {
                while (true)
                {
                    if (LogQueue.TryDequeue(out var queueItem))
                        LogToEvent(queueItem.Kind, queueItem.Msg);
                    else
                        break;
                }
            }
            finally
            {
                LogEndUpdate();
            }
        }

        private static void LogToEvent(LogItemKind kind, params string[] items)
        {
            if (LogMessage is null)
                return;

            LogMessageEventArgs? args = new();
            args.Kind = kind;

            foreach (string s2 in items)
            {
                args.Message = s2;
                LogMessage(Current, args);
            }
        }

        private static void WriteToLogFileIfAllowed(string? msg)
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

        private void NativeApplication_Idle()
        {
            if (HasForms)
            {
                ProcessLogQueue(true);
                ProcessIdleTasks();
            }

            Idle?.Invoke(this, EventArgs.Empty);
        }

        private void OnVisualThemeChanged()
        {
            foreach (var window in Windows)
                window.RecreateAllHandlers();
        }

        private void NativeApplication_LogMessage()
        {
            var s = this.EventArgString;

            if (BeforeNativeLogMessage is not null)
            {
                LogMessageEventArgs e = new(s);
                BeforeNativeLogMessage(Application.Current, e);
                if (e.Cancel)
                    return;
            }

            Log(s);
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }

        internal sealed class Destructor
        {
            ~Destructor()
            {
                if (logFileIsEnabled)
                    LogUtils.LogToFileAppFinished();
            }
        }
    }
}