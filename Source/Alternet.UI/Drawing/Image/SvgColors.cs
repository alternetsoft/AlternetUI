using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains declarations of the known svg colors.
    /// </summary>
    public class SvgColors
    {
        public static SvgColors BlackScheme { get; } = new SvgColorsBlack();

        public static SvgColors WhiteScheme { get; } = new SvgColorsWhite();

        public Color Normal { get; set; }

        public Color Disabled { get; set; }

        public Color Error { get; set; }

        public Color Information { get; set; }

        public Color Warning { get; set; }

        public Color GetSvgColor(KnownSvgColor knownSvgColor)
        {
            switch (knownSvgColor)
            {
                case KnownSvgColor.Normal:
                default:
                    return Normal;
                case KnownSvgColor.Disabled:
                    return Disabled;
                case KnownSvgColor.Error:
                    return Error;
                case KnownSvgColor.Information:
                    return Information;
                case KnownSvgColor.Warning:
                    return Warning;
            }
        }

        public static Color GetSvgColor(KnownSvgColor knownSvgColor, bool isDark)
        {
            if (isDark)
                return BlackScheme.GetSvgColor(knownSvgColor);
            else
                return WhiteScheme.GetSvgColor(knownSvgColor);
        }

        private class SvgColorsBlack : SvgColors
        {
            public SvgColorsBlack()
            {
                Normal = Color.White;
                Disabled = Normal;
                Error = Normal;
                Information = Normal;
                Warning = Normal;
            }
        }

        private class SvgColorsWhite : SvgColors
        {
            public SvgColorsWhite()
            {
                Normal = Color.Black;
                Disabled = Normal;
                Error = Normal;
                Information = Normal;
                Warning = Normal;
            }
        }
    }
}
