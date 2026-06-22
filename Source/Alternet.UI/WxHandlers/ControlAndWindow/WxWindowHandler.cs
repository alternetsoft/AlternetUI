using System;
using System.ComponentModel;
using System.Linq;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxWindowHandler : WxControlHandler, IWindowHandler
    {
        private WeakReferenceValue<DisposableObject> savedMenuRef = new();

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
            get => NativeControl.GetTitle().ToString();
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

        public override void OnBeforeHandleDestroyed()
        {
            if (Control is not null)
            {
                if (Control.Menu is not null)
                {
                    savedMenuRef.Value = Control.Menu;
                    Control.SetMenu(null, performLayout: false);
                }
                else
                {
                    savedMenuRef.Value = null;
                }
            }

            base.OnBeforeHandleDestroyed();
        }

        public override void OnHandleCreated()
        {
            base.OnHandleCreated();

            if (savedMenuRef.Value is null)
                return;

            Control?.DoInsideLayout(() =>
                {
                    Control.SetMenu(savedMenuRef.Value, performLayout: true);
                    savedMenuRef.Value = null;
                });
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeWindow((int)(Control?.GetWindowKind() ?? WindowKind.Control));
        }

        public void SetIcon(IconSet? value)
        {
            NativeControl.Icon = (UI.Native.IconSet?)value?.Handler;
        }

        public void SetMinSize(SizeD size)
        {
            NativeControl.SetMinSize(size);
        }

        public void SetMaxSize(SizeD size)
        {
            NativeControl.SetMaxSize(size);
        }

        protected override void DisposeManaged()
        {
            if (Control is not null)
                MenuUtils.Factory?.SetMainMenu(Control, null);
            base.DisposeManaged();
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