using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a repeat pattern for scheduling events or tasks.
    /// </summary>
    public enum ScheduleRepeatPattern
    {
        /// <summary>
        /// Represents a one-time event with no repetition.
        /// </summary>
        Once,

        /// <summary>
        /// Represents a daily repeat pattern.
        /// </summary>
        Daily,

        /// <summary>
        /// Represents a weekly repeat pattern.
        /// </summary>
        Weekly,

        /// <summary>
        /// Represents a monthly repeat pattern.
        /// </summary>
        Monthly,

        /// <summary>
        /// Represents a yearly repeat pattern.
        /// </summary>
        Yearly,
    }
}
