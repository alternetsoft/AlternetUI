using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Internal items container for list controls.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    public class ListControlItems<T> : Collection<T>, IListControlItems<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItems{T}"/> class.
        /// </summary>
        public ListControlItems()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItems{T}"/> class
        /// class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public ListControlItems(IEnumerable<T> collection)
            : base(collection)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlItems{T}"/> class
        /// that contains elements copied from the specified list.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        public ListControlItems(List<T> list)
            : base(list)
        {
            Initialize();
        }

        /// <inheritdoc/>
        public IList AsList
        {
            get => this;
        }

        /// <summary>
        /// Common initialization method called from the constructors.
        /// </summary>
        protected virtual void Initialize()
        {
            ThrowOnNullAdd = true;
        }
    }
}
