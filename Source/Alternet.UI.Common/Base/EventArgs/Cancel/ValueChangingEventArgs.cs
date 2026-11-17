using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for events that can be canceled and involve a change in value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value that is changing.</typeparam>
    public class ValueChangingEventArgs<TValue> : BaseCancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangingEventArgs{TValue}"/>
        /// class with the specified old and new values.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangingEventArgs(TValue oldValue, TValue newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets or sets the old value before the change.
        /// </summary>
        public TValue OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value after the change.
        /// </summary>
        public TValue NewValue { get; set; }
    }

    /// <summary>
    /// Provides data for events that can be canceled and involve a change in date value.
    /// </summary>
    public class DateChangingEventArgs : ValueChangingEventArgs<DateOnly>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateChangingEventArgs"/> class
        /// with the specified old and new date values.
        /// </summary>
        /// <param name="oldValue">The old date value.</param>
        /// <param name="newValue">The new date value.</param>
        public DateChangingEventArgs(DateOnly oldValue, DateOnly newValue)
            : base(oldValue, newValue)
        {
        }
    }

    /// <summary>
    /// Provides data for events that can be canceled and involve a change in date and time value.
    /// </summary>
    public class DateTimeChangingEventArgs : ValueChangingEventArgs<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeChangingEventArgs"/> class
        /// with the specified old and new date and time values.
        /// </summary>
        /// <param name="oldValue">The old date and time value.</param>
        /// <param name="newValue">The new date and time value.</param>
        public DateTimeChangingEventArgs(DateTime oldValue, DateTime newValue)
            : base(oldValue, newValue)
        {
        }
    }

    /// <summary>
    /// Provides data for events that can be canceled and involve a change in date and time value.
    /// </summary>
    public class NullableDateTimeChangingEventArgs : ValueChangingEventArgs<DateTime?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDateTimeChangingEventArgs"/> class
        /// with the specified old and new date and time values.
        /// </summary>
        /// <param name="oldValue">The old date and time value.</param>
        /// <param name="newValue">The new date and time value.</param>
        public NullableDateTimeChangingEventArgs(DateTime? oldValue, DateTime? newValue)
            : base(oldValue, newValue)
        {
        }
    }

    /// <summary>
    /// Provides data for events that can be canceled and involve a change in time value.
    /// </summary>
    public class TimeChangingEventArgs : ValueChangingEventArgs<TimeOnly>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeChangingEventArgs"/> class
        /// with the specified old and new time values.
        /// </summary>
        /// <param name="oldValue">The old time value.</param>
        /// <param name="newValue">The new time value.</param>
        public TimeChangingEventArgs(TimeOnly oldValue, TimeOnly newValue)
            : base(oldValue, newValue)
        {
        }
    }
}
