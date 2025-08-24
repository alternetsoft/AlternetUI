using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for events that report a value change.
    /// </summary>
    /// <typeparam name="T">The type of the value that changed.</typeparam>
#pragma warning disable
    public class ValueChangedEventArgs<T> : BaseEventArgs
#pragma warning restore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets the previous value before the change occurred.
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// Gets the new value after the change occurred.
        /// </summary>
        public T NewValue { get; }
    }

    /// <summary>
    /// Provides data for an event that signals a change in value.
    /// </summary>
    /// <remarks>This class represents a non-generic version
    /// of <see cref="ValueChangedEventArgs{T}"/> and is
    /// used to encapsulate the old and new values of an object when a value change occurs.</remarks>
#pragma warning disable
    public class ValueChangedEventArgs : ValueChangedEventArgs<object?>
#pragma warning restore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(object? oldValue, object? newValue)
            : base(oldValue, newValue)
        {
        }
    }
}
