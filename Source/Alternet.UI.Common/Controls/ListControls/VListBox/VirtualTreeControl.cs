using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a tree control that inherits from <see cref="VirtualListBox"/>.
    /// This control is under development, please do not use it.
    /// </summary>
    public partial class VirtualTreeControl : VirtualListBox
    {
        /// <summary>
        /// Default margin for each level in the tree.
        /// </summary>
        public static int DefaultLevelMargin = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeControl"/> class.
        /// </summary>
        public VirtualTreeControl()
        {
        }
    }
}
