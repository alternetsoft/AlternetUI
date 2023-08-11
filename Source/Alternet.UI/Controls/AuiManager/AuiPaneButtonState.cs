using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    public enum AuiPaneButtonState
    {
        Normal = 0,

        Hover = 1 << 1,

        Pressed = 1 << 2,

        Disabled = 1 << 3,

        Hidden = 1 << 4,

        Checked = 1 << 5,
    }
}
