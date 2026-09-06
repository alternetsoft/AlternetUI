using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;
using Alternet.UI.Extensions;
using System.Linq;

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
            private readonly PanelSettings panel = new();

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

                GetDaysOfWeekWithTitles(firstDayOfWeek, out var weekdays, out var titles);

                IReadOnlyList<PanelSettingsItem> checkItems = [];

                panel.Horizontal((h) =>
                {
                    panel.Vertical((v) =>
                    {
                        checkItems = panel.AddFlagCheckBoxes<DaysOfWeek>(
                            label: null,
                            getValue: () => Value.WeekDays,
                            setValue: v => Value.WeekDays = v,
                            itemTitles: titles,
                            itemValues: weekdays,
                            e: null);
                    });

                    panel.Vertical((v) =>
                    {
                        var checkBoxes = Group(PanelSettingsItem.GetEditors(checkItems));

                        panel.AddLinkLabel(CommonStrings.Default.ButtonClearAll, () => checkBoxes.Checked(false));
                        panel.AddLinkLabel(CommonStrings.Default.ButtonSelectAll, () => checkBoxes.Checked(true));
                        panel.AddLinkLabel(CommonStrings.Default.ButtonSelectWeekdays, () =>
                        {
                            checkBoxes.Checked((c) => c.Tag is DaysOfWeek day && day.IsWeekday());
                        });
                        panel.AddLinkLabel(CommonStrings.Default.ButtonSelectWeekends, () =>
                        {
                            checkBoxes.Checked((c) => c.Tag is DaysOfWeek day && day.IsWeekend());
                        });
                    });
                });

                intervalWeekPicker.Parent = this;
                panel.Parent = this;
            }

            /// <summary>
            /// Gets the panel that contains the checkboxes for selecting the days of the week in the weekly repeat pattern.
            /// </summary>
            public PanelSettings WeekdaysPanel => panel;

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

            /// <summary>
            /// Enumerates the days of the week based on the specified first day of the week
            /// and returns the corresponding <see cref="DaysOfWeek"/> values and titles.
            /// </summary>
            /// <param name="firstDayOfWeek">The first day of the week.</param>
            /// <param name="weekdays">The array of <see cref="DaysOfWeek"/> values.</param>
            /// <param name="titles">The list of day names.</param>
            protected virtual void GetDaysOfWeekWithTitles(DayOfWeek firstDayOfWeek, out DaysOfWeek[] weekdays, out string[] titles)
            {
                DateUtils.GetDaysOfWeekWithTitles(
                    out weekdays,
                    out titles,
                    DayNamesKind.Full,
                    firstDayOfWeek,
                    FormatProvider);
            }
        }
    }
}
