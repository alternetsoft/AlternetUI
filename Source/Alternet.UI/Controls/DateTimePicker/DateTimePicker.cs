using Alternet.Base.Collections;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected date and allows to change it.
    /// </summary>
    public class DateTimePicker : Control
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value", // Property name
                    typeof(DateTime), // Property type
                    typeof(DateTimePicker), // Property owner
                    new FrameworkPropertyMetadata(
                            DateTime.MinValue, // default value
                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnValuePropertyChanged),    // property changed callback
                            null, // CoerseValueCallback
                            true, // IsAnimationProhibited
                            UpdateSourceTrigger.PropertyChanged
                            //UpdateSourceTrigger.LostFocus   // DefaultUpdateSourceTrigger
                            ));

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the value assigned to the color picker as a selected color.
        /// </summary>
        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
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

        internal new NativeDateTimePickerHandler Handler =>
            (NativeDateTimePickerHandler)base.Handler;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Callback for changes to the Value property
        /// </summary>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker control = (DateTimePicker)d;
            control.OnValuePropertyChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        private void OnValuePropertyChanged(DateTime oldValue, DateTime newValue)
        {
            RaiseValueChanged(EventArgs.Empty);
        }
    }
}
