using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{

    /// <summary>
    /// Contains static methods related to <see cref="IEnumerable"/>.
    /// </summary>
    public static class EnumerableUtils
    {
        /// <summary>
        /// Calls specified action for the each item of the
        /// <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="collection">Items to enumerate.</param>
        /// <param name="action">Action to call on each item.</param>
        /// <typeparam name="T">Type of the item.</typeparam>
        public static void ForEach<T>(IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action(item);
        }

        /// <summary>
        /// Enumerates root items of the <see cref="IEnumerableTree"/>.
        /// </summary>
        /// <typeparam name="T">Type of the result item.</typeparam>
        /// <param name="tree">Data Source.</param>
        /// <remarks>
        /// This method uses <see cref="IEnumerableTree.GetData"/> to get the result.
        /// </remarks>
        public static IEnumerable<T> GetItems<T>(IEnumerableTree tree)
        {
            foreach (var srcItem in tree)
            {
                if (tree.GetData(srcItem) is T data)
                    yield return data;
            }
        }

        /// <summary>
        /// Enumerates children items of the specified item.
        /// </summary>
        /// <typeparam name="T">Type of the result items.</typeparam>
        /// <param name="tree">Data source.</param>
        /// <param name="item">Item which children to return.</param>
        /// <remarks>
        /// This method calls <see cref="IEnumerableTree.GetChildren"/> and
        /// <see cref="IEnumerableTree.GetData"/> for results enumeration.
        /// </remarks>
        public static IEnumerable<T> GetChildren<T>(IEnumerableTree tree, object item)
        {
            var children = tree.GetChildren(item);
            if (children != null)
            {
                foreach (var srcItem in children)
                {
                    if (tree.GetData(srcItem) is T data)
                        yield return data;
                }
            }
        }

        /// <summary>
        /// Calls <paramref name="func"/> action for the each item of the
        /// <see cref="IEnumerableTree"/>.
        /// </summary>
        /// <param name="tree">Data source.</param>
        /// <param name="func">Action to call on each item.</param>
        /// <remarks>
        /// All items are enumerated recursively.
        /// </remarks>
        public static void ForEachItem(IEnumerableTree tree, Action<object> func)
        {
            void Fn(IEnumerable parent)
            {
                foreach (var item in parent)
                {
                    func(item);
                    var childs = tree.GetChildren(item);
                    if (childs != null)
                        Fn(childs);
                }
            }

            Fn(tree);
        }
    }
}
