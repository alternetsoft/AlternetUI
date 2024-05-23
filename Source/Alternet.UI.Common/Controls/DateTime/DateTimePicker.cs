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
        private DateTime value = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        public DateTimePicker()
        {
            if (BaseApplication.IsWindowsOS && BaseApplication.PlatformKind == UIPlatformKind.WxWidgets)
                UserPaint = true;
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
        public override DateTime Value
        {
            get
            {
                return value;
            }

            set
            {
                if (value == this.value)
                    return;
                this.value = value;
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether to edit date part or time part of
        /// the <see cref="DateTime"/> value.
        /// </summary>
        public virtual DateTimePickerKind Kind
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

        /// <summary>
        /// Gets or sets whether to show calendar popup or edit date with spin control.
        /// </summary>
        public virtual DateTimePickerPopupKind PopupKind
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

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        internal bool HasBorder
        {
            get => Handler.HasBorder;
            set => Handler.HasBorder = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        internal new IDateTimePickerHandler Handler =>
            (IDateTimePickerHandler)base.Handler;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return NativePlatform.Default.CreateDateTimePickerHandler(this);
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
            Handler.SetRange(min, max, UseMinDate, UseMaxDate);
        }
    }
}
