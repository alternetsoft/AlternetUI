using System;
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
        public int Count => Items.Count;

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
        public void Add(object item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Selects last item in the control.
        /// </summary>
        public void SelectLastItem()
        {
            if(Count > 0)
                SelectedIndex = Count - 1;
        }

        /// <summary>
        /// Returns the text representation of the specified item.
        /// </summary>
        /// <param name="item">The object from which to get the contents to
        /// display.</param>
        public virtual string GetItemText(object item)
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
            SelectedItem = selectValue;
        }

        /// <summary>
        /// Removed all items from the control.
        /// </summary>
        public void RemoveAll()
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
    }
}