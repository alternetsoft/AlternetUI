using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a restricted date structure that allows setting minimum and maximum date limits.
    /// </summary>
    public partial struct RestrictedDate
    {
        private readonly Action<DateOnly> valueSetter;
        private readonly Func<DateOnly> valueGetter;
        private readonly Action<DateOnly, DateOnly>? setRange;

        private DateOnly max = DateOnly.MaxValue;
        private DateOnly min = DateOnly.MinValue;
        private bool useMinDate = false;
        private bool useMaxDate = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictedDate"/> class.
        /// </summary>
        public RestrictedDate(
            Func<DateOnly> valueGetter,
            Action<DateOnly> valueSetter,
            Action<DateOnly, DateOnly>? setRange)
        {
            this.valueSetter = valueSetter;
            this.valueGetter = valueGetter;
            this.setRange = setRange;
        }

        /// <summary>
        /// Gets or sets the format provider used for culture-specific formatting of date and time values.
        /// </summary>
        public IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets whether to use <see cref="MinDate"/> for the date range limitation.
        /// </summary>
        public bool UseMinDate
        {
            readonly get
            {
                return useMinDate;
            }

            set
            {
                if (useMinDate == value)
                    return;
                useMinDate = value;
                SetPossibleRange();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="MaxDate"/> and
        /// <see cref="MinDate"/> for the date range limitation.
        /// </summary>
        [Browsable(false)]
        public bool UseMinMaxDate
        {
            readonly get
            {
                return useMinDate && useMaxDate;
            }

            set
            {
                if (useMinDate == value && useMaxDate == value)
                    return;
                useMinDate = value;
                useMaxDate = value;
                SetPossibleRange();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="MaxDate"/> for the date range
        /// limitation.
        /// </summary>
        public bool UseMaxDate
        {
            readonly get
            {
                return useMaxDate;
            }

            set
            {
                if (useMaxDate == value)
                    return;
                useMaxDate = value;
                SetPossibleRange();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected date.
        /// </summary>
        public readonly DateOnly Value
        {
            get
            {
                return valueGetter();
            }

            set
            {
                var v = CoerceSystemMinimumMaximum(value);

                if (UseMinDate && v < min)
                    v = min;
                if (UseMaxDate && v > max)
                    v = max;

                valueSetter(v);
            }
        }

        /// <summary>Gets or sets the minimum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The minimum date and time that can be selected in the
        /// control. The default is <see cref="DateUtils.MinDateTime"/>.
        /// </returns>
        public DateOnly MinDate
        {
            readonly get
            {
                return DateUtils.EffectiveMinDate(min);
            }

            set
            {
                value = CoerceSystemMinimumMaximum(value);

                if (value != min)
                {
                    if (value > max)
                        return;

                    min = value;
                    SetPossibleRange();
                }
            }
        }

        /// <summary>Gets or sets the maximum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The maximum date and time that can be selected
        /// in the control. The default is determined as the minimum of the
        /// CurrentCulture's Calendar's
        /// <see cref="System.Globalization.Calendar.MaxSupportedDateTime" />
        /// property and <see cref="DateUtils.MaxDateTime"/>.</returns>
        public DateOnly MaxDate
        {
            readonly get
            {
                return DateUtils.EffectiveMaxDate(max, FormatProvider);
            }

            set
            {
                value = CoerceSystemMinimumMaximum(value);

                if (value != max)
                {
                    if (value < min)
                        return;

                    max = value;
                    SetPossibleRange();
                }
            }
        }

        /// <summary>
        /// Coerces the specified date to ensure it falls within the system minimum and maximum date range.
        /// </summary>
        /// <param name="date">The date to coerce.</param>
        /// <returns>The coerced date.</returns>
        public readonly DateOnly CoerceSystemMinimumMaximum(DateOnly date)
        {
            var systemMin = DateUtils.MinimumDateTime(FormatProvider).ToDateOnly();
            var systemMax = DateUtils.MaximumDateTime(FormatProvider).ToDateOnly();
            if (date < systemMin)
                return systemMin;
            if (date > systemMax)
                return systemMax;
            return date;
        }

        /// <summary>
        /// Determines whether the specified date is restricted based on the current minimum and maximum date settings.
        /// </summary>
        /// <param name="date">The date to check for restriction.</param>
        /// <returns><c>true</c> if the date is restricted; otherwise, <c>false</c>.</returns>
        public readonly bool IsRestricted(DateOnly date)
        {
            if (UseMinDate && date < min)
                return true;
            if (UseMaxDate && date > max)
                return true;

            if (date < DateUtils.MinimumDateTime(FormatProvider).ToDateOnly())
                return true;
            if (date > DateUtils.MaximumDateTime(FormatProvider).ToDateOnly())
                return true;

            return false;
        }

        /// <summary>
        /// Sets possible date range in the native control.
        /// </summary>
        /// <param name="min">Minimal possible date.</param>
        /// <param name="max">Maximal possible date.</param>
        private readonly void SetRange(DateOnly min, DateOnly max)
        {
            setRange?.Invoke(min, max);
        }

        /// <summary>
        /// Updates possible date range using current settings.
        /// </summary>
        private void SetPossibleRange()
        {
            if (UseMinDate)
            {
                if (Value < min)
                    Value = min;
            }

            if (UseMaxDate)
            {
                if (Value > max)
                    Value = max;
            }

            SetRange(DateUtils.EffectiveMinDate(min), DateUtils.EffectiveMaxDate(max));
        }
    }
}
