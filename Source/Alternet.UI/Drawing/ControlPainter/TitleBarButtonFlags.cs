using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    internal enum TitleBarButtonFlags
    {
        Close = 0x01000000,
        Maximize = 0x02000000,
        Iconize = 0x04000000,
        Restore = 0x08000000,
        Help = 0x10000000,
    }
}