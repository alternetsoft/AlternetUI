using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="SystemSettings"/> allows the application to ask for details about the system.
    /// This can include settings such as standard colors, fonts, and user interface element sizes.
    /// </summary>
    public static class SystemSettings
    {
        /// <summary>
        /// Represents the size of the area around the mouse cursor, in device independent units,
        /// that triggers a mouse hover event. Default value is 4 dips.
        /// </summary>
        public static Coord MouseHoverSize = 4;

        private static ISystemSettingsHandler? handler;
        private static bool validColors;
        private static bool? appearanceIsDarkOverride;
        private static bool? isUsingDarkBackgroundOverride;
        private static bool insideResetColors;
        private static int mouseHoverTime = 400;
        private static int colorsVersion;

        static SystemSettings()
        {
            validColors = false;
        }

        /// <summary>
        /// Occurs when the system colors change.
        /// </summary>
        public static event Action? SystemColorsChanged;

        /// <summary>
        /// Gets the version number of the color configuration.
        /// </summary>
        public static int ColorsVersion
        {
            get => colorsVersion;
        }

        /// <summary>
        /// Gets or sets the duration, in milliseconds, that the mouse pointer must
        /// remain stationary within a control and hover rectangle for a hover event to be raised.
        /// Default value is 400 milliseconds.
        /// </summary>
        public static int MouseHoverTime
        {
            get => mouseHoverTime;

            set
            {
                if (mouseHoverTime == value)
                    return;
                mouseHoverTime = value;
                if (TimerUtils.IsMouseHoverTimerCreated)
                {
                    TimerUtils.MouseHoverTimer.Stop();
                    TimerUtils.MouseHoverTimer.Interval = mouseHoverTime;
                }
            }
        }

        /// <summary>
        /// Gets whether <see cref="SystemSettings"/> is initialized and
        /// can be used. Normally this returns False when <see cref="Handler"/>
        /// is not yet assigned.
        /// </summary>
        public static bool HasHandler
        {
            get
            {
                return handler is not null;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ISystemSettingsHandler"/> provider used to work
        /// with system settings.
        /// </summary>
        public static ISystemSettingsHandler Handler
        {
            get => handler ??= App.Handler.CreateSystemSettingsHandler();

            set => handler = value;
        }

        /// <summary>
        /// Returns the name if available or empty string otherwise.
        /// </summary>
        /// <remarks>
        /// This is currently only implemented for macOS and returns a not necessarily
        /// user-readable string such as "NSAppearanceNameAqua" there and an empty string
        /// under all the other platforms.
        /// </remarks>
        public static string AppearanceName
        {
            get => Handler.GetAppearanceName();
        }

        /// <summary>
        /// Gets or sets an override for <see cref="IsUsingDarkBackground"/>.
        /// </summary>
        public static bool? IsUsingDarkBackgroundOverride
        {
            get
            {
                return isUsingDarkBackgroundOverride;
            }

            set
            {
                isUsingDarkBackgroundOverride = value;
            }
        }

        /// <summary>
        /// Gets or sets an override for <see cref="AppearanceIsDark"/>.
        /// </summary>
        public static bool? AppearanceIsDarkOverride
        {
            get
            {
                return appearanceIsDarkOverride;
            }

            set
            {
                appearanceIsDarkOverride = value;
            }
        }

        /// <summary>
        /// Return true if the current system theme is explicitly recognized as
        /// being a dark theme or if the default window background is dark.
        /// </summary>
        /// <remarks>
        /// This method should be used to check whether custom colors more appropriate
        /// for the default (light) or dark appearance should be used.
        /// </remarks>
        public static bool AppearanceIsDark
        {
            get
            {
                return appearanceIsDarkOverride ?? Handler.GetAppearanceIsDark();
            }
        }

        /// <summary>
        /// Returns true if the background is darker than foreground. This is used by
        /// <see cref="AppearanceIsDark"/> if there is no platform-specific way to determine
        /// whether a dark mode is being used and is generally not very useful to call directly.
        /// </summary>
        public static bool IsUsingDarkBackground
        {
            get
            {
                return isUsingDarkBackgroundOverride ?? Handler.IsUsingDarkBackground();
            }
        }

        /// <summary>
        /// Returns the value of a system metric, or -1 if the metric is not supported on
        /// the current system.
        /// </summary>
        /// <param name="index">System metric identifier.</param>
        public static int GetMetric(SystemSettingsMetric index)
        {
            return Handler.GetMetric(index);
        }

        /// <summary>
        /// Returns the value of a system metric, or -1 if the metric is not supported on
        /// the current system.
        /// </summary>
        /// <param name="index">System metric identifier.</param>
        /// <param name="control">Control for which metric is requested (optional).</param>
        /// <remarks>
        /// The value of <paramref name="control"/> determines if the metric returned is a global
        /// value or a control based value, in which case it might determine the widget, the
        /// display the window is on, or something similar. The window given should be as close
        /// to the metric as possible (e.g.a <see cref="Window"/> in case of
        /// the <see cref="SystemSettingsMetric.CaptionY"/> metric).
        /// </remarks>
        /// <remarks>
        /// Specifying the <paramref name="control"/> parameter is encouraged, because some
        /// metrics on some ports are not supported without one,or they might be capable of
        /// reporting better values if given one. If a control does not make sense for a metric,
        /// one should still be given, as for example it might determine which displays
        /// cursor width is requested with <see cref="SystemSettingsMetric.CursorX"/>.
        /// </remarks>
        public static int GetMetric(SystemSettingsMetric index, AbstractControl? control)
        {
            return Handler.GetMetric(index, control);
        }

        /// <summary>
        /// Returns true if <paramref name="backgroundColor"/> is darker
        /// than <paramref name="foregroundColor"/>.
        /// </summary>
        public static bool IsDarkBackground(Color foregroundColor, Color backgroundColor)
        {
            // The threshold here is rather arbitrary, but it seems that using just
            // inequality would be wrong as it could result in false results.
            if (foregroundColor.IsOk && backgroundColor.IsOk)
                return foregroundColor.GetLuminance() - backgroundColor.GetLuminance() > 0.2;
            else
                return SystemSettings.IsUsingDarkBackground;
        }

        /// <summary>
        /// Gets default font size increment depending on the dpi value of the screen.
        /// Uses <see cref="Window.IncFontSize"/> and <see cref="Window.IncFontSizeHighDpi"/>
        /// properties.
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultFontSizeIncrement()
        {
            if (App.IsMaui)
                return 0;

            int GetIncFontSizeHighDpi()
            {
                var result = Window.IncFontSizeHighDpi
                    ?? AllPlatformDefaults.PlatformCurrent.IncFontSizeHighDpi;
                return result;
            }

            int GetIncFontSize()
            {
                var result = Window.IncFontSize
                    ?? AllPlatformDefaults.PlatformCurrent.IncFontSize;
                return result;
            }

            var dpi = Display.MaxDPI;
            var incFont = (dpi > 96) ? GetIncFontSizeHighDpi() : GetIncFontSize();
            return incFont;
        }

        /// <summary>
        /// Initializes <see cref="Window.IncFontSizeHighDpi"/> and
        /// <see cref="Window.IncFontSize"/> with the default values.
        /// </summary>
        public static void InitDefaultFontSizeIncrement(int? value = null)
        {
            Window.IncFontSizeHighDpi = value ?? AllPlatformDefaults.DefaultIncFontSizeHighDpi;
            Window.IncFontSize = value ?? AllPlatformDefaults.DefaultIncFontSize;
        }

        /// <summary>
        /// Logs all fonts enumerated in <see cref="SystemSettingsFont"/>.
        /// </summary>
        public static void LogSystemFonts()
        {
            App.LogBeginSection("System Fonts");

            App.Log($"Control.DefaultFont: {AbstractControl.DefaultFont.ToInfoString()}");
            App.Log($"Default font: {Font.Default.ToInfoString()}");
            App.Log($"Default mono font: {Font.DefaultMono.ToInfoString()}");

            App.LogEmptyLine();

            var values = Enum.GetValues(typeof(GenericFontFamily));
            foreach (var value in values)
            {
                if ((GenericFontFamily)value == GenericFontFamily.None)
                    continue;

                var font = SystemFonts.GetFont((GenericFontFamily)value);

                App.Log($"Font {value}: {font.ToInfoString()}");
            }

            App.LogEmptyLine();

            values = Enum.GetValues(typeof(SystemSettingsFont));
            foreach(var value in values)
            {
                var font = SystemFonts.GetFont((SystemSettingsFont)value);

                App.Log($"Font {value}: {font.ToInfoString()}");
            }

            App.LogEndSection();
        }

        /// <summary>
        /// Increments the version number for the color configuration.
        /// This method is called internally when colors are reset, by <see cref="ResetColors"/>.
        /// </summary>
        /// <remarks>This method updates the internal version number, which can be used to track changes
        /// to the color configuration.</remarks>
        /// <see cref="ColorsVersion"/>.
        public static void IncVersion()
        {
            colorsVersion++;
        }

        /// <summary>
        /// Logs all fonts with fixed widths (<see cref="Font.IsFixedWidth"/> property is true).
        /// </summary>
        public static void LogFixedWidthFonts()
        {
            App.LogBeginSection("Fixed Width Fonts");
            var items = FontFamily.FamiliesNamesAscending;
            var hasFonts = false;
            foreach(var item in items)
            {
                var font = new Font(item, 10);

                if(font.IsFixedWidth && font.Style == FontStyle.Regular && !font.GdiVerticalFont)
                {
                    App.Log(font.ToInfoString());
                    hasFonts = true;
                }
            }

            if (!hasFonts)
            {
                App.Log("No fixed width fonts are found.");
            }

            App.LogEndSection();
        }

        /// <summary>
        /// Returns <c>true</c> if the port has certain feature.
        /// </summary>
        /// <param name="index">System feature identifier.</param>
        public static bool HasFeature(SystemSettingsFeature index)
        {
            return Handler.HasFeature(index);
        }

        /// <summary>
        /// Gets a standard system color.
        /// </summary>
        /// <param name="index">System color identifier.</param>
        public static ColorStruct GetColor(KnownSystemColor index)
        {
            if (!validColors)
            {
                ResetColors();
                validColors = true;
            }

            return PlessSystemColors.GetColor(index);
        }

        /// <summary>
        /// Resets internal structures related to the system colors.
        /// and raises <see cref="SystemColorsChanged"/> event.
        /// </summary>
        public static void ResetColors()
        {
            if (insideResetColors)
                return;
            insideResetColors = true;
            IncVersion();
            try
            {
                validColors = false;
                PlessSystemColors.Reset();
                DefaultColors.Initialize();
                SystemColorsChanged?.Invoke();

                FormUtils.ForEach((form) =>
                {
                    if (form.DisposingOrDisposed)
                        return;
                    form.RaiseSystemColorsChanged(EventArgs.Empty);
                    if (!form.HasChildren)
                        return;
                    form.ForEachChild((c) => c.RaiseSystemColorsChanged(EventArgs.Empty), true);
                });

                FormUtils.InvalidateAll();

                App.DebugLogIf("System colors were changed.", true);
            }
            finally
            {
                insideResetColors = false;
            }
        }

        /// <summary>
        /// Gets system font.
        /// </summary>
        /// <param name="font">System font id.</param>
        /// <returns></returns>
        public static Font GetFont(SystemSettingsFont font) => SystemFonts.GetFont(font);

        /// <summary>
        /// Calls the specified action inside the block which temporary changes
        /// value of the <see cref="AppearanceIsDarkOverride"/> property.
        /// </summary>
        /// <param name="tempAppearanceIsDark">Temporary value for the
        /// <see cref="AppearanceIsDarkOverride"/> property.</param>
        /// <param name="action">Action to call.</param>
        public static void DoInsideTempAppearanceIsDark(bool? tempAppearanceIsDark, Action? action)
        {
            var savedOverride = AppearanceIsDarkOverride;
            try
            {
                AppearanceIsDarkOverride = tempAppearanceIsDark;
                action?.Invoke();
            }
            finally
            {
                AppearanceIsDarkOverride = savedOverride;
            }
        }
    }
}