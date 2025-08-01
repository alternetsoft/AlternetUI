using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Displays a hierarchical collection of labeled items with optional images,
    /// each represented by a <see cref="TreeViewItem"/>.
    /// </summary>
    public partial class TreeView : VirtualTreeControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public TreeView(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeView"/> class.
        /// </summary>
        public TreeView()
        {
        }

    }
}