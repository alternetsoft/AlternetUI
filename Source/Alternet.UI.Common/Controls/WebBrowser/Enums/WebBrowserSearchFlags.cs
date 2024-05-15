using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    public enum WebBrowserSearchFlags
    {
        Default = 0,

        Wrap = 0x0001,

        EntireWord = 0x0002,

        MatchCase = 0x0004,

        HighlightResult = 0x0008,

        Backwards = 0x0010,
    }
}
