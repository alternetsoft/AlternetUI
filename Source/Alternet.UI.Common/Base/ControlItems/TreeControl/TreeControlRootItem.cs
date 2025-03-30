using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class TreeControlRootItem : TreeControlItem
    {
        private readonly object? owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeControlItem"/> class
        /// with the specified owner. Use this constructor only for the root items.
        /// </summary>
        /// <param name="owner">The owner of the item.</param>
        public TreeControlRootItem(object owner)
        {
            this.owner = owner;
        }

        /// <inheritdoc/>
        public override object? Owner
        {
            get
            {
                return owner;
            }
        }
    }
}
