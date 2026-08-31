using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains static members related to colors which have different
    /// argb in dark and light themes.
    /// </summary>
    public static class LightDarkColors
    {
        private static LightDarkColor? yellow;
        private static LightDarkColor? red;
        private static LightDarkColor? green;
        private static LightDarkColor? blue;
        private static LightDarkColor? blueDarker;
        private static LightDarkColor? blueLighter;

        /// <summary>
        /// Gets default red colors pair.
        /// </summary>
        public static LightDarkColor Red
        {
            get
            {
                return red ??= Color.LightDark(light: (192, 10, 22), dark: (244, 75, 86));
            }

            set => red = value;
        }

        /// <summary>
        /// Gets default yellow colors pair.
        /// </summary>
        public static LightDarkColor Yellow
        {
            get
            {
                return yellow ??= Color.LightDark(light: (239, 184, 57), dark: (239, 184, 57));
            }

            set => yellow = value;
        }

        /// <summary>
        /// Gets default green colors pair.
        /// </summary>
        public static LightDarkColor Green
        {
            get
            {
                return green ??= Color.LightDark(light: (30, 124, 30), dark: (138, 226, 138));
            }

            set => green = value;
        }

        /// <summary>
        /// Gets default blue colors pair.
        /// </summary>
        public static LightDarkColor Blue
        {
            get
            {
                return blue ??= Color.LightDark(light: (0, 90, 181), dark: (85, 170, 255));
            }

            set => blue = value;
        }

        /// <summary>
        /// Gets default blue darker colors pair.
        /// </summary>
        public static LightDarkColor BlueDarker
        {
            get
            {
                return blueDarker ??= new(Blue.Light.Darker(), Blue.Dark.Darker());
            }

            set => blueDarker = value;
        }

        /// <summary>
        /// Gets default blue lighter colors pair.
        /// </summary>
        public static LightDarkColor BlueLighter
        {
            get
            {
                return blueLighter ??= new(Blue.Light.Lighter(), Blue.Dark.Lighter());
            }

            set => blueLighter = value;
        }

        internal static LightDarkColor White { get; } = new (Color.White);

        /// <summary>
        /// Contains background colors which look good with light text.
        /// </summary>
        public static class LightTextBackgrounds
        {
            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Black => Color.Black;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Blue => Color.Blue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color BlueViolet => Color.BlueViolet;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Brown => Color.Brown;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color CadetBlue => Color.CadetBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Chocolate => Color.Chocolate;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color CornflowerBlue => Color.CornflowerBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Crimson => Color.Crimson;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkBlue => Color.DarkBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkCyan => Color.DarkCyan;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkGoldenrod => Color.DarkGoldenrod;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkGreen => Color.DarkGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkMagenta => Color.DarkMagenta;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkOliveGreen => Color.DarkOliveGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkOrchid => Color.DarkOrchid;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkRed => Color.DarkRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkSlateBlue => Color.DarkSlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkSlateGray => Color.DarkSlateGray;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DarkViolet => Color.DarkViolet;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color DimGray => Color.DimGray;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Firebrick => Color.Firebrick;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color ForestGreen => Color.ForestGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Green => Color.Green;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color IndianRed => Color.IndianRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Indigo => Color.Indigo;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Maroon => Color.Maroon;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumBlue => Color.MediumBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumOrchid => Color.MediumOrchid;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumPurple => Color.MediumPurple;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumSlateBlue => Color.MediumSlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MediumVioletRed => Color.MediumVioletRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color MidnightBlue => Color.MidnightBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Olive => Color.Olive;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color OrangeRed => Color.OrangeRed;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color RebeccaPurple => Color.RebeccaPurple;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color RoyalBlue => Color.RoyalBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SaddleBrown => Color.SaddleBrown;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SeaGreen => Color.SeaGreen;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Sienna => Color.Sienna;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SlateBlue => Color.SlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SlateGray => Color.SlateGray;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color SteelBlue => Color.SteelBlue;

            /// <summary>
            /// Gets a background color which looks good with light text.
            /// </summary>
            public static Color Teal => Color.Teal;
        }

        /// <summary>
        /// Contains background colors which look good with dark text.
        /// </summary>
        public static class DarkTextBackgrounds
        {
            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color AntiqueWhite => Color.AntiqueWhite;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Aquamarine => Color.Aquamarine;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Bisque => Color.Bisque;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Gold => Color.Gold;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Khaki => Color.Khaki;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Lavender => Color.Lavender;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightBlue => Color.LightBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightCyan => Color.LightCyan;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightGray => Color.LightGray;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightGreen => Color.LightGreen;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightPink => Color.LightPink;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightSalmon => Color.LightSalmon;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightSkyBlue => Color.LightSkyBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color LightYellow => Color.LightYellow;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Linen => Color.Linen;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color MintCream => Color.MintCream;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color MistyRose => Color.MistyRose;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Moccasin => Color.Moccasin;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color NavajoWhite => Color.NavajoWhite;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color OldLace => Color.OldLace;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PaleGoldenrod => Color.PaleGoldenrod;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PaleGreen => Color.PaleGreen;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PaleTurquoise => Color.PaleTurquoise;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PapayaWhip => Color.PapayaWhip;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PeachPuff => Color.PeachPuff;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Pink => Color.Pink;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color PowderBlue => Color.PowderBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color SeaShell => Color.SeaShell;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color SkyBlue => Color.SkyBlue;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Snow => Color.Snow;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Wheat => Color.Wheat;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color WhiteSmoke => Color.WhiteSmoke;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color Yellow => Color.Yellow;

            /// <summary>
            /// Gets a background color which looks good with dark text.
            /// </summary>
            public static Color YellowGreen => Color.YellowGreen;
        }

        /// <summary>
        /// Contains background colors which look good with light and dark text.
        /// </summary>
        public static class LightDarkTextBackgrounds
        {
            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color Chocolate => Color.Chocolate;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color DarkCyan => Color.DarkCyan;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color DarkGoldenrod => Color.DarkGoldenrod;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color DarkGreen => Color.DarkGreen;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color Olive => Color.Olive;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color RoyalBlue => Color.RoyalBlue;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color SlateBlue => Color.SlateBlue;

            /// <summary>
            /// Gets a background color which looks good with light and dark text.
            /// </summary>
            public static Color Teal => Color.Teal;
        }

    }
}