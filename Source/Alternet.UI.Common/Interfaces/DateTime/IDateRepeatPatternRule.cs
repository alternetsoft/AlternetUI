using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a delegate for getting unfiltered dates based on a repeat pattern rule.
    /// </summary>
    /// <param name="prm">The parameters specifying the minimum and maximum dates to consider for the occurrences.</param>
    /// <returns>An enumerable collection of unfiltered dates within the specified range.</returns>
    public delegate PatternRuleGetDatesResult GetDatesDelegate(PatternRuleGetDatesParams prm);

    /// <summary>
    /// Defines a structure to hold the result of getting dates from the repeat pattern rule.
    /// </summary>
    public readonly struct PatternRuleGetDatesResult
    {
        /// <summary>
        /// Gets an empty instance of the <see cref="PatternRuleGetDatesResult"/> struct, representing no dates found.
        /// </summary>
        public static readonly PatternRuleGetDatesResult Empty = new(Array.Empty<DateOnly>());

        /// <summary>
        /// Gets the collection of dates that match the repeat pattern within the specified range.
        /// </summary>
        public IEnumerable<DateOnly> Dates { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRuleGetDatesResult"/> struct
        /// with the specified collection of dates.
        /// </summary>
        /// <param name="dates">The collection of dates that match the repeat pattern within the specified range.</param>
        public PatternRuleGetDatesResult(IEnumerable<DateOnly> dates)
        {
            Dates = dates;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternRuleGetDatesResult"/> struct.
        /// </summary>
        public PatternRuleGetDatesResult()
            : this(Array.Empty<DateOnly>())
        {
        }
    }

    /// <summary>
    /// Defines a structure to hold parameters for getting dates from the repeat pattern rule.
    /// </summary>
    public struct PatternRuleGetDatesParams
    {
        /// <summary>
        /// Gets or sets the minimum date to consider.
        /// </summary>
        public DateOnly MinDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum date to consider.
        /// </summary>
        public DateOnly MaxDate { get; set; }

        /// <summary>
        /// Gets or sets the format provider to use for formatting dates, if applicable.
        /// </summary>
        public IWeekFormatProvider? WeekFormat { get; set; }
    }

    /// <summary>
    /// Defines an interface for a repeat pattern rule that can generate occurrences of dates based on specific rules.
    /// </summary>
    public interface IDateRepeatPatternRule
    {
        /// <summary>
        /// Gets the occurrences of the repeat pattern within the specified range and up to the maximum date.
        /// </summary>
        /// <param name="prm">The parameters specifying the minimum and maximum
        /// dates to consider for the occurrences.</param>
        /// <returns>An enumerable of the occurrence dates.</returns>
        PatternRuleGetDatesResult GetDates(PatternRuleGetDatesParams prm);
    }
}
