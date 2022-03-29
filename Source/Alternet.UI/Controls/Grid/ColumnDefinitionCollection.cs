#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;

namespace Alternet.UI
{
	/// <summary>Provides access to an ordered, strongly typed collection of <see cref="ColumnDefinition" /> objects.</summary>
	public sealed class ColumnDefinitionCollection : IList<ColumnDefinition>, ICollection<ColumnDefinition>, IEnumerable<ColumnDefinition>, IEnumerable, IList, ICollection
	{
		internal ColumnDefinitionCollection(Grid owner)
		{
			this._owner = owner;
			this.PrivateOnModified();
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items from the <see cref="ColumnDefinitionCollection" />.</param>
		/// <param name="index">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException();
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException();
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		/// <summary>Copies an array of <see cref="ColumnDefinition" /> objects to a given index position within a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="array">An array of <see cref="ColumnDefinition" /> objects.</param>
		/// <param name="index">Identifies the index position within <paramref name="array" /> to which the <see cref="ColumnDefinition" /> objects are copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination array. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero. </exception>
		public void CopyTo(ColumnDefinition[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException();
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="ColumnDefinitionCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		int IList.Add(object value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value as ColumnDefinition);
			return this._size - 1;
		}

		/// <summary>Adds a <see cref="ColumnDefinition" /> element to a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="ColumnDefinition" /> to add to the collection.</param>
		public void Add(ColumnDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value);
		}

		/// <summary>Clears the content of the <see cref="ColumnDefinitionCollection" />.</summary>
		public void Clear()
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateOnModified();
			for (int i = 0; i < this._size; i++)
			{
				this.PrivateDisconnectChild(this._items[i]);
				this._items[i] = null;
			}
			this._size = 0;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="ColumnDefinitionCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="ColumnDefinitionCollection" />; otherwise, <see langword="false" />.</returns>
		bool IList.Contains(object value)
		{
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			return columnDefinition != null && columnDefinition.Parent == this._owner;
		}

		/// <summary>Determines whether a given <see cref="ColumnDefinition" /> exists within a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="ColumnDefinition" /> that is being tested.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="ColumnDefinition" /> exists within the collection; otherwise <see langword="false" />.</returns>
		public bool Contains(ColumnDefinition value)
		{
			return value != null && value.Parent == this._owner;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="ColumnDefinitionCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ColumnDefinition);
		}

		/// <summary>Returns the index position of a given <see cref="ColumnDefinition" /> within a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="ColumnDefinition" /> whose index position is desired.</param>
		/// <returns>The index of <paramref name="value" /> if found in the collection; otherwise, -1.</returns>
		public int IndexOf(ColumnDefinition value)
		{
			if (value == null || value.Parent != this._owner)
			{
				return -1;
			}
			return value.Index;
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="ColumnDefinitionCollection" />.</param>
		void IList.Insert(int index, object value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value as ColumnDefinition);
		}

		/// <summary>Inserts a <see cref="ColumnDefinition" /> at the specified index position within a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="index">The position within the collection where the item is inserted.</param>
		/// <param name="value">The <see cref="ColumnDefinition" /> to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
		public void Insert(int index, ColumnDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value);
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="ColumnDefinitionCollection" />.</param>
		void IList.Remove(object value)
		{
			this.PrivateVerifyWriteAccess();
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value as ColumnDefinition);
			}
		}

