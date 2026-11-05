using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Exposes the enumerator, which supports a iteration over a collection with
    /// tree-like structure of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IEnumerableTree<T> : IEnumerableTree, IEnumerable<T>
    {
        /// <inheritdoc cref="IEnumerableTree.GetChildren"/>
        IEnumerable<T>? GetChildren(T item);
    }
}