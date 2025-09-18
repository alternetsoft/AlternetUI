using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to forms handling
    /// </summary>
    public static class FormUtils
    {
        private static WeakReferenceValue<Window> phantomWindow = new();

        static FormUtils()
        {
        }

        /// <summary>
        /// Gets a hidden, borderless window positioned off-screen.
        /// If such a window is not already created, it creates one.
        /// </summary>
        /// <remarks>This phantom window is not shown in the taskbar and is intended for scenarios where
        /// an invisible or off-screen window is required, such as for background
        /// processing or interoperation
        /// purposes.</remarks>
        /// <returns>A <see cref="Window"/> instance configured with minimal dimensions, no border,
        /// and positioned outside the visible screen area.</returns>
        public static Window GetPhantomWindow()
        {
            if(phantomWindow.Value == null)
                phantomWindow.Value = CreatePhantomWindow();
            return phantomWindow.Value;
        }

        /// <summary>
        /// Creates a hidden, borderless window positioned off-screen.
        /// </summary>
        /// <remarks>This method creates a window with minimal dimensions and ensures it does not appear
        /// in the taskbar. The window is positioned off-screen and is intended for scenarios
        /// where a window handle is
        /// required without displaying the window to the user.
        /// Use <see cref="GetPhantomWindow"/> to retrieve a singleton instance of such a window.
        /// </remarks>
        /// <returns>A <see cref="Window"/> instance configured as a hidden, off-screen window.</returns>
        public static Window CreatePhantomWindow()
        {
            var window = new Window
            {
                Width = 1,
                Height = 1,
                ShowInTaskbar = false,
                HasBorder = false,
                StartLocation = WindowStartLocation.Manual,
                Location = new PointD(-32000, -32000),
            };

            window.HandleNeeded();

#pragma warning disable
            var a = window.Handler.GetHandle();
#pragma warning restore

            return window;
        }

        /// <summary>
        /// Executes the specified action on each window in the application.
        /// </summary>
        /// <param name="action">The action to execute on each window.</param>
        public static void ForEach(Action<Window> action)
        {
            var windows = App.Current.Windows.ToArray();

            foreach (var window in windows)
            {
                action(window);
            }
        }

        /// <summary>
        /// Invalidates all windows in the application, causing them to be redrawn.
        /// </summary>
        public static void InvalidateAll()
        {
            ForEach((window) => window.Invalidate());
        }

        /// <summary>
        /// Closes all windows using the specified close action. If <paramref name="action"/>
        /// is not specified, windows are disposed.
        /// </summary>
        public static void CloseOtherWindows(
            WindowCloseAction? action = WindowCloseAction.Dispose,
            Window? exceptWindow = null)
        {
            action ??= WindowCloseAction.Dispose;

            var windows = App.Current.Windows.ToArray();

            foreach (var window in windows)
            {
                if (window.GetType() == exceptWindow?.GetType())
                    continue;
                window.Close(action);
                App.DoEvents();
            }
        }
    }
}
