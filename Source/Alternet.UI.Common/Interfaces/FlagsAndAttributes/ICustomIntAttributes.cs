using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with attributes.
    /// Type of the identifier is integer. Type of the value is object.
    /// </summary>
    public interface ICustomIntAttributes : ICustomAttributes<int, object>
    {
        /// <summary>
        /// Allocate identifier for the specified text string.
        /// </summary>
        /// <param name="name">Text string.</param>
        /// <returns></returns>
        int AllocIntIdentifier(string name);
    }
}
