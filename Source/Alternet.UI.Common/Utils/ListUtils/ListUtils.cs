using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to arrays and lists.
    /// </summary>
    public static class ListUtils
    {
        /// <summary>  
        /// Finds the indices of the specified items in the given collection.  
        /// </summary>  
        /// <typeparam name="T">The type of elements in the collection and items.</typeparam>  
        /// <param name="collection">The collection to search.</param>  
        /// <param name="items">The items to find in the collection.</param>  
        /// <returns>An enumerable of indices where the items are found in the collection.</returns>  
        public static IEnumerable<int> IndexOfRange<T>(IList<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                int index = collection.IndexOf(item);
                if (index != -1)
                    yield return index;
            }
        }

        /// <summary>
        /// Changes the number of elements in the list.
        /// </summary>
        /// <param name="list">List with items.</param>
        /// <param name="newCount">New number of elements.</param>
        /// <param name="createItem">Function which creates new item.</param>
        /// <remarks>
        /// If collection has more items than specified in <paramref name="newCount"/>,
        /// these items are removed. If collection has less items, new items are created
        /// using <paramref name="createItem"/> function.
        /// </remarks>
        public static void SetCount<T>(IList<T> list, int newCount, Func<T> createItem)
        {
            if (newCount < 0)
                newCount = 0;

            for (int i = list.Count - 1; i >= newCount; i--)
                list.RemoveAt(i);

            for (int i = list.Count; i < newCount; i++)
                list.Add(createItem());
        }
    }
}
