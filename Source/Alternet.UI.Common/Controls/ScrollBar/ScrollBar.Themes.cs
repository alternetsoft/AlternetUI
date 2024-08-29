using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class ScrollBar
    {
        private static KnownTheme systemTheme = KnownTheme.WindowsAuto;

        /// <summary>
        /// Enumerates known color and style themes for the <see cref="ScrollBar"/>.
        /// </summary>
        public enum KnownTheme
        {
            /// <summary>
            /// System color theme is used.
            /// </summary>
            System,

            /// <summary>
            /// 'Visual Studio Dark' theme is used if control has dark background
            /// and 'Visual Studio Light' theme is used if control has light background.
            /// </summary>
            VisualStudioAuto,

            /// <summary>
            /// 'Visual Studio Light' theme is used.
            /// </summary>
            VisualStudioLight,

            /// <summary>
            /// 'Visual Studio Dark' theme is used.
            /// </summary>
            VisualStudioDark,

            /// <summary>
            /// 'Windows Dark' theme is used if control has dark background
            /// and 'Windows Light' theme is used if control has light background.
            /// </summary>
            WindowsAuto,

            /// <summary>
            /// 'Windows Dark' theme is used.
            /// </summary>
            WindowsDark,

            /// <summary>
            /// 'Windows Light' theme is used.
            /// </summary>
            WindowsLight,
        }

        /// <summary>
        /// Gets or sets which theme to use when <see cref="KnownTheme.System"/> is used.
        /// </summary>
        public static KnownTheme SystemTheme
        {
            get
            {
                return systemTheme;
            }

            set
            {
                if (value == KnownTheme.System)
                    return;
                systemTheme = value;
            }
        }

        /// <summary>
        /// Contains different themes colors for the scrollbar parts.
        /// </summary>
        public class ThemeMetrics
        {
            /// <summary>
            /// Gets or sets corner background brushes in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, BrushOrColor> CornerBackground = new();

            /// <summary>
            /// Gets or sets scrollbar background brushes in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, BrushOrColor> Background = new();

            /// <summary>
            /// Gets or sets scrollbar arrow colors in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, BrushOrColor> Arrow = new();

            /// <summary>
            /// Gets or sets scrollbar thumb background brushes in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, BrushOrColor> ThumbBackground = new();

            /// <summary>
            /// Gets or sets distance (in dips) between arrow button and arrow in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, Coord> ArrowMargin = new();

            /// <summary>
            /// Gets or sets whether to use arrow width as the thumb width for the vertical scrollbar
            /// and arrow height as the thumb height for the horizontal scrollbar.
            /// Each visual state has it's own setting.
            /// </summary>
            public EnumArray<VisualControlState, bool> UseArrowSizeForThumb = new();

            /// <summary>
            /// Gets or sets distance (in dips) between thumb and scrollbar bounds
            /// in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, Coord> ThumbMargin = new();

            /// <summary>
            /// Gets or sets whether arrows are visible in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, bool> ArrowsVisible = new();

            /// <summary>
            /// Gets or sets whether thumb is visible in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, bool> ThumbVisible = new();

            /// <summary>
            /// Gets or sets whether buttons are visible in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, bool> ButtonsVisible = new();

            /// <summary>
            /// Gets or sets scrollbar thumb border colors in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, BrushOrColor> ThumbBorder = new();

            private static ThemeMetrics? visualStudioLight;
            private static ThemeMetrics? visualStudioDark;
            private static ThemeMetrics? windowsLight;
            private static ThemeMetrics? windowsDark;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThemeMetrics"/> class.
            /// </summary>
            public ThemeMetrics()
            {
                SetArrowMargin(1);
                SetUseArrowSizeForThumb(true);
                SetThumbMargin(1);
                SetArrowsVisible(true);
                SetThumbVisible(true);
                SetButtonsVisible(true);
            }

            /// <summary>
            /// Gets or sets 'Windows Dark' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics WindowsDarkTheme
            {
                get
                {
                    if(windowsDark is null)
                    {
                        windowsDark = new();
                        windowsDark.Background[VisualControlState.Normal] = (46, 46, 46);
                        windowsDark.CornerBackground[VisualControlState.Normal] = (102, 102, 102);
                        windowsDark.Arrow[VisualControlState.Normal] = (153, 153, 153);
                        windowsDark.Arrow[VisualControlState.Hovered] = (153, 153, 153);
                        windowsDark.ThumbBackground[VisualControlState.Normal] = (77, 77, 77);
                        windowsDark.ThumbBorder[VisualControlState.Normal] = (77, 77, 77);
                        windowsDark.SetArrowMargin(1);
                        windowsDark.SetUseArrowSizeForThumb(true);
                        windowsDark.SetThumbMargin(1);
                    }

                    return windowsDark;
                }

                set
                {
                    windowsDark = value;
                }
            }

            /// <summary>
            /// Gets or sets 'Windows Light' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics WindowsLightTheme
            {
                get
                {
                    if(windowsLight is null)
                    {
                        windowsLight = new();
                        windowsLight.Background[VisualControlState.Normal] = (226, 226, 226);
                        windowsLight.CornerBackground[VisualControlState.Normal] = (238, 238, 242);
                        windowsLight.Arrow[VisualControlState.Normal] = (194, 195, 201);
                        windowsLight.Arrow[VisualControlState.Hovered] = (104, 104, 104);
                        windowsLight.ThumbBackground[VisualControlState.Normal] = (194, 195, 201);
                        windowsLight.ThumbBorder[VisualControlState.Normal] = (194, 195, 201);
                        windowsLight.ThumbBackground[VisualControlState.Hovered] = (104, 104, 104);
                        windowsLight.ThumbBorder[VisualControlState.Hovered] = (104, 104, 104);
                        windowsLight.SetArrowMargin(1);
                        windowsLight.SetUseArrowSizeForThumb(true);
                        windowsLight.SetThumbMargin(1);
                    }

                    return windowsLight;
                }

                set
                {
                    windowsLight = value;
                }
            }

            /// <summary>
            /// Gets or sets 'Visual Studio Dark' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics VisualStudioDarkTheme
            {
                get
                {
                    if(visualStudioDark is null)
                    {
                        visualStudioDark = new();
                        visualStudioDark.Background[VisualControlState.Normal] = (62, 62, 66);
                        visualStudioDark.CornerBackground[VisualControlState.Normal] = (62, 62, 66);
                        visualStudioDark.Arrow[VisualControlState.Normal] = (153, 153, 153);
                        visualStudioDark.Arrow[VisualControlState.Hovered] = (28, 151, 234);
                        visualStudioDark.ThumbBackground[VisualControlState.Normal] = (0, 0, 0);
                        visualStudioDark.ThumbBorder[VisualControlState.Normal] = (104, 104, 104);
                        visualStudioDark.SetArrowMargin(1);
                        visualStudioDark.SetUseArrowSizeForThumb(false);
                        visualStudioDark.SetThumbMargin(0);
                    }

                    return visualStudioDark;
                }

                set
                {
                    visualStudioDark = value;
                }
            }

            /// <summary>
            /// Gets or sets 'Visual Studio Light' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics VisualStudioLightTheme
            {
                get
                {
                    if(visualStudioLight is null)
                    {
                        visualStudioLight = new();
                        visualStudioLight.Background[VisualControlState.Normal] = (245, 245, 245);
                        visualStudioLight.Arrow[VisualControlState.Normal] = (134, 137, 153);
                        visualStudioLight.Arrow[VisualControlState.Hovered] = (28, 151, 234);
                        visualStudioLight.ThumbBackground[VisualControlState.Normal] = (255, 255, 255);
                        visualStudioLight.ThumbBorder[VisualControlState.Normal] = (0, 0, 0);
                        visualStudioLight.CornerBackground[VisualControlState.Normal] = (245, 245, 245);
                        visualStudioLight.SetArrowMargin(1);
                        visualStudioLight.SetUseArrowSizeForThumb(false);
                        visualStudioLight.SetThumbMargin(0);
                    }

                    return visualStudioLight;
                }

                set
                {
                    visualStudioLight = value;
                }
            }

            /// <summary>
            /// Returns theme colors for the specified <paramref name="theme"/> and
            /// <paramref name="isDark"/> parameters.
            /// </summary>
            /// <param name="isDark">Whether to return dark or light theme when any of the 'auto'
            /// themes is specified in the <paramref name="theme"/> parameter.</param>
            /// <param name="theme"><see cref="KnownTheme"/> value.</param>
            /// <returns></returns>
            public static ThemeMetrics GetColors(KnownTheme theme, bool isDark = false)
            {
                switch (theme)
                {
                    default:
                    case KnownTheme.System:
                        theme = SystemTheme;
                        if (theme == KnownTheme.System)
                            theme = KnownTheme.WindowsAuto;
                        return GetColors(theme, isDark);
                    case KnownTheme.VisualStudioAuto:
                        if (isDark)
                            return VisualStudioDarkTheme;
                        return VisualStudioLightTheme;
                    case KnownTheme.VisualStudioLight:
                        return VisualStudioLightTheme;
                    case KnownTheme.VisualStudioDark:
                        return VisualStudioDarkTheme;
                    case KnownTheme.WindowsAuto:
                        if (isDark)
                            return WindowsDarkTheme;
                        return WindowsLightTheme;
                    case KnownTheme.WindowsDark:
                        return WindowsDarkTheme;
                    case KnownTheme.WindowsLight:
                        return WindowsLightTheme;
                }
            }

            /// <summary>
            /// Sets whether arrows are visible. Applies value to all states.
            /// </summary>
            public virtual void SetArrowsVisible(bool value)
            {
                ArrowsVisible[VisualControlState.Normal] = value;
                ArrowsVisible[VisualControlState.Hovered] = value;
                ArrowsVisible[VisualControlState.Disabled] = value;
                ArrowsVisible[VisualControlState.Pressed] = value;
            }

            /// <summary>
            /// Sets whether thumb is visible. Applies value to all states.
            /// </summary>
            public virtual void SetThumbVisible(bool value)
            {
                ThumbVisible[VisualControlState.Normal] = value;
                ThumbVisible[VisualControlState.Hovered] = value;
                ThumbVisible[VisualControlState.Disabled] = value;
                ThumbVisible[VisualControlState.Pressed] = value;
            }

            /// <summary>
            /// Sets whether buttons are visible. Applies value to all states.
            /// </summary>
            public virtual void SetButtonsVisible(bool value)
            {
                ButtonsVisible[VisualControlState.Normal] = value;
                ButtonsVisible[VisualControlState.Hovered] = value;
                ButtonsVisible[VisualControlState.Disabled] = value;
                ButtonsVisible[VisualControlState.Pressed] = value;
            }

            /// <summary>
            /// Sets distance (in dips) between arrow button and arrow.
            /// Applies value to all states.
            /// </summary>
            public virtual void SetArrowMargin(Coord value)
            {
                ArrowMargin[VisualControlState.Normal] = value;
                ArrowMargin[VisualControlState.Hovered] = value;
                ArrowMargin[VisualControlState.Disabled] = value;
                ArrowMargin[VisualControlState.Pressed] = value;
            }

            /// <summary>
            /// Sets whether to use arrow width as the thumb width for the vertical scrollbar
            /// and arrow height as the thumb height for the horizontal scrollbar.
            /// Applies value to all states.
            /// </summary>
            public virtual void SetUseArrowSizeForThumb(bool value)
            {
                UseArrowSizeForThumb[VisualControlState.Normal] = value;
                UseArrowSizeForThumb[VisualControlState.Hovered] = value;
                UseArrowSizeForThumb[VisualControlState.Disabled] = value;
                UseArrowSizeForThumb[VisualControlState.Pressed] = value;
            }

            /// <summary>
            /// Sets distance (in dips) between thumb and scrollbar bounds.
            /// Applies value to all states.
            /// </summary>
            public virtual void SetThumbMargin(Coord value)
            {
                ThumbMargin[VisualControlState.Normal] = value;
                ThumbMargin[VisualControlState.Hovered] = value;
                ThumbMargin[VisualControlState.Disabled] = value;
                ThumbMargin[VisualControlState.Pressed] = value;
            }
        }
    }
}