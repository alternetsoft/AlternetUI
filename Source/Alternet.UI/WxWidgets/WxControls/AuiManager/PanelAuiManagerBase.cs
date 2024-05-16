using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Panel with integrated <see cref="AuiManager"/> which allows to implement
    /// advanced docking and floating toolbars and panes.
    /// </summary>
    /// <remarks>
    /// <see cref="PanelAuiManagerBase"/> is a base panel with <see cref="AuiManager"/>.
    /// If you need advanced panel with different built-in panes and toolbars, use
    /// <see cref="PanelAuiManager"/>.
    /// </remarks>
    [ControlCategory("Hidden")]
    public partial class PanelAuiManagerBase : LayoutPanel
    {
        private readonly AuiManager manager = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelAuiManagerBase"/> class.
        /// </summary>
        public PanelAuiManagerBase()
        {
            var flags = DefaultManagerOptions;
            if(flags != AuiManagerOption.Default)
                Manager.SetFlags(flags);
            Manager.SetManagedWindow(this);
        }

        /// <summary>
        /// Gets docking manager.
        /// </summary>
        [Browsable(false)]
        internal AuiManager Manager => manager;

        /// <summary>
        /// Override to provide custom options to the <see cref="Manager"/>.
        /// </summary>
        internal virtual AuiManagerOption DefaultManagerOptions => AuiManagerOption.Default;
    }
}