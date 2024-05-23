using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the item text request events.
    /// </summary>
    public class GetItemTextEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetItemTextEventArgs"/> class.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="item">Item.</param>
        /// <param name="defaultResult">Default string representation of the item.</param>
        public GetItemTextEventArgs(int itemIndex, object? item, string defaultResult)
        {
            ItemIndex = itemIndex;
            Item = item;
            Result = defaultResult;
        }

        /// <summary>
        /// Gets index of the item.
        /// </summary>
        public int ItemIndex { get; }

        /// <summary>
        /// Gets item.
        /// </summary>
        public object? Item { get; }

        /// <summary>
        /// Gets or sets string representation of the item.
        /// </summary>
        public string Result { get; set; }
    }
}
