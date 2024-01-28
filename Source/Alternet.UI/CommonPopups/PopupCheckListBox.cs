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
    internal class PopupCheckListBox : PopupListBox
    {
        /// <summary>
        /// Gets or sets <see cref="CheckListBox"/> control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public new CheckListBox MainControl
        {
            get => (CheckListBox)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl()
        {
            return new CheckListBox()
            {
                HasBorder = false,
            };
        }
    }
}
