using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for events that include a string value as event data.
    /// </summary>
    /// <remarks>This class is a specialized implementation of <see cref="BaseEventArgs{T}"/> where the event
    /// data is of type <see cref="string"/>. It can be used to pass string-based information in event-driven
    /// programming scenarios.</remarks>
    public class StringEventArgs : BaseEventArgs<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringEventArgs"/> class with the specified string value.
        /// </summary>
        /// <param name="value">The string value associated with the event.</param>
        public StringEventArgs(string value)
            : base(value)
        {
        }
    }
}
