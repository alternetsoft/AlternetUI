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
    public partial class PopupCheckListBox : PopupListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupCheckListBox"/> class.
        /// </summary>
        public PopupCheckListBox()
        {
            this.HideOnClick = false;
            this.HideOnDoubleClick = true;
        }

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
