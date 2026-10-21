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
    public partial class RepeatPatternRule : BaseRepeatPatternRule, IEquatable<RepeatPatternRule>
    {
        private readonly DailyRepeatPatternRule dailyRule;
        private readonly WeeklyRepeatPatternRule weeklyRule;
        private readonly MonthlyRepeatPatternRule monthlyRule;
        private readonly YearlyRepeatPatternRule yearlyRule;
        private readonly List<BaseRepeatPatternRule> rules = new();
        private ScheduleRepeatPattern kind = ScheduleRepeatPattern.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternRule"/> class.
        /// </summary>
        public RepeatPatternRule()
        {
            var now = DateTime.Now;
            StartDate = now.ToDateOnly();
            StartTime = now.ToTimeOnly();
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
        /// Gets or sets a JSON string representation of the current state of this instance.
        /// </summary>
        [Browsable(false)]
        public virtual string AsJsonString
        {
            get
            {
                IPropertyBag propertyBag = new PropertyBag();
                SaveTo(propertyBag);
                return propertyBag.AsJsonString;
            }

            set
            {
                IPropertyBag propertyBag = new PropertyBag();
                propertyBag.AsJsonString = value;
                LoadFrom(propertyBag);
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
        public IReadOnlyList<BaseRepeatPatternRule> Rules => rules;

        /// <summary>
        /// Loads the state of this instance from the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag from which to load the state.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        /// <returns><c>true</c> if the state was successfully loaded; otherwise, <c>false</c>.</returns>
        public virtual bool LoadFrom(IPropertyBag? propertyBag, string? prefix = null)
        {
            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            return DoInsideSuspendedPropertyChanged(() =>
            {
                Reset();

                if (propertyBag.TryGetEnumProperty(prefix + nameof(Kind), out ScheduleRepeatPattern parsedKind))
                {
                    Kind = parsedKind;
                }

                if (propertyBag.TryGetDateTimeProperty(prefix + nameof(Start), out var startDateTime))
                {
                    Start = startDateTime;
                }

                if (propertyBag.TryGetDateTimeProperty(prefix + nameof(End), out var endDateTime))
                {
                    End = endDateTime;
                }

                switch (Kind)
                {
                    case ScheduleRepeatPattern.Daily:
                        return DailyRule.LoadFrom(propertyBag, prefix + nameof(DailyRule));
                    case ScheduleRepeatPattern.Weekly:
                        return WeeklyRule.LoadFrom(propertyBag, prefix + nameof(WeeklyRule));
                    case ScheduleRepeatPattern.Monthly:
                        return MonthlyRule.LoadFrom(propertyBag, prefix + nameof(MonthlyRule));
                    case ScheduleRepeatPattern.Yearly:
                        return YearlyRule.LoadFrom(propertyBag, prefix + nameof(YearlyRule));
                }

                return true;
            });
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag to which the state will be saved.</param>
        /// <param name="prefix">An optional prefix to prepend to property names.</param>
        public virtual void SaveTo(IPropertyBag propertyBag, string? prefix = null)
        {
            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            propertyBag.SetStringProperty(prefix + nameof(Kind), Kind.ToString());
            propertyBag.SetDateTimeProperty(prefix + nameof(Start), Start);
            propertyBag.SetDateTimeProperty(prefix + nameof(End), End);

            switch (Kind)
            {
                case ScheduleRepeatPattern.Daily:
                    DailyRule.SaveTo(propertyBag, prefix + nameof(DailyRule));
                    break;
                case ScheduleRepeatPattern.Weekly:
                    WeeklyRule.SaveTo(propertyBag, prefix + nameof(WeeklyRule));
                    break;
                case ScheduleRepeatPattern.Monthly:
                    MonthlyRule.SaveTo(propertyBag, prefix + nameof(MonthlyRule));
                    break;
                case ScheduleRepeatPattern.Yearly:
                    YearlyRule.SaveTo(propertyBag, prefix + nameof(YearlyRule));
                    break;
            }
        }

        /// <summary>
        /// Creates a deep copy of the current instance.
        /// </summary>
        /// <returns>A new instance that is a deep copy of the current instance.</returns>
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
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not RepeatPatternRule other)
            {
                return false;
            }

            return Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(RepeatPatternRule? other)
        {
            if (other == null)
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
        /// Resets the current instance to its default state which was set when the instance was created.
        /// </summary>
        public virtual void Reset()
        {
            Assign(null);
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

                var now = DateTime.Now;
                StartDate = now.ToDateOnly();
                StartTime = now.ToTimeOnly();
                Duration = TimeSpan.Zero;
                OccurrenceCount = 1;
                EndCondition = EndConditionKind.OnDate;
                EndDate = StartDate;
                Kind = ScheduleRepeatPattern.None;

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

                StartDate = otherRule.StartDate;
                StartTime = otherRule.StartTime;
                Duration = otherRule.Duration;
                EndDate = otherRule.EndDate;
                OccurrenceCount = otherRule.OccurrenceCount;
                EndCondition = otherRule.EndCondition;

                DailyRule.Assign(otherRule.DailyRule);
                WeeklyRule.Assign(otherRule.WeeklyRule);
                MonthlyRule.Assign(otherRule.MonthlyRule);
                YearlyRule.Assign(otherRule.YearlyRule);

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
        public override int GetHashCode()
        {
            return (Kind, DailyRule, WeeklyRule, MonthlyRule, YearlyRule).GetHashCode();
        }

        /// <inheritdoc/>
        public override IDateRepeatPatternRule.RuleGetDatesResult GetDates(IDateRepeatPatternRule.RuleGetDatesParams prm)
        {
            var rule = GetSelectedRule();

            if (rule == null)
            {
                return new IDateRepeatPatternRule.RuleGetDatesResult(Internal());
            }

            IEnumerable<DateOnly> Internal()
            {
                var minDate = prm.MinDate;
                var maxDate = prm.MaxDate;
                var startDate = StartDate;
                var endDate = EndDate;

                DateUtils.Sort(ref minDate, ref maxDate);
                DateUtils.Sort(ref startDate, ref endDate);

                if (minDate > endDate || maxDate < startDate)
                {
                    yield break;
                }

                if (minDate > startDate)
                {
                    startDate = minDate;
                }

                if (maxDate < endDate)
                {
                    endDate = maxDate;
                }

                while (startDate <= endDate)
                {
                    yield return startDate;
                    startDate = startDate.AddDays(1);
                }
            }

            return rule.GetDates(prm);
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
    }
}
