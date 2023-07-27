using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties to manage an application, such as methods to start and stop an application,
    /// and properties to get information about an application.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : IDisposable
    {
        private static bool terminating = false;
        private static Application? current;
        private readonly List<Window> windows = new ();
        private UnhandledExceptionMode unhandledExceptionMode;
        private volatile bool isDisposed;

        private Native.Application nativeApplication;

        private VisualTheme visualTheme = StockVisualThemes.Native;

        private readonly KeyboardInputProvider keyboardInputProvider;
        private readonly MouseInputProvider mouseInputProvider;

        private ThreadExceptionEventHandler? threadExceptionHandler;

        private bool inOnThreadException;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
            nativeApplication = new Native.Application();

            SynchronizationContext.InstallIfNeeded();

            nativeApplication.Idle += NativeApplication_Idle;
            nativeApplication.Name = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess()?.MainModule?.FileName!);
            current = this;

            keyboardInputProvider = new KeyboardInputProvider(nativeApplication.Keyboard);
            mouseInputProvider = new MouseInputProvider(nativeApplication.Mouse);

            if (SupressDiagnostics)
                Native.Application.SuppressDiagnostics(-1);
        }

        /// <summary>
        /// Allows to suppress some debug messages.
        /// </summary>
        /// <remarks>
        ///  Currently used to suppress GTK messages under Linux. Default value is true.
        /// </remarks>
        public static bool SupressDiagnostics { get; set; } = true;

        /// <summary>
        /// Gets whether <see cref="Run"/> method execution is finished.
        /// </summary>
        public static bool Terminating { get => terminating; }

        /// <summary>
        ///  Occurs when an untrapped thread exception is thrown.
        /// </summary>
        /// <remarks>
        /// This event allows your application to handle otherwise unhandled exceptions that occur in UI threads. Attach
        /// your event handler to the <see cref="ThreadException"/> event to deal with these exceptions, which will
        /// leave your application in an unknown state. Where possible, exceptions should be handled by a structured
        /// exception handling block. You can change whether this callback is used for unhandled Windows Forms thread
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

        /// <summary>
        /// Occurs when the <see cref="VisualTheme"/> property changes.
        /// </summary>
        public event EventHandler? VisualThemeChanged;

        public event EventHandler? Idle;

        /// <summary>
        /// Gets the <see cref="Application"/> object for the currently runnning application.
        /// </summary>
        public static Application Current
        {
            get
            {
                // todo: maybe make it thread static?
                // todo: maybe move this to native?
                return current ?? throw new InvalidOperationException(ErrorMessages.CurrentApplicationIsNotSet);
            }
        }

        /// <summary>
        /// Gets the instantiated windows in an application.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{Window}"/> that contains references to all window objects in the current application.</value>
        public IReadOnlyList<Window> Windows => windows;

        /// <summary>
        /// Gets whether <see cref="Dispose(bool)"/> has been called.
        /// </summary>
        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        /// <summary>
        /// Gets or sets a <see cref="UI.VisualTheme"/> that is used by UI controls in the application.
        /// </summary>
        public VisualTheme VisualTheme
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
        public string Name
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
        public string DisplayName
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
        public string AppClassName
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
        public string VendorName
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
        public string VendorDisplayName
        {
            get => nativeApplication.VendorDisplayName;
            set => nativeApplication.VendorDisplayName = value;
        }

        internal Native.Clipboard NativeClipboard => nativeApplication.Clipboard;

        internal Native.Keyboard NativeKeyboard => nativeApplication.Keyboard;

        internal Native.Mouse NativeMouse => nativeApplication.Mouse;

        internal bool InUixmlPreviewerMode
        {
            get => nativeApplication.InUixmlPreviewerMode;
            set => nativeApplication.InUixmlPreviewerMode = value;
        }

        internal bool InvokeRequired => nativeApplication.InvokeRequired;

        /// <summary>
        /// Instructs the application how to respond to unhandled exceptions.
        /// </summary>
        /// <param name="mode">An <see cref="UnhandledExceptionMode"/> value describing how the application should
        /// behave if an exception is thrown without being caught.</param>
        public void SetUnhandledExceptionMode(UnhandledExceptionMode mode)
        {
            unhandledExceptionMode = mode;
        }

        /// <summary>
        /// Informs all message pumps that they must terminate, and then closes all application windows after the messages have been processed.
        /// </summary>
        public void Exit()
        {
            nativeApplication.Exit();
        }

        /// <summary>
        /// Starts an application UI event loop and makes the specified window visible.
        /// Begins running a UI event processing loop on the current thread.
        /// </summary>
        /// <param name="window">A <see cref="Window"/> that opens automatically when an application starts.</param>
        /// <remarks>Typically, the main function of an application calls this method and passes to it the main window of the application.</remarks>
        public void Run(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));
            CheckDisposed();
            window.Show();
            nativeApplication.Run(((NativeWindowHandler)window.Handler).NativeControl);
            SynchronizationContext.Uninstall();
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

        internal void OnThreadException(Exception exception)
        {
            if (inOnThreadException)
                return;

            inOnThreadException = true;
            try
            {
                if (unhandledExceptionMode == UnhandledExceptionMode.ThrowException || System.Diagnostics.Debugger.IsAttached)
                    throw exception;

                if (threadExceptionHandler is not null)
                {
                    threadExceptionHandler(Thread.CurrentThread, new ThreadExceptionEventArgs(exception));
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
            windows.Remove(window);
        }

        internal void BeginInvoke(Action action)
        {
            nativeApplication.BeginInvoke(action);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    nativeApplication.Idle -= NativeApplication_Idle;
                    keyboardInputProvider.Dispose();
                    mouseInputProvider.Dispose();
                    nativeApplication.Dispose();
                    nativeApplication = null!;

                    current = null;
                }

                IsDisposed = true;
            }
        }

        private void NativeApplication_Idle(object? sender, EventArgs e) => Idle?.Invoke(this, EventArgs.Empty);

        private void OnVisualThemeChanged()
        {
            foreach (var window in Windows)
                window.RecreateAllHandlers();
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }
    }
}