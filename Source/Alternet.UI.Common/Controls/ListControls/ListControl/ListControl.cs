using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a common implementation of members for the ListBox and
    /// ComboBox classes.
    /// </summary>
    public abstract class ListControl : Control, IReadOnlyStrings
    {
        private StringSearch? search;
        private ListControlItems<object>? items;

        /// <summary>
        /// Occurs when controls needs to get string representation of the item for the display
        /// or other purposes. Called from <see cref="GetItemText(int)"/>.
        /// </summary>
        public event EventHandler<GetItemTextEventArgs>? CustomItemText;

        /// <summary>
        /// Gets or sets string search provider.
        /// </summary>
        [Browsable(false)]
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
        /// the control.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value
        /// of <c>null</c> is returned if no item is selected.</value>
        [Browsable(false)]
        public abstract int? SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in
        /// the control. Same as <see cref="SelectedIndex"/> but is not nullable.
        /// </summary>
        [Browsable(false)]
        public virtual int SelectedIndexAsInt
        {
            get
            {
                var result = SelectedIndex;
                if (result is null)
                    return -1;
                return result.Value;
            }

            set
            {
                if (SelectedIndexAsInt == value)
                    return;
                if (value < 0)
                    SelectedIndex = null;
                else
                    SelectedIndex = value;
            }
        }

        /// <summary>
        /// Returns <see cref="Action"/> associated with the <see cref="SelectedItem"/> if it is
        /// <see cref="ListControlItem"/>.
        /// </summary>
        [Browsable(false)]
        public virtual Action? SelectedAction => (SelectedItem as ListControlItem)?.Action;

        /// <summary>
        /// Returns <see cref="ObjectUniqueId"/> associated
        /// with the <see cref="SelectedItem"/> if it is
        /// <see cref="ListControlItem"/>.
        /// </summary>
        [Browsable(false)]
        public virtual ObjectUniqueId? SelectedUniqueId =>
            (SelectedItem as ListControlItem)?.UniqueId;

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
        public virtual IListControlItems<object> Items
        {
            get
            {
                return items ??= new ListControlItems<object>();
            }

            set
            {
                Items.Clear();
                Items.AddRange(value);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the control.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the control.
        /// </returns>
        [Browsable(false)]
        public virtual int Count
        {
            get => Items.Count;

            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual object? LastItem
        {
            get
            {
                var count = Count;
                if (count > 0)
                    return GetItem(count - 1);
                return null;
            }

            set
            {
                var count = Count;
                if (count > 0 && value is not null)
                    SetItem(count - 1, value);
            }
        }

        /// <summary>
        /// Gets first item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual object? FirstItem
        {
            get
            {
                if (Count > 0)
                    return GetItem(0);
                return null;
            }

            set
            {
                if (Count > 0 && value is not null)
                    SetItem(0, value);
            }
        }

        /// <summary>
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual object? LastRootItem
        {
            get => LastItem;
            set => LastItem = value;
        }

        [Browsable(false)]
        int IReadOnlyStrings.Count
        {
            get
            {
                return Count;
            }
        }

        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        internal new Thickness? MinChildMargin
        {
            get => base.MinChildMargin;
            set => base.MinChildMargin = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public virtual object? this[long index]
        {
            get
            {
                return GetItem((int)index);
            }

            set => SetItem((int)index, value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public virtual object? this[int index]
        {
            get
            {
                return GetItem(index);
            }

            set => SetItem(index, value ?? throw new ArgumentNullException(nameof(value)));
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element or
        /// <c>null</c>.</param>
        /// <returns>The element at the specified index or <c>null</c>.</returns>
        public virtual object? this[int? index]
        {
            get
            {
                if (index is null)
                    return null;
                return GetItem(index.Value);
            }

            set
            {
                if (index is null)
                    return;
                SetItem(index.Value, value ?? throw new ArgumentNullException(nameof(value)));
            }
        }

        string? IReadOnlyStrings.this[int index] => GetItemText(index);

        /// <summary>
        /// Gets item with the specified index.
        /// This methods is called from all other methods that
        /// request the item.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual object? GetItem(int index)
        {
            object? result;
            if (index >= 0 && items is not null && index < items.Count)
                result = Items[index];
            else
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Sets item with the specified index.
        /// This methods is called from all other methods that
        /// change the item.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <param name="value">New item value.</param>
        public virtual void SetItem(int index, object value)
        {
            if (index >= 0 && items is not null && index < items.Count)
                Items[index] = value;
            else
            {
            }
        }

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
        /// Gets item with the specified index.
        /// If index of the item is invalid, returns null.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        public virtual T? SafeItem<T>(int index)
            where T : class
        {
            if (index < 0 || index >= Count)
                return default;
            return GetItem(index) as T;
        }

        /// <summary>
        /// Gets item with the specified index.
        /// If index of the item is invalid, throws an exception.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="index">Index of the item.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T RequiredItem<T>(int index)
            where T : class
            => SafeItem<T>(index) ?? throw new NullReferenceException();

        /// <summary>
        /// Gets <see cref="ListControlItem"/> item with the specified index.
        /// If index of the item is invalid or item is not <see cref="ListControlItem"/>,
        /// returns <c>null</c>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ListControlItem? SafeItem(int index) => SafeItem<ListControlItem>(index);

        /// <summary>
        /// Gets <see cref="ListControlItem"/> item with the specified index.
        /// If index of the item is invalid, an exception is thrown.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ListControlItem RequiredItem(int index)
            => SafeItem<ListControlItem>(index) ?? throw new NullReferenceException();

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int Add(string text, object? data)
        {
            return Add(new ListControlItem(text, data));
        }

        /// <summary>
        /// Adds <paramref name="text"/> with <paramref name="action"/> to the end of
        /// the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="text">Item text (title).</param>
        /// <param name="action">Action associated with the item.</param>
        /// <remarks>
        /// This method creates <see cref="ListControlItem"/>, assigns its properties with
        /// <paramref name="text"/> and <paramref name="action"/>. Created object is added to
        /// the <see cref="Items"/> collection.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int Add(string text, Action action)
        {
            return Add(new ListControlItem(text, action));
        }

        /// <summary>
        /// Selects last item in the control.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SelectLastItem()
        {
            if (Count > 0)
                SelectedIndex = Count - 1;
        }

        /// <summary>
        /// Returns the text representation of item with the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Item index from which to get the contents to display.</param>
        public virtual string GetItemText(int index)
        {
            object? s;
            s = GetItem(index);
            var result = GetItemText(s);

            if(CustomItemText is null)
                return result;

            GetItemTextEventArgs e = new(index, s, result);
            CustomItemText(this, e);
            if (e.Handled)
                return e.Result;
            return result;
        }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AddEnumValues<T>()
            where T : Enum
        {
            AddEnumValues(typeof(T));
        }

        /// <summary>
        /// Casts selected item to <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? SelectedItemAs<T>() => (T?)SelectedItem;

        /// <summary>
        /// Adds enum values to <see cref="Items"/> property of the control.
        /// </summary>
        /// <typeparam name="T">Type of the enum which values are added.</typeparam>
        /// <param name="selectValue">New <see cref="SelectedItem"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AddEnumValues<T>(T selectValue)
            where T : Enum
        {
            AddEnumValues(typeof(T), selectValue);
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
                    result = GetItemText(index);
                else
                    result += $"{separator}{Items[index]}";
            }

            return result;
        }

        /// <inheritdoc cref="StringSearch.FindString(string)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int? FindString(string s) => Search.FindString(s);

        /// <inheritdoc cref="StringSearch.FindString(string, int?)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int? FindString(string s, int? startIndex) => Search.FindString(s, startIndex);

        /// <inheritdoc cref="StringSearch.FindStringExact(string)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int? FindStringExact(string s) => Search.FindStringExact(s);

        /// <inheritdoc cref="StringSearch.FindStringEx(string?, int?, bool, bool)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual int? FindStringEx(
         string? str,
         int? startIndex,
         bool exact,
         bool ignoreCase) => Search.FindStringEx(str, startIndex, exact, ignoreCase);

        /// <summary>
        /// Sets <see cref="SelectedIndex"/>. Implemented as method for the convenience.
        /// </summary>
        /// <param name="index"></param>
        public virtual void SetSelectedIndex(int? index) => SelectedIndex = index;

        /// <summary>
        /// Unselects all items in the control.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to setting the
        /// <see cref="SelectedIndex"/> property to <c>null</c>.
        /// You can use this method to quickly unselect all items in the list.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void ClearSelected()
        {
            SelectedItem = null;
        }
    }
}