using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the flags and attributes used in the library.
    /// Type of the identifier is string. Type of the value is object.
    /// </summary>
    internal class IntFlagsAndAttributes
        : FlagsAndAttributes<int, object>, IIntFlagsAndAttributes,
        ICustomIntFlags, ICustomIntAttributes
    {
        /// <summary>
        /// Allocate identifier for the specified text string.
        /// </summary>
        /// <param name="name">Text string.</param>
        /// <returns></returns>
        public virtual int AllocIntIdentifier(string name)
        {
            return FlagsFactory.AllocIntIdentifier(name);
        }
    }
}
