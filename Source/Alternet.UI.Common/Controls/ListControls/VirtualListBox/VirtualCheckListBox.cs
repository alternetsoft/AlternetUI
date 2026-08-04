using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="VirtualListBox"/> descendant which has visible checkboxes next to the items.
    /// Please use <see cref="ListControlItem"/> with this control.
    /// </summary>
    public partial class VirtualCheckListBox : VirtualListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualCheckListBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public VirtualCheckListBox(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualCheckListBox"/> class.
        /// </summary>
        public VirtualCheckListBox()
        {
            CheckBoxVisible = true;
        }
    }
}
