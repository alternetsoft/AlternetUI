using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list control item with notification capabilities.
    /// </summary>
    public partial class ListControlItemWithNotify : ListControlItem
    {
        /// <inheritdoc/>
        public override string Text
        {
            get => base.Text;
            set
            {
                if (base.Text == value)
                    return;
                base.Text = value;
                RaisePropertyChanged(nameof(Text));
            }
        }
    }
}
