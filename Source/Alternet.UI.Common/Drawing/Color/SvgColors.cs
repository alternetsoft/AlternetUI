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
        public virtual Color Normal { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Disabled svg color.
        /// </summary>
        public virtual Color Disabled { get; set; } = SystemColors.GrayText;

        /// <summary>
        /// Error svg color.
        /// </summary>
        public virtual Color Error { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Information svg color.
        /// </summary>
        public virtual Color Information { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Warning svg color.
        /// </summary>
        public virtual Color Warning { get; set; } = SystemColors.WindowText;

        /// <summary>
        /// Highlight text svg color.
        /// </summary>
        public virtual Color HighlightText { get; set; } = SystemColors.HighlightText;

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

        private abstract class SvgColorOverDefault : SvgColors
        {
            private Color? normal;
            private Color? disabled;
            private Color? error;
            private Color? information;
            private Color? warning;

            public abstract bool IsDark { get; }

            public override Color Normal
            {
                get => normal ?? DefaultColors.SvgNormalColor;
                set => normal = value;
            }

            public override Color Disabled
            {
                get => disabled ?? DefaultColors.SvgDisabledColor;
                set => disabled = value;
            }

            public override Color Error
            {
                get => error ?? Normal;
                set => error = value;
            }

            public override Color Information
            {
                get => information ?? Normal;
                set => information = value;
            }

            public override Color Warning
            {
                get => warning ?? Normal;
                set => warning = value;
            }
        }

        private class SvgColorsDark : SvgColorOverDefault
        {
            public SvgColorsDark()
            {
            }

            public override bool IsDark => true;
        }

        private class SvgColorsWhite : SvgColorOverDefault
        {
            public SvgColorsWhite()
            {
            }

            public override bool IsDark => false;
        }
    }
}
