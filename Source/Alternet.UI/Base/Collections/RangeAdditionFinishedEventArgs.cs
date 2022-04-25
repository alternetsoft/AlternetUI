using System;
using System.Collections.Generic;

namespace Alternet.Base.Collections
{
    /// <summary>
    /// Provides data for the <see cref="Collection{T}.ItemRangeAdditionFinished"/> event.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class RangeAdditionFinishedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAdditionFinishedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="insertionIndex">The index to insert the <see cref="Items"/> at.</param>
        /// <param name="items">The items being inserted.</param>
        public RangeAdditionFinishedEventArgs(int insertionIndex, IEnumerable<T> items)
        {
            InsertionIndex = insertionIndex;
            Items = items;
        }

        /// <summary>
        /// The index to insert the <see cref="Items"/> at.
        /// </summary>
        public int InsertionIndex { get; }

        /// <summary>
        /// The items being inserted.
        /// </summary>
        public IEnumerable<T> Items { get; }
    }
}