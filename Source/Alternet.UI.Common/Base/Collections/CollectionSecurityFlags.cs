using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies security-related flags for collection operations.
    /// </summary>
    [Flags]
    public enum CollectionSecurityFlags
    {
        /// <summary>
        /// No security flags are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Prevents replacing items in the collection.
        /// </summary>
        NoReplace = 1 << 0,

        /// <summary>
        /// Prevents adding <c>null</c> items to the collection.
        /// </summary>
        NoNull = 1 << 1,

        /// <summary>
        /// Specifies a combination of flags that enforce non-null values
        /// and prevent replacement of existing values.
        /// </summary>
        /// <remarks>This enumeration value is a bitwise combination of the <see cref="NoNull"/>
        /// and <see cref="NoReplace"/> flags.
        /// It is used to ensure that values are neither null nor replaced during
        /// operations.</remarks>
        NoNullOrReplace = NoNull | NoReplace,
    }
}
