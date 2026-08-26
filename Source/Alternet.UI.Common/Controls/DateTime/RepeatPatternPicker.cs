using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a repeat pattern for an event or task.
    /// </summary>
    public partial class RepeatPatternPicker : HiddenBorder
    {
        private readonly TabControl tabControl = new();
        private readonly DailyPatternPicker dailyPicker = new();
        private readonly WeeklyPatternPicker weeklyPicker = new();
        private readonly MonthlyPatternPicker monthlyPicker = new();
        private readonly YearlyPatternPicker yearlyPicker = new();
        private readonly HiddenBorder nonePicker = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternPicker"/> class.
        /// </summary>
        public RepeatPatternPicker()
        {
            tabControl.ActiveTabHasBorder = true;
            tabControl.TabHasBorder = true;
            tabControl.HasInteriorBorder = false;
            tabControl.ActiveTabTheme = SpeedButton.KnownTheme.StaticBorder;
            tabControl.TabTheme = SpeedButton.KnownTheme.None;
            tabControl.ContentVisible = false;

            tabControl.Parent = this;

            tabControl.Add("None", nonePicker);
            tabControl.Add("Daily", dailyPicker);
            tabControl.Add("Weekly", weeklyPicker);
            tabControl.Add("Monthly", monthlyPicker);
            tabControl.Add("Yearly", yearlyPicker);

            tabControl.SelectedIndexChanged += OnTabControlSelectedIndexChanged;

            HasBorder = true;
        }

        /// <summary>
        /// Occurs when the selected repeat pattern changes.
        /// </summary>
        public event EventHandler? SelectedPatternChanged;

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
        /// Represents a generic control for selecting a repeat pattern rule.
        /// </summary>
        /// <typeparam name="TValue">The type of the repeat pattern rule.</typeparam>
        public abstract partial class DateRepeatPatternRule<TValue> : HiddenBorder
            where TValue : DateRepeatPatternRule, new()
        {
            private TValue data = new();

            /// <summary>
            /// Gets or sets the value of the daily repeat pattern rule.
            /// </summary>
            public virtual TValue Value
            {
                get
                {
                    return data;
                }

                set
                {
                    value ??= new ();

                    if (data == value)
                        return;
                    data = value;
                }
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a daily repeat pattern for an event or task.
        /// </summary>
        public partial class DailyPatternPicker : DateRepeatPatternRule<DailyRepeatPatternRule>
        {

            /// <summary>
            /// Initializes a new instance of the <see cref="DailyPatternPicker"/> class.
            /// </summary>
            public DailyPatternPicker()
            {
                SuggestedHeight = 100;
            }
        }

        /// <summary>
        /// Represents a control that allows users to select a weekly repeat pattern for an event or task.
        /// </summary>
        public partial class WeeklyPatternPicker : DateRepeatPatternRule<WeeklyRepeatPatternRule>
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
        public partial class MonthlyPatternPicker : DateRepeatPatternRule<MonthlyRepeatPatternRule>
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
        public partial class YearlyPatternPicker : DateRepeatPatternRule<YearlyRepeatPatternRule>
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
