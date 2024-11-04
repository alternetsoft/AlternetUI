using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements popup control which is shown inside client area of another control.
    /// </summary>
    public partial class PopupControl : Border
    {
        private Control? container;
        private ModalResult popupResult = ModalResult.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControl"/> class.
        /// </summary>
        public PopupControl()
        {
            Visible = false;
        }

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
        /// Gets or sets the popup result value, which is updated when popup is closed.
        /// This property is set to <see cref="ModalResult.None"/> at the moment
        /// when popup is shown.
        /// </summary>
        [Browsable(false)]
        public virtual ModalResult PopupResult
        {
            get
            {
                return popupResult;
            }

            set
            {
                popupResult = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Space" key.
        /// </summary>
        public virtual bool AcceptOnSpace { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Tab" key.
        /// </summary>
        public virtual bool AcceptOnTab { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Enter" key.
        /// </summary>
        public virtual bool HideOnEnter { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup disappears automatically
        /// when the user presses "Escape" key.
        /// </summary>
        public virtual bool HideOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup disappears automatically
        /// when the user clicks mouse outside it or if it loses focus in any other way.
        /// </summary>
        public virtual bool HideOnDeactivate { get; set; }

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

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                PopupResult = ModalResult.None;
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeParentMouseDown(object? sender, MouseEventArgs e)
        {
            if (HideOnDeactivate && Visible)
            {
                PopupResult = ModalResult.Canceled;
                Close();
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeParentKeyDown(object? sender, KeyEventArgs e)
        {
            if (HideOnEscape && e.IsEscape)
            {
                PopupResult = ModalResult.Canceled;
                Close();
                e.Suppressed();
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeChildKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.IsEscape)
            {
                if(HideOnEscape)
                    CloseWithResult(ModalResult.Canceled);
                return;
            }

            if (e.IsEnter)
            {
                if(HideOnEnter)
                    CloseWithResult(ModalResult.Accepted);
                return;
            }

            if (e.IsSimpleKey(Key.Tab))
            {
                if(AcceptOnTab)
                    CloseWithResult(ModalResult.Accepted);
                return;
            }

            if (e.IsSimpleKey(Key.Space))
            {
                if(AcceptOnSpace)
                    CloseWithResult(ModalResult.Accepted);
                return;
            }

            void CloseWithResult(ModalResult value)
            {
                PopupResult = value;
                Close();
                e.Suppressed();
            }
        }
    }
}
