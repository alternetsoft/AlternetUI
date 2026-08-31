using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a restricted date time container.
    /// </summary>
    internal interface IRestrictedDateTimeContainer : IValueSource<DateTime?>
    {
        /// <summary>
        /// Sets the possible date range in the native control.
        /// </summary>
        /// <param name="min">The minimum date and time.</param>
        /// <param name="max">The maximum date and time.</param>
        void SetRange(DateTime min, DateTime max);
    }

    /// <summary>
    /// Represents a restricted date time structure that allows setting minimum and maximum date limits.
    /// </summary>
    internal partial class RestrictedDateTime : BaseObject
    {
        private DateTime max = DateTime.MaxValue;
        private DateTime min = DateTime.MinValue;
        private bool useMinDate = false;
        private bool useMaxDate = false;
        private IRestrictedDateTimeContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictedDateTime"/> class.
        /// </summary>
        public RestrictedDateTime(IRestrictedDateTimeContainer container)
        {
            this.container = container;
        }

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
        public virtual DateTime MaxDate
        {
            get
            {
                return DateUtils.EffectiveMaxDate(max);
            }

            set
            {
                if (value != max)
                {
                    if (value < DateUtils.EffectiveMinDate(min))
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    if (value > DateUtils.MaximumDateTime)
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    max = value;
                    SetPossibleRange();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="MinDate"/> for the date range
        /// limitation.
        /// </summary>
        public virtual bool UseMinDate
        {
            get
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
        public virtual bool UseMinMaxDate
        {
            get
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
        public virtual bool UseMaxDate
        {
            get
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
        public virtual DateTime? Value
        {
            get
            {
                return container.Value;
            }

            set
            {
                if (value.HasValue)
                {
                    var v = value.Value;

                    if (UseMinDate && v < min)
                        v = min;
                    if (UseMaxDate && v > max)
                        v = max;

                    container.Value = v;
                }
                else
                {
                    container.Value = null;
                }
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
        public virtual DateTime MinDate
        {
            get
            {
                return DateUtils.EffectiveMinDate(min);
            }

            set
            {
                if (value != min)
                {
                    if (value > DateUtils.EffectiveMaxDate(max))
                        throw new ArgumentOutOfRangeException(nameof(MinDate));
                    if (value < DateUtils.MinimumDateTime)
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
        protected virtual void SetRange(DateTime min, DateTime max)
        {
        }

        /// <summary>
        /// Updates possible date range using current settings.
        /// </summary>
        protected virtual void SetPossibleRange()
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
