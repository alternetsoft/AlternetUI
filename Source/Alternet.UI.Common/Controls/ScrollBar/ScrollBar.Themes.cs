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

            /// <summary>
            /// 'Maui Dark' theme is used if control has dark background
            /// and 'Maui Light' theme is used if control has light background.
            /// </summary>
            MauiAuto,

            /// <summary>
            /// 'Maui Light' theme is used.
            /// </summary>
            MauiLight,

            /// <summary>
            /// 'Maui Dark' theme is used.
            /// </summary>
            MauiDark,
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
        /// Provides data for the scrollbar theme initialization events.
        /// </summary>
        public class ThemeInitializeArgs : BaseEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThemeInitializeArgs"/> class.
            /// </summary>
            /// <param name="metrics">Scrollbar theme settings.</param>
            public ThemeInitializeArgs(ThemeMetrics metrics)
            {
                Metrics = metrics;
            }

            /// <summary>
            /// Gets or sets scrollbar drawable which will be initialized.
            /// </summary>
            public ScrollBarDrawable? ScrollBar { get; set; }

            /// <summary>
            /// Gets or sets interior drawable which will be initialized.
            /// </summary>
            public InteriorDrawable? Interior { get; set; }

            /// <summary>
            /// Gets or sets scrollbar theme settings which are used for the initialization.
            /// </summary>
            public ThemeMetrics Metrics { get; set; }
        }

        /// <summary>
        /// Contains different themes colors for the scrollbar parts.
        /// </summary>
        public class ThemeMetrics
        {
            /// <summary>
            /// Gets default array of the visual states which are initialized
            /// in the constructor with the default values.
            /// </summary>
            public static VisualControlState[] AllStatesDefault =
            {
                VisualControlState.Normal,
                VisualControlState.Hovered,
                VisualControlState.Disabled,
                VisualControlState.Pressed,
            };

            /// <summary>
            /// Gets or sets up arrow images in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, SvgImage> UpArrowImage = new();

            /// <summary>
            /// Gets or sets down arrow images in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, SvgImage> DownArrowImage = new();

            /// <summary>
            /// Gets or sets left images in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, SvgImage> LeftArrowImage = new();

            /// <summary>
            /// Gets or sets right arrow images in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, SvgImage> RightArrowImage = new();

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
            private static ThemeMetrics? mauiLight;
            private static ThemeMetrics? mauiDark;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThemeMetrics"/> class.
            /// </summary>
            public ThemeMetrics()
            {
                var states = AllStates;

                UpArrowImage[states] = KnownSvgImages.ImgTriangleArrowUp;
                DownArrowImage[states] = KnownSvgImages.ImgTriangleArrowDown;
                LeftArrowImage[states] = KnownSvgImages.ImgTriangleArrowLeft;
                RightArrowImage[states] = KnownSvgImages.ImgTriangleArrowRight;

                ArrowMargin[states] = 1;
                UseArrowSizeForThumb[states] = true;
                ThumbMargin[states] = 1;
                ArrowsVisible[states] = true;
                ThumbVisible[states] = true;
                ButtonsVisible[states] = true;
            }

            /// <summary>
            /// Occurs when this theme is assigned to the drawables.
            /// </summary>
            public event EventHandler<ThemeInitializeArgs>? ThemeInitialize;

            /// <summary>
            /// Gets or sets 'Maui Dark' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics MauiDarkTheme
            {
                get
                {
                    if (mauiDark is null)
                    {
                        mauiDark = WindowsDarkTheme.Clone();
                    }

                    return mauiDark;
                }

                set
                {
                    mauiDark = value;
                }
            }

            /// <summary>
            /// Gets or sets 'Maui Light' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics MauiLightTheme
            {
                get
                {
                    if (mauiLight is null)
                    {
                        mauiLight = WindowsLightTheme.Clone();
                    }

                    return mauiLight;
                }

                set
                {
                    mauiLight = value;
                }
            }

            /// <summary>
            /// Gets or sets 'Windows Dark' theme colors as <see cref="ThemeMetrics"/>.
            /// </summary>
            public static ThemeMetrics WindowsDarkTheme
            {
                get
                {
                    if (windowsDark is null)
                    {
                        windowsDark = new();
                        var states = windowsDark.AllStates;
                        windowsDark.Background[VisualControlState.Normal] = (46, 46, 46);
                        windowsDark.CornerBackground[VisualControlState.Normal] = (102, 102, 102);
                        windowsDark.Arrow[VisualControlState.Normal] = (153, 153, 153);
                        windowsDark.Arrow[VisualControlState.Hovered] = (153, 153, 153);
                        windowsDark.ThumbBackground[VisualControlState.Normal] = (77, 77, 77);
                        windowsDark.ThumbBorder[VisualControlState.Normal] = (77, 77, 77);
                        windowsDark.ArrowMargin[states] = 1;
                        windowsDark.UseArrowSizeForThumb[states] = true;
                        windowsDark.ThumbMargin[states] = 1;
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
                    if (windowsLight is null)
                    {
                        windowsLight = new();
                        var states = windowsLight.AllStates;
                        windowsLight.Background[VisualControlState.Normal] = (226, 226, 226);
                        windowsLight.CornerBackground[VisualControlState.Normal] = (238, 238, 242);
                        windowsLight.Arrow[VisualControlState.Normal] = (194, 195, 201);
                        windowsLight.Arrow[VisualControlState.Hovered] = (104, 104, 104);
                        windowsLight.ThumbBackground[VisualControlState.Normal] = (194, 195, 201);
                        windowsLight.ThumbBorder[VisualControlState.Normal] = (194, 195, 201);
                        windowsLight.ThumbBackground[VisualControlState.Hovered] = (104, 104, 104);
                        windowsLight.ThumbBorder[VisualControlState.Hovered] = (104, 104, 104);
                        windowsLight.ArrowMargin[states] = 1;
                        windowsLight.UseArrowSizeForThumb[states] = true;
                        windowsLight.ThumbMargin[states] = 1;
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
                    if (visualStudioDark is null)
                    {
                        visualStudioDark = new();
                        var states = visualStudioDark.AllStates;
                        visualStudioDark.Background[VisualControlState.Normal] = (62, 62, 66);
                        visualStudioDark.CornerBackground[VisualControlState.Normal] = (62, 62, 66);
                        visualStudioDark.Arrow[VisualControlState.Normal] = (153, 153, 153);
                        visualStudioDark.Arrow[VisualControlState.Hovered] = (28, 151, 234);
                        visualStudioDark.ThumbBackground[VisualControlState.Normal] = (0, 0, 0);
                        visualStudioDark.ThumbBorder[VisualControlState.Normal] = (104, 104, 104);
                        visualStudioDark.ArrowMargin[states] = 1;
                        visualStudioDark.UseArrowSizeForThumb[states] = false;
                        visualStudioDark.ThumbMargin[states] = 0;
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
                    if (visualStudioLight is null)
                    {
                        visualStudioLight = new();

                        var states = visualStudioLight.AllStates;

                        visualStudioLight.Background[VisualControlState.Normal] = (245, 245, 245);
                        visualStudioLight.Arrow[VisualControlState.Normal] = (134, 137, 153);
                        visualStudioLight.Arrow[VisualControlState.Hovered] = (28, 151, 234);
                        visualStudioLight.ThumbBackground[VisualControlState.Normal] = (255, 255, 255);
                        visualStudioLight.ThumbBorder[VisualControlState.Normal] = (0, 0, 0);
                        visualStudioLight.CornerBackground[VisualControlState.Normal] = (245, 245, 245);
                        visualStudioLight.ArrowMargin[states] = 1;
                        visualStudioLight.UseArrowSizeForThumb[states] = false;
                        visualStudioLight.ThumbMargin[states] = 0;
                    }

                    return visualStudioLight;
                }

                set
                {
                    visualStudioLight = value;
                }
            }

            /// <summary>
            /// Gets array of the visual states which are initialized
            /// in the constructor with the default values.
            /// </summary>
            public virtual VisualControlState[] AllStates => AllStatesDefault;

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
                    case KnownTheme.MauiDark:
                        return MauiDarkTheme;
                    case KnownTheme.MauiLight:
                        return MauiLightTheme;
                    case KnownTheme.MauiAuto:
                        if (isDark)
                            return MauiDarkTheme;
                        return MauiLightTheme;
                }
            }

            /// <summary>
            /// Assigns properties of this object with the properties of another object.
            /// </summary>
            /// <param name="assignFrom">Object which properties will be assigned to this object.</param>
            public void Assign(ThemeMetrics assignFrom)
            {
                UpArrowImage = assignFrom.UpArrowImage.Clone();
                DownArrowImage = assignFrom.DownArrowImage.Clone();
                LeftArrowImage = assignFrom.LeftArrowImage.Clone();
                RightArrowImage = assignFrom.RightArrowImage.Clone();
                CornerBackground = assignFrom.CornerBackground.Clone();
                Background = assignFrom.Background.Clone();
                Arrow = assignFrom.Arrow.Clone();
                ThumbBackground = assignFrom.ThumbBackground.Clone();
                ArrowMargin = assignFrom.ArrowMargin.Clone();
                UseArrowSizeForThumb = assignFrom.UseArrowSizeForThumb.Clone();
                ThumbMargin = assignFrom.ThumbMargin.Clone();
                ArrowsVisible = assignFrom.ArrowsVisible.Clone();
                ThumbVisible = assignFrom.ThumbVisible.Clone();
                ButtonsVisible = assignFrom.ButtonsVisible.Clone();
                ThumbBorder = assignFrom.ThumbBorder.Clone();
            }

            /// <summary>
            /// Creates clone of this object.
            /// </summary>
            /// <returns></returns>
            public ThemeMetrics Clone()
            {
                ThemeMetrics result = new();
                result.Assign(this);
                return result;
            }

            /// <summary>
            /// Raises <see cref="ThemeInitialize"/> event.
            /// </summary>
            /// <param name="e">Event arguments.</param>
            public virtual void RaiseThemeInitialize(ThemeInitializeArgs e)
            {
                ThemeInitialize?.Invoke(this, e);
            }
        }
    }
}