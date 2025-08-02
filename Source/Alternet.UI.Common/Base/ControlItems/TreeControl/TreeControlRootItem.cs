using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the root item in a tree control.
    /// </summary>
    public partial class TreeControlRootItem : TreeViewItem
    {
        private ITreeControlItemContainer? owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItem"/> class
        /// with the specified owner. Use this constructor only for the root items.
        /// </summary>
        /// <param name="owner">The owner of the item.</param>
        public TreeControlRootItem(ITreeControlItemContainer? owner = null)
        {
            this.owner = owner;
        }

        /// <inheritdoc/>
        public override ITreeControlItemContainer? Owner
        {
            get
            {
                return owner;
            }
        }

        /// <summary>
        /// Sets the owner of the tree control root item. You should not call this method directly.
        /// </summary>
        /// <param name="owner">The owner to set for the tree control root item.</param>
        public virtual void SetOwner(ITreeControlItemContainer? owner)
        {
            this.owner = owner;
        }
    }
}
