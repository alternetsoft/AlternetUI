using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
#if NETSTANDARD2_0

    /// <summary>
    /// Represents an object that provides a custom type.
    /// </summary>
    public interface ICustomTypeProvider
    {
        /// <summary>
        /// Gets the custom type provided by this object.
        /// </summary>
        /// <returns>The custom type.</returns>
        Type GetCustomType();
    }
#endif
}
