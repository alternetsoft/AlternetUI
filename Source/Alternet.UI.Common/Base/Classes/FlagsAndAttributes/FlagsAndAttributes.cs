using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the flags and attributes used in the library.
    /// Type of the identifier is string. Type of the value is object.
    /// </summary>
    internal class FlagsAndAttributes
        : FlagsAndAttributes<string, object>, IFlagsAndAttributes,
        ICustomFlags, ICustomAttributes
    {
    }
}
