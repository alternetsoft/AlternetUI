using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides access to the font factory.
    /// </summary>
    public static class FontFactory
    {
        private static IFontFactoryHandler? handler;

        /// <summary>
        /// Gets whether only SkiaSharp compatible font are allowed.
        /// </summary>
        public static bool OnlySkiaFonts
        {
            get => true;
        }

        /// <summary>
        /// Gets or sets handler which is used to perform font related operations.
        /// </summary>
        public static IFontFactoryHandler Handler
        {
            get => handler ??= GraphicsFactory.Handler.CreateFontFactoryHandler();

            set => handler = value;
        }

        /// <summary>
        /// Gets sample fixed pitch font for the current operating system.
        /// </summary>
        /// <returns></returns>
        public static string? GetSampleFixedPitchFont()
        {
            IEnumerable<string> fonts = GetFixedPitchFonts();
            var result = FontFactory.GetFixedPitchFont(fonts);

            if (result is not null)
                return result;

            return null;
        }

        /// <summary>
        /// Gets sample <see cref="FontNameAndSize"/> for the specified <see cref="SystemSettingsFont"/>
        /// for the current operating system.
        /// </summary>
        /// <param name="font">Known system font.</param>
        /// <returns></returns>
        public static FontNameAndSize GetSampleNameAndSize(SystemSettingsFont font)
        {
            FontNameAndSize result;

            switch (App.BackendOS)
            {
                case OperatingSystems.Windows:
                    result = GetNameAndSizeWindows(font);
                    break;
                default:
                    result = Font.Default;
                    break;
            }

            return FontNameAndSize.SkiaOrDefault(result);

            static FontNameAndSize GetNameAndSizeWindows(SystemSettingsFont font)
            {
                switch (font)
                {
                    case SystemSettingsFont.OemFixed:
                        return new("Terminal", CoordD.Coord7And5);
                    case SystemSettingsFont.AnsiFixed:
                        return new("Courier", CoordD.Coord4And5);
                    case SystemSettingsFont.AnsiVar:
                        return new("MS Sans Serif", CoordD.Coord4And5);
                    case SystemSettingsFont.System:
                        return new("System", CoordD.Coord7And5);
                    case SystemSettingsFont.DeviceDefault:
                        return new("System", CoordD.Coord7And5);
                    case SystemSettingsFont.DefaultGui:
                    default:
                        return new("Segoe UI", 9);
                }
            }
        }

        /// <summary>
        /// Gets sample <see cref="FontNameAndSize"/> for the specified <see cref="GenericFontFamily"/>
        /// for the current operating system.
        /// </summary>
        /// <param name="family">Font family.</param>
        /// <returns></returns>
        public static FontNameAndSize GetSampleNameAndSize(GenericFontFamily family)
        {
            FontNameAndSize result;

            switch (App.BackendOS)
            {
                case OperatingSystems.Linux:
                    result = GetNameAndSizeLinux(family);
                    break;
                case OperatingSystems.IOS:
                case OperatingSystems.MacOs:
                    result = GetNameAndSizeMacOs(family);
                    break;
                case OperatingSystems.Windows:
                case OperatingSystems.Android:
                default:
                    result = GetNameAndSizeWindows(family);
                    break;
            }

            return FontNameAndSize.SkiaOrDefault(result);

            static FontNameAndSize GetNameAndSizeWindows(GenericFontFamily family)
            {
                switch (family)
                {
                    case GenericFontFamily.Default:
                    default:
                    case GenericFontFamily.None:
                        return new("Segoe UI", 9);
                    case GenericFontFamily.SansSerif:
                        return new("Arial", 9);
                    case GenericFontFamily.Serif:
                        return new("Times New Roman", 9);
                    case GenericFontFamily.Monospace:
                        return new("Courier New", 9);
                }
            }

            static FontNameAndSize GetNameAndSizeLinux(GenericFontFamily family)
            {
                switch (family)
                {
                    case GenericFontFamily.Default:
                    default:
                    case GenericFontFamily.None:
                        return new("Ubuntu", 9);
                    case GenericFontFamily.SansSerif:
                        return new("Sans", 9);
                    case GenericFontFamily.Serif:
                        return new("Serif", 9);
                    case GenericFontFamily.Monospace:
                        return new("Monospace", 9);
                }
            }

            static FontNameAndSize GetNameAndSizeMacOs(GenericFontFamily family)
            {
                switch (family)
                {
                    case GenericFontFamily.Default:
                    default:
                    case GenericFontFamily.None:
                        return new("Lucida Grande", 9);
                    case GenericFontFamily.SansSerif:
                        return new("Helvetica", 9);
                    case GenericFontFamily.Serif:
                        return new("Times New Roman", 9);
                    case GenericFontFamily.Monospace:
                        return new("Courier New", 9);
                }
            }
        }

        /// <summary>
        /// Gets collection of the fixed pitch fonts which supposed to be supported
        /// by the current operating system.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetFixedPitchFonts()
        {
            IEnumerable<string> result;

            switch (App.BackendOS)
            {
                case OperatingSystems.Linux:
                    result = GetFixedPitchFontsLinux();
                    break;
                case OperatingSystems.Windows:
                    result = GetFixedPitchFontsWindows();
                    break;
                case OperatingSystems.Android:
                    result = GetFixedPitchFontsAndroid();
                    break;
                case OperatingSystems.MacOs:
                case OperatingSystems.IOS:
                    result = GetFixedPitchFontsMacOs();
                    break;
                default:
                    result = new string[] { "Courier New" };
                    break;
            }

            result = FontFamily.RemoveNonSkiaFonts(result);
            return result;

            static string[] GetFixedPitchFontsAndroid()
            {
                return new string[]
                {
                    "monospace",
                    "serif-monospace",
                };
            }

            static string[] GetFixedPitchFontsWindows()
            {
                return new string[]
                {
                "Cascadia Mono",
                "Consolas",
                "Courier New",
                "Lucida Console",
                };
            }

            static string[] GetFixedPitchFontsMacOs()
            {
                return new string[]
                {
                "Monaco",
                "Menlo",
                "Andale Mono",
                "Courier New",
                };
            }

            static string[] GetFixedPitchFontsLinux()
            {
                return new string[]
                {
                "Ubuntu Mono",
                "Monospace",
                "Courier New",
                "Courier",
                };
            }
        }

        /// <summary>
        /// Gets first fixed pitch font in the collection of the fonts.
        /// </summary>
        /// <param name="fonts">Collection of the fonts.</param>
        /// <returns></returns>
        public static string? GetFixedPitchFont(IEnumerable<string> fonts)
        {
            foreach (var font in fonts)
            {
                if (FontFamily.IsFixedPitchFontFamily(font))
                    return font;
            }

            return null;
        }
    }
}
