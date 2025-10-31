using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a thread-safe first in-first out (FIFO) collection for storing objects.
    /// Inherits from <see cref="ConcurrentQueue{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    public class BaseConcurrentQueue<T> : ConcurrentQueue<T>
    {
    }
}
