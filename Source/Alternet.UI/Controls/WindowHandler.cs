using System;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WindowHandler : WxControlHandler
    {
        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        public new Window Control => (Window)base.Control;

        public new Native.Window NativeControl => (Native.Window)base.NativeControl!;

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <remarks>
        /// When a window is closed, all resources created within the object are
        /// closed and the window is disposed.
        /// You can prevent the closing of a window at run time by handling the
        /// <see cref="Window.Closing"/> event and
        /// setting the <c>Cancel</c> property of the
        /// <see cref="CancelEventArgs"/> passed as a parameter to your event handler.
        /// If the window you are closing is the last open window of your
        /// application, your application ends.
        /// The window is not disposed on <see cref="Close"/> is when you have
        /// displayed the window using <see cref="DialogWindow.ShowModal()"/>.
        /// In this case, you will need to call
        /// <see cref="IDisposable.Dispose"/> manually.
        /// </remarks>
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
            NativeControl.SizeChanged = null;
            NativeControl.LocationChanged = null;
            NativeControl.StateChanged = null;
            NativeControl.Closing -= NativeControl_Closing;
            NativeControl.InputBindingCommandExecuted -= NativeControl_InputBindingCommandExecuted;

            Control.OwnerChanged -= ApplyOwner;
            Control.TitleChanged -= ApplyTitle;
            Control.ShowInTaskbarChanged -= ApplyShowInTaskbar;
            Control.MinimizeEnabledChanged -= ApplyMinimizeEnabled;
            Control.MaximizeEnabledChanged -= ApplyMaximizeEnabled;
            Control.CloseEnabledChanged -= ApplyCloseEnabled;
            Control.AlwaysOnTopChanged -= ApplyAlwaysOnTop;
            Control.IsToolWindowChanged -= ApplyIsToolWindow;
            Control.ResizableChanged -= ApplyResizable;
            Control.HasBorderChanged -= ApplyHasBorder;
            Control.HasTitleBarChanged -= ApplyHasTitleBar;
            Control.HasSystemMenuChanged -= ApplyHasSystemMenu;
            Control.IconChanged -= ApplyIcon;
            Control.MenuChanged -= ApplyMenu;
            Control.ToolBarChanged -= ApplyToolbar;
            Control.StatusBarChanged -= ApplyStatusBar;

            Control.InputBindings.ItemInserted -= InputBindings_ItemInserted;
            Control.InputBindings.ItemRemoved -= InputBindings_ItemRemoved;

            base.OnDetach();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyTitle(null, EventArgs.Empty);
            ApplyShowInTaskbar(null, EventArgs.Empty);
            ApplyOwner(null, EventArgs.Empty);
            ApplyMaximizeEnabled(null, EventArgs.Empty);
            ApplyMinimizeEnabled(null, EventArgs.Empty);
            ApplyCloseEnabled(null, EventArgs.Empty);
            ApplyAlwaysOnTop(null, EventArgs.Empty);
            ApplyIsToolWindow(null, EventArgs.Empty);
            ApplyResizable(null, EventArgs.Empty);
            ApplyHasBorder(null, EventArgs.Empty);
            ApplyHasTitleBar(null, EventArgs.Empty);
            ApplyHasSystemMenu(null, EventArgs.Empty);
            ApplyIcon(null, EventArgs.Empty);
            ApplyMenu(null, EventArgs.Empty);
            ApplyToolbar(null, EventArgs.Empty);
            ApplyStatusBar(null, EventArgs.Empty);
            ApplyInputBindings(null, EventArgs.Empty);

            Control.TitleChanged += ApplyTitle;
            Control.ShowInTaskbarChanged += ApplyShowInTaskbar;
            Control.OwnerChanged += ApplyOwner;
            Control.MinimizeEnabledChanged += ApplyMinimizeEnabled;
            Control.MaximizeEnabledChanged += ApplyMaximizeEnabled;
            Control.CloseEnabledChanged += ApplyCloseEnabled;
            Control.AlwaysOnTopChanged += ApplyAlwaysOnTop;
            Control.IsToolWindowChanged += ApplyIsToolWindow;
            Control.ResizableChanged += ApplyResizable;
            Control.HasBorderChanged += ApplyHasBorder;
            Control.HasTitleBarChanged += ApplyHasTitleBar;
            Control.HasSystemMenuChanged += ApplyHasSystemMenu;
            Control.IconChanged += ApplyIcon;
            Control.MenuChanged += ApplyMenu;
            Control.ToolBarChanged += ApplyToolbar;
            Control.StatusBarChanged += ApplyStatusBar;

            Control.InputBindings.ItemInserted += InputBindings_ItemInserted;
            Control.InputBindings.ItemRemoved += InputBindings_ItemRemoved;

            NativeControl.Closing += NativeControl_Closing;
            NativeControl.SizeChanged = NativeControl_SizeChanged;
            NativeControl.LocationChanged = NativeControl_LocationChanged;
            NativeControl.StateChanged = NativeControl_StateChanged;
            NativeControl.InputBindingCommandExecuted += NativeControl_InputBindingCommandExecuted;
        }

        private void NativeControl_InputBindingCommandExecuted(
            object? sender,
            Native.NativeEventArgs<Native.CommandEventData> e)
        {
            var binding = Control.InputBindings.First(
                x => x.ManagedCommandId == e.Data.managedCommandId);

            e.Handled = false;

            var command = binding.Command;
            if (command == null)
                return;

            if (!command.CanExecute(binding.CommandParameter))
                return;

            command.Execute(binding.CommandParameter);
            e.Handled = true;
        }

        private void InputBindings_ItemInserted(object? sender, int index, InputBinding item)
        {
            AddInputBinding(item);

            Internal.InheritanceContextHelper.ProvideContextForObject(Control, item);
        }

        private void ApplyInputBindings(object? sender, EventArgs e)
        {
            foreach (var binding in Control.InputBindings)
                AddInputBinding(binding);
        }

        private void AddInputBinding(InputBinding value)
        {
            var keyBinding = (KeyBinding)value;
            NativeControl.AddInputBinding(
                keyBinding.ManagedCommandId,
                (Native.Key)keyBinding.Key,
                (Native.ModifierKeys)keyBinding.Modifiers);
        }

        private void InputBindings_ItemRemoved(object? sender, int index, InputBinding item)
        {
            Internal.InheritanceContextHelper.RemoveContextFromObject(Control, item);
            NativeControl.RemoveInputBinding(item.ManagedCommandId);
        }

        private void NativeControl_SizeChanged()
        {
            if (!BaseApplication.IsLinuxOS)
                Control.PerformLayout();
            BaseApplication.AddIdleTask(() =>
            {
                if (Control.IsDisposed)
                    return;
                Control.PerformLayout();
            });
            Control.RaiseSizeChanged(EventArgs.Empty);
        }

        private void NativeControl_StateChanged()
        {
            NativeControl_SizeChanged();
        }

        private void ApplyIcon(object? sender, EventArgs e)
        {
            NativeControl.Icon = (UI.Native.IconSet?)Control.Icon?.NativeObject;
        }

        private void ApplyMenu(object? sender, EventArgs e)
        {
            NativeControl.Menu =
                (Control.Menu?.Handler as MainMenuHandler)?.NativeControl;
        }

        private void ApplyToolbar(object? sender, EventArgs e)
        {
            NativeControl.Toolbar =
                (Control.ToolBar?.Handler as ToolBarHandler)?.NativeControl;
        }

        private void ApplyStatusBar(object? sender, EventArgs e)
        {
            (Control.StatusBar as StatusBar)?.RecreateWidget();
        }

        private void ApplyOwner(object? sender, EventArgs e)
        {
            var newOwner = (Control.Owner as IControl)?.NativeControl as UI.Native.Control;
            var oldOwner = NativeControl.ParentRefCounted;
            if (newOwner == oldOwner)
                return;

            oldOwner?.RemoveChild(NativeControl);

            if (newOwner == null)
                return;

            newOwner.AddChild(NativeControl);
        }

        private void ApplyShowInTaskbar(object? sender, EventArgs e)
        {
            NativeControl.ShowInTaskbar = Control.ShowInTaskbar;
        }

        private void ApplyMinimizeEnabled(object? sender, EventArgs e)
        {
            NativeControl.MinimizeEnabled = Control.MinimizeEnabled;
        }

        private void ApplyMaximizeEnabled(object? sender, EventArgs e)
        {
            NativeControl.MaximizeEnabled = Control.MaximizeEnabled;
        }

        private void ApplyCloseEnabled(object? sender, EventArgs e)
        {
            NativeControl.CloseEnabled = Control.CloseEnabled;
        }

        private void ApplyAlwaysOnTop(object? sender, EventArgs e)
        {
            NativeControl.AlwaysOnTop = Control.TopMost;
        }

        private void ApplyHasSystemMenu(object? sender, EventArgs e)
        {
            NativeControl.HasSystemMenu = Control.HasSystemMenu;
        }

        private void ApplyIsToolWindow(object? sender, EventArgs e)
        {
            NativeControl.IsToolWindow = Control.IsToolWindow;
        }

        private void ApplyResizable(object? sender, EventArgs e)
        {
            NativeControl.Resizable = Control.Resizable;
        }

        private void ApplyHasBorder(object? sender, EventArgs e)
        {
            NativeControl.HasBorder = Control.HasBorder;
        }

        private void ApplyHasTitleBar(object? sender, EventArgs e)
        {
            NativeControl.HasTitleBar = Control.HasTitleBar;
        }

        private void NativeControl_LocationChanged()
        {
            Control.RaiseLocationChanged(EventArgs.Empty);
        }

        private void NativeControl_Closing(object? sender, CancelEventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            // todo: add close reason/force parameter (see wxCloseEvent.CanVeto()).
            var closingEventArgs = new WindowClosingEventArgs(e.Cancel);
            Control.RaiseClosing(closingEventArgs);
            if (closingEventArgs.Cancel)
            {
                e.Cancel = true;
                return;
            }

            Control.RaiseClosed(new WindowClosedEventArgs());
            if (!Control.Modal)
                Control.Dispose();
        }

        private void ApplyTitle(object? sender, EventArgs e)
        {
            NativeControl.Title = Control.Title;
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