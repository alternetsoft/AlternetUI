using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IEnumerableTree<T> : IEnumerableTree, IEnumerable<T>
    {
        IEnumerable<T>? GetChildren(T item);
    }
}
