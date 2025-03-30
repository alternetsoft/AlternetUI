using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the root item in a tree control.
    /// </summary>
    public partial class TreeControlRootItem : TreeControlItem
    {
        private readonly ITreeControlItemContainer owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class
        /// with the specified owner. Use this constructor only for the root items.
        /// </summary>
        /// <param name="owner">The owner of the item.</param>
        public TreeControlRootItem(ITreeControlItemContainer owner)
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
    }
}
