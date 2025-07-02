using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        /// Synchronizes text property of the main control and value selected
        /// in the popup window which is shown when combo button is clicked.
        /// </summary>
        public virtual void SyncTextAndComboButton()
        {
            var btn = ButtonCombo;

            btn.BeforeShowPopup += (s, e) =>
            {
                if(DisposingOrDisposed || ButtonCombo.DisposingOrDisposed)
                    return;
                ButtonCombo.Value = Text;
            };

            btn.ValueChanged += (s, e) =>
            {
                if (DisposingOrDisposed || ButtonCombo.DisposingOrDisposed)
                    return;
                Text = ButtonCombo.GetValueAsString(ButtonCombo.Value) ?? string.Empty;
            };
        }

        /// <inheritdoc/>
        protected override Type GetBtnComboType()
        {
            return typeof(SpeedButtonWithListPopup);
        }
    }
}
