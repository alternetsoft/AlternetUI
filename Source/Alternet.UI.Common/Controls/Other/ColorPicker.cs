using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected color and allows to change it.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class ColorPicker : SpeedColorButton
    {
        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor. Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ColorPicker(AbstractControl parent)
            : this(useDefaultColors: true)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class with specified default colors usage flag.
        /// </summary>
        /// <param name="useDefaultColors">A value indicating whether to use default colors.</param>
        public ColorPicker(bool useDefaultColors)
            : base(useDefaultColors)
        {
            UseTheme = KnownTheme.StaticBorder;
            UseControlColors(DefaultUseControlColors);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
            : this(useDefaultColors: true)
        {
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            UseControlColors(DefaultUseControlColors);
            base.OnSystemColorsChanged(e);
        }
    }
}