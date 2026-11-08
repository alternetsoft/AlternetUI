using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// A control that allows users to select a time period, represented by a numeric value and a unit of
    /// measurement (e.g., days, hours, minutes).
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class TimePeriodPicker : ListPicker
    {
        /// <summary>
        /// Specifies the default set of time period items to be displayed in the picker,
        /// covering a range of common durations from minutes to weeks.
        /// </summary>
        public static TimePeriod[] DefaultItems =
        [
            new (0, TimePeriodUnit.Minutes),
            new (5, TimePeriodUnit.Minutes),
            new (10, TimePeriodUnit.Minutes),
            new (15, TimePeriodUnit.Minutes),
            new (30, TimePeriodUnit.Minutes),

            new (1, TimePeriodUnit.Hours),
            new (2, TimePeriodUnit.Hours),
            new (3, TimePeriodUnit.Hours),
            new (4, TimePeriodUnit.Hours),
            new (5, TimePeriodUnit.Hours),
            new (6, TimePeriodUnit.Hours),
            new (7, TimePeriodUnit.Hours),
            new (8, TimePeriodUnit.Hours),
            new (9, TimePeriodUnit.Hours),
            new (10, TimePeriodUnit.Hours),
            new (11, TimePeriodUnit.Hours),
            new (12, TimePeriodUnit.Hours),
            new (18, TimePeriodUnit.Hours),

            new (1, TimePeriodUnit.Days),
            new (2, TimePeriodUnit.Days),
            new (3, TimePeriodUnit.Days),
            new (4, TimePeriodUnit.Days),

            new (1, TimePeriodUnit.Weeks),
            new (2, TimePeriodUnit.Weeks),
        ];

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriodPicker"/> class,
        /// which allows users to select a time period from a predefined list of durations.
        /// </summary>
        public TimePeriodPicker()
        {
            AddDefaultItems();
            Value = new TimePeriod(15, TimePeriodUnit.Minutes);
        }

        /// <summary>
        /// Gets or sets the currently selected time period, represented by a numeric value and a unit of measurement.
        /// </summary>
        public new virtual TimePeriod Value
        {
            get => (TimePeriod?)SelectedItem ?? new TimePeriod(0, TimePeriodUnit.Minutes);
            set => SelectedItem = value;
        }

        /// <summary>
        /// Selects the "None" item in the picker,
        /// allowing users to indicate that no specific time period is chosen.
        /// </summary>
        public virtual void SelectNoneItem()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is TimePeriod timePeriod && timePeriod.Value is null)
                {
                    Value = timePeriod;
                    return;
                }
            }
        }

        /// <summary>
        /// Inserts a "None" item at the beginning of the picker,
        /// allowing users to select a state where no time period is specified.
        /// </summary>
        public virtual TimePeriod AddNoneItem()
        {
            Items.Insert(0, TimePeriod.None);
            return TimePeriod.None;
        }

        /// <summary>
        /// Determines whether the default set of time period items should include an item with a value of zero.
        /// </summary>
        /// <returns></returns>
        public virtual bool AllowZeroInDefaultItems()
        {
            return true;
        }

        /// <summary>
        /// Adds the default set of time period items to the picker, which can be overridden in derived classes.
        /// By default uses the <see cref="DefaultItems"/> array to populate the picker with common time periods.
        /// </summary>
        public virtual void AddDefaultItems()
        {
            var allowZero = AllowZeroInDefaultItems();

            foreach (var item in DefaultItems)
            {
                if (item.Value is null)
                    continue;

                if (!allowZero && item.Value.Value == TimeSpan.Zero)
                    continue;
                Items.Add(item);
            }
        }
    }

    /// <summary>
    /// Represents a specialized button control that allows users to select a snooze time period from
    /// a predefined list of durations.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class SnoozeSpeedButton : TimePeriodPicker
    {
        private string? labelText;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnoozeSpeedButton"/> class.
        /// </summary>
        public SnoozeSpeedButton()
        {
            ImageVisible = false;
            PopupWindowTitle = CommonStrings.Default.WindowTitleRemindMeIn;
        }

        /// <summary>
        /// Gets or sets the text label displayed on the button, which can be customized to indicate the snooze action.
        /// If not set, a default label text is used.
        /// </summary>
        public virtual string? LabelText
        {
            get => labelText;
            set => labelText = value;
        }

        /// <inheritdoc/>
        protected override string CoerceLabelText(string text)
        {
            return labelText ?? CommonStrings.Default.ButtonSnooze;
        }

        /// <inheritdoc/>
        public override bool UseSpeedButtonTheme()
        {
            return true;
        }

        /// <inheritdoc/>
        public override bool AllowZeroInDefaultItems()
        {
            return false;
        }
    }

}
