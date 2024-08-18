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
    /// flag and attribute with the same identifier.
    /// </remarks>
    public interface IReadOnlyFlagsAndAttributes<TKey, TValue>
        : IReadOnlyFlags<TKey>, IReadOnlyAttributes<TKey, TValue>
    {
        /// <summary>
        /// Gets flags provider.
        /// </summary>
        IReadOnlyFlags<TKey> Flags { get; }

        /// <summary>
        /// Gets attributes provider.
        /// </summary>
        IReadOnlyAttributes<TKey, TValue> Attr { get; }
    }
}
