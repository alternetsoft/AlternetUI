// This code is based on the .NET implementation.
// We thank the .NET team for making it available under the MIT license.
// We have modified this code to fit our needs.

using System.Diagnostics;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Allows to convert colors to ARGB.
    /// </summary>
    public static class KnownColorTable
    {
        /// <summary>
        /// <see cref="KnownColor"/> is system color.
        /// </summary>
        public const byte KnownColorKindSystem = 0;

        /// <summary>
        /// <see cref="KnownColor"/> is web color.
        /// </summary>
        public const byte KnownColorKindWeb = 1;

        /// <summary>
        /// <see cref="KnownColor"/> is unknown color.
        /// </summary>
        public const byte KnownColorKindUnknown = 2;

        /// <summary>
        /// All known color values (in order of definition in the <see cref="KnownColor"/>).
        /// </summary>
        public static readonly ColorStruct[] ColorValueTable =
        {
            /*"not a known color"*/
            0,

            /*"System" colors, Part 1*/
            /* Hard-coded constants, based on default Windows settings.*/
            0xFFD4D0C8,     // ActiveBorder
            0xFF0054E3,     // ActiveCaption
            0xFFFFFFFF,     // ActiveCaptionText
            0xFF808080,     // AppWorkspace
            0xFFECE9D8,     // Control
            0xFFACA899,     // ControlDark
            0xFF716F64,     // ControlDarkDark
            0xFFF1EFE2,     // ControlLight
            0xFFFFFFFF,     // ControlLightLight
            0xFF000000,     // ControlText
            0xFF004E98,     // Desktop
            0xFFACA899,     // GrayText
            0xFF316AC5,     // Highlight
            0xFFFFFFFF,     // HighlightText
            0xFF000080,     // HotTrack
            0xFFD4D0C8,     // InactiveBorder
            0xFF7A96DF,     // InactiveCaption
            0xFFD8E4F8,     // InactiveCaptionText
            0xFFFFFFE1,     // Info
            0xFF000000,     // InfoText
            0xFFFFFFFF,     // Menu
            0xFF000000,     // MenuText
            0xFFD4D0C8,     // ScrollBar
            0xFFFFFFFF,     // Window
            0xFF000000,     // WindowFrame
            0xFF000000,     // WindowText

            // "Web" Colors, Part 1
            0x00FFFFFF,     // Transparent
            0xFFF0F8FF,     // AliceBlue
            0xFFFAEBD7,     // AntiqueWhite
            0xFF00FFFF,     // Aqua
            0xFF7FFFD4,     // Aquamarine
            0xFFF0FFFF,     // Azure
            0xFFF5F5DC,     // Beige
            0xFFFFE4C4,     // Bisque
            0xFF000000,     // Black
            0xFFFFEBCD,     // BlanchedAlmond
            0xFF0000FF,     // Blue
            0xFF8A2BE2,     // BlueViolet
            0xFFA52A2A,     // Brown
            0xFFDEB887,     // BurlyWood
            0xFF5F9EA0,     // CadetBlue
            0xFF7FFF00,     // Chartreuse
            0xFFD2691E,     // Chocolate
            0xFFFF7F50,     // Coral
            0xFF6495ED,     // CornflowerBlue
            0xFFFFF8DC,     // Cornsilk
            0xFFDC143C,     // Crimson
            0xFF00FFFF,     // Cyan
            0xFF00008B,     // DarkBlue
            0xFF008B8B,     // DarkCyan
            0xFFB8860B,     // DarkGoldenrod
            0xFFA9A9A9,     // DarkGray
            0xFF006400,     // DarkGreen
            0xFFBDB76B,     // DarkKhaki
            0xFF8B008B,     // DarkMagenta
            0xFF556B2F,     // DarkOliveGreen
            0xFFFF8C00,     // DarkOrange
            0xFF9932CC,     // DarkOrchid
            0xFF8B0000,     // DarkRed
            0xFFE9967A,     // DarkSalmon
            0xFF8FBC8F,     // DarkSeaGreen
            0xFF483D8B,     // DarkSlateBlue
            0xFF2F4F4F,     // DarkSlateGray
            0xFF00CED1,     // DarkTurquoise
            0xFF9400D3,     // DarkViolet
            0xFFFF1493,     // DeepPink
            0xFF00BFFF,     // DeepSkyBlue
            0xFF696969,     // DimGray
            0xFF1E90FF,     // DodgerBlue
            0xFFB22222,     // Firebrick
            0xFFFFFAF0,     // FloralWhite
            0xFF228B22,     // ForestGreen
            0xFFFF00FF,     // Fuchsia
            0xFFDCDCDC,     // Gainsboro
            0xFFF8F8FF,     // GhostWhite
            0xFFFFD700,     // Gold
            0xFFDAA520,     // Goldenrod
            0xFF808080,     // Gray
            0xFF008000,     // Green
            0xFFADFF2F,     // GreenYellow
            0xFFF0FFF0,     // Honeydew
            0xFFFF69B4,     // HotPink
            0xFFCD5C5C,     // IndianRed
            0xFF4B0082,     // Indigo
            0xFFFFFFF0,     // Ivory
            0xFFF0E68C,     // Khaki
            0xFFE6E6FA,     // Lavender
            0xFFFFF0F5,     // LavenderBlush
            0xFF7CFC00,     // LawnGreen
            0xFFFFFACD,     // LemonChiffon
            0xFFADD8E6,     // LightBlue
            0xFFF08080,     // LightCoral
            0xFFE0FFFF,     // LightCyan
            0xFFFAFAD2,     // LightGoldenrodYellow
            0xFFD3D3D3,     // LightGray
            0xFF90EE90,     // LightGreen
            0xFFFFB6C1,     // LightPink
            0xFFFFA07A,     // LightSalmon
            0xFF20B2AA,     // LightSeaGreen
            0xFF87CEFA,     // LightSkyBlue
            0xFF778899,     // LightSlateGray
            0xFFB0C4DE,     // LightSteelBlue
            0xFFFFFFE0,     // LightYellow
            0xFF00FF00,     // Lime
            0xFF32CD32,     // LimeGreen
            0xFFFAF0E6,     // Linen
            0xFFFF00FF,     // Magenta
            0xFF800000,     // Maroon
            0xFF66CDAA,     // MediumAquamarine
            0xFF0000CD,     // MediumBlue
            0xFFBA55D3,     // MediumOrchid
            0xFF9370DB,     // MediumPurple
            0xFF3CB371,     // MediumSeaGreen
            0xFF7B68EE,     // MediumSlateBlue
            0xFF00FA9A,     // MediumSpringGreen
            0xFF48D1CC,     // MediumTurquoise
            0xFFC71585,     // MediumVioletRed
            0xFF191970,     // MidnightBlue
            0xFFF5FFFA,     // MintCream
            0xFFFFE4E1,     // MistyRose
            0xFFFFE4B5,     // Moccasin
            0xFFFFDEAD,     // NavajoWhite
            0xFF000080,     // Navy
            0xFFFDF5E6,     // OldLace
            0xFF808000,     // Olive
            0xFF6B8E23,     // OliveDrab
            0xFFFFA500,     // Orange
            0xFFFF4500,     // OrangeRed
            0xFFDA70D6,     // Orchid
            0xFFEEE8AA,     // PaleGoldenrod
            0xFF98FB98,     // PaleGreen
            0xFFAFEEEE,     // PaleTurquoise
            0xFFDB7093,     // PaleVioletRed
            0xFFFFEFD5,     // PapayaWhip
            0xFFFFDAB9,     // PeachPuff
            0xFFCD853F,     // Peru
            0xFFFFC0CB,     // Pink
            0xFFDDA0DD,     // Plum
            0xFFB0E0E6,     // PowderBlue
            0xFF800080,     // Purple
            0xFFFF0000,     // Red
            0xFFBC8F8F,     // RosyBrown
            0xFF4169E1,     // RoyalBlue
            0xFF8B4513,     // SaddleBrown
            0xFFFA8072,     // Salmon
            0xFFF4A460,     // SandyBrown
            0xFF2E8B57,     // SeaGreen
            0xFFFFF5EE,     // SeaShell
            0xFFA0522D,     // Sienna
            0xFFC0C0C0,     // Silver
            0xFF87CEEB,     // SkyBlue
            0xFF6A5ACD,     // SlateBlue
            0xFF708090,     // SlateGray
            0xFFFFFAFA,     // Snow
            0xFF00FF7F,     // SpringGreen
            0xFF4682B4,     // SteelBlue
            0xFFD2B48C,     // Tan
            0xFF008080,     // Teal
            0xFFD8BFD8,     // Thistle
            0xFFFF6347,     // Tomato
            0xFF40E0D0,     // Turquoise
            0xFFEE82EE,     // Violet
            0xFFF5DEB3,     // Wheat
            0xFFFFFFFF,     // White
            0xFFF5F5F5,     // WhiteSmoke
            0xFFFFFF00,     // Yellow
            0xFF9ACD32,     // YellowGreen

            /* "System" colors, Part 2 */
            0xFFF0F0F0,     // ButtonFace
            0xFFFFFFFF,     // ButtonHighlight
            0xFFA0A0A0,     // ButtonShadow
            0xFFB9D1EA,     // GradientActiveCaption
            0xFFD7E4F2,     // GradientInactiveCaption
            0xFFF0F0F0,     // MenuBar
            0xFF3399FF,     // MenuHighlight

            /* "Web" colors, Part 2 */
            0xFF663399,     // RebeccaPurple
        };

        /// <summary>
        /// All known color kinds (in order of definition in the <see cref="KnownColor"/> enum).
        /// </summary>
        public static byte[] ColorKindTable => new byte[]
        {
            // "not a known color"
            KnownColorKindUnknown,

            // "System" colors, Part 1
            KnownColorKindSystem,       // ActiveBorder
            KnownColorKindSystem,       // ActiveCaption
            KnownColorKindSystem,       // ActiveCaptionText
            KnownColorKindSystem,       // AppWorkspace
            KnownColorKindSystem,       // Control
            KnownColorKindSystem,       // ControlDark
            KnownColorKindSystem,       // ControlDarkDark
            KnownColorKindSystem,       // ControlLight
            KnownColorKindSystem,       // ControlLightLight
            KnownColorKindSystem,       // ControlText
            KnownColorKindSystem,       // Desktop
            KnownColorKindSystem,       // GrayText
            KnownColorKindSystem,       // Highlight
            KnownColorKindSystem,       // HighlightText
            KnownColorKindSystem,       // HotTrack
            KnownColorKindSystem,       // InactiveBorder
            KnownColorKindSystem,       // InactiveCaption
            KnownColorKindSystem,       // InactiveCaptionText
            KnownColorKindSystem,       // Info
            KnownColorKindSystem,       // InfoText
            KnownColorKindSystem,       // Menu
            KnownColorKindSystem,       // MenuText
            KnownColorKindSystem,       // ScrollBar
            KnownColorKindSystem,       // Window
            KnownColorKindSystem,       // WindowFrame
            KnownColorKindSystem,       // WindowText

            // "Web" Colors, Part 1
            KnownColorKindWeb,      // Transparent
            KnownColorKindWeb,      // AliceBlue
            KnownColorKindWeb,      // AntiqueWhite
            KnownColorKindWeb,      // Aqua
            KnownColorKindWeb,      // Aquamarine
            KnownColorKindWeb,      // Azure
            KnownColorKindWeb,      // Beige
            KnownColorKindWeb,      // Bisque
            KnownColorKindWeb,      // Black
            KnownColorKindWeb,      // BlanchedAlmond
            KnownColorKindWeb,      // Blue
            KnownColorKindWeb,      // BlueViolet
            KnownColorKindWeb,      // Brown
            KnownColorKindWeb,      // BurlyWood
            KnownColorKindWeb,      // CadetBlue
            KnownColorKindWeb,      // Chartreuse
            KnownColorKindWeb,      // Chocolate
            KnownColorKindWeb,      // Coral
            KnownColorKindWeb,      // CornflowerBlue
            KnownColorKindWeb,      // Cornsilk
            KnownColorKindWeb,      // Crimson
            KnownColorKindWeb,      // Cyan
            KnownColorKindWeb,      // DarkBlue
            KnownColorKindWeb,      // DarkCyan
            KnownColorKindWeb,      // DarkGoldenrod
            KnownColorKindWeb,      // DarkGray
            KnownColorKindWeb,      // DarkGreen
            KnownColorKindWeb,      // DarkKhaki
            KnownColorKindWeb,      // DarkMagenta
            KnownColorKindWeb,      // DarkOliveGreen
            KnownColorKindWeb,      // DarkOrange
            KnownColorKindWeb,      // DarkOrchid
            KnownColorKindWeb,      // DarkRed
            KnownColorKindWeb,      // DarkSalmon
            KnownColorKindWeb,      // DarkSeaGreen
            KnownColorKindWeb,      // DarkSlateBlue
            KnownColorKindWeb,      // DarkSlateGray
            KnownColorKindWeb,      // DarkTurquoise
            KnownColorKindWeb,      // DarkViolet
            KnownColorKindWeb,      // DeepPink
            KnownColorKindWeb,      // DeepSkyBlue
            KnownColorKindWeb,      // DimGray
            KnownColorKindWeb,      // DodgerBlue
            KnownColorKindWeb,      // Firebrick
            KnownColorKindWeb,      // FloralWhite
            KnownColorKindWeb,      // ForestGreen
            KnownColorKindWeb,      // Fuchsia
            KnownColorKindWeb,      // Gainsboro
            KnownColorKindWeb,      // GhostWhite
            KnownColorKindWeb,      // Gold
            KnownColorKindWeb,      // Goldenrod
            KnownColorKindWeb,      // Gray
            KnownColorKindWeb,      // Green
            KnownColorKindWeb,      // GreenYellow
            KnownColorKindWeb,      // Honeydew
            KnownColorKindWeb,      // HotPink
            KnownColorKindWeb,      // IndianRed
            KnownColorKindWeb,      // Indigo
            KnownColorKindWeb,      // Ivory
            KnownColorKindWeb,      // Khaki
            KnownColorKindWeb,      // Lavender
            KnownColorKindWeb,      // LavenderBlush
            KnownColorKindWeb,      // LawnGreen
            KnownColorKindWeb,      // LemonChiffon
            KnownColorKindWeb,      // LightBlue
            KnownColorKindWeb,      // LightCoral
            KnownColorKindWeb,      // LightCyan
            KnownColorKindWeb,      // LightGoldenrodYellow
            KnownColorKindWeb,      // LightGray
            KnownColorKindWeb,      // LightGreen
            KnownColorKindWeb,      // LightPink
            KnownColorKindWeb,      // LightSalmon
            KnownColorKindWeb,      // LightSeaGreen
            KnownColorKindWeb,      // LightSkyBlue
            KnownColorKindWeb,      // LightSlateGray
            KnownColorKindWeb,      // LightSteelBlue
            KnownColorKindWeb,      // LightYellow
            KnownColorKindWeb,      // Lime
            KnownColorKindWeb,      // LimeGreen
            KnownColorKindWeb,      // Linen
            KnownColorKindWeb,      // Magenta
            KnownColorKindWeb,      // Maroon
            KnownColorKindWeb,      // MediumAquamarine
            KnownColorKindWeb,      // MediumBlue
            KnownColorKindWeb,      // MediumOrchid
            KnownColorKindWeb,      // MediumPurple
            KnownColorKindWeb,      // MediumSeaGreen
            KnownColorKindWeb,      // MediumSlateBlue
            KnownColorKindWeb,      // MediumSpringGreen
            KnownColorKindWeb,      // MediumTurquoise
            KnownColorKindWeb,      // MediumVioletRed
            KnownColorKindWeb,      // MidnightBlue
            KnownColorKindWeb,      // MintCream
            KnownColorKindWeb,      // MistyRose
            KnownColorKindWeb,      // Moccasin
            KnownColorKindWeb,      // NavajoWhite
            KnownColorKindWeb,      // Navy
            KnownColorKindWeb,      // OldLace
            KnownColorKindWeb,      // Olive
            KnownColorKindWeb,      // OliveDrab
            KnownColorKindWeb,      // Orange
            KnownColorKindWeb,      // OrangeRed
            KnownColorKindWeb,      // Orchid
            KnownColorKindWeb,      // PaleGoldenrod
            KnownColorKindWeb,      // PaleGreen
            KnownColorKindWeb,      // PaleTurquoise
            KnownColorKindWeb,      // PaleVioletRed
            KnownColorKindWeb,      // PapayaWhip
            KnownColorKindWeb,      // PeachPuff
            KnownColorKindWeb,      // Peru
            KnownColorKindWeb,      // Pink
            KnownColorKindWeb,      // Plum
            KnownColorKindWeb,      // PowderBlue
            KnownColorKindWeb,      // Purple
            KnownColorKindWeb,      // Red
            KnownColorKindWeb,      // RosyBrown
            KnownColorKindWeb,      // RoyalBlue
            KnownColorKindWeb,      // SaddleBrown
            KnownColorKindWeb,      // Salmon
            KnownColorKindWeb,      // SandyBrown
            KnownColorKindWeb,      // SeaGreen
            KnownColorKindWeb,      // SeaShell
            KnownColorKindWeb,      // Sienna
            KnownColorKindWeb,      // Silver
            KnownColorKindWeb,      // SkyBlue
            KnownColorKindWeb,      // SlateBlue
            KnownColorKindWeb,      // SlateGray
            KnownColorKindWeb,      // Snow
            KnownColorKindWeb,      // SpringGreen
            KnownColorKindWeb,      // SteelBlue
            KnownColorKindWeb,      // Tan
            KnownColorKindWeb,      // Teal
            KnownColorKindWeb,      // Thistle
            KnownColorKindWeb,      // Tomato
            KnownColorKindWeb,      // Turquoise
            KnownColorKindWeb,      // Violet
            KnownColorKindWeb,      // Wheat
            KnownColorKindWeb,      // White
            KnownColorKindWeb,      // WhiteSmoke
            KnownColorKindWeb,      // Yellow
            KnownColorKindWeb,      // YellowGreen

            // "System" colors, Part 2
            KnownColorKindSystem,       // ButtonFace
            KnownColorKindSystem,       // ButtonHighlight
            KnownColorKindSystem,       // ButtonShadow
            KnownColorKindSystem,       // GradientActiveCaption
            KnownColorKindSystem,       // GradientInactiveCaption
            KnownColorKindSystem,       // MenuBar
            KnownColorKindSystem,       // MenuHighlight

            // "Web" colors, Part 2
            KnownColorKindWeb,      // RebeccaPurple
        };

        /// <summary>
        /// Gets ARGB from <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Known color.</param>
        public static ColorStruct KnownColorToArgb(KnownColor color)
        {
            Debug.Assert(color > 0 && color <= KnownColor.RebeccaPurple, nameof(KnownColorToArgb));

            ColorStruct result;

            if (ColorKindTable[(int)color] == KnownColorKindSystem)
                result = SystemSettings.GetColor((KnownSystemColor)color);
            else
                result = ColorValueTable[(int)color];

            return result;
        }

        /// <summary>
        /// Converts ARGB color value to <see cref="Color"/>.
        /// </summary>
        /// <param name="argb">ARGB color value.</param>
        public static Color ArgbToKnownColor(uint argb)
        {
            Debug.Assert(
                (argb & Color.ARGBAlphaMask) == Color.ARGBAlphaMask,
                nameof(ArgbToKnownColor));
            Debug.Assert(ColorValueTable.Length == ColorKindTable.Length, nameof(ArgbToKnownColor));

            for (int index = 1; index < ColorValueTable.Length; ++index)
            {
                if (ColorKindTable[index] == KnownColorKindWeb && ColorValueTable[index] == argb)
                {
                    return Color.FromKnownColor((KnownColor)index);
                }
            }

            // Not a known color
            return Color.FromArgb((int)argb);
        }
    }
}
