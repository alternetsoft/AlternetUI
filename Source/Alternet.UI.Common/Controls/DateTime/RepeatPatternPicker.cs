using System;
using System.Collections.Generic;
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
            dailyPicker = CreateDailyPatternPicker();
            weeklyPicker = CreateWeeklyPatternPicker();
            monthlyPicker = CreateMonthlyPatternPicker();
            yearlyPicker = CreateYearlyPatternPicker();

            tabControl.ActiveTabHasBorder = true;
            tabControl.TabHasBorder = true;
            tabControl.HasInteriorBorder = false;
            tabControl.ActiveTabTheme = SpeedButton.KnownTheme.StaticBorder;
            tabControl.TabTheme = SpeedButton.KnownTheme.None;
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
            private TValue data = new();

            /// <summary>
            /// Occurs when the value of the repeat pattern rule changes.
            /// </summary>
            public event EventHandler? ValueChanged;

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
            private readonly XRadioButton everyDayRadioButton = new();
            private readonly XRadioButton evenDaysRadioButton = new();
            private readonly XRadioButton oddDaysRadioButton = new();
            private readonly XRadioButton intervalRadioButton = new();
            private readonly XRadioButton weekdaysRadioButton = new();
            private readonly XRadioButton weekendsRadioButton = new();
            private readonly ControlSet<XRadioButton> radioButtons;

            /// <summary>
            /// Initializes a new instance of the <see cref="DailyPatternPicker"/> class.
            /// </summary>
            public DailyPatternPicker()
            {
                MinChildMargin = 5;
                Layout = LayoutStyle.Vertical;

                everyDayRadioButton.Text = CommonStrings.Default.DailyRepeatPatternRuleKindEveryDay;
                evenDaysRadioButton.Text = CommonStrings.Default.DailyRepeatPatternRuleKindEvenDays;
                oddDaysRadioButton.Text = CommonStrings.Default.DailyRepeatPatternRuleKindOddDays;
                intervalRadioButton.Text = CommonStrings.Default.DailyRepeatPatternRuleKindIntervalDay;
                weekdaysRadioButton.Text = CommonStrings.Default.DailyRepeatPatternRuleKindWeekdays;
                weekendsRadioButton.Text = CommonStrings.Default.DailyRepeatPatternRuleKindWeekends;

                radioButtons = new(
                    everyDayRadioButton,
                    evenDaysRadioButton,
                    oddDaysRadioButton,
                    weekdaysRadioButton,
                    weekendsRadioButton,
                    intervalRadioButton);

                radioButtons.Parent(this);

                // string DailyRepeatPatternRuleKindIntervalDays = "Every {0} days";
            }

            /// <summary>
            /// Gets the radio button for selecting the "Every Day" repeat pattern.
            /// </summary>
            public XRadioButton EveryDayRadioButton => everyDayRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Even Days" repeat pattern.
            /// </summary>
            public XRadioButton EvenDaysRadioButton => evenDaysRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Odd Days" repeat pattern.
            /// </summary>
            public XRadioButton OddDaysRadioButton => oddDaysRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Interval Day" repeat pattern.
            /// </summary>
            public XRadioButton IntervalRadioButton => intervalRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Weekdays" repeat pattern.
            /// </summary>
            public XRadioButton WeekdaysRadioButton => weekdaysRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Weekends" repeat pattern.
            /// </summary>
            public XRadioButton WeekendsRadioButton => weekendsRadioButton;

            /// <summary>
            /// Gets the set of all radio buttons in the daily pattern picker.
            /// </summary>
            public ControlSet<XRadioButton> RadioButtons => radioButtons;
        }

        /// <summary>
        /// Represents a control that allows users to select a weekly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class WeeklyPatternPicker : DateRepeatPatternRulePicker<WeeklyRepeatPatternRule>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WeeklyPatternPicker"/> class.
            /// </summary>
            public WeeklyPatternPicker()
            {
                SuggestedHeight = 100;
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a monthly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class MonthlyPatternPicker : DateRepeatPatternRulePicker<MonthlyRepeatPatternRule>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MonthlyPatternPicker"/> class.
            /// </summary>
            public MonthlyPatternPicker()
            {
                SuggestedHeight = 100;
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a yearly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.Date)]
        public partial class YearlyPatternPicker : DateRepeatPatternRulePicker<YearlyRepeatPatternRule>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="YearlyPatternPicker"/> class.
            /// </summary>
            public YearlyPatternPicker()
            {
                SuggestedHeight = 100;
            }
        }
    }
}
