using System;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WindowHandler : WxControlHandler, IWindowHandler
    {
        private object? statusBar;

        public Action<HandledEventArgs<string>>? InputBindingCommandExecuted { get; set; }

        public Action<CancelEventArgs>? Closing { get; set; }

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

        public bool HasBorder
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

        public Action? StateChanged
        {
            get
            {
                return NativeControl.StateChanged;
            }

            set
            {
                NativeControl.StateChanged = value;
            }
        }

        public WindowStartLocation StartLocation
        {
            get
            {
                return NativeControl.WindowStartLocation;
            }

            set
            {
                NativeControl.WindowStartLocation = value;
            }
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

        public Window[] OwnedWindows
        {
            get
            {
                var result = NativeControl.OwnedWindows.Select(
                    x => ((WindowHandler)(WxControlHandler.NativeControlToHandler(x) ??
                    throw new Exception())).Control).ToArray();
                return result;
            }
        }

        public ModalResult ModalResult
        {
            get
            {
                return NativeControl.ModalResult;
            }

            set
            {
                NativeControl.ModalResult = value;
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
                if (Control.GetWindowKind() == WindowKind.Dialog)
                {
                    statusBar = value;
                    return;
                }

                SetStatusBar(statusBar, value);
                statusBar = value;
            }
        }

        public bool IsModal
        {
            get
            {
                return NativeControl.Modal;
            }
        }

        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        public new Window Control => (Window)base.Control;

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

        public ModalResult ShowModal(IWindow? owner)
        {
            NativeControl.ShowModal(WxApplicationHandler.WxWidget(owner));
            return ModalResult;
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
                if (!Control.Modal)
                    Control.Dispose();
            }
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeWindow((int)Control.GetWindowKind());
        }

        protected override void OnDetach()
        {
            NativeControl.Closing -= NativeControl_Closing;
            NativeControl.InputBindingCommandExecuted -= NativeControl_InputBindingCommandExecuted;

            base.OnDetach();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Closing += NativeControl_Closing;
            NativeControl.InputBindingCommandExecuted += NativeControl_InputBindingCommandExecuted;
        }

        private void SetStatusBar(object? oldValue, object? value)
        {
            var nc = NativeControl;
            
            if (value is StatusBar asStatusBar)
            {
                if ((value as IDisposableObject)?.IsDisposed ?? false)
                    throw new ObjectDisposedException(nameof(StatusBar));
                if (asStatusBar.Window is not null && asStatusBar.Window != Control)
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
                    handler.Window = window;
            }
        }

        private void NativeControl_InputBindingCommandExecuted(
            object? sender,
            Native.NativeEventArgs<Native.CommandEventData> e)
        {
            if(InputBindingCommandExecuted is not null)
            {
                HandledEventArgs<string> args = new(e.Data.managedCommandId);
                InputBindingCommandExecuted.Invoke(args);
                e.Handled = args.Handled;
            }
        }

        public void AddInputBinding(InputBinding value)
        {
            var keyBinding = (KeyBinding)value;
            NativeControl.AddInputBinding(
                keyBinding.ManagedCommandId,
                keyBinding.Key,
                keyBinding.Modifiers);
        }

        public void RemoveInputBinding(InputBinding item)
        {
            NativeControl.RemoveInputBinding(item.ManagedCommandId);
        }

        public void SetOwner(Window? owner)
        {
            var newOwner = (owner as IControl)?.NativeControl as UI.Native.Control;
            var oldOwner = NativeControl.ParentRefCounted;
            if (newOwner == oldOwner)
                return;

            oldOwner?.RemoveChild(NativeControl);

            if (newOwner == null)
                return;

            newOwner.AddChild(NativeControl);
        }

        public void SetIcon(IconSet? value)
        {
            NativeControl.Icon = (UI.Native.IconSet?)value?.Handler;
        }

        public void SetMenu(object? value)
        {
            NativeControl.Menu = (value as IControl)?.NativeControl as Native.MainMenu;
        }

        private void NativeControl_Closing(object? sender, CancelEventArgs e)
        {
            Closing?.Invoke(e);
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