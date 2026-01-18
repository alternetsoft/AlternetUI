using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected color and allows to change it.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ColorPicker : SpeedColorButton
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor using <see cref="AbstractControl.UseControlColors"/>.
        /// Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ColorPicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            UseTheme = KnownTheme.StaticBorder;
            UseControlColors(DefaultUseControlColors);
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ColorPicker;

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            UseControlColors(DefaultUseControlColors);
            base.OnSystemColorsChanged(e);
        }
    }
}