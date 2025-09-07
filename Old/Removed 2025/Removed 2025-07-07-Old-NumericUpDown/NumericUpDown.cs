using System;
using System.ComponentModel;

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
    [ControlCategory("Common")]
    public partial class NumericUpDown : Control
    {
        private int minimum = 0;
        private int maximum = 100;
        private int val = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public NumericUpDown(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        public NumericUpDown()
        {
        }

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
        public virtual int Value
        {
            get
            {
                return val;
            }

            set
            {
                value = CoerceValue(value);
                if (this.val == value)
                    return;
                this.val = value;
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.NumericUpDown;

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
        public virtual int Minimum
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
        public virtual int Maximum
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

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
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
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
            Refresh();
            Update();
        }

        /// <summary>
        /// Increments or decrements value.
        /// </summary>
        /// <param name="incValue">Delta to add to the value.</param>
        public virtual void IncrementValue(int incValue = 1)
        {
            var newValue = Value + incValue;

            if (newValue < Minimum)
                Value = Minimum;
            else
            if (newValue > Maximum)
                Value = Maximum;
            else
                Value = newValue;
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateNumericUpDownHandler(this);
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

        /// <summary>
        /// Coerces value to fit in the allowed bounds.
        /// </summary>
        /// <param name="value">Value to coerce.</param>
        /// <returns></returns>
        protected virtual int CoerceValue(int value)
        {
            if (value < Minimum)
                return Minimum;

            if (value > Maximum)
                return Maximum;

            return value;
        }

        /// <summary>
        /// Raises the <see cref="MinimumChanged"/> event and calls
        /// <see cref="OnMinimumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        private void RaiseMinimumChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMinimumChanged(e);
            MinimumChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MaximumChanged"/> event and calls
        /// <see cref="OnMaximumChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        private void RaiseMaximumChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnMaximumChanged(e);
            MaximumChanged?.Invoke(this, e);
        }
    }
}