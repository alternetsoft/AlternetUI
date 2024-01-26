using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that makes up an application's user interface.
    /// </summary>
    /// <remarks>A <see cref="Window"/> is a representation of any window displayed in
    /// your application.</remarks>
    [DesignerCategory("Code")]
    [ControlCategory("Hidden")]
    public partial class Window : Control
    {
        private static RectD defaultBounds = new(100, 100, 400, 400);
        private static int incFontSizeHighDpi = 2;
        private static int incFontSize = 0;

        private readonly WindowInfo info = new();
        private string title = string.Empty;
        private Toolbar? toolbar = null;
        private StatusBar? statusBar = null;
        private IconSet? icon = null;
        private MainMenu? menu = null;
        private Window? owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
            Application.Current.RegisterWindow(this);
            SetVisibleValue(false);
            Bounds = GetDefaultBounds();

            if (Control.DefaultFont != Font.Default)
                Font = Control.DefaultFont;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Menu"/> property changes.
        /// </summary>
        public event EventHandler? MenuChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Icon"/> property changes.
        /// </summary>
        public event EventHandler? IconChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Toolbar"/> property changes.
        /// </summary>
        public event EventHandler? ToolbarChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="StatusBar"/> property changes.
        /// </summary>
        public event EventHandler? StatusBarChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Owner"/> property changes.
        /// </summary>
        public event EventHandler? OwnerChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ShowInTaskbar"/> property changes.
        /// </summary>
        public event EventHandler? ShowInTaskbarChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        public event EventHandler? TitleChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="State"/> property changes.
        /// </summary>
        public event EventHandler? StateChanged;

        /// <summary>
        /// Occurs before the window is closed.
        /// </summary>
        /// <remarks>
        /// The <see cref="Closing"/> event occurs as the window is being closed. When a
        /// window is closed, it is disposed,
        /// releasing all resources associated with the form. If you cancel this event, the
        /// window remains opened.
        /// To cancel the closure of a window, set the <see cref="CancelEventArgs.Cancel"/>
        /// property of the
        /// <see cref="WindowClosingEventArgs"/> passed to your event handler to <c>true</c>.
        /// </remarks>
        public event EventHandler<WindowClosingEventArgs>? Closing;

        /// <summary>
        /// Occurs when the window is closed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Closed"/> event occurs after the window has been closed by the user
        /// or programmatically.
        /// To prevent a window from closing, handle the <see cref="Closing"/> event and set
        /// the <see cref="CancelEventArgs.Cancel"/> property
        /// of the <see cref="WindowClosingEventArgs"/> passed to your event handler to true.
        /// </para>
        /// <para>
        /// You can use this event to perform tasks such as freeing resources used by the window
        /// and to save information entered in the form or to update its parent window.
        /// </para>
        /// </remarks>
        public event EventHandler<WindowClosedEventArgs>? Closed;

        /// <summary>
        /// Occurs when the value of the <see cref="MaximizeEnabled"/> property changes.
        /// </summary>
        public event EventHandler? MaximizeEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MinimizeEnabled"/> property changes.
        /// </summary>
        public event EventHandler? MinimizeEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CloseEnabled"/> property changes.
        /// </summary>
        public event EventHandler? CloseEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="AlwaysOnTop"/> property changes.
        /// </summary>
        public event EventHandler? AlwaysOnTopChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsToolWindow"/> property changes.
        /// </summary>
        public event EventHandler? IsToolWindowChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HasSystemMenu"/> property changes.
        /// </summary>
        public event EventHandler? HasSystemMenuChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Resizable"/> property changes.
        /// </summary>
        public event EventHandler? ResizableChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HasBorder"/> property changes.
        /// </summary>
        public event EventHandler? HasBorderChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HasTitleBar"/> property changes.
        /// </summary>
        public event EventHandler? HasTitleBarChanged;

        /// <summary>
        /// Gets the currently active window for this application.
        /// </summary>
        /// <value>A <see cref="Window"/> that represents the currently active window,
        /// or <see langword="null"/> if there is no active window.</value>
        public static Window? ActiveWindow => WindowHandler.ActiveWindow;

        /// <summary>
        /// Gets or sets default location and position of the window.
        /// </summary>
        public static RectD DefaultBounds
        {
            get
            {
                return defaultBounds;
            }

            set
            {
                defaultBounds = value;
                Native.Window.SetDefaultBounds(defaultBounds);
            }
        }

        /// <summary>
        /// Gets or sets default control font size increment (in points) on high dpi displays (DPI greater than 96).
        /// Default value is 2.
        /// </summary>
        public static int IncFontSizeHighDpi
        {
            get => incFontSizeHighDpi;

            set
            {
                if (incFontSizeHighDpi == value)
                    return;
                incFontSizeHighDpi = value;
                UpdateDefaultFont();
            }
        }

        /// <summary>
        /// Gets or sets default control font size increment (in points) on normal dpi displays (DPI less or equal to 96).
        /// Default value is 0.
        /// </summary>
        public static int IncFontSize
        {
            get => incFontSize;
            set
            {
                if (incFontSize == value)
                    return;
                incFontSize = value;
                UpdateDefaultFont();
            }
        }

        /// <summary>
        /// Gets DPI of the first created window.
        /// </summary>
        public static SizeD? DefaultDPI => Application.FirstWindow()?.GetDPI();

        /// <summary>
        /// Gets a value indicating whether the window is the currently active window for
        /// this application.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsActive => NativeControl.IsActive;

        /// <summary>
        /// Gets or sets a boolean value indicating whether window has title bar.
        /// </summary>
        public virtual bool HasTitleBar
        {
            get => info.HasTitleBar;

            set
            {
                if (info.HasTitleBar == value)
                    return;
                info.HasTitleBar = value;
                OnHasTitleBarChanged(EventArgs.Empty);
                HasTitleBarChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets border style of the window.
        /// </summary>
        public new ControlBorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set => base.BorderStyle = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get => info.HasBorder;

            set
            {
                if (info.HasBorder == value)
                    return;
                info.HasBorder = value;
                OnHasBorderChanged(EventArgs.Empty);
                HasBorderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window can be resized by user.
        /// </summary>
        public virtual bool Resizable
        {
            get => info.Resizable;

            set
            {
                if (info.Resizable == value)
                    return;
                info.Resizable = value;
                OnResizableChanged(EventArgs.Empty);
                ResizableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// A tool window does not appear in the Windows taskbar or in the window that appears
        /// when the user presses ALT+TAB.
        /// On Windows, a tool window doesn't have minimize and maximize buttons.
        /// </summary>
        public virtual bool IsToolWindow
        {
            get => info.IsToolWindow;

            set
            {
                if (info.IsToolWindow == value)
                    return;
                info.IsToolWindow = value;
                OnIsToolWindowChanged(EventArgs.Empty);
                IsToolWindowChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to create a special popup window.
        /// </summary>
        /// <remarks>
        /// Set this flag to create a special popup window: it will be always shown
        /// on top of other windows, will capture the mouse and will be dismissed when
        /// the mouse is clicked outside of it or if it loses focus in any other way.
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsPopupWindow
        {
            get => info.IsPopupWindow;

            set
            {
                if (info.IsPopupWindow == value)
                    return;
                info.IsPopupWindow = value;
                Handler.NativeControl.IsPopupWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets whether system menu is visible for this window.
        /// </summary>
        public virtual bool HasSystemMenu
        {
            get
            {
                return info.HasSystemMenu;
            }

            set
            {
                if (info.HasSystemMenu == value)
                    return;
                info.HasSystemMenu = value;
                OnHasSystemMenuChanged(EventArgs.Empty);
                HasSystemMenuChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window should be placed above all
        /// non-topmost windows and
        /// should stay above them, even when the window is deactivated.
        /// </summary>
        public virtual bool AlwaysOnTop
        {
            get => info.AlwaysOnTop;

            set
            {
                if (info.AlwaysOnTop == value)
                    return;
                info.AlwaysOnTop = value;
                OnAlwaysOnTopChanged(EventArgs.Empty);
                AlwaysOnTopChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled close button.
        /// </summary>
        public virtual bool CloseEnabled
        {
            get => info.CloseEnabled;

            set
            {
                if (info.CloseEnabled == value)
                    return;
                info.CloseEnabled = value;
                OnCloseEnabledChanged(EventArgs.Empty);
                CloseEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled maximize button.
        /// </summary>
        public virtual bool MaximizeEnabled
        {
            get => info.MaximizeEnabled;

            set
            {
                if (info.MaximizeEnabled == value)
                    return;
                info.MaximizeEnabled = value;
                OnMaximizeEnabledChanged(EventArgs.Empty);
                MaximizeEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form is displayed in the Windows or
        /// Linux taskbar.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to display the form in the Windows taskbar at run time;
        /// otherwise, <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
        /// <remarks>
        /// You can use this property to prevent users from selecting your form through the taskbar.
        /// For example, if you display a Find and Replace tool window in your application,
        /// you might want to prevent that window from being selected through the taskbar
        /// because you would need both the application's main window and the Find and Replace
        /// tool window displayed in order to process searches appropriately.
        /// </remarks>
        public virtual bool ShowInTaskbar
        {
            get => info.ShowInTaskbar;

            set
            {
                if (info.ShowInTaskbar == value)
                    return;
                info.ShowInTaskbar = value;
                OnShowInTaskbarChanged(EventArgs.Empty);
                ShowInTaskbarChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled minimize button.
        /// </summary>
        public virtual bool MinimizeEnabled
        {
            get => info.MinimizeEnabled;

            set
            {
                if (info.MinimizeEnabled == value)
                    return;
                info.MinimizeEnabled = value;
                OnMinimizeEnabledChanged(EventArgs.Empty);
                MinimizeEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the window that owns this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> that represents the window that is the owner of this window.
        /// </value>
        /// <remarks>
        /// When a window is owned by another window, it is closed or hidden with the owner window.
        /// </remarks>
        [Browsable(false)]
        public virtual Window? Owner
        {
            get => owner;

            set
            {
                if (owner == value)
                    return;
                owner = value;
                OnOwnerChanged(EventArgs.Empty);
                OwnerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a title of this window.
        /// </summary>
        /// <remarks>A string that contains a title of this window. Default value is empty
        /// string ("").</remarks>
        public virtual string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                    return;
                title = value;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the position of the window when first shown.
        /// </summary>
        /// <value>A <see cref="WindowStartLocation"/> that represents the starting position
        /// of the window.</value>
        /// <remarks>
        /// This property enables you to set the starting position of the window when it is
        /// first shown.
        /// This property should be set before the window is shown.
        /// </remarks>
        public virtual WindowStartLocation StartLocation
        {
            get => info.StartLocation;
            set
            {
                if (info.StartLocation == value)
                    return;
                info.StartLocation = value;
                Handler.StartLocation = value;
            }
        }

        /// <summary>
        /// Gets an array of <see cref="Window"/> objects that represent all windows that are
        /// owned by this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> array that represents the owned windows for this window.
        /// </value>
        /// <remarks>
        /// This property returns an array that contains all windows that are owned by this
        /// window. To make a window owned by another window, set the
        /// <see cref="Owner"/> property.
        /// When a window is owned by another window, it is closed or hidden
        /// with the owner window.
        /// </remarks>
        [Browsable(false)]
        public virtual Window[] OwnedWindows { get => Handler.OwnedWindows; }

        /// <summary>
        /// Gets or sets a value that indicates whether window is minimized,
        /// maximized, or normal.
        /// </summary>
        public virtual WindowState State
        {
            get => info.State;

            set
            {
                if (info.State == value)
                    return;

                info.State = value;
                OnStateChanged(EventArgs.Empty);
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="StatusBar"/> that is displayed in the window.
        /// </summary>
        /// <value>
        /// A <see cref="StatusBar"/> that represents the status bar to display in the window.
        /// </value>
        /// <remarks>
        /// You can use this property to switch between complete status bar sets at run time.
        /// </remarks>
        [Browsable(false)]
        public virtual StatusBar? StatusBar
        {
            get => statusBar;

            set
            {
                if (statusBar == value)
                    return;

                if(GetWindowKind() == WindowKind.Dialog)
                {
                    statusBar = value;
                    return;
                }

                if (value is not null)
                {
                    if (value.IsDisposed)
                        throw new ObjectDisposedException(nameof(StatusBar));
                    if (value.Window is not null && value.Window != this)
                    {
                        throw new ArgumentException(
                            "Object is already attached to the window",
                            nameof(StatusBar));
                    }
                }

                if (statusBar is not null)
                {
                    statusBar.Window = null;
                    var oldHandle = Handler.NativeControl.WxStatusBar;
                    if (oldHandle != default)
                        Native.WxStatusBarFactory.DeleteStatusBar(oldHandle);
                }

                statusBar = value;
                if (statusBar is not null)
                    statusBar.Window = this;
                else
                    Handler.NativeControl.WxStatusBar = default;

                OnStatusBarChanged(EventArgs.Empty);
                StatusBarChanged?.Invoke(this, EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this window is displayed modally.
        /// </summary>
        [Browsable(false)]
        public virtual bool Modal
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="MainMenu"/> that is displayed in the window.
        /// </summary>
        /// <value>
        /// A <see cref="MainMenu"/> that represents the menu to display in the window.
        /// </value>
        /// <remarks>
        /// You can use this property to switch between complete menu sets at run time.
        /// </remarks>
        public virtual MainMenu? Menu
        {
            get => menu;

            set
            {
                if (menu == value)
                    return;

                var oldValue = menu;
                menu = value;

                if (GetWindowKind() == WindowKind.Dialog)
                    return;

                oldValue?.SetParentInternal(null);
                menu?.SetParentInternal(this);

                OnMenuChanged(EventArgs.Empty);
                MenuChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the icon for the window.
        /// </summary>
        /// <value>
        /// An <see cref="ImageSet"/> that represents the icon for the window.
        /// </value>
        /// <remarks>
        /// A window's icon designates the picture that represents the window in the taskbar
        /// as well as the icon that is displayed for the control box of the window.
        /// If images of several sizes are contained within the <see cref="ImageSet"/>, the
        /// most fitting size is selected automatically.
        /// </remarks>
        public virtual IconSet? Icon
        {
            get => icon;

            set
            {
                if (icon == value)
                    return;

                icon = value;
                OnIconChanged(EventArgs.Empty);
                IconChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Toolbar"/> that is displayed in the window.
        /// </summary>
        /// <value>
        /// A <see cref="Toolbar"/> that represents the toolbar to display in the window.
        /// </value>
        /// <remarks>
        /// You can use this property to switch between complete toolbar sets at run time.
        /// </remarks>
        [Browsable(false)]
        public virtual Toolbar? Toolbar
        {
            get => toolbar;

            set
            {
                if (toolbar == value)
                    return;

                var oldValue = toolbar;
                toolbar = value;

                if (GetWindowKind() == WindowKind.Dialog)
                    return;

                oldValue?.SetParentInternal(null);
                toolbar?.SetParentInternal(this);

                OnToolbarChanged(EventArgs.Empty);
                ToolbarChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }

            set
            {
                if (Visible == value)
                    return;
                if (value)
                {
                    ApplyStartLocationOnce(Owner);
                }

                base.Visible = value;
            }
        }

        /// <summary>
        /// Gets the collection of input bindings associated with this window.
        /// </summary>
        [Browsable(false)]
        public virtual Collection<InputBinding> InputBindings { get; } = [];

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Window;

        /// <summary>
        /// Gets a <see cref="WindowHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new WindowHandler Handler => (WindowHandler)base.Handler;

        [Browsable(false)]
        internal new Native.Window NativeControl => (Native.Window)base.NativeControl;

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection
        {
            get
            {
                foreach (var item in base.LogicalChildrenCollection)
                    yield return item;

                if (Menu != null)
                    yield return Menu;

                if (Toolbar != null)
                    yield return Toolbar;

                if (StatusBar != null)
                    yield return StatusBar;
            }
        }

        /// <summary>
        /// Updates default control font after changes in <see cref="IncFontSizeHighDpi"/>
        /// or <see cref="IncFontSize"/>. You should not call this method directly.
        /// </summary>
        public static void UpdateDefaultFont()
        {
            if (!Application.Initialized)
                return;

            var dpi = Display.Primary.DPI;
            var incFont = (dpi.Width > 96) ? Window.IncFontSizeHighDpi : Window.IncFontSize;

            if (incFont > 0)
            {
                FontInfo info = Font.Default;
                info.SizeInPoints += incFont;
                Font font = info;
                Control.DefaultFont = font;
            }
        }

        /// <inheritdoc/>
        public override void PerformLayout(bool layoutParent = true)
        {
            if (IsLayoutPerform || IsLayoutSuspended)
                return;
            base.PerformLayout(layoutParent);
        }

        /// <summary>
        /// Shows window and focuses it.
        /// </summary>
        /// <param name="useIdle">Whether to use <see cref="Application.Idle"/>
        /// event to show the window.</param>
        public virtual void ShowAndFocus(bool useIdle = false)
        {
            if (useIdle)
                Application.AddIdleTask(Fn);
            else
                Fn();

            void Fn()
            {
                Show();
                Raise();
                if (CanFocus)
                    SetFocus();
            }
        }

        /// <summary>
        /// Activates the window.
        /// </summary>
        /// <remarks>
        /// Activating a window brings it to the front if this is the active application,
        /// or it flashes the window caption if this is not the active application.
        /// </remarks>
        public virtual void Activate() => NativeControl.Activate();

        /// <summary>
        /// Gets default bounds assigned to the window.
        /// </summary>
        /// <returns></returns>
        public virtual RectD GetDefaultBounds()
        {
            return DefaultBounds;
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <remarks>
        /// When a window is closed, all resources created within the object are closed and the
        /// window is disposed.
        /// You can prevent the closing of a window at run time by handling the
        /// <see cref="Window.Closing"/> event and
        /// setting the <c>Cancel</c> property of the <see cref="CancelEventArgs"/> passed as
        /// a parameter to your event handler.
        /// If the window you are closing is the last open window of your application,
        /// your application ends.
        /// The window is not disposed on <see cref="Close"/> when you have displayed the
        /// window using <see cref="DialogWindow.ShowModal()"/>.
        /// In this case, you will need to call <see cref="IDisposable.Dispose"/> manually.
        /// </remarks>
        public virtual void Close()
        {
            Visible = false;

            CheckDisposed();

            Handler.Close();
        }

        /// <summary>
        /// Initializes <see cref="Window"/> properties so it looks like popup window.
        /// </summary>
        public virtual void MakeAsPopup()
        {
            ShowInTaskbar = false;
            StartLocation = WindowStartLocation.Manual;
            HasTitleBar = false;
            HasBorder = false;
            AlwaysOnTop = true;
            CloseEnabled = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Resizable = false;
            HasSystemMenu = false;
        }

        internal static Window? GetParentWindow(DependencyObject dp)
        {
            if (dp is Window w)
                return w;

            if (dp is not Control c)
                return null;

            if (c.Parent == null)
                return null;

            return GetParentWindow(c.Parent);
        }

        internal virtual WindowKind GetWindowKind() => WindowKind.Window;

        internal void RaiseClosing(WindowClosingEventArgs e) => OnClosing(e);

        internal void RaiseClosed(WindowClosedEventArgs e) => OnClosed(e);

        internal void RecreateAllHandlers()
        {
            void GetAllChildren(Control control, List<Control> result)
            {
                foreach (var child in control.Children)
                    GetAllChildren(child, result);

                if (control != this)
                    result.Add(control);
            }

            var children = new List<Control>();
            GetAllChildren(this, children);

            foreach (var child in children)
                child.DetachHandler();

            foreach (var child in children.AsEnumerable().Reverse())
                child.EnsureHandlerCreated();
        }

        internal void ApplyStartLocationOnce(Control? owner)
        {
            if (!StateFlags.HasFlag(ControlFlags.StartLocationApplied))
            {
                StateFlags |= ControlFlags.StartLocationApplied;
                ApplyStartLocation(owner);
            }
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler() => new WindowHandler();

        /// <summary>
        /// Raises the <see cref="Closing"/> event and calls
        /// <see cref="OnClosing(WindowClosingEventArgs)"/>.
        /// See <see cref="Closing"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="WindowClosingEventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnClosing(WindowClosingEventArgs e) => Closing?.Invoke(this, e);

        /// <summary>
        /// Called when the value of the <see cref="CloseEnabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnCloseEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="ShowInTaskbar"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnShowInTaskbarChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="AlwaysOnTop"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnAlwaysOnTopChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="MaximizeEnabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMaximizeEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="MinimizeEnabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMinimizeEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="Closed"/> event and calls
        /// <see cref="OnClosed(WindowClosedEventArgs)"/>.
        /// See <see cref="Closed"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="WindowClosedEventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnClosed(WindowClosedEventArgs e) => Closed?.Invoke(this, e);

        /// <summary>
        /// Called when the value of the <see cref="Owner"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnOwnerChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            Visible = false;

            base.Dispose(disposing);

            if (disposing)
            {
                Application.Current.UnregisterWindow(this);
                if (!Application.Current.VisibleWindows.Any())
                    Application.Current.NativeApplication.ExitMainLoop();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="IsToolWindow"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnIsToolWindowChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="HasSystemMenu"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHasSystemMenuChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Resizable"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnResizableChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="HasTitleBar"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHasTitleBarChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="HasBorder"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHasBorderChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Icon"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnIconChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Menu"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMenuChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Toolbar"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnToolbarChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Applies <see cref="Window.StartLocation"/> to the location of the window.
        /// </summary>
        protected virtual void ApplyStartLocation(Control? owner)
        {
            RectD displayRect = GetDisplay().ClientAreaDip;
            RectD parentRect = RectD.Empty;
            bool center = true;

            if (StartLocation == WindowStartLocation.CenterScreen)
            {
                parentRect = displayRect;
            }
            else
            if (StartLocation == WindowStartLocation.CenterOwner)
            {
                if (owner is null)
                    parentRect = displayRect;
                else
                    parentRect = new(owner.ClientToScreen((0, 0)), owner.ClientSize);
            }
            else
                center = false;

            var bounds = Bounds;

            var newWidth = Math.Min(bounds.Width, displayRect.Width);
            var newHeight = Math.Min(bounds.Height, displayRect.Height);

            bounds.Width = newWidth;
            bounds.Height = newHeight;

            if (center)
                bounds = bounds.CenterIn(parentRect);

            Bounds = bounds;
        }

        /// <summary>
        /// Called when the value of the <see cref="StatusBar"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStatusBarChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="State"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnStateChanged(EventArgs e)
        {
        }
    }
}