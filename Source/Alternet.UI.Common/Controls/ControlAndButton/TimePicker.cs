using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a time and allows to edit it.
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Date")]
    public partial class TimePicker : ControlAndButton<ToolBar>
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor using <see cref="AbstractControl.UseControlColors"/>.
        /// Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Gets or sets the default image used for incrementing the time.
        /// </summary>
        public static SvgImage? DefaultIncrementImage;

        /// <summary>
        /// Gets or sets the default image used for decrementing the time.
        /// </summary>
        public static SvgImage? DefaultDecrementImage;

        /// <summary>
        /// Gets or sets the default hour format for the <c>TimePicker</c>.
        /// </summary>
        public static TimePickerHourFormat DefaultHourFormat = TimePickerHourFormat.System;

        private readonly SpeedButton hoursButton;
        private readonly AbstractControl hoursAndMinutesSeparator;
        private readonly SpeedButton minutesButton;
        private readonly AbstractControl minutesAndSecondsSeparator;
        private readonly SpeedButton secondsButton;
        private readonly AbstractControl secondsAndAmPmSeparator;
        private readonly SpeedButton amPmButton;

        private TimePickerHourFormat? hourFormat;
        private IFormatProvider? formatProvider;
        private DateTime time;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class.
        /// </summary>
        public TimePicker()
        {
            UseControlColors(DefaultUseControlColors);
            WantChars = true;
            IsGraphicControl = false;
            CanSelect = true;
            TabStop = true;

            time = DateTime.Now;
            InnerOuterBorder = InnerOuterSelector.Outer;

            var timeSeparator = GetTimeSeparator() ?? ":";
            var amPM = GetAmPmValue();
            var is12HourFormat = amPM is not null;

            hoursButton = MainControl.AddTextBtnCore();
            hoursAndMinutesSeparator = MainControl.AddTextCore();
            minutesButton = MainControl.AddTextBtnCore();
            minutesAndSecondsSeparator = MainControl.AddTextCore();
            secondsButton = MainControl.AddTextBtnCore();
            secondsAndAmPmSeparator = MainControl.AddSpacerCore();
            amPmButton = MainControl.AddTextBtnCore();
            UpdateButtons();

            void FocusMe()
            {
                App.AddIdleTask(
                    () =>
                    {
                        if (DisposingOrDisposed)
                            return;
                        SetFocusIfPossible();
                    });
            }

            MainControl.MouseLeftButtonDown += (s, e) =>
            {
                SelectedPart = TimePickerValuePart.Hour;
                FocusMe();
                e.Handled = true;
            };

            hoursButton.ClickAction = () =>
            {
                SelectedPart = TimePickerValuePart.Hour;
                FocusMe();
            };

            minutesButton.ClickAction = () =>
            {
                SelectedPart = TimePickerValuePart.Minute;
                FocusMe();
            };

            secondsButton.ClickAction = () =>
            {
                SelectedPart = TimePickerValuePart.Second;
                FocusMe();
            };

            amPmButton.ClickAction = () =>
            {
                SelectedPart = TimePickerValuePart.AmPm;
                FocusMe();
            };

            HasBtnComboBox = false;

            SetPlusMinusImages(
                DefaultIncrementImage ?? KnownSvgImages.ImgAngleUp,
                DefaultDecrementImage ?? KnownSvgImages.ImgAngleDown);

            HasBtnPlusMinus = true;
            Buttons.DoubleClickAsClick = false;
            IsBtnClickRepeated = true;
            ButtonPlus?.SetEnabled(false);
            ButtonMinus?.SetEnabled(false);
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the currently selected part of the time value.
        /// </summary>
        public virtual TimePickerValuePart? SelectedPart
        {
            get
            {
                if (hoursButton.Sticky)
                    return TimePickerValuePart.Hour;
                if (minutesButton.Sticky)
                    return TimePickerValuePart.Minute;
                if (secondsButton.Sticky)
                    return TimePickerValuePart.Second;
                if (amPmButton.Sticky)
                    return TimePickerValuePart.AmPm;
                return null;
            }

            set
            {
                if (!amPmButton.IsVisible && value == TimePickerValuePart.AmPm)
                {
                    value = null;
                }

                if (!secondsButton.IsVisible && value == TimePickerValuePart.Second)
                {
                    value = null;
                }

                if (SelectedPart == value)
                    return;

                hoursButton.Sticky = value == TimePickerValuePart.Hour;
                minutesButton.Sticky = value == TimePickerValuePart.Minute;
                secondsButton.Sticky = value == TimePickerValuePart.Second;
                amPmButton.Sticky = value == TimePickerValuePart.AmPm;
                ButtonPlus?.SetEnabled(value is not null);
                ButtonMinus?.SetEnabled(value is not null);
            }
        }

        /// <summary>
        /// Gets or sets selected time.
        /// </summary>
        public virtual DateTime Value
        {
            get => time;

            set
            {
                if (time == value)
                    return;
                time = value;
                UpdateButtons();
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether the seconds part of the time is visible.
        /// </summary>
        public virtual bool SecondsVisible
        {
            get => secondsButton.IsVisible;
            set
            {
                if (secondsButton.IsVisible == value)
                    return;
                secondsButton.IsVisible = value;
                minutesAndSecondsSeparator.IsVisible = value;
                UpdateButtons();
            }
        }

        /// <summary>
        /// Gets or sets the hour format for the <c>TimePicker</c>.
        /// Default is null, which means <see cref="DefaultHourFormat"/> will be used.
        /// </summary>
        public virtual TimePickerHourFormat? HourFormat
        {
            get => hourFormat;
            set
            {
                if (hourFormat == value)
                    return;
                hourFormat = value;
                UpdateButtons();
            }
        }

        /// <summary>
        /// Gets or sets the format provider used for formatting the time.
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
                UpdateButtons();
            }
        }

        /// <inheritdoc/>
        public override void OnButtonClick(ControlAndButtonClickEventArgs e)
        {
            base.OnButtonClick(e);
            if (e.IsButtonPlus(this))
            {
                e.Handled = true;
                IncPartValue(1);
            }
            else if (e.IsButtonMinus(this))
            {
                e.Handled = true;
                IncPartValue(-1);
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public override IEnumerable<AbstractControl> GetFocusableChildren(bool recursive)
        {
            return Array.Empty<AbstractControl>();
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if(!IsAnyPartSelected())
                SelectedPart = TimePickerValuePart.Hour;
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            SelectedPart = null;
        }

        /// <summary>
        /// Gets the last part of the time value that can be selected.
        /// </summary>
        /// <returns>The last part of the time value.</returns>
        protected virtual TimePickerValuePart GetLastPart()
        {
            if (amPmButton.IsVisible)
                return TimePickerValuePart.AmPm;
            else
            if (secondsButton.IsVisible)
                return TimePickerValuePart.Second;
            else
                return TimePickerValuePart.Minute;
        }

        /// <summary>
        /// Selects the last part of the time value.
        /// </summary>
        protected virtual void SelectLastPart()
        {
            SelectedPart = GetLastPart();
        }

        /// <summary>
        /// Selects the next part of the time value.
        /// </summary>
        /// <param name="forward">The direction to select the next part.
        /// If true, selects forward; otherwise, selects backward.</param>
        protected virtual void SelectNextPart(bool forward)
        {
            if(SelectedPart == null)
            {
                SelectedPart = TimePickerValuePart.Hour;
                return;
            }

            if (forward)
            {
                if(SelectedPart == GetLastPart())
                {
                    SelectedPart = TimePickerValuePart.Hour;
                    return;
                }

                SelectedPart++;
            }
            else
            {
                if (SelectedPart == TimePickerValuePart.Hour)
                {
                    SelectLastPart();
                    return;
                }

                SelectedPart--;
            }
        }

        /// <summary>
        /// Increments the hours part of the time value by the specified amount.
        /// </summary>
        /// <param name="value">The amount to increment the hours by.</param>
        protected virtual void IncHours(int value)
        {
            var v = Value.AddHours(value);
            Value = v;
        }

        /// <summary>
        /// Increments the minutes part of the time value by the specified amount.
        /// </summary>
        /// <param name="value">The amount to increment the minutes by.</param>
        protected virtual void IncMinutes(int value)
        {
            var v = Value.AddMinutes(value);
            Value = v;
        }

        /// <summary>
        /// Increments the seconds part of the time value by the specified amount.
        /// </summary>
        /// <param name="value">The amount to increment the seconds by.</param>
        protected virtual void IncSeconds(int value)
        {
            var v = Value.AddSeconds(value);
            Value = v;
        }

        /// <summary>
        /// Increments the value of the currently selected part of the time.
        /// </summary>
        /// <param name="value">The amount to increment the selected part by.</param>
        protected virtual void IncPartValue(int value)
        {
            var part = SelectedPart;
            if(part is null)
                return;

            switch (part)
            {
                case TimePickerValuePart.Hour:
                    IncHours(value);
                    break;
                case TimePickerValuePart.Minute:
                    IncMinutes(value);
                    break;
                case TimePickerValuePart.Second:
                    IncSeconds(value);
                    break;
                case TimePickerValuePart.AmPm:
                    if (HourFormat == TimePickerHourFormat.Hour24)
                        return;
                    IncHours(12);
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if(!IsEnabled || DisposingOrDisposed)
                return;
            switch (e.Key)
            {
                case Key.Home:
                    SelectedPart = TimePickerValuePart.Hour;
                    e.Suppressed();
                    break;
                case Key.End:
                    SelectLastPart();
                    e.Suppressed();
                    break;
                case Key.Left:
                    SelectNextPart(false);
                    e.Suppressed();
                    break;
                case Key.Right:
                    SelectNextPart(true);
                    e.Suppressed();
                    break;
                case Key.Up:
                    IncPartValue(1);
                    e.Suppressed();
                    break;
                case Key.Down:
                    IncPartValue(-1);
                    e.Suppressed();
                    break;
            }
        }

        /// <summary>
        /// Gets hours as a string for the current value selected in <c>TimePicker</c>.
        /// </summary>
        /// <param name="is12HourFormat">The <c>is12HourFormat</c> determines whether
        /// to return the hours in 12-hour format.</param>
        /// <returns>The formatted string representing the hours.</returns>
        protected virtual string GetHoursAsString(bool is12HourFormat)
        {
            return is12HourFormat ? time.ToString("hh") : time.ToString("HH");
        }

        /// <summary>
        /// Gets minutes as a string for the current value selected in <c>TimePicker</c>.
        /// </summary>
        /// <returns>The formatted string representing the minutes.</returns>
        protected virtual string GetMinutesAsString()
        {
            return time.ToString("mm");
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            SetFocusIfPossible();
            e.Handled = true;
        }

        /// <summary>
        /// Gets seconds as a string for the current value selected in <c>TimePicker</c>.
        /// </summary>
        /// <returns>The formatted string representing the seconds.</returns>
        protected virtual string GetSecondsAsString()
        {
            return time.ToString("ss");
        }

        /// <summary>
        /// Gets am/pm value for the current value selected in <c>TimePicker</c>.
        /// </summary>
        /// <returns>The formatted string representing the am/pm value.</returns>
        protected virtual string? GetAmPmValue()
        {
            if (HourFormat == TimePickerHourFormat.Hour24)
                return null;
            var result = time.ToString("tt");
            if (string.IsNullOrWhiteSpace(result))
                return null;
            return result;
        }

        /// <summary>
        /// Updates the text of the buttons in the <c>TimePicker</c>.
        /// </summary>
        protected virtual void UpdateButtons()
        {
            var timeSeparator = GetTimeSeparator() ?? ":";
            var amPM = GetAmPmValue();
            var is12HourFormat = amPM is not null;

            hoursButton.Text = GetHoursAsString(is12HourFormat);
            hoursAndMinutesSeparator.Text = timeSeparator;
            minutesButton.Text = GetMinutesAsString();
            minutesAndSecondsSeparator.Text = timeSeparator;
            secondsButton.Text = GetSecondsAsString();

            amPmButton.Text = amPM ?? string.Empty;
            amPmButton.IsVisible = is12HourFormat;
            secondsAndAmPmSeparator.IsVisible = is12HourFormat;
        }

        /// <summary>
        /// Gets time separator used in the <c>TimePicker</c>.
        /// Typically it is a colon (:).
        /// </summary>
        /// <returns>The time separator as a string.</returns>
        protected virtual string GetTimeSeparator()
        {
            return DateUtils.GetTimeSeparator(FormatProvider);
        }

        private bool IsAnyPartSelected()
        {
            return hoursButton.Sticky || minutesButton.Sticky
                || secondsButton.Sticky || amPmButton.Sticky;
        }
    }
}
