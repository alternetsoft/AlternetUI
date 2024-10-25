using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements popup control which is shown inside client area of another control.
    /// </summary>
    public partial class PopupControl : Border
    {
        private Control? container;

        /// <summary>
        /// Occurs when popup is closed
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Gets or sets container where popup will be shown.
        /// </summary>
        public virtual Control? Container
        {
            get
            {
                return container;
            }

            set
            {
                if (container == value)
                    return;
                container = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a popup disappears automatically
        /// when the user presses "Escape" key.
        /// </summary>
        public bool HideOnEscape { get; set; } = true;

        /// <summary>
        /// Closes popup window and raises <see cref="Closed"/> event.
        /// </summary>
        public virtual void Close()
        {
            Hide();
            Parent = null;
            App.DoEvents();
            Container?.SetFocusIfPossible();
            App.DoEvents();
            App.AddIdleTask(() =>
            {
                if (IsDisposed)
                    return;
                Closed?.Invoke(this, EventArgs.Empty);
            });
        }

        /// <summary>
        /// Handles common keys for all the popups. This includes "Escape" and other keys.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandlePopupKeyDown(object sender, KeyEventArgs e)
        {
            if (HideOnEscape && e.Key == Key.Escape)
            {
                Close();
                e.Suppressed();
            }
        }
    }
}
