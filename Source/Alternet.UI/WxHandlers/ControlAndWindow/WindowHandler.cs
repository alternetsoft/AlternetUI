using System;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WindowHandler : WxControlHandler, IWindowHandler
    {
        private object? statusBar;

        public override bool VisibleOnScreen
        {
            get
            {
                return Visible;
            }
        }

        public bool ShowInTaskbar
        {
            get => NativeControl.ShowInTaskbar;
            set => NativeControl.ShowInTaskbar = value;
        }

        public bool MaximizeEnabled
        {
            get => NativeControl.MaximizeEnabled;
            set => NativeControl.MaximizeEnabled = value;
        }

        public bool MinimizeEnabled
        {
            get => NativeControl.MinimizeEnabled;
            set => NativeControl.MinimizeEnabled = value;
        }

        public bool CloseEnabled
        {
            get => NativeControl.CloseEnabled;
            set => NativeControl.CloseEnabled = value;
        }

        public bool AlwaysOnTop
        {
            get => NativeControl.AlwaysOnTop;
            set => NativeControl.AlwaysOnTop = value;
        }

        public bool IsToolWindow
        {
            get => NativeControl.IsToolWindow;
            set => NativeControl.IsToolWindow = value;
        }

        public bool Resizable
        {
            get => NativeControl.Resizable;
            set => NativeControl.Resizable = value;
        }

        public override bool HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        public bool HasTitleBar
        {
            get => NativeControl.HasTitleBar;
            set => NativeControl.HasTitleBar = value;
        }

        public bool HasSystemMenu
        {
            get => NativeControl.HasSystemMenu;
            set => NativeControl.HasSystemMenu = value;
        }

        public string Title
        {
            get => NativeControl.Title;
            set => NativeControl.Title = value;
        }

        public WindowState State
        {
            get
            {
                return NativeControl.State;
            }

            set
            {
                NativeControl.State = value;
            }
        }

        public object? StatusBar
        {
            get
            {
                return statusBar;
            }

            set
            {
                if (Control is null)
                    return;
                SetStatusBar(statusBar, value);
                statusBar = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        public new Window? Control => (Window?)base.Control;

        public new Native.Window NativeControl => (Native.Window)base.NativeControl;

        public bool IsPopupWindow
        {
            get
            {
                return NativeControl.IsPopupWindow;
            }

            set
            {
                NativeControl.IsPopupWindow = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return NativeControl.IsActive;
            }
        }

        public static void SetDefaultBounds(RectD defaultBounds)
        {
            Native.Window.SetDefaultBounds(defaultBounds);
        }

        public void SetIsPopupWindow(bool value)
        {
            NativeControl.IsPopupWindow = value;
        }

        public void Activate()
        {
            NativeControl.Activate();
        }

        public void Close()
        {
            if (NativeControlCreated)
                NativeControl.Close();
            else
            {
                if (Control is null)
                    return;

                if (!Control.Modal)
                    Control.Dispose();
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeWindow((int)(Control?.GetWindowKind() ?? WindowKind.Control));
        }

        private void SetStatusBar(object? oldValue, object? value)
        {
            var nc = NativeControl;
            
            if (value is StatusBar asStatusBar)
            {
                if ((value as IDisposableObject)?.IsDisposed ?? false)
                    throw new ObjectDisposedException(nameof(StatusBar));
                if (asStatusBar.AttachedTo is not null && asStatusBar.AttachedTo != Control)
                {
                    throw new Exception("Object is already attached to the window");
                }
            }

            if (oldValue is StatusBar asStatusBar2)
            {
                SetWindow(asStatusBar2, null);
                var oldHandle = nc.WxStatusBar;
                if (oldHandle != default)
                    Native.WxStatusBarFactory.DeleteStatusBar(oldHandle);
            }

            if (value is StatusBar asStatusBar3)
            {
                SetWindow(asStatusBar3, Control);
            }
            else
                nc.WxStatusBar = default;

            void SetWindow(StatusBar sb, Window? window)
            {
                if (sb.Handler is StatusBarHandler handler)
                    handler.AttachedTo = window;
            }
        }

        public void SetIcon(IconSet? value)
        {
            NativeControl.Icon = (UI.Native.IconSet?)value?.Handler;
        }

        public void SetMenu(object? value)
        {
            if(value is MainMenu asMainMenu)
            {
                NativeControl.Menu = asMainMenu.Handler as Native.MainMenu;
            }
            else
            {
                NativeControl.Menu = null;
            }
        }

        public void SetMinSize(SizeD size)
        {
            NativeControl.SetMinSize(size);
        }

        public void SetMaxSize(SizeD size)
        {
            NativeControl.SetMaxSize(size);
        }

        private class NativeWindow : Native.Window
        {
            public NativeWindow(int kind)
            {
                SetNativePointer(NativeApi.Window_CreateEx_(kind));
            }

            public NativeWindow(IntPtr nativePointer)
                : base(nativePointer)
            {
            }
        }
    }
}