using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    public partial class RepeatPatternPicker
    {
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
            public MonthlyPatternPicker(MonthlyRepeatPatternRule data) : base(data)
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
            /// Gets the <see cref="XIntPickerWithLabels"/> control used to
            /// select the interval in months for the monthly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XIntPickerWithLabels IntervalMonthPicker => intervalMonthPicker;

            /// <summary>
            /// Gets the <see cref="RelativeWeekdayOfMonthPicker"/> control used to
            /// select the day of the month for the monthly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public RelativeWeekdayOfMonthPicker RelativeWeekdayOfMonthPicker => relativeWeekdayOfMonthPicker;

            /// <summary>
            /// Gets the <see cref="XRadioButtonAndSuffix"/> control used to select
            /// the "Day of Month" option for the monthly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix DayOfMonthRadioButton => dayOfMonthRadioButton;

            /// <summary>
            /// Gets the <see cref="XRadioButtonAndSuffix"/> control used to select
            /// the "Relative Weekday" option for the monthly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix RelativeWeekdayRadioButton => relativeWeekdayRadioButton;
            
            /// <summary>
            /// Gets the <see cref="XIntPickerWithLabels"/> control used to select
            /// the day of the month for the monthly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XIntPickerWithLabels DayOfMonthPicker => dayOfMonthPicker;

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
                intervalMonthPicker.SuffixLabel.Text = TimePeriodUnit.Months.ToDisplayString(intervalMonthPicker.Value);
            }
        }
    }
}
