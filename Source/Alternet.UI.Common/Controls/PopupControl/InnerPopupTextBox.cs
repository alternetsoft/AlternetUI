using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a popup control that hosts a <see cref="TextBox"/> control.
    /// This popup control is intended to be used as an inner popup for editing text
    /// within another control.
    /// </summary>
    public partial class InnerPopupTextBox : PopupControl<TextBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupTextBox"/> class.
        /// </summary>
        public InnerPopupTextBox()
            : base()
        {
            FitIntoParent = false;
            HideOnEscape = true;
            HideOnEnter = true;
            HideOnClickParent = true;
            CancelOnLostFocus = true;
            Content.HasBorder = false;
            Content.AllowFormKeyPreview = false;
            Content.ProcessEnter = true;

            Content.LostFocus += OnContentLostFocus;

            Content.KeyDown += OnContentKeyDown;
            Content.KeyPress += OnContentKeyPress;
        }

        /// <summary>
        /// Defines the parameters for the <see cref="ShowAsItemEditor"/> method.
        /// </summary>
        public struct ShowAsItemEditorParams
        {
            /// <summary>
            /// Gets or sets the bounds of the item being edited. This is used to position the popup control.
            /// Rectangle is in client coordinates of the <see cref="ItemContainer"/>.
            /// </summary>
            public RectD ItemRect;

            /// <summary>
            /// Gets or sets the container control that hosts the item being edited.
            /// This is used to determine the parent control for the popup.
            /// </summary>
            public AbstractControl? ItemContainer;

            /// <summary>
            /// Gets the function that is called to get initial text for the <see cref="TextBox"/> control.
            /// </summary>
            public Func<string>? GetItemText;

            /// <summary>
            /// Gets the function that is called to set the text from the <see cref="TextBox"/>
            /// control back to the item being edited. This is only
            /// called if the user confirms the edit (e.g., by pressing Enter).
            /// </summary>
            public Action<string>? SetItemText;
        }

        /// <summary>
        /// Shows the popup control as an item editor for a specified item.
        /// </summary>
        /// <param name="prm">The parameters for showing the item editor.</param>
        public virtual void ShowAsItemEditor(ShowAsItemEditorParams prm)
        {
            var popupRect = prm.ItemRect;

            ResetClosedEvent();
            Parent = prm.ItemContainer;
            Content.Text = prm.GetItemText?.Invoke() ?? string.Empty;

            ClosedAction = () =>
            {
                prm.ItemContainer?.SetFocusIdle();
                if (PopupResult != ModalResult.Accepted)
                    return;
                var newText = Content.Text;
                prm.SetItemText?.Invoke(newText);
            };

            var preferredSize = Content.GetPreferredSize();
            var textBoxHeight = preferredSize.Height;

            if (textBoxHeight > popupRect.Height)
            {
                popupRect.Height = textBoxHeight;
                popupRect.Top = prm.ItemRect.Top - (textBoxHeight - prm.ItemRect.Height) / 2;
                if (popupRect.Top < ClientRectangle.Top)
                    popupRect.Top = ClientRectangle.Top;
            }

            Bounds = popupRect;

            Show();
            Content.SetFocusIfPossible();
        }

        /// <summary>
        /// Called when a character key is pressed while the content control has focus.
        /// This method can be overridden in derived classes
        /// to provide custom behavior for character key presses.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnContentKeyPress(object? sender, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// Called when a key is pressed while the content control has focus.
        /// This method can be overridden in derived classes to provide custom behavior for key presses.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnContentKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.HasModifiers)
            {
                Close(ModalResult.Accepted, new(Key.Enter));
                e.Suppressed();
            }

            if (e.Key == Key.Escape && !e.HasModifiers)
            {
                Close(ModalResult.Canceled, new(Key.Escape));
                e.Suppressed();
            }
        }

        /// <summary>
        /// Called when the content control loses focus. This method can be overridden in derived classes
        /// to provide custom behavior when the content control loses focus.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnContentLostFocus(object? sender, EventArgs e)
        {
        }
    }
}
