using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a date and shows popup calendar when it is clicked.
    /// </summary>
    [ControlCategory("Other")]
    public partial class DatePicker : SpeedDateButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public DatePicker(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        public DatePicker()
        {
            UseTheme = KnownTheme.StaticBorder;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Other;
    }
}