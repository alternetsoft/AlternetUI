using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class ListControlItems<T>
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
