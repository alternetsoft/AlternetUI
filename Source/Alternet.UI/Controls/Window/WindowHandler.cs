using System;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WindowHandler : ControlHandler
    {
        public static Window? ActiveWindow
        {
            get
            {
                var activeWindow = Native.Window.ActiveWindow;
                if (activeWindow == null)
                    return null;

                var handler = NativeControlToHandler(activeWindow) ??
                    throw new InvalidOperationException();
                return ((WindowHandler)handler).Control;
            }
        }

        /// <summary>
        /// Gets a <see cref="Window"/> this handler provides the implementation for.
        /// </summary>
        public new Window Control => (Window)base.Control;

        public new Native.Window NativeControl => (Native.Window)base.NativeControl!;

        /// <summary>
        /// Gets an array of <see cref="Window"/> objects that represent
        /// all windows that are owned by this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> array that represents the owned windows for
        /// this window.
        /// </value>
        /// <remarks>
        /// This property returns an array that contains all windows that are
        /// owned by this window. To make a window owned by another window, se
        /// the <see cref="Window.Owner"/> property.
        /// </remarks>
        public Window[] OwnedWindows
        {
            get
            {
                return NativeControl.OwnedWindows.Select(
                    x => ((WindowHandler)(NativeControlToHandler(x) ??
                    throw new Exception())).Control).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the position of the window when first shown.
        /// </summary>
        /// <value>A <see cref="WindowStartLocation"/> that represents the
        /// starting position of the window.</value>
        /// <remarks>
        /// This property enables you to set the starting position of the window
        /// when it is first shown.
        /// This property should be set before the window is shown.
        /// </remarks>
        public WindowStartLocation StartLocation
        {
            get
            {
                return (WindowStartLocation)NativeControl.WindowStartLocation;
            }

            set
            {
                NativeControl.WindowStartLocation = (Native.WindowStartLocation)value;
            }
        }

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
            Control.StateChanged -= ApplyState;
            Control.IconChanged -= ApplyIcon;
            Control.MenuChanged -= ApplyMenu;
            Control.ToolbarChanged -= ApplyToolbar;
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
            ApplyState(null, EventArgs.Empty);
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
            Control.StateChanged += ApplyState;
            Control.IconChanged += ApplyIcon;
            Control.MenuChanged += ApplyMenu;
            Control.ToolbarChanged += ApplyToolbar;
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

        private void NativeControl_StateChanged()
        {
            Control.State = (WindowState)NativeControl.State;
        }

        private void ApplyState(object? sender, EventArgs e)
        {
            NativeControl.State = (Native.WindowState)Control.State;
        }

        private void ApplyIcon(object? sender, EventArgs e)
        {
            NativeControl.Icon = Control.Icon?.NativeIconSet;
        }

        private void ApplyMenu(object? sender, EventArgs e)
        {
            NativeControl.Menu =
                (Control.Menu?.Handler as MainMenuHandler)?.NativeControl;
        }

        private void ApplyToolbar(object? sender, EventArgs e)
        {
            NativeControl.Toolbar =
                (Control.Toolbar?.Handler as NativeToolbarHandler)?.NativeControl;
        }

        private void ApplyStatusBar(object? sender, EventArgs e)
        {
            Control.StatusBar?.RecreateWidget();
        }

        private void ApplyOwner(object? sender, EventArgs e)
        {
            var newOwner = Control.Owner?.Handler?.NativeControl;
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

        private void NativeControl_SizeChanged()
        {
            Control.RaiseSizeChanged(EventArgs.Empty);
            Application.AddIdleTask(() => { Control.PerformLayout(); });
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