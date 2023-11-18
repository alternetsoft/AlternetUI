using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    // todo: copy DataSource, DisplayMember, ValueMember etc implementation from WinForms

    /// <summary>
    /// Provides a common implementation of members for the ListBox and
    /// ComboBox classes.
    /// </summary>
    public abstract class ListControl : Control, IReadOnlyStrings
    {
        private StringSearch? search;

        /// <summary>
        /// Gets or sets string search provider.
        /// </summary>
        public virtual StringSearch Search
        {
            get => search ??= new(this);

            set
            {
                if (value is null)
                    return;
                search = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item in the control.
        /// </summary>
        /// <value>An object that represents the current selection in the
        /// control, or <c>null</c> if no item is selected.</value>
        [Browsable(false)]
        public abstract object? SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in
        /// the control/>.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value
        /// of <c>null</c> is returned if no item is selected.</value>
        public abstract int? SelectedIndex { get; set; }

        /// <summary>
        /// Gets the items of the <see cref="ListControl"/>.
        /// </summary>
        /// <value>An <see cref="Collection{Object}"/> representing the items
        /// in the <see cref="ListControl"/>.</value>
        /// <remarks>This property enables you to obtain a reference to the list
        /// of items that are currently stored in the <see cref="ListControl"/>.
        /// With this reference, you can add items, remove items, and obtain
        /// a count of the items in the collection.</remarks>
        [Content]
        public Collection<object> Items { get; } =
            new Collection<object> { ThrowOnNullAdd = true };

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="Items"/>
        /// collection.
        /// </summary>
        /// <returns>
        /// The number of elements actually contained in the <see cref="Items"/>.
        /// </returns>
        [Browsable(false)]
        public int Count
        {
            get => Items.Count;
        }

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public object? LastItem
        {
            get
            {
                if (Count > 0)
                    return Items[Count - 1];
                return null;
            }

            set
            {
                if (Count > 0 && value is not null)
                    Items[Count - 1] = value;
            }
        }

        /// <summary>
        /// Gets first item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public object? FirstItem
        {
            get
            {
                if (Count > 0)
                    return Items[0];
                return null;
            }

            set
            {
                if (Count > 0 && value is not null)
                    Items[0] = value;
            }
        }

        /// <summary>
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public object? LastRootItem
        {
            get => LastItem;
            set => LastItem = value;
        }

        int IReadOnlyStrings.Count => Items.Count;

        /// <summary>
        /// Gets or sets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Items"/> element
        /// to get or set.</param>
        /// <returns>The <see cref="Items"/> element at the specified index.</returns>
        public object this[long index]
        {
            get => Items[(int)index];
            set => Items[(int)index] = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Items"/> element
        /// to get or set.</param>
        /// <returns>The <see cref="Items"/> element at the specified index.</returns>
        public object this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        /// <summary>
        /// Gets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the <see cref="Items"/> element or
        /// <c>null</c>.</param>
        /// <returns>The <see cref="Items"/> element at the specified index or <c>null</c>.</returns>
        public object? this[int? index]
        {
            get => Items[index];
        }

        string? IReadOnlyStrings.this[int index] => GetItemText(index);

        /// <summary>
        /// Adds an object to the end of the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="item">The object to be added to the end of the
        /// <see cref="Items"/> collection.</param>
        public virtual int Add(object item)
        {
            Items.Add(item);
            return Items.Count - 1;
        }

        /// <summary>
        /// Adds <paramref name="text"/> with <paramref name="data"/> to the end of
        /// the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="text">Item text (title).</param>
        /// <param name="data">Item data.</param>
        /// <remarks>
        /// This method creates <see cref="ListControlItem"/>, assigns its properties with
        /// <paramref name="text"/> and <paramref name="data"/>. Created object is added to
        /// the <see cref="Items"/> collection.
        /// </remarks>
        public virtual int Add(string text, object? data)
        {
            return Add(new ListControlItem(text, data));
        }

        /// <summary>
        /// Selects last item in the control.
        /// </summary>
        public virtual void SelectLastItem()
        {
            if (Count > 0)
                SelectedIndex = Count - 1;
        }

        /// <summary>
        /// Returns the text representation of item with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Item index from which to get the contents to display.</param>
        public virtual string GetItemText(int index) => GetItemText(Items[index]);

        /// <summary>
        /// Returns the text representation of the specified item.
        /// </summary>
        /// <param name="item">The object from which to get the contents to
        /// display.</param>
        public virtual string GetItemText(object? item)
        {
            return item switch
            {
                null => string.Empty,
                string s => s,
                _ => Convert.ToString(item, CultureInfo.CurrentCulture) ?? string.Empty
            };
        }

        /// <summary>
        /// Selects first item in the control.
        /// </summary>
        public virtual void SelectFirstItem()
        {
            SelectedItem = FirstItem;
        }

        /// <summary>
        /// Adds enum values to <see cref="Items"/> property of the control.
        /// </summary>
        /// <param name="type">Type of the enum which values are added.</param>
        /// <param name="selectValue">New <see cref="SelectedItem"/> value.</param>
        public virtual void AddEnumValues(Type type, object? selectValue = null)
        {
            foreach (var item in Enum.GetValues(type))
                Items.Add(item);
            if(selectValue is not null)
                SelectedItem = selectValue;
        }

        /// <summary>
        /// Adds enum values to <see cref="Items"/> property of the control.
        /// </summary>
        /// <typeparam name="T">Type of the enum which values are added.</typeparam>
        public virtual void AddEnumValues<T>()
            where T : Enum
        {
            AddEnumValues(typeof(T));
        }

        /// <summary>
        /// Removed all items from the control.
        /// </summary>
        public virtual void RemoveAll()
        {
            BeginUpdate();
            try
            {
                ClearSelected();
                Items.Clear();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Removes items from the control.
        /// </summary>
        public virtual void RemoveItems(IReadOnlyList<int> items)
        {
            if (items == null || items.Count == 0)
                return;

            BeginUpdate();
            try
            {
                ClearSelected();
                foreach (int index in items)
                {
                    if (index < Items.Count)
                        Items.RemoveAt(index);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Gets items as <see cref="string"/>.
        /// </summary>
        /// <remarks>Each item is separated by <paramref name="separator"/> or
        /// <see cref="Environment.NewLine"/> if it is empty.</remarks>
        /// <param name="separator">Items separator string.</param>
        /// <param name="indexes">Items indexes.</param>
        public virtual string? ItemsAsText(IReadOnlyList<int> indexes, string? separator = default)
        {
            separator ??= Environment.NewLine;
            string? result = null;

            foreach (var index in indexes)
            {
                if (result is null)
                    result = Items[index].ToString();
                else
                    result += $"{separator}{Items[index]}";
            }

            return result;
        }

        /// <inheritdoc cref="StringSearch.FindString(string)"/>
        public virtual int? FindString(string s) => Search.FindString(s);

        /// <inheritdoc cref="StringSearch.FindString(string, int?)"/>
        public virtual int? FindString(string s, int? startIndex) => Search.FindString(s, startIndex);

        /// <inheritdoc cref="StringSearch.FindStringExact(string)"/>
        public virtual int? FindStringExact(string s) => Search.FindStringExact(s);

        /// <inheritdoc cref="StringSearch.FindStringEx(string?, int?, bool, bool)"/>
        public virtual int? FindStringEx(
         string? str,
         int? startIndex,
         bool exact,
         bool ignoreCase) => Search.FindStringEx(str, startIndex, exact, ignoreCase);

        /// <summary>
        /// Unselects all items in the control.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the
        /// <see cref="SelectedIndex"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the list.
        /// </remarks>
        public virtual void ClearSelected()
        {
            SelectedItem = null;
        }

        private int GetItemCount() => Items.Count;
    }
}