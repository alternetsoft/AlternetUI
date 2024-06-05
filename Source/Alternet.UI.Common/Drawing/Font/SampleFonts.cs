using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public static class SampleFonts
    {
        public static string? GetFixedPitchFont()
        {
            IEnumerable<string> fonts = GetFixedPitchFonts();
            var result = GetFixedPitchFont(fonts);

            if (result is not null)
                return result;

            fonts = FontFamily.FamiliesNames;
            result = GetFixedPitchFont(fonts);
            if (result is not null)
                return result;

            return null;
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

        public static IEnumerable<string> GetFixedPitchFonts()
        {
            IEnumerable<string> result;

            switch (App.BackendOS)
            {
                case OperatingSystems.Linux:
                    result = GetFixedPitchFontsLinux();
                    break;
                case OperatingSystems.Windows:
                case OperatingSystems.MacOs:
                case OperatingSystems.Android:
                case OperatingSystems.IOS:
                default:
                    result = GetFixedPitchFontsWindows();
                    break;
            }

            result = FontFamily.RemoveNonSkiaFonts(result);
            return result;
        }

        public static FontNameAndSize GetNameAndSize(SystemSettingsFont font)
        {
            FontNameAndSize result;

            switch (App.BackendOS)
            {
                case OperatingSystems.Linux:
                    result = GetNameAndSizeLinux(font);
                    break;
                case OperatingSystems.Windows:
                case OperatingSystems.MacOs:
                case OperatingSystems.Android:
                case OperatingSystems.IOS:
                default:
                    result = GetNameAndSizeWindows(font);
                    break;
            }

            return FontNameAndSize.SkiaOrDefault(result);
        }

        public static FontNameAndSize GetNameAndSize(GenericFontFamily family)
        {
            FontNameAndSize result;

            switch (App.BackendOS)
            {
                case OperatingSystems.Linux:
                    result = GetNameAndSizeLinux(family);
                    break;
                case OperatingSystems.Windows:
                case OperatingSystems.MacOs:
                case OperatingSystems.Android:
                case OperatingSystems.IOS:
                default:
                    result = GetNameAndSizeWindows(family);
                    break;
            }

            return FontNameAndSize.SkiaOrDefault(result);
        }

        internal static FontNameAndSize GetNameAndSizeWindows(GenericFontFamily family)
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

        internal static string[] GetFixedPitchFontsWindows()
        {
            return new string[]
            {
                "Cascadia Mono",
                "Consolas",
                "Courier New",
                "Lucida Console",
                "Lucida Sans Typewriter",
                "MS Gothic",
                "Cascadia Code",
                "Fira Code",
                "Fira Code Retina",
                "Hack",
                "JetBrains Mono",
                "JetBrains Mono NL",
                "NSimSun",
                "SimSun",
                "Source Code Pro",
            };
        }

        internal static string[] GetFixedPitchFontsLinux()
        {
            return new string[]
            {
                "Ubuntu Mono",
                "Monospace",
                "DejaVu Sans Mono",
                "Liberation Mono",
                "Nimbus Mono PS",
                "Noto Mono",
                "Noto Sans SignWriting",
            };
        }

        internal static FontNameAndSize GetNameAndSizeLinux(GenericFontFamily family)
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

        internal static FontNameAndSize GetNameAndSizeLinux(SystemSettingsFont font)
        {
            return new("Ubuntu", 9);
        }

        internal static FontNameAndSize GetNameAndSizeWindows(SystemSettingsFont font)
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
}
