using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the <see langword="ItemCheck" /> event
    /// of a <see cref="VirtualListBox" /> control.</summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An <see cref="ItemCheckEventArgs" /> that contains the event data.</param>
    internal delegate void ItemCheckEventHandler(object? sender, ItemCheckEventArgs e);

    /// <summary>
    /// Provides data for the <see langword="ItemCheck" /> event
    /// of the <see cref="VirtualListBox" /> control.
    /// </summary>
    internal class ItemCheckEventArgs : EventArgs
    {
        private readonly int index;
        private readonly CheckState currentValue;

        private CheckState newValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCheckEventArgs" /> class.
        /// </summary>
        /// <param name="index">The zero-based index of the item to change.</param>
        /// <param name="newCheckValue">One of the <see cref="CheckState" /> values
        /// that indicates whether to change the check box for the item to
        /// be checked, unchecked, or indeterminate.</param>
        /// <param name="currentValue">One of the <see cref="CheckState" /> values
        /// that indicates whether the check box for the item is currently checked,
        /// unchecked, or indeterminate.</param>
        public ItemCheckEventArgs(int index, CheckState newCheckValue, CheckState currentValue)
        {
            this.index = index;
            newValue = newCheckValue;
            this.currentValue = currentValue;
        }

        /// <summary>
        /// Gets the zero-based index of the item to change.
        /// </summary>
        /// <returns>
        /// The zero-based index of the item to change.
        /// </returns>
        public int Index => index;

        /// <summary>
        /// Gets or sets a value indicating whether to set the check box for the
        /// item to be checked, unchecked, or indeterminate.
        /// </summary>
        /// <returns>One of the <see cref="CheckState" /> values.</returns>
        public CheckState NewValue
        {
            get
            {
                return newValue;
            }

            set
            {
                newValue = value;
            }
        }

        /// <summary>
        /// Gets a value indicating the current state of the item's check box.
        /// </summary>
        /// <returns>
        /// One of the <see cref="CheckState" /> values.
        /// </returns>
        public CheckState CurrentValue => currentValue;
    }
}
