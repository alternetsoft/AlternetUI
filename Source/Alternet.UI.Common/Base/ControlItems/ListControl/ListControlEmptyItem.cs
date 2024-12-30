using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements an empty item which can be used as a spacer.
    /// </summary>
    public class ListControlEmptyItem : ListControlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlEmptyItem"/> class.
        /// </summary>
        public ListControlEmptyItem()
        {
            this.HideSelection = true;
        }
    }
}
