using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a repeat pattern for an event or task.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class RepeatPatternPicker : HiddenBorder
    {
        private readonly TabControl tabControl = new();
        private readonly DailyPatternPicker dailyPicker;
        private readonly WeeklyPatternPicker weeklyPicker;
        private readonly MonthlyPatternPicker monthlyPicker;
        private readonly YearlyPatternPicker yearlyPicker;
        private readonly HiddenBorder nonePicker = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternPicker"/> class.
        /// </summary>
        public RepeatPatternPicker()
        {
            Padding = 5;

            dailyPicker = CreateDailyPatternPicker();
            weeklyPicker = CreateWeeklyPatternPicker();
            monthlyPicker = CreateMonthlyPatternPicker();
            yearlyPicker = CreateYearlyPatternPicker();

            tabControl.ActiveTabHasBorder = true;
            tabControl.TabHasBorder = true;
            tabControl.HasInteriorBorder = false;
            tabControl.ActiveTabTheme = SpeedButton.KnownTheme.StaticBorder;
            tabControl.TabTheme = SpeedButton.KnownTheme.Default;
            tabControl.ContentVisible = false;

            tabControl.Parent = this;

            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternNone, nonePicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternDaily, dailyPicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternWeekly, weeklyPicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternMonthly, monthlyPicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternYearly, yearlyPicker);

            tabControl.SelectedIndexChanged += OnTabControlSelectedIndexChanged;

            HasBorder = true;
        }

        /// <summary>
        /// Occurs when the selected repeat pattern changes.
        /// </summary>
        public event EventHandler? SelectedPatternChanged;

        /// <summary>
        /// Gets the inner <see cref="TabControl"/> used for selecting the repeat pattern.
        /// Its pages contain controls for selecting specific repeat pattern rules.
        /// </summary>
        public TabControl InnerTabControl => tabControl;

        /// <summary>
        /// Gets the <see cref="DailyPatternPicker"/> control for selecting a daily repeat pattern.
        /// </summary>
        public DailyPatternPicker DailyPicker => dailyPicker;

        /// <summary>
        /// Gets the <see cref="WeeklyPatternPicker"/> control for selecting a weekly repeat pattern.
        /// </summary>
        public WeeklyPatternPicker WeeklyPicker => weeklyPicker;

        /// <summary>
        /// Gets the <see cref="MonthlyPatternPicker"/> control for selecting a monthly repeat pattern.
        /// </summary>
        public MonthlyPatternPicker MonthlyPicker => monthlyPicker;

        /// <summary>
        /// Gets the <see cref="YearlyPatternPicker"/> control for selecting a yearly repeat pattern.
        /// </summary>
        public YearlyPatternPicker YearlyPicker => yearlyPicker;

        /// <summary>
        /// Gets the control for selecting "no repeat" pattern.
        /// </summary>
        public HiddenBorder NonePicker => nonePicker;

        /// <summary>
        /// Gets or sets the selected repeat pattern.
        /// </summary>
        public virtual ScheduleRepeatPattern SelectedPattern
        {
            get
            {
                var index = tabControl.SelectedIndex;

                if (index >= 0 && index <= (int)ScheduleRepeatPattern.Yearly)
                    return (ScheduleRepeatPattern)index;
                return ScheduleRepeatPattern.None;
            }

            set
            {
                tabControl.SelectedIndex = (int)value;
            }
        }

        /// <summary>
        /// Called when the selected index of the inner tab control changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTabControlSelectedIndexChanged(object? sender, EventArgs e)
        {
            tabControl.ContentVisible = SelectedPattern != ScheduleRepeatPattern.None;
            SelectedPatternChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DailyPatternPicker"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual DailyPatternPicker CreateDailyPatternPicker() => new();

        /// <summary>
        /// Creates a new instance of the <see cref="WeeklyPatternPicker"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual WeeklyPatternPicker CreateWeeklyPatternPicker() => new();

        /// <summary>
        /// Creates a new instance of the <see cref="MonthlyPatternPicker"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual MonthlyPatternPicker CreateMonthlyPatternPicker() => new();

        /// <summary>
        /// Creates a new instance of the <see cref="YearlyPatternPicker"/> control.
        /// </summary>
        /// <returns></returns>
        protected virtual YearlyPatternPicker CreateYearlyPatternPicker() => new();

        /// <summary>
        /// Represents a generic control for selecting a repeat pattern rule.
        /// </summary>
        /// <typeparam name="TValue">The type of the repeat pattern rule.</typeparam>
        [ControlCategory(KnownControlCategory.Date)]
        public abstract partial class DateRepeatPatternRulePicker<TValue> : HiddenBorder
            where TValue : DateRepeatPatternRule, new()
        {
            /// <summary>
            /// Gets or sets the default minimum margin for child controls within the repeat pattern rule picker.
            /// </summary>
            public static Thickness DefaultMinChildMargin = (2, 2, 2, 2);

            private TValue data = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="DateRepeatPatternRulePicker{TValue}"/> class.
            /// </summary>
            public DateRepeatPatternRulePicker()
            {
                MinChildMargin = DefaultMinChildMargin;
                Padding = 5;
            }

            /// <summary>
            /// Occurs when the value of the repeat pattern rule changes.
            /// </summary>
            public event EventHandler? ValueChanged;

            /// <summary>
            /// Gets or sets the format provider used for formatting and parsing date and time values.
            /// </summary>
            public virtual IFormatProvider? FormatProvider { get; set; }

            /// <summary>
            /// Gets or sets the value of the date repeat pattern rule.
            /// </summary>
            public virtual TValue Value
            {
                get
                {
                    return data;
                }

                set
                {
                    value ??= new();

                    if (data == value)
                        return;
                    data = value;
                    OnValueChanged();
                }
            }

            /// <summary>
            /// Creates a new instance of the <see cref="XRadioButton"/> control with optional text and tag.
            /// </summary>
            /// <param name="text">The text to display on the radio button.</param>
            /// <param name="tag">The tag associated with the radio button.</param>
            /// <returns>A new instance of the <see cref="XRadioButton"/> control.</returns>
            protected virtual XRadioButton CreateRadioButton(string? text = null, object? tag = null)
            {
                var radioButton = new XRadioButton();

                if (text != null) radioButton.Text = text;

                radioButton.Tag = tag;
                return radioButton;
            }

            /// <summary>
            /// Called when the value of the repeat pattern rule changes.
            /// </summary>
            protected virtual void OnValueChanged()
            {
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a daily repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class DailyPatternPicker : DateRepeatPatternRulePicker<DailyRepeatPatternRule>
        {
            private readonly XRadioButtonAndSuffix everyDayRadioButton;
            private readonly XRadioButtonAndSuffix evenDaysRadioButton;
            private readonly XRadioButtonAndSuffix oddDaysRadioButton;
            private readonly XRadioButtonAndSuffix intervalRadioButton;
            private readonly XRadioButtonAndSuffix weekdaysRadioButton;
            private readonly XRadioButtonAndSuffix weekendsRadioButton;
            private readonly ControlSet<XRadioButton> radioButtons;
            private readonly XIntPickerWithLabels intervalDayPicker = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="DailyPatternPicker"/> class.
            /// </summary>
            public DailyPatternPicker()
            {
                everyDayRadioButton = new();
                everyDayRadioButton.MainControl.Tag = DailyRepeatPatternRule.RepeatKind.EveryDay;
                everyDayRadioButton.SuffixControl.Text = CommonStrings.Default.DailyRepeatPatternRuleKindEveryDay;
                everyDayRadioButton.Parent = this;

                evenDaysRadioButton = new();
                evenDaysRadioButton.MainControl.Tag = DailyRepeatPatternRule.RepeatKind.EvenDays;
                evenDaysRadioButton.SuffixControl.Text = CommonStrings.Default.DailyRepeatPatternRuleKindEvenDays;
                evenDaysRadioButton.Parent = this;

                oddDaysRadioButton = new();
                oddDaysRadioButton.MainControl.Tag = DailyRepeatPatternRule.RepeatKind.OddDays;
                oddDaysRadioButton.SuffixControl.Text = CommonStrings.Default.DailyRepeatPatternRuleKindOddDays;
                oddDaysRadioButton.Parent = this;

                intervalDayPicker.PrefixText = CommonStrings.Default.DateRepeatPatternPrefixLabelEvery;
                intervalDayPicker.Minimum = 1;
                intervalDayPicker.VerticalAlignment = VerticalAlignment.Center;
                intervalDayPicker.Value = Value.IntervalDays;
                intervalDayPicker.ValueChanged += OnIntervalDayPickerValueChanged;

                UpdateSuffixLabelText();

                intervalRadioButton = new(intervalDayPicker);
                intervalRadioButton.MainControl.Tag = DailyRepeatPatternRule.RepeatKind.IntervalDays;
                intervalRadioButton.Parent = this;

                intervalDayPicker.Click += (s, e) =>
                {
                    intervalRadioButton.IsChecked = true;
                };

                weekdaysRadioButton = new();
                weekdaysRadioButton.MainControl.Tag = DailyRepeatPatternRule.RepeatKind.Weekdays;
                weekdaysRadioButton.SuffixControl.Text = CommonStrings.Default.DailyRepeatPatternRuleKindWeekdays;
                weekdaysRadioButton.Parent = this;

                weekendsRadioButton = new();
                weekendsRadioButton.MainControl.Tag = DailyRepeatPatternRule.RepeatKind.Weekends;
                weekendsRadioButton.SuffixControl.Text = CommonStrings.Default.DailyRepeatPatternRuleKindWeekends;
                weekendsRadioButton.Parent = this;

                Layout = LayoutStyle.Vertical;

                radioButtons = new(
                    everyDayRadioButton.MainControl,
                    evenDaysRadioButton.MainControl,
                    oddDaysRadioButton.MainControl,
                    weekdaysRadioButton.MainControl,
                    weekendsRadioButton.MainControl,
                    intervalRadioButton.MainControl);

                radioButtons.ForEach(c =>
                {
                    var kind = c.Tag as DailyRepeatPatternRule.RepeatKind?;

                    c.IsChecked = Value.Kind == kind;

                    c.RadioSiblings = radioButtons.Items;

                    c.CheckedChanged += (s, e) =>
                    {
                        Value.Kind = kind ?? DailyRepeatPatternRule.RepeatKind.EveryDay;
                    };
                });

            }

            /// <summary>
            /// Gets the integer picker control for selecting the interval day value in the daily repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XIntPickerWithLabels IntervalDayPicker => intervalDayPicker;

            /// <summary>
            /// Gets the radio button for selecting the "Every Day" repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix EveryDayRadioButton => everyDayRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Even Days" repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix EvenDaysRadioButton => evenDaysRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Odd Days" repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix OddDaysRadioButton => oddDaysRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Interval Day" repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix IntervalRadioButton => intervalRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Weekdays" repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix WeekdaysRadioButton => weekdaysRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Weekends" repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix WeekendsRadioButton => weekendsRadioButton;

            /// <summary>
            /// Called when the value of the interval day picker changes.
            /// </summary>
            /// <param name="sender">The sender of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnIntervalDayPickerValueChanged(object? sender, EventArgs e)
            {
                Value.IntervalDays = intervalDayPicker.Value;
                UpdateSuffixLabelText();
            }

            /// <summary>
            /// Updates the text of the suffix label based on the value of the interval day picker.
            /// </summary>
            protected virtual void UpdateSuffixLabelText()
            {
                if (intervalDayPicker.Value == 1)
                {
                    intervalDayPicker.SuffixText = CommonStrings.Default.TimePeriodUnitDay;
                }
                else
                {
                    intervalDayPicker.SuffixText = CommonStrings.Default.TimePeriodUnitDays;
                }
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a weekly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class WeeklyPatternPicker : DateRepeatPatternRulePicker<WeeklyRepeatPatternRule>
        {
            private readonly XIntPickerWithLabels intervalWeekPicker = new();
            private readonly PanelSettings weekDaysPanel = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="WeeklyPatternPicker"/> class.
            /// </summary>
            public WeeklyPatternPicker()
            {
                Layout = LayoutStyle.Vertical;

                intervalWeekPicker.PrefixLabel.MarginLeft = XCheckBox.DefaultCheckBoxMargin.Left + 3;               
                intervalWeekPicker.PrefixText = CommonStrings.Default.DateRepeatPatternPrefixLabelEvery;
                intervalWeekPicker.Minimum = 1;
                intervalWeekPicker.Value = Value.IntervalWeeks;
                intervalWeekPicker.ValueChanged += OnIntervalWeekPickerValueChanged;
                UpdateSuffixLabelText();

                var firstDayOfWeek = DateUtils.SystemFirstDayOfWeek;

                DaysOfWeek[] weekdays;
                List<string> titles = new();
                var dayNames = DateUtils.GetDayNames(DayNamesKind.Full, FormatProvider);

                if (firstDayOfWeek == DayOfWeek.Sunday)
                {
                    weekdays = new DaysOfWeek[]
                    {
                        DaysOfWeek.Sunday,
                        DaysOfWeek.Monday,
                        DaysOfWeek.Tuesday,
                        DaysOfWeek.Wednesday,
                        DaysOfWeek.Thursday,
                        DaysOfWeek.Friday,
                        DaysOfWeek.Saturday,
                    };

                    titles.AddRange(dayNames);
                }
                else
                {
                    weekdays = new DaysOfWeek[]
                    {
                        DaysOfWeek.Monday,
                        DaysOfWeek.Tuesday,
                        DaysOfWeek.Wednesday,
                        DaysOfWeek.Thursday,
                        DaysOfWeek.Friday,
                        DaysOfWeek.Saturday,
                        DaysOfWeek.Sunday,
                    };

                    titles.AddRange(dayNames);
                    titles.RemoveAt(0);
                    titles.Add(dayNames[0]);
                }

                weekDaysPanel.AddFlagCheckBoxes(
                    label: null,
                    getValue: () => Value.WeekDays,
                    setValue: v => Value.WeekDays = v,
                    itemTitles: titles.ToArray(),
                    itemValues: weekdays,
                    e: null);

                intervalWeekPicker.Parent = this;
                weekDaysPanel.Parent = this;
            }

            /// <summary>
            /// Gets the integer picker control for selecting the interval week value in the weekly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XIntPickerWithLabels IntervalWeekPicker => intervalWeekPicker;

            /// <summary>
            /// Gets the label that displays the suffix text for the interval week picker in the weekly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public Label IntervalWeekSuffixLabel => intervalWeekPicker.SuffixLabel;

            /// <summary>
            /// Gets the label that displays the prefix text for the interval week picker in the weekly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public Label IntervalWeekPrefixLabel => intervalWeekPicker.PrefixLabel;

            /// <summary>
            /// Called when the value of the interval week picker changes.
            /// </summary>
            /// <param name="sender">The sender of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnIntervalWeekPickerValueChanged(object? sender, EventArgs e)
            {
                Value.IntervalWeeks = intervalWeekPicker.Value;
                UpdateSuffixLabelText();
            }

            /// <summary>
            /// Updates the text of the suffix label based on the value of the interval week picker.
            /// </summary>
            protected virtual void UpdateSuffixLabelText()
            {
                if (intervalWeekPicker.Value == 1)
                {
                    intervalWeekPicker.SuffixLabel.Text = CommonStrings.Default.TimePeriodUnitWeek;
                }
                else
                {
                    intervalWeekPicker.SuffixLabel.Text = CommonStrings.Default.TimePeriodUnitWeeks;
                }
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a monthly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class MonthlyPatternPicker : DateRepeatPatternRulePicker<MonthlyRepeatPatternRule>
        {
            private readonly XIntPickerWithLabels intervalMonthPicker = new();
            private readonly RelativeWeekdayOfMonthPicker relativeWeekdayOfMonthPicker;
            private readonly XRadioButtonAndSuffix dayOfMonthRadioButton;
            private readonly XRadioButtonAndSuffix relativeWeekdayRadioButton;
            private readonly XIntPickerWithLabels dayOfMonthPicker = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="MonthlyPatternPicker"/> class.
            /// </summary>
            public MonthlyPatternPicker()
            {
                Layout = LayoutStyle.Vertical;

                intervalMonthPicker.PrefixLabel.MarginLeft = XCheckBox.DefaultCheckBoxMargin.Left + 3;
                intervalMonthPicker.PrefixText = CommonStrings.Default.DateRepeatPatternPrefixLabelEvery;
                intervalMonthPicker.Minimum = 1;
                intervalMonthPicker.Value = Value.IntervalMonths;
                intervalMonthPicker.ValueChanged += OnIntervalMonthPickerValueChanged;
                UpdateSuffixLabelText();

                relativeWeekdayOfMonthPicker = new();
                relativeWeekdayOfMonthPicker.IsMonthVisible = false;
                
                dayOfMonthPicker.PrefixText = CommonStrings.Default.OnDayPrefix;
                dayOfMonthPicker.SuffixLabel.IsVisible = false;
                dayOfMonthPicker.Value = Value.DayOfMonth;

                dayOfMonthRadioButton = new(dayOfMonthPicker);
                relativeWeekdayRadioButton = new(relativeWeekdayOfMonthPicker);

                dayOfMonthPicker.Minimum = 1;
                dayOfMonthPicker.Maximum = 31;
                dayOfMonthPicker.Click += (s, e) =>
                {
                    dayOfMonthRadioButton.IsChecked = true;
                };

                relativeWeekdayOfMonthPicker.Click += (s, e) =>
                {
                    relativeWeekdayRadioButton.IsChecked = true;
                };

                XRadioButton[] radioGroup = { dayOfMonthRadioButton.MainControl, relativeWeekdayRadioButton.MainControl };

                dayOfMonthRadioButton.MainControl.RadioSiblings = radioGroup;
                relativeWeekdayRadioButton.MainControl.RadioSiblings = radioGroup;

                dayOfMonthRadioButton.IsChecked = Value.Kind == MonthlyRepeatPatternRule.RepeatKind.DayOfMonth;
                relativeWeekdayRadioButton.IsChecked = Value.Kind == MonthlyRepeatPatternRule.RepeatKind.RelativeWeekday;

                dayOfMonthRadioButton.MainControl.CheckedChanged += (s, e) =>
                {
                    if (dayOfMonthRadioButton.IsChecked)
                        Value.Kind = MonthlyRepeatPatternRule.RepeatKind.DayOfMonth;
                };

                relativeWeekdayRadioButton.MainControl.CheckedChanged += (s, e) =>
                {
                    if (relativeWeekdayRadioButton.IsChecked)
                        Value.Kind = MonthlyRepeatPatternRule.RepeatKind.RelativeWeekday;
                };

                intervalMonthPicker.Parent = this;
                dayOfMonthRadioButton.Parent = this;
                relativeWeekdayRadioButton.Parent = this;
            }

            /// <summary>
            /// Called when the value of the interval month picker changes.
            /// </summary>
            /// <param name="sender">The sender of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnIntervalMonthPickerValueChanged(object? sender, EventArgs e)
            {
                Value.IntervalMonths = intervalMonthPicker.Value;
                UpdateSuffixLabelText();
            }

            /// <summary>
            /// Updates the text of the suffix label based on the value of the interval month picker.
            /// </summary>
            protected virtual void UpdateSuffixLabelText()
            {
                if (intervalMonthPicker.Value == 1)
                {
                    intervalMonthPicker.SuffixLabel.Text = CommonStrings.Default.TimePeriodUnitMonth;
                }
                else
                {
                    intervalMonthPicker.SuffixLabel.Text = CommonStrings.Default.TimePeriodUnitMonths;
                }
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a yearly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class YearlyPatternPicker : DateRepeatPatternRulePicker<YearlyRepeatPatternRule>
        {
            private readonly XIntPickerWithLabels intervalYearPicker = new();
            private readonly XRadioButtonAndSuffix dayOfMonthRadioButton;
            private readonly XRadioButtonAndSuffix relativeWeekdayRadioButton;
            private readonly RelativeWeekdayOfMonthPicker relativeWeekdayOfMonthPicker;
            private readonly MonthAndDayPicker monthAndDayPicker;

            /// <summary>
            /// Initializes a new instance of the <see cref="YearlyPatternPicker"/> class.
            /// </summary>
            public YearlyPatternPicker()
            {
                Layout = LayoutStyle.Vertical;

                intervalYearPicker.PrefixLabel.MarginLeft = XCheckBox.DefaultCheckBoxMargin.Left + 3;
                intervalYearPicker.PrefixText = CommonStrings.Default.DateRepeatPatternPrefixLabelEvery;
                intervalYearPicker.Minimum = 1;
                intervalYearPicker.Value = Value.IntervalYears;
                intervalYearPicker.ValueChanged += OnIntervalYearPickerValueChanged;
                UpdateSuffixLabelText();

                monthAndDayPicker = new();
                monthAndDayPicker.Label.Text = CommonStrings.Default.OnPrefix;
                monthAndDayPicker.Label.Visible = true;

                relativeWeekdayOfMonthPicker = new();

                dayOfMonthRadioButton = new(monthAndDayPicker);
                relativeWeekdayRadioButton = new(relativeWeekdayOfMonthPicker);

                monthAndDayPicker.Click += (s, e) =>
                {
                    dayOfMonthRadioButton.IsChecked = true;
                };

                relativeWeekdayOfMonthPicker.Click += (s, e) =>
                {
                    relativeWeekdayRadioButton.IsChecked = true;
                };

                dayOfMonthRadioButton.IsChecked = Value.Kind == YearlyRepeatPatternRule.RepeatKind.DayOfMonth;
                relativeWeekdayRadioButton.IsChecked = Value.Kind == YearlyRepeatPatternRule.RepeatKind.RelativeWeekday;

                XRadioButton[] radioGroup = {dayOfMonthRadioButton.MainControl, relativeWeekdayRadioButton.MainControl};

                dayOfMonthRadioButton.MainControl.RadioSiblings = radioGroup;
                relativeWeekdayRadioButton.MainControl.RadioSiblings = radioGroup;

                dayOfMonthRadioButton.MainControl.CheckedChanged += (s, e) =>
                {
                    if (dayOfMonthRadioButton.IsChecked)
                        Value.Kind = YearlyRepeatPatternRule.RepeatKind.DayOfMonth;
                };

                relativeWeekdayRadioButton.MainControl.CheckedChanged += (s, e) =>
                {
                    if (relativeWeekdayRadioButton.IsChecked)
                        Value.Kind = YearlyRepeatPatternRule.RepeatKind.RelativeWeekday;
                };

                intervalYearPicker.Parent = this;
                dayOfMonthRadioButton.Parent = this;
                relativeWeekdayRadioButton.Parent = this;
            }

            /// <summary>
            /// Gets the integer picker control for selecting the interval year value in the yearly repeat pattern.
            /// </summary>
            public XIntPickerWithLabels IntervalYearPicker => intervalYearPicker;

            /// <summary>
            /// Gets the radio button for selecting the "Day of Month" repeat pattern in the yearly repeat pattern.
            /// </summary>
            public XRadioButtonAndSuffix DayOfMonthRadioButton => dayOfMonthRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Relative Weekday" repeat pattern in the yearly repeat pattern.
            /// </summary>
            public XRadioButtonAndSuffix RelativeWeekdayRadioButton => relativeWeekdayRadioButton;

            /// <summary>
            /// Gets the <see cref="RelativeWeekdayOfMonthPicker"/> control used to select the
            /// relative weekday of the month in the yearly repeat pattern.
            /// </summary>
            public RelativeWeekdayOfMonthPicker RelativeWeekdayOfMonthPicker => relativeWeekdayOfMonthPicker;

            /// <summary>
            /// Gets the <see cref="MonthAndDayPicker"/> control used to select the month and day in the yearly repeat pattern.
            /// </summary>
            public MonthAndDayPicker MonthAndDayPicker => monthAndDayPicker;

            /// <summary>
            /// Called when the value of the interval year picker changes.
            /// </summary>
            /// <param name="sender">The sender of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnIntervalYearPickerValueChanged(object? sender, EventArgs e)
            {
                Value.IntervalYears = intervalYearPicker.Value;
                UpdateSuffixLabelText();
            }

            /// <summary>
            /// Updates the text of the suffix label based on the value of the interval year picker.
            /// </summary>
            protected virtual void UpdateSuffixLabelText()
            {
                if (intervalYearPicker.Value == 1)
                {
                    intervalYearPicker.SuffixLabel.Text = CommonStrings.Default.TimePeriodUnitYear;
                }
                else
                {
                    intervalYearPicker.SuffixLabel.Text = CommonStrings.Default.TimePeriodUnitYears;
                }
            }
        }
    }
}
