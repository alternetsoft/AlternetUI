using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    [Flags]
    public enum LogItemKindFlags
    {
        None = 0,

        Information = 1,

        Error = 2,

        Warning = 4,

        Other = 8,
    }
}
