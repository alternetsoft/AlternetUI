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
                intervalDayPicker.SuffixText = TimePeriodUnit.Days.ToDisplayString(intervalDayPicker.Value);
            }
        }
    }
}
