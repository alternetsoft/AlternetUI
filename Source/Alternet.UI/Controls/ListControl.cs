using System;
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
            new Collection<object> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Gets the number of elements actually contained in the <see cref="Items"/>
        /// collection.
        /// </summary>
        /// <returns>
        /// The number of elements actually contained in the <see cref="Items"/>.
        /// </returns>
        public int Count => Items.Count;

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
    }
}