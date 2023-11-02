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
    public class PopupAuiManager : PopupWindow
    {
        /// <summary>
        /// Gets or sets <see cref="PanelAuiManager"/> control used in the popup window.
        /// </summary>
        public new PanelAuiManager MainControl
        {
            get => (PanelAuiManager)base.MainControl;
            set => base.MainControl = value;
        }

        /// <inheritdoc/>
        protected override Control CreateMainControl() => new PanelAuiManager();
    }
}
