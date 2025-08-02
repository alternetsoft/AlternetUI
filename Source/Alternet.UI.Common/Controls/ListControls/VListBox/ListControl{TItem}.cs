using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a common implementation of members for the <see cref="StdListBox"/>,
    /// <see cref="ComboBox"/> and other controls.
    /// </summary>
    /// <typeparam name="TItem">Type of the item. Can be <see cref="object"/>,
    /// <see cref="ListControlItem"/> or any other type.</typeparam>
    public abstract partial class ListControl<TItem>
        : UserControl, IReadOnlyStrings, IListControl<TItem>
        where TItem : class, new()
    {
        private StringSearch? search;
        private BaseCollection<TItem>? items;
        private SvgImage? checkImageUnchecked;
        private SvgImage? checkImageChecked;
        private SvgImage? checkImageIndeterminate;

        /// <summary>
        /// Occurs when controls needs to get string representation of the item for the display
        /// or other purposes. Called from <see cref="GetItemText(int, bool)"/>.
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
        /// Gets or sets format provider used to get text representation of the item
        /// for the display and other purposes. Default is <c>null</c>.
        /// If not specified, <see cref="CultureInfo.CurrentCulture"/> is used.
        /// </summary>
        public virtual IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual TItem? LastItem
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
        /// Gets or sets the unchecked state image for the checkbox.
        /// </summary>
        public virtual SvgImage? CheckImageUnchecked
        {
            get
            {
                return checkImageUnchecked;
            }

            set
            {
                if (checkImageUnchecked == value)
                    return;
                checkImageUnchecked = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the checked state image for the checkbox.
        /// </summary>
        public virtual SvgImage? CheckImageChecked
        {
            get
            {
                return checkImageChecked;
            }

            set
            {
                if (checkImageChecked == value)
                    return;
                checkImageChecked = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the indeterminate state image for the checkbox.
        /// </summary>
        public virtual SvgImage? CheckImageIndeterminate
        {
            get
            {
                return checkImageIndeterminate;
            }

            set
            {
                if (checkImageIndeterminate == value)
                    return;
                checkImageIndeterminate = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets first item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public virtual TItem? FirstItem
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
        public virtual TItem? LastRootItem
        {
            get => LastItem;
            set => LastItem = value;
        }

        /// <summary>
        /// Gets or sets the items of the control.
        /// </summary>
        /// <value>A collection representing the items
        /// in the control.</value>
        /// <remarks>This property enables you to obtain a reference to the list
        /// of items that are currently stored in the control.
        /// With this reference, you can add items, remove items, and obtain
        /// a count of the items in the collection.</remarks>
        [Content]
        public virtual BaseCollection<TItem> Items
        {
            get
            {
                return SafeItems();
            }

            set
            {
                DoInsideUpdate(() =>
                {
                    RemoveAll();
                    Items.AddRange(value);
                });
            }
        }

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
        /// Gets the number of items contained in the control.
        /// </summary>
        /// <returns>
        /// The number of items contained in the control.
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
        public virtual TItem? this[long index]
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
        public virtual TItem? this[int index]
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
        public virtual TItem? this[int? index]
        {
            get
            {
                if (index is null)
                    return default;
                return GetItem(index.Value);
            }

            set
            {
                if (index is null)
                    return;
                SetItem(index.Value, value ?? throw new ArgumentNullException(nameof(value)));
            }
        }

        string? IReadOnlyStrings.this[int index] => GetItemText(index, false);

        /// <summary>
        /// Gets item with the specified index.
        /// This methods is called from all other methods that
        /// request the item.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public virtual TItem? GetItem(int index)
        {
            TItem? result;
            if (index >= 0 && items is not null && index < items.Count)
                result = Items[index];
            else
            {
                result = default;
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
        public virtual void SetItem(int index, TItem value)
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
        public virtual int Add(TItem item)
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
        /// <param name="forDisplay">The flag which specifies whether to get item's text
        /// for display purposes or the real value.</param>
        public virtual string GetItemText(int index, bool forDisplay)
        {
            TItem? s;
            s = GetItem(index);
            var result = GetItemText(s, forDisplay);

            if (CustomItemText is null)
                return result;

            GetItemTextEventArgs e = new(index, s, result, forDisplay);
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
        /// <param name="forDisplay">The flag which specifies whether to get text
        /// for display purposes or the real value.</param>
        public virtual string GetItemText(TItem? item, bool forDisplay)
        {
            if (item is null)
                return string.Empty;
            if (item is string s)
                return s;

            if (item is ListControlItem listItem)
            {
                object result;

                if (forDisplay)
                {
                    result = listItem.DisplayText ?? listItem.Text ?? listItem.Value ?? string.Empty;
                }
                else
                {
                    result = listItem.Text ?? listItem.Value ?? string.Empty;
                }

                return Cnv(result);
            }
            else
            {
                return Cnv(item);
            }

            string Cnv(object v)
            {
                var result = Convert.ToString(v, FormatProvider ?? CultureInfo.CurrentCulture);
                return result ?? string.Empty;
            }
        }

        /// <summary>
        /// Selects first item in the control.
        /// </summary>
        public virtual void SelectFirstItem()
        {
            if (Count == 0)
                return;

            DoInsideUpdate(() =>
            {
                SelectedIndex = 0;
            });
        }

        /// <summary>
        /// Casts selected item to <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? SelectedItemAs<T>()
            where T : TItem => (T?)SelectedItem;

        /// <summary>
        /// Removes all items from the control.
        /// </summary>
        public virtual void RemoveAll()
        {
            if (Items.Count == 0)
                return;
            DoInsideUpdate(() =>
            {
                ClearSelected();
                Items.Clear();
            });
        }

        /// <summary>
        /// Removes items from the control.
        /// </summary>
        public virtual bool RemoveItems(IReadOnlyList<int> items)
        {
            if (items == null || items.Count == 0)
                return false;

            BeginUpdate();
            try
            {
                ClearSelected();
                foreach (int index in items)
                {
                    if (index < Items.Count)
                        Items.RemoveAt(index);
                }

                return true;
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
                    result = GetItemText(index, false);
                else
                    result += $"{separator}{Items[index]}";
            }

            return result;
        }

        /// <inheritdoc cref="StringSearch.FindString(string)"/>
        public virtual int? FindString(string? s) => Search.FindString(s);

        /// <inheritdoc cref="StringSearch.FindString(string, int?)"/>
        public virtual int? FindString(string? s, int? startIndex)
        {
            return Search.FindString(s, startIndex);
        }

        /// <inheritdoc cref="StringSearch.FindStringExact(string)"/>
        public virtual int? FindStringExact(string? s)
        {
            return Search.FindStringExact(s);
        }

        /// <inheritdoc cref="StringSearch.FindStringEx(string?, int?, bool, bool)"/>
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
        public virtual void ClearSelected()
        {
            SelectedItem = default;
        }

        /// <summary>
        /// Same as <see cref="RemoveItemWithValue"/>, but checks for the condition
        /// before removing the item.
        /// </summary>
        public bool RemoveItemWithValueIf(object? value, bool condition)
        {
            if (condition)
                return RemoveItemWithValue(value);
            return false;
        }

        /// <summary>
        /// Removes item with the specified value. Item is removed if it equals
        /// the specified value or
        /// if it is <see cref="ListControlItem"/> and it's <see cref="ListControlItem.Value"/>
        /// property equals the value.
        /// </summary>
        public virtual bool RemoveItemWithValue(object? value)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];

                if (ValueEquals(item))
                {
                    Items.RemoveAt(i);
                    return true;
                }
            }

            bool ValueEquals(object item)
            {
                if (item.Equals(value))
                    return true;

                if (item is not ListControlItem listItem)
                    return false;

                return listItem.Value?.Equals(value) ?? (value is null);
            }

            return false;
        }

        /// <summary>
        /// Changes the number of elements in the <see cref="Items"/>.
        /// </summary>
        /// <param name="newCount">New number of elements.</param>
        /// <param name="createItem">Function which creates new item.</param>
        /// <remarks>
        /// If collection has more items than specified in <paramref name="newCount"/>,
        /// these items are removed. If collection has less items, new items are created
        /// using <paramref name="createItem"/> function.
        /// </remarks>
        public virtual void SetCount(int newCount, Func<TItem> createItem)
        {
            var safeItems = SafeItems();
            safeItems.SetCount(newCount, createItem);
        }

        /// <summary>
        /// Called when items are attached to the control.
        /// </summary>
        /// <param name="itm">Attached items.</param>
        protected virtual void AttachItems(BaseCollection<TItem>? itm)
        {
            if (itm is null)
                return;
            itm.CollectionChanged += ItemsCollectionChanged;
        }

        /// <summary>
        /// Called when items are detached to the control.
        /// </summary>
        /// <param name="itm">Detached items.</param>
        protected virtual void DetachItems(BaseCollection<TItem>? itm)
        {
            if (itm is null)
                return;
            itm.CollectionChanged -= ItemsCollectionChanged;
        }

        /// <summary>
        /// Callback which is called when items are changed in the control.
        /// </summary>
        protected virtual void ItemsCollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Recreates items. Before calling this method, you need to unbind all events
        /// connected to the <see cref="Items"/>.
        /// </summary>
        protected virtual void RecreateItems(BaseCollection<TItem>? newItems = null)
        {
            DetachItems(Items);
            items = newItems;
        }

        /// <summary>
        /// Gets items.
        /// </summary>
        /// <returns></returns>
        protected virtual BaseCollection<TItem> SafeItems()
        {
            if(items is null)
            {
                items = new NotNullCollection<TItem>();
                AttachItems(items);
            }

            return items;
        }
    }
}