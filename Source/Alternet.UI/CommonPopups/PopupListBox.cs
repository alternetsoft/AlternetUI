using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="ListBox"/> control.
    /// </summary>
    public class PopupListBox : PopupWindow
    {
        private ListBox? listBox;

        /// <summary>
        /// Gets or sets <see cref="ListBox"/> control used in the popup window.
        /// </summary>
        public ListBox ListBox
        {
            get
            {
                listBox ??= new()
                {
                    HasBorder = false,
                    Parent = this.Border,
                };

                return listBox;
            }

            set
            {
                if (listBox == value || listBox is null)
                    return;
                listBox = value;
                listBox.Parent = Border;
            }
        }
    }
}
