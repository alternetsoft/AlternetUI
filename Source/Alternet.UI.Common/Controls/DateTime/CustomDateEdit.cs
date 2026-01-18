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
    /// Base class for date editors.
    /// </summary>
    public abstract partial class CustomDateEdit : Control
    {
        /// <summary>Specifies the maximum date value of the
        /// <see cref="DateTimePicker"/> and other date editors.
        /// This field is read-only.</summary>
        public static readonly DateTime MaxDateTime = new(9998, 12, 31);

        /// <summary>Gets the minimum date value of the
        /// <see cref="DateTimePicker"/> and other date editors.</summary>
        public static readonly DateTime MinDateTime = new(1753, 1, 1);

        private DateTime max = DateTime.MaxValue;
        private DateTime min = DateTime.MinValue;
        private bool useMinDate = false;
        private bool useMaxDate = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDateEdit"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CustomDateEdit(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDateEdit"/> class.
        /// </summary>
        public CustomDateEdit()
        {
        }

        /// <summary>Gets the maximum date value allowed for the control.</summary>
        /// <returns>A <see cref="System.DateTime" /> representing the
        /// maximum date value for the control.</returns>
        public static DateTime MaximumDateTime
        {
            get
            {
                DateTime maxSupportedDateTime =
                    CultureInfo.CurrentCulture.Calendar.MaxSupportedDateTime;
                if (maxSupportedDateTime.Year > MaxDateTime.Year)
                {
                    return MaxDateTime;
                }

                return maxSupportedDateTime;
            }
        }

        /// <summary>Gets the minimum date value allowed for the control.</summary>
        /// <returns>A <see cref="System.DateTime"/> representing the
        /// minimum date value for the control.</returns>
        public static DateTime MinimumDateTime
        {
            get
            {
                DateTime minSupportedDateTime =
                    CultureInfo.CurrentCulture.Calendar.MinSupportedDateTime;
                if (minSupportedDateTime.Year < MinDateTime.Year)
                {
                    return MinDateTime;
                }

                return minSupportedDateTime;
            }
        }

        /// <summary>Gets or sets the maximum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The maximum date and time that can be selected
        /// in the control. The default is determined as the minimum of the
        /// CurrentCulture's Calendar's
        /// <see cref="System.Globalization.Calendar.MaxSupportedDateTime" />
        /// property and <see cref="MaxDateTime"/>.</returns>
        /// <exception cref="System.ArgumentException">The value assigned is less
        /// than the <see cref="MinDate" />
        /// value.</exception>
        /// <exception cref="System.SystemException">The value assigned is greater
        /// than the <see cref="MaxDateTime" />
        /// value.</exception>
        public virtual DateTime MaxDate
        {
            get
            {
                return EffectiveMaxDate(max);
            }

            set
            {
                if (value != max)
                {
                    if (value < EffectiveMinDate(min))
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    if (value > MaximumDateTime)
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    max = value;
                    SetRange();
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
                SetRange();
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
                SetRange();
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
                SetRange();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected date.
        /// </summary>
        public abstract DateTime? Value { get; set; }

        /// <summary>Gets or sets the minimum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The minimum date and time that can be selected in the
        /// control. The default is <see cref="MinDateTime"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">The value assigned is
        /// not less than the <see cref="MaxDate" /> value.
        /// </exception>
        /// <exception cref="System.SystemException">The value assigned is
        /// less than the <see cref="MinDateTime" /> value.
        /// </exception>
        public virtual DateTime MinDate
        {
            get
            {
                return EffectiveMinDate(min);
            }

            set
            {
                if (value != min)
                {
                    if (value > EffectiveMaxDate(max))
                        throw new ArgumentOutOfRangeException(nameof(MinDate));
                    if (value < MinimumDateTime)
                        throw new ArgumentOutOfRangeException(nameof(MinDate));
                    min = value;
                    SetRange();
                }
            }
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        internal static DateTime EffectiveMaxDate(DateTime maxDate)
        {
            DateTime maximumDateTime = MaximumDateTime;
            if (maxDate > maximumDateTime)
            {
                return maximumDateTime;
            }

            return maxDate;
        }

        internal static DateTime EffectiveMinDate(DateTime minDate)
        {
            DateTime minimumDateTime = MinimumDateTime;
            if (minDate < minimumDateTime)
            {
                return minimumDateTime;
            }

            return minDate;
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
        protected virtual void SetRange()
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

            SetRange(EffectiveMinDate(min), EffectiveMaxDate(max));
        }
    }
}
