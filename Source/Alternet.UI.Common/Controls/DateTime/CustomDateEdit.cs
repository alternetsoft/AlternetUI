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
        public abstract DateTime Value { get; set; }

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

            SetRange(DateUtils.EffectiveMinDate(min), DateUtils.EffectiveMaxDate(max));
        }
    }
}
