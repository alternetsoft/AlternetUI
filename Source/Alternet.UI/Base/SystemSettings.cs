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
            get => Native.WxOtherFactory.SystemAppearanceGetName();
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
            get => Native.WxOtherFactory.SystemAppearanceIsDark();
        }

        /// <summary>
        /// Return true if the background is darker than foreground. This is used by
        /// <see cref="AppearanceIsDark"/> if there is no platform-specific way to determine
        /// whether a dark mode is being used and is generally not very useful to call directly.
        /// </summary>
        public static bool IsUsingDarkBackground
        {
            get => Native.WxOtherFactory.SystemAppearanceIsUsingDarkBackground();
        }

        /// <summary>
        /// Returns <c>true</c> if the port has certain feature.
        /// </summary>
        /// <param name="index">System feature identifier.</param>
        public static bool HasFeature(SystemSettingsFeature index)
        {
            return Native.WxOtherFactory.SystemSettingsHasFeature((int)index);
        }

        /// <summary>
        /// Gets a standard system color.
        /// </summary>
        /// <param name="index">System color identifier.</param>
        public static Color GetColor(SystemSettingsColor index)
        {
            return Native.WxOtherFactory.SystemSettingsGetColor((int)index);
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
        public static int GetMetric(SystemSettingsMetric index, Control? control = default)
        {
            IntPtr wx = control?.WxWidget ?? default;
            return Native.WxOtherFactory.SystemSettingsGetMetric((int)index, wx);
        }
    }
}