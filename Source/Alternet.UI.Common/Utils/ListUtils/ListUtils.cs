using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to lists and collections.
    /// </summary>
    public static class ListUtils
    {
        /// <summary>
        /// Moves the specified item to the front of the list, if it exists.
        /// </summary>
        public static void MoveItemToFront<T>(List<T> list, T item)
        {
            if (list == null || item == null) return;

            int index = list.IndexOf(item);
            if (index > 0)
            {
                list.RemoveAt(index);
                list.Insert(0, item);
            }
        }

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

        /// <summary>
        /// Routes a collection change event to the appropriate handler
        /// based on the action type.
        /// </summary>
        /// <remarks>This method processes the collection change event by invoking
        /// the corresponding method on the provided <paramref name="router"/> based on
        /// the <see cref="NotifyCollectionChangedAction"/> specified in <paramref name="e"/>.
        /// Supported actions include Add, Remove, Replace, Move, and Reset.</remarks>
        /// <param name="sender">The source of the collection change event.
        /// This may be <see langword="null"/>.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance
        /// containing details about the collection change.</param>
        /// <param name="router">The <see cref="ICollectionChangeRouter"/> responsible
        /// for handling the collection change actions.</param>
        /// <returns>
        /// <see langword="true"/> if the collection change action was successfully routed and handled;
        /// <see langword="false"/> if the action type is not supported.
        /// </returns>
        public static bool RouteCollectionChange(
                object? sender,
                NotifyCollectionChangedEventArgs e,
                ICollectionChangeRouter router)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    router.OnCollectionAdd(sender, e.NewItems!, e.NewStartingIndex);
                    return true;

                case NotifyCollectionChangedAction.Remove:
                    router.OnCollectionRemove(sender, e.OldItems!, e.OldStartingIndex);
                    return true;

                case NotifyCollectionChangedAction.Replace:
                    router.OnCollectionReplace(sender, e.OldItems!, e.NewItems!, e.OldStartingIndex);
                    return true;

                case NotifyCollectionChangedAction.Move:
                    router.OnCollectionMove(
                        sender,
                        e.OldItems!,
                        e.OldStartingIndex,
                        e.NewStartingIndex);
                    return true;

                case NotifyCollectionChangedAction.Reset:
                    router.OnCollectionReset(sender);
                    return true;

                default:
                    return false;
            }
        }
    }
}


/*
        protected virtual void OnMenuItemsCollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            if(sender is not IMenuProperties menuProperties)
                return;

            void InsertItem(int index, IMenuItemProperties menuItem)
            {
                if(menuItem.Text == "-")
                {
                    var separator = AddSeparatorCore();
                    separator.DataContext = menuItem;
                    return;
                }
                else
                {
                }
            }

            void ResetItems()
            {
                DeleteAll(true);

                DoInsideLayout(() =>
                {
                    for (int i = 0; i < menuProperties.ItemCount; i++)
                    {
                        var menuItem = menuProperties.GetItem(i);
                        if (menuItem is not IMenuItemProperties item)
                            continue;
                        InsertItem(i, item);
                    }
                });
            }

            void RemoveItems(IList items)
            {
                foreach (var oldItem in items)
                {
                    if (oldItem is not IMenuItemProperties menuItem)
                        continue;
                    var child = FindChildWithDataContextId(menuItem.UniqueId);
                    if (child is null)
                        continue;
                    child.Parent = null;
                    child.Dispose();
                }
            }

            void MoveItems(IList items, int oldIndex, int newIndex)
            {
                if (oldIndex == newIndex)
                    return;
                RemoveItems(items);
                InsertItems(items, newIndex);
            }

            void InsertItems(IList items, int index)
            {
                foreach (var newItem in items)
                {
                    if (newItem is not IMenuItemProperties menuItem)
                        continue;
                    InsertItem(index, menuItem);
                    index++;
                }
            }

            void ReplaceItems(IList fromItems, IList toItems, int index)
            {
                RemoveItems(fromItems);
                InsertItems(toItems, index);
            }

            DoInsideLayout(Internal);

            void Internal()
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        InsertItems(e.NewItems!, e.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        RemoveItems(e.OldItems!);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        ReplaceItems(e.OldItems!, e.NewItems!, e.OldStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        MoveItems(e.OldItems!, e.OldStartingIndex, e.NewStartingIndex);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        ResetItems();
                        break;
                }
            }
        }

*/