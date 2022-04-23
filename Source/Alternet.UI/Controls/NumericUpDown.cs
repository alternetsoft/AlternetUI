using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an up-down control (also known as spin box) that displays numeric values.
    /// </summary>
    /// <remarks>
    /// A <see cref="NumericUpDown"/> control contains a single numeric value that can be incremented or decremented
    /// by clicking the up or down buttons of the control.
    /// To specify the allowable range of values for the control, set the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
    /// </remarks>
    public class NumericUpDown : Control
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value", // Property name
                    typeof(decimal), // Property type
                    typeof(NumericUpDown), // Property owner
                    new FrameworkPropertyMetadata( // Property metadata
                            0m, // default value
                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | // Flags
                                FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnValuePropertyChanged),    // property changed callback
                            new CoerceValueCallback(CoerceValue),
                            true, // IsAnimationProhibited
                            UpdateSourceTrigger.PropertyChanged
                            //UpdateSourceTrigger.LostFocus   // DefaultUpdateSourceTrigger
                            ));

        private decimal minimum;

        private decimal maximum = 100;

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new value that is read by the control.</remarks>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Minimum"/> property changes.
        /// </summary>
        public event EventHandler? MinimumChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Maximum"/> property changes.
        /// </summary>
        public event EventHandler? MaximumChanged;

        /// <summary>
        /// Gets or sets the value assigned to the spin box (also known as an up-down control).
        /// </summary>
        /// <value>The numeric value of the <see cref="NumericUpDown"/> control.</value>
        /// <remarks>When the <see cref="Value"/> property is set, the new value is validated
        /// to be between the <see cref="Minimum"/> and <see cref="Maximum"/> values.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value specified is greater than the value of the <see cref="Maximum"/> property or the value specified is less than the value of the <see cref="Minimum"/> property.
        /// </exception>
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the numeric up-down control.
        /// </summary>
        /// <value>The minimum allowed value for the numeric up-down control. The default value is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value, the <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public decimal Minimum
        {
            get
            {
                CheckDisposed();
                return minimum;
            }

            set
            {
                CheckDisposed();
                if (minimum == value)
                    return;

                minimum = value;

                if (value > Maximum)
                    Maximum = value;
                if (Value < value)
                    Value = value;

                MinimumChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the numeric up-down control.
        /// </summary>
        /// <value>The maximum allowed value for the numeric up-down control. The default value is 100.</value>
        /// <remarks>
        ///  If the <see cref="Minimum"/> property is greater than the new <see cref="Maximum"/> property, the <see cref="Minimum"/>
        ///  property value is set equal to the <see cref="Maximum"/> value.
        ///  If the current <see cref="Value"/> is greater than the new <see cref="Maximum"/> value, the <see cref="Value"/> property
        ///  value is set equal to the <see cref="Maximum"/> value.
        /// </remarks>
        public decimal Maximum
        {
            get
            {
                CheckDisposed();
                return maximum;
            }

            set
            {
                CheckDisposed();
                if (maximum == value)
                    return;

                maximum = value;

                if (value < Minimum)
                    Minimum = value;
                if (Value > value)
                    Value = value;

                MaximumChanged?.Invoke(this, EventArgs.Empty);
            }
        }

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
            NumericUpDown control = (NumericUpDown)d;
            control.OnValuePropertyChanged((decimal)e.OldValue, (decimal)e.NewValue);
        }

        private static object CoerceValue(DependencyObject d, object value)
        {
            var o = (NumericUpDown)d;

            var intValue = (decimal)value;
            if (intValue < o.Minimum)
                return o.Minimum;

            if (intValue > o.Maximum)
                return o.Maximum;

            return value;
        }

        private void OnValuePropertyChanged(decimal oldValue, decimal newValue)
        {
            RaiseValueChanged(EventArgs.Empty);
        }
    }
}