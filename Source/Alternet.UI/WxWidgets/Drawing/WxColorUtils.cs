using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    internal static class WxColorUtils
    {
        /// <summary>
        /// All known color values (in order of definition in the <see cref="KnownColor"/>).
        /// </summary>
        private static readonly SystemSettingsColor[] KnownColorToSystemSettingsColorTable
            = new SystemSettingsColor[]
        {
            /*"not a known color"*/
            SystemSettingsColor.Max,

            /*"System" colors, Part 1*/
            SystemSettingsColor.ActiveBorder,     // ActiveBorder
            SystemSettingsColor.ActiveCaption,     // ActiveCaption
            SystemSettingsColor.ActiveCaptionText,     // ActiveCaptionText
            SystemSettingsColor.AppWorkspace,     // AppWorkspace
            SystemSettingsColor.ButtonFace,     // Control
            SystemSettingsColor.ButtonShadow,     // ControlDark
            SystemSettingsColor.ControlDarkDark,     // ControlDarkDark
            SystemSettingsColor.ControlLight,     // ControlLight
            SystemSettingsColor.ButtonHighlight,     // ControlLightLight
            SystemSettingsColor.ControlText,     // ControlText
            SystemSettingsColor.Desktop,     // Desktop
            SystemSettingsColor.GrayText,     // GrayText
            SystemSettingsColor.Highlight,     // Highlight
            SystemSettingsColor.HighlightText,     // HighlightText
            SystemSettingsColor.HotTrack,     // HotTrack
            SystemSettingsColor.InactiveBorder,     // InactiveBorder
            SystemSettingsColor.InactiveCaption,     // InactiveCaption
            SystemSettingsColor.InactiveCaptionText,     // InactiveCaptionText
            SystemSettingsColor.Info,     // Info
            SystemSettingsColor.InfoText,     // InfoText
            SystemSettingsColor.Menu,     // Menu
            SystemSettingsColor.MenuText,     // MenuText
            SystemSettingsColor.ScrollBar,     // ScrollBar
            SystemSettingsColor.Window,     // Window
            SystemSettingsColor.WindowFrame,     // WindowFrame
            SystemSettingsColor.WindowText,     // WindowText

            // "Web" Colors, Part 1
            SystemSettingsColor.Max,     // Transparent
            SystemSettingsColor.Max,     // AliceBlue
            SystemSettingsColor.Max,     // AntiqueWhite
            SystemSettingsColor.Max,     // Aqua
            SystemSettingsColor.Max,     // Aquamarine
            SystemSettingsColor.Max,     // Azure
            SystemSettingsColor.Max,     // Beige
            SystemSettingsColor.Max,     // Bisque
            SystemSettingsColor.Max,     // Black
            SystemSettingsColor.Max,     // BlanchedAlmond
            SystemSettingsColor.Max,     // Blue
            SystemSettingsColor.Max,     // BlueViolet
            SystemSettingsColor.Max,     // Brown
            SystemSettingsColor.Max,     // BurlyWood
            SystemSettingsColor.Max,     // CadetBlue
            SystemSettingsColor.Max,     // Chartreuse
            SystemSettingsColor.Max,     // Chocolate
            SystemSettingsColor.Max,     // Coral
            SystemSettingsColor.Max,     // CornflowerBlue
            SystemSettingsColor.Max,     // Cornsilk
            SystemSettingsColor.Max,     // Crimson
            SystemSettingsColor.Max,     // Cyan
            SystemSettingsColor.Max,     // DarkBlue
            SystemSettingsColor.Max,     // DarkCyan
            SystemSettingsColor.Max,     // DarkGoldenrod
            SystemSettingsColor.Max,     // DarkGray
            SystemSettingsColor.Max,     // DarkGreen
            SystemSettingsColor.Max,     // DarkKhaki
            SystemSettingsColor.Max,     // DarkMagenta
            SystemSettingsColor.Max,     // DarkOliveGreen
            SystemSettingsColor.Max,     // DarkOrange
            SystemSettingsColor.Max,     // DarkOrchid
            SystemSettingsColor.Max,     // DarkRed
            SystemSettingsColor.Max,     // DarkSalmon
            SystemSettingsColor.Max,     // DarkSeaGreen
            SystemSettingsColor.Max,     // DarkSlateBlue
            SystemSettingsColor.Max,     // DarkSlateGray
            SystemSettingsColor.Max,     // DarkTurquoise
            SystemSettingsColor.Max,     // DarkViolet
            SystemSettingsColor.Max,     // DeepPink
            SystemSettingsColor.Max,     // DeepSkyBlue
            SystemSettingsColor.Max,     // DimGray
            SystemSettingsColor.Max,     // DodgerBlue
            SystemSettingsColor.Max,     // Firebrick
            SystemSettingsColor.Max,     // FloralWhite
            SystemSettingsColor.Max,     // ForestGreen
            SystemSettingsColor.Max,     // Fuchsia
            SystemSettingsColor.Max,     // Gainsboro
            SystemSettingsColor.Max,     // GhostWhite
            SystemSettingsColor.Max,     // Gold
            SystemSettingsColor.Max,     // Goldenrod
            SystemSettingsColor.Max,     // Gray
            SystemSettingsColor.Max,     // Green
            SystemSettingsColor.Max,     // GreenYellow
            SystemSettingsColor.Max,     // Honeydew
            SystemSettingsColor.Max,     // HotPink
            SystemSettingsColor.Max,     // IndianRed
            SystemSettingsColor.Max,     // Indigo
            SystemSettingsColor.Max,     // Ivory
            SystemSettingsColor.Max,     // Khaki
            SystemSettingsColor.Max,     // Lavender
            SystemSettingsColor.Max,     // LavenderBlush
            SystemSettingsColor.Max,     // LawnGreen
            SystemSettingsColor.Max,     // LemonChiffon
            SystemSettingsColor.Max,     // LightBlue
            SystemSettingsColor.Max,     // LightCoral
            SystemSettingsColor.Max,     // LightCyan
            SystemSettingsColor.Max,     // LightGoldenrodYellow
            SystemSettingsColor.Max,     // LightGray
            SystemSettingsColor.Max,     // LightGreen
            SystemSettingsColor.Max,     // LightPink
            SystemSettingsColor.Max,     // LightSalmon
            SystemSettingsColor.Max,     // LightSeaGreen
            SystemSettingsColor.Max,     // LightSkyBlue
            SystemSettingsColor.Max,     // LightSlateGray
            SystemSettingsColor.Max,     // LightSteelBlue
            SystemSettingsColor.Max,     // LightYellow
            SystemSettingsColor.Max,     // Lime
            SystemSettingsColor.Max,     // LimeGreen
            SystemSettingsColor.Max,     // Linen
            SystemSettingsColor.Max,     // Magenta
            SystemSettingsColor.Max,     // Maroon
            SystemSettingsColor.Max,     // MediumAquamarine
            SystemSettingsColor.Max,     // MediumBlue
            SystemSettingsColor.Max,     // MediumOrchid
            SystemSettingsColor.Max,     // MediumPurple
            SystemSettingsColor.Max,     // MediumSeaGreen
            SystemSettingsColor.Max,     // MediumSlateBlue
            SystemSettingsColor.Max,     // MediumSpringGreen
            SystemSettingsColor.Max,     // MediumTurquoise
            SystemSettingsColor.Max,     // MediumVioletRed
            SystemSettingsColor.Max,     // MidnightBlue
            SystemSettingsColor.Max,     // MintCream
            SystemSettingsColor.Max,     // MistyRose
            SystemSettingsColor.Max,     // Moccasin
            SystemSettingsColor.Max,     // NavajoWhite
            SystemSettingsColor.Max,     // Navy
            SystemSettingsColor.Max,     // OldLace
            SystemSettingsColor.Max,     // Olive
            SystemSettingsColor.Max,     // OliveDrab
            SystemSettingsColor.Max,     // Orange
            SystemSettingsColor.Max,     // OrangeRed
            SystemSettingsColor.Max,     // Orchid
            SystemSettingsColor.Max,     // PaleGoldenrod
            SystemSettingsColor.Max,     // PaleGreen
            SystemSettingsColor.Max,     // PaleTurquoise
            SystemSettingsColor.Max,     // PaleVioletRed
            SystemSettingsColor.Max,     // PapayaWhip
            SystemSettingsColor.Max,     // PeachPuff
            SystemSettingsColor.Max,     // Peru
            SystemSettingsColor.Max,     // Pink
            SystemSettingsColor.Max,     // Plum
            SystemSettingsColor.Max,     // PowderBlue
            SystemSettingsColor.Max,     // Purple
            SystemSettingsColor.Max,     // Red
            SystemSettingsColor.Max,     // RosyBrown
            SystemSettingsColor.Max,     // RoyalBlue
            SystemSettingsColor.Max,     // SaddleBrown
            SystemSettingsColor.Max,     // Salmon
            SystemSettingsColor.Max,     // SandyBrown
            SystemSettingsColor.Max,     // SeaGreen
            SystemSettingsColor.Max,     // SeaShell
            SystemSettingsColor.Max,     // Sienna
            SystemSettingsColor.Max,     // Silver
            SystemSettingsColor.Max,     // SkyBlue
            SystemSettingsColor.Max,     // SlateBlue
            SystemSettingsColor.Max,     // SlateGray
            SystemSettingsColor.Max,     // Snow
            SystemSettingsColor.Max,     // SpringGreen
            SystemSettingsColor.Max,     // SteelBlue
            SystemSettingsColor.Max,     // Tan
            SystemSettingsColor.Max,     // Teal
            SystemSettingsColor.Max,     // Thistle
            SystemSettingsColor.Max,     // Tomato
            SystemSettingsColor.Max,     // Turquoise
            SystemSettingsColor.Max,     // Violet
            SystemSettingsColor.Max,     // Wheat
            SystemSettingsColor.Max,     // White
            SystemSettingsColor.Max,     // WhiteSmoke
            SystemSettingsColor.Max,     // Yellow
            SystemSettingsColor.Max,     // YellowGreen

            // "System" colors, Part 2
            SystemSettingsColor.ButtonFace,     // ButtonFace
            SystemSettingsColor.ButtonHighlight,     // ButtonHighlight
            SystemSettingsColor.ButtonShadow,     // ButtonShadow
            SystemSettingsColor.GradientActiveCaption,     // GradientActiveCaption
            SystemSettingsColor.GradientInactiveCaption,     // GradientInactiveCaption
            SystemSettingsColor.MenuBar,     // MenuBar
            SystemSettingsColor.MenuHighlight,     // MenuHighlight

            // "Web" colors, Part 2
            SystemSettingsColor.Max,     // RebeccaPurple
        };

        private static readonly KnownColor[] SystemSettingsColorToKnownColorTable = new KnownColor[]
        {
                /*0x00*/KnownColor.ScrollBar,
                /*0x01*/KnownColor.Desktop,
                /*0x02*/KnownColor.ActiveCaption,
                /*0x03*/KnownColor.InactiveCaption,
                /*0x04*/KnownColor.Menu,
                /*0x05*/KnownColor.Window,
                /*0x06*/KnownColor.WindowFrame,
                /*0x07*/KnownColor.MenuText,
                /*0x08*/KnownColor.WindowText,
                /*0x09*/KnownColor.ActiveCaptionText,
                /*0x0A*/KnownColor.ActiveBorder,
                /*0x0B*/KnownColor.InactiveBorder,
                /*0x0C*/KnownColor.AppWorkspace,
                /*0x0D*/KnownColor.Highlight,
                /*0x0E*/KnownColor.HighlightText,
                /*0x0F*/KnownColor.ButtonFace,
                /*0x10*/KnownColor.ButtonShadow,
                /*0x11*/KnownColor.GrayText,
                /*0x12*/KnownColor.ControlText,
                /*0x13*/KnownColor.InactiveCaptionText,
                /*0x14*/KnownColor.ButtonHighlight,
                /*0x15*/KnownColor.ControlDarkDark,
                /*0x16*/KnownColor.ControlLight,
                /*0x17*/KnownColor.InfoText,
                /*0x18*/KnownColor.Info,
                /*0x19*/0,
                /*0x1A*/KnownColor.HotTrack,
                /*0x1B*/KnownColor.GradientActiveCaption,
                /*0x1C*/KnownColor.GradientInactiveCaption,
                /*0x1D*/KnownColor.MenuHighlight,
                /*0x1E*/KnownColor.MenuBar,
        };

        /// <summary>
        /// Converts <see cref="SystemSettingsColor"/> to <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <returns>Converted color value.</returns>
        public static KnownColor Convert(SystemSettingsColor color)
        {
            if (color >= 0 && (int)color <= 0x1E)
                return SystemSettingsColorToKnownColorTable[(int)color];
            return 0;
        }

        /// <summary>
        /// Converts <see cref="KnownColor"/> to <see cref="SystemSettingsColor"/>.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <returns>Converted color value.</returns>
        public static SystemSettingsColor Convert(KnownColor color)
        {
            var result = KnownColorToSystemSettingsColorTable[(int)color];
            return result;
        }

        /// <summary>
        /// Converts <see cref="KnownSystemColor"/> to <see cref="SystemSettingsColor"/>.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <returns>Converted color value.</returns>
        public static SystemSettingsColor Convert(KnownSystemColor color)
        {
            var result = KnownColorToSystemSettingsColorTable[(int)color];
            return result;
            /*
                        return color switch
                        {
                            KnownColor.ActiveBorder => SystemSettingsColor.ActiveBorder,
                            KnownColor.ActiveCaption => SystemSettingsColor.ActiveCaption,
                            KnownColor.ActiveCaptionText => SystemSettingsColor.ActiveCaptionText,
                            KnownColor.AppWorkspace => SystemSettingsColor.AppWorkspace,
                            KnownColor.ButtonFace => SystemSettingsColor.ButtonFace,
                            KnownColor.ButtonHighlight => SystemSettingsColor.ButtonHighlight,
                            KnownColor.ButtonShadow => SystemSettingsColor.ButtonShadow,
                            KnownColor.Control => SystemSettingsColor.ButtonFace,
                            KnownColor.ControlDark => SystemSettingsColor.ButtonShadow,
                            KnownColor.ControlDarkDark => SystemSettingsColor.ControlDarkDark,
                            KnownColor.ControlLight => SystemSettingsColor.ControlLight,
                            KnownColor.ControlLightLight => SystemSettingsColor.ButtonHighlight,
                            KnownColor.ControlText => SystemSettingsColor.ControlText,
                            KnownColor.Desktop => SystemSettingsColor.Desktop,
                            KnownColor.GradientActiveCaption => SystemSettingsColor.GradientActiveCaption,
                            KnownColor.GradientInactiveCaption => SystemSettingsColor.GradientInactiveCaption,
                            KnownColor.GrayText => SystemSettingsColor.GrayText,
                            KnownColor.Highlight => SystemSettingsColor.Highlight,
                            KnownColor.HighlightText => SystemSettingsColor.HighlightText,
                            KnownColor.HotTrack => SystemSettingsColor.HotTrack,
                            KnownColor.InactiveBorder => SystemSettingsColor.InactiveBorder,
                            KnownColor.InactiveCaption => SystemSettingsColor.InactiveCaption,
                            KnownColor.InactiveCaptionText => SystemSettingsColor.InactiveCaptionText,
                            KnownColor.Info => SystemSettingsColor.Info,
                            KnownColor.InfoText => SystemSettingsColor.InfoText,
                            KnownColor.Menu => SystemSettingsColor.Menu,
                            KnownColor.MenuBar => SystemSettingsColor.MenuBar,
                            KnownColor.MenuHighlight => SystemSettingsColor.MenuHighlight,
                            KnownColor.MenuText => SystemSettingsColor.MenuText,
                            KnownColor.ScrollBar => SystemSettingsColor.ScrollBar,
                            KnownColor.Window => SystemSettingsColor.Window,
                            KnownColor.WindowFrame => SystemSettingsColor.WindowFrame,
                            KnownColor.WindowText => SystemSettingsColor.WindowText,
                            _ => SystemSettingsColor.Max,
                        };
            */
        }
    }
}
