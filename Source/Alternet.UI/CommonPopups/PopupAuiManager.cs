using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="PanelAuiManager"/> control.
    /// </summary>
    internal class PopupAuiManager : PopupWindow
    {
        /// <inheritdoc/>
        protected override Control CreateMainControl() => new PanelAuiManager();
    }
}
