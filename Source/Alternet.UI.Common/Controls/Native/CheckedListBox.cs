using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list box control that displays a collection of items, each with an associated checkbox,
    /// allowing the user to select multiple items.
    /// This control encapsulates the native check list box implemented by the operating system.
    /// It has limited functionality.
    /// For the full featured list control, use the <see cref="VirtualListBox"/>
    /// or <see cref="VirtualCheckListBox"/> controls.
    /// </summary>
    /// <remarks>The <see cref="CheckedListBox"/> control extends the functionality of a standard
    /// <see cref="ListBox"/> by adding checkboxes to each item. This control is commonly used
    /// in scenarios where multiple selections are required.
    /// To use this control, populate the list with items and handle user interactions as needed.</remarks>
    public class CheckedListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckedListBox"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CheckedListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckedListBox"/> class.
        /// </summary>
        public CheckedListBox()
        {
        }

        /// <summary>
        /// Occurs when the collection of checked items changes.
        /// </summary>
        /// <remarks>This event is raised whenever an item is added to or removed from the collection of
        /// checked items. Subscribers can use this event to respond to changes in the selection state.</remarks>
        public event EventHandler? CheckedItemsChanged;

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool CheckBoxVisible
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Collection of checked indices in this control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<int> CheckedIndices
        {
            get
            {
                var result = ListUtils.GetSelectedItems(
                    PlatformControl.UpdateCheckedIndexes,
                    PlatformControl.GetCheckedIndexesCount,
                    PlatformControl.GetCheckedIndexesItem);
                return result;
            }
        }

        /// <summary>
        ///  Collection of checked items in this CheckedListBox.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IReadOnlyList<object> CheckedItems
        {
            get
            {
                object GetItemAtIndex(int index)
                {
                    var itemIndex = PlatformControl.GetCheckedIndexesItem(index);
                    if (itemIndex >= 0 && itemIndex < Items.Count)
                        return Items[itemIndex];
                    return null!;
                }

                var result = ListUtils.GetSelectedItems(
                    PlatformControl.UpdateCheckedIndexes,
                    PlatformControl.GetCheckedIndexesCount,
                    GetItemAtIndex);

                return result;
            }
        }

        /// <summary>
        /// Raises the <see cref="CheckedItemsChanged"/> event and <see cref="OnCheckedItemsChanged"/> method.
        /// </summary>
        /// <remarks>This method invokes the <see cref="CheckedItemsChanged"/> event, allowing
        /// subscribers to handle changes to the checked items.
        /// It can be called to programmatically trigger the event.</remarks>
        public void RaiseCheckedItemsChanged()
        {
            OnCheckedItemsChanged(EventArgs.Empty);
            CheckedItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the item at the specified index is checked.
        /// </summary>
        /// <param name="index">The zero-based index of the item to check.</param>
        /// <returns><see langword="true"/> if the item at the specified index is checked;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool GetItemChecked(int index)
        {
            if (index < 0 || index >= Items.Count)
                return false;
            return PlatformControl.IsChecked(index);
        }

        /// <summary>
        /// Sets the checked state of the item at the specified index.
        /// </summary>
        /// <remarks>Use this method to programmatically change the checked state of an item in the
        /// control.</remarks>
        /// <param name="index">The zero-based index of the item to update. Must be within the valid range of items.</param>
        /// <param name="value"><see langword="true"/> to check the item; <see langword="false"/> to uncheck it.</param>
        /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
        public virtual bool SetItemChecked(int index, bool value)
        {
            if(index < 0 || index >= Items.Count)
                return false;
            PlatformControl.Check(index, value);
            return true;
        }

        /// <summary>
        /// Called when the list of checked items changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnCheckedItemsChanged(EventArgs e)
        {
        }
    }
}
