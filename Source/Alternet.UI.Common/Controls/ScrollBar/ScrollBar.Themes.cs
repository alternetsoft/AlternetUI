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
        /// Custom scrollbar theme without any properties.
        /// </summary>
        public class CustomThemeMetrics
        {
            /// <summary>
            /// Initializes interior drawable with this theme colors and metrics.
            /// </summary>
            public virtual void AssignTo(InteriorDrawable interior)
            {
            }

            /// <summary>
            /// Initializes scrollbar drawable with this theme colors and metrics.
            /// </summary>
            public virtual void AssignTo(ScrollBarDrawable scrollBar)
            {
            }
        }

        /// <summary>
        /// Contains themes colors and metrics for the scrollbar parts.
        /// </summary>
        public class ThemeMetrics : CustomThemeMetrics
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
            /// Gets or sets distance (in dips) between arrow button and
            /// arrow in the different visual states.
            /// </summary>
            public EnumArray<VisualControlState, Coord> ArrowMargin = new();

            /// <summary>
            /// Gets or sets whether to use arrow width as the thumb width
            /// for the vertical scrollbar
            /// and arrow height as the thumb height for the horizontal scrollbar.
            /// Each visual state has its own setting.
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
            private static LightDarkColor? defaultHoveredColor;

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
            /// Occurs when this theme is assigned to the drawable objects.
            /// </summary>
            public event EventHandler<ThemeInitializeArgs>? ThemeInitialize;

            /// <summary>
            /// Gets or sets the factor by which the color is adjusted
            /// when a dark theme element is hovered.
            /// </summary>
            /// <remarks>
            /// If this property is set to <c>null</c>, the hovered color will not be adjusted.
            /// Positive value makes the color lighter, negative value makes it darker.
            /// </remarks>
            public static float? DarkThemeHoveredFactor { get; set; } = 0.5f;

            /// <summary>
            /// Gets or sets the factor by which the color is adjusted
            /// when a light theme element is hovered.
            /// </summary>
            /// <remarks>
            /// If this property is set to <c>null</c>, the hovered color will not be adjusted.
            /// Positive value makes the color lighter, negative value makes it darker.
            /// </remarks>
            public static float? LightThemeHoveredFactor { get; set; } = -0.01f;

            /// <summary>
            /// Gets or sets the default color used when an element is hovered over.
            /// </summary>
            public static LightDarkColor? DefaultHoveredColor
            {
                get => defaultHoveredColor;
                set => defaultHoveredColor = value;
            }

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
                    windowsDark ??= CreateWindowsDarkTheme();
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
                    windowsLight ??= CreateWindowsLightTheme();
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
                        Color hoveredColor = (28, 151, 234);

                        visualStudioDark = new();
                        var states = visualStudioDark.AllStates;
                        visualStudioDark.Background[VisualControlState.Normal] = (62, 62, 66);
                        visualStudioDark.CornerBackground[VisualControlState.Normal] = (62, 62, 66);
                        visualStudioDark.Arrow[VisualControlState.Normal] = (153, 153, 153);
                        visualStudioDark.Arrow[VisualControlState.Hovered] = hoveredColor;
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
                        visualStudioLight.CornerBackground[VisualControlState.Normal]
                            = (245, 245, 245);
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
            /// Creates a new instance of the 'Windows Dark' theme with
            /// predefined colors and metrics.
            /// </summary>
            /// <returns>A <see cref="ThemeMetrics"/> object representing
            /// the 'Windows Dark' theme.</returns>
            public static ThemeMetrics CreateWindowsDarkTheme()
            {
                ThemeMetrics windowsDark = new();

                Color background = (47, 47, 47);
                Color foreground = (160, 160, 160);

                windowsDark.SetThemeColors(background, foreground);
                var states = windowsDark.AllStates;
                windowsDark.ArrowMargin[states] = 1;
                windowsDark.UseArrowSizeForThumb[states] = true;
                windowsDark.ThumbMargin[states] = 1;

                windowsDark.SetHoveredColors(true, foreground);

                return windowsDark;
            }

            /// <summary>
            /// Determines the default hovered color based on the specified theme and base color.
            /// </summary>
            /// <param name="isDark">A boolean value indicating whether the dark
            /// theme is applied. <see langword="true"/> for dark theme;
            /// otherwise, <see langword="false"/>.</param>
            /// <param name="baseColor">The base <see cref="Color"/> from which the
            /// hovered color is derived.</param>
            /// <returns>The calculated hovered <see cref="Color"/> based on the
            /// theme and base color. If no specific factor is
            /// available, returns the base color.</returns>
            public static Color GetDefaultHoveredColor(bool isDark, Color baseColor)
            {
                if (DefaultHoveredColor is not null)
                {
                    return DefaultHoveredColor.LightOrDark(isDark);
                }

                float? factor;

                if (isDark)
                {
                    factor = DarkThemeHoveredFactor;
                }
                else
                {
                    factor = LightThemeHoveredFactor;
                }

                if (factor is not null)
                {
                    var hoveredColor = baseColor.LighterOrDarker(factor.Value);
                    return hoveredColor;
                }

                return baseColor;
            }

            /// <summary>
            /// Creates a new instance of the 'Windows Light' theme with
            /// predefined colors and metrics.
            /// </summary>
            /// <returns>A <see cref="ThemeMetrics"/> object representing
            /// the 'Windows Light' theme.</returns>
            public static ThemeMetrics CreateWindowsLightTheme()
            {
                ThemeMetrics windowsLight = new();
                Color background = (240, 240, 240);
                Color foreground = (194, 195, 201);

                windowsLight.SetThemeColors(background, foreground);
                var states = windowsLight.AllStates;
                windowsLight.ArrowMargin[states] = 1;
                windowsLight.UseArrowSizeForThumb[states] = true;
                windowsLight.ThumbMargin[states] = 3;

                windowsLight.SetHoveredColors(false, foreground);

                return windowsLight;
            }

            /// <summary>
            /// Returns theme colors for the specified <paramref name="theme"/> and
            /// <paramref name="isDark"/> parameters.
            /// </summary>
            /// <param name="isDark">Whether to return dark or light theme when any of the 'auto'
            /// themes is specified in the <paramref name="theme"/> parameter.</param>
            /// <param name="theme"><see cref="KnownTheme"/> value.</param>
            /// <returns></returns>
            public static ThemeMetrics GetTheme(KnownTheme theme, bool isDark = false)
            {
                switch (theme)
                {
                    default:
                    case KnownTheme.System:
                        theme = SystemTheme;
                        if (theme == KnownTheme.System)
                            theme = KnownTheme.WindowsAuto;
                        return GetTheme(theme, isDark);
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
            /// <param name="assignFrom">Object which properties will be assigned
            /// to this object.</param>
            public virtual void Assign(ThemeMetrics assignFrom)
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
            public virtual ThemeMetrics Clone()
            {
                ThemeMetrics result = new();
                result.Assign(this);
                return result;
            }

            /// <inheritdoc/>
            public override void AssignTo(InteriorDrawable interior)
            {
                interior.VertScrollBar ??= new();
                interior.VertScrollBar.IsVertical = true;
                this.AssignTo(interior.VertScrollBar);

                interior.HorzScrollBar ??= new();
                interior.HorzScrollBar.IsVertical = false;
                this.AssignTo(interior.HorzScrollBar);

                interior.Corner ??= new();
                interior.Corner.Brush = this.CornerBackground[VisualControlState.Normal]?.AsBrush
                    ?? this.Background[VisualControlState.Normal]?.AsBrush;

                if(ThemeInitialize is not null)
                {
                    ScrollBar.ThemeInitializeArgs e = new(this);
                    e.Interior = interior;
                    ThemeInitialize.Invoke(this, e);
                }
            }

            /// <summary>
            /// Sets the colors for the hovered state of the visual elements
            /// based on the specified theme and base color.
            /// </summary>
            /// <remarks>This method updates the hovered state colors for the thumb background, arrow,
            /// and thumb border using a color derived from the specified base color and theme.</remarks>
            /// <param name="isDark">A <see langword="true"/> if the dark theme is applied;
            /// otherwise, <see langword="false"/>.</param>
            /// <param name="baseColor">The base <see cref="Color"/> used to determine
            /// the hovered state color.</param>
            public void SetHoveredColors(bool isDark, Color baseColor)
            {
                var hoveredColor = GetDefaultHoveredColor(isDark, baseColor);

                ThumbBackground[VisualControlState.Hovered] = hoveredColor;
                Arrow[VisualControlState.Hovered] = hoveredColor;
                ThumbBorder[VisualControlState.Hovered] = hoveredColor;
            }

            /// <summary>
            /// Sets the same background and foreground colors for all parts of the theme.
            /// </summary>
            /// <param name="background">The background color to apply.</param>
            /// <param name="foreground">The foreground color to apply.</param>
            public void SetThemeColors(Color background, Color foreground)
            {
                Background[VisualControlState.Normal] = background;
                CornerBackground[VisualControlState.Normal] = background;
                Arrow[VisualControlState.Normal] = foreground;
                Arrow[VisualControlState.Hovered] = foreground;
                ThumbBackground[VisualControlState.Normal] = foreground;
                ThumbBorder[VisualControlState.Normal] = foreground;
            }

            /// <inheritdoc/>
            public override void AssignTo(ScrollBarDrawable scrollBar)
            {
                scrollBar.ArrowMargin = this.ArrowMargin;
                scrollBar.UseArrowSizeForThumb = this.UseArrowSizeForThumb;
                scrollBar.ThumbMargin = this.ThumbMargin;

                scrollBar.Background = new();
                scrollBar.Background.Normal = CreateBackgroundState(VisualControlState.Normal);
                scrollBar.Background.Hovered = CreateBackgroundState(VisualControlState.Hovered);
                scrollBar.Background.Disabled = CreateBackgroundState(VisualControlState.Disabled);

                scrollBar.Thumb = new();
                scrollBar.Thumb.Normal = CreateThumbState(VisualControlState.Normal);
                scrollBar.Thumb.Hovered = CreateThumbState(VisualControlState.Hovered);
                scrollBar.Thumb.Disabled = CreateThumbState(VisualControlState.Disabled);

                InitArrowStates(ref scrollBar.UpArrow, this.UpArrowImage);
                InitArrowStates(ref scrollBar.DownArrow, this.DownArrowImage);
                InitArrowStates(ref scrollBar.LeftArrow, this.LeftArrowImage);
                InitArrowStates(ref scrollBar.RightArrow, this.RightArrowImage);

                ScrollBar.ThemeInitializeArgs e = new(this);
                e.ScrollBar = scrollBar;
                ThemeInitialize?.Invoke(this, e);

                RectangleDrawable? CreateBackgroundState(VisualControlState state)
                {
                    var background = this.Background[state];

                    if (background is null)
                        return null;

                    RectangleDrawable result = new();

                    result.Brush = background.AsBrush;

                    return result;
                }

                RectangleDrawable? CreateThumbState(VisualControlState state)
                {
                    var background = this.ThumbBackground[state];
                    var border = this.ThumbBorder[state];

                    if (background is null && border is null)
                        return null;

                    RectangleDrawable result = new();

                    result.Brush = background?.AsBrush;

                    if (border is not null)
                    {
                        result.Border = new();
                        result.Border.Color = border.AsColor;
                        result.Border.Width = 1;
                    }

                    return result;
                }

                void InitArrowStates(
                    ref ControlStateObjects<RectangleDrawable>? arrow,
                    EnumArray<VisualControlState, SvgImage> svgImage)
                {
                    arrow ??= new();

                    arrow.Normal = CreateStateProps(VisualControlState.Normal);
                    arrow.Hovered = CreateStateProps(VisualControlState.Hovered);
                    arrow.Disabled = CreateStateProps(VisualControlState.Disabled);

                    RectangleDrawable? CreateStateProps(VisualControlState state)
                    {
                        var arrow = this.Arrow[state];

                        if (arrow is null)
                            return null;

                        RectangleDrawable result = new();

                        result.SvgImage = new(svgImage[state], arrow.AsColor);
                        result.HasImage = true;
                        result.Stretch = false;
                        result.CenterHorz = true;
                        result.CenterVert = true;

                        return result;
                    }
                }
            }
        }
    }
}