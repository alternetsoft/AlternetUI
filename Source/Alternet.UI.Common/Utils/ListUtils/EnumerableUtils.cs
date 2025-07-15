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
        /// Gets the count of elements in an IEnumerable.
        /// Uses ICollection for efficiency when available.
        /// </summary>
        /// <param name="enumerable">The enumerable collection.</param>
        /// <returns>The number of elements.</returns>
        public static int GetCount(IEnumerable enumerable)
        {
            if (enumerable is ICollection collection)
                return collection.Count;

            int count = 0;
            foreach (var item in enumerable)
                count++;

            return count;
        }

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
        /// Converts items from the <see cref="IEnumerable{T}"/> specified in the
        /// <paramref name="source"/> parameter to the destination by calling <paramref name="addToDest"/>
        /// function. Item type is converted using <paramref name="convertItem"/> function.
        /// </summary>
        /// <typeparam name="TSource">Type of the item in the <paramref name="source"/>.</typeparam>
        /// <typeparam name="TDest">Type of the destination item.</typeparam>
        /// <param name="convertItem">The function which converts item from the
        /// <typeparamref name="TSource"/> type to the <typeparamref name="TDest"/> type.
        /// Return Null to ignore the item.</param>
        /// <param name="addToDest">The function which is used to add items to the destination.</param>
        /// <param name="source">Source of the items for the conversion.</param>
        /// <param name="bufferSize">Size of the buffer for the items. Optional. Default is 10.</param>
        public static void ConvertItems<TSource, TDest>(
            Func<TSource, TDest?> convertItem,
            Func<IEnumerable<TDest>, bool> addToDest,
            IEnumerable<TSource> source,
            int bufferSize = 10)
        {
            List<TDest> items = new(bufferSize);

            foreach (var sourceItem in source)
            {
                var item = convertItem(sourceItem);
                if (item is null)
                    continue;
                items.Add(item);

                if (items.Count == bufferSize)
                {
                    if (!addToDest(items))
                        return;
                    items.Clear();
                }
            }

            addToDest(items);
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
        /// Removes the specified prefix from each string in the sequence,
        /// using the given comparison rule.
        /// </summary>
        /// <param name="items">The collection of strings to process.</param>
        /// <param name="prefix">The prefix to remove from each string.</param>
        /// <param name="comparison">
        /// The <see cref="StringComparison"/> option to use when matching the prefix.
        /// For example, use <see cref="StringComparison.OrdinalIgnoreCase"/> for
        /// case-insensitive matching.
        /// </param>
        /// <returns>
        /// A sequence of strings with the prefix removed where applicable.
        /// If an item does not start with the prefix (based on the specified comparison),
        /// it is returned unchanged.
        /// </returns>
        public static IEnumerable<string?> RemovePrefix(
            IEnumerable<string?> items,
            string prefix,
            StringComparison comparison = StringComparison.CurrentCulture)
        {
            foreach (var item in items)
            {
                if (item is null)
                    yield return null;

                if (item!.StartsWith(prefix, comparison))
                    yield return item.Substring(prefix.Length);
                else
                    yield return item;
            }
        }

        /// <summary>
        /// Inserts <paramref name="prefix"/> in the beginning of the each string
        /// in <paramref name="items"/>.
        /// </summary>
        /// <param name="items">Strings.</param>
        /// <param name="prefix">This string is inserted in the beginning.</param>
        /// <returns></returns>
        public static IEnumerable<string?> InsertPrefix(IEnumerable<string?> items, string? prefix)
        {
            foreach (var item in items)
                yield return $"{prefix}{item}";
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
                    var children = tree.GetChildren(item);
                    if (children != null)
                        Fn(children);
                }
            }

            Fn(tree);
        }
    }
}
