using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates flags used when value is set in the property grid control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
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
