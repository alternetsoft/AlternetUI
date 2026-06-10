using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class ListControl<TItem>
    {
        /// <summary>
        /// Gets or sets the source of items for the control.
        /// </summary>
        public abstract IListSource<TItem> Items { get; set; }
    }
}
