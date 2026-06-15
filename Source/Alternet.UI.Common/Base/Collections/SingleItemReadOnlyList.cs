using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI;

/// <summary>
/// Represents a read-only list that contains a single item.
/// </summary>
public sealed class SingleItemReadOnlyList : IList
{
    private readonly object? item;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleItemReadOnlyList"/> class that contains a single item.
    /// </summary>
    /// <param name="item">The single item to be contained in the list.</param>
    public SingleItemReadOnlyList(object? item) => this.item = item;

    /// <inheritdoc/>
    public object? this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(index, 0);
            return item;
        }
        set => throw new NotSupportedException();
    }

    /// <inheritdoc/>
    public bool IsFixedSize => true;

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <inheritdoc/>
    public int Count => 1;

    /// <inheritdoc/>
    public bool IsSynchronized => false;

    /// <inheritdoc/>
    public object SyncRoot => this;

    /// <inheritdoc/>
    public IEnumerator GetEnumerator()
    {
        yield return item;
    }

    /// <inheritdoc/>
    public bool Contains(object? value) => item is null ? value is null : item.Equals(value);

    /// <inheritdoc/>
    public int IndexOf(object? value) => Contains(value) ? 0 : -1;

    /// <inheritdoc/>
    public void CopyTo(Array array, int index)
    {
        ListUtils.ValidateCopyToArguments(1, array, index);
        array.SetValue(item, index);
    }

    /// <inheritdoc/>
    public int Add(object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Clear() => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Insert(int index, object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void Remove(object? value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public void RemoveAt(int index) => throw new NotSupportedException();
}