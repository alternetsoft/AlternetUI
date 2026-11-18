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
    public class TimePeriod : BaseObject, IEquatable<TimePeriod>
    {
        /// <summary>
        /// Gets a <see cref="TimePeriod"/> instance that represents no specific time period (null value).
        /// </summary>
        public static TimePeriod None => new ImmutableTimePeriod(null);

        private string? text;
        private TimeSpan? data;
        private TimePeriodUnit zeroUnit = TimePeriodUnit.Minutes;
        private DateUtils.TimePeriodUnitTextFormat? unitFormat;
        private string? textFormat;

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
        /// Initializes a new instance of the <see cref="TimePeriod"/> class
        /// with the specified <see cref="TimeSpan"/> value and an optional text representation.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> value representing the time period.</param>
        /// <param name="text">The optional text representation of the time period.</param>
        public TimePeriod(TimeSpan? timeSpan, string? text)
        {
            data = timeSpan;
            this.text = text;
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
        /// Gets a value indicating whether the <see cref="TimePeriod"/> instance represents
        /// a null time period (no specific value).
        /// </summary>
        public bool IsNull => Value is null;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TimePeriod"/> instance represents a zero time period.
        /// </summary>
        public bool IsZero => Value == TimeSpan.Zero;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TimePeriod"/> instance has a non-null value.
        /// </summary>
        public bool HasValue => Value is not null;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TimePeriod"/> instance has a non-null and non-zero value.
        /// </summary>
        public bool HasNonZeroValue => Value is not null && Value != TimeSpan.Zero;

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
        public virtual DateUtils.TimePeriodUnitTextFormat? UnitFormat
        {
            get => unitFormat;
            set => unitFormat = value;
        }

        /// <summary>
        /// Gets or sets the optional text format for displaying the time period,
        /// allowing customization of how the time period is represented in string form.
        /// Value should be specified in the same way as in <see cref="string.Format(string, object[])"/>.
        /// Only the first argument is used, which is the <see cref="TimePeriod"/> instance itself.
        /// </summary>
        public virtual string? TextFormat
        {
            get => textFormat;
            set => textFormat = value;
        }

        /// <summary>
        /// Gets or sets an optional title or description for the time period,
        /// providing additional context or information.
        /// </summary>
        public virtual string Text
        {
            get
            {
                if (TextFormat is not null && TextFormat.Length > 0)
                {
                    return string.Format(TextFormat, InternalText());
                }

                return InternalText();

                string InternalText()
                {
                    if (text is not null && text.Length > 0)
                        return text;

                    if (Value is null)
                        return CommonStrings.Default.TimePeriodNone;

                    var fmt = UnitFormat ?? DateUtils.TimePeriodUnitTextFormat.Long;

                    if (Value == TimeSpan.Zero)
                        return DateUtils.ValueAndUnitToString(0, zeroUnit, fmt);

                    return DateUtils.ToDisplayString(Value.Value, fmt);
                }
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

        /// <summary>
        /// Assigns the values from another <see cref="TimePeriod"/> instance to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="TimePeriod"/> instance to copy values from.</param>
        public virtual void Assign(TimePeriod? other)
        {
            if (other is null)
            {
                Value = null;
                Text = string.Empty;
                UnitFormat = null;
                TextFormat = null;
                return;
            }

            Value = other.Value;
            Text = other.Text;
            UnitFormat = other.UnitFormat;
            TextFormat = other.TextFormat;
        }

        /// <summary>
        /// Creates a new <see cref="TimePeriod"/> instance that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="TimePeriod"/> instance that is a copy of the current instance.</returns>
        public virtual TimePeriod Clone()
        {
            var result = new TimePeriod();
            result.Assign(this);
            return result;
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
