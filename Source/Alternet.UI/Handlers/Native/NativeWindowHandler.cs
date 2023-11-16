using System;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeWindowHandler : WindowHandler
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
                return ((NativeWindowHandler)handler).Control;
            }
        }

        public new Native.Window NativeControl => (Native.Window)base.NativeControl!;

        public bool IsActive => NativeControl.IsActive;

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
                    x => ((NativeWindowHandler)(NativeControlToHandler(x) ??
                    throw new Exception())).Control).ToArray();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this window is displayed modally.
        /// </summary>
        public bool Modal => NativeControl.Modal;

        /// <summary>
        /// Gets or sets the modal result value, which is the value that is
        /// returned from the <see cref="ShowModal()"/> method.
        /// </summary>
        public ModalResult ModalResult
        {
            get
            {
                return (ModalResult)NativeControl.ModalResult;
            }

            set
            {
                NativeControl.ModalResult = (Native.ModalResult)value;
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

        public void Activate()
        {
            NativeControl.Activate();
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
        /// displayed the window using <see cref="ShowModal()"/>.
        /// In this case, you will need to call
        /// <see cref="IDisposable.Dispose"/> manually.
        /// </remarks>
        public void Close()
        {
            if (NativeControlCreated)
                NativeControl.Close();
            else
            {
                if (!Modal)
                    Control.Dispose();
            }
        }

        /// <inheritdoc/>
        public override void SetSizeToContent(WindowSizeToContentMode mode)
        {
            if (mode == WindowSizeToContentMode.None)
                return;

            var newSize = GetChildrenMaxPreferredSizePadded(
                new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (newSize != Size.Empty)
            {
                var currentSize = Control.ClientSize;
                switch (mode)
                {
                    case WindowSizeToContentMode.Width:
                        newSize.Height = currentSize.Height;
                        break;
                    case WindowSizeToContentMode.Height:
                        newSize.Width = currentSize.Width;
                        break;
                    case WindowSizeToContentMode.WidthAndHeight:
                        break;
                    default:
                        throw new Exception();
                }

                Control.ClientSize = newSize + new Size(1, 0);
                Control.ClientSize = newSize;
                Control.Refresh();
                NativeControl.SendSizeEvent();
            }
        }

        /// <summary>
        /// Opens a window and returns only when the newly opened window is closed.
        /// User interaction with all other windows in the application is
        /// disabled until the modal window is closed.
        /// </summary>
        public void ShowModal()
        {
            NativeControl.ShowModal();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Window();
        }

        protected override void OnDetach()
        {
            NativeControl.SizeChanged -= NativeControl_SizeChanged;
            NativeControl.LocationChanged -= NativeControl_LocationChanged;
            NativeControl.Closing -= Control_Closing;
            NativeControl.Activated -= NativeControl_Activated;
            NativeControl.Deactivated -= NativeControl_Deactivated;
            NativeControl.InputBindingCommandExecuted -= NativeControl_InputBindingCommandExecuted;

            Control.OwnerChanged -= Control_OwnerChanged;
            Control.TitleChanged -= Control_TitleChanged;
            Control.ShowInTaskbarChanged -= Control_ShowInTaskbarChanged;
            Control.MinimizeEnabledChanged -= Control_MinimizeEnabledChanged;
            Control.MaximizeEnabledChanged -= Control_MaximizeEnabledChanged;
            Control.CloseEnabledChanged -= Control_CloseEnabledChanged;
            Control.AlwaysOnTopChanged -= Control_AlwaysOnTopChanged;
            Control.IsToolWindowChanged -= Control_IsToolWindowChanged;
            Control.ResizableChanged -= Control_ResizableChanged;
            Control.HasBorderChanged -= Control_HasBorderChanged;
            Control.HasTitleBarChanged -= Control_HasTitleBarChanged;
            Control.HasSystemMenuChanged -= Control_HasSystemMenuChanged;
            Control.StateChanged -= Control_StateChanged;
            Control.IconChanged -= Control_IconChanged;
            Control.MenuChanged -= Control_MenuChanged;
            Control.ToolbarChanged -= Control_ToolbarChanged;
            Control.StatusBarChanged -= Control_StatusBarChanged;

            base.OnDetach();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyTitle();
            ApplyShowInTaskbar();
            ApplyOwner();
            ApplyMaximizeEnabled();
            ApplyMinimizeEnabled();
            ApplyCloseEnabled();
            ApplyAlwaysOnTop();
            ApplyIsToolWindow();
            ApplyResizable();
            ApplyHasBorder();
            ApplyHasTitleBar();
            ApplyHasSystemMenu();
            ApplyState();
            ApplyIcon();
            ApplyMenu();
            ApplyToolbar();
            ApplyStatusBar();
            ApplyInputBindings();

            Control.TitleChanged += Control_TitleChanged;

            Control.ShowInTaskbarChanged += Control_ShowInTaskbarChanged;
            Control.OwnerChanged += Control_OwnerChanged;
            Control.MinimizeEnabledChanged += Control_MinimizeEnabledChanged;
            Control.MaximizeEnabledChanged += Control_MaximizeEnabledChanged;
            Control.CloseEnabledChanged += Control_CloseEnabledChanged;
            Control.AlwaysOnTopChanged += Control_AlwaysOnTopChanged;
            Control.IsToolWindowChanged += Control_IsToolWindowChanged;
            Control.ResizableChanged += Control_ResizableChanged;
            Control.HasBorderChanged += Control_HasBorderChanged;
            Control.HasTitleBarChanged += Control_HasTitleBarChanged;
            Control.HasSystemMenuChanged += Control_HasSystemMenuChanged;

            Control.StateChanged += Control_StateChanged;
            Control.IconChanged += Control_IconChanged;
            Control.MenuChanged += Control_MenuChanged;
            Control.ToolbarChanged += Control_ToolbarChanged;
            Control.StatusBarChanged += Control_StatusBarChanged;

            Control.InputBindings.ItemInserted += InputBindings_ItemInserted;
            Control.InputBindings.ItemRemoved += InputBindings_ItemRemoved;

            NativeControl.Closing += Control_Closing;
            NativeControl.SizeChanged += NativeControl_SizeChanged;
            NativeControl.LocationChanged += NativeControl_LocationChanged;
            NativeControl.Activated += NativeControl_Activated;
            NativeControl.Deactivated += NativeControl_Deactivated;
            NativeControl.StateChanged += NativeControl_StateChanged;
            NativeControl.InputBindingCommandExecuted += NativeControl_InputBindingCommandExecuted;
        }

        private void Control_ToolbarChanged(object? sender, EventArgs e)
        {
            ApplyToolbar();
        }

        private void Control_StatusBarChanged(object? sender, EventArgs e)
        {
            ApplyStatusBar();
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

        private void ApplyInputBindings()
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

        private void Control_IconChanged(object? sender, EventArgs e)
        {
            ApplyIcon();
        }

        private void Control_MenuChanged(object? sender, EventArgs e)
        {
            ApplyMenu();
        }

        private void NativeControl_StateChanged(object? sender, EventArgs e)
        {
            Control.State = (WindowState)NativeControl.State;
        }

        private void Control_StateChanged(object? sender, EventArgs e)
        {
            ApplyState();
        }

        private void ApplyState()
        {
            NativeControl.State = (Native.WindowState)Control.State;
        }

        private void ApplyIcon()
        {
            NativeControl.Icon = Control.Icon?.NativeImageSet ?? null;
        }

        private void ApplyMenu()
        {
            NativeControl.Menu =
                (Control.Menu?.Handler as NativeMainMenuHandler)?.NativeControl ?? null;
        }

        private void ApplyToolbar()
        {
            NativeControl.Toolbar =
                (Control.Toolbar?.Handler as NativeToolbarHandler)?
                .NativeControl ?? null;
        }

        private void ApplyStatusBar()
        {
            Control.StatusBar?.RecreateWidget();
        }

        private void NativeControl_Deactivated(object? sender, EventArgs e)
        {
            Control.RaiseDeactivated();
        }

        private void NativeControl_Activated(object? sender, EventArgs e)
        {
            Control.RaiseActivated();
        }

        private void Control_AlwaysOnTopChanged(object? sender, EventArgs e)
        {
            ApplyAlwaysOnTop();
        }

        private void Control_IsToolWindowChanged(object? sender, EventArgs e)
        {
            ApplyIsToolWindow();
        }

        private void Control_ResizableChanged(object? sender, EventArgs e)
        {
            ApplyResizable();
        }

        private void Control_HasBorderChanged(object? sender, EventArgs e)
        {
            ApplyHasBorder();
        }

        private void Control_HasSystemMenuChanged(object? sender, EventArgs e)
        {
            ApplyHasSystemMenu();
        }

        private void Control_HasTitleBarChanged(object? sender, EventArgs e)
        {
            ApplyHasTitleBar();
        }

        private void Control_CloseEnabledChanged(object? sender, EventArgs e)
        {
            ApplyCloseEnabled();
        }

        private void Control_MaximizeEnabledChanged(object? sender, EventArgs e)
        {
            ApplyMaximizeEnabled();
        }

        private void Control_MinimizeEnabledChanged(object? sender, EventArgs e)
        {
            ApplyMinimizeEnabled();
        }

        private void ApplyOwner()
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

        private void Control_OwnerChanged(object? sender, EventArgs e)
        {
            ApplyOwner();
        }

        private void Control_ShowInTaskbarChanged(object? sender, EventArgs e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            ApplyShowInTaskbar();
        }

        private void ApplyShowInTaskbar()
        {
            NativeControl.ShowInTaskbar = Control.ShowInTaskbar;
        }

        private void ApplyMinimizeEnabled()
        {
            NativeControl.MinimizeEnabled = Control.MinimizeEnabled;
        }

        private void ApplyMaximizeEnabled()
        {
            NativeControl.MaximizeEnabled = Control.MaximizeEnabled;
        }

        private void ApplyCloseEnabled()
        {
            NativeControl.CloseEnabled = Control.CloseEnabled;
        }

        private void ApplyAlwaysOnTop()
        {
            NativeControl.AlwaysOnTop = Control.AlwaysOnTop;
        }

        private void ApplyHasSystemMenu()
        {
            NativeControl.HasSystemMenu = Control.HasSystemMenu;
        }

        private void ApplyIsToolWindow()
        {
            NativeControl.IsToolWindow = Control.IsToolWindow;
        }

        private void ApplyResizable()
        {
            NativeControl.Resizable = Control.Resizable;
        }

        private void ApplyHasBorder()
        {
            NativeControl.HasBorder = Control.HasBorder;
        }

        private void ApplyHasTitleBar()
        {
            NativeControl.HasTitleBar = Control.HasTitleBar;
        }

        private void NativeControl_SizeChanged(object? sender, EventArgs e)
        {
            Control.RaiseSizeChanged(EventArgs.Empty);
            PerformLayout();
        }

        private void NativeControl_LocationChanged(object? sender, EventArgs e)
        {
            Control.RaiseLocationChanged(EventArgs.Empty);
        }

        private void Control_Closing(object? sender, CancelEventArgs? e)
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
            if (!Modal)
                Control.Dispose();
        }

        private void Control_TitleChanged(object? sender, System.EventArgs? e)
        {
            if (e is null)
                throw new System.ArgumentNullException(nameof(e));

            ApplyTitle();
        }

        private void ApplyTitle()
        {
            NativeControl.Title = Control.Title;
        }
    }
}