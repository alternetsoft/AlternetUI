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
            for (int i = list.Count - 1; i >= newCount; i--)
                list.RemoveAt(i);

            for (int i = list.Count; i < newCount; i++)
                list.Add(createItem());
        }
    }
}
