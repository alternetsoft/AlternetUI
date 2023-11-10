using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Gets standard system fonts.
    /// </summary>
    public class SystemFonts
    {
        private static Font? oemFixed;
        private static Font? ansiFixed;
        private static Font? ansiVar;
        private static Font? system;
        private static Font? deviceDefault;

        /// <summary>
        /// Original equipment manufacturer dependent fixed-pitch font.
        /// </summary>
        public static Font OemFixed =>
            oemFixed ??= SystemSettings.GetFont(SystemSettingsFont.OemFixed);

        /// <summary>
        /// Windows fixed-pitch (monospaced) font.
        /// </summary>
        public static Font AnsiFixed =>
            ansiFixed ??= SystemSettings.GetFont(SystemSettingsFont.AnsiFixed);

        /// <summary>
        /// Windows variable-pitch (proportional) font.
        /// </summary>
        public static Font AnsiVar =>
            ansiVar ??= SystemSettings.GetFont(SystemSettingsFont.AnsiVar);

        /// <summary>
        /// System font. By default, the system uses the system font to draw menus,
        /// dialog box controls, and text.
        /// </summary>
        public static Font System =>
            system ??= SystemSettings.GetFont(SystemSettingsFont.System);

        /// <summary>
        /// Device-dependent font.
        /// </summary>
        public static Font DeviceDefault =>
            deviceDefault ??= SystemSettings.GetFont(SystemSettingsFont.DeviceDefault);

        /// <summary>
        /// Default font for user interface objects such as menus and dialog boxes.
        /// </summary>
        /// <remarks>
        /// Note that with modern GUIs nothing guarantees that the same font is used for
        /// all GUI elements, so some controls might use a different font by default.
        /// </remarks>
        public static Font DefaultGui => Font.Default;
    }
}
