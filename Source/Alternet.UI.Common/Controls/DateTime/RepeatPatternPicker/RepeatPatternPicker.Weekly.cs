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
            public WeeklyPatternPicker(WeeklyRepeatPatternRule data) : base(data)
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
                intervalWeekPicker.SuffixLabel.Text = TimePeriodUnit.Weeks.ToDisplayString(intervalWeekPicker.Value);
            }
        }
    }
}
