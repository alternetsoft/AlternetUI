using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a component that creates an icon in the notification area.
    /// </summary>
    /// <remarks>
    /// Icons in the notification area are shortcuts to processes that are running in the background of a computer, such
    /// as a virus protection program or a volume control. These processes do not come with their own user interfaces.
    /// The <see cref="NotifyIcon"/> class provides a way to program in this functionality. The <see cref="Icon"/>
    /// property defines the icon that appears in the notification area. Pop-up menus for an icon are addressed with the
    /// <see cref="Menu"/> property. The <see cref="Text"/> property assigns tool tip text. In order for the icon to
    /// show up in the notification area, the Visible property must be set to true.
    /// </remarks>
    public class NotifyIcon : IDisposable
    {
        private bool isDisposed;

        private Native.NotifyIcon nativeNotifyIcon;

        private Image? icon;

        private ContextMenu? menu;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyIcon"/> class.
        /// </summary>
        public NotifyIcon()
        {
            nativeNotifyIcon = new Native.NotifyIcon();
            nativeNotifyIcon.Click += NativeNotifyIcon_Click;
            nativeNotifyIcon.DoubleClick += NativeNotifyIcon_DoubleClick;
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
                return Native.NotifyIcon.IsAvailable;
            }
        }

        /// <summary>
        /// Gets whether icon was actually installed and visible.
        /// </summary>
        public bool IsIconInstalled
        {
            get
            {
                CheckDisposed();
                return nativeNotifyIcon.IsIconInstalled;
            }
        }

        /// <summary>
        /// Gets whether this object initialized successfully.
        /// </summary>
        public bool IsOk
        {
            get
            {
                CheckDisposed();
                return nativeNotifyIcon.IsOk;
            }
        }

        /// <summary>
        /// Gets or sets the tool tip text displayed when the mouse pointer rests on a notification area icon.
        /// </summary>
        /// <value>
        /// The tool tip text displayed when the mouse pointer rests on a notification area icon.
        /// </value>
        /// <remarks>
        /// If the text is <see langword="null"/>, no tool tip is displayed.
        /// </remarks>
        public string? Text
        {
            get
            {
                CheckDisposed();
                return nativeNotifyIcon.Text;
            }

            set
            {
                CheckDisposed();
                nativeNotifyIcon.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the current icon.
        /// </summary>
        /// <value>
        /// The <see cref="Image"/> displayed by the <see cref="NotifyIcon"/> component. The default value is <see langword="null"/>.
        /// </value>
        public Image? Icon
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
                nativeNotifyIcon.Icon = value?.NativeImage ?? null;
            }
        }

        /// <summary>
        /// Gets or sets the menu associated with the <see cref="NotifyIcon"/>.
        /// </summary>
        public ContextMenu? Menu
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

                if (value == null)
                    nativeNotifyIcon.Menu = null;
                else
                    nativeNotifyIcon.Menu = ((NativeContextMenuHandler)value.Handler).NativeControl;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the icon is visible in the notification area of the taskbar.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the icon is visible in the notification area; otherwise,
        /// <see langword="false"/>. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// Since the default value is <see langword="false"/>, in order for the icon to show up in the notification
        /// area, you must set the <see cref="Visible"/> property to <see langword="true"/>.
        /// </remarks>
        public bool Visible
        {
            get
            {
                CheckDisposed();
                return nativeNotifyIcon.Visible;
            }

            set
            {
                CheckDisposed();
                nativeNotifyIcon.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets an arbitrary <see cref="object"/> representing some type of user state.
        /// </summary>
        /// <value>An arbitrary <see cref="object"/> representing some type of user state.</value>
        public object? Tag { get; set; }

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    nativeNotifyIcon.Click -= NativeNotifyIcon_Click;
                    nativeNotifyIcon.DoubleClick -= NativeNotifyIcon_DoubleClick;
                    nativeNotifyIcon.Dispose();
                    nativeNotifyIcon = null!;
                }

                isDisposed = true;
            }
        }

        private void NativeNotifyIcon_Click(object? sender, EventArgs e)
        {
            OnClick(EventArgs.Empty);
        }

        private void NativeNotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            OnDoubleClick(EventArgs.Empty);
        }

        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }
    }
}