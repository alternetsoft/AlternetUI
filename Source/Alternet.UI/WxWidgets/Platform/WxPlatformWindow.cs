using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxPlatformWindow : NativeWindow
    {
        /// <inheritdoc/>
        public override Window? GetActiveWindow()
        {
            var activeWindow = Native.Window.ActiveWindow;
            if (activeWindow == null)
                return null;

            var handler = WxControlHandler.NativeControlToHandler(activeWindow) ??
                throw new InvalidOperationException();
            return ((WindowHandler)handler).Control;
        }

        /// <inheritdoc/>
        public override Window[] GetOwnedWindows(IWindow window)
        {
            var nc = (UI.Native.Window)window.NativeControl;
            var result = nc.OwnedWindows.Select(
                x => ((WindowHandler)(WxControlHandler.NativeControlToHandler(x) ??
                throw new Exception())).Control).ToArray();
            return result;
        }

        /// <inheritdoc/>
        public override ModalResult ShowModal(IWindow window, IWindow? owner)
        {
            ((UI.Native.Window)window.NativeControl).ShowModal(WxPlatformControl.WxWidget(owner));
            return GetModalResult(window);
        }

        /// <inheritdoc/>
        public override ModalResult GetModalResult(IWindow window)
        {
            return (ModalResult)((UI.Native.Window)window.NativeControl).ModalResult;
        }

        /// <inheritdoc/>
        public override void SetModalResult(IWindow window, ModalResult value)
        {
            ((UI.Native.Window)window.NativeControl).ModalResult = (Native.ModalResult)value;
        }

        /// <inheritdoc/>
        public override bool GetModal(IWindow window)
        {
            return ((UI.Native.Window)window.NativeControl).Modal;
        }

        /// <inheritdoc/>
        public override void Close(IWindow window)
        {
            var handler = ((Window)window).Handler;

            ((WindowHandler)handler).Close();
        }

        /// <inheritdoc/>
        public override void SetDefaultBounds(RectD defaultBounds)
        {
            Native.Window.SetDefaultBounds(defaultBounds);
        }

        /// <inheritdoc/>
        public override bool IsActive(IWindow window)
        {
            return ((UI.Native.Window)window.NativeControl).IsActive;
        }

        /// <inheritdoc/>
        public override void SetIsPopupWindow(IWindow window, bool value)
        {
            ((UI.Native.Window)window.NativeControl).IsPopupWindow = value;
        }

        /// <inheritdoc/>
        public override WindowStartLocation GetStartLocation(IWindow window)
        {
            return (WindowStartLocation)((UI.Native.Window)window.NativeControl).WindowStartLocation;
        }

        /// <inheritdoc/>
        public override void SetStartLocation(IWindow window, WindowStartLocation value)
        {
            ((UI.Native.Window)window.NativeControl).WindowStartLocation
                = (Native.WindowStartLocation)value;
        }

        /// <inheritdoc/>
        public override WindowState GetState(IWindow window)
        {
            return (WindowState)((UI.Native.Window)window.NativeControl).State;
        }

        /// <inheritdoc/>
        public override void SetState(IWindow window, WindowState value)
        {
            ((UI.Native.Window)window.NativeControl).State = (Native.WindowState)value;
        }

        /// <inheritdoc/>
        public override void Activate(IWindow window)
        {
            ((UI.Native.Window)window.NativeControl).Activate();
        }

        /// <inheritdoc/>
        public override void SetStatusBar(
            IWindow window,
            FrameworkElement? oldValue,
            FrameworkElement? value)
        {
            var nc = (UI.Native.Window)window.NativeControl;
            var thisWindow = window as Window;

            if (value is StatusBar asStatusBar)
            {
                if (value.IsDisposed)
                    throw new ObjectDisposedException(nameof(StatusBar));
                if (asStatusBar.Window is not null && asStatusBar.Window != thisWindow)
                {
                    throw new Exception("Object is already attached to the window");
                }
            }

            if (oldValue is StatusBar asStatusBar2)
            {
                asStatusBar2.Window = null;
                var oldHandle = nc.WxStatusBar;
                if (oldHandle != default)
                    Native.WxStatusBarFactory.DeleteStatusBar(oldHandle);
            }

            if (value is StatusBar asStatusBar3)
                asStatusBar3.Window = thisWindow;
            else
                nc.WxStatusBar = default;
        }
    }
}
