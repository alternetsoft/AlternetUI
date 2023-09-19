using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="IEnumerable"/> interface so tree-like structures can be enumerated.
    /// </summary>
    public interface IEnumerableTree : IEnumerable
    {
        /// <summary>
        /// Enumerates children of the specified item.
        /// </summary>
        /// <param name="item">Item id.</param>
        /// <returns></returns>
        IEnumerable? GetChildren(object item);

        /// <summary>
        /// Gets data associated with the item.
        /// </summary>
        /// <param name="item">Item id.</param>
        object? GetData(object item);
    }
}
