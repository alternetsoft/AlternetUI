using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods and properties to manage an application, such as methods to start and stop an application,
    /// and properties to get information about an application.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : IDisposable
    {
        private static Application? current;
        private readonly List<Window> windows = new List<Window>();
        private volatile bool isDisposed;

        private Native.Application nativeApplication;
        
        private VisualTheme visualTheme = StockVisualThemes.Native;

        private KeyboardInputProvider keyboardInputProvider;
        private MouseInputProvider mouseInputProvider;

        internal event EventHandler? Idle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
            nativeApplication = new Native.Application();
            nativeApplication.Idle += NativeApplication_Idle;
            current = this;

            keyboardInputProvider = new KeyboardInputProvider(nativeApplication.Keyboard);
            mouseInputProvider = new MouseInputProvider(nativeApplication.Mouse);
        }

        internal Native.Keyboard NativeKeyboard => nativeApplication.Keyboard;
        internal Native.Mouse NativeMouse => nativeApplication.Mouse;

        private void NativeApplication_Idle(object? sender, EventArgs e) => Idle?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when the <see cref="VisualTheme"/> property changes.
        /// </summary>
        public event EventHandler? VisualThemeChanged;

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
        }

        internal void RegisterWindow(Window window)
        {
            windows.Add(window);
        }

        internal void UnregisterWindow(Window window)
        {
            windows.Remove(window);
        }

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

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}