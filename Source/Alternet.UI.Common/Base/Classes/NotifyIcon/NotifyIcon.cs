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
        /// Occurs when the user clicks the icon in the notification area.
        /// </summary>
        public event EventHandler? Click;

        /// <summary>
        /// Occurs when the user double clicks the icon in the notification area.
        /// </summary>
        public event EventHandler? DoubleClick;

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
                CheckDisposed();
                return menu;
            }

            set
            {
                CheckDisposed();
                menu = value;
                Handler.Menu = value;
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
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnClick(EventArgs e)
        {
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DoubleClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDoubleClick(EventArgs e)
        {
            DoubleClick?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (IsHandlerCreated)
            {
                Handler.Click = null;
                Handler.DoubleClick = null;
            }

            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override INotifyIconHandler CreateHandler()
        {
            var result = App.Handler.CreateNotifyIconHandler();
            result.Click = RaiseClick;
            result.DoubleClick = RaiseDoubleClick;
            return result;
        }

        private void RaiseClick()
        {
            OnClick(EventArgs.Empty);
        }

        private void RaiseDoubleClick()
        {
            OnDoubleClick(EventArgs.Empty);
        }
    }
}