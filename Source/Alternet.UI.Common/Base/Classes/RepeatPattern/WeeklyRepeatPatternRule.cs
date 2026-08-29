using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a weekly repeat pattern rule.
    /// </summary>
    public partial class WeeklyRepeatPatternRule : DateRepeatPatternRule
    {
        private int intervalWeeks = 1;
        private DaysOfWeek days;

        /// <summary>
        /// Gets or sets the number of weeks between occurrences.
        /// </summary>
        public virtual int IntervalWeeks
        {
            get => intervalWeeks;
            set
            {
                if (value < 1)
                    value = 1;
                intervalWeeks = GetNewFieldValue(intervalWeeks, value);
            }
        }

        /// <summary>
        /// Gets or sets the days of the week on which the event occurs.
        /// </summary>
        public virtual DaysOfWeek WeekDays
        {
            get => days;
            set
            {
                days = GetNewFieldValue(days, value);
            }
        }
    }
}
