using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a date or time and allows to change it.
    /// Use <see cref="Kind"/> property to specify whether to edit date or time part of the value.
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory(KnownControlCategory.Date)]
    public partial class DateTimePicker : GenericDateEdit
    {
        /// <summary>
        /// Gets or sets the default distance between date and time pickers in the <see cref="DateTimePicker"/> control.
        /// </summary>
        public static float DefaultDateTimeDistance = 5;

        /// <summary>
        /// Gets or sets the default margin for the date and time picker icons in the <see cref="DateTimePicker"/> control.
        /// </summary>
        public static Thickness DefaultIconMargin = (0, 0, 5, 0);

        private readonly DatePicker datePicker = new();
        private readonly TimePicker timePicker = new();
        private readonly TransparentPanel datePanel = new();
        private readonly TransparentPanel timePanel = new();
        private readonly TransparentPanel spacer = new();
        private readonly PictureBox datePictureBox = new();
        private readonly PictureBox timePictureBox = new();
        private readonly DateTimePickerPopupKind popupKind = DateTimePickerPopupKind.DropDown;

        private int suppressCounter;
        private DateTimePickerKind kind = DateTimePickerKind.Date;
        private DateTime? dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public DateTimePicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        public DateTimePicker()
        {
            Layout = LayoutStyle.Vertical;

            datePanel.Layout = LayoutStyle.Horizontal;

            datePictureBox.Visible = false;
            datePictureBox.VerticalAlignment = VerticalAlignment.Center;
            datePictureBox.Parent = datePanel;
            datePictureBox.Margin = DefaultIconMargin;

            datePicker.HorizontalAlignment = HorizontalAlignment.Fill;
            datePicker.Parent = datePanel;
            
            datePanel.Parent = this;

            spacer.SuggestedHeight = DefaultDateTimeDistance;
            spacer.HorizontalAlignment = HorizontalAlignment.Fill;
            spacer.Visible = false;
            spacer.Parent = this;

            timePictureBox.Visible = false;
            timePictureBox.VerticalAlignment = VerticalAlignment.Center;
            timePictureBox.Parent = timePanel;
            timePictureBox.Margin = DefaultIconMargin;

            timePanel.Layout = LayoutStyle.Horizontal;
            timePanel.Visible = false;
            timePicker.HorizontalAlignment = HorizontalAlignment.Fill;
            timePicker.Parent = timePanel;
            timePanel.Parent = this;

            datePicker.ValueChanged += OnDatePickerValueChanged;
            timePicker.ValueChanged += OnTimePickerValueChanged;

            Value = DateTime.Now;

            datePictureBox.SetSvgImage(DefaultDateIcon ?? KnownSvgImages.ImgCalendar);
            timePictureBox.SetSvgImage(DefaultTimeIcon ?? KnownSvgImages.ImgClock);
        }

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in
        /// some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the
        /// <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new
        /// value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the default icon for the date picker in the <see cref="DateTimePicker"/> control.
        /// </summary>
        public static SvgImage? DefaultDateIcon { get; set; }

        /// <summary>
        /// Gets or sets the default icon for the time picker in the <see cref="DateTimePicker"/> control.
        /// </summary>
        public static SvgImage? DefaultTimeIcon { get; set; }

        /// <summary>
        /// Gets the inner <see cref="DatePicker"/> control used to edit date part of the <see cref="Value"/>.
        /// </summary>
        [Browsable(false)]
        public DatePicker DatePicker => datePicker;

        /// <summary>
        /// Gets the inner <see cref="TimePicker"/> control used to edit time part of the <see cref="Value"/>.
        /// </summary>
        [Browsable(false)]
        public TimePicker TimePicker => timePicker;
        
        /// <summary>
        /// Gets the inner panel used to contain the date picker.
        /// </summary>
        [Browsable(false)]
        public TransparentPanel DatePanel => datePanel;

        /// <summary>
        /// Gets the inner panel used to contain the time picker.
        /// </summary>
        [Browsable(false)]
        public TransparentPanel TimePanel => timePanel;

        /// <summary>
        /// Gets the inner panel used as a spacer between the date and time pickers.
        /// </summary>
        [Browsable(false)]
        public TransparentPanel Spacer => spacer;

        /// <summary>
        /// Gets the inner picture box used to display the date picker icon.
        /// </summary>
        [Browsable(false)]
        public PictureBox DateIcon => datePictureBox;

        /// <summary>
        /// Gets the inner picture box used to display the time picker icon.
        /// </summary>
        [Browsable(false)]
        public PictureBox TimeIcon => timePictureBox;
        
        /// <summary>
        /// Gets or sets the value assigned to the <see cref="DateTimePicker"/>
        /// as a selected <see cref="DateTime"/>.
        /// </summary>
        public override DateTime? Value
        {
            get
            {
                return dateTime;
            }

            set
            {
                if (dateTime == value) return;
                dateTime = value;

                try
                {
                    suppressCounter++;
                    datePicker.Value = value;
                    timePicker.Value = value ?? DateTime.Now.Date;
                }
                finally
                {
                    suppressCounter--;
                }

                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets selected time as <see cref="TimeOnly"/>.
        /// </summary>
        public virtual TimeOnly? AsTimeOnly
        {
            get
            {
                var value = Value;

                if (value is null)
                    return null;
                return TimeOnly.FromDateTime(value.Value);
            }

            set
            {
                if (value is null)
                    Value = null;
                else
                {
                    Value = DateUtils.ToDateTime(time: value.Value, date: DateOnly.FromDateTime(Value ?? DateTime.Now));
                }
            }
        }

        /// <summary>
        /// Gets or sets selected date as <see cref="DateOnly"/>.
        /// </summary>
        public virtual DateOnly? AsDateOnly
        {
            get
            {
                var value = Value;

                if (value is null)
                    return null;
                return DateOnly.FromDateTime(value.Value);
            }

            set
            {
                if (value is null)
                    Value = null;
                else
                {
                    Value = DateUtils.ToDateTime(time: TimeOnly.FromDateTime(Value ?? DateTime.Now), date: value.Value);
                }
            }
        }

        /// <inheritdoc/>
        public override bool UseMinDate
        {
            get => base.UseMinDate;
            set
            {
                base.UseMinDate = value;
                datePicker.UseMinDate = value;
            }
        }

        /// <inheritdoc/>
        public override bool UseMaxDate
        {
            get => base.UseMaxDate;
            set
            {
                base.UseMaxDate = value;
                datePicker.UseMaxDate = value;
            }
        }

        /// <inheritdoc/>
        public override DateTime MinDate
        {
            get => base.MinDate;
            set
            {
                base.MinDate = value;
                datePicker.MinDate = value;
            }
        }

        /// <inheritdoc/>
        public override DateTime MaxDate
        {
            get => base.MaxDate;
            set
            {
                base.MaxDate = value;
                datePicker.MaxDate = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the picker is set to time only mode.
        /// </summary>
        [Browsable(false)]
        public bool IsTimeOnly
        {
            get => kind == DateTimePickerKind.Time;
        }

        /// <summary>
        /// Gets a value indicating whether the picker is set to date only mode.
        /// </summary>
        [Browsable(false)]
        public bool IsDateOnly
        {
            get => kind == DateTimePickerKind.Date;
        }

        /// <summary>
        /// Gets a value indicating whether the picker is set to date and time mode.
        /// </summary>
        [Browsable(false)]
        public bool IsDateTime
        {
            get => kind == DateTimePickerKind.DateTime;
        }

        /// <summary>
        /// Gets or sets whether to edit date part or time part of
        /// the <see cref="DateTime"/> value.
        /// </summary>
        public virtual DateTimePickerKind Kind
        {
            get
            {
                return kind;
            }

            set
            {
                if (kind == value)
                    return;
                kind = value;

                DoInsideLayout(() =>
                {
                    spacer.Visible = IsDateTime;
                    datePictureBox.Visible = IsDateTime;
                    timePictureBox.Visible = IsDateTime;
                    datePanel.Visible = IsDateTime || IsDateOnly;
                    timePanel.Visible = IsDateTime || IsTimeOnly;
                });
            }
        }

        /// <summary>
        /// Gets or sets whether to show calendar popup or edit date with spin control.
        /// Currently only <see cref="DateTimePickerPopupKind.DropDown"/> is implemented.
        /// </summary>
        [Browsable(false)]
        public virtual DateTimePickerPopupKind PopupKind
        {
            get
            {
                return popupKind;
            }

            set
            {
            }
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
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
        public override bool SetFocus()
        {
            return base.SetFocus();
        }

        /// <summary>
        /// Called when the value of the inner <see cref="TimePicker"/> has changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnTimePickerValueChanged(object? sender, EventArgs e)
        {
            if (suppressCounter > 0)
                return;
            AsTimeOnly = timePicker.AsTimeOnly;
        }

        /// <summary>
        /// Called when the value of the inner <see cref="DatePicker"/> has changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnDatePickerValueChanged(object? sender, EventArgs e)
        {
            if (suppressCounter > 0)
                return;
            AsDateOnly = datePicker.AsDateOnly;
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void SetRange(DateTime min, DateTime max)
        {
        }
    }
}
