using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items.
    /// Please consider using <see cref="VirtualListBox"/>
    /// instead of this simple control.
    /// </summary>
    /// <remarks>
    /// The <see cref="ListBox"/> control enables you to display a list of items to
    /// the user that the user can select by clicking.
    /// A <see cref="ListBox"/> control can provide single or multiple selections
    /// using the <see cref="SelectionMode"/> property.
    /// The <see cref="AbstractControl.BeginUpdate"/> and <see cref="AbstractControl.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the ListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="ListControl{T}.Items"/>, <see cref="CustomListBox{T}.SelectedItems"/>, and
    /// <see cref="CustomListBox{T}.SelectedIndices"/> properties provide access to the three
    /// collections that are used by the <see cref="ListBox"/>.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class ListBox : CustomListBox<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBox"/> class.
        /// </summary>
        public ListBox()
        {
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateListBoxHandler(this);
        }
    }
}