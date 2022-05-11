using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that makes up an application's user interface.
    /// </summary>
    /// <remarks>A <see cref="Window"/> is a representation of any window displayed in your application.</remarks>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Control
    {
        internal static Window? GetParentWindow(DependencyObject dp)
        {
            // For use instead of PresentationSource.CriticalFromVisual(focusScope).

            if (dp is Window w)
                return w;

            if (!(dp is Control c))
                return null;

            if (c.Parent == null)
                return null;

            return GetParentWindow(c.Parent);
        }

        /// <summary>
        /// Gets an array of <see cref="Window"/> objects that represent all windows that are owned by this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> array that represents the owned windows for this window.
        /// </value>
        /// <remarks>
        /// This property returns an array that contains all windows that are owned by this window. To make a window owned by another window, set the <see cref="Owner"/> property.
        /// </remarks>
        public Window[] OwnedWindows { get => Handler.OwnedWindows; }

        private string title = "";
        private WindowStartLocation startLocation = WindowStartLocation.SystemDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
            Application.Current.RegisterWindow(this);
            SetVisibleValue(false);
            Bounds = new Rect(100, 100, 400, 400);
        }

        private new NativeWindowHandler Handler => (NativeWindowHandler)base.Handler;

        /// <summary>
        /// Gets a value indicating whether the window is the currently active window for this application.
        /// </summary>
        public bool IsActive => Handler.IsActive;

        /// <summary>
        /// Activates the window.
        /// </summary>
        /// <remarks>
        /// Activating a window brings it to the front if this is the active application,
        /// or it flashes the window caption if this is not the active application.
        /// </remarks>
        public void Activate() => Handler.Activate();

        /// <summary>
        /// Gets the currently active window for this application.
        /// </summary>
        /// <value>A <see cref="Window"/> that represents the currently active window, or <see langword="null"/> if there is no active window.</value>
        public static Window ActiveWindow => NativeWindowHandler.ActiveWindow;

        /// <summary>
        /// Occurs when the window is activated in code or by the user.
        /// </summary>
        /// <remarks>
        /// To activate a window at run time using code, call the <see cref="Activate"/> method. You can use this event for
        /// tasks such as updating the contents of the window based on changes made to the window's data when the window was not activated.
        /// </remarks>
        public event EventHandler? Activated;

        internal void RaiseActivated() => Activated?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Occurs when the window loses focus and is no longer the active window.
        /// </summary>
        /// <remarks>
        /// You can use this event to perform tasks such as updating another window in your application with data from the deactivated window.
        /// </remarks>
        public event EventHandler? Deactivated;

        internal void RaiseDeactivated() => Deactivated?.Invoke(this, EventArgs.Empty);

        private bool hasTitleBar = true;

        /// <summary>
        /// Gets or sets
        /// </summary>
        /// <value>
        /// </value>
        /// <remarks>
        /// </remarks>
        public bool HasTitleBar
        {
            get => hasTitleBar;

            set
            {
                hasTitleBar = value;
                OnHasTitleBarChanged(EventArgs.Empty);
                HasTitleBarChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="HasTitleBar"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHasTitleBarChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HasTitleBar"/> property changes.
        /// </summary>
        public event EventHandler? HasTitleBarChanged;


        private bool hasBorder = true;

        /// <summary>
        /// Gets or sets a value indicating whether the window has a border.
        /// </summary>
        public bool HasBorder
        {
            get => hasBorder;

            set
            {
                hasBorder = value;
                OnHasBorderChanged(EventArgs.Empty);
                HasBorderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="HasBorder"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnHasBorderChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="HasBorder"/> property changes.
        /// </summary>
        public event EventHandler? HasBorderChanged;


        private bool resizable = true;

        /// <summary>
        /// Gets or sets a value indicating whether the window can be resized by user.
        /// </summary>
        public bool Resizable
        {
            get => resizable;

            set
            {
                resizable = value;
                OnResizableChanged(EventArgs.Empty);
                ResizableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Resizable"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnResizableChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Resizable"/> property changes.
        /// </summary>
        public event EventHandler? ResizableChanged;


        private bool isToolWindow = false;

        /// <summary>
        /// A tool window does not appear in the Windows taskbar or in the window that appears when the user presses ALT+TAB.
        /// On Windows, a tool window doesn't have minimize and maximize buttons.
        /// </summary>
        public bool IsToolWindow
        {
            get => isToolWindow;

            set
            {
                isToolWindow = value;
                OnIsToolWindowChanged(EventArgs.Empty);
                IsToolWindowChanged?.Invoke(this, EventArgs.Empty);
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
        /// Occurs when the value of the <see cref="IsToolWindow"/> property changes.
        /// </summary>
        public event EventHandler? IsToolWindowChanged;


        private bool alwaysOnTop = false;

        /// <summary>
        /// Gets or sets a value indicating whether the window should be placed above all non-topmost windows and
        /// should stay above them, even when the window is deactivated.
        /// </summary>
        public bool AlwaysOnTop
        {
            get => alwaysOnTop;

            set
            {
                alwaysOnTop = value;
                OnAlwaysOnTopChanged(EventArgs.Empty);
                AlwaysOnTopChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="AlwaysOnTop"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnAlwaysOnTopChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AlwaysOnTop"/> property changes.
        /// </summary>
        public event EventHandler? AlwaysOnTopChanged;

        private bool closeEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled close button.
        /// </summary>
        public bool CloseEnabled
        {
            get => closeEnabled;

            set
            {
                closeEnabled = value;
                OnCloseEnabledChanged(EventArgs.Empty);
                CloseEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="CloseEnabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnCloseEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CloseEnabled"/> property changes.
        /// </summary>
        public event EventHandler? CloseEnabledChanged;

        private bool maximizeEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled maximize button.
        /// </summary>
        /// <value>
        /// </value>
        /// <remarks>
        /// </remarks>
        public bool MaximizeEnabled
        {
            get => maximizeEnabled;

            set
            {
                maximizeEnabled = value;
                OnMaximizeEnabledChanged(EventArgs.Empty);
                MaximizeEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="MaximizeEnabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMaximizeEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MaximizeEnabled"/> property changes.
        /// </summary>
        public event EventHandler? MaximizeEnabledChanged;


        private bool minimizeEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled minimize button.
        /// </summary>
        public bool MinimizeEnabled
        {
            get => minimizeEnabled;

            set
            {
                minimizeEnabled = value;
                OnMinimizeEnabledChanged(EventArgs.Empty);
                MinimizeEnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="MinimizeEnabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnMinimizeEnabledChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MinimizeEnabled"/> property changes.
        /// </summary>
        public event EventHandler? MinimizeEnabledChanged;


        private bool showInTaskbar = true;

        /// <summary>
        /// Gets or sets a value indicating whether the form is displayed in the Windows or Linux taskbar.
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
        public bool ShowInTaskbar
        {
            get => showInTaskbar;
            
            set
            {
                showInTaskbar = value;
                OnShowInTaskbarChanged(EventArgs.Empty);
                ShowInTaskbarChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="ShowInTaskbar"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnShowInTaskbarChanged(EventArgs e)
        {
        }

        private Window? owner;

        /// <summary>
        /// Gets or sets the window that owns this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> that represents the window that is the owner of this window.
        /// </value>
        /// <remarks>
        /// </remarks>
        public Window? Owner
        {
            get => owner;

            set
            {
                owner = value;
                OnOwnerChanged(EventArgs.Empty);
                OwnerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="Owner"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnOwnerChanged(EventArgs e)
        {
        }

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
        /// Occurs before the window is closed.
        /// </summary>
        /// <remarks>
        /// The <see cref="Closing"/> event occurs as the window is being closed. When a window is closed, it is disposed,
        /// releasing all resources associated with the form. If you cancel this event, the window remains opened.
        /// To cancel the closure of a window, set the <see cref="CancelEventArgs.Cancel"/> property of the
        /// <see cref="WindowClosingEventArgs"/> passed to your event handler to <c>true</c>.
        /// </remarks>
        public event EventHandler<WindowClosingEventArgs>? Closing;

        /// <summary>
        /// Occurs when the window is closed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Closed"/> event occurs after the window has been closed by the user or programmatically.
        /// To prevent a window from closing, handle the <see cref="Closing"/> event and set the <see cref="CancelEventArgs.Cancel"/> property
        /// of the <see cref="WindowClosingEventArgs"/> passed to your event handler to true.
        /// </para>
        /// <para>
        /// You can use this event to perform tasks such as freeing resources used by the window
        /// and to save information entered in the form or to update its parent window.
        /// </para>
        /// </remarks>
        public event EventHandler<WindowClosedEventArgs>? Closed;

        /// <summary>
        /// Gets or sets a title of this window.
        /// </summary>
        /// <remarks>A string that contains a title of this window. Default value is empty string ("").</remarks>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the position of the window when first shown.
        /// </summary>
        /// <value>A <see cref="WindowStartLocation"/> that represents the starting position of the window.</value>
        /// <remarks>
        /// This property enables you to set the starting position of the window when it is first shown.
        /// This property should be set before the window is shown.
        /// </remarks>
        public WindowStartLocation StartLocation
        {
            get => startLocation;
            set
            {
                startLocation = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the window.
        /// </summary>
        /// <value>The size of the window, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="Drawing.Size"/>(<see cref="double.NaN"/>, <see cref="double.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the window.
        /// Set this property to <see cref="Drawing.Size"/>(<see cref="double.NaN"/>, <see cref="double.NaN"/>) to specify system-default sizing
        /// behavior when the window is first shown.
        /// </remarks>
        public override Size Size
        {
            get
            {
                return Bounds.Size;
            }

            set
            {
                Handler.Bounds = new Rect(Bounds.Location, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>The width of the window, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the width of the window.
        /// Set this property to <see cref="double.NaN"/> to specify system-default sizing
        /// behavior before the window is first shown.
        /// </remarks>
        public override double Width { get => Size.Width; set => Size = new Size(value, Height); }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>The height of the window, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the height of the window.
        /// Set this property to <see cref="double.NaN"/> to specify system-default sizing
        /// behavior before the window is first shown.
        /// </remarks>
        public override double Height { get => Size.Height; set => Size = new Size(Width, value); }

        /// <summary>
        /// Gets or sets the location of upper-left corner of the window, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <value>The position of the window's upper-left corner, in logical units (1/96th of an inch).</value>
        /// <remarks>
        /// To specify the window positioning behavior when it is being shown for the first time,
        /// use <see cref="StartLocation"/> property.
        /// </remarks>
        public Point Location
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new Rect(value, Bounds.Size);
            }
        }

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

        /// <summary>
        /// Raises the <see cref="Closing"/> event and calls <see cref="OnClosing(WindowClosingEventArgs)"/>.
        /// See <see cref="Closing"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="WindowClosingEventArgs"/> that contains the event data.</param>
        protected virtual void OnClosing(WindowClosingEventArgs e) => Closing?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="Closed"/> event and calls <see cref="OnClosed(WindowClosedEventArgs)"/>.
        /// See <see cref="Closed"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="WindowClosedEventArgs"/> that contains the event data.</param>
        protected virtual void OnClosed(WindowClosedEventArgs e) => Closed?.Invoke(this, e);

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Application.Current.UnregisterWindow(this);
            }
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler() => new NativeWindowHandler();
    }
}