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

        public static bool OnlySkiaFonts
        {
            get => true;
        }

        public static IFontFactoryHandler Handler
        {
            get => handler ??= GraphicsFactory.Handler.CreateFontFactoryHandler();

            set => handler = value;
        }

        public static string? GetSampleFixedPitchFont()
        {
            IEnumerable<string> fonts = GetFixedPitchFonts();
            var result = FontFactory.GetFixedPitchFont(fonts);

            if (result is not null)
                return result;

            return null;
        }

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
                        return new("Terminal", 7.5);
                    case SystemSettingsFont.AnsiFixed:
                        return new("Courier", 4.5);
                    case SystemSettingsFont.AnsiVar:
                        return new("MS Sans Serif", 4.5);
                    case SystemSettingsFont.System:
                        return new("System", 7.5);
                    case SystemSettingsFont.DeviceDefault:
                        return new("System", 7.5);
                    case SystemSettingsFont.DefaultGui:
                    default:
                        return new("Segoe UI", 9);
                }
            }
        }

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
