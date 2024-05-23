using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="VListBox"/> descendant which has visible checkboxes next to the items.
    /// Please use <see cref="ListControlItem"/> with this control.
    /// </summary>
    public class VCheckListBox : VListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VCheckListBox"/> class.
        /// </summary>
        public VCheckListBox()
        {
            CheckBoxVisible = true;
        }
    }
}
