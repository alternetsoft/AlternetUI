using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="TreeViewItem"/> with horizontal separator painting.
    /// </summary>
    public class TreeViewSeparatorItem : TreeViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewSeparatorItem"/> class.
        /// </summary>
        public TreeViewSeparatorItem()
        {
            this.HideSelection = true;
            ForegroundColor = ListControlSeparatorItem.DefaultSeparatorColor ?? DefaultColors.BorderColor;
            DrawForegroundAction = ListControlSeparatorItem.DefaultDrawSeparator;
            DrawBackgroundAction = (s, e) => { };
        }
    }
}
