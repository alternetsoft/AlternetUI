using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with attributes.
    /// Type of the identifier is string. Type of the value is object.
    /// </summary>
    public interface ICustomAttributes : ICustomAttributes<string, object>
    {
    }
}
