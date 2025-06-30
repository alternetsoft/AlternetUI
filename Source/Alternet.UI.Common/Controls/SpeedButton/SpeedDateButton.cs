using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a specialized speed button control that displays a date
    /// and shows popup calendar when it is clicked.
    /// </summary>
    public partial class SpeedDateButton : SpeedButtonWithPopup<PopupCalendar, Calendar>
    {
        private IFormatProvider? formatProvider;
        private string? format;
        private DateTime max = DateTime.MaxValue;
        private DateTime min = DateTime.MinValue;
        private bool useMinDate = false;
        private bool useMaxDate = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedDateButton"/> class.
        /// </summary>
        public SpeedDateButton()
        {
            PopupWindowTitle = CommonStrings.Default.WindowTitleSelectDate;
            Value = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the format provider used for formatting the date.
        /// </summary>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider
        {
            get
            {
                return formatProvider;
            }

            set
            {
                if (formatProvider == value)
                    return;
                formatProvider = value;
                UpdateBaseText();
            }
        }

        /// <summary>
        /// Gets or sets the format used for formatting the date.
        /// </summary>
        [Browsable(false)]
        public virtual string? Format
        {
            get
            {
                return format;
            }

            set
            {
                if (format == value)
                    return;
                format = value;
                UpdateBaseText();
            }
        }

        /// <summary>
        /// Gets or sets selected date.
        /// </summary>
        public new virtual DateTime? Value
        {
            get => (DateTime?)base.Value;
            set => base.Value = value;
        }

        /// <summary>
        /// Gets the calendar control used in the popup window.
        /// </summary>
        public Calendar Calendar => PopupWindow.MainControl;

        /// <summary>Gets or sets the maximum date that can be
        /// selected in the control.</summary>
        /// <returns>The maximum date that can be selected
        /// in the control. The default is determined as the minimum of the
        /// CurrentCulture's Calendar's
        /// <see cref="System.Globalization.Calendar.MaxSupportedDateTime" />
        /// property and <see cref="CustomDateEdit.MaxDateTime"/>.</returns>
        /// <exception cref="System.ArgumentException">The value assigned is less
        /// than the <see cref="MinDate" />
        /// value.</exception>
        /// <exception cref="System.SystemException">The value assigned is greater
        /// than the <see cref="CustomDateEdit.MaxDateTime" />
        /// value.</exception>
        public virtual DateTime MaxDate
        {
            get
            {
                return CustomDateEdit.EffectiveMaxDate(max);
            }

            set
            {
                if (value != max)
                {
                    if (value < CustomDateEdit.EffectiveMinDate(min))
                        throw new ArgumentOutOfRangeException(nameof(MaxDate));

                    if (value > CustomDateEdit.MaximumDateTime)
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

        /// <summary>Gets or sets the minimum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The minimum date and time that can be selected in the
        /// control. The default is <see cref="CustomDateEdit.MinDateTime"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">The value assigned is
        /// not less than the <see cref="MaxDate" /> value.
        /// </exception>
        /// <exception cref="System.SystemException">The value assigned is
        /// less than the <see cref="CustomDateEdit.MinDateTime" /> value.
        /// </exception>
        public virtual DateTime MinDate
        {
            get
            {
                return CustomDateEdit.EffectiveMinDate(min);
            }

            set
            {
                if (value != min)
                {
                    if (value > CustomDateEdit.EffectiveMaxDate(max))
                        throw new ArgumentOutOfRangeException(nameof(MinDate));
                    if (value < CustomDateEdit.MinimumDateTime)
                        throw new ArgumentOutOfRangeException(nameof(MinDate));
                    min = value;
                    SetRange();
                }
            }
        }

        /// <inheritdoc/>
        public override void ShowPopup()
        {
            Calendar.Required();
            SetRange();
            Calendar.Value = Value ?? DateTime.Now;
            base.ShowPopup();
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

            if (DisposingOrDisposed || !IsPopupWindowCreated)
                return;

            var effectiveMin = CustomDateEdit.EffectiveMinDate(min);
            var effectiveMax = CustomDateEdit.EffectiveMaxDate(max);

            Calendar.MinDate = effectiveMin;
            Calendar.MaxDate = effectiveMax;
            Calendar.UseMinDate = UseMinDate;
            Calendar.UseMaxDate = UseMaxDate;
        }

        /// <inheritdoc/>
        public override string? GetValueAsString(object? d)
        {
            if (d is not DateTime dateTime)
                return null;

            if (Format is null)
            {
                if (FormatProvider is null)
                    return dateTime.ToShortDateString();
                else
                    return dateTime.ToString(FormatProvider);
            }
            else
            {
                if (FormatProvider is null)
                    return dateTime.ToString(Format);
                else
                    return dateTime.ToString(Format, FormatProvider);
            }
        }

        /// <inheritdoc/>
        protected override void OnPopupWindowClosed(object? sender, EventArgs e)
        {
            if (PopupWindow.IsPopupAccepted)
            {
                Value = PopupWindow.MainControl.Value;
            }
        }
    }
}
