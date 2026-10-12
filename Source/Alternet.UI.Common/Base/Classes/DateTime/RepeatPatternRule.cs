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
    public partial class RepeatPatternRule : BaseRepeatPatternRule
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
        /// Gets a JSON string representation of the current state of this <see cref="RepeatPatternRule"/> instance.
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
        /// Converts the specified <see cref="DateTime"/> to its string representation.
        /// </summary>
        /// <param name="date">The date and time to convert.</param>
        /// <returns>A string representation of the specified <see cref="DateTime"/>.</returns>
        public virtual string DateTimeToString(DateTime date)
        {
            return DateTimeSerialization.Instance.DateTimeToString(date);
        }

        /// <summary>
        /// Attempts to convert the specified string representation of a date and time to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">The string representation of the date and time to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="DateTime"/>
        /// if the conversion succeeded, or <see cref="DateTime.MinValue"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryDateTimeFromString(string? s, out DateTime result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = DateTime.MinValue;
                return false;
            }

            return DateTimeSerialization.Instance.TryDateTimeFromString(s, out result);
        }

        /// <summary>
        /// Loads the state of this instance from the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag from which to load the state.</param>
        /// <returns><c>true</c> if the state was successfully loaded; otherwise, <c>false</c>.</returns>
        public virtual bool LoadFrom(IPropertyBag? propertyBag)
        {
            if (propertyBag == null)
                return false;

            return DoInsideSuspendedPropertyChanged(() =>
            {
                if (propertyBag.TryGetEnumProperty(nameof(Kind), out ScheduleRepeatPattern parsedKind))
                {
                    Kind = parsedKind;
                }
                else
                {
                    return false;
                }

                if (propertyBag.TryGetStringProperty(nameof(Start), out var startString) &&
                    TryDateTimeFromString(startString, out var startDateTime))
                {
                    Start = startDateTime;
                }
                else
                {
                    return false;
                }

                if (propertyBag.TryGetStringProperty(nameof(End), out var endString) &&
                    TryDateTimeFromString(endString, out var endDateTime))
                {
                    End = endDateTime;
                }
                else
                {
                    return false;
                }

                switch (Kind)
                {
                    case ScheduleRepeatPattern.Daily:
                        return DailyRule.LoadFrom(propertyBag, nameof(DailyRule));
                    case ScheduleRepeatPattern.Weekly:
                        return WeeklyRule.LoadFrom(propertyBag, nameof(WeeklyRule));
                    case ScheduleRepeatPattern.Monthly:
                        return MonthlyRule.LoadFrom(propertyBag, nameof(MonthlyRule));
                    case ScheduleRepeatPattern.Yearly:
                        return YearlyRule.LoadFrom(propertyBag, nameof(YearlyRule));
                }

                return true;
            });
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag to which the state will be saved.</param>
        public virtual void SaveTo(IPropertyBag propertyBag)
        {
            propertyBag.SetStringProperty(nameof(Kind), Kind.ToString());
            propertyBag.SetStringProperty(nameof(Start), DateTimeToString(Start));
            propertyBag.SetStringProperty(nameof(End), DateTimeToString(End));

            switch (Kind)
            {
                case ScheduleRepeatPattern.Daily:
                    DailyRule.SaveTo(propertyBag, nameof(DailyRule));
                    break;
                case ScheduleRepeatPattern.Weekly:
                    WeeklyRule.SaveTo(propertyBag, nameof(WeeklyRule));
                    break;
                case ScheduleRepeatPattern.Monthly:
                    MonthlyRule.SaveTo(propertyBag, nameof(MonthlyRule));
                    break;
                case ScheduleRepeatPattern.Yearly:
                    YearlyRule.SaveTo(propertyBag, nameof(YearlyRule));
                    break;
            }
        }

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
