using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements an empty item which can be used as a spacer.
    /// </summary>
    public class TreeViewEmptyItem : TreeViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewEmptyItem"/> class.
        /// </summary>
        public TreeViewEmptyItem()
        {
            this.HideSelection = true;
        }
    }
}
