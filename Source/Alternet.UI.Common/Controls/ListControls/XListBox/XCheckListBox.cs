using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control to display a list of items with checkboxes.
    /// This control is implemented inside the library and doesn't use native check list box control.
    /// </summary>
    /// <remarks>
    /// The <see cref="XCheckListBox"/> control enables you to display a list of
    /// items to the user that the user can check by clicking.
    /// A <see cref="XCheckListBox"/> control can provide single or
    /// multiple selections using the <see cref="VirtualListControl.SelectionMode"/> property.
    /// The <see cref="AbstractControl.BeginUpdate"/> and <see cref="AbstractControl.EndUpdate"/>
    /// methods enable
    /// you to add a large number of items to the CheckListBox without the control
    /// being repainted each time an item is added to the list.
    /// The <see cref="VirtualListControl.SelectedItems"/>, <see cref="VirtualListControl.SelectedIndices"/>, and
    /// <see cref="VirtualListControl.CheckedIndices"/>
    /// properties provide access to the selected and checked items collections.
    /// </remarks>
    [ControlCategory(KnownControlCategory.Common)]
    public partial class XCheckListBox : XListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XCheckListBox"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public XCheckListBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCheckListBox"/> class.
        /// </summary>
        public XCheckListBox()
        {
            CheckBoxVisible = true;
        }
    }
}
