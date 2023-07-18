using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected date and allows to change it.
    /// </summary>
    public class DateTimePicker : Control
    {
        /// <summary>Specifies the maximum date value of the
        /// <see cref="DateTimePicker" /> control.
        /// This field is read-only.</summary>
        public static readonly DateTime MaxDateTime = new(9998, 12, 31);

        /// <summary>Gets the minimum date value of the
        /// <see cref="DateTimePicker" /> control.</summary>
        public static readonly DateTime MinDateTime = new(1753, 1, 1);

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(DateTime),
                typeof(DateTimePicker),
                new FrameworkPropertyMetadata(
                    DateTime.MinValue, // default value
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                        FrameworkPropertyMetadataOptions.AffectsPaint,
                    new PropertyChangedCallback(OnValuePropertyChanged),
                    null, // CoerseValueCallback
                    true, // IsAnimationProhibited
                    UpdateSourceTrigger.PropertyChanged));

        private DateTime max = DateTime.MaxValue;
        private DateTime min = DateTime.MinValue;
        private bool useMinDate = false;
        private bool useMaxDate = false;

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in
        /// some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the
        /// <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new
        /// value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <summary>Gets the maximum date value allowed for the
        /// <see cref="DateTimePicker" /> control.</summary>
        /// <returns>A <see cref="System.DateTime" /> representing the
        /// maximum date value for the <see cref="DateTimePicker" />
        /// control.</returns>
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

        /// <summary>Gets the minimum date value allowed for the
        /// <see cref="DateTimePicker" /> control.</summary>
        /// <returns>A <see cref="System.DateTime" /> representing the
        /// minimum date value for the <see cref="DateTimePicker" />
        /// control.</returns>
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


        /// <summary>
        /// Gets or sets the value assigned to the color picker as a selected color.
        /// </summary>
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>Gets or sets the maximum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The maximum date and time that can be selected
        /// in the control. The default is determined as the minimum of the
        /// CurrentCulture's Calendar's
        /// <see cref="System.Globalization.Calendar.MaxSupportedDateTime" />
        /// property and <see cref="DateTimePicker.MaxDateTime"/>.</returns>
        /// <exception cref="T:System.ArgumentException">The value assigned is less
        /// than the <see cref="DateTimePicker.MinDate" />
        /// value.</exception>
        /// <exception cref="System.SystemException">The value assigned is greater
        /// than the <see cref="DateTimePicker.MaxDateTime" />
        /// value.</exception>
        public DateTime MaxDate
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
                    if (Value > max)
                        Value = max;
                }
            }
        }

        public bool UseMinDate
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

        public bool UseMinMaxDate
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

        public bool UseMaxDate
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
        /// control. The default is <see cref="DateTimePicker.MinDateTime"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">The value assigned is
        /// not less than the <see cref="DateTimePicker.MaxDate" /> value.
        /// </exception>
        /// <exception cref="System.SystemException">The value assigned is
        /// less than the <see cref="DateTimePicker.MinDateTime" /> value.
        /// </exception>
        public DateTime MinDate
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
                    if (Value < min)
                        Value = min;
                }
            }
        }

        public DateTimePickerKind Kind
        {
            get
            {
                return Handler.Kind;
            }

            set
            {
                Handler.Kind = value;
            }
        }

        public DateTimePickerPopupKind PopupKind
        {
            get
            {
                return Handler.PopupKind;
            }

            set
            {
                Handler.PopupKind = value;
            }
        }

        internal new NativeDateTimePickerHandler Handler =>
            (NativeDateTimePickerHandler)base.Handler;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
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
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Callback for changes to the Value property
        /// </summary>
        private static void OnValuePropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker control = (DateTimePicker)d;
            control.OnValuePropertyChanged(
                (DateTime)e.OldValue,
                (DateTime)e.NewValue);
        }

        private void OnValuePropertyChanged(DateTime oldValue, DateTime newValue)
        {
            RaiseValueChanged(EventArgs.Empty);
        }

        private void SetRange(DateTime min, DateTime max)
        {
            Handler.SetRange(min, max, useMinDate, useMaxDate);
        }

        private void SetRange()
        {
            SetRange(EffectiveMinDate(min), EffectiveMaxDate(max));
        }
    }
}
