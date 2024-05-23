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
        internal Native.Application nativeApplication;

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
            nativeApplication.SetTopWindow(WxPlatform.WxWidget(window));
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

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            nativeApplication.Idle -= NativeApplication_Idle;
            nativeApplication.LogMessage -= NativeApplication_LogMessage;
            keyboardInputProvider.Dispose();
            mouseInputProvider.Dispose();
            nativeApplication.Dispose();
            nativeApplication = null!;

            BaseApplication.Current = null!;
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
   }
}