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
        /// <summary>
        /// Gets known svg colors for the dark scheme.
        /// </summary>
        public static SvgColors DarkScheme { get; } = new SvgColorsDark();

        /// <summary>
        /// Gets known svg colors for the white scheme.
        /// </summary>
        public static SvgColors WhiteScheme { get; } = new SvgColorsWhite();

        /// <summary>
        /// Normal svg color.
        /// </summary>
        public Color Normal { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Disabled svg color.
        /// </summary>
        public Color Disabled { get; set; } = SystemColors.GrayText;

        /// <summary>
        /// Error svg color.
        /// </summary>
        public Color Error { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Information svg color.
        /// </summary>
        public Color Information { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Warning svg color.
        /// </summary>
        public Color Warning { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Gets known svg color for the specified <see cref="KnownSvgColor"/>.
        /// </summary>
        /// <param name="knownSvgColor">Known Svg color identifier.</param>
        /// <param name="isDark">Chooses <see cref="DarkScheme"/>
        /// or <see cref="WhiteScheme"/>.</param>
        public static Color GetSvgColor(KnownSvgColor knownSvgColor, bool isDark)
        {
            if (isDark)
                return DarkScheme.GetSvgColor(knownSvgColor);
            else
                return WhiteScheme.GetSvgColor(knownSvgColor);
        }

        /// <summary>
        /// Gets known svg color for the specified <see cref="KnownSvgColor"/>.
        /// </summary>
        /// <param name="knownSvgColor">Known Svg color identifier.</param>
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

        private class SvgColorsDark : SvgColors
        {
            public SvgColorsDark()
            {
                Normal = Color.White;
                Disabled = SystemColors.GrayText;
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
                Disabled = SystemColors.GrayText;
                Error = Normal;
                Information = Normal;
                Warning = Normal;
            }
        }
    }
}
