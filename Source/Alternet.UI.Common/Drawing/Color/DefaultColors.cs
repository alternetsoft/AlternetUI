using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides default colors for the application.
    /// </summary>
    public static class DefaultColors
    {
        /// <summary>
        /// Gets the default border color for user controls when dark theme is used.
        /// </summary>
        public static readonly Color DefaultBorderColorDark;

        /// <summary>
        /// Gets the default border color for user controls when light theme is used.
        /// </summary>
        // This is also used to get default disabled svg image color for light theme.
        // So please do not change this value unless you completely sure.
        public static readonly Color DefaultBorderColorLight;

        /// <summary>
        /// Gets the default background color for controls when dark theme is used.
        /// </summary>
        public static readonly Color DefaultControlBackColorDark;

        /// <summary>
        /// Gets the default foreground color for controls when dark theme is used.
        /// </summary>
        public static readonly Color DefaultControlForeColorDark;

        /// <summary>
        /// Gets the default background color for windows when dark theme is used.
        /// </summary>
        public static readonly Color DefaultWindowBackColorDark;

        /// <summary>
        /// Gets the default foreground color for windows when dark theme is used.
        /// </summary>
        public static readonly Color DefaultWindowForeColorDark;

        private static LightDarkColor? borderColor;
        private static LightDarkColor windowBackColor;
        private static LightDarkColor windowForeColor;
        private static LightDarkColor controlBackColor;
        private static LightDarkColor controlForeColor;
        private static LightDarkColor? svgDisabledColor;
        private static LightDarkColor? svgNormalColor;

#pragma warning disable
        static DefaultColors()
#pragma warning restore
        {
            if (App.IsMaui)
            {
                DefaultBorderColorDark = Color.FromRgb(61, 61, 61);
            }
            else
            {
                DefaultBorderColorDark = Color.FromRgb(123, 123, 123);
            }

            DefaultBorderColorLight = Color.FromRgb(204, 206, 219);

            DefaultControlBackColorDark = SystemColors.Control;
            DefaultControlForeColorDark = SystemColors.ControlText;
            DefaultWindowBackColorDark = SystemColors.Window;
            DefaultWindowForeColorDark = SystemColors.WindowText;

            Initialize();
        }

        /// <summary>
        /// Gets or sets the default color of a checkbox in its normal state.
        /// </summary>
        public static LightDarkColor DefaultCheckBoxColor { get; set; }
            = new(light: (0, 103, 192), dark: new(76, 194, 255));

        /// <summary>
        /// Gets or sets default border color of the user control.
        /// </summary>
        public static LightDarkColor BorderColor
        {
            get => borderColor ??= Color.LightDark(DefaultBorderColorLight, DefaultBorderColorDark);

            set
            {
                borderColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default foreground color of the control.
        /// </summary>
        public static LightDarkColor ControlForeColor
        {
            get => controlForeColor;

            set
            {
                controlForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default background color of the control.
        /// </summary>
        public static LightDarkColor ControlBackColor
        {
            get => controlBackColor;

            set
            {
                controlBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the default background color of the window.
        /// </summary>
        public static LightDarkColor WindowBackColor
        {
            get => windowBackColor;

            set
            {
                windowBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color used for SVG images in both light and dark themes.
        /// </summary>
        public static LightDarkColor SvgNormalColor
        {
            get => svgNormalColor ??= new LightDarkColor(
                light: Color.Black,
                dark: Color.White);
            set => svgNormalColor = value;
        }

        /// <summary>
        /// Gets or sets the color used for disabled SVG images in both light and dark themes.
        /// </summary>
        public static LightDarkColor SvgDisabledColor
        {
            get => svgDisabledColor ??= new LightDarkColor(
                light: SystemColors.GrayText,
                dark: DefaultColors.GetBorderColor(isDark: false));
            set => svgDisabledColor = value;
        }

        /// <summary>
        /// Gets or sets the default foreground color of the window.
        /// </summary>
        public static LightDarkColor WindowForeColor
        {
            get => windowForeColor;

            set
            {
                windowForeColor = value;
            }
        }

        /// <summary>
        /// Gets default border color of the user control.
        /// </summary>
        /// <returns></returns>
        public static Color GetBorderColor(bool isDark)
        {
            if (isDark)
            {
                return BorderColor.Dark;
            }
            else
            {
                return BorderColor.Light;
            }
        }

        /// <summary>
        /// Gets default fore color of the window.
        /// </summary>
        /// <returns></returns>
        public static Color GetWindowForeColor(bool isDark)
        {
            if (isDark)
            {
                return WindowForeColor.Dark;
            }
            else
            {
                return WindowForeColor.Light;
            }
        }

        /// <summary>
        /// Gets default back color of the window.
        /// </summary>
        /// <returns></returns>
        public static Color GetWindowBackColor(bool isDark)
        {
            if (isDark)
            {
                return WindowBackColor.Dark;
            }
            else
            {
                return WindowBackColor.Light;
            }
        }

        /// <summary>
        /// Gets the color for the border of the specified user control.
        /// </summary>
        /// <param name="control">The user control for which to get the border color.</param>
        /// <returns>The color for the border of the specified user control.</returns>
        public static Color GetControlBorderColor(AbstractControl control)
        {
            var color = control.Borders?.GetObjectOrNull(VisualControlState.Normal)?.Color;
            color ??= GetBorderColor(control.IsDarkBackground);
            return color;
        }

        /// <summary>
        /// Gets the brush for the border of the specified user control.
        /// </summary>
        /// <param name="control">The user control for which to get the border brush.</param>
        /// <returns>The brush for the border of the specified user control.</returns>
        public static Brush GetControlBorderBrush(AbstractControl control)
        {
            var brush = control.Background
                ?? control.BackgroundColor?.AsBrush
                ?? GetControlBorderColor(control).AsBrush;
            return brush;
        }

        /// <summary>
        /// Initializes the <see cref="DefaultColors"/> based on the current system theme.
        /// </summary>
        /// <remarks>This method determines appropriate light and dark color values for various UI
        /// elements (such as window and control backgrounds and foregrounds)
        /// by inspecting the system's current color
        /// scheme. It ensures that the application adapts to both light and dark themes
        /// for a consistent user
        /// experience.</remarks>
        public static void Initialize()
        {
            Color darkBackColor;
            Color darkForeColor;

            if (SystemColors.Control.IsDark())
            {
                darkBackColor = DefaultControlBackColorDark;
                darkForeColor = DefaultControlForeColorDark;
            }
            else
            {
                darkBackColor = (30, 30, 30);
                darkForeColor = (164, 164, 164);
            }

            windowBackColor = new(light: new(240, 240, 240), dark: darkBackColor);
            windowForeColor = new(light: Color.Black, dark: darkForeColor);

            if (SystemColors.Window.IsDark())
            {
                darkBackColor = DefaultWindowBackColorDark;
                darkForeColor = DefaultWindowForeColorDark;
            }
            else
            {
                darkBackColor = (30, 30, 30);
                darkForeColor = (164, 164, 164);
            }

            controlBackColor = new(light: Color.White, dark: darkBackColor);
            controlForeColor = new(light: Color.Black, dark: darkForeColor);
        }

        /// <summary>
        /// Provides predefined color values for styling toolbar elements in .NET MAUI applications.
        /// </summary>
        /// <remarks>This static class contains color definitions for various toolbar states and themes,
        /// including light and dark modes. The colors can be used to ensure consistent appearance across toolbars in
        /// MAUI-based user interfaces.</remarks>
        public static class MauiToolBar
        {
            /// <summary>
            /// Gets or sets the background color for toolbars in dark theme.
            /// </summary>
            public static Color BackColorDark = (31, 31, 31);

            /// <summary>
            /// Gets or sets the background color for toolbars in light theme.
            /// </summary>
            public static Color BackColorLight = Color.White;

            /// <summary>
            /// Gets or sets the text color for toolbars in dark theme.
            /// </summary>
            public static Color TextColorDark = (214, 214, 214);

            /// <summary>
            /// Gets or sets the separator color for toolbars in dark theme.
            /// </summary>
            public static Color SeparatorColorDark = Color.FromRgb(61, 61, 61);

            /// <summary>
            /// Gets or sets the separator color for toolbars in light theme.
            /// </summary>
            public static Color SeparatorColorLight = Color.FromRgb(204, 206, 219);

            /// <summary>
            /// Gets or sets the sticky underline color for toolbars in dark theme.
            /// </summary>
            public static Color StickyUnderlineColorDark = Color.FromRgb(76, 194, 255);

            /// <summary>
            /// Gets or sets the sticky underline color for toolbars in light theme.
            /// </summary>
            public static Color StickyUnderlineColorLight = Color.FromRgb(0, 120, 212);

            /// <summary>
            /// Gets or sets the disabled text color for toolbars in dark theme.
            /// </summary>
            public static Color DisabledTextColorDark = Color.Gray;

            /// <summary>
            /// Gets or sets the disabled text color for toolbars in light theme.
            /// </summary>
            public static Color DisabledTextColorLight = Color.Gray;

            /// <summary>
            /// Gets or sets the hot (hover) border color for toolbars in dark theme.
            /// </summary>
            public static Color HotBorderColorDark = Color.DarkGray;

            /// <summary>
            /// Gets or sets the pressed border color for toolbars in dark theme.
            /// </summary>
            public static Color PressedBorderColorDark = Color.FromRgb(61, 61, 61);

            /// <summary>
            /// Gets or sets the hot (hover) border color for toolbars in light theme.
            /// </summary>
            public static Color HotBorderColorLight = Color.FromRgb(0, 108, 190);

            /// <summary>
            /// Gets or sets the text color for toolbars in light theme.
            /// </summary>
            public static Color TextColorLight = Color.Black;

            /// <summary>
            /// Gets or sets the pressed border color for toolbars in light theme.
            /// </summary>
            public static Color PressedBorderColorLight = Color.DarkGray;

            /// <summary>
            /// Gets the background color for toolbars based on the specified theme.
            /// </summary>
            /// <param name="isDark">Indicates whether the background is dark. If <see langword="true"/>, a color suitable for dark
            /// backgrounds is returned; otherwise, a color suitable for light backgrounds is returned.</param>
            /// <returns>A <see cref="Color"/> value representing the appropriate text color for the specified background type.</returns>
            public static Color BackColor(bool isDark)
            {
                return isDark ? BackColorDark : BackColorLight;
            }

            /// <summary>
            /// Returns the recommended text color for either dark or light backgrounds.
            /// </summary>
            /// <param name="isDark">Indicates whether the background is dark. If <see langword="true"/>, a color suitable for dark
            /// backgrounds is returned; otherwise, a color suitable for light backgrounds is returned.</param>
            /// <returns>A <see cref="Color"/> value representing the appropriate text color for the specified background type.</returns>
            public static Color TextColor(bool isDark)
            {
                return isDark ? TextColorDark : TextColorLight;
            }
        }
    }
}
