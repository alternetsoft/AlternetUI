using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a thread-safe, last-in-first-out (LIFO) collection of objects.
    /// This class extends <see cref="ConcurrentStack{T}"/> to provide additional functionality or customization.
    /// </summary>
    /// <typeparam name="T">The type of elements in the stack.</typeparam>
    public class BaseConcurrentStack<T> : ConcurrentStack<T>
    {
    }
}
