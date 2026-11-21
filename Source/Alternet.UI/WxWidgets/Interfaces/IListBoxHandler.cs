using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the platform-specific handler contract for list box controls.
    /// Implementations provide selection, item management and hit-testing behavior.
    /// </summary>
    public interface IListBoxHandler
    {
        /// <summary>
        /// Gets or sets the flags that define the behavior and appearance of the list box.
        /// </summary>
        ListBoxHandlerFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list box has a visible border.
        /// </summary>
        bool HasBorder { get; set; }

        /// <summary>
        /// Gets the index of the current selection.
        /// </summary>
        /// <returns>The zero-based index of the selected item, or -1 if no selection.</returns>
        int GetSelection();

        /// <summary>
        /// Determines whether the item at the specified index is selected.
        /// </summary>
        /// <param name="n">Zero-based index of the item to test.</param>
        /// <returns><c>true</c> if the item is selected; otherwise, <c>false</c>.</returns>
        bool IsSelected(int n);

        /// <summary>
        /// Determines whether the items in the list box are displayed in sorted order.
        /// </summary>
        /// <returns><c>true</c> if items are sorted; otherwise, <c>false</c>.</returns>
        bool IsSorted();

        /// <summary>
        /// Performs a check on the specified item, with an optional flag to enable or disable the check.
        /// </summary>
        /// <param name="item">The item to be checked. Must be a valid integer representing
        /// the target of the check.</param>
        /// <param name="check">A boolean value indicating whether the check should be performed.
        /// <see langword="true"/> to perform the
        /// check; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        void Check(int item, bool check = true);

        /// <summary>
        /// Determines whether the specified item is checked.
        /// </summary>
        /// <param name="item">The identifier of the item to check.</param>
        /// <returns><see langword="true"/> if the specified item is checked; otherwise, <see langword="false"/>.</returns>
        bool IsChecked(int item);

        /// <summary>
        /// Updates the selection state of an item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to update.
        /// Must be within the valid range of items.</param>
        /// <param name="select">A value indicating whether to select or deselect the item.
        /// <see langword="true"/> to select the item; <see langword="false"/> to deselect it.</param>
        void SetSelection(int index, bool select);

        /// <summary>
        /// Selects or deselects the first item whose content matches the specified string.
        /// </summary>
        /// <param name="s">The string to match.</param>
        /// <param name="select"><c>true</c> to select the matching item; <c>false</c> to deselect.</param>
        /// <returns><c>true</c> if an item was found and its selection state changed; otherwise, <c>false</c>.</returns>
        bool SetStringSelection(string s, bool select);

        /// <summary>
        /// Finds the index of the first item whose string representation matches the specified text.
        /// </summary>
        /// <param name="s">The string to search for.</param>
        /// <param name="bCase">If <c>true</c>, perform a case-sensitive search; otherwise case-insensitive.</param>
        /// <returns>The zero-based index of the matching item, or -1 if not found.</returns>
        int FindString(string s, bool bCase = false);

        /// <summary>
        /// Gets the number of items that are visible per page in the list box viewport.
        /// </summary>
        /// <returns>The number of items per page.</returns>
        int GetCountPerPage();

        /// <summary>
        /// Gets the index of the topmost visible item in the list box.
        /// </summary>
        /// <returns>The zero-based index of the top item.</returns>
        int GetTopItem();

        /// <summary>
        /// Performs a hit test at the specified point and returns the index of the item under that point.
        /// </summary>
        /// <param name="point">Point in control coordinates to test.</param>
        /// <returns>The zero-based index of the item under the point, or -1 if none.</returns>
        int HitTest(PointD point);

        /// <summary>
        /// Gets the string representation of the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item.</param>
        /// <returns>The item's string.</returns>
        string GetString(int n);

        /// <summary>
        /// Gets the number of items in the list box.
        /// </summary>
        /// <returns>The count of items.</returns>
        int GetCount();

        /// <summary>
        /// Deselects the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to deselect.</param>
        void Deselect(int n);

        /// <summary>
        /// Ensures that the item at the given index is visible in the viewport.
        /// </summary>
        /// <param name="n">Zero-based index of the item to make visible.</param>
        void EnsureVisible(int n);

        /// <summary>
        /// Sets the specified item to be the first (top) visible item.
        /// </summary>
        /// <param name="n">Zero-based index of the item to place first.</param>
        void SetFirstItem(int n);

        /// <summary>
        /// Sets the first visible item by searching for the specified string and making that item first.
        /// </summary>
        /// <param name="s">The string of the item to set as first.</param>
        void SetFirstItemStr(string s);

        /// <summary>
        /// Sets the selection to the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to select.</param>
        void SetSelection(int n);

        /// <summary>
        /// Replaces the string representation of the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item.</param>
        /// <param name="s">The new string for the item.</param>
        void SetString(int n, string s);

        /// <summary>
        /// Removes all items from the list box.
        /// </summary>
        void Clear();

        /// <summary>
        /// Deletes the item at the specified index.
        /// </summary>
        /// <param name="n">Zero-based index of the item to delete.</param>
        void Delete(int n);

        /// <summary>
        /// Appends an item with the specified string to the end of the list box.
        /// </summary>
        /// <param name="s">The string to append.</param>
        /// <returns>The zero-based index of the newly appended item.</returns>
        int Append(string s);

        /// <summary>
        /// Inserts an item at the specified position.
        /// </summary>
        /// <param name="item">The string for the item to insert.</param>
        /// <param name="pos">Zero-based position to insert the item at.</param>
        /// <returns>The zero-based index of the inserted item.</returns>
        int Insert(string item, int pos);

        /// <summary>
        /// Gets the number of currently selected items (for multi-selection list boxes).
        /// </summary>
        /// <returns>The count of selected items.</returns>
        int GetSelectionsCount();

        /// <summary>
        /// Gets the index of the selected item at the specified selection order.
        /// </summary>
        /// <param name="index">Zero-based index into the selection list.</param>
        /// <returns>The zero-based item index corresponding to that selection entry.</returns>
        int GetSelectionsItem(int index);

        /// <summary>
        /// Updates internal selection state. Implementations should refresh selection caches when necessary.
        /// </summary>
        void UpdateSelections();

        /// <summary>
        /// Gets the count of indexes that are currently checked.
        /// </summary>
        /// <returns>The number of indexes that are checked. Returns 0 if no indexes are checked.</returns>
        int GetCheckedIndexesCount();

        /// <summary>
        /// Retrieves the item index in the specified position inside the checked items list.
        /// </summary>
        int GetCheckedIndexesItem(int index);

        /// <summary>
        /// Updates the collection of indexes that are marked as checked.
        /// </summary>
        /// <remarks>This method refreshes the state of the checked indexes, ensuring that the collection
        /// reflects the current selection or state. Use this method when the underlying data or selection criteria have
        /// changed and the checked indexes need to be recalculated.</remarks>
        void UpdateCheckedIndexes();
    }
}