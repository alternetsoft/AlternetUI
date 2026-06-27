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
        /// Gets or sets a value indicating whether the text in the control is editable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEditable { get; set; } = true;

        /// <summary>
        /// Gets a value indicating whether the text in the control is currently being edited
        /// by <see cref="TextBox"/> control.
        /// </summary>
        [Browsable(false)]
        internal virtual bool IsEditing
        {
            get
            {
                return false;
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
            if (!Label.Bounds.Contains(e.Location))
            {
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

            InnerPopupTextBox.ShowAsItemEditorParams prm = new()
            {
                BackColor = backColor,
                ForeColor = foreColor,
                HasBorder = false,
                GetItemText = () => Text,
                SetItemText = text =>
                {
                    Value = text;
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