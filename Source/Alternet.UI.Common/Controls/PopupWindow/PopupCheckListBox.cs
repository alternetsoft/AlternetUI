using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="StdCheckListBox"/> control.
    /// </summary>
    public partial class PopupCheckListBox : PopupListBox<VirtualCheckListBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCheckListBox"/> class.
        /// </summary>
        public PopupCheckListBox()
        {
            Title = CommonStrings.Default.WindowTitleSelectItems;
            this.HideOnClick = false;
            this.HideOnDoubleClick = true;
        }
    }
}
