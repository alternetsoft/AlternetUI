using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class SpeedButton
    {
        /// <summary>
        /// Represents the default border radius used for rounding corners.
        /// This value is used in the <see cref="KnownTheme.RoundBorder"/> theme.
        /// </summary>
        public static Coord DefaultRoundBorderRadius = 25;

        /// <summary>
        /// Indicates whether the default round border radius is expressed as a percentage.
        /// This value is used in the <see cref="KnownTheme.RoundBorder"/> theme.
        /// </summary>
        public static bool DefaultRoundBorderRadiusIsPercent = true;

        /// <summary>
        /// Gets or sets default click repeat delay used when
        /// <see cref="SpeedButton.IsClickRepeated"/> is True.
        /// </summary>
        /// <remarks>
        /// The default value of delay is 50 milliseconds. This means
        /// the first event is initiated after 250 milliseconds (5 times the specified value).
        /// Each subsequent event is initiated after 50 milliseconds delay.
        /// </remarks>
        public static int DefaultClickRepeatDelay = 50;

        /// <summary>
        /// Gets or sets default image and label distance in the <see cref="SpeedButton"/>.
        /// </summary>
        public static Coord DefaultImageLabelDistance = 4;

        /// <summary>
        /// Gets or sets default padding in the <see cref="SpeedButton"/>.
        /// </summary>
        public static Coord DefaultPadding = 4;

        /// <summary>
        /// Gets or sets default border width of the <see cref="SpeedButton"/> in the sticky state.
        /// </summary>
        public static Coord DefaultStickyBorderWidth = 1;

        /// <summary>
        /// Gets ot sets default value of <see cref="UseTheme"/> property.
        /// </summary>
        public static KnownTheme DefaultUseTheme = KnownTheme.Default;

        /// <summary>
        /// Gets ot sets default value of <see cref="UseThemeForSticky"/> property.
        /// </summary>
        public static KnownTheme DefaultUseThemeForSticky = KnownTheme.StickyBorder;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.Default"/>.
        /// </summary>
        public static ControlColorAndStyle DefaultTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.StaticBorder"/>.
        /// </summary>
        public static ControlColorAndStyle StaticBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.RoundBorder"/>.
        /// </summary>
        public static ControlColorAndStyle RoundBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.SquareCorners"/>.
        /// </summary>
        public static ControlColorAndStyle SquareCornersTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.StickyBorder"/>.
        /// </summary>
        public static ControlColorAndStyle StickyBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.CheckBorder"/>.
        /// </summary>
        public static ControlColorAndStyle CheckBorderTheme;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.NoBorder"/>.
        /// </summary>
        public static ControlColorAndStyle NoBorderTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.PushButton"/>.
        /// </summary>
        public static ControlColorAndStyle PushButtonTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.PushButtonHovered"/>.
        /// </summary>
        public static ControlColorAndStyle PushButtonHoveredTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.PushButtonPressed"/>.
        /// </summary>
        public static ControlColorAndStyle PushButtonPressedTheme = new();

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.Custom"/>.
        /// </summary>
        public static ControlColorAndStyle? DefaultCustomTheme = null;

        /// <summary>
        /// Gets or sets default color and style settings
        /// for all <see cref="SpeedButton"/> controls
        /// which have <see cref="UseTheme"/> equal to <see cref="KnownTheme.TabControl"/>.
        /// </summary>
        public static ControlColorAndStyle TabControlTheme;

        private static bool? ignoreMenuItemShortcuts;
        private static int toolTipSuppressCount = 0;

        /// <summary>
        /// Gets or sets a value indicating whether menu item shortcuts should be ignored.
        /// this property is used when <see cref="SpeedButton"/> is used for displaying a menu item.
        /// Default value is true on phone devices, false otherwise.
        /// </summary>
        public static bool IgnoreMenuItemShortcuts
        {
            get
            {
                return ignoreMenuItemShortcuts ??= App.IsPhoneDevice;
            }

            set
            {
                ignoreMenuItemShortcuts = value;
            }
        }

        /// <summary>
        /// Indicates whether tooltips in all <see cref="SpeedButton"/> controls are
        /// suppressed throughout the application.
        /// </summary>
        /// <remarks>When set to <see langword="true"/>, no tooltips will be displayed, regardless of
        /// individual control settings. This can be useful in scenarios where tooltips may interfere with user
        /// experience or accessibility requirements.</remarks>
        public static bool AllToolTipsSuppressed
        {
            get
            {
                return toolTipSuppressCount > 0;
            }
        }

        /// <summary>
        /// Gets or sets default template for the shortcut when it is shown in the tooltip.
        /// </summary>
        /// <remarks>
        /// Default value is "({0})".
        /// </remarks>
        public static string DefaultShortcutToolTipTemplate { get; set; } = "({0})";

        /// <summary>
        /// Resets color themes of <see cref="SpeedButton"/> to their initial values.
        /// </summary>
        [MemberNotNull(nameof(StaticBorderTheme))]
        [MemberNotNull(nameof(RoundBorderTheme))]
        [MemberNotNull(nameof(SquareCornersTheme))]
        [MemberNotNull(nameof(StickyBorderTheme))]
        [MemberNotNull(nameof(CheckBorderTheme))]
        [MemberNotNull(nameof(TabControlTheme))]
        [MemberNotNull(nameof(PushButtonTheme))]
        [MemberNotNull(nameof(PushButtonHoveredTheme))]
        [MemberNotNull(nameof(PushButtonPressedTheme))]
        public static void ResetThemes()
        {
            var borderColor = DefaultColors.BorderColor;

            var corners = new BorderCornerRadius(DefaultRoundBorderRadius, DefaultRoundBorderRadiusIsPercent);
            var squareCorners = new BorderCornerRadius(0, false);

            InitThemeLight(DefaultTheme.Light, corners);
            InitThemeDark(DefaultTheme.Dark, corners);

            SquareCornersTheme = new();
            InitThemeLight(SquareCornersTheme.Light, squareCorners);
            InitThemeDark(SquareCornersTheme.Dark, squareCorners);

            TabControlTheme = new();
            InitThemeLight(TabControlTheme.Light, squareCorners);
            InitThemeDark(TabControlTheme.Dark, squareCorners);

            StaticBorderTheme = DefaultTheme.Clone();
            StaticBorderTheme.NormalBorderAsHovered();
            StaticBorderTheme.DisabledBorderAsHovered();
            StaticBorderTheme.SetBorderColor(borderColor);

            RoundBorderTheme = StaticBorderTheme.Clone();
            RoundBorderTheme.SetCornerRadius(DefaultRoundBorderRadius, DefaultRoundBorderRadiusIsPercent);

            StickyBorderTheme = CreateBordered(borderColor, corners);
            CheckBorderTheme = CreateBordered(DefaultColors.DefaultCheckBoxColor, corners);

            ControlColorAndStyle CreateBordered(LightDarkColor? color, BorderCornerRadius? cornerRadius = null)
            {
                var result = DefaultTheme.Clone();
                result.SetBorderFromBorder(stateToChange: VisualControlState.Normal, assignFromState: VisualControlState.Hovered);
                result.SetBorderWidth(DefaultStickyBorderWidth);
                result.SetBorderColor(color);
                return result;
            }

            PushButtonTheme = CreatePushButtonTheme(null);
            PushButtonHoveredTheme = CreatePushButtonTheme(DrawingUtils.DrawPushButtonHovered);
            PushButtonPressedTheme = CreatePushButtonTheme(DrawingUtils.DrawPushButtonPressed);

            ControlColorAndStyle CreatePushButtonTheme(PaintEventHandler? normalStateOverride)
            {
                var result = DefaultTheme.Clone();
                result.SetAsPushButton();
                if (normalStateOverride is null)
                    return result;
                result.Light.BackgroundActions!.Normal = normalStateOverride;
                result.Dark.BackgroundActions!.Normal = normalStateOverride;
                return result;
            }
        }

        /// <summary>
        /// Initializes default colors and styles for the <see cref="SpeedButton"/>
        /// using 'Light' color theme.
        /// </summary>
        /// <param name="theme"><see cref="ControlStateSettings"/> to initialize.</param>
        /// <param name="cornerRadius">Corner radius for the borders. If null, default value is used.</param>
        public static void InitThemeLight(ControlStateSettings theme, BorderCornerRadius? cornerRadius = null)
        {
            AllStateColors colors = new()
            {
                HoveredForeColor = (0, 0, 0),
                HoveredBackColor = (229, 243, 255),

                DisabledForeColor = SystemColors.GrayText,

                PressedForeColor = (0, 0, 0),
                PressedBackColor = (204, 228, 247),
            };

            theme.Borders = CreateBorders(color: (204, 232, 255), cornerRadius);
            theme.Colors = colors.AllStates;
            theme.Backgrounds = theme.Colors;
        }

        /// <summary>
        /// Suppresses all tooltips in <see cref="SpeedButton"/> controls globally by
        /// incrementing the suppression count.
        /// In order to resume tooltips, a corresponding call to <see cref="AllToolTipsResume"/> must be made.
        /// </summary>
        /// <remarks>Call this method to prevent tooltips from being displayed until suppression is
        /// lifted. This is typically used when performing operations where tooltips would be distracting or
        /// undesirable. To re-enable tooltips, ensure that the corresponding method to decrease the suppression count
        /// is called.</remarks>
        public static void AllToolTipsSuppress()
        {
            toolTipSuppressCount++;
        }

        /// <summary>
        /// Resumes all tooltips in <see cref="SpeedButton"/> controls globally by decrementing the suppression count.
        /// This method should be called after a corresponding call to <see cref="AllToolTipsSuppress"/>
        /// to re-enable tooltips.
        /// </summary>
        public static void AllToolTipsResume()
        {
            if (toolTipSuppressCount > 0)
                toolTipSuppressCount--;
            else
                throw new InvalidOperationException("ToolTip suppression count is already zero.");
        }

        /// <summary>
        /// Initializes default colors and styles for the <see cref="SpeedButton"/>
        /// using 'Dark' color theme.
        /// </summary>
        /// <param name="theme"><see cref="ControlStateSettings"/> to initialize.</param>
        /// <param name="cornerRadius">Optional corner radius for the borders. If not specified,
        /// default value will be used.</param>
        public static void InitThemeDark(ControlStateSettings theme, BorderCornerRadius? cornerRadius = null)
        {
            AllStateColors colors = new()
            {
                HoveredForeColor = (250, 250, 250),
                HoveredBackColor = (61, 61, 61),

                DisabledForeColor = SystemColors.GrayText,

                PressedForeColor = (214, 214, 214),
                PressedBackColor = (34, 34, 34),
            };

            theme.Borders = CreateBorders((112, 112, 112), cornerRadius);
            theme.Colors = colors.AllStates;
            theme.Backgrounds = theme.Colors;
        }

        /// <summary>
        /// Creates borders for the <see cref="SpeedButton"/>
        /// using default border width
        /// and using specified <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Border color.</param>
        /// <param name="cornerRadius">Corner radius for the border.</param>
        /// <returns></returns>
        public static ControlStateBorders CreateBorders(Color color, BorderCornerRadius? cornerRadius = null)
        {
            BorderSettings hoveredBorder = new()
            {
                Width = 1,
                Color = color,
            };

            hoveredBorder.SetCornerRadius(cornerRadius);

            ControlStateBorders borders = new();
            var pressedBorder = hoveredBorder.Clone();
            borders.SetObject(hoveredBorder, VisualControlState.Hovered);
            borders.SetObject(pressedBorder, VisualControlState.Pressed);
            return borders;
        }
    }
}
