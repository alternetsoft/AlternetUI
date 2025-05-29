using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a collection that does not allow null elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <remarks>
    /// This collection ensures that all added items are non-null, preventing
    /// accidental null references.
    /// </remarks>
    public class NotNullCollection<T> : BaseCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullCollection{T}"/> class.
        /// </summary>
        public NotNullCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullCollection{T}"/> class
        /// class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public NotNullCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the collection
        /// as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public NotNullCollection(List<T> list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public override bool ThrowOnNullAdd
        {
            get => true;
            set
            {
            }
        }
    }
}
