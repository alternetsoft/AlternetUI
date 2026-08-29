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
        /// <summary>
        /// Gets the default padding for the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static readonly Thickness DefaultPadding = 5;

        private readonly TabControl tabControl = new();
        private readonly DailyPatternPicker dailyPicker;
        private readonly WeeklyPatternPicker weeklyPicker;
        private readonly MonthlyPatternPicker monthlyPicker;
        private readonly YearlyPatternPicker yearlyPicker;
        private readonly HiddenBorder nonePicker = new();
        private readonly CompositeRepeatPatternRule data;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternPicker"/> class.
        /// </summary>
        public RepeatPatternPicker()
        {
            Padding = DefaultPadding;

            data = CreateRule();

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

            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternNone, nonePicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternDaily, dailyPicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternWeekly, weeklyPicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternMonthly, monthlyPicker);
            tabControl.Add(CommonStrings.Default.ScheduleRepeatPatternYearly, yearlyPicker);

            HasBorder = true;

            tabControl.Parent = this;

            tabControl.SelectedIndexChanged += OnTabControlSelectedIndexChanged;
            data.PropertyChanged += OnValuePropertyChanged;
        }

        /// <summary>
        /// Occurs when the selected repeat pattern changes.
        /// </summary>
        public event EventHandler? ValueChanged;

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
        /// Gets the <see cref="CompositeRepeatPatternRule"/> instance representing the selected repeat
        /// pattern and its associated rules.
        /// </summary>
        public virtual CompositeRepeatPatternRule Value => data;

        /// <summary>
        /// Gets or sets the selected repeat pattern.
        /// </summary>
        public virtual ScheduleRepeatPattern SelectedPattern
        {
            get
            {
                return data.Kind;
            }

            set
            {
                data.Kind = value;
            }
        }

        /// <summary>
        /// Called when the selected index of the inner tab control changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTabControlSelectedIndexChanged(object? sender, EventArgs e)
        {
            var index = tabControl.SelectedIndex;

            var newPattern = ScheduleRepeatPattern.None;

            if (index >= 0 && index <= (int)ScheduleRepeatPattern.Yearly)
                newPattern = (ScheduleRepeatPattern)index;

            tabControl.ContentVisible = newPattern != ScheduleRepeatPattern.None;

            SelectedPattern = newPattern;
        }

        /// <summary>
        /// Called when a property of the repeat pattern rule changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnValuePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            tabControl.SelectedIndex = (int)SelectedPattern;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CompositeRepeatPatternRule"/> class.
        /// </summary>
        /// <returns> A new instance of the <see cref="CompositeRepeatPatternRule"/> class. </returns>
        protected virtual CompositeRepeatPatternRule CreateRule() => new ();

        /// <summary>
        /// Creates a new instance of the <see cref="DailyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="DailyPatternPicker"/> control. </returns>
        protected virtual DailyPatternPicker CreateDailyPatternPicker() => new(data.DailyRule);

        /// <summary>
        /// Creates a new instance of the <see cref="WeeklyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="WeeklyPatternPicker"/> control. </returns>
        protected virtual WeeklyPatternPicker CreateWeeklyPatternPicker() => new(data.WeeklyRule);

        /// <summary>
        /// Creates a new instance of the <see cref="MonthlyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="MonthlyPatternPicker"/> control. </returns>
        protected virtual MonthlyPatternPicker CreateMonthlyPatternPicker() => new(data.MonthlyRule);

        /// <summary>
        /// Creates a new instance of the <see cref="YearlyPatternPicker"/> control.
        /// </summary>
        /// <returns> A new instance of the <see cref="YearlyPatternPicker"/> control. </returns>
        protected virtual YearlyPatternPicker CreateYearlyPatternPicker() => new(data.YearlyRule);
    }
}
