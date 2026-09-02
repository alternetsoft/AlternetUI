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

        /// <summary>Gets or sets the maximum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The maximum date and time that can be selected
        /// in the control. The default is determined as the minimum of the
        /// CurrentCulture's Calendar's
        /// <see cref="System.Globalization.Calendar.MaxSupportedDateTime" />
        /// property and <see cref="DateUtils.MaxDateTime"/>.</returns>
        /// <exception cref="System.ArgumentException">The value assigned is less
        /// than the <see cref="MinDate" />
        /// value.</exception>
        /// <exception cref="System.SystemException">The value assigned is greater
        /// than the <see cref="DateUtils.MaxDateTime" />
        /// value.</exception>
        public DateOnly MaxDate
        {
            readonly get
            {
                return DateUtils.EffectiveMaxDate(max, FormatProvider);
            }

            set
            {
                if (value != max)
                {
                    if (value < DateUtils.EffectiveMinDate(min, FormatProvider))
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    if (value > DateUtils.MaximumDateTime(FormatProvider).ToDateOnly())
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    max = value;
                    SetPossibleRange();
                }
            }
        }

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
                    var v = value;

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
        /// <exception cref="System.ArgumentException">The value assigned is
        /// not less than the <see cref="MaxDate" /> value.
        /// </exception>
        /// <exception cref="System.SystemException">The value assigned is
        /// less than the <see cref="DateUtils.MinDateTime" /> value.
        /// </exception>
        public DateOnly MinDate
        {
            readonly get
            {
                return DateUtils.EffectiveMinDate(min);
            }

            set
            {
                if (value != min)
                {
                    if (value > DateUtils.EffectiveMaxDate(max, FormatProvider))
                        throw new ArgumentOutOfRangeException(nameof(MinDate));
                    if (value < DateUtils.MinimumDateTime(FormatProvider).ToDateOnly())
                        throw new ArgumentOutOfRangeException(nameof(MinDate));

                    min = value;
                    SetPossibleRange();
                }
            }
        }

        /// <summary>
        /// Sets possible date range in the native control.
        /// </summary>
        /// <param name="min">Minimal possible date.</param>
        /// <param name="max">Maximal possible date.</param>
        public readonly void SetRange(DateOnly min, DateOnly max)
        {
            setRange?.Invoke(min, max);
        }

        /// <summary>
        /// Updates possible date range using current settings.
        /// </summary>
        public void SetPossibleRange()
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
