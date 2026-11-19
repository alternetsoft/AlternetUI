using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;
using Alternet.UI.Extensions;
using System.Linq;

namespace Alternet.UI
{
    public partial class RepeatPatternPicker : IReadOnlyProperty
    {
        /// <summary>
        /// Represents a control that allows users to select a weekly repeat pattern for an event or task.
        /// </summary>
        [ControlCategory(KnownControlCategory.DateTime)]
        public partial class WeeklyPatternPicker : DateRepeatPatternRulePicker<WeeklyRepeatPatternRule>
        {
            private readonly XIntPickerWithLabels intervalWeekPicker = new();
            private readonly PanelSettings panel = new();
            private IReadOnlyList<PanelSettingsItem> checkItems = [];

            /// <summary>
            /// Initializes a new instance of the <see cref="WeeklyPatternPicker"/> class.
            /// </summary>
            public WeeklyPatternPicker(WeeklyRepeatPatternRule data) : base(data)
            {
                Layout = LayoutStyle.Vertical;

                intervalWeekPicker.PrefixLabel.MarginLeft = XCheckBox.DefaultCheckBoxMargin.Left + 3;
                intervalWeekPicker.PrefixText = CommonStrings.Default.DateRepeatPatternPrefixLabelEvery;
                intervalWeekPicker.Minimum = 1;

                var firstDayOfWeek = DateUtils.SystemFirstDayOfWeek;

                GetDaysOfWeekWithTitles(firstDayOfWeek, out var weekdays, out var titles);

                var weekdayParts = ArrayUtils.SplitArray(weekdays, 2).ToArray();
                var titleParts = ArrayUtils.SplitArray(titles, 2).ToArray();

                List<PanelSettingsItem> allCheckItems = new();

                void AddColumn(int index)
                {
                    var pnl = panel.Vertical((v) =>
                    {
                        allCheckItems.AddRange(panel.AddFlagCheckBoxes<DaysOfWeek>(
                            label: null,
                            getValue: () => Value.WeekDays,
                            setValue: v => Value.WeekDays = v,
                            itemTitles: titleParts[index],
                            itemValues: weekdayParts[index],
                            e: null));
                    });
                }

                panel.Horizontal((v) =>
                {
                    AddColumn(0);
                    v.Add<Spacer>().SuggestedWidth = 5;
                    AddColumn(1);
                });

                checkItems = allCheckItems;

                foreach (var checkBox in GetCheckBoxes())
                {
                    checkBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    checkBox.PreferredSizeMode = PreferredSizeMode.Content;
                }

                panel.Horizontal((h) =>
                {
                    panel.AddLabel(Localized.ButtonSelect + " ").Editor?.VerticalAlignment = VerticalAlignment.Center;

                    var c1 = panel.AddLinkLabel(Localized.ButtonNone, () => GetCheckBoxes().Checked(false)).Editor;
                    var c2 = panel.AddLinkLabel(Localized.ButtonAll, () => GetCheckBoxes().Checked(true)).Editor;
                    var c3 = panel.AddLinkLabel(Localized.ButtonWeekdays, () =>
                    {
                        GetCheckBoxes().Checked((c) => c.Tag is DaysOfWeek day && day.IsWeekday());
                    }).Editor;
                    var c4 = panel.AddLinkLabel(Localized.ButtonWeekends, () =>
                    {
                        GetCheckBoxes().Checked((c) => c.Tag is DaysOfWeek day && day.IsWeekend());
                    }).Editor;

                    Group(c1!, c2!, c3!, c4!).HasBorder(false);
                });

                intervalWeekPicker.Parent = this;
                panel.Parent = this;

                ValueToControls();

                intervalWeekPicker.ValueChanged += OnIntervalWeekPickerValueChanged;
            }

            /// <summary>
            /// Gets or sets a value indicating whether the control is read-only.
            /// </summary>
            public virtual bool ReadOnly
            {
                get => intervalWeekPicker.IntPicker.ReadOnly;
                set
                {
                    intervalWeekPicker.IntPicker.ReadOnly = value;
                    panel.ReadOnly = value;
                    foreach (var checkBox in GetCheckBoxes())
                    {
                        if (checkBox is XCheckBox ch)
                        {
                            ch.ReadOnly = value;
                        }
                    }
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether the values of the inner controls are transparent.
            /// </summary>
            public virtual bool ValuesTransparent
            {
                get
                {
                    return intervalWeekPicker.IntPicker.TextPicker.TextTransparent;
                }

                set
                {
                    if (ValuesTransparent == value)
                        return;
                    intervalWeekPicker.IntPicker.TextPicker.TextTransparent = value;
                }
            }

            /// <summary>
            /// Gets the panel that contains the checkboxes for selecting the days
            /// of the week in the weekly repeat pattern.
            /// </summary>
            public PanelSettings WeekdaysPanel => panel;

            /// <summary>
            /// Gets the integer picker control for selecting the interval
            /// week value in the weekly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public XIntPickerWithLabels IntervalWeekPicker => intervalWeekPicker;

            /// <summary>
            /// Gets the label that displays the suffix text for the
            /// interval week picker in the weekly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public Label IntervalWeekSuffixLabel => intervalWeekPicker.SuffixLabel;

            /// <summary>
            /// Gets the label that displays the prefix text for the interval
            /// week picker in the weekly repeat pattern.
            /// </summary>
            [Browsable(false)]
            public Label IntervalWeekPrefixLabel => intervalWeekPicker.PrefixLabel;

            /// <summary>
            /// Called to update the controls based on the current value of the weekly repeat pattern rule.
            /// </summary>
            public virtual void ValueToControls()
            {
                intervalWeekPicker.Value = Value.IntervalWeeks;
                UpdateSuffixLabelText();

                foreach (var checkBox in GetCheckBoxes())
                {
                    if (checkBox is XCheckBox ch && ch.Tag is DaysOfWeek dof)
                    {
                        ch.IsChecked = Value.WeekDays.HasFlag(dof);
                    }
                }
            }

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
            /// Gets the control set containing the checkboxes for selecting the days of the week.
            /// </summary>
            /// <returns>The control set containing the checkboxes.</returns>
            protected virtual ControlSet<XCheckBox> GetCheckBoxes()
            {
                var checkBoxes = new ControlSet<XCheckBox>(PanelSettingsItem.GetEditors<XCheckBox>(checkItems));
                return checkBoxes;
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
            protected virtual void GetDaysOfWeekWithTitles(
                DayOfWeek firstDayOfWeek,
                out DaysOfWeek[] weekdays,
                out string[] titles)
            {
                DateUtils.GetDaysOfWeekWithTitles(
                    out weekdays,
                    out titles,
                    DayNamesKind.Abbreviated,
                    firstDayOfWeek,
                    FormatProvider);
            }
        }
    }
}
