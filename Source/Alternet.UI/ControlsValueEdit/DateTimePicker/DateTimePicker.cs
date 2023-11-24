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
    public class DateTimePicker : CustomDateEdit
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(DateTime),
                typeof(DateTimePicker),
                new FrameworkPropertyMetadata(
                    DateTime.Now, // default value
                    PropMetadataOption.BindsTwoWayByDefault | PropMetadataOption.AffectsPaint,
                    new PropertyChangedCallback(OnValuePropertyChanged),
                    null, // CoerseValueCallback
                    true, // IsAnimationProhibited
                    UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePicker"/> class.
        /// </summary>
        public DateTimePicker()
        {
            if (Application.IsWindowsOS)
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
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether to edit date part or time part of
        /// the <see cref="DateTime"/> value.
        /// </summary>
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

        /// <summary>
        /// Gets or sets whether to show calendar popup or edit date with spin control.
        /// </summary>
        public DateTimePickerPopupKind PopupKind
        {
            get
            {
                return Handler.PopupKind;
            }

            set
            {
                if(!Application.IsWindowsOS)
                {
                    value = DateTimePickerPopupKind.Default;
                }

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

        /// <summary>
        /// Binds <see cref="Value"/> to the specified property of the
        /// <see cref="FrameworkElement.DataContext"/>
        /// </summary>
        /// <param name="propName">Property name.</param>
        public void BindValue(string propName)
        {
            Binding myBinding = new(propName) { Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(this, DateTimePicker.ValueProperty, myBinding);
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
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateDateTimePickerHandler(this);
        }

        /// <inheritdoc/>
        protected override void SetRange(DateTime min, DateTime max)
        {
            Handler.SetRange(min, max, UseMinDate, UseMaxDate);
        }

        /// <summary>
        /// Callback for changes to the Value property
        /// </summary>
        private static void OnValuePropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker control = (DateTimePicker)d;
            control.RaiseValueChanged(EventArgs.Empty);
        }
    }
}
