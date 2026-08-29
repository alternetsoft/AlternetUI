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
    }
}
