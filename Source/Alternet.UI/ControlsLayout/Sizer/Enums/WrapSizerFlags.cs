using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    [Flags]
    internal enum WrapSizerFlags
    {
        ExtendLastOnEachLine = 1,

        // don't leave spacers in the beginning of a new row
        RemoveLeadingSpaces = 2,

        Default = ExtendLastOnEachLine | RemoveLeadingSpaces,
    }
}