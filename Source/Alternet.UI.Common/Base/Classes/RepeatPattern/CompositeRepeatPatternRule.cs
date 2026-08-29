using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a composite repeat pattern rule that combines daily, weekly, monthly, yearly
    /// and other repeat pattern rules.
    /// </summary>
    public class CompositeRepeatPatternRule : DateRepeatPatternRule
    {
        private readonly DailyRepeatPatternRule dailyRule;
        private readonly WeeklyRepeatPatternRule weeklyRule;
        private readonly MonthlyRepeatPatternRule monthlyRule;
        private readonly YearlyRepeatPatternRule yearlyRule;
        private readonly List<DateRepeatPatternRule> rules = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeRepeatPatternRule"/> class.
        /// </summary>
        public CompositeRepeatPatternRule()
        {
            dailyRule = CreateDailyRule();
            weeklyRule = CreateWeeklyRule();
            monthlyRule = CreateMonthlyRule();
            yearlyRule = CreateYearlyRule();

            rules.Add(dailyRule);
            rules.Add(weeklyRule);
            rules.Add(monthlyRule);
            rules.Add(yearlyRule);
        }

        /// <summary>
        /// Gets the <see cref="DailyRepeatPatternRule"/> instance for daily recurrence settings.
        /// </summary>
        public DailyRepeatPatternRule DailyRule => dailyRule;

        /// <summary>
        /// Gets the <see cref="WeeklyRepeatPatternRule"/> instance for weekly recurrence settings. 
        /// </summary>
        public WeeklyRepeatPatternRule WeeklyRule => weeklyRule;

        /// <summary>
        /// Gets the <see cref="MonthlyRepeatPatternRule"/> instance for monthly recurrence settings.
        /// </summary>
        public MonthlyRepeatPatternRule MonthlyRule => monthlyRule;

        /// <summary>
        /// Gets the <see cref="YearlyRepeatPatternRule"/> instance for yearly recurrence settings.
        /// </summary>
        public YearlyRepeatPatternRule YearlyRule => yearlyRule;

        /// <summary>
        /// Gets a read-only list of all the repeat pattern rules contained in this composite rule.
        /// </summary>
        public IReadOnlyList<DateRepeatPatternRule> Rules => rules;

        /// <summary>
        /// Creates a new instance of the <see cref="YearlyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="YearlyRepeatPatternRule"/> class.</returns>
        protected virtual YearlyRepeatPatternRule CreateYearlyRule()
        {
            return new YearlyRepeatPatternRule();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MonthlyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="MonthlyRepeatPatternRule"/> class.</returns>
        protected virtual MonthlyRepeatPatternRule CreateMonthlyRule()
        {
            return new MonthlyRepeatPatternRule();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WeeklyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="WeeklyRepeatPatternRule"/> class.</returns>
        protected virtual WeeklyRepeatPatternRule CreateWeeklyRule()
        {
            return new WeeklyRepeatPatternRule();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DailyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="DailyRepeatPatternRule"/> class.</returns>
        protected virtual DailyRepeatPatternRule CreateDailyRule()
        {
            return new DailyRepeatPatternRule();
        }
    }
}
