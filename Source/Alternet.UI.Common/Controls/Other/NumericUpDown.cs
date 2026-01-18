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
    /// <see cref="IntPicker.Minimum"/> and <see cref="IntPicker.Maximum"/> properties.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class NumericUpDown : IntPicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public NumericUpDown(AbstractControl parent)
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
    }
}