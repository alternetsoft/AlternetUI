using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows the user to select a start date and time and an end date and time.
    /// Additionally, a duration can be specified, which will automatically update the end date and time.
    /// </summary>
    [ControlCategory(KnownControlCategory.DateTime)]
    public partial class DateTimeRangePicker : TransparentPanel
    {
        /// <summary>
        /// Gets or sets the default distance between the inner controls.
        /// </summary>
        public static float DefaultControlDistance = 5;

        private readonly DateTimePicker startPicker = new ();
        private readonly DateTimePicker endPicker = new ();
        private readonly Label startLabel = new(Localized.Starts);
        private readonly Label endLabel = new(Localized.Ends);

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRangePicker"/> class.
        /// </summary>
        public DateTimeRangePicker()
        {
            Layout = LayoutStyle.Vertical;

            startLabel.MarginBottom = DefaultControlDistance;
            startLabel.Parent = this;

            startPicker.MarginBottom = DefaultControlDistance;
            startPicker.Parent = this;

            endLabel.MarginBottom = DefaultControlDistance;
            endLabel.Parent = this;
            
            endPicker.Parent = this;

            Kind = DateTimePickerKind.DateTime;

            startPicker.ValueChanged += OnStartPickerChanged;
            endPicker.ValueChanged += OnEndPickerChanged;
        }

        /// <summary>
        /// Occurs when the value of either the start or end date and time picker changes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the start date and time value of the control.
        /// </summary>
        public DateTime? StartValue
        {
            get => startPicker.Value;
            set => startPicker.Value = value;
        }

        /// <summary>
        /// Gets or sets the end date and time value of the control.
        /// </summary>
        public DateTime? EndValue
        {
            get => endPicker.Value;
            set => endPicker.Value = value;
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
            }
        }

        /// <summary>
        /// Gets the label for the start date and time picker.
        /// </summary>
        public Label StartLabel => startLabel;

        /// <summary>
        /// Gets the label for the end date and time picker.
        /// </summary>
        public Label EndLabel => endLabel;

        /// <summary>
        /// Gets the start date and time picker.
        /// </summary>
        public DateTimePicker StartPicker => startPicker;

        /// <summary>
        /// Gets the end date and time picker.
        /// </summary>
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
                startPicker.Kind = value;
                endPicker.Kind = value;
            }
        }

        /// <summary>
        /// Called when the value of the end date and time picker changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnEndPickerChanged(object? sender, EventArgs e)
        {
            OnValueChanged();
        }

        /// <summary>
        /// Called when the value of the start date and time picker changes.
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"></param>
        protected virtual void OnStartPickerChanged(object? sender, EventArgs e)
        {
            OnValueChanged();
        }

        /// <summary>
        /// Called when the value of either the start or end date and time picker changes.
        /// Raises the <see cref="ValueChanged"/> event. Override this method in derived classes
        /// to provide custom behavior when the value changes.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
