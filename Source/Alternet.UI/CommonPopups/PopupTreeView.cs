using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="TreeView"/> control.
    /// </summary>
    internal class PopupTreeView : PopupWindow
    {
        /// <inheritdoc/>
        protected override Control CreateMainControl() => new TreeView();
    }
}
