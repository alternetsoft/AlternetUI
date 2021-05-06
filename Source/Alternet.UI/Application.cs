using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Alternet.UI
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class Application : Component
    {
        private static Application? current;
        private readonly List<Window> windows = new List<Window>();
        private volatile bool isDisposed;

        private Native.Application nativeApplication;

        private VisualTheme visualTheme = StockVisualThemes.Native;

        public Application()
        {
            nativeApplication = new Native.Application();
            current = this;
        }

        public event EventHandler? VisualThemeChanged;

        public static Application Current
        {
            get
            {
                // todo: maybe make it thread static?
                // todo: maybe move this to native?
                return current ?? throw new InvalidOperationException(ErrorMessages.CurrentApplicationIsNotSet);
            }
        }

        public IReadOnlyList<Window> Windows => windows;

        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

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

        private void OnVisualThemeChanged()
        {
            foreach (var window in Windows)
                window.RecreateAllHandlers();
        }

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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!IsDisposed)
            {
                nativeApplication.Dispose();
                nativeApplication = null!;

                current = null;

                IsDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(null);
        }
    }
}