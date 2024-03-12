using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="CheckListBox"/> control.
    /// </summary>
    public partial class PopupCheckListBox : PopupListBox<CheckListBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCheckListBox"/> class.
        /// </summary>
        public PopupCheckListBox()
        {
            this.HideOnClick = false;
            this.HideOnDoubleClick = true;
        }
    }
}
