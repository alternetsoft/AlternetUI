using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// A control that allows users to select a relative occurrence of a weekday within a month,
    /// combining the relative weekday, day of the week, and month selections.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class RelativeWeekdayOfMonthPicker : TransparentPanel
    {
        /// <summary>
        /// The default margin between the label and the picker control.
        /// </summary>
        public static float DefaultLabelAndPickerMargin = 5;

        private readonly RelativeWeekdayPicker relativeWeekdayPicker;
        private readonly DayOfWeekPicker dayOfWeekPicker;
        private readonly Label dayOfWeekAndMonthSeparatorLabel = new();
        private readonly MonthPicker monthPicker;
        private readonly Label prefixLabel = new();
        private readonly TransparentPanel firstPanel = new();
        private readonly TransparentPanel secondPanel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeWeekdayOfMonthPicker"/> class.
        /// </summary>
        public RelativeWeekdayOfMonthPicker()
        {
            Layout = LayoutStyle.Horizontal;

            prefixLabel.Text = CommonStrings.Default.OnThePrefix;
            prefixLabel.VerticalAlignment = VerticalAlignment.Center;
            prefixLabel.InputTransparent = true;

            firstPanel.InputTransparent = true;
            secondPanel.InputTransparent = true;

            relativeWeekdayPicker = new RelativeWeekdayPicker();

            dayOfWeekPicker = new DayOfWeekPicker();
            dayOfWeekPicker.SetExtendedItemsVisibility(true);

            dayOfWeekAndMonthSeparatorLabel.Text = CommonStrings.Default.DayOfWeekAndMonthSeparator;
            dayOfWeekAndMonthSeparatorLabel.VerticalAlignment = VerticalAlignment.Center;
            dayOfWeekAndMonthSeparatorLabel.InputTransparent = true;

            monthPicker = new MonthPicker();

            firstPanel.Layout = LayoutStyle.Horizontal;
            secondPanel.Layout = LayoutStyle.Horizontal;

            prefixLabel.MarginRight = DefaultLabelAndPickerMargin;
            dayOfWeekAndMonthSeparatorLabel.Margin = (DefaultLabelAndPickerMargin, 0, DefaultLabelAndPickerMargin, 0);

            prefixLabel.Parent = firstPanel;
            relativeWeekdayPicker.Parent = firstPanel;
            dayOfWeekPicker.Parent = firstPanel;
            dayOfWeekAndMonthSeparatorLabel.Parent = secondPanel;
            monthPicker.Parent = secondPanel;

            firstPanel.Parent = this;
            secondPanel.Parent = this;
        }

        /// <summary>
        /// Occurs when the selected relative weekday changes, allowing external handlers to respond to the change.
        /// </summary>
        public event EventHandler? RelativeWeekdayChanged
        {
            add
            {
                relativeWeekdayPicker.ValueChanged += value;
            }

            remove
            {
                relativeWeekdayPicker.ValueChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the selected day of the week changes, allowing external handlers to respond to the change.
        /// </summary>
        public event EventHandler? DayOfWeekChanged
        {
            add
            {
                dayOfWeekPicker.ValueChanged += value;
            }
            remove
            {
                dayOfWeekPicker.ValueChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the selected month changes, allowing external handlers to respond to the change.
        /// </summary>
        public event EventHandler? MonthChanged
        {
            add
            {
                monthPicker.ValueChanged += value;
            }

            remove
            {
                monthPicker.ValueChanged -= value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the month picker.
        /// </summary>
        public virtual bool IsMonthVisible
        {
            get
            {
                return monthPicker.Visible;
            }

            set
            {
                dayOfWeekAndMonthSeparatorLabel.Visible = value;
                monthPicker.Visible = value;
            }
        }

        /// <summary>
        /// Gets the first panel that contains the prefix label, relative weekday picker, and day of the week picker.
        /// </summary>
        public TransparentPanel FirstPanel => firstPanel;

        /// <summary>
        /// Gets the second panel that contains the separator label and the month picker.
        /// </summary>
        public TransparentPanel SecondPanel => secondPanel;

        /// <summary>
        /// Gets the currently selected <see cref="RelativeWeekdayOfMonth"/> value,
        /// which combines the selected relative weekday, day of the week, and month.
        /// </summary>
        public RelativeWeekdayOfMonth Value
        {
            get
            {
                return new RelativeWeekdayOfMonth(
                    relativeWeekdayPicker.Value,
                    dayOfWeekPicker.Value,
                    monthPicker.Value);
            }

            set
            {
                relativeWeekdayPicker.Value = value.RelativeWeekday;
                dayOfWeekPicker.Value = value.DayOfWeek;
                monthPicker.Value = value.Month;
            }
        }

        /// <summary>
        /// Gets the <see cref="RelativeWeekdayPicker"/> control used to select the relative weekday occurrence.
        /// </summary>
        [Browsable(false)]
        public RelativeWeekdayPicker RelativeWeekdayPicker => relativeWeekdayPicker;

        /// <summary>
        /// Gets the <see cref="DayOfWeekPicker"/> control used to select the day of the week.
        /// </summary>
        [Browsable(false)]
        public DayOfWeekPicker DayOfWeekPicker => dayOfWeekPicker;

        /// <summary>
        /// Gets the <see cref="Label"/> control used as a prefix before the relative weekday, day of the week, and month pickers.
        /// </summary>
        [Browsable(false)]
        public Label PrefixLabel => prefixLabel;

        /// <summary>
        /// Gets the <see cref="Label"/> control used as a separator between the day of the week and month pickers.
        /// </summary>
        [Browsable(false)]
        public Label DayOfWeekAndMonthSeparatorLabel => dayOfWeekAndMonthSeparatorLabel;

        /// <summary>
        /// Gets the <see cref="MonthPicker"/> control used to select the month.
        /// </summary>
        [Browsable(false)]
        public MonthPicker MonthPicker => monthPicker;
    }
}
