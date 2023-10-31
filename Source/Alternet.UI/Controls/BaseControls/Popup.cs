using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// The <see cref="Popup"/> control displays content in a separate window that floats
    /// over the current application window.
    /// </summary>
    /// <remarks>
    /// <see cref="Popup"/> doesn't work properly on Linux. Please use <see cref="PopupWindow"/>
    /// </remarks>
    [System.ComponentModel.DesignerCategory("Code")]
    [ControlCategory("Hidden")]
    internal class Popup : Control
    {
        private Window? owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Popup()
        {
            SetVisibleValue(false);

            Bounds = new Rect(0, 0, 100, 100);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Owner"/> property changes.
        /// </summary>
        public event EventHandler? OwnerChanged;

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Popup;

        /// <summary>
        /// Gets or sets the window that owns this popup.
        /// </summary>
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
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user clicks mouse outside it or if it loses focus in any other way.
        /// </summary>
        /// <remarks>
        /// This style can be useful for implementing custom combobox-like controls for example.
        /// </remarks>
        public bool IsTransient
        {
            get
            {
                return Handler.NativeControl.IsTransient;
            }

            set
            {
                Handler.NativeControl.IsTransient = value;
            }
        }

        /// <summary>
        /// Gets or sets border style of the popup.
        /// </summary>
        public new ControlBorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set => base.BorderStyle = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a popup window will take focus from
        /// its parent window.
        /// </summary>
        /// <remarks>
        /// By default in Windows, a popup window will not take focus from its parent window.
        /// However many standard controls, including common ones such as <see cref="TextBox"/>,
        /// need focus to function correctly and will not work when placed on a default popup.
        /// This flag can be used to make the popup take focus and let all controls work but at
        /// the price of not allowing the parent window to keep focus while the popup is shown,
        /// which can also be sometimes desirable. This style is currently only implemented
        /// in Windows and simply does nothing under the other platforms.
        /// </remarks>
        public bool PuContainsControls
        {
            get
            {
                return Handler.NativeControl.PuContainsControls;
            }

            set
            {
                Handler.NativeControl.PuContainsControls = value;
            }
        }

        private new NativePopupHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativePopupHandler)base.Handler;
            }
        }

        /// <summary>
        /// Changes size of the window to fit the size of its content.
        /// </summary>
        /// <param name="mode">Specifies how a window will size itself to fit
        /// the size of its content.</param>
        public void SetSizeToContent(WindowSizeToContentMode mode =
            WindowSizeToContentMode.WidthAndHeight)
        {
            Handler.SetSizeToContent(mode);
        }

        /// <summary>
        /// Called when the value of the <see cref="Owner"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnOwnerChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler() => new NativePopupHandler();
    }
}