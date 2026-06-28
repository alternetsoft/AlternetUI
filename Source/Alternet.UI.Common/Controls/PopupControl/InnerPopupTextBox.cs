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
            /// Gets or sets a value indicating whether the popup should hide when clicking on the parent control.
            /// </summary>
            public bool HideClickOnParent { get; set; } = true;

            /// <summary>
            /// Gets or sets a value indicating whether text should be committed each time the text changes.
            /// If set to true, the text is commited on each text change. If false,
            /// the text is commited only when the user presses Enter.
            /// </summary>
            public bool CommitTextOnKeyPress { get; set; }

            /// <summary>
            /// Gets or sets the background color of the popup.
            /// </summary>
            public Color? BackColor { get; set; }

            /// <summary>
            /// Gets or sets the foreground color of the popup.
            /// </summary>
            public Color? ForeColor { get; set; }

            /// <summary>
            /// Gets or sets the bounds of the item being edited. This is used to position the popup control.
            /// Rectangle is in client coordinates of the <see cref="ItemContainer"/>.
            /// </summary>
            public RectD ItemRect;

            /// <summary>
            /// Gets or sets the container control that hosts the popup.
            /// This is used to determine the parent control for the popup.
            /// </summary>
            public AbstractControl? ItemContainer;

            /// <summary>
            /// Gets or sets the target control for which popup is being shown.
            /// This is different from <see cref="ItemContainer"/> in that it could be a generic control.
            /// </summary>
            public AbstractControl? TargetControl;

            /// <summary>
            /// Gets or sets the function that is called to get initial text for the <see cref="TextBox"/> control.
            /// </summary>
            public Func<string?>? GetItemText;

            /// <summary>
            /// Gets or sets the function that is called to set the text from the <see cref="TextBox"/>
            /// control back to the item being edited. This is only
            /// called if the user confirms the edit (e.g., by pressing Enter).
            /// </summary>
            public Action<string?>? SetItemText;

            /// <summary>
            /// Gets or sets the action that is called when the popup is closed.
            /// This can be used to perform any cleanup or additional actions after the popup is closed.
            /// </summary>
            public Action? Closed;

            /// <summary>
            /// Gets or sets the action that is called when the popup is closing.
            /// This can be used to perform any actions before the popup is closed.
            /// </summary>
            public Action? Closing;

            /// <summary>
            /// Gets or sets a value indicating whether the border should be visible.
            /// </summary>
            public bool HasBorder = true;

            /// <summary>
            /// Sets target control and item container for the popup.
            /// This is a convenience method to set both properties at once.
            /// Item container is determined by the target control's first parent which is a platform control.
            /// </summary>
            /// <param name="targetControl">The target control for which the popup is being shown.</param>
            /// <param name="itemRect">The bounds of the item being edited.</param>
            public void SetTargetControl(AbstractControl? targetControl, RectD itemRect)
            {
                TargetControl = targetControl;
                ItemContainer = targetControl;

                while (ItemContainer != null && !ItemContainer.IsPlatformControl)
                {
                    itemRect.Location += ItemContainer.Location;
                    ItemContainer = ItemContainer.Parent;
                }

                ItemRect = itemRect;
            }
        }

        /// <summary>
        /// Shows the popup control as an item editor for a specified item.
        /// </summary>
        /// <param name="prm">The parameters for showing the item editor.</param>
        public virtual bool ShowAsItemEditor(ShowAsItemEditorParams prm)
        {
            if (prm.ItemContainer is null)
                return false;

            targetControlUniqueId = prm.TargetControl?.UniqueId;

            var popupRect = prm.ItemRect;

            ResetClosedEvent();
            HideOnClickParent = prm.HideClickOnParent;
            BackColor = prm.BackColor ?? prm.ItemContainer.BackColor;
            ForeColor = prm.ForeColor ?? prm.ItemContainer.ForeColor;
            HasBorder = prm.HasBorder;
            Parent = prm.ItemContainer;
            Content.Text = prm.GetItemText?.Invoke() ?? string.Empty;

            void OnContentTextChanged(object? sender, EventArgs e)
            {
                if (!prm.CommitTextOnKeyPress)
                    return;
                var newText = Content.Text;
                prm.SetItemText?.Invoke(newText);
            }

            Content.TextChanged -= OnContentTextChanged;
            Content.TextChanged += OnContentTextChanged;

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
            if (e.Key == Key.Enter && !e.HasModifiers && HideOnEnter)
            {
                Close(ModalResult.Accepted, new(Key.Enter));
                e.Suppressed();
            }

            if (e.Key == Key.Escape && !e.HasModifiers && HideOnEscape)
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
