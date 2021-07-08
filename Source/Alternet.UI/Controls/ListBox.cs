using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items.
    /// </summary>
    /// <remarks>
    /// The <see cref="ListBox"/> control enables you to display a list of items to the user that the user can select by clicking.
    /// A <see cref="ListBox"/> control can provide single or multiple selections using the <see cref="SelectionMode"/> property.
    /// The <see cref="Control.BeginUpdate"/> and <see cref="Control.EndUpdate"/> methods enable
    /// you to add a large number of items to the ListBox without the control being repainted each time an item is added to the list.
    /// The <see cref="ListControl.Items"/>, <see cref="SelectedItems"/>, and <see cref="SelectedIndices"/> properties provide access to the three
    /// collections that are used by the <see cref="ListBox"/>.
    /// </remarks>
    public class ListBox : ListControl
    {
        private HashSet<int> selectedIndices = new HashSet<int>();

        private ListBoxSelectionMode selectionMode = ListBoxSelectionMode.Single;

        /// <summary>
        /// Occurs when the <see cref="SelectedIndex"/> property or the <see cref="SelectedIndices"/> collection has changed.
        /// </summary>
        /// <remarks>
        /// You can create an event handler for this event to determine when the selected index in the <see cref="ListBox"/> has been changed.
        /// This can be useful when you need to display information in other controls based on the current selection in the <see cref="ListBox"/>.
        /// <para>
        /// You can use the event handler for this event to load the information in the other controls. If the <see cref="SelectionMode"/> property
        /// is set to <see cref="ListBoxSelectionMode.Multiple"/>, any change to the <see cref="SelectedIndices"/> collection,
        /// including removing an item from the selection, will raise this event.
        /// </para>
        /// </remarks>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="SelectionMode"/> property changes.
        /// </summary>
        public event EventHandler? SelectionModeChanged;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all currently selected items in the <see cref="ListBox"/>.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the currently selected items in the control.
        /// If no items are currently selected, an empty <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        /// <remarks>
        /// For a multiple-selection <see cref="ListBox"/>, this property returns a collection containing the indexes to all items that are selected
        /// in the <see cref="ListBox"/>. For a single-selection <see cref="ListBox"/>, this property returns a collection containing a
        /// single element containing the index of the only selected item in the <see cref="ListBox"/>.
        /// <para>
        /// The <see cref="ListBox"/> class provides a number of ways to reference selected items. Instead of using the <see cref="SelectedIndices"/>
        /// property to obtain the index position of the currently selected item in a single-selection <see cref="ListBox"/>, you
        /// can use the <see cref="SelectedIndex"/> property. If you want to obtain the item that is currently selected in the <see cref="ListBox"/>,
        /// instead of the index position of the item, use the <see cref="SelectedItem"/> property. In addition,
        /// you can use the <see cref="SelectedItems"/> property if you want to obtain all the selected items in a multiple-selection <see cref="ListBox"/>.
        /// </para>
        /// </remarks>
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
                    InvokeSelectionChanged(EventArgs.Empty);
            }
        }

        public int? SelectedIndex
        {
            get
            {
                CheckDisposed();
                return selectedIndices.FirstOrDefault();
            }

            set
            {
                CheckDisposed();

                ClearSelected();
                if (value != null)
                    SetSelected(value.Value, true);
            }
        }

        public object? SelectedItem
        {
            get
            {
                CheckDisposed();

                if (SelectedIndex == null)
                    return null;

                return Items[SelectedIndex.Value];
            }

            set
            {
                CheckDisposed();

                if (value == null)
                {
                    SelectedIndex = -1;
                    return;
                }

                var index = Items.IndexOf(value);
                if (index != -1)
                    SelectedIndex = index;
            }
        }

        public IReadOnlyList<object> SelectedItems
        {
            get
            {
                CheckDisposed();

                return SelectedIndices.Select(x => Items[x]).ToArray();
            }
        }

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
        /// Unselects all items in the ListBox.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the <see cref="SelectedIndex"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the list.
        /// </remarks>
        public void ClearSelected()
        {
            ClearSelectedCore();
            InvokeSelectionChanged(EventArgs.Empty);
        }

        public void SetSelected(int index, bool value)
        {
            var changed = SetSelectedCore(index, value);

            if (changed)
                InvokeSelectionChanged(EventArgs.Empty);
        }

        public void InvokeSelectionChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

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