using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface to work with a list of items.
    /// </summary>
    /// <typeparam name="TItem">Type of the items in the list.</typeparam>
    public interface IListSource<TItem> : IReadOnlyListSource<TItem>, IList<TItem>
    {
        /// <summary>
        /// Occurs when the list of items changes.
        /// </summary>
        event ListChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when an item is inserted.
        /// </summary>
        event CollectionItemChangedHandler<TItem>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed.
        /// </summary>
        event CollectionItemChangedHandler<TItem>? ItemRemoved;

        /// <summary>
        /// Gets the items as <see cref="IList"/>.
        /// </summary>
        IList AsList {  get; }

        /// <summary>
        /// Gets or sets the number of items in the list.
        /// </summary>
        /// <returns>
        /// The number of items in the list.
        /// </returns>
        new int Count { get; set;}

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get.</param>
        /// <returns>The item at the specified index.</returns>
        new TItem this[int index] { get; }

        /// <summary>
        /// Sets size of the list.
        /// </summary>
        /// <param name="count">The new size of the list.</param>
        /// <param name="fnCreateItem">A function to create new items if the list is expanded.</param>
        void SetCount(int count, Func<TItem> fnCreateItem);

        /// <summary>
        /// Sets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to set.</param>
        /// <param name="value">The item to set at the specified index.</param>
        void SetItem(int index, TItem value);

        /// <summary>
        /// Adds a range of items.
        /// </summary>
        /// <param name="items">The items to add.</param>
        void AddRange(IEnumerable<TItem> items);

        /// <summary>
        /// Sorts the items in the list.
        /// </summary>
        void Sort();

        /// <summary>
        /// Sorts the elements. Uses the provided comparer.
        /// </summary>
        void Sort(IComparer<TItem>? comparer);

        /// <summary>
        /// Sorts the elements in a section of this collection. The sort compares the
        /// elements to each other using the given IComparer interface. If
        /// comparer is null, the elements are compared to each other using
        /// the IComparable interface, which in that case must be implemented by all
        /// elements of the list.
        /// </summary>
        void Sort(int index, int count, IComparer<TItem>? comparer);

        /// <summary>
        /// Sorts the elements in the entire collection using the specified comparison.
        /// </summary>
        void Sort(Comparison<TItem> comparison);

        /// <summary>
        /// Sorts the items in descending order based on the specified comparison logic.
        /// </summary>
        /// <remarks>This method performs the sorting operation only if the collection contains items.
        /// The sorting is executed in descending order by reversing the result of the provided
        /// comparison logic.</remarks>
        /// <param name="comparison">A delegate that defines the comparison logic
        /// to apply to the items. The delegate
        /// should return a value less than zero if the first item is less than the second,
        /// zero if they are equal, or
        /// greater than zero if the first item is greater than the second.</param>
        void SortDescending(Comparison<TItem> comparison);
    }
}