using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a time period defined by a numeric value and a unit of measurement.
    /// </summary>
    public struct TimePeriod
    {
        private string? text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriod"/> struct with the specified value and unit.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        public TimePeriod(int value, TimePeriodUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriod"/> struct with default values (0 days).
        /// </summary>
        public TimePeriod()
        {
            Value = 0;
            Unit = TimePeriodUnit.Days;
        }

        /// <summary>
        /// Gets or sets the numeric value representing the magnitude of the time period.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the unit of measurement for the time period, indicating whether it is in years,
        /// months, weeks, days, hours, minutes, seconds, or milliseconds.
        /// </summary>
        public TimePeriodUnit Unit { get; set; }

        /// <summary>
        /// Gets or sets the optional text format for displaying the time period,
        /// allowing customization of how the unit is represented in string form.
        /// </summary>
        public DateUtils.TimePeriodUnitTextFormat? TextFormat { get; set; }

        /// <summary>
        /// Gets or sets an optional title or description for the time period, providing additional context or information.
        /// </summary>
        public string Text
        {
            get
            {
                return text ?? DateUtils.ToDisplayString(Unit, Value, TextFormat ?? DateUtils.TimePeriodUnitTextFormat.Long);
            }

            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Converts the time period to a <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> representing the time period.</returns>
        public readonly TimeSpan AsTimeSpan()
        {
            switch (Unit)
            {
                case TimePeriodUnit.Years:
                    return TimeSpan.FromDays(Value * 365);
                case TimePeriodUnit.Months:
                    return TimeSpan.FromDays(Value * 30);
                case TimePeriodUnit.Weeks:
                    return TimeSpan.FromDays(Value * 7);
                case TimePeriodUnit.Days:
                    return TimeSpan.FromDays(Value);
                case TimePeriodUnit.Hours:
                    return TimeSpan.FromHours(Value);
                case TimePeriodUnit.Minutes:
                    return TimeSpan.FromMinutes(Value);
                case TimePeriodUnit.Seconds:
                    return TimeSpan.FromSeconds(Value);
                case TimePeriodUnit.Milliseconds:
                    return TimeSpan.FromMilliseconds(Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
