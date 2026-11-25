using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a time and allows to edit it.
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory(KnownControlCategory.DateTime)]
    public partial class TimePicker : GenericControlAndButton<ToolBar>, IReadOnlyProperty
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor. Default is <c>true</c>.
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
        private readonly Label hoursAndMinutesSeparator;
        private readonly SpeedButton minutesButton;
        private readonly Label minutesAndSecondsSeparator;
        private readonly SpeedButton secondsButton;
        private readonly Spacer secondsAndAmPmSeparator;
        private readonly SpeedButton amPmButton;
        private readonly SpeedButton ellipsisButton;
        private readonly ContextMenuStrip ellipsisButtonMenu = new();

        private TimePickerHourFormat? hourFormat;
        private IFormatProvider? formatProvider;
        private TimeOnly time;
        private bool readOnly;
        private bool isValueEmpty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePicker"/> class.
        /// </summary>
        public TimePicker()
        {
            IgnoreTransparency = true;

            MainControl.IsTransparent = true;
            UseControlColors(DefaultUseControlColors);
            IsGraphicControl = false;
            CanSelect = true;
            TabStop = true;

            time = DateUtils.CurrentTimeWithoutSeconds;
            InnerOuterBorder = InnerOuterSelector.Outer;

            hoursButton = MainControl.AddTextBtnCore();
            hoursAndMinutesSeparator = MainControl.AddTextCore();
            minutesButton = MainControl.AddTextBtnCore();
            minutesAndSecondsSeparator = MainControl.AddTextCore();
            secondsButton = MainControl.AddTextBtnCore();
            secondsAndAmPmSeparator = MainControl.AddSpacerCore();
            amPmButton = MainControl.AddTextBtnCore();

            secondsAndAmPmSeparator.Margin = 0;
            hoursAndMinutesSeparator.Margin = 0;
            minutesAndSecondsSeparator.Margin = 0;

            hoursButton.KeepSquareShape = true;
            minutesButton.KeepSquareShape = true;
            secondsButton.KeepSquareShape = true;

            hoursButton.IsNormalTransparent = true;
            minutesButton.IsNormalTransparent = true;
            secondsButton.IsNormalTransparent = true;
            amPmButton.IsNormalTransparent = true;

            hoursButton.IsDisabledTransparent = true;
            minutesButton.IsDisabledTransparent = true;
            secondsButton.IsDisabledTransparent = true;
            amPmButton.IsDisabledTransparent = true;

            UpdateMinElementWidth();
            UpdateButtons();

            void FocusMe()
            {
                SetFocus();
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
            ellipsisButton = Buttons.AddSpeedBtnCore(KnownButton.VertEllipsis);
            ellipsisButton.DropDownMenu = ellipsisButtonMenu;
            IsBtnClickRepeated = true;
            ButtonPlus?.SetEnabled(false);
            ButtonMinus?.SetEnabled(false);

            RoundCorners();

            var itemEnterValue = ellipsisButtonMenu.Add(
                "MenuItemEnterValue", Localized.EnterValue + "...", ShowEnterValueDialog);

            ellipsisButtonMenu.AddSeparator();

            var itemSetCurrentTime = ellipsisButtonMenu.Add(
                "MenuItemSetCurrentTime", Localized.SetCurrentTime, SetCurrentTime);

            ellipsisButtonMenu.AddSeparator();

            var itemRoundToMinute = ellipsisButtonMenu.Add(
                "MenuItemRoundToMinute",
                Localized.RoundToMinute,
                () =>
                {
                    AsTimeOnly = new TimeOnly(time.Hour, time.Minute);
                });

            var itemRoundTo5Minutes = ellipsisButtonMenu.Add(
                "MenuItemRoundTo5Minutes",
                string.Format(Localized.RoundToMinutes, 5),
                () =>
                {
                    AsTimeOnly = new TimeOnly(time.Hour, time.Minute / 5 * 5);
                });

            var itemRoundTo15Minutes = ellipsisButtonMenu.Add(
                "MenuItemRoundTo15Minutes",
                string.Format(Localized.RoundToMinutes, 15),
                () =>
                {
                    AsTimeOnly = new TimeOnly(time.Hour, time.Minute / 15 * 15);
                });

            ellipsisButtonMenu.AddSeparator();

            var itemCopy = ellipsisButtonMenu.Add(
                "MenuItemCopy",
                Localized.ButtonCopy, () =>
                {
                    Clipboard.SetText(AsClipboardString);
                });

            var itemPaste = ellipsisButtonMenu.Add(
                "MenuItemPaste",
                Localized.ButtonPaste,
                () =>
                {
                    SetTimeOrShowError(Clipboard.GetText());
                });

            ContextMenu = ellipsisButtonMenu;
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property is changing.
        /// In this event, you can cancel the change.
        /// </summary>
        public event EventHandler<TimeChangingEventArgs>? ValueChanging;

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets the button used for incrementing the time value. 
        /// </summary>
        public SpeedButton HoursButton => hoursButton;

        /// <summary>
        /// Gets or sets a value indicating whether the <c>TimePicker</c> is empty.
        /// </summary>
        public virtual bool IsValueEmpty
        {
            get
            {
                return isValueEmpty;
            }

            set
            {
                if (isValueEmpty == value)
                    return;
                isValueEmpty = value;
                TextTransparent = isValueEmpty;
            }
        }

        /// <summary>
        /// Gets the context menu associated with the ellipsis button of the <c>TimePicker</c>.
        /// </summary>
        public ContextMenuStrip EllipsisButtonMenu => ellipsisButtonMenu;

        /// <summary>
        /// Gets or sets a value indicating whether the <c>TimePicker</c> is read-only.
        /// </summary>
        public virtual bool ReadOnly
        {
            get => readOnly;
            set
            {
                if (readOnly == value)
                    return;
                readOnly = value;
            }
        }

        /// <summary>
        /// Gets the separator control between hours and minutes buttons.
        /// </summary>
        public AbstractControl HoursAndMinutesSeparator => hoursAndMinutesSeparator;

        /// <summary>
        /// Gets the button used for incrementing the time value. 
        /// </summary>
        public SpeedButton MinutesButton => minutesButton;

        /// <summary>
        /// Gets the ellipsis button used for opening a drop down menu.
        /// </summary>
        public SpeedButton EllipsisButton => ellipsisButton;

        /// <summary>
        /// Gets the separator control between minutes and seconds buttons.
        /// </summary>
        public AbstractControl MinutesAndSecondsSeparator => minutesAndSecondsSeparator;

        /// <summary>
        /// Gets the button used for incrementing the time value. 
        /// </summary>
        public SpeedButton SecondsButton => secondsButton;

        /// <summary>
        /// Gets the separator control between seconds and AM/PM buttons.
        /// </summary>
        public AbstractControl SecondsAndAmPmSeparator => secondsAndAmPmSeparator;

        /// <summary>
        /// Gets the button used for incrementing the time value. 
        /// </summary>
        public SpeedButton AmPmButton => amPmButton;

        /// <summary>
        /// Gets or sets the time value of the <c>TimePicker</c> as a string for clipboard operations.
        /// </summary>
        public string? AsClipboardString
        {
            get
            {
                if (SecondsVisible)
                    return Value.ToString("T");
                else
                    return Value.ToString("t");
            }

            set
            {
                if (TryParse(value, out var result))
                    AsTimeOnly = result;
            }
        }

        /// <summary>
        /// Tries to parse the specified string value into a <see cref="TimeOnly"/> object.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="result">The parsed <see cref="TimeOnly"/> result.</param>
        /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
        public virtual bool TryParse(string? value, out TimeOnly result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }

            if (!TimeOnly.TryParse(value, FormatProvider, System.Globalization.DateTimeStyles.None, out result))
                return false;

            return true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text of the control is visible.
        /// </summary>
        public virtual bool TextTransparent
        {
            get
            {
                return hoursButton.TextTransparent;
            }

            set
            {
                if (TextTransparent == value)
                    return;
                hoursButton.TextTransparent = value;
                minutesButton.TextTransparent = value;
                secondsButton.TextTransparent = value;
                amPmButton.TextTransparent = value;
                hoursAndMinutesSeparator.TextTransparent = value;
                minutesAndSecondsSeparator.TextTransparent = value;

                if (value)
                    SelectedPart = null;
            }
        }

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
                if (value is not null)
                {
                    if (TextTransparent)
                    {
                        value = null;
                    }
                }

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
        /// Gets or sets selected time as <see cref="TimeOnly"/>.
        /// </summary>
        public virtual TimeOnly AsTimeOnly
        {
            get
            {
                return time;
            }

            set
            {
                if (!SecondsVisible)
                    value = new TimeOnly(value.Hour, value.Minute);
                if (time == value)
                    return;
                if (!OnValueChanging(time, value))
                    return;
                time = value;
                UpdateButtons();
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets selected time as <see cref="DateTime"/>.
        /// </summary>
        public virtual DateTime Value
        {
            get => time.ToDateTime(DateOnly.FromDateTime(DateTime.Now));

            set
            {
                var newTime = TimeOnly.FromDateTime(value);
                AsTimeOnly = newTime;
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

                if (!value)
                {
                    if (SelectedPart == TimePickerValuePart.Second)
                        SelectedPart = TimePickerValuePart.Minute;

                    AsTimeOnly = new TimeOnly(time.Hour, time.Minute);
                }

                UpdateButtons();
                PerformLayoutAndInvalidate();
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

        /// <summary>
        /// Parses the specified string value into a <see cref="TimeOnly"/> object.
        /// If parsing fails, a tooltip with an error message is displayed.
        /// If parsing is successful, the <see cref="AsTimeOnly"/> property is updated with the parsed value.
        /// </summary>
        /// <param name="s">The string representation of the time to parse. </param>
        public virtual void SetTimeOrShowError(string? s)
        {
            if (TryParse(s, out var timeValue))
                AsTimeOnly = timeValue;
            else
            {
                Post(() =>
                {
                    if (DisposingOrDisposed)
                        return;

                    RichToolTipParams tooltip = new()
                    {
                        Text = Localized.ErrInvalidTimeFormat,
                        Icon = MessageBoxIcon.Error,
                        AutoHideOnEvents = false,
                    };

                    UI.ToolTip.Instance.ShowTemp(tooltip, MainControl);
                });
            }
        }

        /// <inheritdoc/>
        public override void OnButtonClick(ControlAndButtonClickEventArgs e)
        {
            base.OnButtonClick(e);

            SetFocus();

            if (e.ButtonId == IdButtonPlus)
            {
                e.Handled = true;
                if (!ReadOnly)
                    IncPartValue(1);
            }
            else if (e.ButtonId == IdButtonMinus)
            {
                e.Handled = true;
                if (!ReadOnly)
                    IncPartValue(-1);
            }
        }

        /// <summary>
        /// Shows a dialog that prompts the user to enter a time value using a text input.
        /// </summary>
        public virtual void ShowEnterValueDialog()
        {
            TextFromUserParams prm = new()
            {
                Title = Localized.EnterValue,
                DefaultValue = AsClipboardString,
                IsValidPredicate = (s) =>
                {
                    if (string.IsNullOrWhiteSpace(s))
                        return true;
                    return TryParse(s, out _);
                },
                OnApply = (s) =>
                {
                    if (string.IsNullOrWhiteSpace(s))
                        return;
                    SetTimeOrShowError(s);
                },
            };
            DialogFactory.GetTextFromUserAsync(prm);
        }

        /// <summary>
        /// Sets the current system time as the value of the <c>TimePicker</c>.
        /// If the <see cref="SecondsVisible"/> property is true, the current time with seconds is used;
        /// otherwise, the current time without seconds is used.
        /// </summary>
        public virtual void SetCurrentTime()
        {
            if (SecondsVisible)
                AsTimeOnly = DateTime.Now.ToTimeOnly();
            else
                AsTimeOnly = DateUtils.CurrentTimeWithoutSeconds;
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
            if (!IsAnyPartSelected())
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
            if (SelectedPart == null)
            {
                SelectedPart = TimePickerValuePart.Hour;
                return;
            }

            if (forward)
            {
                if (SelectedPart == GetLastPart())
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

        /// <inheritdoc/>
        protected override void OnChildMouseDown(object? sender, MouseEventArgs e)
        {
            SetFocus();
            base.OnChildMouseDown(sender, e);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            SetFocus();
            base.OnMouseLeftButtonDown(e);
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
            if (part is null)
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
                    if (Uses24HourFormat())
                        return;
                    IncHours(12);
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (!IsEnabled || DisposingOrDisposed)
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
                    if (!ReadOnly)
                        IncPartValue(1);
                    e.Suppressed();
                    break;
                case Key.Down:
                    if (!ReadOnly)
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

        /// <summary>
        /// Gets seconds as a string for the current value selected in <c>TimePicker</c>.
        /// </summary>
        /// <returns>The formatted string representing the seconds.</returns>
        protected virtual string GetSecondsAsString()
        {
            return time.ToString("ss");
        }

        /// <summary>
        /// Gets a value indicating whether the <c>TimePicker</c> uses 24-hour format.
        /// </summary>
        /// <returns><c>true</c> if the <c>TimePicker</c> uses 24-hour format; otherwise, <c>false</c>.</returns>
        protected virtual bool Uses24HourFormat()
        {
            switch (HourFormat ?? DefaultHourFormat)
            {
                default:
                case TimePickerHourFormat.System:
                    var uses24Hour = DateUtils.Uses24Hour(FormatProvider);
                    return uses24Hour;
                case TimePickerHourFormat.Hour12:
                    return false;
                case TimePickerHourFormat.Hour24:
                    return true;
            }
        }

        /// <summary>
        /// Gets am/pm value for the current value selected in <c>TimePicker</c>.
        /// </summary>
        /// <returns>The formatted string representing the am/pm value.</returns>
        protected virtual string? GetAmPmValue()
        {
            if (Uses24HourFormat())
                return null;
            var result = DateUtils.GetAmOrPmDesignator(time, FormatProvider);
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

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (AutoUpdateColors)
                UseControlColors(DefaultUseControlColors);
            base.OnSystemColorsChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateMinElementWidth();
        }

        /// <summary>
        /// Updates the minimum width of the AM/PM and other buttons based on their text and font.
        /// </summary>
        protected virtual void UpdateMinElementWidth()
        {
            var (am, pm) = DateUtils.GetAmPmDesignators(FormatProvider);

            var minWidth = DrawingUtils.GetMaxStringSize(MeasureCanvas, amPmButton.RealFont, am, pm).Width + 10;
            amPmButton.MinWidth = minWidth;
        }

        /// <inheritdoc/>
        protected override void OnContextMenuChanged(EventArgs e)
        {
            base.OnContextMenuChanged(e);

            foreach (var child in GetChildren(recursive: true))
            {
                child.ContextMenuStrip = ContextMenuStrip;
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanging"/> event.
        /// </summary>
        /// <param name="oldTime">The old time value.</param>
        /// <param name="newTime">The new time value.</param>
        /// <returns><c>true</c> if the value change is not canceled; otherwise, <c>false</c>.</returns>
        protected virtual bool OnValueChanging(TimeOnly oldTime, TimeOnly newTime)
        {
            if (ValueChanging is null)
                return true;

            var e = new TimeChangingEventArgs(oldTime, newTime);
            ValueChanging.Invoke(this, e);
            return !e.Cancel;
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
