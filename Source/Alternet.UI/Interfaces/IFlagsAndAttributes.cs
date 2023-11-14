using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which work with flags and attributes.
    /// </summary>
    /// <remarks>
    /// Both flags and attributes share the same dictionary, so you can't have
    /// flag and attribute with the same name.
    /// </remarks>
    public interface IFlagsAndAttributes : ICustomFlags, ICustomAttributes
    {
    }
}
