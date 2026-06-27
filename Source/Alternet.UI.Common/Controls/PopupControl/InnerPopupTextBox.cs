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

            Content.VerticalAlignment = VerticalAlignment.Center;
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
            /// Initializes a new instance of the <see cref="ShowAsItemEditorParams"/> struct.
            /// </summary>
            public ShowAsItemEditorParams()
            {
            }

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
            public Func<string?>? GetItemText;

            /// <summary>
            /// Gets the function that is called to set the text from the <see cref="TextBox"/>
            /// control back to the item being edited. This is only
            /// called if the user confirms the edit (e.g., by pressing Enter).
            /// </summary>
            public Action<string?>? SetItemText;

            /// <summary>
            /// Gets or sets a value indicating whether the border should be visible.
            /// </summary>
            public bool HasBorder = true;
        }

        /// <summary>
        /// Shows the popup control as an item editor for a specified item.
        /// </summary>
        /// <param name="prm">The parameters for showing the item editor.</param>
        public virtual bool ShowAsItemEditor(ShowAsItemEditorParams prm)
        {
            if (prm.ItemContainer is null)
                return false;

            var popupRect = prm.ItemRect;

            ResetClosedEvent();
            HasBorder = prm.HasBorder;
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

            var preferredSize = GetPreferredSize();
            var textBoxHeight = preferredSize.Height;

            popupRect.Height = MathF.Max(textBoxHeight, prm.ItemRect.Height);

            if (popupRect.Height != prm.ItemRect.Height)
            {
                popupRect.Top += (textBoxHeight - prm.ItemRect.Height) / 2;
            }

            var containerRect = prm.ItemContainer.ClientRectangle;

            if (popupRect.Bottom > containerRect.Bottom)
                popupRect.Bottom = containerRect.Bottom;
            if (popupRect.Top < containerRect.Top)
                popupRect.Top = containerRect.Top;

            Bounds = popupRect;

            Show();
            Content.SetFocusIfPossible();
            return true;
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