		/// <summary>Removes a <see cref="ColumnDefinition" /> from a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="ColumnDefinition" /> to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="ColumnDefinition" /> was found in the collection and removed; otherwise, <see langword="false" />.</returns>
		public bool Remove(ColumnDefinition value)
		{
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value);
			}
			return flag;
		}

		/// <summary>Removes a <see cref="ColumnDefinition" /> from a <see cref="ColumnDefinitionCollection" /> at the specified index position.</summary>
		/// <param name="index">The position within the collection at which the <see cref="ColumnDefinition" /> is removed.</param>
		public void RemoveAt(int index)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.PrivateRemove(this._items[index]);
		}

		/// <summary>Removes a range of <see cref="ColumnDefinition" /> objects from a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="index">The position within the collection at which the first <see cref="ColumnDefinition" /> is removed.</param>
		/// <param name="count">The total number of <see cref="ColumnDefinition" /> objects to remove from the collection.</param>
		public void RemoveRange(int index, int count)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (this._size - index < count)
			{
				throw new ArgumentException();
			}
			this.PrivateOnModified();
			if (count > 0)
			{
				for (int i = index + count - 1; i >= index; i--)
				{
					this.PrivateDisconnectChild(this._items[i]);
				}
				this._size -= count;
				for (int j = index; j < this._size; j++)
				{
					this._items[j] = this._items[j + count];
					this._items[j].Index = j;
					this._items[j + count] = null;
				}
			}
		}

		/// <summary>For a description of this member, see <see cref="M:System.Collections.IEnumerable.GetEnumerator" />.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ColumnDefinitionCollection.Enumerator(this);
		}

		IEnumerator<ColumnDefinition> IEnumerable<ColumnDefinition>.GetEnumerator()
		{
			return new ColumnDefinitionCollection.Enumerator(this);
		}

		/// <summary>Gets the total number of items within this instance of <see cref="ColumnDefinitionCollection" />.</summary>
		/// <returns>The total number of items in the collection. This property has no default value.</returns>
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="ColumnDefinitionCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		bool IList.IsFixedSize
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether a <see cref="ColumnDefinitionCollection" /> is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise <see langword="false" />. This property has no default value.</returns>
		public bool IsReadOnly
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether access to this <see cref="ColumnDefinitionCollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="true" /> if access to this collection is synchronized; otherwise, <see langword="false" />.</returns>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="ColumnDefinitionCollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="ColumnDefinitionCollection" />.</returns>
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the list.</exception>
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException();
				}
				return this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value as ColumnDefinition);
			}
		}

		/// <summary>Gets a value that indicates the current item within a <see cref="ColumnDefinitionCollection" />.</summary>
		/// <param name="index">The current item in the collection.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the collection.</exception>
		public ColumnDefinition this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException();
				}
				return (ColumnDefinition)this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value);
			}
		}

		internal void InternalTrimToSize()
		{
			this.PrivateSetCapacity(this._size);
		}

		internal int InternalCount
		{
			get
			{
				return this._size;
			}
		}

		internal DefinitionBase[] InternalItems
		{
			get
			{
				return this._items;
			}
		}

		private void PrivateVerifyWriteAccess()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException();
			}
		}

		private void PrivateValidateValueForAddition(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			if (columnDefinition == null)
			{
				throw new ArgumentException();
			}
			if (columnDefinition.Parent != null)
			{
				throw new ArgumentException();
			}
		}

		private bool PrivateValidateValueForRemoval(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ColumnDefinition columnDefinition = value as ColumnDefinition;
			if (columnDefinition == null)
			{
				throw new ArgumentException();
			}
			return columnDefinition.Parent == this._owner;
		}

		private void PrivateConnectChild(int index, DefinitionBase value)
		{
			this._items[index] = value;
			value.Index = index;
			value.Parent = _owner;
			//this._owner.AddLogicalChild(value); //yezo
			//value.OnEnterParentTree();
		}

		private void PrivateDisconnectChild(DefinitionBase value)
		{
			value.OnExitParentTree();
			this._items[value.Index] = null;
			value.Index = -1;
			value.Parent = null;
			//this._owner.RemoveLogicalChild(value); yezo
		}

		private void PrivateInsert(int index, DefinitionBase value)
		{
			this.PrivateOnModified();
			if (this._items == null)
			{
				this.PrivateSetCapacity(4);
			}
			else if (this._size == this._items.Length)
			{
				this.PrivateSetCapacity(Math.Max(this._items.Length * 2, 4));
			}
			for (int i = this._size - 1; i >= index; i--)
			{
				this._items[i + 1] = this._items[i];
				this._items[i].Index = i + 1;
			}
			this._items[index] = null;
			this._size++;
			this.PrivateConnectChild(index, value);
		}

		private void PrivateRemove(DefinitionBase value)
		{
			this.PrivateOnModified();
			int index = value.Index;
			this.PrivateDisconnectChild(value);
			this._size--;
			for (int i = index; i < this._size; i++)
			{
				this._items[i] = this._items[i + 1];
				this._items[i].Index = i;
			}
			this._items[this._size] = null;
		}

		private void PrivateOnModified()
		{
			this._version++;
			this._owner.ColumnDefinitionCollectionDirty = true;
			this._owner.InvalidateCells();
		}

		private void PrivateSetCapacity(int value)
		{
			if (value <= 0)
			{
				this._items = null;
				return;
			}
			if (this._items == null || value != this._items.Length)
			{
				ColumnDefinition[] array = new ColumnDefinition[value];
				if (this._size > 0)
				{
					Array.Copy(this._items, 0, array, 0, this._size);
				}
				this._items = array;
			}
		}

		private readonly Grid _owner;

		private DefinitionBase[] _items;

		private int _size;

		private int _version;

		private const int c_defaultCapacity = 4;

		internal struct Enumerator : IEnumerator<ColumnDefinition>, IDisposable, IEnumerator
		{
			internal Enumerator(ColumnDefinitionCollection collection)
			{
				this._collection = collection;
				this._index = -1;
				this._version = ((this._collection != null) ? this._collection._version : -1);
				this._currentElement = collection;
			}

			public bool MoveNext()
			{
				if (this._collection == null)
				{
					return false;
				}
				this.PrivateValidate();
				if (this._index < this._collection._size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection._size;
				return false;
			}

			object IEnumerator.Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException();
					}
					throw new InvalidOperationException();
				}
			}

			public ColumnDefinition Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (ColumnDefinition)this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException();
					}
					throw new InvalidOperationException();
				}
			}

			public void Reset()
			{
				if (this._collection == null)
				{
					return;
				}
				this.PrivateValidate();
				this._currentElement = this._collection;
				this._index = -1;
			}

			public void Dispose()
			{
				this._currentElement = null;
			}

			private void PrivateValidate()
			{
				if (this._currentElement == null)
				{
					throw new InvalidOperationException();
				}
				if (this._version != this._collection._version)
				{
					throw new InvalidOperationException();
				}
			}

			private ColumnDefinitionCollection _collection;

			private int _index;

			private int _version;

			private object _currentElement;
		}
	}
}