using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a component that creates an icon in the notification area.
    /// </summary>
    /// <remarks>
    /// Icons in the notification area are shortcuts to processes that are running
    /// in the background of a computer, such
    /// as a virus protection program or a volume control. These processes do not
    /// come with their own user interfaces.
    /// The <see cref="NotifyIcon"/> class provides a way to program in this functionality.
    /// The <see cref="Icon"/>
    /// property defines the icon that appears in the notification area. Pop-up menus
    /// for an icon are addressed with the
    /// <see cref="Menu"/> property. The <see cref="Text"/> property assigns tool tip text.
    /// In order for the icon to
    /// show up in the notification area, the Visible property must be set to true.
    /// </remarks>
    public class NotifyIcon : HandledObject<INotifyIconHandler>
    {
        private static bool? isAvailable;

        private Image? icon;
        private ContextMenu? menu;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyIcon"/> class.
        /// </summary>
        public NotifyIcon()
        {
        }

        /// <summary>
        /// Occurs when the icon in the notification area is clicked with the right mouse button.
        /// </summary>
        public event EventHandler? Click;

        /// <summary>
        /// Occurs when the icon in the notification area is double-clicked with the right mouse button.
        /// </summary>
        public event EventHandler? RightMouseButtonDoubleClick;

        /// <summary>
        /// Occurs when the icon in the notification area is pressed with the right mouse button.
        /// </summary>
        public event EventHandler? RightMouseButtonDown;

        /// <summary>
        /// Occurs when the icon in the notification area is released with the right mouse button.
        /// </summary>
        public event EventHandler? RightMouseButtonUp;

        /// <summary>
        /// Occurs when the icon in the notification area is double-clicked with the left mouse button.
        /// </summary>
        public event EventHandler? LeftMouseButtonDoubleClick;

        /// <summary>
        /// Occurs when the icon in the notification area is pressed with the left mouse button.
        /// </summary>
        public event EventHandler? LeftMouseButtonDown;

        /// <summary>
        /// Occurs when the icon in the notification area is released with the left mouse button.
        /// </summary>
        public event EventHandler? LeftMouseButtonUp;

        /// <summary>
        /// Gets whether system tray is available in the desktop environment the app runs under.
        /// </summary>
        /// <remarks>
        /// On Windows and macOS, the tray is always available and this function simply returns true.
        /// On Linux environment may or may not provide the tray, depending on
        /// user's desktop environment. Tray availability may change during application's
        /// lifetime under Linux and so applications shouldn't cache the result.
        /// </remarks>
        public static bool IsAvailable
        {
            get
            {
                return isAvailable
                    ??= (bool?)App.Handler.GetAttributeValue("NotifyIcon.IsAvailable") ?? false;
            }
        }

        /// <summary>
        /// Gets whether icon was actually installed and visible.
        /// </summary>
        public virtual bool IsIconInstalled
        {
            get
            {
                CheckDisposed();
                return Handler.IsIconInstalled;
            }
        }

        /// <summary>
        /// Gets whether this object initialized successfully.
        /// </summary>
        public virtual bool IsOk
        {
            get
            {
                CheckDisposed();
                return Handler.IsOk;
            }
        }

        /// <summary>
        /// Gets or sets the tool tip text displayed when the mouse pointer rests on a
        /// notification area icon.
        /// </summary>
        /// <value>
        /// The tool tip text displayed when the mouse pointer rests on a notification area icon.
        /// </value>
        /// <remarks>
        /// If the text is <see langword="null"/>, no tool tip is displayed.
        /// </remarks>
        public virtual string? Text
        {
            get
            {
                CheckDisposed();
                return Handler.Text;
            }

            set
            {
                CheckDisposed();
                Handler.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the current icon.
        /// </summary>
        /// <value>
        /// The <see cref="Image"/> displayed by the <see cref="NotifyIcon"/> component.
        /// The default value is <see langword="null"/>.
        /// </value>
        public virtual Image? Icon
        {
            get
            {
                CheckDisposed();
                return icon;
            }

            set
            {
                CheckDisposed();
                icon = value;
                Handler.Icon = value;
            }
        }

        /// <summary>
        /// Gets or sets the menu associated with the <see cref="NotifyIcon"/>.
        /// </summary>
        public virtual ContextMenu? Menu
        {
            get
            {
                return menu;
            }

            set
            {
                if (menu == value)
                    return;
                menu = value;
                Handler.SetContextMenu(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the icon is visible in the notification
        /// area of the taskbar.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the icon is visible in the notification area; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// Since the default value is <see langword="false"/>, in order for the icon
        /// to show up in the notification
        /// area, you must set the <see cref="Visible"/> property to <see langword="true"/>.
        /// </remarks>
        public virtual bool Visible
        {
            get
            {
                CheckDisposed();
                return Handler.Visible;
            }

            set
            {
                CheckDisposed();
                Handler.Visible = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="RightMouseButtonDoubleClick"/> event
        /// and calls <see cref="OnRightMouseButtonDoubleClick"/>.
        /// </summary>
        public void RaiseRightMouseButtonDoubleClick()
        {
            RightMouseButtonDoubleClick?.Invoke(this, EventArgs.Empty);
            OnRightMouseButtonDoubleClick(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="RightMouseButtonDown"/> event
        /// and calls <see cref="OnRightMouseButtonDown"/>.
        /// </summary>
        public void RaiseRightMouseButtonDown()
        {
            RightMouseButtonDown?.Invoke(this, EventArgs.Empty);
            OnRightMouseButtonDown(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="RightMouseButtonUp"/> event
        /// and calls <see cref="OnRightMouseButtonUp"/>.
        /// </summary>
        public void RaiseRightMouseButtonUp()
        {
            RightMouseButtonUp?.Invoke(this, EventArgs.Empty);
            OnRightMouseButtonUp(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="LeftMouseButtonDoubleClick"/> event
        /// and calls <see cref="OnLeftMouseButtonDoubleClick"/>.
        /// </summary>
        public void RaiseLeftMouseButtonDoubleClick()
        {
            LeftMouseButtonDoubleClick?.Invoke(this, EventArgs.Empty);
            OnLeftMouseButtonDoubleClick(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="LeftMouseButtonDown"/> event
        /// and calls <see cref="OnLeftMouseButtonDown"/>.
        /// </summary>
        public void RaiseLeftMouseButtonDown()
        {
            LeftMouseButtonDown?.Invoke(this, EventArgs.Empty);
            OnLeftMouseButtonDown(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="LeftMouseButtonUp"/> event
        /// and calls <see cref="OnLeftMouseButtonUp"/>.
        /// </summary>
        public void RaiseLeftMouseButtonUp()
        {
            LeftMouseButtonUp?.Invoke(this, EventArgs.Empty);
            OnLeftMouseButtonUp(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls <see cref="OnClick"/>.
        /// </summary>
        public void RaiseClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
            OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="Click"/> event is raised.
        /// </summary>
        /// <remarks>
        /// Derived classes can
        /// override this method to provide additional handling when the event is raised.
        /// When overriding, ensure to
        /// call the base implementation to preserve the event invocation.</remarks>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClick(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (IsHandlerCreated)
            {
                Handler.Click = null;
                Handler.LeftMouseButtonUp = null;
                Handler.LeftMouseButtonDown = null;
                Handler.LeftMouseButtonDoubleClick = null;
                Handler.RightMouseButtonUp = null;
                Handler.RightMouseButtonDown = null;
                Handler.RightMouseButtonDoubleClick = null;
            }

            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override INotifyIconHandler CreateHandler()
        {
            var result = App.Handler.CreateNotifyIconHandler();
            result.Click = RaiseClick;
            result.LeftMouseButtonUp = RaiseLeftMouseButtonUp;
            result.LeftMouseButtonDown = RaiseLeftMouseButtonDown;
            result.LeftMouseButtonDoubleClick = RaiseLeftMouseButtonDoubleClick;
            result.RightMouseButtonUp = RaiseRightMouseButtonUp;
            result.RightMouseButtonDown = RaiseRightMouseButtonDown;
            result.RightMouseButtonDoubleClick = RaiseRightMouseButtonDoubleClick;
            result.Created = () =>
            {
                result.SetContextMenu(menu);
            };
            return result;
        }

        /// <summary>
        /// Called when the left mouse button is released over the notification icon.
        /// </summary>
        /// <param name="empty">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// Derived classes can override this method to provide additional
        /// handling when the event is raised.
        /// When overriding, ensure to call the base implementation
        /// to preserve the event invocation.
        /// </remarks>
        protected virtual void OnLeftMouseButtonUp(EventArgs empty)
        {
        }

        /// <summary>
        /// Called when the right mouse button is double-clicked over the notification icon.
        /// </summary>
        /// <param name="empty">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// Derived classes can override this method to provide additional
        /// handling when the event is raised.
        /// When overriding, ensure to call the base implementation
        /// to preserve the event invocation.
        /// </remarks>
        protected virtual void OnRightMouseButtonDoubleClick(EventArgs empty)
        {
        }

        /// <summary>
        /// Called when the right mouse button is released over the notification icon.
        /// </summary>
        /// <param name="empty">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// Derived classes can override this method to provide additional
        /// handling when the event is raised.
        /// When overriding, ensure to call the base implementation
        /// to preserve the event invocation.
        /// </remarks>
        protected virtual void OnRightMouseButtonUp(EventArgs empty)
        {
        }

        /// <summary>
        /// Called when the right mouse button is pressed over the notification icon.
        /// </summary>
        /// <param name="empty">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// Derived classes can override this method to provide
        /// additional handling when the event is raised.
        /// When overriding, ensure to call the base implementation
        /// to preserve the event invocation.
        /// </remarks>
        protected virtual void OnRightMouseButtonDown(EventArgs empty)
        {
        }

        /// <summary>
        /// Called when the left mouse button is double-clicked over the notification icon.
        /// </summary>
        /// <param name="empty">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// Derived classes can override this method to provide additional
        /// handling when the event is raised.
        /// When overriding, ensure to call the base implementation
        /// to preserve the event invocation.
        /// </remarks>
        protected virtual void OnLeftMouseButtonDoubleClick(EventArgs empty)
        {
        }

        /// <summary>
        /// Called when the left mouse button is pressed over the notification icon.
        /// </summary>
        /// <param name="empty">An <see cref="EventArgs"/> that contains the event data.</param>
        /// <remarks>
        /// Derived classes can override this method to provide
        /// additional handling when the event is raised.
        /// When overriding, ensure to call the base implementation
        /// to preserve the event invocation.
        /// </remarks>
        protected virtual void OnLeftMouseButtonDown(EventArgs empty)
        {
        }
    }
}