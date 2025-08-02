using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Interface to a control which displays a list of items with checkboxes.
    /// </summary>
    public interface ICheckListBox<TItem> : ICustomListBox<TItem>
        where TItem : class, new()
    {
        /// <summary>
        /// Occurs when the checkbox state of the item has changed.
        /// </summary>
        public event EventHandler? CheckedChanged;

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of all
        /// currently checked items in the control.
        /// </summary>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently checked items in the control.
        /// If no items are currently checked, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        IReadOnlyList<int> CheckedIndices { get; set; }

        /// <summary>
        /// Gets a collection that contains the zero-based indexes of
        /// all currently checked items in the control.
        /// </summary>
        /// <remarks>
        /// Indexes are returned in the descending order (maximal index
        /// is the first).
        /// </remarks>
        /// <value>
        /// An <see cref="IReadOnlyList{T}"/> containing the indexes of the
        /// currently checked items in the control.
        /// If no items are currently selected, an empty
        /// <see cref="IReadOnlyList{T}"/> is returned.
        /// </value>
        IReadOnlyList<int> CheckedIndicesDescending { get; }

        /// <summary>
        /// Gets or sets the zero-based index of the currently checked item
        /// in a control.
        /// </summary>
        /// <value>A zero-based index of the currently checked item. A value
        /// of <c>null</c> is returned if no item is checked.</value>
        int? CheckedIndex { get; set; }

        /// <summary>
        /// Removes checked items from the control.
        /// </summary>
        void RemoveCheckedItems();

        /// <summary>
        /// Checks items with specified indexes in the control.
        /// </summary>
        void CheckItems(params int[] indexes);

        /// <summary>
        /// Unchecks all items in the control.
        /// </summary>
        void ClearChecked();

        /// <summary>
        /// Checks or clears the check state for the specified item in
        /// a <see cref="StdCheckListBox"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a
        /// <see cref="StdCheckListBox"/> to set or clear the check state.</param>
        /// <param name="value"><c>true</c> to check the specified item;
        /// otherwise, false.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified
        /// index was outside the range of valid values.</exception>
        void SetChecked(int index, bool value);

        /// <summary>
        /// Raises the <see cref="CheckedChanged"/> event and calls related method.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        void RaiseCheckedChanged(EventArgs e);
    }
}