using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Internal items container for list controls.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    public class ListControlItems<T>
        : Collection<T>, IListControlItems<T>, IList, ICollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItems{T}"/> class.
        /// </summary>
        public ListControlItems()
        {
            ThrowOnNullAdd = true;
        }
    }
}
