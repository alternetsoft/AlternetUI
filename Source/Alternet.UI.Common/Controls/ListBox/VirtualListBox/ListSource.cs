using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a source of items for a list control, providing
    /// functionality to manage the collection of items and notify changes.
    /// </summary>
    public class ListSource : ListSource<ListControlItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListSource"/> class.
        /// </summary>
        public ListSource()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSource"/> class with the specified collection of items.
        /// </summary>
        /// <param name="collection">The collection of items to initialize the list source with.</param>
        public ListSource(BaseCollection<ListControlItem>? collection)
            : base(collection)
        {
        }
    }
}
