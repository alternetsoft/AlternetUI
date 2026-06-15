using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Alternet.UI;

/// <summary>
/// Represents a read-only wrapper to a list.
/// </summary>
public sealed class ReadOnlyListWrapper : IList
{
    private readonly IList list;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyListWrapper"/> class that wraps the specified list.
    /// </summary>
    /// <param name="list">The list to wrap.</param>
    internal ReadOnlyListWrapper(IList list)
    {
        Debug.Assert(list != null);
        this.list = list;
    }

    /// <inheritdoc/>
    public int Count => list.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <inheritdoc/>
    public bool IsFixedSize => true;

    /// <inheritdoc/>
    public bool IsSynchronized => list.IsSynchronized;

    /// <inheritdoc/>
    public object? this[int index]
    {
        get => list[index];
        set => throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public object SyncRoot => list.SyncRoot;

    /// <inheritdoc/>
    public int Add(object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Clear() => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool Contains(object? value) => list.Contains(value);

    /// <inheritdoc/>
    public void CopyTo(Array array, int index) => list.CopyTo(array, index);

    /// <inheritdoc/>
    public IEnumerator GetEnumerator() => list.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(object? value) => list.IndexOf(value);

    /// <inheritdoc/>
    public void Insert(int index, object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Remove(object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void RemoveAt(int index) => throw new NotSupportedException();
}
