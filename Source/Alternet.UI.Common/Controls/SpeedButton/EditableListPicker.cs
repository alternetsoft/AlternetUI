using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays an editable text combined with a drop down
    /// list box, which enables the user
    /// to select items from the list or enter a new value.
    /// <see cref="EditableListPicker"/> behaves like a combo box, but it is <see cref="SpeedButton"/>
    /// descendant, so it can be used in toolbars and other places where a button is needed.
    /// <see cref="EditableListPicker"/> doesn't have an internal text box, but it uses
    /// a text box popup provided by the application handler.
    /// This text box is shown as a popup window when the user starts to edit the text.
    /// <see cref="EditableListPicker"/> is a generic control and is not attached to any native control of the
    /// operating system. It is implemented using other controls, so it can be used in any environment where 
    /// a native combo box is not available or suitable.
    /// </summary>
    public partial class EditableListPicker : ListPicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableListPicker"/> class.
        /// </summary>
        public EditableListPicker()
        {
        }

        /// <summary>
        /// Occurs when the text is edited. In the event handler
        /// you need to apply the new text to the item. The event is raised when the user presses Enter or
        /// when the editing is finished programmatically.
        /// </summary>
        public event EventHandler<StringEventArgs>? TextEdited;

        /// <summary>
        /// Occurs when the text is requested for the editor. In the event handler
        /// you need to provide the text which will be assigned to the text box editor.
        /// The event is raised when the user starts editing the text.   
        /// </summary>
        public event EventHandler<StringEventArgs>? EditorTextRequested;

        /// <summary>
        /// Occurs when the Tab key is pressed. In the event handler
        /// you can handle the Tab key press event.
        /// </summary>
        public event EventHandler? TabPressed;

        /// <summary>
        /// Occurs when the Enter key is pressed. In the event handler
        /// you can handle the Enter key press event.
        /// </summary>
        public event EventHandler? EnterPressed;

        /// <summary>
        /// Occurs when the Escape key is pressed. In the event handler
        /// you can handle the Escape key press event.
        /// </summary>
        public event EventHandler? EscapePressed;
        
        /// <summary>
        /// Occurs when a key is pressed in the inplace editor.
        /// </summary>
        public event EventHandler<KeyEventArgs>? EditorKeyDown;

        /// <summary>
        /// Gets or sets a value indicating whether the text in the control is editable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEditable { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether the text in the control is currently being edited
        /// by <see cref="TextBox"/> control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEditing
        {
            get
            {
                return ControlFactory.PopupEntryHandler?.HasActivePopupEntry(UniqueId) ?? false;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying the style of the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ComboBoxStyle" /> values.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(ComboBoxStyle.DropDown)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public override ComboBoxStyle DropDownStyle
        {
            get
            {
                if (IsEditable)
                    return ComboBoxStyle.DropDown;
                else
                    return ComboBoxStyle.DropDownList;
            }

            set
            {
                if (DropDownStyle == value)
                    return;
                switch (value)
                {
                    case ComboBoxStyle.DropDown:
                        IsEditable = true;
                        break;
                    case ComboBoxStyle.DropDownList:
                        IsEditable = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets empty text hint displayed in the control when the text is empty.
        /// </summary>
        public virtual string? EmptyTextHint
        {
            get
            {
                return Label.EmptyTextHint;
            }

            set
            {
                Label.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Sets minimal height of the control using <see cref="IPopupEntryHandler.GetPopupEntryHeight"/>.
        /// </summary>
        public virtual void MinHeightFromPopupEntry()
        {
            if (ControlFactory.PopupEntryHandler is null)
                return;
            Label.MinHeight = ControlFactory.PopupEntryHandler.GetPopupEntryHeight(Label, RealFont, true);
        }

        /// <inheritdoc/>
        protected override void TogglePopupVisible(MouseEventArgs e)
        {
            if (!IsEditable || !Label.Bounds.Contains(e.Location))
            {
                CancelEdit();
                base.TogglePopupVisible(e);
            }
        }

        /// <inheritdoc/>
        protected override KnownTheme GetDefaultUseTheme()
        {
            return KnownTheme.StaticBorderNoHover;
        }

        /// <inheritdoc/>
        public override bool SetFocus()
        {
            return base.SetFocus();
        }

        /// <inheritdoc/>
        public override void ShowPopup()
        {
            if (ImageVisible)
                base.ShowPopup();
        }

        /// <summary>
        /// Sets the text when user has finished editing.
        /// This method is called when the user has finished editing in the popup text box.
        /// By default this method calls <see cref="TextEdited"/> event
        /// to update the text. If event is not assigned, the new text is assigned to the
        /// <c>Value</c> property.
        /// </summary>
        /// <param name="text">The new text.</param>
        protected virtual void RaiseTextEdited(string? text)
        {
            if (TextEdited != null)
            {
                var args = new StringEventArgs(text ?? string.Empty);

                TextEdited(this, args);
            }
            else
            {
                Value = text;
            }
        }

        /// <inheritdoc/>
        public override void UpdateBaseText()
        {
            base.UpdateBaseText();
        }

        /// <summary>
        /// Ends the editing of the text in the control.
        /// </summary>
        public virtual void CancelEdit()
        {
            ControlFactory.PopupEntryHandler?.CloseActivePopupEntry(UniqueId);
        }

        /// <summary>
        /// Starts editing the text in the control using popup text box.
        /// </summary>
        public virtual void BeginEdit()
        {
            var prm = CreatePopupEditorParams();

            if (prm is null)
                return;

            Post(() => {
                ControlFactory.PopupEntryHandler?.ShowPopupEntry(prm.Value);
            });
        }

        /// <summary>
        /// Creates parameters for the popup text box editor.
        /// </summary>
        /// <returns>The parameters for the popup text box editor.</returns>
        protected virtual PopupEntryParams? CreatePopupEditorParams()
        {
            var backColor = GetBackColor(VisualControlState.Normal);
            var foreColor = GetLabelTextColor(VisualControlState.Normal);

            backColor ??= DefaultColors.ControlBackColor.Current;
            foreColor ??= DefaultColors.ControlForeColor.Current;

            var s = RequestTextForItemEditor();

            PopupEntryParams prm = new()
            {
                DebugIdentifier = DebugIdentifier ?? Label.DebugIdentifier,
                BackColor = backColor,
                Font = Label.RealFont,
                ForeColor = foreColor,
                HideClickOnParent = false,
                CommitTextOnKeyPress = true,
                HideOnEscape = false,
                HideOnEnter = false,
                HasBorder = false,
                EmptyTextHint = this.EmptyTextHint,
                TabPressed = () =>
                {
                    TabPressed?.Invoke(this, EventArgs.Empty);
                },
                EnterPressed = () =>
                {
                    EnterPressed?.Invoke(this, EventArgs.Empty);
                },
                EscapePressed = () =>
                {
                    EscapePressed?.Invoke(this, EventArgs.Empty);
                },
                EntryHeightChanged = OnPopupEntryHeightChanged,
                KeyDown = (s, e) => EditorKeyDown?.Invoke(this, e),
                GetItemText = () => s,
                SetItemText = text =>
                {
                    RaiseTextEdited(text);
                },
            };

            prm.SetTargetControl(this, () => Label.Bounds);

            return prm;
        }

        /// <summary>
        /// Gets text that can be used in the text box
        /// when user is editing the item. This method is called when
        /// the user starts editing.
        /// </summary>
        /// <returns>The text for the editor.</returns>
        protected virtual string? RequestTextForItemEditor()
        {
            var result = Text ?? string.Empty;

            if (EditorTextRequested is not null)
            {
                var e = new StringEventArgs(result);
                EditorTextRequested(this, e);
                result = e.Value;
            }

            return result;
        }

        /// <summary>
        /// This method is called when the height of the popup entry changes.
        /// </summary>
        /// <param name="itemHeight">The new height of the popup entry.</param>
        protected virtual void OnPopupEntryHeightChanged(float itemHeight)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            CancelEdit();
            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Label.Bounds.Contains(e.Location) && IsEditable)
                {
                    e.Handled = true;

                    ControlFactory.PopupEntryHandler?.CloseAllPopupEntries();

                    Post(() =>
                    {
                        BeginEdit();
                    });

                }
            }

            base.OnMouseDown(e);
        }
    }
}