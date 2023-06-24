using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    internal enum AnchorStyles
    {
        None = 0x00000000,
        Top = 0x00000001,
        Bottom = 0x00000002,
        Left = 0x00000004,
        Right = 0x00000008,
    }
}
