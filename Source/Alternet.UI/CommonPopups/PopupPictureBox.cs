using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PictureBox"/> control.
    /// </summary>
    public class PopupPictureBox : PopupWindow
    {
        /// <summary>
        /// Gets or sets <see cref="PictureBox"/> control used in the popup window.
        /// </summary>
        public new PictureBox MainControl
        {
            get => (PictureBox)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl() => new PictureBox();
    }
}
