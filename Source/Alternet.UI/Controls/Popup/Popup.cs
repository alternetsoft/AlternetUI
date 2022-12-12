using Alternet.Drawing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// The <see cref="Popup"/> control displays content in a separate window that floats over the current application window.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Popup : Control
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
        /// Gets or sets the size of the popup.
        /// </summary>
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
        /// Gets or sets the width of the popup.
        /// </summary>
        public override double Width { get => Size.Width; set => Size = new Size(value, Height); }

        /// <summary>
        /// Gets or sets the height of the popup.
        /// </summary>
        public override double Height { get => Size.Height; set => Size = new Size(Width, value); }

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
        /// <param name="mode">Specifies how a window will size itself to fit the size of its content.</param>
        public void SetSizeToContent(WindowSizeToContentMode mode = WindowSizeToContentMode.WidthAndHeight)
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