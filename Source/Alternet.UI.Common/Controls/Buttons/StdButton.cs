using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic button control. This control is implemented inside the library and can be used in the
    /// same way as a regular native <see cref="Button"/> control. <see cref="StdButton"/> is used when you need
    /// to have the same code for all platforms. <see cref="StdButton"/> provides many additional features,
    /// which are not available in the native <see cref="Button"/> control.
    /// </summary>
    [ControlCategory("Common")]
    public partial class StdButton : GenericItemControl, INotifyPropertyChanged, IDialogButtonRoles
    {
        /// <summary>
        /// Gets or sets default value of the <see cref="IsBoldWhenIsd"/> property. It indicates whether the
        /// text is displayed in bold when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static bool DefaultIsBoldWhenIsd = true;

        /// <summary>
        /// Represents the default padding value which is applied to <see cref="StdButton"/> controls in the constructor.
        /// </summary>
        /// <remarks>This static field provides a standard padding size that is applied to <see cref="StdButton"/> controls
        /// to ensure consistent spacing across components.</remarks>
        public static Thickness DefaultPadding = 4;

        /// <summary>
        /// Represents the default minimum size for the component, expressed as a SizeD structure.
        /// </summary>
        public static SizeD DefaultMinSize = 32;

        /// <summary>
        /// Specifies the default corner radius,
        /// in device-independent pixels or in percentage, used for rendering the corners of button elements.
        /// Use of percentage is determined by the value of <see cref="DefaultCornerRadiusIsPercent"/> field.
        /// If percentage is used, the actual corner radius will be calculated as a percentage of the control's size.
        /// </summary>
        public static float DefaultCornerRadius = 35;

        /// <summary>
        /// Indicates whether the default corner radius is specified as a percentage of the element's dimensions.
        /// </summary>
        public static bool DefaultCornerRadiusIsPercent = true;

        private static LightDarkColor? defaultHoveredBorderColor;
        private static LightDarkColor? defaultBorderColor;
        private static LightDarkColor? defaultBorderColorIsd;
        private static LightDarkColor? defaultHoveredBorderColorIsd;
        private static LightDarkColor? defaultBackColorIsd;
        private static LightDarkColor? defaultForeColorIsd;
        private static LightDarkColor? defaultBackColor;
        private static LightDarkColor? defaultForeColor;
        private static LightDarkColor? defaultHoveredBackColorIsd;
        private static LightDarkColor? defaultHoveredBackColor;
        private static LightDarkColor? defaultPressedBackColor;

        private bool isDefault;
        private bool isCancel;
        private bool exactFit = false;

        static StdButton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdButton"/> class
        /// with the specified parent control.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public StdButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new <see cref="StdButton"/> instance.
        /// </summary>
        public StdButton()
        {
            WantTab = true;
            IsGraphicControl = false;
            IsTransparent = false;
            ParentBackColor = false;
            ParentForeColor = false;

            UniformBorderCornerRadius = DefaultCornerRadius;
            UniformBorderRadiusIsPercent = DefaultCornerRadiusIsPercent;

            Borders?.SetAllExceptNormal(() => Borders.Normal?.Clone());

            MinimumSize = DefaultMinSize;
            Padding = DefaultPadding;
            Item.HorizontalAlignment = HorizontalAlignment.Center;

            SetColorTheme();
        }

        /// <summary>
        /// Initializes a new <see cref="StdButton"/> instance with the specified text.
        /// </summary>
        public StdButton(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdButton"/> class
        /// with the specified text, click action and parent control.
        /// </summary>
        public StdButton(Control parent, string text, Action? clickAction = null)
            : this()
        {
            Text = text;
            ClickAction = clickAction;
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StdButton"/> class
        /// with the specified text and click action.
        /// </summary>
        public StdButton(string text, Action clickAction)
            : this()
        {
            Text = text;
            ClickAction = clickAction;
        }

        /// <summary>
        /// Specifies options for applying a color theme, allowing control over which aspects of the theme are modified.
        /// </summary>
        /// <remarks>This enumeration supports a bitwise combination of its member values to enable
        /// multiple options when applying a color theme. The default value is Default, which applies no specific
        /// options.</remarks>
        [Flags]
        public enum ColorThemeApplyOptions
        {
            /// <summary>
            /// No specific options are applied when this option is used. This is the default value.
            /// </summary>
            None = 0,

            /// <summary>
            /// When this option is used, the background color of the control is updated to match the color theme being applied.
            /// </summary>
            BackColor = 1,

            /// <summary>
            /// When this option is used, the foreground color of the control is updated to match the color theme being applied.
            /// </summary>
            ForeColor = 2,
            
            /// <summary>
            /// When this option is used, the border color of the control is updated to match the color theme being applied.
            /// </summary>
            BorderColor = 4,

            /// <summary>
            /// All parts of the color theme are applied when this option is used.
            /// </summary>
            All = BackColor | ForeColor | BorderColor,
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="StdButton"/> control when
        /// it is in hovered state and its <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultHoveredBackColorIsd
        {
            get => defaultHoveredBackColorIsd ?? DefaultHoveredBackColor;
            set => defaultHoveredBackColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="StdButton"/> control when
        /// its <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultNormalBackColorIsd
        {
            get => defaultBackColorIsd ?? DefaultNormalBackColor;
            set => defaultBackColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default foreground color used for <see cref="StdButton"/> control when
        /// it's <see cref="IsDefault"/> property is set to true.
        /// </summary>
        public static LightDarkColor DefaultForeColorIsd
        {
            get => defaultForeColorIsd ?? DefaultNormalForeColor;
            set => defaultForeColorIsd = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="StdButton"/> control.
        /// </summary>
        public static LightDarkColor DefaultNormalBackColor
        {
            get => defaultBackColor ??= new (light: (245, 245, 245), dark: (63, 63, 63));
            set => defaultBackColor = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for the pressed state of buttons.
        /// </summary>
        public static LightDarkColor DefaultPressedBackColor
        {
            get => defaultPressedBackColor ?? DefaultColors.ControlBackColor;
            set => defaultPressedBackColor = value;
        }

        /// <summary>
        /// Gets or sets the default background color used for <see cref="StdButton"/> control when it is in a hovered state.
        /// </summary>
        public static LightDarkColor DefaultHoveredBackColor
        {
            get => defaultHoveredBackColor ??= new(light: (235, 235, 235), dark: (69, 69, 69));
            set => defaultHoveredBackColor = value;
        }


        /// <summary>
        /// Gets or sets the default foreground color used for <see cref="StdButton"/> control.
        /// </summary>
        public static LightDarkColor DefaultNormalForeColor
        {
            get => defaultForeColor ?? DefaultColors.ControlForeColor;
            set => defaultForeColor = value;
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="StdButton"/> elements in a hot (hovered or active) state.
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
        /// Gets or sets the default border color used for <see cref="StdButton"/> controls in the application.
        /// </summary>
        /// <remarks>If not explicitly set, the default border color is initialized to <see cref="DefaultColors.BorderColor"/>.</remarks>
        public static LightDarkColor DefaultBorderColor
        {
            get
            {
                return defaultBorderColor ?? DefaultColors.BorderColor;
            }

            set
            {
                defaultBorderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default border color used for <see cref="StdButton"/> controls in the application
        /// when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static LightDarkColor DefaultBorderColorIsd
        {
            get
            {
                return defaultBorderColorIsd ?? DefaultColors.BorderColor;
            }

            set
            {
                defaultBorderColorIsd = value;
            }
        }

        /// <summary>
        /// Gets or sets the default hovered border color used for <see cref="StdButton"/> controls in the application
        /// when <see cref="IsDefault"/> property of the control is set to true.
        /// </summary>
        public static LightDarkColor DefaultHoveredBorderColorIsd
        {
            get
            {
                return defaultHoveredBorderColorIsd ?? DefaultBorderColor;
            }

            set
            {
                defaultHoveredBorderColorIsd = value;
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Button;

        /// <summary>
        /// Gets or sets a value that determines if the background is drawn using visual styles,
        /// if supported.</summary>
        /// <returns>
        /// <see langword="true" /> if the background is drawn using visual styles;
        /// otherwise, <see langword="false" />.</returns>
        /// <remarks>
        /// Currently this property doesn't do anything and is added for compatibility.
        /// </remarks>
        [Category("Appearance")]
        [Browsable(false)]
        public virtual bool UseVisualStyleBackColor { get; set; } = true;

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is
        /// the default button. In a modal dialog,
        /// a user invokes the default button by pressing the ENTER key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is the default
        /// button; otherwise, <see
        /// langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public virtual bool IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (isDefault == value)
                    return;
                isDefault = value;

                if (AutoUpdateColors)
                {
                    SetColorTheme();
                }

                if (value)
                {
                    Item.IsBold = RealFont.IsBold || (IsBoldWhenIsd ?? DefaultIsBoldWhenIsd);
                }
                else
                {
                    Item.IsBold = RealFont.IsBold;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the text is displayed in bold when <see cref="IsDefault"/> property of the control is set to true.
        /// If not set, the value of <see cref="DefaultIsBoldWhenIsd"/> static field is used. This property allows you to specify
        /// whether the button's text should be rendered in bold font style when the button is marked as the default action in a dialog or form.
        /// </summary>
        [Browsable(false)]
        public virtual bool? IsBoldWhenIsd { get; set; }

        /// <inheritdoc/>
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (BackgroundColor == value)
                    return;
                value ??= GetEffectiveBackColor();
                base.BackgroundColor = value;
            }
        }

        /// <inheritdoc/>
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set
            {
                if (DisposingOrDisposed)
                    return;
                if (ForegroundColor == value)
                    return;
                value ??= GetEffectiveForeColor();
                base.ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets whether buttons are made of at least the standard button size, even
        /// if their contents is small enough to fit into a smaller size.
        /// Currently this property doesn't do anything and is added for compatibility.
        /// </summary>
        /// <remarks>
        /// Standard button size is used
        /// for consistency as most platforms use buttons of the same size in the native
        /// dialogs. If this flag is specified, the
        /// button will be made just big enough for its contents. Notice that under MSW
        /// the button will still have at least the standard height, even with this style,
        /// if it has a non-empty label.
        /// </remarks>
        [Browsable(false)]
        public virtual bool ExactFit
        {
            get
            {
                return exactFit;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (exactFit == value)
                    return;
                exactFit = value;
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/>
        /// is a 'Cancel' button. In a modal dialog, a
        /// user can activate the 'Cancel' button by pressing the ESC key.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the <see cref="Button"/> is a 'Cancel'
        /// button; otherwise, <see langword="false"/>.
        /// The default is <see langword="false"/>.
        /// </value>
        public virtual bool IsCancel
        {
            get
            {
                return isCancel;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (isCancel == value)
                    return;
                isCancel = value;
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override void DrawBorderAndBackground(
            PaintEventArgs e,
            DrawDefaultBackgroundFlags flags = DrawDefaultBackgroundFlags.DrawBorderAndBackground)
        {
            base.DrawBorderAndBackground(e, flags);
        }

        /// <summary>
        /// Sets the application's color theme to match the current system appearance setting.
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
        /// <param name="isDark">A value indicating whether to apply the dark theme (<see langword="true"/>) or the light theme (<see
        /// langword="false"/>).</param>
        /// <param name="opt">A set of options that specifies which color properties (such as background, foreground, and border colors)
        /// the theme should be applied to.</param>
        public virtual void SetColorThemeToLightOrDark(bool isDark, ColorThemeApplyOptions opt = ColorThemeApplyOptions.All)
        {
            DoInsideUpdate(() =>
            {
                if (opt.HasFlag(ColorThemeApplyOptions.BackColor))
                    BackColor = GetEffectiveBackColor(isDark);
                if (opt.HasFlag(ColorThemeApplyOptions.ForeColor))
                    ForeColor = GetEffectiveForeColor(isDark);

                if (IsDefault)
                {
                    if (opt.HasFlag(ColorThemeApplyOptions.BorderColor))
                    {
                        this.Borders?.Normal?.SetColor(DefaultBorderColorIsd.LightOrDark(isDark));
                        this.Borders?.Hovered?.SetColor(DefaultHoveredBorderColorIsd.LightOrDark(isDark));
                    }
                }
                else
                {
                    if (opt.HasFlag(ColorThemeApplyOptions.BorderColor))
                    {
                        this.Borders?.Normal?.SetColor(DefaultBorderColor.LightOrDark(isDark));
                        this.Borders?.Hovered?.SetColor(DefaultHoveredBorderColor.LightOrDark(isDark));
                    }
                }
            });
        }

        /// <summary>
        /// Gets the effective foreground color based on the specified dark mode and other settings.
        /// </summary>
        /// <param name="isDark">A value indicating whether the application is in dark mode. If <see langword="true"/>, a color suitable for
        /// dark backgrounds is returned; otherwise, a color suitable for light backgrounds is returned.</param>
        /// <returns>A <see cref="Color"/> representing the effective foreground color, determined by the current 
        /// settings and the dark mode parameter.</returns>
        public virtual Color GetEffectiveForeColor(bool? isDark = null)
        {
            var isd = isDark ?? SystemSettings.AppearanceIsDark;

            if (IsDefault)
            {
                return DefaultForeColorIsd.LightOrDark(isd);
            }
            else
            {
                return DefaultNormalForeColor.LightOrDark(isd);
            }
        }

        /// <summary>
        /// Gets the effective background color based on the specified dark mode and other settings.
        /// </summary>
        /// <param name="isDark">A value indicating whether the application is in dark mode. If <see langword="true"/>, a color suitable for
        /// dark backgrounds is returned; otherwise, a color suitable for light backgrounds is returned.</param>
        /// <returns>A <see cref="Color"/> representing the effective background color, determined by the current default
        /// settings and the dark mode parameter.</returns>
        public virtual Color GetEffectiveBackColor(bool? isDark = null)
        {
            var isd = isDark ?? SystemSettings.AppearanceIsDark;

            if (IsDefault)
            {
                return DefaultNormalBackColorIsd.LightOrDark(isd);
            }
            else
            {
                return DefaultNormalBackColor.LightOrDark(isd);
            }
        }

        /// <summary>
        /// Gets the effective background color for the pressed state, adjusted for the specified or current appearance setting.
        /// </summary>
        /// <remarks>Use this method to obtain the appropriate pressed background color for a button,
        /// ensuring consistency with the application's light or dark mode.</remarks>
        /// <param name="isDark">An optional value indicating whether to use dark appearance. If null, the method uses the system's current
        /// appearance setting.</param>
        /// <returns>A Color representing the background color to use when the button is pressed, based on the effective
        /// appearance.</returns>
        public virtual Color GetEffectivePressedBackColor(bool? isDark = null)
        {
            var isd = isDark ?? SystemSettings.AppearanceIsDark;
            return DefaultPressedBackColor.LightOrDark(isd);
        }

        /// <summary>
        /// Gets the effective background color for the hovered state based on the specified dark mode and other settings.
        /// </summary>
        /// <param name="isDark">A value indicating whether the application is in dark mode. If <see langword="true"/>, a color suitable for
        /// dark backgrounds is returned; otherwise, a color suitable for light backgrounds is returned.</param>
        /// <returns>A <see cref="Color"/> representing the effective background color for the hovered state, determined by the current default
        /// settings and the dark mode parameter.</returns>
        public virtual Color GetEffectiveHoveredBackColor(bool? isDark = null)
        {
            var isd = isDark ?? SystemSettings.AppearanceIsDark;

            if (IsDefault)
            {
                return DefaultHoveredBackColorIsd.LightOrDark(isd);
            }
            else
            {
                return DefaultHoveredBackColor.LightOrDark(isd);
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

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            Color color;

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

            return color.AsBrush;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            if (AutoUpdateColors)
            {
                SetColorTheme();
            }

            base.OnSystemColorsChanged(e);
        }

        /// <inheritdoc/>
        protected override bool GetDefaultHasBorder()
        {
            return true;
        }
    }
}