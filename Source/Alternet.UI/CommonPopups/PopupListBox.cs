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
                if(listBox == null)
                {
                    listBox = new()
                    {
                        HasBorder = false,
                        Parent = this.Border,
                    };
                    BindEvents(listBox);
                }

                return listBox;
            }

            set
            {
                if (listBox == value || listBox is null)
                    return;
                UnbindEvents(listBox);
                listBox = value;
                BindEvents(listBox);
                listBox.Parent = Border;
            }
        }

        private void BindEvents(ListBox control)
        {
            control.MouseDoubleClick += PopupControl_MouseDoubleClick;
        }

        private void UnbindEvents(ListBox control)
        {
            control.MouseDoubleClick -= PopupControl_MouseDoubleClick;
        }
    }
}
