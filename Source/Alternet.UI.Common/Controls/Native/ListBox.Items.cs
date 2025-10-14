using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class ListBox
    {
        private ObjectCollection? itemsCollection;

        /// <summary>
        /// Gets the items of the <see cref="ListBox" />.</summary>
        /// <returns>An <see cref="ObjectCollection" /> representing the items
        /// in the <see cref="ListBox" /> control.</returns>
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        [MergableProperty(false)]
        public ObjectCollection Items
        {
            get
            {
                if (itemsCollection == null)
                {
                    itemsCollection = CreateItemCollection();
                }

                return itemsCollection;
            }
        }

        /// <summary>
        /// Creates a new instance of the item collection.
        /// </summary>
        /// <returns>A <see cref="ObjectCollection" /> that represents the new item collection.</returns>
        protected virtual ObjectCollection CreateItemCollection()
        {
            return new BaseObjectCollection(this);
        }

        /// <summary>
        /// Represents a collection of objects that can be used with a <see cref="ListBox"/> control.
        /// </summary>
        /// <remarks>This collection provides functionality for managing objects in a
        /// <see cref="ListBox"/> control,  including adding, removing, and accessing items.
        /// It inherits from <see cref="ObjectCollection"/>  and overrides its members to provide
        /// specific behavior for object management.</remarks>
        public class BaseObjectCollection : ObjectCollection
        {
            private readonly BaseCollection<object> items = new(CollectionSecurityFlags.None);

            /// <summary>
            /// Initializes a new instance of the <see cref="BaseObjectCollection"/> class.
            /// </summary>
            /// <param name="owner"></param>
            public BaseObjectCollection(ListBox owner)
                : base(owner)
            {
                items.CollectionChanged += (s, e) =>
                {
                    ListUtils.RouteCollectionChange(
                        s,
                        e,
                        owner);
                };
            }

            /// <inheritdoc/>
            public override bool IsFixedSize
            {
                get => false;
            }

            /// <inheritdoc/>
            public override bool IsReadOnly
            {
                get => false;
            }

            /// <inheritdoc/>
            public override int Count
            {
                get => items.Count;
            }

            /// <inheritdoc/>
            public override bool IsSynchronized
            {
                get => false;
            }

            /// <inheritdoc/>
            public override object SyncRoot
            {
                get => this;
            }

            /// <inheritdoc/>
            public override object this[int index]
            {
                get => items[index];
                set => items[index] = value;
            }

            /// <inheritdoc/>
            public override int Add(object value)
            {
                items.Add(value);
                return items.Count - 1;
            }

            /// <inheritdoc/>
            public override void Clear()
            {
                items.Clear();
            }

            /// <inheritdoc/>
            public override bool Contains(object value)
            {
                return items.Contains(value);
            }

            /// <inheritdoc/>
            public override void CopyTo(Array array, int index)
            {
                (items as ICollection).CopyTo(array, index);
            }

            /// <inheritdoc/>
            public override IEnumerator GetEnumerator()
            {
                return items.GetEnumerator();
            }

            /// <inheritdoc/>
            public override int IndexOf(object value)
            {
                return items.IndexOf(value);
            }

            /// <inheritdoc/>
            public override void Insert(int index, object value)
            {
                items.Insert(index, value);
            }

            /// <inheritdoc/>
            public override void Remove(object value)
            {
                items.Remove(value);
            }

            /// <inheritdoc/>
            public override void RemoveAt(int index)
            {
                items.RemoveAt(index);
            }
        }

        /// <summary>
        /// Represents the abstract collection of items in a <see cref="ListBox" />.
        /// </summary>
        [ListBindable(false)]
        public abstract class ObjectCollection : IList, ICollection, IEnumerable
        {
            private readonly ListBox owner;

            /// <summary>
            /// Initializes a new instance of the <see cref="ObjectCollection"/> class.
            /// </summary>
            /// <param name="owner"></param>
            public ObjectCollection(ListBox owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// Gets the <see cref="ListBox"/> that owns this instance.
            /// </summary>
            public ListBox Owner => owner;

            /// <inheritdoc/>
            public abstract bool IsFixedSize { get; }

            /// <inheritdoc/>
            public abstract bool IsReadOnly { get; }

            /// <inheritdoc/>
            public abstract int Count { get; }

            /// <inheritdoc/>
            public abstract bool IsSynchronized { get; }

            /// <inheritdoc/>
            public abstract object SyncRoot { get; }

            /// <inheritdoc/>
            public abstract object this[int index] { get; set; }

            /// <inheritdoc/>
            public abstract int Add(object value);

            /// <inheritdoc/>
            public abstract void Clear();

            /// <inheritdoc/>
            public abstract bool Contains(object value);

            /// <inheritdoc/>
            public abstract void CopyTo(Array array, int index);

            /// <inheritdoc/>
            public abstract IEnumerator GetEnumerator();

            /// <inheritdoc/>
            public abstract int IndexOf(object value);

            /// <inheritdoc/>
            public abstract void Insert(int index, object value);

            /// <inheritdoc/>
            public abstract void Remove(object value);

            /// <inheritdoc/>
            public abstract void RemoveAt(int index);
        }
    }
}
