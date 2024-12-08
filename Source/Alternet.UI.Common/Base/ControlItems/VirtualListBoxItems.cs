using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Items collection used in <see cref="VirtualListBox"/>.
    /// </summary>
    public class VirtualListBoxItems : ListControlItems<ListControlItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBoxItems"/> class.
        /// </summary>
        public VirtualListBoxItems()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBoxItems"/> class
        /// class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public VirtualListBoxItems(IEnumerable<ListControlItem> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualListBoxItems"/> class
        /// that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        public VirtualListBoxItems(List<ListControlItem> list)
            : base(list)
        {
        }
    }
}
