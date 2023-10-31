using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// The <see cref="PopupWindow"/> displays content in a separate window that floats
    /// over the current application window.
    /// </summary>
    public class PopupWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWindow"/> class.
        /// </summary>
        public PopupWindow()
            : base()
        {
            MakeAsPopup();
            Deactivated += Popup_Deactivated;
        }

        private void Popup_Deactivated(object? sender, EventArgs e)
        {
            (sender as Window)?.Hide();
        }
    }
}
