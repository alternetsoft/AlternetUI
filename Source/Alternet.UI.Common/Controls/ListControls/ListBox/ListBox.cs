using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items.
    /// Please consider using <see cref="VirtualListBox"/>
    /// instead of this control as it is faster.
    /// </summary>
    /// <remarks>
    /// The <see cref="ListBox"/> control enables you to display a list of items to
    /// the user that the user can select by clicking.
    /// A <see cref="ListBox"/> control can provide single or multiple selections
    /// using the <see cref="SelectionMode"/> property.
    /// The <see cref="AbstractControl.BeginUpdate"/> and <see cref="AbstractControl.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the ListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="ListControl{T}.Items"/>, <see cref="VirtualListControl.SelectedItems"/>, and
    /// <see cref="VirtualListControl.SelectedIndices"/> properties provide access to the three
    /// collections that are used by the control.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class ListBox : VirtualListBox, ICustomListBox<object>
    {
        private readonly ListBoxItems adapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        public ListBox()
        {
            adapter = new ListBoxItems(base.Items);
        }

        /// <summary>
        /// Gets last item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public new object? LastItem
        {
            get
            {
                return ItemToData(base.LastItem);
            }

            set
            {
                if (LastItem == value)
                    return;
                base.LastItem = DataToItem(value);
            }
        }

        /// <summary>
        /// Gets first item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public new object? FirstItem
        {
            get
            {
                return ItemToData(base.FirstItem);
            }

            set
            {
                if (FirstItem == value)
                    return;
                base.FirstItem = DataToItem(value);
            }
        }

        /// <summary>
        /// Gets last root item in the control or <c>null</c> if there are no items.
        /// </summary>
        [Browsable(false)]
        public new object? LastRootItem
        {
            get
            {
                return ItemToData(base.LastRootItem);
            }

            set
            {
                if (LastRootItem == value)
                    return;
                base.LastRootItem = DataToItem(value);
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item in the control.
        /// </summary>
        /// <value>An object that represents the current selection in the control,
        /// or <c>null</c> if no item is selected.</value>
        /// <remarks>
        /// When single selection mode is used, you can use this property to
        /// determine the index of the item that is selected
        /// in the control. If the <see cref="SelectionMode"/>
        /// property of the control is set to either
        /// <see cref="ListBoxSelectionMode.Multiple"/> (which indicates a
        /// multiple-selection control) and multiple items
        /// are selected in the list, this property can return the index to
        /// any selected item.
        /// <para>
        /// To retrieve a collection containing all selected items in a
        /// multiple-selection control, use the
        /// <see cref="VirtualListControl.SelectedItems"/> property.
        /// If you want to obtain the index position of the currently selected
        /// item in the control, use the
        /// <see cref="VirtualListControl.SelectedIndex"/> property.
        /// In addition, you can use the <see cref="VirtualListControl.SelectedIndices"/>
        /// property to obtain all the selected indexes in a multiple-selection control.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public new object? SelectedItem
        {
            get
            {
                return ItemToData(base.SelectedItem);
            }

            set
            {
                if (SelectedItem == value)
                    return;
                base.SelectedItem = DataToItem(value);
            }
        }

        /// <summary>
        /// Gets a collection of the selected items.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<object> SelectedItemsCollection
        {
            get
            {
                foreach (var item in base.SelectedItems)
                {
                    var result = ItemToData(item);
                    if(result is not null)
                        yield return result;
                }
            }
        }

        /// <summary>
        /// Gets or sets items as collection <see cref="ListControlItem"/> items.
        /// This is the fastest way to access items.
        /// </summary>
        [Browsable(false)]
        public BaseCollection<ListControlItem> BaseItems
        {
            get
            {
                return base.Items;
            }

            set
            {
                base.Items = value;
            }
        }

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        public new ListBoxItems Items
        {
            get
            {
                return adapter;
            }

            set
            {
                NotNullCollection<ListControlItem> newItems = new();
                foreach(var valueItem in value)
                {
                    ListControlItem item = new();
                    SetItemData(item, valueItem);
                    newItems.Add(item);
                }

                base.Items = newItems;
            }
        }

        /// <summary>
        /// Gets an array of the selected items.
        /// </summary>
        [Browsable(false)]
        public new IReadOnlyList<object> SelectedItems
        {
            get
            {
                return SelectedItemsCollection.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new object? this[int? index]
        {
            get
            {
                return ItemToData(base[index]);
            }

            set
            {
                SetItemData(base[index], value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new object? this[int index]
        {
            get
            {
                return ItemToData(base[index]);
            }

            set
            {
                SetItemData(base[index], value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Items"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element
        /// to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public new object? this[long index]
        {
            get
            {
                return ItemToData(base[index]);
            }

            set
            {
                SetItemData(base[index], value);
            }
        }

        /// <summary>
        /// Gets item with the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        public new object? GetItem(int index)
        {
            return ItemToData(base.GetItem(index));
        }

        /// <summary>
        /// Sets item with the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <param name="value">New item value.</param>
        public virtual void SetItem(int index, object value)
        {
            if (value is ListControlItem listItem)
            {
                base.SetItem(index, listItem);
            }
            else
            {
                ListControlItem item = new();
                SetItemData(item, value);
                base.SetItem(index, item);
            }
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="value">The object to be added to the end of the
        /// <see cref="Items"/> collection.</param>
        public virtual int Add(object value)
        {
            if(value is ListControlItem listItem)
            {
                return base.Add(listItem);
            }
            else
            {
                ListControlItem item = new();
                SetItemData(item, value);
                return base.Add(item);
            }
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
        public void SetCount(int newCount, Func<object> createItem)
        {
            base.SetCount(newCount, () =>
            {
                ListControlItem item = new();
                SetItemData(item, createItem());
                return item;
            });
        }

        /// <summary>
        /// Gets data associated with the item.
        /// </summary>
        /// <param name="item">Item which contains the data.</param>
        /// <returns></returns>
        protected virtual object? ItemToData(ListControlItem? item)
        {
            return item?.Value;
        }

        /// <summary>
        /// Finds the item with the specified data.
        /// </summary>
        /// <param name="data">Data to search for.</param>
        /// <returns></returns>
        protected virtual ListControlItem? DataToItem(object? data)
        {
            return FindItemWithValue(data);
        }

        /// <summary>
        /// Sets item data.
        /// </summary>
        protected virtual void SetItemData(ListControlItem? item, object? data)
        {
            item?.SetValue(data);
        }
    }
}