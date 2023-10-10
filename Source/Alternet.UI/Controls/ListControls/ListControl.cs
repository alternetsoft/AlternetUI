using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a common implementation of members for the ListBox and
    /// ComboBox classes.
    /// </summary>
    public abstract class ListControl : Control
    {
        // todo: copy DataSource, DisplayMember, ValueMember etc implementation
        // from WinForms

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
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public object? LastRootItem
        {
            get => LastItem;
            set => LastItem = value;
        }

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
        /// Adds an object to the end of the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="item">The object to be added to the end of the
        /// <see cref="Items"/> collection.</param>
        public virtual void Add(object item)
        {
            Items.Add(item);
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
        /// Returns the index of the first item in the control that starts
        /// with the specified string.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to search for.</param>
        /// <returns>The zero-based index of the first item found;
        /// returns <c>null</c> if no match is found.</returns>
        /// <remarks>
        /// The search performed by this method is not case-sensitive.
        /// The <paramref name="s"/> parameter is a substring to compare against
        /// the text associated with the items in the control. The search performs
        /// a partial match starting from the beginning of the text, and returning
        /// the first item in the list that matches the specified substring.
        /// </remarks>
        public virtual int? FindString(string s)
        {
            return FindStringInternal(
                s,
                Items,
                startIndex: null,
                exact: false,
                ignoreCase: true);
        }

        /// <summary>
        /// Returns the index of the first item in the control beyond the specified
        /// index that contains the specified string. The search is not case sensitive.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before
        /// the first item to be searched. Set to <c>null</c> to search from the beginning
        /// of the control.</param>
        /// <returns>The zero-based index of the first item found; returns <c>null</c> if
        /// no match is found.</returns>
        /// <remarks>
        /// The search performed by this method is not case-sensitive. The <paramref name="s"/>
        /// parameter is a substring to compare against the text associated with the
        /// <see cref="Items"/> in the control. The search performs a partial match
        /// starting from the beginning of the text, returning the first item in the
        /// list that matches the specified substring.
        /// </remarks>
        public virtual int? FindString(string s, int? startIndex)
        {
            return FindStringInternal(
                s,
                Items,
                startIndex,
                exact: false,
                ignoreCase: true);
        }

        /// <summary>
        /// Finds the first item in the combo box that matches the specified string.
        /// </summary>
        /// <param name="s">The string to search for.</param>
        /// <returns>The zero-based index of the first item found; returns
        /// <c>null</c> if no match is found.</returns>
        /// <remarks>
        /// The search performed by this method is not case-sensitive.
        /// </remarks>
        public virtual int? FindStringExact(string s)
        {
            return FindStringInternal(
                s,
                Items,
                startIndex: null,
                exact: true,
                ignoreCase: true);
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

        /// <summary>
        /// Returns the index of the first item in the control beyond the specified
        /// index that contains or equal the specified string.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to search for.</param>
        /// <param name="startIndex">The zero-based index of the item before
        /// the first item to be searched. Set to <c>null</c> to search from the beginning
        /// of the control.</param>
        /// <returns>The zero-based index of the first item found; returns <c>null</c> if
        /// no match is found.</returns>
        /// <remarks>
        /// The <paramref name="str"/>
        /// parameter is a substring to compare against the text associated with the
        /// <see cref="Items"/> in the control.
        /// The search performs a partial match (<paramref name="exact"/> is <c>false</c>)
        /// or exact match (<paramref name="exact"/> is <c>true</c>)
        /// Search starts from the beginning of the text, returning the first item in the
        /// list that matches the specified substring.
        /// </remarks>
        /// <param name="exact"><c>true</c> uses exact comparison; <c>false</c> uses partial
        /// compare.</param>
        /// <param name="ignoreCase">Whether to ignore text case or not.</param>
        public virtual int? FindStringEx(
         string? str,
         int? startIndex,
         bool exact,
         bool ignoreCase)
        {
            return FindStringInternal(
             str,
             Items,
             startIndex,
             exact,
             ignoreCase);
        }

        private int? FindStringInternal(
             string? str,
             IList<object> items,
             int? startIndex,
             bool exact,
             bool ignoreCase)
        {
            if (str is null)
                return null;
            if (items is null || items.Count == 0)
                return null;

            var startIndexInt = startIndex ?? -1;

            if (startIndexInt < -1 || startIndexInt >= items.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            // Start from the start index and wrap around until we find the string
            // in question. Use a separate counter to ensure that we arent cycling
            // through the list infinitely.
            int numberOfTimesThroughLoop = 0;

            // this API is really Find NEXT String...
            for (
                int index = (startIndexInt + 1) % items.Count;
                numberOfTimesThroughLoop < items.Count;
                index = (index + 1) % items.Count)
            {
                numberOfTimesThroughLoop++;

                bool found;
                if (exact)
                {
                    found = string.Compare(
                        str,
                        GetItemText(items[index]),
                        ignoreCase,
                        CultureInfo.CurrentCulture) == 0;
                }
                else
                {
                    found = string.Compare(
                        str,
                        0,
                        GetItemText(items[index]),
                        0,
                        str.Length,
                        ignoreCase,
                        CultureInfo.CurrentCulture) == 0;
                }

                if (found)
                    return index;
            }

            return null;
        }
    }
}