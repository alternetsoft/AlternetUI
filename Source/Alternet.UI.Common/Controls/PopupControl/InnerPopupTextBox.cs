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
        private ObjectUniqueId? targetControlUniqueId;

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupTextBox"/> class.
        /// </summary>
        public InnerPopupTextBox()
            : base()
        {
            FitIntoParent = false;
            HideOnEscape = true;
            HideOnEnter = true;
            CancelOnLostFocus = true;
            Content.HasBorder = false;
            Content.AllowFormKeyPreview = false;
            Content.ProcessEnter = true;
            Content.WantTab = true;

            Content.VerticalAlignment = VerticalAlignment.Center;
            Content.LostFocus += OnContentLostFocus;

            Content.KeyDown += OnContentKeyDown;
            Content.KeyPress += OnContentKeyPress;

            ParentForeColor = false;
            ParentBackColor = false;
            AutoUpdateColors = false;

            Content.AutoUpdateColors = false;
            Content.ParentBackColor = true;
            Content.ParentForeColor = true;
        }

        /// <summary>
        /// Gets the unique identifier of the target control that this popup is associated with.
        /// </summary>
        public ObjectUniqueId? TargetControlUniqueId => targetControlUniqueId;

        /// <summary>
        /// Gets or sets the action to be performed when the Tab key is pressed while the popup is active.
        /// </summary>
        public Action? TabPressedAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be performed when the Enter key is pressed while the popup is active.
        /// </summary>
        public Action? EnterPressedAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be performed when the Escape key is pressed while the popup is active.
        /// </summary>
        public Action? EscapePressedAction { get; set; }

        /// <summary>
        /// Gets or sets the action to be performed when a key is pressed while the popup is active.
        /// </summary>
        public KeyEventHandler? ContentKeyDownAction { get; set; }

        /// <summary>
        /// Shows the popup control as an item editor for a specified item.
        /// </summary>
        /// <param name="prm">The parameters for showing the item editor.</param>
        public virtual bool ShowAsItemEditor(PopupEntryParams prm)
        {
            if (prm.ItemContainer is null)
                return false;

            targetControlUniqueId = prm.TargetControl?.UniqueId;

            var popupRect = prm.ItemRect;

            ResetClosedEvent();
            HideOnClickParent = prm.HideClickOnParent;
            HideOnEscape = prm.HideOnEscape;
            Content.EmptyTextHint = prm.EmptyTextHint;
            HideOnEnter = prm.HideOnEnter;
            BackColor = prm.BackColor ?? prm.ItemContainer.BackColor;
            ForeColor = prm.ForeColor ?? prm.ItemContainer.ForeColor;
            ParentFont = false;
            Font = prm.Font ?? Control.DefaultFont;
            HasBorder = prm.HasBorder;
            Parent = prm.ItemContainer;
            Content.Text = prm.GetItemText?.Invoke() ?? string.Empty;
            Content.IsPassword = prm.IsPassword;

            void OnContentTextChanged(object? sender, EventArgs e)
            {
                if (!prm.CommitTextOnKeyPress)
                    return;
                var newText = Content.Text;
                prm.SetItemText?.Invoke(newText);
            }

            Content.TextChanged -= OnContentTextChanged;
            Content.TextChanged += OnContentTextChanged;

            TabPressedAction = prm.TabPressed;
            EnterPressedAction = prm.EnterPressed;
            EscapePressedAction = prm.EscapePressed;
            ContentKeyDownAction = prm.KeyDown;

            ClosingAction = () =>
            {
                Content.TextChanged -= OnContentTextChanged;

                prm.ItemContainer?.SetFocusIdle();

                prm.Closing?.Invoke();

                if (PopupResult != ModalResult.Accepted)
                    return;
                var newText = Content.Text;
                prm.SetItemText?.Invoke(newText);
            };

            ClosedAction = () =>
            {
                prm.Closed?.Invoke();
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
            Content.MoveToEndOfText();
            Content.SelectAll();
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
            ContentKeyDownAction?.Invoke(sender, e);

            if (e.Key == Key.Tab)
            {
                TabPressedAction?.Invoke();
                e.Suppressed();
                return;
            }

            if (e.Key == Key.Enter && !e.HasModifiers)
            {
                EnterPressedAction?.Invoke();

                if (HideOnEnter)
                {
                    Close(ModalResult.Accepted, new(Key.Enter));
                }

                e.Suppressed();
                return;
            }

            if (e.Key == Key.Escape && !e.HasModifiers)
            {
                EscapePressedAction?.Invoke();

                if (HideOnEscape)
                {
                    Close(ModalResult.Canceled, new(Key.Escape));
                }

                e.Suppressed();
                return;
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
