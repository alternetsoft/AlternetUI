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
    /// This can include settings such as standard colours, fonts, and user interface element sizes.
    /// </summary>
    public static class SystemSettings
    {
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
            get => NativePlatform.Default.SystemSettingsAppearanceName();
        }

        /// <summary>
        /// Return true if the current system there is explicitly recognized as
        /// being a dark theme or if the default window background is dark.
        /// </summary>
        /// <remarks>
        /// This method should be used to check whether custom colours more appropriate
        /// for the default (light) or dark appearance should be used.
        /// </remarks>
        public static bool AppearanceIsDark
        {
            get => NativePlatform.Default.SystemSettingsAppearanceIsDark();
        }

        /// <summary>
        /// Returns true if the background is darker than foreground. This is used by
        /// <see cref="AppearanceIsDark"/> if there is no platform-specific way to determine
        /// whether a dark mode is being used and is generally not very useful to call directly.
        /// </summary>
        public static bool IsUsingDarkBackground
        {
            get => NativePlatform.Default.SystemSettingsIsUsingDarkBackground();
        }

        /// <summary>
        /// Returns the value of a system metric, or -1 if the metric is not supported on
        /// the current system.
        /// </summary>
        /// <param name="index">System metric identifier.</param>
        public static int GetMetric(SystemSettingsMetric index)
        {
            return NativePlatform.Default.SystemSettingsGetMetric(index);
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
        public static int GetMetric(SystemSettingsMetric index, IControl? control)
        {
            return NativePlatform.Default.SystemSettingsGetMetric(index, control);
        }

        /// <summary>
        /// Returns true if <paramref name="backgroundColor"/> is darker
        /// than <paramref name="foregroundColor"/>.
        /// </summary>
        public static bool IsDarkBackground(Color foregroundColor, Color backgroundColor)
        {
            // The threshold here is rather arbitrary, but it seems that using just
            // inequality would be wrong as it could result in false positivies.
            if (foregroundColor.IsOk && backgroundColor.IsOk)
                return foregroundColor.GetLuminance() - backgroundColor.GetLuminance() > 0.2;
            else
                return SystemSettings.IsUsingDarkBackground;
        }

        /// <summary>
        /// Logs all fonts enumerated in <see cref="SystemSettingsFont"/>.
        /// </summary>
        public static void LogSystemFonts()
        {
            BaseApplication.Log($"Default font: {Font.Default.ToInfoString()}");
            BaseApplication.Log($"Default mono font: {Font.DefaultMono.ToInfoString()}");

            var family = FontFamily.GenericMonospace;
            var fontGenericMonospace = new Font(family, 9);
            BaseApplication.Log($"GenericMonospace: {fontGenericMonospace.ToInfoString()}");

            var values = Enum.GetValues(typeof(SystemSettingsFont));
            foreach(var value in values)
            {
                var font = GetFont((SystemSettingsFont)value);

                BaseApplication.Log($"Font {value}: {font.ToInfoString()}");
            }
        }

        /// <summary>
        /// Logs all fonts with fixed widths (<see cref="Font.IsFixedWidth"/> property is true).
        /// </summary>
        public static void LogFixedWidthFonts()
        {
            BaseApplication.LogBeginSection();
            var items = FontFamily.FamiliesNamesAscending;
            foreach(var item in items)
            {
                var font = new Font(item, 10);

                if(font.IsFixedWidth && font.Style == FontStyle.Regular && !font.GdiVerticalFont)
                {
                    BaseApplication.Log(font.ToInfoString());
                }
            }

            BaseApplication.LogEndSection();
        }

        /// <summary>
        /// Returns <c>true</c> if the port has certain feature.
        /// </summary>
        /// <param name="index">System feature identifier.</param>
        public static bool HasFeature(SystemSettingsFeature index)
        {
            return NativePlatform.Default.SystemSettingsHasFeature(index);
        }

        /// <summary>
        /// Gets a standard system color.
        /// </summary>
        /// <param name="index">System color identifier.</param>
        public static Color GetColor(SystemSettingsColor index)
        {
            return NativePlatform.Default.SystemSettingsGetColor(index);
        }

        /// <summary>
        /// Gets a standard system font.
        /// </summary>
        /// <param name="systemFont">Font identifier.</param>
        public static Font GetFont(SystemSettingsFont systemFont)
        {
            var fnt = NativePlatform.Default.SystemSettingsGetFont(systemFont);
            return fnt;
        }
    }
}