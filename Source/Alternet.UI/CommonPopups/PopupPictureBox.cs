using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class PopupPictureBox : PopupWindow
    {
        /// <inheritdoc/>
        protected override Control CreateMainControl() => new PictureBox();
    }
}
