using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items.
    /// </summary>
    /// <remarks>
    /// The <see cref="ListBox"/> control enables you to display a list of items to
    /// the user that the user can select by clicking.
    /// A <see cref="ListBox"/> control can provide single or multiple selections
    /// using the <see cref="SelectionMode"/> property.
    /// The <see cref="Control.BeginUpdate"/> and <see cref="Control.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the ListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="ListControl.Items"/>, <see cref="SelectedItems"/>, and
    /// <see cref="SelectedIndices"/> properties provide access to the three
    /// collections that are used by the <see cref="ListBox"/>.
    /// </remarks>
    [ControlCategory("Common")]
    public class ListBox : ListControl
    {
        private readonly HashSet<int> selectedIndices = [];
        private int ignoreSelectEvents = 0;

        private ListBoxSelectionMode selectionMode = ListBoxSelectionMode.Single;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        public ListBox()
        {
           if (Application.IsWindowsOS)
                UserPaint = true;

            bool? hasBorder = AllPlatformDefaults.GetHasBorderOverride(ControlKind);

            if (hasBorder is not null)
                HasBorder = hasBorder.Value;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property or the
        /// <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the
        /// selected index in the <see cref="ListBox"/> has been changed.
        /// This can be useful when you need to display information in other
        /// controls based on the current selection in the <see cref="ListBox"/>.
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
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Gets a <see cref="ListBoxHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public new ListBoxHandler Handler
        {
            get
            {
                CheckDisposed();
                return (ListBoxHandler)base.Handler;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ListBox;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected items in the <see cref="ListBox"/>.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently selected items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListBox"/>, this property
        /// returns a collection containing the indexes to all items that are selected
        /// in the <see cref="ListBox"/>. For a single-selection
        /// <see cref="ListBox"/>, this property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// <see cref="ListBox"/>.
        /// <para>
        /// The <see cref="ListBox"/> class provides a number of ways to
        /// reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item
        /// in a single-selection <see cref="ListBox"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain
        /// the item that is currently selected in the <see cref="ListBox"/>,
        /// instead of the index position of the item, use the
        /// <see cref="SelectedItem"/> property. In addition,
        /// you can use the <see cref="SelectedItems"/> property if you want to
        /// obtain all the selected items in a multiple-selection <see cref="ListBox"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="SelectedIndicesDescending"/>
        [Browsable(false)]
        public IReadOnlyList<int> SelectedIndices
        {
            get
            {
                CheckDisposed();
                return selectedIndices.ToArray();
            }

            set
            {
                CheckDisposed();

                ClearSelectedCore();

                bool changed = false;
                foreach (var index in value)
                {
                    if (SetSelectedCore(index, true))
                        changed = true;
                }

                if (changed)
                    RaiseSelectionChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Same as <see cref="SelectedIndices"/>
        /// </summary>
        [Browsable(false)]
        public IReadOnlyList<int> SelectedIndexes => SelectedIndices;

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                CheckDisposed();
                if (Handler is not NativeListBoxHandler handler)
                    return true;
                return handler.NativeControl.HasBorder;
            }

            set
            {
                CheckDisposed();
                if (Handler is not NativeListBoxHandler handler)
                    return;
                handler.NativeControl.HasBorder = value;
            }
        }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently selected
        /// items in the <see cref="ListBox"/>.
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
        public IReadOnlyList<int> SelectedIndicesDescending
        {
            get
            {
                int[] sortedCopy = SelectedIndices.OrderByDescending(i => i).ToArray();
                return sortedCopy;
            }
        }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in a
        /// <see cref="ListBox"/>.
        /// </summary>
        /// <value>A zero-based index of the currently selected item.
        /// A value of <c>null</c> is returned if no item is selected.</value>
        /// <remarks>
        /// For a standard <see cref="ListBox"/>, you can use this property to
        /// determine the index of the item that is selected
        /// in the <see cref="ListBox"/>. If the <see cref="SelectionMode"/>
        /// property of the <see cref="ListBox"/> is set to either
        /// <see cref="ListBoxSelectionMode.Multiple"/> (which indicates a
        /// multiple-selection <see cref="ListBox"/>) and multiple items
        /// are selected in the list, this property can return the index to
        /// any selected item.
        /// <para>
        /// To retrieve a collection containing the indexes of all selected items
        /// in a multiple-selection <see cref="ListBox"/>,
        /// use the <see cref="SelectedIndices"/> property. If you want to obtain
        /// the item that is currently selected in the <see cref="ListBox"/>,
        /// use the <see cref="SelectedItem"/> property. In addition, you can use
        /// the <see cref="SelectedItems"/> property to obtain
        /// all the selected items in a multiple-selection <see cref="ListBox"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The assigned value is
        /// less than 0 or greater than or equal to the item count.</exception>
        public override int? SelectedIndex
        {
            get
            {
                if (selectedIndices.Count == 0)
                    return null;
                else
                    return selectedIndices.First();
            }

            set
            {
                CheckDisposed();

                var oldSelected = SelectedIndex;
                var oldCount = selectedIndices.Count;

                if (oldSelected == value && oldCount <= 1)
                    return;

                if (value != null && (value < 0 || value >= Items.Count))
                    throw new ArgumentOutOfRangeException(nameof(value));

                ignoreSelectEvents++;
                try
                {
                    ClearSelected();
                    if (value != null)
                        SetSelected(value.Value, true);
                }
                finally
                {
                    ignoreSelectEvents--;
                    RaiseSelectionChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item in the ListBox.
        /// </summary>
        /// <value>An object that represents the current selection in the control,
        /// or <c>null</c> if no item is selected.</value>
        /// <remarks>
        /// For a standard <see cref="ListBox"/>, you can use this property to
        /// determine the index of the item that is selected
        /// in the <see cref="ListBox"/>. If the <see cref="SelectionMode"/>
        /// property of the <see cref="ListBox"/> is set to either
        /// <see cref="ListBoxSelectionMode.Multiple"/> (which indicates a
        /// multiple-selection <see cref="ListBox"/>) and multiple items
        /// are selected in the list, this property can return the index to
        /// any selected item.
        /// <para>
        /// To retrieve a collection containing all selected items in a
        /// multiple-selection <see cref="ListBox"/>, use the
        /// <see cref="SelectedItems"/> property.
        /// If you want to obtain the index position of the currently selected
        /// item in the <see cref="ListBox"/>, use the
        /// <see cref="SelectedIndex"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property to
        /// obtain all the selected indexes in a multiple-selection
        /// <see cref="ListBox"/>.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public override object? SelectedItem
        {
            get
            {
                CheckDisposed();

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
                CheckDisposed();

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
        /// Gets a collection containing the currently selected items in the
        /// <see cref="ListBox"/>.
        /// </summary>
        /// <value>A <see cref="IReadOnlyList{T}"/> containing the currently
        /// selected items in the control.</value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListBox"/>, this property
        /// returns a collection containing the indexes to all items that are selected
        /// in the <see cref="ListBox"/>. For a single-selection
        /// <see cref="ListBox"/>, this property returns a collection containing a
        /// single element containing the index of the only selected item in the
        /// <see cref="ListBox"/>.
        /// <para>
        /// The <see cref="ListBox"/> class provides a number of ways to
        /// reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item
        /// in a single-selection <see cref="ListBox"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain
        /// the item that is currently selected in the <see cref="ListBox"/>,
        /// instead of the index position of the item, use the
        /// <see cref="SelectedItem"/> property.
        /// In addition, you can use the <see cref="SelectedIndices"/> property
        /// to obtain all the selected indexes in a multiple-selection
        /// <see cref="ListBox"/>.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public IReadOnlyList<object> SelectedItems
        {
            get
            {
                CheckDisposed();

                return SelectedIndices.Select(x => Items[x]).ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the method in which items are selected in the
        /// <see cref="ListBox"/>.
        /// </summary>
        /// <value>One of the <see cref="ListBoxSelectionMode"/> values.
        /// The default is <see cref="ListBoxSelectionMode.Single"/>.</value>
        /// <remarks>
        /// The <see cref="SelectionMode"/> property enables you to determine
        /// how many items in the <see cref="ListBox"/>
        /// a user can select at one time.
        /// </remarks>
        public ListBoxSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();

                return selectionMode;
            }

            set
            {
                CheckDisposed();

                if (selectionMode == value)
                    return;

                selectionMode = value;

                SelectionModeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Removes selected items from the <see cref="ListBox"/>.
        /// </summary>
        public void RemoveSelectedItems()
        {
            RemoveItems(SelectedIndicesDescending);
        }

        /// <summary>
        /// Ensures that the item is visible within the control, scrolling the
        /// contents of the control, if necessary.
        /// </summary>
        /// <param name="itemIndex">The item index to scroll into visibility.</param>
        public void EnsureVisible(int itemIndex) => Handler.EnsureVisible(itemIndex);

        /// <summary>
        /// Returns the zero-based index of the item at the specified coordinates.
        /// </summary>
        /// <param name="position">A <see cref="Point"/> object containing
        /// the coordinates used to obtain the item
        /// index.</param>
        /// <returns>The zero-based index of the item found at the specified
        /// coordinates; returns <see langword="null"/>
        /// if no match is found.</returns>
        public int? HitTest(Point position) => Handler.HitTest(position);

        /// <summary>
        /// Gets only valid indexes from the list of indexes in
        /// the <see cref="ListBox"/>.
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
        /// Selects items with specified indexes in the <see cref="ListBox"/>.
        /// </summary>
        public void SelectItems(params int[] indexes)
        {
            SelectedIndices = GetValidIndexes(indexes);
        }

        /// <summary>
        /// Checks whether index is valid in the <see cref="ListBox"/>.
        /// </summary>
        public bool IsValidIndex(int index)
        {
            return index >= 0 && index < Items.Count;
        }

        /// <inheritdoc/>
        public override void ClearSelected()
        {
            ClearSelectedCore();
            RaiseSelectionChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Selects or clears the selection for the specified item in a
        /// <see cref="ListBox"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a
        /// <see cref="ListBox"/> to select or clear the selection for.</param>
        /// <param name="value"><c>true</c> to select the specified item;
        /// otherwise, false.</param>
        /// <remarks>
        /// You can use this property to set the selection of items in a
        /// multiple-selection <see cref="ListBox"/>.
        /// To select an item in a single-selection <see cref="ListBox"/>, use
        /// the <see cref="SelectedIndex"/> property.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">The specified index
        /// was outside the range of valid values.</exception>
        public bool SetSelected(int index, bool value)
        {
            if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(value));

            CheckDisposed();

            var changed = SetSelectedCore(index, value);

            if (changed)
                RaiseSelectionChanged(EventArgs.Empty);

            return changed;
        }

        /// <summary>
        /// Raises the <see cref="SelectionChanged"/> event and calls
        /// <see cref="OnSelectionChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseSelectionChanged(EventArgs e)
        {
            if (ignoreSelectEvents > 0)
                return;
            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateListBoxHandler(this);
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

        private void ClearSelectedCore()
        {
            selectedIndices.Clear();
        }

        private bool SetSelectedCore(int index, bool value)
        {
            bool changed;
            if (value)
                changed = selectedIndices.Add(index);
            else
                changed = selectedIndices.Remove(index);

            return changed;
        }
    }
}