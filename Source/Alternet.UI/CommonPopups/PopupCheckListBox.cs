using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="CheckListBox"/> control.
    /// </summary>
    public class PopupCheckListBox : PopupListBox
    {
        /// <inheritdoc/>
        protected override Control CreateMainControl() => new CheckListBox();
    }
}
