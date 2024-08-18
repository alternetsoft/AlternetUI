using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to work with flags and attributes.
    /// </summary>
    /// <remarks>
    /// Both flags and attributes share the same dictionary, so you can't have
    /// flag and attribute with the same name.
    /// </remarks>
    /// <typeparam name="TKey">Type of the attribute identifier.</typeparam>
    /// <typeparam name="TValue">Type of the attribute value.</typeparam>
    public interface IFlagsAndAttributes<TKey, TValue>
        : ICustomFlags<TKey>, ICustomAttributes<TKey, TValue>
    {
        /// <summary>
        /// Gets custom flags provider.
        /// </summary>
        ICustomFlags<TKey> Flags { get; }

        /// <summary>
        /// Gets custom attributes provider.
        /// </summary>
        ICustomAttributes<TKey, TValue> Attr { get; }
    }
}
