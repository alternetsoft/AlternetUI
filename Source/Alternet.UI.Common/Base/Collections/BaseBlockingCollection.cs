using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a thread-safe collection that provides blocking and bounding capabilities.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class BaseBlockingCollection<T> : BlockingCollection<T>
    {
        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            while (TryTake(out _))
            {
            }
        }
    }
}
