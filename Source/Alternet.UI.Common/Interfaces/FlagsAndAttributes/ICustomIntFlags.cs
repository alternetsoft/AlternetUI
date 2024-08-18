using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with flags specified using integer identifiers.
    /// </summary>
    public interface ICustomIntFlags : ICustomFlags<int>
    {
        /// <summary>
        /// Allocate identifier for the specified text string.
        /// </summary>
        /// <param name="name">Text string.</param>
        /// <returns></returns>
        int AllocIntIdentifier(string name);
    }
}
