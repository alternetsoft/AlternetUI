using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected enum value and allows to change it
    /// with the popup window.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class EnumPicker : SpeedEnumButton
    {
        /// <summary>
        /// Gets or sets the default left padding for the text displayed in the control.
        /// </summary>
        public static float DefaultTextLeftPadding = 7;
        
        /// <summary>
        /// Gets or sets the default right padding for the text displayed in the control.
        /// </summary>
        public static float DefaultTextRightPadding = 7;

        /// <summary>
        /// Gets or sets whether to assign default control colors
        /// in the constructor. Default is <c>true</c>.
        /// </summary>
        public static bool DefaultUseControlColors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public EnumPicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPicker"/> class.
        /// </summary>
        public EnumPicker()
        {
            if (!ShowAsSpeedButton())
            {
                Label.Padding = Label.Padding.WithLeftRight(DefaultTextLeftPadding, DefaultTextRightPadding);
                UseTheme = KnownTheme.StaticBorder;
                UseControlColors(DefaultUseControlColors);
            }
        }

        /// <summary>
        /// Determines whether the control should be displayed as a speed button or as a picker control.
        /// </summary>
        /// <returns><c>true</c> if the control should be displayed as a speed button; otherwise, <c>false</c>.</returns>
        protected virtual bool ShowAsSpeedButton()
        {
            return false;
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (!ShowAsSpeedButton())
            {
                UseControlColors(DefaultUseControlColors);
            }

            base.OnSystemColorsChanged(e);
        }
    }
}