using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    internal enum PropertyGridSetValueFlags
    {
        RefreshEditor = 0x0001,

        Aggregated = 0x0002,

        FromParent = 0x0004,

        // Set if value changed by user
        ByUser = 0x0008,
    }
}
