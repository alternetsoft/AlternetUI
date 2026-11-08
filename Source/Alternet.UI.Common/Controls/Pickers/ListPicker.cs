using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents control that displays a selected item and allows to change it
    /// with the popup window with virtual list box.
    /// </summary>
    [ControlCategory(KnownControlCategory.Editors)]
    public partial class ListPicker : SpeedButtonWithListPopup
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
        /// Initializes a new instance of the <see cref="ListPicker"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ListPicker(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListPicker"/> class.
        /// </summary>
        public ListPicker()
        {
            Label.Margin = Label.Margin.WithLeftRight(DefaultTextLeftPadding, DefaultTextRightPadding);
        }

        /// <inheritdoc/>
        public override ControlColorMode? ColorMode
        {
            get => base.ColorMode;
            set
            {
                base.ColorMode = value;

                if(!UseSpeedButtonTheme())
                    UseControlColors(DefaultUseControlColors);
            }
        }

        /// <summary>
        /// Gets or sets a value specifying the style of the control.
        /// </summary>
        /// <returns>
        /// One of the <see cref="ComboBoxStyle" /> values.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(ComboBoxStyle.DropDown)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public virtual ComboBoxStyle DropDownStyle
        {
            get
            {
                return ComboBoxStyle.DropDownList;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control should use the SpeedButton theme.
        /// When set to <c>true</c>, the control will use the theme of a SpeedButton, when <c>false</c>,
        /// it will use the default theme and look like TextPicker.
        /// </summary>
        /// <returns></returns>
        public virtual bool UseSpeedButtonTheme()
        {
            return false;
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if(!UseSpeedButtonTheme())
                UseControlColors(DefaultUseControlColors);
            base.OnSystemColorsChanged(e);
        }

        /// <inheritdoc/>
        protected override KnownTheme GetDefaultUseTheme()
        {
            if (!UseSpeedButtonTheme())
                return DefaultThemes.Picker;
            return base.GetDefaultUseTheme();
        }
    }
}