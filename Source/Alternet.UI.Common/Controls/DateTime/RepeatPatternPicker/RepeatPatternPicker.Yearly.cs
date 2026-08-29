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
            public YearlyPatternPicker(YearlyRepeatPatternRule data) : base(data)
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

                XRadioButton[] radioGroup = { dayOfMonthRadioButton.MainControl, relativeWeekdayRadioButton.MainControl };

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
            [Browsable(false)]
            public XIntPickerWithLabels IntervalYearPicker => intervalYearPicker;

            /// <summary>
            /// Gets the radio button for selecting the "Day of Month" repeat pattern in the yearly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix DayOfMonthRadioButton => dayOfMonthRadioButton;

            /// <summary>
            /// Gets the radio button for selecting the "Relative Weekday" repeat pattern in the yearly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XRadioButtonAndSuffix RelativeWeekdayRadioButton => relativeWeekdayRadioButton;

            /// <summary>
            /// Gets the <see cref="RelativeWeekdayOfMonthPicker"/> control used to select the
            /// relative weekday of the month in the yearly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public RelativeWeekdayOfMonthPicker RelativeWeekdayOfMonthPicker => relativeWeekdayOfMonthPicker;

            /// <summary>
            /// Gets the <see cref="MonthAndDayPicker"/> control used to select the month and day in the yearly repeat pattern.
            /// </summary>
            [Browsable(false)]
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
                intervalYearPicker.SuffixLabel.Text = TimePeriodUnit.Years.ToDisplayString(intervalYearPicker.Value);
            }
        }
    }
}
