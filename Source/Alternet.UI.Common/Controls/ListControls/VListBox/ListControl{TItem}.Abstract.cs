using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class ListControl<TItem>
    {
        /// <summary>
        /// Gets or sets the currently selected item in the control.
        /// </summary>
        /// <value>An object that represents the current selection in the
        /// control, or <c>null</c> if no item is selected.</value>
        [Browsable(false)]
        public abstract TItem? SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the currently selected item in
        /// the control.
        /// </summary>
        /// <value>A zero-based index of the currently selected item. A value
        /// of <c>null</c> is returned if no item is selected.</value>
        [Browsable(false)]
        public abstract int? SelectedIndex { get; set; }
    }
}
