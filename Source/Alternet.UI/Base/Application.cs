using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties to manage an application, such as
    /// methods to start and stop an application,
    /// and properties to get information about an application.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : IDisposable
    {
        internal const int BuildCounter = 2;
        internal static readonly Destructor MyDestructor = new();

        private static bool terminating = false;
        private static bool logFileIsEnabled;
        private static Application? current;
        private static OperatingSystems backendOS = OperatingSystems.None;

#if NET5_0_OR_GREATER
        private static bool? isAndroidOS;
        private static bool? isOS;
#endif

        private readonly List<Window> windows = [];
        private readonly KeyboardInputProvider keyboardInputProvider;
        private readonly MouseInputProvider mouseInputProvider;

        private UnhandledExceptionMode unhandledExceptionMode;
        private volatile bool isDisposed;
        private Native.Application nativeApplication;
        private VisualTheme visualTheme = StockVisualThemes.Native;
        private ThreadExceptionEventHandler? threadExceptionHandler;
        private bool inOnThreadException;
        private Window? window;

        static Application()
        {
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
        }

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
        public event ThreadExceptionEventHandler ThreadException
        {
            add
            {
                threadExceptionHandler = value;
            }

            remove
            {
                threadExceptionHandler -= value;
            }
        }

        /// <summary>
        /// Occurs when the <see cref="VisualTheme"/> property changes.
        /// </summary>
        public event EventHandler? VisualThemeChanged;

        /// <summary>
        /// Occurs when the application finishes processing and is
        /// about to enter the idle state.
        /// </summary>
        public event EventHandler? Idle;

        /// <summary>
        /// Occurs when debug message needs to be displayed.
        /// </summary>
        public event EventHandler<LogMessageEventArgs>? LogMessage;

        /// <summary>
        /// Gets or sets whether to call <see cref="Debug.WriteLine(string)"/> when\
        /// <see cref="Application.Log"/> is called. Default is <c>false</c>.
        /// </summary>
        public static bool DebugWriteLine { get; set; } = false;

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
        /// Gets operating system as <see cref="OperatingSystems"/> enumeration.
        /// </summary>
        public static OperatingSystems BackendOS
        {
            get
            {
                if (backendOS == OperatingSystems.None)
                {
                    switch (WebBrowser.GetBackendOS())
                    {
                        case WebBrowserBackendOS.MacOS:
                            backendOS = OperatingSystems.MacOs;
                            break;
                        case WebBrowserBackendOS.Unix:
                            backendOS = OperatingSystems.Linux;
                            break;
                        case WebBrowserBackendOS.Windows:
                            backendOS = OperatingSystems.Windows;
                            break;
                        default:
                            if(IsAndroidOS)
                                backendOS = OperatingSystems.Android;
                            else
                            if(IsIOS)
                                backendOS = OperatingSystems.IOS;
                            break;
                    }
                }

                return backendOS;
            }
        }

        /// <summary>
        /// Indicates whether the current application is running on Android.
        /// </summary>
        public static bool IsAndroidOS
        {
            get
            {
#if NET5_0_OR_GREATER
                return isAndroidOS ??= OperatingSystem.IsAndroid();
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Indicates whether the current application is running on Android.
        /// </summary>
        public static bool IsIOS
        {
            get
            {
#if NET5_0_OR_GREATER
                return isOS ??= OperatingSystem.IsIOS();
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Returns true if operating system is Windows.
        /// </summary>
        public static bool IsWindowsOS
        {
            get
            {
                return WebBrowser.GetBackendOS() == WebBrowserBackendOS.Windows;
            }
        }

        /// <summary>
        /// Returns true if operating system is Linux.
        /// </summary>
        public static bool IsLinuxOS
        {
            get
            {
                return WebBrowser.GetBackendOS() == WebBrowserBackendOS.Unix;
            }
        }

        /// <summary>
        /// Returns true if between two <see cref="BeginBusyCursor"/> and
        /// <see cref="EndBusyCursor"/> calls.
        /// </summary>
        public static bool IsBusyCursor => Native.WxOtherFactory.IsBusyCursor();

        /// <summary>
        /// Returns true if operating system is Apple macOS.
        /// </summary>
        public static bool IsMacOs
        {
            get
            {
                return WebBrowser.GetBackendOS() == WebBrowserBackendOS.MacOS;
            }
        }

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
        /// Gets the <see cref="Application"/> object for the currently
        /// runnning application.
        /// </summary>
        public static Application Current
        {
            get
            {
                // todo: maybe make it thread static?
                // todo: maybe move this to native?
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
        /// Gets or sets a <see cref="UI.VisualTheme"/> that is used by
        /// UI controls in the application.
        /// </summary>
        public virtual VisualTheme VisualTheme
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
        /// Gets the layout direction for the current locale or <see cref="LayoutDirection.Default"/>
        /// if it's unknown.
        /// </summary>
        public virtual LayoutDirection LayoutDirection
        {
            get
            {
                return (LayoutDirection)nativeApplication.GetLayoutDirection();
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
        /// Sets wxSystemOptions value.
        /// </summary>
        /// <param name="name">Option name.</param>
        /// <param name="value">Option value.</param>
        public static void SetSystemOption(string name, int value)
        {
            Native.Application.SetSystemOptionInt(name, value);
        }

        /// <summary>
        /// Logs name and value pair as "{name} = {value}".
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public static void LogNameValue(string name, object? value)
        {
            Application.Log($"{name} = {value}");
        }

        /// <summary>
        /// Calls <see cref="LogMessage"/> event.
        /// </summary>
        /// <param name="obj">Message text or object to log.</param>
        public static void Log(object? obj)
        {
            var msg = obj?.ToString();

            if (msg is null)
                return;

            WriteToLogFileIfAllowed(msg);

            string[] result = msg.Split(StringUtils.StringSplitToArrayChars, StringSplitOptions.RemoveEmptyEntries);
            var args = new LogMessageEventArgs();
            var evt = Current?.LogMessage;

            foreach (string s2 in result)
            {
                if(DebugWriteLine)
                    Debug.WriteLine(s2);

                if(evt is not null)
                {
                    args.Message = s2;
                    evt(Current, args);
                }
            }
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
        /// Begins log section.
        /// </summary>
        public static void LogBeginSection(string? title = null)
        {
            Log(LogUtils.SectionSeparator);

            if(title is not null)
            {
                Log(title);
                Log(LogUtils.SectionSeparator);
            }
        }

        /// <summary>
        /// Logs separator.
        /// </summary>
        public static void LogSeparator()
        {
            Log(LogUtils.SectionSeparator);
        }

        /// <summary>
        /// Ends log section.
        /// </summary>
        public static void LogEndSection()
        {
            Log(LogUtils.SectionSeparator);
        }

        /// <summary>
        /// Returns first window or <c>null</c> if there are no windows.
        /// </summary>
        public static Window? FirstWindow()
        {
            return FirstWindow<Window>();
        }

        /// <summary>
        /// Calls <see cref="LogMessage"/> event to add or replace log message.
        /// </summary>
        /// <param name="obj">Message text.</param>
        /// <param name="prefix">Message text prefix.</param>
        /// <remarks>
        /// If last logged message
        /// contains <paramref name="prefix"/>, last log item is replaced with
        /// <paramref name="obj"/> instead of adding new log item.
        /// </remarks>
        public static void LogReplace(object? obj, string? prefix = null)
        {
            var msg = obj?.ToString();
            if (string.IsNullOrEmpty(msg))
                return;
            WriteToLogFileIfAllowed(msg);
            if (DebugWriteLine)
                Debug.WriteLine(msg);
            prefix ??= msg;
            Current?.LogMessage?.Invoke(Current, new LogMessageEventArgs(msg!, prefix!, true));
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
        /// Informs all message pumps that they must terminate, and then closes
        /// all application windows after the messages have been processed.
        /// </summary>
        public virtual void Exit()
        {
            nativeApplication.Exit();
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
            this.window = window ?? throw new ArgumentNullException(nameof(window));
            CheckDisposed();
            window.Show();
            nativeApplication.Run(
                ((NativeWindowHandler)window.Handler).NativeControl);
            SynchronizationContext.Uninstall();
            this.window = null;
            terminating = true;
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
        internal static T HandleThreadExceptions<T>(Func<T> func)
        {
            if (current == null)
                return func();

            return current.HandleThreadExceptionsCore(func);
        }

        internal static void HandleThreadExceptions(Action action)
        {
            if (current == null)
                action();
            else
                current.HandleThreadExceptionsCore(action);
        }

        /// <summary>
        /// Sets the 'top' window.
        /// </summary>
        /// <param name="window">New 'top' window.</param>
        internal virtual void SetTopWindow(Window window)
        {
            nativeApplication.SetTopWindow(window.WxWidget);
        }

        internal void OnThreadException(Exception exception)
        {
            if (inOnThreadException)
                return;

            inOnThreadException = true;
            try
            {
                if (unhandledExceptionMode ==
                    UnhandledExceptionMode.ThrowException
                    || System.Diagnostics.Debugger.IsAttached)
                    throw exception;

                if (threadExceptionHandler is not null)
                {
                    threadExceptionHandler(
                        Thread.CurrentThread,
                        new ThreadExceptionEventArgs(exception));
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
                        Exit();
                        Environment.Exit(0);
                    }
                }
            }
            finally
            {
                inOnThreadException = false;
            }
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
        private T HandleThreadExceptionsCore<T>(Func<T> func)
        {
            if (unhandledExceptionMode == UnhandledExceptionMode.ThrowException)
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

        private void HandleThreadExceptionsCore(Action action)
        {
            if (unhandledExceptionMode == UnhandledExceptionMode.ThrowException)
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

        private void NativeApplication_Idle(
            object? sender,
            EventArgs e) => Idle?.Invoke(this, EventArgs.Empty);

        private void OnVisualThemeChanged()
        {
            foreach (var window in Windows)
                window.RecreateAllHandlers();
        }

        private void NativeApplication_LogMessage(object? sender, EventArgs e)
        {
            Log(this.EventArgString);
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
                if(logFileIsEnabled)
                    LogUtils.LogToFileAppFinished();
            }
        }
   }
}