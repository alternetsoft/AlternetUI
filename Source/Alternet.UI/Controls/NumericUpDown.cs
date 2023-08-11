using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an up-down control (also known as spin box) that displays
    /// numeric values.
    /// </summary>
    /// <remarks>
    /// A <see cref="NumericUpDown"/> control contains a single numeric value
    /// that can be incremented or decremented
    /// by clicking the up or down buttons of the control.
    /// To specify the allowable range of values for the control, set the
    /// <see cref="Minimum"/> and <see cref="Maximum"/> properties.
    /// </remarks>
    public class NumericUpDown : Control
    {
        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(decimal),
                    typeof(NumericUpDown),
                    new FrameworkPropertyMetadata(
                            0m,
                            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnValuePropertyChanged),
                            new CoerceValueCallback(CoerceValue),
                            isAnimationProhibited: true,
                            UpdateSourceTrigger.PropertyChanged));

        /*/// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                    "Minimum",
                    typeof(decimal),
                    typeof(NumericUpDown),
                    new FrameworkPropertyMetadata(
                            0m,
                            FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnMinimumPropertyChanged),
                            coerceValueCallback: null,
                            isAnimationProhibited: true,
                            UpdateSourceTrigger.PropertyChanged));*/

        /*/// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                    "Maximum",
                    typeof(decimal),
                    typeof(NumericUpDown),
                    new FrameworkPropertyMetadata( // Property metadata
                            100m,
                            FrameworkPropertyMetadataOptions.AffectsPaint,
                            new PropertyChangedCallback(OnMaximumPropertyChanged),
                            coerceValueCallback: CoerceMaximum,
                            isAnimationProhibited: true,
                            UpdateSourceTrigger.PropertyChanged));*/

        private decimal minimum = 0m;
        private decimal maximum = 100m;

        /// <summary>
        /// Occurs when the <see cref="Value"/> property has been changed in some way.
        /// </summary>
        /// <remarks>For the <see cref="ValueChanged"/> event to occur, the
        /// <see cref="Value"/> property can be changed in code,
        /// by clicking the up or down button, or by the user entering a new value
        /// that is read by the control.</remarks>
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
        /// Gets or sets the value assigned to the spin box (also known as an
        /// up-down control).
        /// </summary>
        /// <value>The numeric value of the <see cref="NumericUpDown"/>
        /// control.</value>
        /// <remarks>When the <see cref="Value"/> property is set, the new
        /// value is validated
        /// to be between the <see cref="Minimum"/> and <see cref="Maximum"/>
        /// values.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value specified is greater than the value of the
        /// <see cref="Maximum"/> property or the value specified is
        /// less than the value of the <see cref="Minimum"/> property.
        /// </exception>
        public decimal Value
        {
            get
            {
                return (decimal)GetValue(ValueProperty);
            }

            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.NumericUpDown;

        /// <summary>
        /// Gets or sets the minimum allowed value for the numeric up-down control.
        /// </summary>
        /// <value>The minimum allowed value for the numeric up-down control.
        /// The default value is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified
        /// by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the
        /// <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the
        /// <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new
        /// <see cref="Minimum"/> value, the <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public decimal Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (value > maximum)
                    value = maximum;
                if (minimum == value)
                    return;
                minimum = value;
                if (Value < minimum)
                    Value = minimum;
                RaiseMinimumChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the numeric up-down control.
        /// </summary>
        /// <value>The maximum allowed value for the numeric up-down control.
        /// The default value is 100.</value>
        /// <remarks>
        ///  If the <see cref="Minimum"/> property is greater than the new
        ///  <see cref="Maximum"/> property, the <see cref="Minimum"/>
        ///  property value is set equal to the <see cref="Maximum"/> value.
        ///  If the current <see cref="Value"/> is greater than the new
        ///  <see cref="Maximum"/> value, the <see cref="Value"/> property
        ///  value is set equal to the <see cref="Maximum"/> value.
        /// </remarks>
        public decimal Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                if (value < minimum)
                    value = minimum;
                if (maximum == value)
                    return;
                maximum = value;
                if (Value > maximum)
                    Value = maximum;
                RaiseMaximumChanged(EventArgs.Empty);
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

        internal new NativeNumericUpDownHandler Handler =>
            (NativeNumericUpDownHandler)base.Handler;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
            Refresh();
            Update();
        }

        /// <summary>
        /// Increments or decrements value.
        /// </summary>
        /// <param name="incValue">Delta to add to the value.</param>
        public void IncrementValue(decimal incValue = 1)
        {
            decimal newValue = Value + incValue;

            if (newValue < Minimum)
                Value = Minimum;
            else
            if (newValue > Maximum)
                Value = Maximum;
            else
                Value = newValue;
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the minimum of the <see cref="Minimum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnMinimumChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the maximum of the <see cref="Maximum"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnMaximumChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().
                CreateNumericUpDownHandler(this);
        }

        /*private static object CoerceMaximum(DependencyObject d, object value)
        {
            var o = (NumericUpDown)d;

            decimal min = o.Minimum;
            if ((decimal)value < min)
            {
                return min;
            }

            return value;
        }*/

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

        /*/// <summary>
        /// Callback for changes to the Maximum property
        /// </summary>
        private static void OnMaximumPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)d;
            control.OnMaximumPropertyChanged((decimal)e.OldValue, (decimal)e.NewValue);
        }*/

        /*/// <summary>
        /// Callback for changes to the Minimum property
        /// </summary>
        private static void OnMinimumPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)d;
            control.OnMinimumPropertyChanged((decimal)e.OldValue, (decimal)e.NewValue);
        }*/

        /// <summary>
        /// Callback for changes to the Value property
        /// </summary>
        private static void OnValuePropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)d;
            control.OnValuePropertyChanged((decimal)e.OldValue, (decimal)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnValuePropertyChanged(decimal oldValue, decimal newValue)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            RaiseValueChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event and calls
        /// <see cref="OnMinimumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        private void RaiseMinimumChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnMinimumChanged(e);
            MinimumChanged?.Invoke(this, e);
        }

        /*private void OnMinimumPropertyChanged(decimal oldValue, decimal newValue)
        {
            RaiseMinimumChanged(EventArgs.Empty);
        }*/

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event and calls
        /// <see cref="OnMaximumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        private void RaiseMaximumChanged(EventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnMaximumChanged(e);
            MaximumChanged?.Invoke(this, e);
        }

        /*private void OnMaximumPropertyChanged(decimal oldValue, decimal newValue)
        {
            RaiseMaximumChanged(EventArgs.Empty);
        }*/
    }
}