using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class VirtualListControl
    {
        private int ignoreSelectEvents = 0;
        private DelayedEvent<EventArgs> delayedSelectionChanged = new();
        private ListBoxSelectionMode selectionMode = ListBoxSelectionMode.Single;
        private ListControlItem[]? selectionContext;
        private int? selectedIndex;

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the
        /// selected index in the control has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the control.
        /// <para>
        /// You can use the event handler for this event to load the information in
        /// the other controls. If the <see cref="SelectionMode"/> property
        /// is set to <see cref="ListBoxSelectionMode.Multiple"/>, any change to the
        /// <see cref="SelectedIndices"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// </remarks>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/>, <see cref="SelectedItem"/> or the
        /// <see cref="SelectedIndices"/> have changed.
        /// </summary>
        /// <remarks>
        /// This is a delayed event. If multiple events are occurred during the delay,
        /// they are ignored.
        /// </remarks>
        public event EventHandler<EventArgs>? DelayedSelectionChanged
        {
            add => delayedSelectionChanged.Delayed += value;
            remove => delayedSelectionChanged.Delayed -= value;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property has changed.
        /// Same as <see cref="SelectionChanged"/>, see it for the details.
        /// </summary>
        public event EventHandler? SelectedIndexChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ListBox;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected items in the control.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently selected items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        /// <remarks>
        /// For a multiple-selection control, this property
        /// returns a collection containing the indexes to all items that are selected
        /// in the control. For a single-selection
        /// control, this property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// control.
        /// <para>
        /// There are a number of ways to
        /// reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item
        /// in a single-selection control, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain
        /// the item that is currently selected in the control,
        /// instead of the index position of the item, use the
        /// <see cref="SelectedItem"/> property. In addition,
        /// you can use the <see cref="SelectedItems"/> property if you want to
        /// obtain all the selected items in a multiple-selection control.
        /// </para>
        /// </remarks>
        /// <seealso cref="SelectedIndicesDescending"/>
        [Browsable(false)]
        public virtual IReadOnlyList<int> SelectedIndices
        {
            get
            {
                if (DisposingOrDisposed)
                    return [];

                List<int> result = new();

                var index = SelectedIndex;

                if (index is not null && IsSelected(index.Value))
                    result.Add(index.Value);

                for (int i = 0; i < Count; i++)
                {
                    if (i == index)
                        continue;
                    if (IsSelected(i))
                    {
                        result.Add(i);
                    }
                }

                return result;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

                DoInsideSuspendedSelectionEvents(() =>
                {
                    ClearSelected();
                    SelectedIndex = value.FirstOrDefault();
                    foreach (var index in value)
                    {
                        SetSelectedCore(index, true);
                    }
                });
            }
        }

        /// <summary>
        /// Gets or sets default timeout interval (in msec) for timer that calls
        /// <see cref="DelayedSelectionChanged"/> event. If not specified,
        /// <see cref="TimerUtils.DefaultDelayedTextChangedTimeout"/> is used.
        /// </summary>
        [Browsable(false)]
        public int? DelayedSelectionChangedInterval
        {
            get => delayedSelectionChanged.Interval;
            set => delayedSelectionChanged.Interval = value;
        }

        /// <summary>
        /// Same as <see cref="SelectedIndices"/>.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<int> SelectedIndexes => SelectedIndices;

        /// <summary>
        /// Gets selected items as array.
        /// </summary>
        [Browsable(false)]
        public virtual ListControlItem[]? SelectedItemsArray
        {
            get
            {
                if (IsSelectionModeSingle)
                {
                    var item = SelectedItem;
                    if (item is null)
                        return null;
                    else
                        return [item];
                }
                else
                {
                    return SelectedItems.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected
        /// items in the control.
        /// </summary>
        /// <remarks>
        /// Indexes are returned in the descending order (maximal index is
        /// the first).
        /// </remarks>
        /// <seealso cref="SelectedIndices"/>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently selected items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        [Browsable(false)]
        public virtual IReadOnlyList<int> SelectedIndicesDescending
        {
            get
            {
#pragma warning disable
                int[] sortedCopy = SelectedIndices.OrderByDescending(i => i).ToArray();
#pragma warning restore
                return sortedCopy;
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in the control.
        /// </summary>
        /// <value>A zero-based index of the currently selected item.
        /// A value of <c>null</c> is returned if no item is selected.</value>
        /// <remarks>
        /// For a single selection mode, you can use this property to
        /// determine the index of the item that is selected
        /// in the control. If the <see cref="SelectionMode"/>
        /// property of the control is set to either
        /// <see cref="ListBoxSelectionMode.Multiple"/> (which indicates a
        /// multiple-selection control) and multiple items
        /// are selected in the list, this property can return the index to
        /// any selected item.
        /// <para>
        /// To retrieve a collection containing the indexes of all selected items
        /// in a multiple-selection control,
        /// use the <see cref="SelectedIndices"/> property. If you want to obtain
        /// the item that is currently selected in the control,
        /// use the <see cref="SelectedItem"/> property. In addition, you can use
        /// the <see cref="SelectedItems"/> property to obtain
        /// all the selected items in a multiple-selection control.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value is
        /// less than 0 or greater than or equal to the item count.</exception>
        public override int? SelectedIndex
        {
            get
            {
                if (DisposingOrDisposed)
                    return default;
                if (selectedIndex >= Count)
                {
                    selectedIndex = null;
                    return null;
                }

                return selectedIndex;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

                var oldSelected = SelectedIndex;
                if (oldSelected == value)
                    return;

                if (value != null && (value < 0 || value >= Items.Count))
                    value = null;

                DoInsideSuspendedSelectionEvents(() =>
                {
                    if (IsSelectionModeMultiple)
                        ClearSelected();
                    selectedIndex = value;
                });
            }
        }

        /// <summary>
        /// Gets whether <see cref="SelectionChanged"/> (and other events
        /// which are raised on selection change) are suspended.
        /// </summary>
        [Browsable(false)]
        public virtual bool AreSelectEventsSuspended
        {
            get
            {
                return ignoreSelectEvents > 0;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                if (DisposingOrDisposed)
                    return false;
                return Handler.HasBorder;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                Handler.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item in the control.
        /// </summary>
        /// <value>An object that represents the current selection in the control,
        /// or <c>null</c> if no item is selected.</value>
        /// <remarks>
        /// When single selection mode is used, you can use this property to
        /// determine the index of the item that is selected
        /// in the control. If the <see cref="SelectionMode"/>
        /// property of the control is set to either
        /// <see cref="ListBoxSelectionMode.Multiple"/> (which indicates a
        /// multiple-selection control) and multiple items
        /// are selected in the list, this property can return the index to
        /// any selected item.
        /// <para>
        /// To retrieve a collection containing all selected items in a
        /// multiple-selection control, use the
        /// <see cref="SelectedItems"/> property.
        /// If you want to obtain the index position of the currently selected
        /// item in the control, use the
        /// <see cref="SelectedIndex"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property to
        /// obtain all the selected indexes in a multiple-selection
        /// control.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public override ListControlItem? SelectedItem
        {
            get
            {
                if (DisposingOrDisposed)
                    return null;

                var selectedIndex = SelectedIndex;

                if (selectedIndex == null)
                    return null;

                int value = selectedIndex.Value;

                if (value >= Items.Count || value < 0)
                    return null;

                return Items[value];
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

                if (value == null)
                {
                    SelectedIndex = null;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        /// <summary>
        /// Gets 'Tag' property of the selected item if it is <see cref="BaseObjectWithAttr"/>.
        /// </summary>
        [Browsable(false)]
        public virtual object? SelectedItemTag => (SelectedItem as BaseObjectWithAttr)?.Tag;

        /// <summary>
        /// Gets a collection containing the currently selected items in the
        /// control.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{T}"/> containing the currently
        /// selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection control, this property
        /// returns a collection containing the indexes to all items that are selected
        /// in the control. For a single-selection
        /// control, this property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// control.
        /// <para>
        /// There are a number of ways to
        /// reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item
        /// in a single-selection control, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain
        /// the item that is currently selected in the control,
        /// instead of the index position of the item, use the
        /// <see cref="SelectedItem"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property
        /// to obtain all the selected indexes in a multiple-selection
        /// control.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<ListControlItem> SelectedItems
        {
            get
            {
                if (DisposingOrDisposed)
                    return [];

                List<ListControlItem> result = new();

                var index = SelectedIndex;
                var selectedItem = SelectedItem;

                if (index is not null && IsSelected(index.Value) && selectedItem is not null)
                    result.Add(selectedItem);

                for (int i = 0; i < Count; i++)
                {
                    if (i == index)
                        continue;
                    var item = SafeItem(i);
                    if (item is null)
                        continue;
                    if (IsSelected(i))
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets whether selection mode is <see cref="ListBoxSelectionMode.Single"/>
        /// </summary>
        [Browsable(false)]
        public bool IsSelectionModeSingle
        {
            get
            {
                return SelectionMode == ListBoxSelectionMode.Single;
            }

            set
            {
                if(value)
                    SelectionMode = ListBoxSelectionMode.Single;
                else
                    SelectionMode = ListBoxSelectionMode.Multiple;
            }
        }

        /// <summary>
        /// Gets or sets whether selection mode is <see cref="ListBoxSelectionMode.Multiple"/>
        /// </summary>
        [Browsable(false)]
        public bool IsSelectionModeMultiple
        {
            get
            {
                return SelectionMode == ListBoxSelectionMode.Multiple;
            }

            set
            {
                if (value)
                    SelectionMode = ListBoxSelectionMode.Multiple;
                else
                    SelectionMode = ListBoxSelectionMode.Single;
            }
        }

        /// <summary>
        /// Gets or sets the method in which items are selected in the
        /// control.
        /// </summary>
        /// <value>One of the <see cref="ListBoxSelectionMode"/> values.
        /// The default is <see cref="ListBoxSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine
        /// how many items in the control
        /// a user can select at one time.
        /// </remarks>
        public virtual ListBoxSelectionMode SelectionMode
        {
            get
            {
                if (DisposingOrDisposed)
                    return ListBoxSelectionMode.Single;

                return selectionMode;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;

                if (selectionMode == value)
                    return;

                selectionMode = value;

                SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a <see cref="IVListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        internal new IVListBoxHandler Handler
        {
            get
            {
                return (IVListBoxHandler)base.Handler;
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <summary>
        /// Removes selected items from the control.
        /// </summary>
        public virtual void RemoveSelectedItems()
        {
            RemoveItems(SelectedIndicesDescending);
        }

        /// <summary>
        /// Ensures that the item is visible within the control, scrolling the
        /// contents of the control, if necessary.
        /// </summary>
        /// <param name="itemIndex">The item index to scroll into visibility.</param>
        public virtual void EnsureVisible(int itemIndex)
        {
            if (DisposingOrDisposed)
                return;
            if (Count > 0)
                Handler.EnsureVisible(itemIndex);
        }

        /// <summary>
        /// Returns the zero-based index of the item at the specified coordinates.
        /// </summary>
        /// <param name="position">A <see cref="PointD"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        /// <returns>The zero-based index of the item found at the specified
        /// coordinates; returns <see langword="null"/>
        /// if no match is found.</returns>
        public virtual int? HitTest(PointD position)
        {
            if (DisposingOrDisposed)
                return null;
            return Handler.HitTest(position);
        }

        /// <summary>
        /// Gets only valid indexes from the list of indexes in
        /// the control.
        /// </summary>
        public IReadOnlyList<int> GetValidIndexes(params int[] indexes)
        {
            var validIndexes = new List<int>();

            foreach (int index in indexes)
            {
                if (IsValidIndex(index))
                    validIndexes.Add(index);
            }

            return validIndexes;
        }

        /// <summary>
        /// Copies result of the <see cref="SelectedItemsAsText"/> to clipboard.
        /// </summary>
        public virtual bool SelectedItemsToClipboard(string? separator = default)
        {
            var text = SelectedItemsAsText();
            if (string.IsNullOrEmpty(text))
                return false;
            Clipboard.SetText(text ?? string.Empty);
            return true;
        }

        /// <summary>
        /// Gets selected items as <see cref="string"/>.
        /// </summary>
        /// <remarks>Each item is separated by <paramref name="separator"/> or
        /// <see cref="Environment.NewLine"/> if it is empty.</remarks>
        /// <param name="separator">Items separator string.</param>
        public virtual string? SelectedItemsAsText(string? separator = default)
        {
            return ItemsAsText(SelectedIndexes, separator);
        }

        /// <summary>
        /// Selects items with specified indexes in the control.
        /// </summary>
        public virtual void SelectItems(params int[] indexes)
        {
            SelectedIndices = GetValidIndexes(indexes);
        }

        /// <summary>
        /// Checks whether index is valid in the control.
        /// </summary>
        public virtual bool IsValidIndex(int index)
        {
            return index >= 0 && index < Items.Count;
        }

        /// <inheritdoc/>
        public override void ClearSelected()
        {
            DoInsideSuspendedSelectionEvents(() =>
            {
                foreach (var item in Items)
                {
                    item.SetSelected(this, false);
                }

                selectedIndex = null;
            });
        }

        /// <summary>
        /// Selects or clears the selection for the specified item in a
        /// control.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a
        /// control to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item;
        /// otherwise, false.</param>
        /// <remarks>
        /// You can use this property to set the selection of items in a
        /// multiple-selection control.
        /// To select an item in a single-selection control, use
        /// the <see cref="SelectedIndex"/> property.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The specified index
        /// was outside the range of valid values.</exception>
        public virtual bool SetSelected(int index, bool value)
        {
            if (DisposingOrDisposed)
                return false;
            if (index < 0 || index >= Items.Count)
                return false;

            var result = false;

            DoInsideSuspendedSelectionEvents(() =>
            {
                result = SetSelectedCore(index, value);
            });

            return result;
        }

        /// <summary>
        /// Gets whether control has items.
        /// </summary>
        public virtual bool HasItems()
        {
            return Items.Count > 0;
        }

        /// <summary>
        /// Gets whether control has selected items.
        /// </summary>
        public virtual bool HasSelectedItems()
        {
            var indexes = SelectedIndexes;

            return (indexes is not null) && indexes.Count > 0;
        }

        /// <summary>
        /// Gets whether selected item can be removed.
        /// </summary>
        /// <returns></returns>
        public virtual bool CanRemoveSelectedItem()
        {
            var item = SelectedItem;

            if (item is not ListControlItem item2)
                return item != null;

            return item2.CanRemove;
        }

        /// <summary>
        /// Removes selected item from the control.
        /// </summary>
        public virtual void RemoveSelectedItem()
        {
            var index = SelectedIndex;
            if (index is null)
                return;
            Items.RemoveAt(index.Value);
        }

        /// <summary>
        /// Runs action specified in the <see cref="ListControlItem.DoubleClickAction"/> property
        /// of the selected item.
        /// </summary>
        public virtual void RunSelectedItemDoubleClickAction()
        {
            if (DisposingOrDisposed)
                return;
            var item = SelectedItem as ListControlItem;
            var action = item?.DoubleClickAction;
            action?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls
        /// <see cref="OnSelectionChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseSelectionChanged(EventArgs? e = null)
        {
            if (DisposingOrDisposed)
                return;
            if (ignoreSelectEvents > 0)
                return;
            OnSelectionChanged(EventArgs.Empty);
            OnSelectedIndexChanged(EventArgs.Empty);
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            delayedSelectionChanged.Raise(this, EventArgs.Empty, () => IsDisposed);
        }

        /// <summary>
        /// Calls the specified action inside the block with suspended selection events.
        /// </summary>
        /// <remarks>
        /// Before calling the action, selection events are suspended with
        /// <see cref="SuspendSelectionEvents"/>. After the action is called, selection events
        /// are resumed with <see cref="ResumeSelectionEvents"/>.
        /// </remarks>
        /// <param name="action">Action to call inside the block with
        /// suspended selection events.</param>
        public virtual void DoInsideSuspendedSelectionEvents(Action action)
        {
            SuspendSelectionEvents();

            try
            {
                action();
            }
            finally
            {
                ResumeSelectionEvents();
            }
        }

        /// <summary>
        /// Suspend raising of <see cref="SelectionChanged"/> and other events
        /// which are raised on selection change.
        /// </summary>
        public virtual void SuspendSelectionEvents()
        {
            if (ignoreSelectEvents == 0)
            {
                selectionContext = SelectedItemsArray;
            }

            ignoreSelectEvents++;
        }

        /// <summary>
        /// Resumes raising of <see cref="SelectionChanged"/> and other events
        /// which are raised on selection change.
        /// </summary>
        public virtual void ResumeSelectionEvents()
        {
            if (ignoreSelectEvents <= 0)
            {
                throw new InvalidOperationException(
                    "Call to ResumeSelectEvents without previous call to SuspendSelectEvents");
            }

            ignoreSelectEvents--;

            if (ignoreSelectEvents == 0)
            {
                var newSelection = SelectedItemsArray;
                bool changed = ArrayUtils.AreNotEqual(selectionContext, newSelection);
                if (changed)
                {
                    Invalidate();
                    RaiseSelectionChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        /// <remarks>See <see cref="SelectionChanged"/> for details.</remarks>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        /// <remarks>See <see cref="SelectionChanged"/> for details.</remarks>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            RunSelectedItemDoubleClickAction();
        }

        private bool SetSelectedCore(int index, bool value)
        {
            var item = SafeItem(index);

            if (item is null)
                return false;

            var changed = item.SetSelected(this, value);
            return changed;
        }
    }
}
