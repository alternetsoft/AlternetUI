using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items with checkboxes.
    /// Please consider using <see cref="VirtualCheckListBox"/>
    /// instead of this control as it is faster.
    /// </summary>
    /// <remarks>
    /// The <see cref="CheckListBox"/> control enables you to display a list of
    /// items to the user that the user can check by clicking.
    /// A <see cref="CheckListBox"/> control can provide single or
    /// multiple selections using the <see cref="VirtualListControl.SelectionMode"/> property.
    /// The <see cref="AbstractControl.BeginUpdate"/> and <see cref="AbstractControl.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the CheckListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="ListControl{T}.Items"/>, <see cref="VirtualListControl.SelectedItems"/>,
    /// <see cref="VirtualListControl.SelectedIndices"/>, and
    /// <see cref="VirtualListControl.CheckedIndices"/>
    /// properties provide access to the
    /// collections that are used by the control.
    /// </remarks>
    [ControlCategory("Common")]
    public class CheckListBox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListBox"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CheckListBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckListBox"/> class.
        /// </summary>
        public CheckListBox()
        {
            CheckBoxVisible = true;
        }
    }
}
