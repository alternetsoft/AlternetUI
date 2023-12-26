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
        /// Inserts <paramref name="prefix"/> in the beginning of the each string
        /// in <paramref name="items"/>.
        /// </summary>
        /// <param name="items">Strings.</param>
        /// <param name="prefix">This string is inserted in the beginning.</param>
        /// <returns></returns>
        public static IEnumerable<string> InsertPrefix(IEnumerable<string> items, string prefix)
        {
            List<string> result = [];
            foreach (var item in items)
                result.Add($"{prefix}{item}");
            return result;
        }

        /// <summary>
        /// Gets whether <paramref name="value"/> equals any item in the <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="value">Value to compare with <paramref name="items"/>.</param>
        /// <param name="items">Items to compare with <paramref name="value"/>.</param>
        /// <returns></returns>
        public static bool EqualsRange<T>(T value, IEnumerable<T> items)
        {
            if(value is null)
            {
                foreach (T item in items)
                {
                    if (item is null)
                        return true;
                }

                return false;
            }

            foreach (T item in items)
            {
                if (value.Equals(item))
                    return true;
            }

            return false;
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
        public static void ForEach(IEnumerableTree tree, Action<object> func)
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
