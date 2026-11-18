using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a time interval defined by a start <see cref="DateTime"/>
    /// and a <see cref="TimeSpan"/> duration.
    /// </summary>
    /// <param name="Start">The start time of the interval.</param>
    /// <param name="Duration">The duration of the interval.</param>
    public readonly record struct DateTimeInterval(
        DateTime Start,
        TimeSpan Duration)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeInterval"/> struct
        /// with the specified start time and a duration of zero.
        /// </summary>
        /// <param name="Start">The start time of the interval.</param>
        public DateTimeInterval(DateTime Start)
            : this(Start, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Gets the end time of the interval, calculated
        /// as the sum of <see cref="Start"/> and <see cref="Duration"/>.
        /// </summary>
        public DateTime End
        {
            get
            {
                return Start + Duration;
            }
        }

        /// <summary>
        /// Returns a string representation of the interval in the format "Start - End".
        /// </summary>
        /// <returns>A string representation of the interval.</returns>
        public override string ToString()
        {
            return $"{Start:G} - {End:G}";
        }
    }
}
