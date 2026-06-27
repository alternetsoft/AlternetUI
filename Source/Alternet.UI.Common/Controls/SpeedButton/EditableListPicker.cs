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
    /// a text box popup provided by <see cref="KnownPopupControls.GetPopupTextBox"/>.
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
                var popup = KnownPopupControls.Default.GetPopupTextBox();

                if (popup is null)
                    return false;
                if (!popup.IsVisible || popup.Parent is null)
                    return false;
                if(popup.TargetControlUniqueId != UniqueId)
                    return false;

                return true;
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

        /// <inheritdoc/>
        protected override void OnInsertedToWindow(Window parentWindow)
        {
            base.OnInsertedToWindow(parentWindow);
        }

        /// <inheritdoc/>
        protected override void OnRemovedFromWindow(Window parentWindow)
        {
            base.OnRemovedFromWindow(parentWindow);
        }

        /// <inheritdoc/>
        protected override void TogglePopupVisible(MouseEventArgs e)
        {
            if (!IsEditable || !Label.Bounds.Contains(e.Location))
            {
                if (IsEditing)
                {
                }
                else
                {
                    base.TogglePopupVisible(e);
                }
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

        /// <summary>
        /// Sets the text when user has finished editing.
        /// This method is called when the user has finished editing in the popup text box.
        /// By default this method calls <see cref="TextEdited"/> event
        /// to update the text. If event is not assigned, the new text is assigned to the
        /// <c>Value</c> property.
        /// </summary>
        /// <param name="text">The new text.</param>
        protected virtual void RaiseItemTextEdited(string? text)
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

        /// <summary>
        /// Starts editing the text in the control using popup text box.
        /// </summary>
        public virtual void BeginEdit()
        {
            var itemRect = Label.Bounds;

            var backColor = GetBackColor(VisualControlState.Normal);
            var foreColor = GetLabelTextColor(VisualControlState.Normal);

            backColor ??= DefaultColors.ControlBackColor.Current;
            foreColor ??= DefaultColors.ControlForeColor.Current;

            var s = RequestTextForItemEditor();

            InnerPopupTextBox.ShowAsItemEditorParams prm = new()
            {
                BackColor = backColor,
                ForeColor = foreColor,
                HasBorder = false,
                GetItemText = () => s,
                SetItemText = text =>
                {
                    RaiseItemTextEdited(text);
                },
            };

            prm.SetTargetControl(this, itemRect);

            var popup = KnownPopupControls.Default.GetPopupTextBox();

            if (popup is null)
                return;

            Post(() => {
                popup.ShowAsItemEditor(prm);
            });
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

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Label.Bounds.Contains(e.Location) && IsEditable)
                {                    
                    e.Handled = true;
                    BeginEdit();
                }
            }

            base.OnMouseDown(e);
        }
    }
}