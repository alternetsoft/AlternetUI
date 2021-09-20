using Alternet.Base.Collections;
using System;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a common implementation of members for the ListBox and ComboBox classes.
    /// </summary>
    public abstract class ListControl : Control
    {
        // todo: copy DataSource, DisplayMember, ValueMember etc implementation from WinForms

        /// <summary>
        /// Gets the items of the <see cref="ListControl"/>.
        /// </summary>
        /// <value>An <see cref="Collection{Object}"/> representing the items in the <see cref="ListControl"/>.</value>
        /// <remarks>This property enables you to obtain a reference to the list of items that are currently stored in the <see cref="ListControl"/>.
        /// With this reference, you can add items, remove items, and obtain a count of the items in the collection.</remarks>
        [Content]
        public Collection<object> Items { get; } = new Collection<object> { ThrowOnNullItemAddition = true };

        /// <summary>
        /// Returns the text representation of the specified item.
        /// </summary>
        /// <param name="item">The object from which to get the contents to display.</param>
        public string GetItemText(object item)
        {
            return item switch
            {
                null => string.Empty,
                string s => s,
                _ => Convert.ToString(item, CultureInfo.CurrentCulture) ?? string.Empty
            };
        }
    }
}