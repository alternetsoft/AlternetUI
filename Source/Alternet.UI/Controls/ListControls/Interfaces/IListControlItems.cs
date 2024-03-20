using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties to work with the items of
    /// <see cref="ListControl"/> descendants.
    /// </summary>
    public interface IListControlItems<T>
        : IList<T>, IEnumerable<T>, IEnumerable,
        INotifyPropertyChanged, INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when an item is inserted in the collection.
        /// </summary>
        event CollectionItemChangedHandler<T>? ItemInserted;

        /// <summary>
        /// Occurs when an item is removed from the collection.
        /// </summary>
        event CollectionItemChangedHandler<T>? ItemRemoved;

        /// <summary>
        /// Occurs when an item range addition is finished in the collection.
        /// </summary>
        event CollectionItemRangeChangedHandler<T>? ItemRangeAdditionFinished;

        /// <summary>
        /// Returns <see langword="true"/> if <see cref="AddRange"/> is being
        /// executed at the moment.
        /// </summary>
        bool RangeOpInProgress { get; }

        /// <inheritdoc cref="Collection{T}.AddRange"/>
        void AddRange(IEnumerable<T> collection);
    }
}