using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allo to work with readonly flags and attributes.
    /// </summary>
    /// <remarks>
    /// Both flags and attributes share the same dictionary, so you can't have
    /// flag and attribute with the same name.
    /// </remarks>
    public interface IReadOnlyFlagsAndAttributes : IReadOnlyCustomFlags, IReadOnlyCustomAttributes
    {
        /// <summary>
        /// Gets custom flags provider.
        /// </summary>
        IReadOnlyCustomFlags Flags { get; }

        /// <summary>
        /// Gets custom attributes provider.
        /// </summary>
        IReadOnlyCustomAttributes Attr { get; }
    }
}
