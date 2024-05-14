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
    public partial class Application : BaseApplication, IDisposable
    {
        internal static readonly Destructor MyDestructor = new();

        internal Native.Application nativeApplication;

        private static IconSet? icon;
        private static bool inOnThreadException;

        private readonly KeyboardInputProvider keyboardInputProvider;
        private readonly MouseInputProvider mouseInputProvider;

        static Application()
        {
            WxPlatform.Initialize();
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
            BaseApplication.Current = this;

            keyboardInputProvider = new KeyboardInputProvider(
                nativeApplication.Keyboard);
            mouseInputProvider = new MouseInputProvider(nativeApplication.Mouse);

            if (SupressDiagnostics)
                Native.Application.SuppressDiagnostics(-1);

#if DEBUG
            WebBrowser.CrtSetDbgFlag(0);
#endif

            Keyboard.PrimaryDevice = InputManager.UnsecureCurrent.PrimaryKeyboardDevice;
            Mouse.PrimaryDevice = InputManager.UnsecureCurrent.PrimaryMouseDevice;

            Initialized = true;
            Window.UpdateDefaultFont();
        }

        /// <summary>
        /// Occurs before native debug message needs to be displayed.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs>? BeforeNativeLogMessage;

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
        /// Gets whether execution is inside the <see cref="Run"/> method.
        /// </summary>
        public static bool IsRunning { get; internal set; }

        /// <summary>
        /// Gets the <see cref="Application"/> object for the currently
        /// runnning application.
        /// </summary>
        public static new Application Current
        {
            get
            {
                return (Application)BaseApplication.Current;
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

        /// <inheritdoc/>
        public override bool InUixmlPreviewerMode
        {
            get => nativeApplication.InUixmlPreviewerMode;
            set => nativeApplication.InUixmlPreviewerMode = value;
        }

        internal Native.Clipboard NativeClipboard => nativeApplication.Clipboard;

        internal Native.Keyboard NativeKeyboard => nativeApplication.Keyboard;

        internal Native.Application NativeApplication => nativeApplication;

        internal Native.Mouse NativeMouse => nativeApplication.Mouse;

        internal string EventArgString => NativeApplication.EventArgString;

        /// <inheritdoc/>
        protected override bool InvokeRequired => nativeApplication.InvokeRequired;

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
        /// Executes <see cref="BaseApplication.IdleLog"/> using <see cref="Invoke(Action?)"/>.
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
        /// Informs all message pumps that they must terminate, and then closes
        /// all application windows after the messages have been processed.
        /// </summary>
        public static void Exit()
        {
            if(HasApplication)
                Current.nativeApplication.Exit();
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
        /// Checks whether there are any pending events in the queue.
        /// </summary>
        /// <returns><c>true</c> if there are any pending events in the queue,
        /// <c>false</c> otherwise.</returns>
        public bool HasPendingEvents()
        {
            return nativeApplication.HasPendingEvents();
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
                this.MainWindow = window ?? throw new ArgumentNullException(nameof(window));
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
                this.MainWindow = null;
            }
            finally
            {
                Terminating = true;
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

                if (ThreadExceptionAssigned)
                {
                    var args = new ThreadExceptionEventArgs(exception);
                    RaiseThreadException(Thread.CurrentThread, args);
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
            nativeApplication.SetTopWindow(WxPlatformControl.WxWidget(window));
        }

        internal void WakeUpIdle()
        {
            nativeApplication.WakeUpIdle();
        }

        internal void RecreateAllHandlers()
        {
            foreach (var window in Windows)
                window.RecreateAllHandlers();
        }

        /// <inheritdoc/>
        protected override void BeginInvoke(Action action)
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
            if (!IsDisposed)
            {
                if (disposing)
                {
                    nativeApplication.Idle -= NativeApplication_Idle;
                    nativeApplication.LogMessage -= NativeApplication_LogMessage;
                    keyboardInputProvider.Dispose();
                    mouseInputProvider.Dispose();
                    nativeApplication.Dispose();
                    nativeApplication = null!;

                    BaseApplication.Current = null!;
                }

                IsDisposed = true;
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

            RaiseIdle();
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
                if (LogFileIsEnabled)
                    LogUtils.LogToFileAppFinished();
            }
        }
    }
}