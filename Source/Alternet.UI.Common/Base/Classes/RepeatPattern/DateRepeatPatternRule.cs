using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a rule for a repeat pattern in scheduling events or tasks.
    /// </summary>
    public abstract partial class DateRepeatPatternRule : BaseObjectWithNotify
    {
        private DateOnly startDate;
        private DateOnly? endDate;
        private int occurrenceCount;

        /// <summary>
        /// Gets or sets the start date of the repeat pattern range. 
        /// </summary>
        public virtual DateOnly StartDate
        {
            get => startDate;
            set
            {
                SetProperty(ref startDate, value, OnStartDateChanged);
            }
        }

        /// <summary>
        /// Gets or sets the end date of the repeat pattern range.
        /// If not set, the repeat pattern is considered to have no end date.
        /// </summary>
        public virtual DateOnly? EndDate
        {
            get => endDate;
            set
            {
                SetProperty(ref endDate, value, OnEndDateChanged);
            }
        }

        /// <summary>
        /// Gets or sets the number of occurrences within the range.
        /// If 0, the repeat pattern is considered to have no limit on occurrences.
        /// </summary>
        public virtual int OccurrenceCount
        {
            get => occurrenceCount;
            set
            {
                SetProperty(ref occurrenceCount, value, OnOccurrenceCountChanged);
            }
        }

        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date.
        /// </summary>
        /// <param name="minDate">The minimum date to consider for the occurrences.</param>
        /// <param name="maxDate">The maximum date to consider for the occurrences.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        public virtual IEnumerable<DateOnly> GetDates(DateOnly minDate, DateOnly maxDate)
        {
            return Array.Empty<DateOnly>();
        }

        /// <summary>
        /// Called when the <see cref="EndDate"/> property changes.
        /// Override this method to implement custom behavior when the end date is updated.
        /// </summary>
        protected virtual void OnEndDateChanged()
        {
        }

        /// <summary>
        /// Called when the <see cref="StartDate"/> property changes.
        /// Override this method to implement custom behavior when the start date is updated.
        /// </summary>
        protected virtual void OnStartDateChanged()
        {
        }

        /// <summary>
        /// Called when the <see cref="OccurrenceCount"/> property changes.
        /// Override this method to implement custom behavior when the occurrence count is updated.
        /// </summary>
        protected virtual void OnOccurrenceCountChanged()
        {
        }

        /// <summary>
        /// Coerces the specified minimum and maximum dates based on the start and end dates of the repeat pattern.
        /// </summary>
        /// <param name="minDate">The minimum date to consider.</param>
        /// <param name="maxDate">The maximum date to consider.</param>
        protected virtual void CoerceMinMaxDate(ref DateOnly minDate, ref DateOnly maxDate)
        {
            if (EndDate is not null && EndDate < maxDate)
                maxDate = EndDate.Value;

            // Here we can add optimization for some cases and to use minDate as is.

            minDate = StartDate;
        }
    }
}
