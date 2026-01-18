using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that displays a date and shows popup calendar when it is clicked.
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultBindingProperty("Value")]
    [ControlCategory("Date")]
    public partial class DatePicker : SpeedDateButton
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor using <see cref="AbstractControl.UseControlColors"/>.
        /// Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public DatePicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePicker"/> class.
        /// </summary>
        public DatePicker()
        {
            Initialize();
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Other;

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            Initialize();
        }

        private void Initialize()
        {
            UseTheme = KnownTheme.StaticBorder;
            UseControlColors(DefaultUseControlColors);
        }
    }
}