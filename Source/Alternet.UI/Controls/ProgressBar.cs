using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a progress bar control.
    /// </summary>
    /// <remarks>
    /// A <see cref="ProgressBar"/> control visually indicates the progress of a lengthy operation.
    /// <para>
    /// The <see cref="Maximum"/> and <see cref="Minimum"/> properties define the range of values to
    /// represent the progress of a task. The <see cref="Minimum"/> property is typically set to a value of 0,
    /// and the <see cref="Maximum"/> property is typically set to a value indicating the completion of a task.
    /// For example, to properly display the progress when copying a group of files,
    /// the <see cref="Maximum"/> property could be set to the total number of files to be copied.
    /// </para>
    /// <para>
    /// The <see cref="Value"/> property represents the progress that the application has made toward completing
    /// the operation. The value displayed by the <see cref="ProgressBar"/> only approximates the current value of the Value property.
    /// Based on the size of the <see cref="ProgressBar"/>, the <see cref="Value"/> property determines when to increase the size of
    /// the visually highlighted bar.
    /// </para>
    /// </remarks>
    public class ProgressBar : Control
    {
        private int value;

        private int minimum;

        private int maximum = 100;

        /// <summary>
        /// Occurs when the value of the <see cref="Value"/> property changes.
        /// </summary>
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
        /// Gets or sets the current position of the progress bar.
        /// </summary>
        /// <value>The position within the range of the progress bar. The default is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        /// When the <see cref="Value"/> property is set, the new value is validated
        /// to be between the <see cref="Minimum"/> and <see cref="Maximum"/> values.</remarks>
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
                if (this.value == value)
                    return;

                this.value = value;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the progress bar control.
        /// </summary>
        /// <value>The minimum allowed value for the progress bar control. The default value is 0.</value>
        /// <remarks>
        /// The minimum and maximum values of the Value property are specified by the <see cref="Minimum"/> and <see cref="Maximum"/> properties.
        ///  If the new <see cref="Minimum"/> property value is greater than the <see cref="Maximum"/> property value,
        ///  the <see cref="Maximum"/> value is set equal to the <see cref="Minimum"/> value.
        ///  If the <see cref="Value"/> is less than the new <see cref="Minimum"/> value, the <see cref="Value"/> property
        ///  is also set equal to the <see cref="Minimum"/> value.
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
        /// Gets or sets the maximum allowed value for the progress bar control.
        /// </summary>
        /// <value>The maximum allowed value for the progress bar control. The default value is 100.</value>
        /// <remarks>
        ///  If the <see cref="Minimum"/> property is greater than the new <see cref="Maximum"/> property, the <see cref="Minimum"/>
        ///  property value is set equal to the <see cref="Maximum"/> value.
        ///  If the current <see cref="Value"/> is greater than the new <see cref="Maximum"/> value, the <see cref="Value"/> property
        ///  value is set equal to the <see cref="Maximum"/> value.
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
                MaximumChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}