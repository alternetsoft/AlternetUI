using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// A control that allows users to select a time period, represented by a numeric value and a unit of
    /// measurement (e.g., days, hours, minutes).
    /// </summary>
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
        public new TimePeriod Value
        {
            get => (TimePeriod?)SelectedItem ?? new TimePeriod(0, TimePeriodUnit.Minutes);
            set => SelectedItem = value;
        }

        /// <summary>
        /// Adds the default set of time period items to the picker, which can be overridden in derived classes.
        /// By default uses the <see cref="DefaultItems"/> array to populate the picker with common time periods.
        /// </summary>
        protected virtual void AddDefaultItems()
        {
            foreach (var item in DefaultItems)
            {
                Items.Add(item);
            }
        }
    }
}
