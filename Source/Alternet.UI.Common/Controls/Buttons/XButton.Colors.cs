using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class XButton
    {
        /// <summary>
        /// Represents the default background color used for hovered buttons in light themes.
        /// </summary>
        public static readonly ColorStruct DefaultHoveredBackColorLight = new(235, 235, 235);

        /// <summary>
        /// Represents the default background color used for hovered buttons in dark themes.
        /// </summary>
        public static readonly ColorStruct DefaultHoveredBackColorDark = new(69, 69, 69);

        /// <summary>
        /// Represents the default background color for normal state buttons in light themes.
        /// </summary>
        public static readonly ColorStruct DefaultNormalBackColorLight = new(245, 245, 245);

        /// <summary>
        /// Represents the default background color for normal state buttons in dark themes.
        /// </summary>
        public static readonly ColorStruct DefaultNormalBackColorDark = new(63, 63, 63);

        /// <summary>
        /// Represents the default alternate normal background color used for buttons with <see cref="IsDefault"/> property set to true.
        /// </summary>
        internal static readonly ColorStruct DefaultAltNormalBackColorIsd = new(3, 102, 214);

        /// <summary>
        /// Represents the default alternate hovered background color used for buttons with <see cref="IsDefault"/> property set to true.
        /// </summary>
        internal static readonly ColorStruct DefaultAltHoveredBackColorIsd = new(3, 114, 239);

        private static LightDarkColor? defaultHoveredBackColorIsd;
        private static LightDarkColor? defaultHoveredBackColor;
        private static LightDarkColor? defaultNormalBackColorIsd;
        private static LightDarkColor? defaultNormalBackColor;
        private static LightDarkColor? defaultPressedBackColor;

        private static LightDarkColor? defaultNormalBorderColor;
        private static LightDarkColor? defaultNormalBorderColorIsd;
        private static LightDarkColor? defaultHoveredBorderColorIsd;
        private static LightDarkColor? defaultHoveredBorderColor;
        private static LightDarkColor? defaultFocusedBorderColor;

        private static LightDarkColor? defaultForeColorIsd;
        private static LightDarkColor? defaultForeColor;

        /// <summary>
        /// Gets or sets the default background color used for <see cref="XButton"/> control when
        /// it is in hovered state and its <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultHoveredBackColorIsd
        {
            get
            {
                return defaultHoveredBackColorIsd ?? DefaultHoveredBackColor;
            }

            set => defaultHoveredBackColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="XButton"/> control when
        /// its <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultNormalBackColorIsd
        {
            get
            {
                return defaultNormalBackColorIsd ?? DefaultNormalBackColor;
            }

            set => defaultNormalBackColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default foreground color used for <see cref="XButton"/> control when
        /// it's <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultForeColorIsd
        {
            get
            {
                return defaultForeColorIsd ?? DefaultNormalForeColor;
            }

            set => defaultForeColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="XButton"/> control.
        /// </summary>
        public static LightDarkColor DefaultNormalBackColor
        {
            get
            {
        
                return defaultNormalBackColor ??= LightDarkColor.FromColorStruct(DefaultNormalBackColorLight, DefaultNormalBackColorDark);
            }

            set => defaultNormalBackColor = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for the pressed state of buttons.
        /// </summary>
        public static LightDarkColor DefaultPressedBackColor
        {
            get
            {
                return defaultPressedBackColor ?? DefaultColors.ControlBackColor;
            }

            set => defaultPressedBackColor = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="XButton"/> control when it is in a hovered state.
        /// </summary>
        public static LightDarkColor DefaultHoveredBackColor
        {
            get
            {
                return defaultHoveredBackColor ??= LightDarkColor.FromColorStruct(DefaultHoveredBackColorLight, DefaultHoveredBackColorDark);
            }

            set => defaultHoveredBackColor = value;
        }


        /// <summary>
        /// Gets or sets the default foreground color used for <see cref="XButton"/> control.
        /// </summary>
        public static LightDarkColor DefaultNormalForeColor
        {
            get
            {
                return defaultForeColor ?? DefaultColors.ControlForeColor;
            }

            set => defaultForeColor = value;
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="XButton"/> elements in a hot (hovered or active) state.
        /// </summary>
        /// <remarks>The default value is initialized to <see cref="LightDarkColors.Blue"/> if not previously set.</remarks>
        public static LightDarkColor DefaultHoveredBorderColor
        {
            get
            {
                return defaultHoveredBorderColor ?? DefaultBorderColor;
            }

            set
            {
                defaultHoveredBorderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="XButton"/> controls in the normal state.
        /// </summary>
        public static LightDarkColor DefaultBorderColor
        {
            get
            {
                return defaultNormalBorderColor ?? DefaultColors.BorderColor;
            }

            set
            {
                defaultNormalBorderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="XButton"/> controls in the focused state.
        /// </summary>
        public static LightDarkColor DefaultFocusedBorderColor
        {
            get
            {
                return defaultFocusedBorderColor ?? DefaultColors.AccentColor;
            }

            set
            {
                defaultFocusedBorderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="XButton"/> controls in the application
        /// when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static LightDarkColor DefaultBorderColorIsd
        {
            get
            {
                return defaultNormalBorderColorIsd ??= new(light: Color.Gray, dark: (245, 245, 245));
            }

            set
            {
                defaultNormalBorderColorIsd = value;
            }
        }

        /// <summary>
        /// Gets or sets the default hovered border color used for <see cref="XButton"/> controls in the application
        /// when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static LightDarkColor DefaultHoveredBorderColorIsd
        {
            get
            {
                return defaultHoveredBorderColorIsd ?? DefaultHoveredBorderColor;
            }

            set
            {
                defaultHoveredBorderColorIsd = value;
            }
        }

        /// <summary>
        /// Sets color theme to match the current system appearance setting.
        /// </summary>
        /// <remarks>This method applies either a dark or light color theme based on the system's appearance preference. </remarks>
        public virtual void SetColorTheme(ColorThemeApplyOptions opt = ColorThemeApplyOptions.All)
        {
            SetColorThemeToLightOrDark(SystemSettings.AppearanceIsDark, opt);
        }

        /// <summary>
        /// Applies a light or dark color theme to the control based on the specified parameters.
        /// </summary>
        /// <remarks>This method updates the control's color properties according to the selected theme
        /// and the specified options. Only the color aspects indicated by the <paramref name="opt"/> parameter are
        /// affected, allowing for selective theme application. The method respects the control's default or custom
        /// color settings as appropriate.</remarks>
        /// <param name="isDark">A value indicating whether to apply the dark theme
        /// (<see langword="true"/>) or the light theme (<see
        /// langword="false"/>).</param>
        /// <param name="opt">A set of options that specifies which color properties
        /// (such as background, foreground, and border colors)
        /// the theme should be applied to.</param>
        public virtual void SetColorThemeToLightOrDark(bool isDark, ColorThemeApplyOptions opt = ColorThemeApplyOptions.All)
        {
            DoInsideUpdate(() =>
            {
                if (opt.HasFlag(ColorThemeApplyOptions.BackColor))
                    BackgroundColor = GetEffectiveBackColor();
                if (opt.HasFlag(ColorThemeApplyOptions.ForeColor))
                    ForegroundColor = GetEffectiveForeColor();

                if (opt.HasFlag(ColorThemeApplyOptions.BorderColor))
                {
                    this.Borders?.Hovered?.SetColor(GetEffectiveHoveredBorderColor());
                    this.Borders?.Normal?.SetColor(GetEffectiveBorderColor());
                    this.Borders?.Focused?.SetColor(DefaultFocusedBorderColor);
                    this.Borders?.Pressed?.SetColor(GetEffectiveBorderColor());
                }
            });
        }

        /// <summary>
        /// Gets the effective border color based on the specified dark mode and other settings.
        /// </summary>
        /// <param name="isDark">A value indicating whether the application is in dark mode.
        /// If <see langword="true"/>, a color suitable for
        /// dark backgrounds is returned; otherwise, a color suitable for light backgrounds is returned.</param>
        /// <returns>A <see cref="Color"/> representing the effective border color, determined by the current 
        /// settings and the dark mode parameter.</returns>
        public virtual LightDarkColor GetEffectiveBorderColor(bool? isDark = null)
        {
            if (IsDefault)
            {
                return DefaultBorderColorIsd;
            }
            else
            {
                return DefaultBorderColor;
            }
        }

        /// <summary>
        /// Gets the effective hovered border color based on the specified dark mode and other settings.
        /// </summary>
        /// <returns>A <see cref="Color"/> representing the effective hovered border color, determined by the current 
        /// settings and the dark mode parameter.</returns>
        public virtual LightDarkColor GetEffectiveHoveredBorderColor()
        {
            if (IsDefault)
            {
                return DefaultHoveredBorderColorIsd;
            }
            else
            {
                return DefaultHoveredBorderColor;
            }
        }

        /// <summary>
        /// Gets the effective foreground color based on the specified dark mode and other settings.
        /// </summary>
        /// <returns>A <see cref="Color"/> representing the effective foreground color, determined by the current 
        /// settings and the dark mode parameter.</returns>
        public virtual LightDarkColor GetEffectiveForeColor()
        {
            if (IsDefault)
            {
                return DefaultForeColorIsd;
            }
            else
            {
                return DefaultNormalForeColor;
            }
        }

        /// <summary>
        /// Gets the effective background color based on the specified dark mode and other settings.
        /// </summary>
        /// <returns>A <see cref="LightDarkColor"/> representing the effective background color, determined by the current default
        /// settings.</returns>
        public virtual LightDarkColor GetEffectiveBackColor()
        {
            if (IsDefault)
            {
                return DefaultNormalBackColorIsd;
            }
            else
            {
                return DefaultNormalBackColor;
            }
        }

        /// <summary>
        /// Gets the effective background color for the pressed state, adjusted for the specified or current appearance setting.
        /// </summary>
        /// <remarks>Use this method to obtain the appropriate pressed background color for a button,
        /// ensuring consistency with the application's light or dark mode.</remarks>
        /// <returns>A <see cref="LightDarkColor"/> representing the background color
        /// to use when the button is pressed, based on the effective
        /// appearance.</returns>
        public virtual LightDarkColor GetEffectivePressedBackColor()
        {
            return DefaultPressedBackColor;
        }

        /// <summary>
        /// Gets the effective background color for the hovered state based on the specified dark mode and other settings.
        /// </summary>
        /// <returns>A <see cref="LightDarkColor"/> representing the effective background color for the
        /// hovered state, determined by the current default
        /// settings.</returns>
        public virtual LightDarkColor GetEffectiveHoveredBackColor()
        {
            if (IsDefault)
            {
                return DefaultHoveredBackColorIsd;
            }
            else
            {
                return DefaultHoveredBackColor;
            }
        }

        /// <summary>
        /// Sets colors used in the control to the dark theme.
        /// </summary>
        public virtual void SetColorThemeToDark(ColorThemeApplyOptions opt = ColorThemeApplyOptions.All)
        {
            SetColorThemeToLightOrDark(isDark: true, opt);
        }

        /// <summary>
        /// Sets colors used in the control to the light theme.
        /// </summary>
        public virtual void SetColorThemeToLight(ColorThemeApplyOptions opt = ColorThemeApplyOptions.All)
        {
            SetColorThemeToLightOrDark(isDark: false, opt);
        }

        /// <summary>
        /// Gets the background color associated with the specified visual control state.
        /// </summary>
        /// <remarks>The returned color may vary depending on whether the control is in a normal, hovered,
        /// or pressed state. Override this method to customize background color selection for additional states or
        /// behaviors.</remarks>
        /// <param name="state">The visual state of the control for which to retrieve the background color.</param>
        /// <returns>A <see cref="Color"/> representing the background color for the given state,
        /// or <see langword="null"/> if no color is defined.</returns>
        public virtual LightDarkColor? GetBackgroundColor(VisualControlState state)
        {
            LightDarkColor color;

            if (state == VisualControlState.Hovered)
            {
                color = GetEffectiveHoveredBackColor();
            }
            else
                if (state == VisualControlState.Pressed)
                {
                    color = GetEffectivePressedBackColor();
                }
                else
                {
                    color = GetEffectiveBackColor();
                }

            return color;
        }

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            if (!UseVisualStyleBackColor)
            {
                var result = base.GetBackground(state);

                if (result != null)
                    return result;
            }

            LightDarkColor color = GetBackgroundColor(state) ?? DefaultColors.ControlBackColor;

            return color.LightOrDark(this).AsBrush;
        }

        /// <summary>
        /// Applies default for the alternative background color for buttons with <see cref="IsDefault"/> property set to true.
        /// </summary>
        internal static void UseAltBackColorIsd()
        {
            DefaultHoveredBackColorIsd = LightDarkColor.FromColorStruct(DefaultAltHoveredBackColorIsd, DefaultAltHoveredBackColorIsd);
            DefaultNormalBackColorIsd = LightDarkColor.FromColorStruct(DefaultAltNormalBackColorIsd, DefaultAltNormalBackColorIsd);
            DefaultForeColorIsd = LightDarkColors.White;
            DefaultBorderColorIsd = DefaultNormalBackColorIsd;
            DefaultHoveredBorderColorIsd = DefaultNormalBackColorIsd;
        }
    }
}
