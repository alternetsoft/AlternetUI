using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a time period defined by a numeric value and a unit of measurement.
    /// </summary>
    public class TimePeriod : IEquatable<TimePeriod>
    {
        /// <summary>
        /// Gets a <see cref="TimePeriod"/> instance that represents no specific time period (null value).
        /// </summary>
        public static TimePeriod None => new ImmutableTimePeriod(null);

        private string? text;
        private TimeSpan? data;
        private TimePeriodUnit zeroUnit = TimePeriodUnit.Minutes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriod"/> class with the specified value and unit.
        /// </summary>
        /// <param name="value">The numeric value representing the magnitude of the time period.</param>
        /// <param name="unit">The unit of measurement for the time period.</param>
        public TimePeriod(int value, TimePeriodUnit unit)
        {
            data = DateUtils.AsTimeSpan(value, unit);
            this.zeroUnit = unit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriod"/> class
        /// with the specified <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="timeSpan"></param>
        public TimePeriod(TimeSpan? timeSpan)
        {
            data = timeSpan;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriod"/> class with default values (0 minutes).
        /// </summary>
        public TimePeriod()
            : this(TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Gets or sets the unit of measurement for the time period when the value is zero.
        /// </summary>
        public TimePeriodUnit ZeroUnit
        {
            get => zeroUnit;
            set => zeroUnit = value;
        }

        /// <summary>
        /// Gets or sets the value representing the time period.
        /// </summary>
        public virtual TimeSpan? Value
        {
            get => data;
            set
            {
                if (data == value)
                    return;
                data = value;
            }
        }

        /// <summary>
        /// Gets or sets the optional text format for displaying the time period,
        /// allowing customization of how the unit is represented in string form.
        /// </summary>
        public virtual DateUtils.TimePeriodUnitTextFormat? TextFormat { get; set; }

        /// <summary>
        /// Gets or sets an optional title or description for the time period,
        /// providing additional context or information.
        /// </summary>
        public virtual string Text
        {
            get
            {
                if (text is not null)
                    return text;

                if (Value is null)
                    return CommonStrings.Default.TimePeriodNone;

                var fmt = TextFormat ?? DateUtils.TimePeriodUnitTextFormat.Long;

                if (Value == TimeSpan.Zero)
                    return DateUtils.ValueAndUnitToString(0, zeroUnit, fmt);

                return text ?? DateUtils.ToDisplayString(Value.Value, fmt);
            }

            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Defines the equality operator for comparing two <see cref="TimePeriod"/>
        /// instances based on their value and unit.
        /// </summary>
        /// <param name="left">The first <see cref="TimePeriod"/> to compare.</param>
        /// <param name="right">The second <see cref="TimePeriod"/> to compare.</param>
        /// <returns><c>true</c> if the two <see cref="TimePeriod"/> instances are equal;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator ==(TimePeriod left, TimePeriod right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Defines the inequality operator for comparing two <see cref="TimePeriod"/>
        /// instances based on their value and unit.
        /// </summary>
        /// <param name="left">The first <see cref="TimePeriod"/> to compare.</param>
        /// <param name="right">The second <see cref="TimePeriod"/> to compare.</param>
        /// <returns><c>true</c> if the two <see cref="TimePeriod"/> instances are not equal;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator !=(TimePeriod left, TimePeriod right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Text;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is TimePeriod period && Equals(period);
        }

        /// <inheritdoc />
        public virtual bool Equals(TimePeriod? other)
        {
            return other is not null && Value == other.Value;
        }
    }

    /// <summary>
    /// Represents an immutable version of the <see cref="TimePeriod"/> class,
    /// which cannot be modified after creation.
    /// </summary>
    public class ImmutableTimePeriod : TimePeriod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableTimePeriod"/> class
        /// with the specified <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> value of the time period.</param>
        public ImmutableTimePeriod(TimeSpan? timeSpan)
            : base(timeSpan)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableTimePeriod"/> class
        /// with the specified value and unit.
        /// </summary>
        /// <param name="value">The numeric value of the time period.</param>
        /// <param name="unit">The unit of measurement for the time period.</param>
        public ImmutableTimePeriod(int value, TimePeriodUnit unit)
            : base(value, unit)
        {
        }

        /// <inheritdoc/>
        public override TimeSpan? Value
        {
            get => base.Value;
            set
            {
                throw new InvalidOperationException("Cannot modify an immutable TimePeriod.");
            }
        }
    }
}
