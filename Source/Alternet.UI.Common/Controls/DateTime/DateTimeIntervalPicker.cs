using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows the user to select a start date and time and an end date and time.
    /// Additionally, a duration can be specified, which will automatically update the end date and time.
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory(KnownControlCategory.DateTime)]
    public partial class DateTimeIntervalPicker : TransparentPanel, IReadOnlyProperty
    {
        /// <summary>
        /// Gets or sets the default image for the duration picker.
        /// If not specified, the default known image will be used.
        /// </summary>
        public static SvgImage? DefaultDurationImage;

        /// <summary>
        /// Gets or sets the default distance between the inner controls.
        /// </summary>
        public static float DefaultControlDistance = 5;
        private bool valueChangedSuspended;
        private DateTimeInterval data;
        private bool isTimeZoneVisible = true;
        private readonly DateTimePicker startPicker;
        private readonly DateTimePicker endPicker;
        private readonly GenericControlAndImage<TimePeriodPicker> durationPicker
            = new(DefaultDurationImage ?? KnownSvgImages.ImgHourglassHalf);
        
        private readonly LinkLabel timeZoneLinkLabel = new(Localized.CalendarItemLabelTimeZone)
        {
            VerticalAlignment = VerticalAlignment.Center,
        };

        private readonly TransparentPanel timeZonePanel = new()
        {
            Layout = LayoutStyle.Horizontal,
            MarginTop = DefaultControlDistance,
        };

        private readonly TimeZonePicker timeZonePicker = new()
        {
            MarginLeft = DefaultControlDistance,
            IgnorePaint = true,
            Enabled = false,
            HorizontalAlignment = HorizontalAlignment.Fill,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeIntervalPicker"/> class.
        /// </summary>
        public DateTimeIntervalPicker()
        {
            data = new(DateUtils.NowWithoutSeconds);
            startPicker = CreateDateTimePicker();
            endPicker = CreateDateTimePicker();

            startPicker.DatePicker.UseSingleBottomBorder();
            endPicker.DatePicker.UseSingleBottomBorder();
            startPicker.TimePicker.UseSingleBottomBorder();
            endPicker.TimePicker.UseSingleBottomBorder();
            durationPicker.MainControl.UseSingleBottomBorder();
            timeZonePicker.UseSingleBottomBorder();

            Layout = LayoutStyle.Vertical;

            Kind = DateTimePickerKind.DateTime;

            SecondsVisible = false;

            startPicker.ForceIcons = true;
            endPicker.ForceIcons = true;

            startPicker.TimeIcon.IgnorePaint = true;
            endPicker.TimeIcon.IgnorePaint = true;

            startPicker.DateIcon.SvgImage = KnownSvgImages.ImgRangeStart;
            endPicker.DateIcon.SvgImage = KnownSvgImages.ImgRangeEnd;
            startPicker.TimeIcon.SvgImage = KnownSvgImages.ImgRangeStart;
            endPicker.TimeIcon.SvgImage = KnownSvgImages.ImgRangeEnd;

            startPicker.MarginBottom = DefaultControlDistance;
            startPicker.Parent = this;

            endPicker.MarginBottom = DefaultControlDistance;
            endPicker.Parent = this;

            durationPicker.MainControl.AutoUpdateColors = false;
            durationPicker.MainControl.UseControlColors(false);
            durationPicker.MainControl.ImageVisible = false;
            durationPicker.MainControl.UseTheme = SpeedButton.KnownTheme.Default;
            durationPicker.Parent = this;

            timeZoneLinkLabel.HasBorder = false;
            timeZoneLinkLabel.Parent = timeZonePanel;

            timeZonePicker.Parent = timeZonePanel;

            timeZonePanel.Parent = this;

            timeZonePicker.ValueChanged += OnTimeZonePickerValueChanged;

            timeZoneLinkLabel.LinkClicked += OnTimeZoneClicked;
            startPicker.ValueChanged += OnStartPickerChanged;
            endPicker.ValueChanged += OnEndPickerChanged;
            durationPicker.MainControl.ValueChanged += OnDurationPickerChanged;
            startPicker.TimePicker.ValueChanging += OnStartTimePickerValueChanging;
            endPicker.TimePicker.ValueChanging += OnEndTimePickerValueChanging;
            startPicker.DatePicker.ValueChanging += OnStartDatePickerValueChanging;
            endPicker.DatePicker.ValueChanging += OnEndDatePickerValueChanging;

            OnValueChanged();
        }

        /// <summary>
        /// Occurs when the value of either the start or end date and time picker changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets the duration picker control.
        /// </summary>
        public GenericControlAndImage<TimePeriodPicker> DurationPicker => durationPicker;

        /// <summary>
        /// Gets or sets a value indicating whether the seconds part of the time is visible.
        /// </summary>
        public virtual bool SecondsVisible
        {
            get => startPicker.SecondsVisible;
            set
            {
                startPicker.SecondsVisible = value;
                endPicker.SecondsVisible = value;
            }
        }

        /// <summary>
        /// Gets the time zone link label control.
        /// </summary>
        public LinkLabel TimeZoneLinkLabel => timeZoneLinkLabel;

        /// <summary>
        /// Gets the time zone picker control.
        /// </summary>
        public TimeZonePicker TimeZonePicker => timeZonePicker;

        /// <summary>
        /// Gets the time zone panel control where the time zone link label and time zone picker are placed.
        /// </summary>
        public TransparentPanel TimeZonePanel => timeZonePanel;

        /// <summary>
        /// Gets or sets a value indicating whether the time zone panel is visible.
        /// </summary>
        public virtual bool IsTimeZoneVisible
        {
            get => isTimeZoneVisible;
            set
            {
                if (isTimeZoneVisible == value)
                    return;
                isTimeZoneVisible = value;
                UpdateTimeZoneVisibility();
            }
        }

        /// <summary>
        /// Gets or sets the end date for the display purposes.
        /// </summary>
        [Browsable(false)]
        public virtual DateTime DisplayEndValue
        {
            get
            {
                return IsDateOnly
                ? EndValue.Date.AddDays(-1)
                : EndValue;
            }

            set
            {
                if (IsDateOnly)
                {
                    var newValue = value.Date.AddDays(1);
                    EndValue = newValue;
                }
                else
                {
                    EndValue = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTimeInterval"/> value of the control,
        /// which includes the start date and time and the duration.
        /// </summary>
        [Browsable(false)]
        public virtual DateTimeInterval Value
        {
            get => data;
            set
            {
                if (data == value)
                    return;
                data = value;
                OnValueChanged();
            }
        }

        /// <summary>
        /// Gets or sets the duration of the interval represented by the control.
        /// </summary>
        public virtual TimeSpan Duration
        {
            get => data.Duration;
            set
            {
                Value = new DateTimeInterval(data.Start, value, Value.TimeZone);
            }
        }

        /// <summary>
        /// Gets or sets the start date and time value of the control.
        /// </summary>
        public virtual DateTime StartValue
        {
            get => data.Start;
            set
            {
                Value = new DateTimeInterval(value, data.Duration, Value.TimeZone);
            }
        }

        /// <summary>
        /// Gets or sets the end date and time value of the control.
        /// This is calculated based on the start value and the duration, so setting
        /// this value will adjust the duration accordingly.
        /// </summary>
        public virtual DateTime EndValue
        {
            get => data.End;
            set
            {
                Value = new DateTimeInterval(data.Start, value - data.Start, Value.TimeZone);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the time icon should be ignored when painting the control.
        /// </summary>
        public virtual bool IgnoreTimeIconPaint
        {
            get => startPicker.TimeIcon.IgnorePaint;
            set
            {
                startPicker.TimeIcon.IgnorePaint = value;
                endPicker.TimeIcon.IgnorePaint = value;
            }
        }

        /// <summary>
        /// Gets or sets the format provider used for formatting the date.
        /// </summary>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider
        {
            get
            {
                return startPicker.FormatProvider;
            }

            set
            {
                startPicker.FormatProvider = value;
                endPicker.FormatProvider = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is read-only.
        /// </summary>
        public virtual bool ReadOnly
        {
            get
            {
                return startPicker.ReadOnly;
            }
            set
            {
                startPicker.ReadOnly = value;
                endPicker.ReadOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in the control is transparent.
        /// </summary>
        public virtual bool TextTransparent
        {
            get
            {
                return startPicker.TextTransparent;
            }

            set
            {
                startPicker.TextTransparent = value;
                endPicker.TextTransparent = value;
                durationPicker.MainControl.TextTransparent = value;
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
                return startPicker.Format;
            }

            set
            {
                startPicker.Format = value;
                endPicker.Format = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the date icon is forced to be visible.
        /// </summary>
        public virtual bool ForceDateIcon
        {
            get => startPicker.ForceDateIcon;
            set
            {
                startPicker.ForceDateIcon = value;
                endPicker.ForceDateIcon = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the time icon is forced to be visible.
        /// </summary>
        public virtual bool ForceTimeIcon
        {
            get => startPicker.ForceTimeIcon;
            set
            {
                startPicker.ForceTimeIcon = value;
                endPicker.ForceTimeIcon = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should force icons to be visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool ForceIcons
        {
            get => startPicker.ForceIcons;
            set
            {
                startPicker.ForceIcons = value;
                endPicker.ForceIcons = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the duration picker is visible.
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowDuration
        {
            get => durationPicker.Visible;
            set => durationPicker.Visible = value;
        }

        /// <summary>
        /// Gets the start date and time picker.
        /// </summary>
        [Browsable(false)]
        public DateTimePicker StartPicker => startPicker;

        /// <summary>
        /// Gets the end date and time picker.
        /// </summary>
        [Browsable(false)]
        public DateTimePicker EndPicker => endPicker;

        /// <summary>
        /// Gets or sets a value indicating whether the control should use the minimum date and time value.
        /// </summary>
        public virtual bool UseMinDate
        {
            get => startPicker.UseMinDate;
            set
            {
                startPicker.UseMinDate = value;
                endPicker.UseMinDate = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should use the maximum date and time value.
        /// </summary>
        public virtual bool UseMaxDate
        {
            get => startPicker.UseMaxDate;
            set
            {
                startPicker.UseMaxDate = value;
                endPicker.UseMaxDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum date and time value that can be selected in the control.
        /// </summary>
        public virtual DateTime MinDate
        {
            get => startPicker.MinDate;
            set
            {
                startPicker.MinDate = value;
                endPicker.MinDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum date and time value that can be selected in the control.
        /// </summary>
        public virtual DateTime MaxDate
        {
            get => startPicker.MaxDate;
            set
            {
                startPicker.MaxDate = value;
                endPicker.MaxDate = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the picker is set to time only mode.
        /// </summary>
        [Browsable(false)]
        public bool IsTimeOnly
        {
            get => Kind == DateTimePickerKind.Time;
        }

        /// <summary>
        /// Gets a value indicating whether the time part of the date and time is visible.
        /// </summary>
        public bool IsTimeVisible
        {
            get => Kind == DateTimePickerKind.DateTime || Kind == DateTimePickerKind.Time;

            set
            {
                if (value)
                    Kind = DateTimePickerKind.DateTime;
                else
                    Kind = DateTimePickerKind.Date;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the picker is set to date only mode.
        /// </summary>
        [Browsable(false)]
        public bool IsDateOnly
        {
            get => Kind == DateTimePickerKind.Date;
        }

        /// <summary>
        /// Gets a value indicating whether the picker is set to date and time mode.
        /// </summary>
        [Browsable(false)]
        public bool IsDateTime
        {
            get => Kind == DateTimePickerKind.DateTime;
        }

        /// <summary>
        /// Gets or sets whether to edit date part or time part of
        /// the <see cref="DateTime"/> value.
        /// </summary>
        public virtual DateTimePickerKind Kind
        {
            get
            {
                return startPicker.Kind;
            }

            set
            {
                if (Kind == value)
                    return;

                DoInsideLayout(() =>
                {
                    startPicker.Kind = value;
                    endPicker.Kind = value;

                    switch (value)
                    {
                        case DateTimePickerKind.Date:
                            Value = new DateTimeInterval(Value.Start.Date, TimeSpan.FromDays(1), Value.TimeZone);
                            break;
                        case DateTimePickerKind.Time:
                            break;
                        case DateTimePickerKind.DateTime:
                            break;
                    }

                    IgnoreTimeIconPaint = value != DateTimePickerKind.Time;
                });

                durationPicker.MainControl.ReadOnly = value != DateTimePickerKind.DateTime;
                durationPicker.Visible = value != DateTimePickerKind.Time;
                UpdateTimeZoneVisibility();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the time zone panel is visible.
        /// </summary>
        protected virtual void UpdateTimeZoneVisibility()
        {
            timeZonePanel.Visible = Kind != DateTimePickerKind.Date && IsTimeZoneVisible;
        }

        /// <summary>
        /// Called when the value of the end time picker is changing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="TimeChangingEventArgs"/> that contains the event data.</param>
        protected virtual void OnEndTimePickerValueChanging(object? sender, TimeChangingEventArgs e)
        {
            var canChange = DateUtils.CanChangeEndTime(startPicker.Value, endPicker.Value, e.NewValue);
            e.Cancel = !canChange;
        }

        /// <summary>
        /// Called when the value of the start time picker is changing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="TimeChangingEventArgs"/> that contains the event data.</param> 
        protected virtual void OnStartTimePickerValueChanging(object? sender, TimeChangingEventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the start date and time picker changes.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> An <see cref="EventArgs"/> that contains the event data. </param>
        protected virtual void OnStartPickerChanged(object? sender, EventArgs e)
        {
            if (valueChangedSuspended)
                return;
            StartValue = startPicker.Value ?? DateUtils.NowWithoutSeconds;
        }

        /// <summary>
        /// Called when the value of the end date and time picker changes.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> An <see cref="EventArgs"/> that contains the event data. </param>
        protected virtual void OnEndPickerChanged(object? sender, EventArgs e)
        {
            if (valueChangedSuspended)
                return;
            DisplayEndValue = endPicker.Value ?? DateUtils.NowWithoutSeconds;
        }

        /// <summary>
        /// Called when the value of the duration picker changes.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> An <see cref="EventArgs"/> that contains the event data. </param>
        protected virtual void OnDurationPickerChanged(object? sender, EventArgs e)
        {
            if (valueChangedSuspended)
                return;

            var duration = durationPicker.MainControl.Value.Value ?? TimeSpan.Zero;
            Value = new DateTimeInterval(StartValue, duration, Value.TimeZone);
        }

        /// <summary>
        /// Creates a new instance of the inner <see cref="DateTimePicker"/> control
        /// used for selecting the start and end date and time values.
        /// </summary>
        /// <returns></returns>
        protected virtual DateTimePicker CreateDateTimePicker()
        {
            return new DateTimePicker();
        }

        /// <summary>
        /// Called when the value of the end date picker is changing.
        /// </summary>
        /// <param name="sender"> The source of the event.</param>
        /// <param name="e">The <see cref="NullableDateTimeChangingEventArgs"/>
        /// instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void OnEndDatePickerValueChanging(object? sender, NullableDateTimeChangingEventArgs e)
        {
            if (e.NewValue is null)
                return;

            var canChange = DateUtils.CanChangeEndDate(StartValue, EndValue, e.NewValue.Value.ToDateOnly());
            e.Cancel = !canChange;
        }

        /// <summary>
        /// Called when the value of the time zone picker changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnTimeZonePickerValueChanged(object? sender, EventArgs e)
        {
            Value = new DateTimeInterval(Value, timeZonePicker.Value);
        }

        /// <summary>
        /// Called when the value of the start date picker is changing.
        /// </summary>
        /// <param name="sender"> The source of the event.</param>
        /// <param name="e">The <see cref="NullableDateTimeChangingEventArgs"/>
        /// instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual void OnStartDatePickerValueChanging(object? sender, NullableDateTimeChangingEventArgs e)
        {
        }

        /// <summary>
        /// Called when the time zone link label is clicked.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> A <see cref="CancelEventArgs"/> that contains the event data. </param>
        protected virtual void OnTimeZoneClicked(object? sender, CancelEventArgs e)
        {
            if (Value.TimeZone is null)
            {
                Value = new DateTimeInterval(Value, TimeZoneInfo.Local);
            }
            else
            {
                Value = new DateTimeInterval(Value, (TimeZoneInfo?)null);
            }
        }

        /// <summary>
        /// Called when the value of either the start or end date and time picker changes.
        /// Raises the <see cref="ValueChanged"/> event. Override this method in derived classes
        /// to provide custom behavior when the value changes.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (valueChangedSuspended)
                return;

            valueChangedSuspended = true;

            try
            {
                endPicker.MinDate = StartValue;
                endPicker.UseMinDate = true;
                startPicker.Value = StartValue;
                endPicker.Value = DisplayEndValue;

                durationPicker.MainControl.Value = new(Value.Duration);

                timeZonePicker.Value = Value.TimeZone;
                timeZonePicker.IgnorePaint = Value.TimeZone is null;
                timeZonePicker.Enabled = Value.TimeZone is not null;

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                valueChangedSuspended = false;
            }
        }
    }
}
