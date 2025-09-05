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
        public NotNullCollection(CollectionSecurityFlags securityFlags = CollectionSecurityFlags.None)
            : base(securityFlags | CollectionSecurityFlags.NoNull)
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
    }
}
