using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PictureBox"/> control.
    /// </summary>
    public class PopupPictureBox : PopupWindow<PictureBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupPictureBox"/> class.
        /// </summary>
        public PopupPictureBox()
        {
            ShowOkButton = false;
        }
    }
}
