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
        private static Font? sansSerif;
        private static Font? serif;
        private static Font? menu;

        /// <summary>
        /// Original equipment manufacturer dependent fixed-pitch font.
        /// </summary>
        public static Font OemFixed
        {
            get => oemFixed ??= CreateSystemFont(SystemSettingsFont.OemFixed);
            set => oemFixed = value ?? CreateSystemFont(SystemSettingsFont.OemFixed);
        }

        /// <summary>
        /// Windows fixed-pitch (monospaced) font.
        /// </summary>
        public static Font AnsiFixed
        {
            get => ansiFixed ??= CreateSystemFont(SystemSettingsFont.AnsiFixed);
            set => ansiFixed = value ?? CreateSystemFont(SystemSettingsFont.AnsiFixed);
        }

        /// <summary>
        /// Windows variable-pitch (proportional) font.
        /// </summary>
        public static Font AnsiVar
        {
            get => ansiVar ??= CreateSystemFont(SystemSettingsFont.AnsiVar);
            set => ansiVar = value ?? CreateSystemFont(SystemSettingsFont.AnsiVar);
        }

        /// <summary>
        /// System font. By default, the system uses the system font to draw menus,
        /// dialog box controls, and text.
        /// </summary>
        public static Font System
        {
            get => system ??= CreateSystemFont(SystemSettingsFont.System);
            set => system = value ?? CreateSystemFont(SystemSettingsFont.System);
        }

        /// <summary>
        /// Device-dependent font.
        /// </summary>
        public static Font DeviceDefault
        {
            get => deviceDefault ??= CreateSystemFont(SystemSettingsFont.DeviceDefault);
            set => deviceDefault = value ?? CreateSystemFont(SystemSettingsFont.DeviceDefault);
        }

        /// <summary>
        /// Default font for user interface objects such as menus and dialog boxes.
        /// This is the same as <see cref="Font.Default"/>.
        /// </summary>
        /// <remarks>
        /// Note that with modern GUIs nothing guarantees that the same font is used for
        /// all GUI elements, so some controls might use a different font by default.
        /// </remarks>
        public static Font DefaultGui
        {
            get => Font.Default;
            set => Font.Default = value;
        }

        /// <summary>
        /// Gets or sets menu font.
        /// This property added for the compatibility with legacy code.
        /// </summary>
        public static Font MenuFont
        {
            get
            {
                return menu ?? Control.DefaultFont;
            }

            set
            {
                menu = value;
            }
        }

        /// <summary>
        /// Alias to <see cref="Font.Default"/>.
        /// </summary>
        public static Font Default
        {
            get => Font.Default;
            set => Font.Default = value;
        }

        /// <summary>
        /// Alias to <see cref="Font.DefaultMono"/>.
        /// </summary>
        public static Font DefaultMono
        {
            get => Font.DefaultMono;
            set => Font.DefaultMono = value;
        }

        /// <summary>
        /// Gets generic "Serif" font.
        /// </summary>
        public static Font Serif
        {
            get => serif ??= new Font(FontFamily.GenericSerif, Default.SizeInPoints);
            set => serif = value ?? new Font(FontFamily.GenericSerif, Default.SizeInPoints);
        }

        /// <summary>
        /// Gets generic "Sans serif" font.
        /// </summary>
        public static Font SansSerif
        {
            get => sansSerif ??= new Font(FontFamily.GenericSansSerif, Default.SizeInPoints);
            set => sansSerif = value ?? new Font(FontFamily.GenericSansSerif, Default.SizeInPoints);
        }

        /// <summary>
        /// Gets system font.
        /// </summary>
        public static Font GetFont(SystemSettingsFont font)
        {
            switch (font)
            {
                case SystemSettingsFont.OemFixed:
                    return OemFixed;
                case SystemSettingsFont.AnsiFixed:
                    return AnsiFixed;
                case SystemSettingsFont.AnsiVar:
                    return AnsiVar;
                case SystemSettingsFont.System:
                    return System;
                case SystemSettingsFont.DeviceDefault:
                    return DeviceDefault;
                case SystemSettingsFont.DefaultGui:
                default:
                    return DefaultGui;
            }
        }

        /// <summary>
        /// Sets system font.
        /// </summary>
        public static void SetFont(SystemSettingsFont font, Font value)
        {
            switch (font)
            {
                case SystemSettingsFont.OemFixed:
                    OemFixed = value;
                    break;
                case SystemSettingsFont.AnsiFixed:
                    AnsiFixed = value;
                    break;
                case SystemSettingsFont.AnsiVar:
                    AnsiVar = value;
                    break;
                case SystemSettingsFont.System:
                    System = value;
                    break;
                case SystemSettingsFont.DeviceDefault:
                    DeviceDefault = value;
                    break;
                case SystemSettingsFont.DefaultGui:
                    DefaultGui = value;
                    break;
            }
        }

        /// <summary>
        /// Gets font for the specified <see cref="GenericFontFamily"/>.
        /// </summary>
        public static void SetFont(GenericFontFamily font, Font value)
        {
            switch (font)
            {
                case GenericFontFamily.Default:
                    Font.Default = value;
                    break;
                case GenericFontFamily.SansSerif:
                    SansSerif = value;
                    break;
                case GenericFontFamily.Serif:
                    Serif = value;
                    break;
                case GenericFontFamily.Monospace:
                    Font.DefaultMono = value;
                    break;
            }
        }

        /// <summary>
        /// Gets font for the specified <see cref="GenericFontFamily"/>.
        /// </summary>
        public static Font GetFont(GenericFontFamily font)
        {
            switch (font)
            {
                case GenericFontFamily.Default:
                case GenericFontFamily.None:
                default:
                    return Font.Default;
                case GenericFontFamily.SansSerif:
                    return SansSerif;
                case GenericFontFamily.Serif:
                    return Serif;
                case GenericFontFamily.Monospace:
                    return Font.DefaultMono;
            }
        }

        /// <summary>
        /// Creates system font.
        /// </summary>
        /// <param name="font">Specifies system font to create.</param>
        /// <returns></returns>
        public static Font CreateSystemFont(SystemSettingsFont font)
        {
            var result = FontFactory.Handler.CreateSystemFont(font);

            if (result is not null)
                return result;

            var nameAndSize = FontFactory.GetSampleNameAndSize(font);

            if (FontFamily.IsFamilyValid(nameAndSize.Name))
                return new(nameAndSize);

            return new Font(Font.Default);
        }
    }
}
