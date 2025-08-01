using System;
using System.ComponentModel;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an item (also known as a node) of a <see cref="TreeView"/>.
    /// </summary>
    public class TreeViewItem : TreeControlItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class
        /// with default values.
        /// </summary>
        public TreeViewItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/>
        /// class with the specified item text.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        public TreeViewItem(string text)
            : base(text)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/>
        /// class with the specified item text and
        /// the image index position of the item's icon.
        /// </summary>
        /// <param name="text">The text to display for the item.</param>
        /// <param name="imageIndex">
        /// The zero-based index of the image within the
        /// <see cref="VirtualTreeControl.ImageList"/>
        /// associated with the <see cref="VirtualTreeControl"/> that contains the
        /// item.
        /// </param>
        public TreeViewItem(string text, int? imageIndex)
            : base(text, imageIndex)
        {
        }
    }
}