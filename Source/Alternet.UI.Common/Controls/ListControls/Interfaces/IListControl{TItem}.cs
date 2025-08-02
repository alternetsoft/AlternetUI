using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Common interface for the <see cref="StdListBox"/>, <see cref="ComboBox"/>
    /// and other controls with items.
    /// </summary>
    /// <typeparam name="TItem">Type of the item. Can be <see cref="object"/>,
    /// <see cref="ListControlItem"/> or any other type.</typeparam>
    public interface IListControl<TItem> : IControl
        where TItem : class, new()
    {
        /// <summary>
        /// Occurs when controls needs to get string representation of the item for the display
        /// or other purposes. Called from <see cref="GetItemText(int, bool)"/>.
        /// </summary>
        public event EventHandler<GetItemTextEventArgs>? CustomItemText;

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        TItem? LastItem { get; set; }

        /// <summary>
        /// Gets first item in the control or <c>null</c> if there are no items.
        /// </summary>
        TItem? FirstItem { get; set; }

        /// <summary>
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        TItem? LastRootItem { get; set; }

        /// <summary>
        /// Gets or sets the currently selected item in the control.
        /// </summary>
        /// <value>An object that represents the current selection in the
        /// control, or <c>null</c> if no item is selected.</value>
        TItem? SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in
        /// the control.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value
        /// of <c>null</c> is returned if no item is selected.</value>
        int? SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in
        /// the control. Same as <see cref="SelectedIndex"/> but is not nullable.
        /// </summary>
        int SelectedIndexAsInt { get; set; }

        /// <summary>
        /// Returns <see cref="Action"/> associated with the <see cref="SelectedItem"/> if it is
        /// <see cref="ListControlItem"/>.
        /// </summary>
        Action? SelectedAction { get; }

        /// <summary>
        /// Returns <see cref="ObjectUniqueId"/> associated
        /// with the <see cref="SelectedItem"/> if it is
        /// <see cref="ListControlItem"/>.
        /// </summary>
        ObjectUniqueId? SelectedUniqueId { get; }

        /// <summary>
        /// Gets the number of elements contained in the control.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the control.
        /// </returns>
        int Count { get; set; }

        /// <summary>
        /// Gets or sets string search provider.
        /// </summary>
        StringSearch Search { get; set; }

        /// <summary>
        /// Gets or sets item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        TItem? this[long index] { get; set; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        TItem? this[int index] { get; set; }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element or
        /// <c>null</c>.</param>
        /// <returns>The element at the specified index or <c>null</c>.</returns>
        TItem? this[int? index] { get; set; }

        /// <summary>
        /// Gets item with the specified index.
        /// This methods is called from all other methods that
        /// request the item.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        TItem? GetItem(int index);

        /// <summary>
        /// Sets item with the specified index.
        /// This methods is called from all other methods that
        /// change the item.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <param name="value">New item value.</param>
        void SetItem(int index, TItem value);

        /// <summary>
        /// Adds an object to the end of the items collection.
        /// </summary>
        /// <param name="item">The object to be added to the end of the
        /// items collection.</param>
        int Add(TItem item);

        /// <summary>
        /// Gets item with the specified index.
        /// If index of the item is invalid, returns null.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        T? SafeItem<T>(int index)
            where T : class;

        /// <summary>
        /// Gets item with the specified index.
        /// If index of the item is invalid, throws an exception.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        public T RequiredItem<T>(int index)
            where T : class;

        /// <summary>
        /// Gets <see cref="ListControlItem"/> item with the specified index.
        /// If index of the item is invalid or item is not <see cref="ListControlItem"/>,
        /// returns <c>null</c>.
        /// </summary>
        public ListControlItem? SafeItem(int index);

        /// <summary>
        /// Gets <see cref="ListControlItem"/> item with the specified index.
        /// If index of the item is invalid, an exception is thrown.
        /// </summary>
        public ListControlItem RequiredItem(int index);

        /// <summary>
        /// Selects last item in the control.
        /// </summary>
        void SelectLastItem();

        /// <summary>
        /// Returns the text representation of item with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Item index from which to get the contents to display.</param>
        /// <param name="forDisplay">The flag which specifies whether to get item's text
        /// for display purposes or the real value.</param>
        string GetItemText(int index, bool forDisplay);

        /// <summary>
        /// Selects first item in the control.
        /// </summary>
        void SelectFirstItem();

        /// <summary>
        /// Removed all items from the control.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Removes items from the control.
        /// </summary>
        bool RemoveItems(IReadOnlyList<int> items);

        /// <summary>
        /// Gets items as <see cref="string"/>.
        /// </summary>
        /// <remarks>Each item is separated by <paramref name="separator"/> or
        /// <see cref="Environment.NewLine"/> if it is empty.</remarks>
        /// <param name="separator">Items separator string.</param>
        /// <param name="indexes">Items indexes.</param>
        string? ItemsAsText(IReadOnlyList<int> indexes, string? separator = default);

        /// <inheritdoc cref="StringSearch.FindString(string)"/>
        int? FindString(string s);

        /// <inheritdoc cref="StringSearch.FindString(string, int?)"/>
        int? FindString(string s, int? startIndex);

        /// <inheritdoc cref="StringSearch.FindStringExact(string)"/>
        int? FindStringExact(string s);

        /// <inheritdoc cref="StringSearch.FindStringEx(string?, int?, bool, bool)"/>
        int? FindStringEx(
         string? str,
         int? startIndex,
         bool exact,
         bool ignoreCase);

        /// <summary>
        /// Sets <see cref="SelectedIndex"/>. Implemented as method for the convenience.
        /// </summary>
        /// <param name="index"></param>
        void SetSelectedIndex(int? index);

        /// <summary>
        /// Unselects all items in the control.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the
        /// <see cref="SelectedIndex"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the list.
        /// </remarks>
        void ClearSelected();

        /// <summary>
        /// Same as <see cref="RemoveItemWithValue"/>, but checks for the condition
        /// before removing the item.
        /// </summary>
        public bool RemoveItemWithValueIf(object? value, bool condition);

        /// <summary>
        /// Removes item with the specified value. Item is removed if it equals
        /// the specified value or
        /// if it is <see cref="ListControlItem"/> and it's <see cref="ListControlItem.Value"/>
        /// property equals the value.
        /// </summary>
        bool RemoveItemWithValue(object? value);

        /// <summary>
        /// Changes the number of elements in the items.
        /// </summary>
        /// <param name="newCount">New number of elements.</param>
        /// <param name="createItem">Function which creates new item.</param>
        /// <remarks>
        /// If collection has more items than specified in <paramref name="newCount"/>,
        /// these items are removed. If collection has less items, new items are created
        /// using <paramref name="createItem"/> function.
        /// </remarks>
        void SetCount(int newCount, Func<TItem> createItem);
    }
}