using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class ListBox
    {
        private bool horizontalScrollbar;
        private bool integralHeight = true;
        private bool scrollAlwaysVisible;

        /// <summary>
        /// This event is not relevant for this control and is hidden from intellisense.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler? PaddingChanged
        {
            add => base.PaddingChanged += value;
            remove => base.PaddingChanged -= value;
        }

        /// <summary>
        /// This event is not relevant for this control and is hidden from intellisense.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler? TextChanged
        {
            add => base.TextChanged += value;
            remove => base.TextChanged -= value;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex" /> property or the <see cref="SelectedIndices" />
        /// collection has changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? SelectedIndexChanged;

        /// <summary>
        /// This property is not relevant for this control and is hidden from intellisense.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        /// <summary>
        /// Indicates whether or not the list box should display a horizontal scrollbar
        /// when the items extend beyond the right edge of the list box.
        /// If true, the scrollbar will automatically set its extent depending on the length
        /// of items in the list box. This property may not be supported on some platforms.
        /// Default is false.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(true)]
        public new virtual bool HorizontalScrollbar
        {
            get
            {
                return horizontalScrollbar;
            }

            set
            {
                if (value != horizontalScrollbar)
                {
                    horizontalScrollbar = value;

                    SetHandlerFlag(ListBoxHandlerFlags.ShowHorzScrollWhenNeeded, value);
                }
            }
        }

        /// <summary>
        /// Indicates if the list box should avoid showing partial items. If so,
        /// then only full items will be displayed, and the list box will be resized
        /// to prevent partial items from being shown.  Otherwise, they will be shown.
        /// Default is true.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(true)]
        [Description("Indicates whether the list box should avoid showing partial items.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual bool IntegralHeight
        {
            get
            {
                return integralHeight;
            }

            set
            {
                if (integralHeight != value)
                {
                    integralHeight = value;
                    SetHandlerFlag(ListBoxHandlerFlags.IntegralHeight, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the vertical scrollbar is shown at all times.
        /// Default is false, which means the scrollbar is shown only when needed.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool ScrollAlwaysVisible
        {
            get
            {
                return scrollAlwaysVisible;
            }

            set
            {
                if (scrollAlwaysVisible != value)
                {
                    scrollAlwaysVisible = value;
                    SetHandlerFlag(ListBoxHandlerFlags.AlwaysShowVertScroll, value);
                }
            }
        }

        /// <summary>
        /// The index of the currently selected item in the list, if there
        /// is one. If the value is -1, there is currently no selection. If the
        /// value is 0 or greater, than the value is the index of the currently
        /// selected item.
        /// selection.
        /// </summary>
        [Browsable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int SelectedIndex
        {
            get
            {
                return GetSelection();
            }

            set
            {
                var oldSelectedIndex = SelectedIndex;
                if (oldSelectedIndex == value)
                    return;
                SetSelection(value);
            }
        }

        /// <summary>
        /// A collection of the indices of the selected items in the
        /// list box. If there are no selected items in the list box, the result is
        /// an empty collection.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IReadOnlyList<int> SelectedIndices
        {
            get
            {
                var result = ListUtils.GetSelectedItems(
                    PlatformControl.UpdateSelections,
                    PlatformControl.GetSelectionsCount,
                    PlatformControl.GetSelectionsItem);

                return result;
            }
        }

        /// <summary>
        /// The value of the currently selected item in the list, if there
        /// is one. If the value is null, there is currently no selection. If the
        /// value is non-null, then the value is that of the currently selected
        /// item. If the MultiSelect property on the list box is true, then a
        /// non-null return value for this method is the value of the first item selected.
        /// </summary>
        [Browsable(false)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object? SelectedItem
        {
            get
            {
                var index = SelectedIndex;
                if (index < 0 || index >= Items.Count)
                    return null;
                return Items[index];
            }

            set
            {
                if (value == null)
                {
                    SelectedIndex = -1;
                    return;
                }

                var index = Items.IndexOf(value);
                SelectedIndex = index;
            }
        }

        /// <summary>
        /// The collection of selected items.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<object> SelectedItems
        {
            get
            {
                object GetItemAtIndex(int index)
                {
                    var itemIndex = PlatformControl.GetSelectionsItem(index);
                    if (itemIndex >= 0 && itemIndex < Items.Count)
                        return Items[itemIndex];
                    return null!;
                }

                var result = ListUtils.GetSelectedItems(
                    PlatformControl.UpdateSelections,
                    PlatformControl.GetSelectionsCount,
                    GetItemAtIndex);

                return result;
            }
        }

        /// <summary>
        /// Controls how many items at a time can be selected in the list box. Valid
        /// values are from the <see cref="Alternet.UI.SelectionMode"/> enumeration.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(SelectionMode.One)]
        public virtual SelectionMode SelectionMode
        {
            get
            {
                if (PlatformControl.Flags.HasFlag(ListBoxHandlerFlags.SingleSelection))
                    return SelectionMode.One;
                else if (PlatformControl.Flags.HasFlag(ListBoxHandlerFlags.MultipleSelection))
                    return SelectionMode.MultiSimple;
                else if (PlatformControl.Flags.HasFlag(ListBoxHandlerFlags.ExtendedSelection))
                    return SelectionMode.MultiExtended;
                else
                    return SelectionMode.None;
            }

            set
            {
                if(SelectionMode == value)
                    return;
                switch (value)
                {
                    case SelectionMode.None:
                        ChangeHandlerFlags(
                            ListBoxHandlerFlags.None,
                            ListBoxHandlerFlags.SingleSelection | ListBoxHandlerFlags.MultipleSelection | ListBoxHandlerFlags.ExtendedSelection);
                        break;
                    case SelectionMode.One:
                        ChangeHandlerFlags(
                            ListBoxHandlerFlags.SingleSelection,
                            ListBoxHandlerFlags.MultipleSelection | ListBoxHandlerFlags.ExtendedSelection);
                        break;
                    case SelectionMode.MultiSimple:
                        ChangeHandlerFlags(
                            ListBoxHandlerFlags.MultipleSelection,
                            ListBoxHandlerFlags.SingleSelection | ListBoxHandlerFlags.ExtendedSelection);
                        break;
                    case SelectionMode.MultiExtended:
                        ChangeHandlerFlags(
                            ListBoxHandlerFlags.ExtendedSelection,
                            ListBoxHandlerFlags.SingleSelection | ListBoxHandlerFlags.MultipleSelection);
                        break;
                }
            }
        }

        /// <summary>
        /// The index of the first visible item in a list box. Initially
        /// the item with index 0 is at the top of the list box, but if the list
        /// box contents have been scrolled another item may be at the top.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int TopIndex
        {
            get => PlatformControl.GetTopItem();

            set
            {
                if (value < 0 || value >= Items.Count)
                    return;
                PlatformControl.SetFirstItem(value);
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Bindable(false)]
        [AllowNull]
        public override string Text
        {
            get
            {
                var selectedItem = SelectedItem;

                if (SelectionMode != SelectionMode.None && selectedItem is not null)
                {
                    return GetItemText(selectedItem) ?? string.Empty;
                }
                else
                {
                    return base.Text;
                }
            }

            set
            {
                base.Text = value ?? string.Empty;

                if (SelectionMode != SelectionMode.None && value is not null
                    && (SelectedItem is null || !value.Equals(GetItemText(SelectedItem))))
                {
                    int cnt = Items.Count;
                    for (int index = 0; index < cnt; ++index)
                    {
                        if (string.Compare(value, GetItemText(Items[index]), true, CultureInfo.CurrentCulture) == 0)
                        {
                            SelectedIndex = index;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="SelectedIndexChanged"/> event and <see cref="OnSelectedIndexChanged"/> method.
        /// </summary>
        /// <remarks>This method invokes the <see cref="SelectedIndexChanged"/> event, allowing
        /// subscribers to handle changes to the selected index.
        /// It can be called to programmatically trigger the event.</remarks>
        public void RaiseSelectedIndexChanged()
        {
            OnSelectedIndexChanged(EventArgs.Empty);
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex" /> property or the <see cref="SelectedIndices" />
        /// property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Sets or clears the specified flag on the handler.
        /// </summary>
        /// <remarks>This method modifies the state of the handler by updating the specified flag.
        /// Derived classes can override this method to provide additional
        /// behavior when flags are modified.</remarks>
        /// <param name="flag">The <see cref="ListBoxHandlerFlags"/> value to set or clear.</param>
        /// <param name="value"><see langword="true"/> to set the specified flag; <see langword="false"/> to clear it.</param>
        private void SetHandlerFlag(ListBoxHandlerFlags flag, bool value)
        {
            if (value)
                PlatformControl.Flags |= flag;
            else
                PlatformControl.Flags &= ~flag;
        }

        private void ChangeHandlerFlags(ListBoxHandlerFlags setFlags, ListBoxHandlerFlags clearFlags)
        {
            var oldFlags = PlatformControl.Flags;
            var currentFlags = oldFlags;
            currentFlags |= setFlags;
            currentFlags &= ~clearFlags;
            if (currentFlags != oldFlags)
                PlatformControl.Flags = currentFlags;
        }
    }
}
