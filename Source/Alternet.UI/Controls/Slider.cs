using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a slider control (also known as track bar).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Slider"/> is a scrollable control similar to the scroll bar control.
    /// You can configure ranges through which the value of the <see cref="Value"/> property of a
    /// slider scrolls by setting the <see cref="Minimum"/> property to specify the lower end
    /// of the range and the <see cref="Maximum"/> property to specify the upper end of the range.
    /// </para>
    /// <para>
    /// The slider can be displayed horizontally or vertically.
    /// </para>
    /// <para>
    /// You can use this control to input numeric data obtained through the <see cref="Value"/> property.
    /// You can display this numeric data in a control or use it in code.
    /// </para>
    /// </remarks>
    public class Slider : Control
    {
        private int value;

        private int minimum;

        private int maximum = 10;

        private int smallChange = 1;

        private int largeChange = 5;

        private int tickFrequency = 1;

        /// <summary>
        /// Occurs when the <see cref="Value"/> property of a slider changes,
        /// either by movement of the scroll box or by manipulation in code.
        /// </summary>
        /// <remarks>You can use this event to update other controls when the value represented in the slider changes.</remarks>
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
        /// Occurs when the value of the <see cref="SmallChange"/> property changes.
        /// </summary>
        public event EventHandler? SmallChangeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="LargeChange"/> property changes.
        /// </summary>
        public event EventHandler? LargeChangeChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TickFrequency"/> property changes.
        /// </summary>
        public event EventHandler? TickFrequencyChanged;

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the scroll box on the slider.
        /// </summary>
        /// <value>A numeric value that is within the <see cref="Minimum"/> and <see cref="Maximum"/> range. The default value is 0.</value>
        /// <remarks>The <see cref="Value"/> property contains the number that represents the current position of the scroll box on the slider.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The value specified is greater than the value of the <see cref="Maximum"/> property or the value specified is less than the value of the <see cref="Minimum"/> property.
        /// </exception>
        public int Value
        {
            get
            {
                CheckDisposed();
                return value;
            }

            set
            {
                CheckDisposed();

                if (value < Minimum || value > Maximum)
                    throw new ArgumentOutOfRangeException(nameof(value));

                if (this.value == value)
                    return;

                this.value = value;
                RaiseValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the lower limit of the range this <see cref="Slider"/> is working with.
        /// </summary>
        /// <value>The minimum value for the <see cref="Slider"/>. The default is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value, the <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public int Minimum
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
        /// Gets or sets the upper limit of the range this <see cref="Slider"/> is working with.
        /// </summary>
        /// <value>The maximum value for the <see cref="Slider"/>. The default is 10.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// If the new <see cref="Minimum"/> property value is greater than the <see cref="Maximum"/> property value,
        /// the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        /// If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value, the <see cref="Value"/> property
        /// is also set equal to the <see cref="Minimum"/> value.
        /// </remarks>
        public int Maximum
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
        /// Gets or sets the value added to or subtracted from the <see cref="Value"/> property
        /// when the scroll box is moved a small distance.
        /// </summary>
        /// <value>A numeric value. The default value is 1.</value>
        /// <exception cref="ArgumentException">The assigned value is less than 0.</exception>
        public int SmallChange
        {
            get
            {
                CheckDisposed();
                return smallChange;
            }

            set
            {
                CheckDisposed();

                if (value < 0)
                    throw new ArgumentException(nameof(value));

                if (smallChange == value)
                    return;

                smallChange = value;
                SmallChangeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the <see cref="Value"/> property
        /// when the scroll box is moved a large distance.
        /// </summary>
        /// <value>A numeric value. The default is 5.</value>
        /// <exception cref="ArgumentException">The assigned value is less than 0.</exception>
        public int LargeChange
        {
            get
            {
                CheckDisposed();
                return largeChange;
            }

            set
            {
                CheckDisposed();

                if (value < 0)
                    throw new ArgumentException(nameof(value));

                if (largeChange == value)
                    return;

                largeChange = value;
                LargeChangeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies the delta between ticks drawn on the control.
        /// </summary>
        /// <value>The numeric value representing the delta between ticks. The default is 1.</value>
        public int TickFrequency
        {
            get
            {
                CheckDisposed();
                return tickFrequency;
            }

            set
            {
                CheckDisposed();
                if (tickFrequency == value)
                    return;

                tickFrequency = value;
                TickFrequencyChanged?.Invoke(this, EventArgs.Empty);
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
    }
}