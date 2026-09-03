using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a composite repeat pattern rule that combines daily, weekly, monthly, yearly
    /// and other repeat pattern rules.
    /// </summary>
    public partial class RepeatPatternRule : DateRepeatPatternRule
    {
        private readonly DailyRepeatPatternRule dailyRule;
        private readonly WeeklyRepeatPatternRule weeklyRule;
        private readonly MonthlyRepeatPatternRule monthlyRule;
        private readonly YearlyRepeatPatternRule yearlyRule;
        private readonly List<DateRepeatPatternRule> rules = new();
        private ScheduleRepeatPattern kind = ScheduleRepeatPattern.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternRule"/> class.
        /// </summary>
        public RepeatPatternRule()
        {
            StartDate = DateTime.Now.ToDateOnly();
            EndDate = StartDate;

            dailyRule = CreateDailyRule();
            weeklyRule = CreateWeeklyRule();
            monthlyRule = CreateMonthlyRule();
            yearlyRule = CreateYearlyRule();

            rules.Add(dailyRule);
            rules.Add(weeklyRule);
            rules.Add(monthlyRule);
            rules.Add(yearlyRule);

            dailyRule.PropertyChanged += OnChildRulePropertyChanged;
            weeklyRule.PropertyChanged += OnChildRulePropertyChanged;
            monthlyRule.PropertyChanged += OnChildRulePropertyChanged;
            yearlyRule.PropertyChanged += OnChildRulePropertyChanged;
        }

        /// <summary>
        /// Gets or sets the kind of repeat pattern represented by this composite rule.
        /// </summary>
        public virtual ScheduleRepeatPattern Kind
        {
            get => kind;
            set
            {
                SetProperty(ref kind, value);
            }
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
        /// Creates a deep copy of the current <see cref="RepeatPatternRule"/> instance.
        /// </summary>
        /// <returns>A new <see cref="RepeatPatternRule"/> instance that is a deep copy of the current instance.</returns>
        public virtual RepeatPatternRule Clone()
        {
            var clone = new RepeatPatternRule();
            clone.Assign(this);
            return clone;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="RepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not RepeatPatternRule other)
            {
                return false;
            }

            return Kind == other.Kind &&
                   DailyRule.Equals(other.DailyRule) &&
                   WeeklyRule.Equals(other.WeeklyRule) &&
                   MonthlyRule.Equals(other.MonthlyRule) &&
                   YearlyRule.Equals(other.YearlyRule);
        }

        /// <summary>
        /// Assigns the values from another instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values.</param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                SuspendPropertyChanged();
                DailyRule.Assign(null);
                WeeklyRule.Assign(null);
                MonthlyRule.Assign(null);
                YearlyRule.Assign(null);
                ResumePropertyChanged();
                return;
            }

            if (other is RepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return;

                SuspendPropertyChanged();
                DailyRule.Assign(otherRule.dailyRule);
                WeeklyRule.Assign(otherRule.weeklyRule);
                MonthlyRule.Assign(otherRule.monthlyRule);
                YearlyRule.Assign(otherRule.yearlyRule);
                Kind = otherRule.Kind;
                ResumePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the currently selected repeat pattern rule based on the <see cref="Kind"/> property.
        /// </summary>
        /// <returns>The currently selected <see cref="OwnedRepeatPatternRule"/> instance,
        /// or <c>null</c> if no rule is selected.</returns>
        public virtual OwnedRepeatPatternRule? GetSelectedRule()
        {
            switch (Kind)
            {
                case ScheduleRepeatPattern.Daily:
                    return dailyRule;
                case ScheduleRepeatPattern.Weekly:
                    return weeklyRule;
                case ScheduleRepeatPattern.Monthly:
                    return monthlyRule;
                case ScheduleRepeatPattern.Yearly:
                    return yearlyRule;
                default:
                    return null;
            }
        }

        /// <inheritdoc/>
        public override IDateRepeatPatternRule.RuleGetDatesResult GetDates(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            return GetSelectedRule()?.GetDates(prm) ?? IDateRepeatPatternRule.RuleGetDatesResult.Empty;
        }

        /// <summary>
        /// Called when a property of any child rule changes, allowing the composite rule to respond accordingly.
        /// </summary>
        /// <param name="sender">The child rule that raised the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnChildRulePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged();
        }

        /// <inheritdoc/>
        protected override void OnEndDateChanged()
        {
            base.OnEndDateChanged();
        }

        /// <inheritdoc/>
        protected override void OnStartDateChanged()
        {
            base.OnStartDateChanged();
        }

        /// <inheritdoc/>
        protected override void OnOccurrenceCountChanged()
        {
            base.OnOccurrenceCountChanged();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="YearlyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="YearlyRepeatPatternRule"/> class.</returns>
        protected virtual YearlyRepeatPatternRule CreateYearlyRule()
        {
            return new YearlyRepeatPatternRule(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MonthlyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="MonthlyRepeatPatternRule"/> class.</returns>
        protected virtual MonthlyRepeatPatternRule CreateMonthlyRule()
        {
            return new MonthlyRepeatPatternRule(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WeeklyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="WeeklyRepeatPatternRule"/> class.</returns>
        protected virtual WeeklyRepeatPatternRule CreateWeeklyRule()
        {
            return new WeeklyRepeatPatternRule(this);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DailyRepeatPatternRule"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="DailyRepeatPatternRule"/> class.</returns>
        protected virtual DailyRepeatPatternRule CreateDailyRule()
        {
            return new DailyRepeatPatternRule(this);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Kind, DailyRule, WeeklyRule, MonthlyRule, YearlyRule).GetHashCode();
        }
    }
}
