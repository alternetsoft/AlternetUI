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
        private static readonly WxSystemSettingsColor[] KnownColorToSystemSettingsColorTable
            = new WxSystemSettingsColor[]
        {
            /*"not a known color"*/
            WxSystemSettingsColor.Max,

            /*"System" colors, Part 1*/
            WxSystemSettingsColor.ActiveBorder,     // ActiveBorder
            WxSystemSettingsColor.ActiveCaption,     // ActiveCaption
            WxSystemSettingsColor.ActiveCaptionText,     // ActiveCaptionText
            WxSystemSettingsColor.AppWorkspace,     // AppWorkspace
            WxSystemSettingsColor.ButtonFace,     // Control
            WxSystemSettingsColor.ButtonShadow,     // ControlDark
            WxSystemSettingsColor.ControlDarkDark,     // ControlDarkDark
            WxSystemSettingsColor.ControlLight,     // ControlLight
            WxSystemSettingsColor.ButtonHighlight,     // ControlLightLight
            WxSystemSettingsColor.ControlText,     // ControlText
            WxSystemSettingsColor.Desktop,     // Desktop
            WxSystemSettingsColor.GrayText,     // GrayText
            WxSystemSettingsColor.Highlight,     // Highlight
            WxSystemSettingsColor.HighlightText,     // HighlightText
            WxSystemSettingsColor.HotTrack,     // HotTrack
            WxSystemSettingsColor.InactiveBorder,     // InactiveBorder
            WxSystemSettingsColor.InactiveCaption,     // InactiveCaption
            WxSystemSettingsColor.InactiveCaptionText,     // InactiveCaptionText
            WxSystemSettingsColor.Info,     // Info
            WxSystemSettingsColor.InfoText,     // InfoText
            WxSystemSettingsColor.Menu,     // Menu
            WxSystemSettingsColor.MenuText,     // MenuText
            WxSystemSettingsColor.ScrollBar,     // ScrollBar
            WxSystemSettingsColor.Window,     // Window
            WxSystemSettingsColor.WindowFrame,     // WindowFrame
            WxSystemSettingsColor.WindowText,     // WindowText

            // "Web" Colors, Part 1
            WxSystemSettingsColor.Max,     // Transparent
            WxSystemSettingsColor.Max,     // AliceBlue
            WxSystemSettingsColor.Max,     // AntiqueWhite
            WxSystemSettingsColor.Max,     // Aqua
            WxSystemSettingsColor.Max,     // Aquamarine
            WxSystemSettingsColor.Max,     // Azure
            WxSystemSettingsColor.Max,     // Beige
            WxSystemSettingsColor.Max,     // Bisque
            WxSystemSettingsColor.Max,     // Black
            WxSystemSettingsColor.Max,     // BlanchedAlmond
            WxSystemSettingsColor.Max,     // Blue
            WxSystemSettingsColor.Max,     // BlueViolet
            WxSystemSettingsColor.Max,     // Brown
            WxSystemSettingsColor.Max,     // BurlyWood
            WxSystemSettingsColor.Max,     // CadetBlue
            WxSystemSettingsColor.Max,     // Chartreuse
            WxSystemSettingsColor.Max,     // Chocolate
            WxSystemSettingsColor.Max,     // Coral
            WxSystemSettingsColor.Max,     // CornflowerBlue
            WxSystemSettingsColor.Max,     // Cornsilk
            WxSystemSettingsColor.Max,     // Crimson
            WxSystemSettingsColor.Max,     // Cyan
            WxSystemSettingsColor.Max,     // DarkBlue
            WxSystemSettingsColor.Max,     // DarkCyan
            WxSystemSettingsColor.Max,     // DarkGoldenrod
            WxSystemSettingsColor.Max,     // DarkGray
            WxSystemSettingsColor.Max,     // DarkGreen
            WxSystemSettingsColor.Max,     // DarkKhaki
            WxSystemSettingsColor.Max,     // DarkMagenta
            WxSystemSettingsColor.Max,     // DarkOliveGreen
            WxSystemSettingsColor.Max,     // DarkOrange
            WxSystemSettingsColor.Max,     // DarkOrchid
            WxSystemSettingsColor.Max,     // DarkRed
            WxSystemSettingsColor.Max,     // DarkSalmon
            WxSystemSettingsColor.Max,     // DarkSeaGreen
            WxSystemSettingsColor.Max,     // DarkSlateBlue
            WxSystemSettingsColor.Max,     // DarkSlateGray
            WxSystemSettingsColor.Max,     // DarkTurquoise
            WxSystemSettingsColor.Max,     // DarkViolet
            WxSystemSettingsColor.Max,     // DeepPink
            WxSystemSettingsColor.Max,     // DeepSkyBlue
            WxSystemSettingsColor.Max,     // DimGray
            WxSystemSettingsColor.Max,     // DodgerBlue
            WxSystemSettingsColor.Max,     // Firebrick
            WxSystemSettingsColor.Max,     // FloralWhite
            WxSystemSettingsColor.Max,     // ForestGreen
            WxSystemSettingsColor.Max,     // Fuchsia
            WxSystemSettingsColor.Max,     // Gainsboro
            WxSystemSettingsColor.Max,     // GhostWhite
            WxSystemSettingsColor.Max,     // Gold
            WxSystemSettingsColor.Max,     // Goldenrod
            WxSystemSettingsColor.Max,     // Gray
            WxSystemSettingsColor.Max,     // Green
            WxSystemSettingsColor.Max,     // GreenYellow
            WxSystemSettingsColor.Max,     // Honeydew
            WxSystemSettingsColor.Max,     // HotPink
            WxSystemSettingsColor.Max,     // IndianRed
            WxSystemSettingsColor.Max,     // Indigo
            WxSystemSettingsColor.Max,     // Ivory
            WxSystemSettingsColor.Max,     // Khaki
            WxSystemSettingsColor.Max,     // Lavender
            WxSystemSettingsColor.Max,     // LavenderBlush
            WxSystemSettingsColor.Max,     // LawnGreen
            WxSystemSettingsColor.Max,     // LemonChiffon
            WxSystemSettingsColor.Max,     // LightBlue
            WxSystemSettingsColor.Max,     // LightCoral
            WxSystemSettingsColor.Max,     // LightCyan
            WxSystemSettingsColor.Max,     // LightGoldenrodYellow
            WxSystemSettingsColor.Max,     // LightGray
            WxSystemSettingsColor.Max,     // LightGreen
            WxSystemSettingsColor.Max,     // LightPink
            WxSystemSettingsColor.Max,     // LightSalmon
            WxSystemSettingsColor.Max,     // LightSeaGreen
            WxSystemSettingsColor.Max,     // LightSkyBlue
            WxSystemSettingsColor.Max,     // LightSlateGray
            WxSystemSettingsColor.Max,     // LightSteelBlue
            WxSystemSettingsColor.Max,     // LightYellow
            WxSystemSettingsColor.Max,     // Lime
            WxSystemSettingsColor.Max,     // LimeGreen
            WxSystemSettingsColor.Max,     // Linen
            WxSystemSettingsColor.Max,     // Magenta
            WxSystemSettingsColor.Max,     // Maroon
            WxSystemSettingsColor.Max,     // MediumAquamarine
            WxSystemSettingsColor.Max,     // MediumBlue
            WxSystemSettingsColor.Max,     // MediumOrchid
            WxSystemSettingsColor.Max,     // MediumPurple
            WxSystemSettingsColor.Max,     // MediumSeaGreen
            WxSystemSettingsColor.Max,     // MediumSlateBlue
            WxSystemSettingsColor.Max,     // MediumSpringGreen
            WxSystemSettingsColor.Max,     // MediumTurquoise
            WxSystemSettingsColor.Max,     // MediumVioletRed
            WxSystemSettingsColor.Max,     // MidnightBlue
            WxSystemSettingsColor.Max,     // MintCream
            WxSystemSettingsColor.Max,     // MistyRose
            WxSystemSettingsColor.Max,     // Moccasin
            WxSystemSettingsColor.Max,     // NavajoWhite
            WxSystemSettingsColor.Max,     // Navy
            WxSystemSettingsColor.Max,     // OldLace
            WxSystemSettingsColor.Max,     // Olive
            WxSystemSettingsColor.Max,     // OliveDrab
            WxSystemSettingsColor.Max,     // Orange
            WxSystemSettingsColor.Max,     // OrangeRed
            WxSystemSettingsColor.Max,     // Orchid
            WxSystemSettingsColor.Max,     // PaleGoldenrod
            WxSystemSettingsColor.Max,     // PaleGreen
            WxSystemSettingsColor.Max,     // PaleTurquoise
            WxSystemSettingsColor.Max,     // PaleVioletRed
            WxSystemSettingsColor.Max,     // PapayaWhip
            WxSystemSettingsColor.Max,     // PeachPuff
            WxSystemSettingsColor.Max,     // Peru
            WxSystemSettingsColor.Max,     // Pink
            WxSystemSettingsColor.Max,     // Plum
            WxSystemSettingsColor.Max,     // PowderBlue
            WxSystemSettingsColor.Max,     // Purple
            WxSystemSettingsColor.Max,     // Red
            WxSystemSettingsColor.Max,     // RosyBrown
            WxSystemSettingsColor.Max,     // RoyalBlue
            WxSystemSettingsColor.Max,     // SaddleBrown
            WxSystemSettingsColor.Max,     // Salmon
            WxSystemSettingsColor.Max,     // SandyBrown
            WxSystemSettingsColor.Max,     // SeaGreen
            WxSystemSettingsColor.Max,     // SeaShell
            WxSystemSettingsColor.Max,     // Sienna
            WxSystemSettingsColor.Max,     // Silver
            WxSystemSettingsColor.Max,     // SkyBlue
            WxSystemSettingsColor.Max,     // SlateBlue
            WxSystemSettingsColor.Max,     // SlateGray
            WxSystemSettingsColor.Max,     // Snow
            WxSystemSettingsColor.Max,     // SpringGreen
            WxSystemSettingsColor.Max,     // SteelBlue
            WxSystemSettingsColor.Max,     // Tan
            WxSystemSettingsColor.Max,     // Teal
            WxSystemSettingsColor.Max,     // Thistle
            WxSystemSettingsColor.Max,     // Tomato
            WxSystemSettingsColor.Max,     // Turquoise
            WxSystemSettingsColor.Max,     // Violet
            WxSystemSettingsColor.Max,     // Wheat
            WxSystemSettingsColor.Max,     // White
            WxSystemSettingsColor.Max,     // WhiteSmoke
            WxSystemSettingsColor.Max,     // Yellow
            WxSystemSettingsColor.Max,     // YellowGreen

            // "System" colors, Part 2
            WxSystemSettingsColor.ButtonFace,     // ButtonFace
            WxSystemSettingsColor.ButtonHighlight,     // ButtonHighlight
            WxSystemSettingsColor.ButtonShadow,     // ButtonShadow
            WxSystemSettingsColor.GradientActiveCaption,     // GradientActiveCaption
            WxSystemSettingsColor.GradientInactiveCaption,     // GradientInactiveCaption
            WxSystemSettingsColor.MenuBar,     // MenuBar
            WxSystemSettingsColor.MenuHighlight,     // MenuHighlight

            // "Web" colors, Part 2
            WxSystemSettingsColor.Max,     // RebeccaPurple
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
        /// Converts <see cref="WxSystemSettingsColor"/> to <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <returns>Converted color value.</returns>
        public static KnownColor Convert(WxSystemSettingsColor color)
        {
            if (color >= 0 && (int)color <= 0x1E)
                return SystemSettingsColorToKnownColorTable[(int)color];
            return 0;
        }

        /// <summary>
        /// Converts <see cref="KnownColor"/> to <see cref="WxSystemSettingsColor"/>.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <returns>Converted color value.</returns>
        public static WxSystemSettingsColor Convert(KnownColor color)
        {
            var result = KnownColorToSystemSettingsColorTable[(int)color];
            return result;
        }

        /// <summary>
        /// Converts <see cref="KnownSystemColor"/> to <see cref="WxSystemSettingsColor"/>.
        /// </summary>
        /// <param name="color">Color value.</param>
        /// <returns>Converted color value.</returns>
        public static WxSystemSettingsColor Convert(KnownSystemColor color)
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
