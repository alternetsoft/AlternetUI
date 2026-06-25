using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a thread-safe counter that wraps around on overflow.
    /// </summary>
    public struct OverflowSafeCounter
    {
        private int value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverflowSafeCounter"/>
        /// struct with an optional starting value.
        /// </summary>
        /// <param name="start">The initial value of the counter. Optional. Defaults to 0.</param>
        public OverflowSafeCounter(int start = 0)
        {
            value = start;
        }

        /// <summary>
        /// Increments the counter in a thread-safe manner and returns the new value.
        /// </summary>
        /// <returns></returns>
        public int Increment()
        {
            int newValue = Interlocked.Increment(ref value);

            if (newValue > int.MaxValue)
            {
                // Should never happen, but guard anyway
                newValue = 0;
                Interlocked.Exchange(ref value, newValue);
            }
            else if (newValue == int.MaxValue)
            {
                // Wrap to 0 on next increment
                Interlocked.Exchange(ref value, 0);
                newValue = 0;
            }

            return newValue;
        }

        /// <summary>
        /// Gets the current value of the counter in a thread-safe manner.
        /// </summary>
        public int Value => Volatile.Read(ref value);

        /// <summary>
        /// Resets the counter to a specified value in a thread-safe manner.
        /// </summary>
        /// <param name="start">The value to reset the counter to. Optional. Defaults to 0.</param>
        public void Reset(int start = 0)
        {
            Interlocked.Exchange(ref value, start);
        }

        /// <summary>
        /// Returns a string representation of the current value of the counter.
        /// </summary>
        /// <returns>A string representation of the current value of the counter.</returns>
        public override string ToString() => Value.ToString();
    }
}
