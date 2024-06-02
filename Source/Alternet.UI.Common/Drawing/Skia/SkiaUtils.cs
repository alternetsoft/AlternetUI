using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.UI;
using Alternet.UI.Extensions;

namespace Alternet.Drawing
{
    public static class SkiaUtils
    {
        private static FontSize defaultFontSize = 12;
        private static string? defaultFontName;
        private static string? defaultMonoFontName;
        private static SKFont? defaultSkiaFont;

        public static SKFont DefaultFont
        {
            get => defaultSkiaFont ??= CreateDefaultFont();
            set => defaultSkiaFont = value ?? CreateDefaultFont();
        }
        
        public static double DefaultFontSize
        {
            get => defaultFontSize;

            set
            {
                defaultFontSize = value;
            }
        }

        public static string DefaultFontName
        {
            get => defaultFontName ?? SKTypeface.Default.FamilyName;
            set => defaultFontName = value;
        }

        public static string DefaultMonoFontName
        {
            get => defaultMonoFontName ?? SKTypeface.Default.FamilyName;
            set => defaultMonoFontName = value;
        }

        public static (string Name, FontSize Size) GetFontNameAndSize(SystemSettingsFont systemFont)
        {
            ///FontFamily.GetSampleFontNameAndSizeWindows(SystemSettingsFont font)

            switch (systemFont)
            {
                case SystemSettingsFont.OemFixed:
                case SystemSettingsFont.AnsiFixed:
                    return (DefaultMonoFontName, DefaultFontSize);
                case SystemSettingsFont.AnsiVar:
                case SystemSettingsFont.System:
                case SystemSettingsFont.DeviceDefault:
                case SystemSettingsFont.DefaultGui:
                default:
                    return (DefaultFontName, DefaultFontSize);
            }
        }

        public static string[] GetFontFamiliesNames()
        {
            return SKFontManager.Default.GetFontFamilies();
        }

        public static string GetFontFamilyName(GenericFontFamily genericFamily)
        {
            if (genericFamily == GenericFontFamily.Default)
                return SkiaUtils.DefaultFontName;

            var (name, _) = FontFamily.GetSampleFontNameAndSize(genericFamily);

            if (!FontFamily.IsFamilyValid(name))
                name = SkiaUtils.DefaultFontName;

            return name;
        }

        public static SKFont CreateDefaultFont()
        {
            return new SKFont(SKTypeface.Default, (float)DefaultFontSize);
        }
    }
}
