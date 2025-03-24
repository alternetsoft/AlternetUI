using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates possible net framework identifiers.
    /// </summary>
    public enum NetFrameworkIdentifier
    {
        /// <summary>
        /// .NET 5 and later versions.
        /// </summary>
        Net,

        /// <summary>
        /// .NET Core 1.0-3.1.
        /// </summary>
        NetCore,

        /// <summary>
        /// .NET Framework.
        /// </summary>
        NetFramework,

        /// <summary>
        /// .NET Native.
        /// </summary>
        NetNative,

        /// <summary>
        /// Other.
        /// </summary>
        Other,
    }
}