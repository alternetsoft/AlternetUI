using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to work with flags and attributes.
    /// Type of the identifier is string. Type of the value is object.
    /// </summary>
    /// <remarks>
    /// Both flags and attributes share the same dictionary, so you can't have
    /// flag and attribute with the same name.
    /// </remarks>
    public interface IFlagsAndAttributes : IFlagsAndAttributes<string, object>
    {
    }
}
