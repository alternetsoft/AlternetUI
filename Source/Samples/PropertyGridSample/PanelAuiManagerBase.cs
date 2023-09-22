using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    public class PanelAuiManagerBase : LayoutPanel
    {
        private readonly AuiManager manager = new();

        public AuiManager Manager => manager;

        protected virtual AuiManagerOption DefaultManagerOptions => AuiManagerOption.Default;

        public PanelAuiManagerBase()
        {
            Manager.SetFlags(DefaultManagerOptions);
            Manager.SetManagedWindow(this);
        }
    }
}
