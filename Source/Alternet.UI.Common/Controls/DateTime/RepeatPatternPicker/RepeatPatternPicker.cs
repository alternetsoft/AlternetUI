using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
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
        /// Gets or sets a value indicating whether the drop-down image for the combo controls is shown by default.
        /// </summary>
        public static bool DefaultShowDropDownImage = false;

        /// <summary>
        /// Gets or sets the default date format used for displaying dates in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static string DefaultDateFormat = "D";

        private IFormatProvider? formatProvider;

        /// <summary>
        /// Gets the default padding for the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public static readonly Thickness DefaultPadding = 5;

        private readonly DateTimePicker startDatePicker = new();
        private readonly GenericControlAndLabel<DatePicker, Label> endDatePicker = new();
        private readonly BoldLabel startDateLabel;
        private readonly BoldLabel endDateLabel;
        private readonly XRadioButtonAndSuffix endsNeverRadioButton;
        private readonly XRadioButtonAndSuffix endsOnRadioButton;
        private readonly XRadioButtonAndSuffix endsAfterOccurrenceRadioButton;
        private readonly XIntPickerWithLabels occurrencePicker = new();

        private readonly TabControl tabControl = new();
        private readonly DailyPatternPicker dailyPicker;
        private readonly WeeklyPatternPicker weeklyPicker;
        private readonly MonthlyPatternPicker monthlyPicker;
        private readonly YearlyPatternPicker yearlyPicker;
        private readonly HiddenBorder nonePicker = new();
        private readonly RepeatPatternRule data;
        private readonly Label repeatLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPatternPicker"/> class.
        /// </summary>
        public RepeatPatternPicker()
        {
            Layout = LayoutStyle.Vertical;
            Padding = DefaultPadding;
            MinChildMargin = 5;

            data = CreateRule();

            // Start on date

            startDateLabel = Add<BoldLabel>(CommonStrings.Default.Starts);

            startDatePicker.Kind = DateTimePickerKind.DateTime;
            startDatePicker.DatePicker.ImageVisible = DefaultShowDropDownImage;
            startDatePicker.Parent = this;

            // End on date

            endDateLabel = Add<BoldLabel>(CommonStrings.Default.Ends);

            endDatePicker.Label.Text = CommonStrings.Default.OnPrefix;
            endDatePicker.Label.InputTransparent = true;
            endDatePicker.MainControl.ImageVisible = DefaultShowDropDownImage;
            
            endsOnRadioButton = new(endDatePicker);
            endsOnRadioButton.Parent = this;
            endDatePicker.Click += (s, e) => endsOnRadioButton.IsChecked = true;

            // Ends after cccurrence

            occurrencePicker.PrefixText = CommonStrings.Default.After;

            endsAfterOccurrenceRadioButton = new(occurrencePicker);
            endsAfterOccurrenceRadioButton.Parent = this;
            occurrencePicker.Click += (s, e) => endsAfterOccurrenceRadioButton.IsChecked = true;

            // Ends never

            endsNeverRadioButton = new();
            endsNeverRadioButton.SuffixControl.Text = CommonStrings.Default.Never;
            endsNeverRadioButton.Parent = this;

            // Other initializations

            XRadioButton[] radioButtons
                = { endsOnRadioButton.MainControl, endsAfterOccurrenceRadioButton.MainControl, endsNeverRadioButton.MainControl };

            endsOnRadioButton.MainControl.RadioSiblings = radioButtons;
            endsAfterOccurrenceRadioButton.MainControl.RadioSiblings = radioButtons;
            endsNeverRadioButton.MainControl.RadioSiblings = radioButtons;

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

            Add<HorizontalLine>();

            repeatLabel = Add<BoldLabel>(CommonStrings.Default.Repeat);

            tabControl.Parent = this;

            tabControl.SelectedIndexChanged += OnTabControlSelectedIndexChanged;
            data.PropertyChanged += OnValuePropertyChanged;

            DateFormat = DefaultDateFormat;

            // DataToControls

            ValueToControls();

            // Event handlers for updating the repeat pattern rule based on user interactions with the controls

            startDatePicker.ValueChanged += (s, e) =>
            {
                data.StartDate = startDatePicker.AsDateOnlyOrToday;
            };
            endDatePicker.MainControl.ValueChanged += (s, e) =>
            {
                data.EndDate = endDatePicker.MainControl.AsDateOnlyOrToday;
            };
            occurrencePicker.ValueChanged += (s, e) =>
            {
                data.OccurrenceCount = occurrencePicker.Value;
                UpdateOccurrenceText();
            };
            endsNeverRadioButton.CheckedChanged += (s, e) =>
            {
                if (endsNeverRadioButton.IsChecked)
                {
                    data.EndCondition = DateRepeatPatternRule.EndConditionKind.Never;
                }
            };
            endsOnRadioButton.CheckedChanged += (s, e) =>
            {
                if (endsOnRadioButton.IsChecked)
                {
                    data.EndCondition = DateRepeatPatternRule.EndConditionKind.OnDate;
                }
            };
            endsAfterOccurrenceRadioButton.CheckedChanged += (s, e) =>
            {
                if (endsAfterOccurrenceRadioButton.IsChecked)
                {
                    data.EndCondition = DateRepeatPatternRule.EndConditionKind.AfterOccurrence;
                }
            };
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
        /// Gets or sets the format provider used for culture-specific formatting of date and time values.
        /// </summary>
        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set
            {
                if (value == formatProvider) return;

                formatProvider = value;
                startDatePicker.FormatProvider = value;
                endDatePicker.MainControl.FormatProvider = value;
                dailyPicker.FormatProvider = value;
                weeklyPicker.FormatProvider = value;
                monthlyPicker.FormatProvider = value;
                yearlyPicker.FormatProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the start date picker allows selection of both date and time.
        /// </summary>
        public virtual bool CanSelectTime
        {
            get => startDatePicker.Kind == DateTimePickerKind.DateTime;
            set
            {
                startDatePicker.Kind = value ? DateTimePickerKind.DateTime : DateTimePickerKind.Date;
            }
        }

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
        /// Gets the <see cref="RepeatPatternRule"/> instance representing the selected repeat
        /// pattern and its associated rules.
        /// </summary>
        public virtual RepeatPatternRule Value => data;

        /// <summary>
        /// Gets or sets the date format used for displaying dates in the <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public virtual string? DateFormat
        {
            get => startDatePicker.Format;
            set
            {
                startDatePicker.Format = value;
                endDatePicker.MainControl.Format = value;
            }
        }

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
        /// Called to update the control values based on the current state of the <see cref="RepeatPatternRule"/> instance.
        /// </summary>
        protected virtual void ValueToControls()
        {
            tabControl.SelectedIndex = (int)SelectedPattern;
            startDatePicker.AsDateOnly = data.StartDate;
            endDatePicker.MainControl.AsDateOnly = data.EndDate;

            endsOnRadioButton.IsChecked = data.EndCondition == DateRepeatPatternRule.EndConditionKind.OnDate;
            endsAfterOccurrenceRadioButton.IsChecked = data.EndCondition == DateRepeatPatternRule.EndConditionKind.AfterOccurrence;
            endsNeverRadioButton.IsChecked = data.EndCondition == DateRepeatPatternRule.EndConditionKind.Never;

            occurrencePicker.Value = data.OccurrenceCount;
            UpdateOccurrenceText();
        }

        /// <summary>
        /// Updates the suffix text of the occurrence picker based on the current occurrence count.
        /// </summary>
        protected virtual void UpdateOccurrenceText()
        {
            occurrencePicker.SuffixText = data.OccurrenceCount == 1
                ? CommonStrings.Default.Occurrence : CommonStrings.Default.Occurrences;
        }

        /// <summary>
        /// Called when a property of the repeat pattern rule changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnValuePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValueToControls();
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RepeatPatternRule"/> class.
        /// </summary>
        /// <returns> A new instance of the <see cref="RepeatPatternRule"/> class. </returns>
        protected virtual RepeatPatternRule CreateRule() => new();

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

    /// <summary>
    /// Represents a scrollable version of the <see cref="RepeatPatternPicker"/> control.
    /// </summary>
    public class ScrollableRepeatPatternPicker : ScrollViewer<RepeatPatternPicker>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableRepeatPatternPicker"/> class.
        /// </summary>
        public ScrollableRepeatPatternPicker()
        {
            ScrolledControl.HasBorder = false;
        }

        /// <summary>
        /// Occurs when the selected repeat pattern changes in the inner <see cref="RepeatPatternPicker"/> control.
        /// </summary>
        public event EventHandler? ValueChanged
        {
            add => ScrolledControl.ValueChanged += value;
            remove => ScrolledControl.ValueChanged -= value;
        }

        /// <summary>
        /// Gets the <see cref="RepeatPatternRule"/> instance representing the selected repeat pattern and its associated rules.
        /// </summary>
        public RepeatPatternRule Value => ScrolledControl.Value;
    }
}
