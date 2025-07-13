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
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Common")]
    public partial class DateTimePicker : CustomDateEdit
    {
        private readonly DatePicker datePicker = new();
        private readonly TimePicker timePicker = new();

        private DateTimePickerKind kind = DateTimePickerKind.Date;
        private DateTimePickerPopupKind popupKind = DateTimePickerPopupKind.DropDown;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public DateTimePicker(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        public DateTimePicker()
        {
            datePicker.Parent = this;
            timePicker.Visible = false;
            timePicker.Parent = this;

            timePicker.SizeChanged += (s, e) =>
            {
            };

            datePicker.ValueChanged += (s, e) =>
            {
                timePicker.Value = Value ?? DateTime.Now;
                RaiseValueChanged(e);
            };

            timePicker.ValueChanged += (s, e) =>
            {
                Value = (Value ?? DateTime.Now).Date + timePicker.Value.TimeOfDay;
            };
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

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.DateTimePicker;

        /// <summary>
        /// Gets or sets the value assigned to the <see cref="DateTimePicker"/>
        /// as a selected <see cref="DateTime"/>.
        /// </summary>
        public override DateTime? Value
        {
            get
            {
                return datePicker.Value;
            }

            set
            {
                datePicker.Value = value;
            }
        }

        /// <inheritdoc/>
        public override bool UseMinDate
        {
            get => datePicker.UseMinDate;
            set => datePicker.UseMinDate = value;
        }

        /// <inheritdoc/>
        public override bool UseMaxDate
        {
            get => datePicker.UseMaxDate;
            set => datePicker.UseMaxDate = value;
        }

        /// <inheritdoc/>
        public override DateTime MinDate
        {
            get => datePicker.MinDate;
            set => datePicker.MinDate = value;
        }

        /// <inheritdoc/>
        public override DateTime MaxDate
        {
            get => datePicker.MaxDate;
            set => datePicker.MaxDate = value;
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

                var isDate = kind == DateTimePickerKind.Date;

                DoInsideLayout(() =>
                {
                    timePicker.Visible = !isDate;
                    datePicker.Visible = isDate;
                });
            }
        }

        /// <summary>
        /// Gets or sets whether to show calendar popup or edit date with spin control.
        /// Currently only <see cref="DateTimePickerPopupKind.DropDown"/> is implemented.
        /// </summary>
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
            if(datePicker.Visible)
                return datePicker.SetFocus();
            else
                return timePicker.SetFocus();
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
        protected override void SetRange(DateTime min, DateTime max)
        {
        }
    }
}
