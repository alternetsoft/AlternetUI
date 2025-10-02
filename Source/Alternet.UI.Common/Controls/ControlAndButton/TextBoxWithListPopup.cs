using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a <see cref="TextBox"/> with a combo button that, when clicked,
    /// displays a popup window containing a <see cref="VirtualListBox"/>.
    /// </summary>
    public partial class TextBoxWithListPopup : TextBoxAndButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxWithListPopup"/> class.
        /// </summary>
        public TextBoxWithListPopup()
        {
            HasBtnComboBox = true;
            InnerOuterBorder = InnerOuterSelector.Outer;
            ButtonCombo.PopupOwner = MainControl;
        }

        /// <summary>
        /// Occurs when the drop-down portion of the control is no longer visible.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? DropDownClosed;

        /// <summary>
        /// Occurs when the drop-down portion of the control is shown.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? DropDown;

        /// <summary>
        /// Gets a value indicating whether the combo box is displaying its drop-down portion.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the drop-down portion is displayed;
        /// otherwise, <see langword="false" />. The default is false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool DroppedDown
        {
            get
            {
                return ButtonCombo.IsPopupWindowCreated && ButtonCombo.PopupWindow.Visible;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (DroppedDown == value)
                    return;
                if (value)
                    ShowPopup();
                else
                    HidePopup();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to perform lookup by value
        /// when popup window is opened.
        /// </summary>
        public virtual bool LookupByValue
        {
            get => ButtonCombo.LookupByValue;
            set => ButtonCombo.LookupByValue = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to perform an exact text lookup
        /// when popup window is opened.
        /// </summary>
        public virtual bool LookupExactText
        {
            get => ButtonCombo.LookupExactText;
            set => ButtonCombo.LookupExactText = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore case when looking up items
        /// in the popup window.
        /// </summary>
        public virtual bool LookupIgnoreCase
        {
            get => ButtonCombo.LookupIgnoreCase;
            set => ButtonCombo.LookupIgnoreCase = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use a context menu as the popup window.
        /// </summary>
        /// <remarks>
        /// If set to <see langword="true"/>, the popup window will be a context menu.
        /// Otherwise, it will use the default popup kind.
        /// </remarks>
        public virtual bool UseContextMenuAsPopup
        {
            get
            {
                return ButtonCombo.UseContextMenuAsPopup;
            }

            set
            {
                ButtonCombo.UseContextMenuAsPopup = value;
            }
        }

        /// <summary>
        /// Gets simple items where item is <c>object</c>.
        /// It is mapped from <see cref="ListControlItem.Value"/> elements
        /// of the <see cref="Items"/> collection.
        /// </summary>
        public virtual ListBoxItems SimpleItems
        {
            get
            {
                return ButtonCombo.SimpleItems;
            }
        }

        /// <summary>
        /// Gets the list box control used in the popup window.
        /// </summary>
        public VirtualListBox ListBox
        {
            get
            {
                return ButtonCombo.ListBox;
            }
        }

        /// <summary>
        /// Gets the collection of items used in the list box control within the popup window.
        /// </summary>
        public virtual BaseCollection<ListControlItem> Items
        {
            get
            {
                return ButtonCombo.Items;
            }

            set
            {
                ButtonCombo.Items = value;
            }
        }

        /// <summary>
        /// Gets combo button if it is available.
        /// </summary>
        [Browsable(false)]
        public new SpeedButtonWithListPopup ButtonCombo
        {
            get
            {
                return (SpeedButtonWithListPopup)base.ButtonCombo!;
            }
        }

        /// <summary>
        /// Gets the starting index of text selected in the combo box.
        /// </summary>
        /// <value>The zero-based index of the first character in the string
        /// of the current text selection.</value>
        [Browsable(false)]
        public virtual int TextSelectionStart
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return MainControl.SelectionStart;
            }
        }

        /// <summary>
        /// Gets the number of characters selected in the editable portion
        /// of the combo box.
        /// </summary>
        /// <value>The number of characters selected.</value>
        [Browsable(false)]
        public virtual int TextSelectionLength
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return MainControl.SelectionLength;
            }
        }

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused text control.
        /// </summary>
        public virtual string? EmptyTextHint
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                return MainControl.EmptyTextHint;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                MainControl.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Adds a collection of items to the control.
        /// </summary>
        /// <remarks>If an item in the collection is already a <see cref="ListControlItem"/>,
        /// it is added directly. Otherwise, a new <see cref="ListControlItem"/> is created
        /// for the item, with its <c>Value</c>
        /// property set to the item, and then added to the list.</remarks>
        /// <param name="items">The collection of items to add.
        /// Each item can either be a <see cref="ListControlItem"/> or an object that
        /// will be wrapped in a new <see cref="ListControlItem"/>.</param>
        public virtual void AddRange(IEnumerable items)
        {
            ButtonCombo.AddRange(items);
        }

        /// <summary>
        /// Shows the popup window associated with the control.
        /// </summary>
        public virtual void ShowPopup()
        {
            App.AddIdleTask(() =>
            {
                if (DisposingOrDisposed || ButtonCombo.DisposingOrDisposed)
                    return;
                ButtonCombo.ShowPopup();
            });
        }

        /// <summary>
        /// Synchronizes text property of the main control and value selected
        /// in the popup window which is shown when combo button is clicked.
        /// </summary>
        public virtual void SyncTextAndComboButton()
        {
            var btn = ButtonCombo;

            btn.BeforeShowPopup -= OnButtonComboBeforeShowPopup;
            btn.BeforeShowPopup += OnButtonComboBeforeShowPopup;

            btn.ValueChanged -= OnButtonComboValueChanged;
            btn.ValueChanged += OnButtonComboValueChanged;
        }

        /// <summary>
        /// Hides the popup window associated with the control and sets
        /// the result of the popup window operation.
        /// </summary>
        /// <param name="result">The result of the popup window operation.</param>
        public virtual void HidePopup(ModalResult result = ModalResult.Canceled)
        {
            if (DisposingOrDisposed || ButtonCombo.DisposingOrDisposed)
                return;
            if(!ButtonCombo.IsPopupWindowCreated)
                return;
            ButtonCombo.PopupWindow.HidePopup(result);
        }

        /// <summary>
        /// Raises the <see cref="DropDown"/> event and calls <see cref="OnDropDown"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseDropDown()
        {
            if (DisposingOrDisposed)
                return;
            OnDropDown(EventArgs.Empty);
            DropDown?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="DropDownClosed"/> event and
        /// calls <see cref="OnDropDownClosed"/> method.
        /// </summary>
        [Browsable(false)]
        public void RaiseDropDownClosed()
        {
            if (DisposingOrDisposed)
                return;
            OnDropDownClosed(EventArgs.Empty);
            DropDownClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="DropDown"/> event is fired.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDropDown(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="DropDownClosed"/> event is fired.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDropDownClosed(EventArgs e)
        {
        }

        /// <summary>
        /// Handles the event triggered when the data value of the button combo changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments associated with the value change.</param>
        protected virtual void OnButtonComboValueChanged(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed || ButtonCombo.DisposingOrDisposed)
                return;
            Text = ButtonCombo.GetValueAsString(ButtonCombo.Value) ?? string.Empty;
            RaiseDropDownClosed();
        }

        /// <summary>
        /// Handles the event triggered before the popup is displayed for the button combo.
        /// </summary>
        /// <remarks>This method sets the popup owner and value for the button combo control
        /// based on the current state.</remarks>
        /// <param name="sender">The source of the event, typically the button combo control.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnButtonComboBeforeShowPopup(object? sender, EventArgs e)
        {
            if (DisposingOrDisposed || ButtonCombo.DisposingOrDisposed)
                return;
            ButtonCombo.PopupOwner = this;
            ButtonCombo.Value = Text;
            RaiseDropDown();
        }

        /// <inheritdoc/>
        protected override void OnSubstituteControlMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            base.OnSubstituteControlMouseLeftButtonDown(sender, e);
            if (e.Handled)
                return;
            DroppedDown = !DroppedDown;
            e.Handled = true;
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.Handled)
                return;
            DroppedDown = !DroppedDown;
            e.Handled = true;
        }

        /// <inheritdoc/>
        protected override void UpdateSubstituteControlText()
        {
            if (!IsSubstituteControlCreated)
                return;
            var useTextHint = string.IsNullOrEmpty(MainControl.Text);
            SubstituteControl.Text = useTextHint ? EmptyTextHint ?? string.Empty : MainControl.Text;
            SubstituteControl.Refresh();
        }

        /// <inheritdoc/>
        protected override Type GetBtnComboType()
        {
            return typeof(SpeedButtonWithListPopup);
        }
    }
}
