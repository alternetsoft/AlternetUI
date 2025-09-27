using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a cached collection of <see cref="Color"/>
    /// instances corresponding to all <see cref="KnownColor"/> values.
    /// </summary>
    /// <remarks>This class improves performance by precomputing and caching all
    /// <see cref="Color"/> instances derived
    /// from <see cref="KnownColor"/> values. Use the
    /// <see cref="Get"/> method to retrieve
    /// a cached <see cref="Color"/> instance.</remarks>
    public static class ColorCache
    {
        private static readonly Color[] colors;

        static ColorCache()
        {
            colors = new Color[(int)KnownColor.RebeccaPurple + 1];
            colors[(int)KnownColor.ActiveBorder] = SystemColors.ActiveBorder;
            colors[(int)KnownColor.ActiveCaption] = SystemColors.ActiveCaption;
            colors[(int)KnownColor.ActiveCaptionText] = SystemColors.ActiveCaptionText;
            colors[(int)KnownColor.AppWorkspace] = SystemColors.AppWorkspace;
            colors[(int)KnownColor.Control] = SystemColors.Control;
            colors[(int)KnownColor.ControlDark] = SystemColors.ControlDark;
            colors[(int)KnownColor.ControlDarkDark] = SystemColors.ControlDarkDark;
            colors[(int)KnownColor.ControlLight] = SystemColors.ControlLight;
            colors[(int)KnownColor.ControlLightLight] = SystemColors.ControlLightLight;
            colors[(int)KnownColor.ControlText] = SystemColors.ControlText;
            colors[(int)KnownColor.Desktop] = SystemColors.Desktop;
            colors[(int)KnownColor.GrayText] = SystemColors.GrayText;
            colors[(int)KnownColor.Highlight] = SystemColors.Highlight;
            colors[(int)KnownColor.HighlightText] = SystemColors.HighlightText;
            colors[(int)KnownColor.HotTrack] = SystemColors.HotTrack;
            colors[(int)KnownColor.InactiveBorder] = SystemColors.InactiveBorder;
            colors[(int)KnownColor.InactiveCaption] = SystemColors.InactiveCaption;
            colors[(int)KnownColor.InactiveCaptionText] = SystemColors.InactiveCaptionText;
            colors[(int)KnownColor.Info] = SystemColors.Info;
            colors[(int)KnownColor.InfoText] = SystemColors.InfoText;
            colors[(int)KnownColor.Menu] = SystemColors.Menu;
            colors[(int)KnownColor.MenuText] = SystemColors.MenuText;
            colors[(int)KnownColor.ScrollBar] = SystemColors.ScrollBar;
            colors[(int)KnownColor.Window] = SystemColors.Window;
            colors[(int)KnownColor.WindowFrame] = SystemColors.WindowFrame;
            colors[(int)KnownColor.WindowText] = SystemColors.WindowText;
            colors[(int)KnownColor.Transparent] = Color.Transparent;
            colors[(int)KnownColor.AliceBlue] = Color.AliceBlue;
            colors[(int)KnownColor.AntiqueWhite] = Color.AntiqueWhite;
            colors[(int)KnownColor.Aqua] = Color.Aqua;
            colors[(int)KnownColor.Aquamarine] = Color.Aquamarine;
            colors[(int)KnownColor.Azure] = Color.Azure;
            colors[(int)KnownColor.Beige] = Color.Beige;
            colors[(int)KnownColor.Bisque] = Color.Bisque;
            colors[(int)KnownColor.Black] = Color.Black;
            colors[(int)KnownColor.BlanchedAlmond] = Color.BlanchedAlmond;
            colors[(int)KnownColor.Blue] = Color.Blue;
            colors[(int)KnownColor.BlueViolet] = Color.BlueViolet;
            colors[(int)KnownColor.Brown] = Color.Brown;
            colors[(int)KnownColor.BurlyWood] = Color.BurlyWood;
            colors[(int)KnownColor.CadetBlue] = Color.CadetBlue;
            colors[(int)KnownColor.Chartreuse] = Color.Chartreuse;
            colors[(int)KnownColor.Chocolate] = Color.Chocolate;
            colors[(int)KnownColor.Coral] = Color.Coral;
            colors[(int)KnownColor.CornflowerBlue] = Color.CornflowerBlue;
            colors[(int)KnownColor.Cornsilk] = Color.Cornsilk;
            colors[(int)KnownColor.Crimson] = Color.Crimson;
            colors[(int)KnownColor.Cyan] = Color.Cyan;
            colors[(int)KnownColor.DarkBlue] = Color.DarkBlue;
            colors[(int)KnownColor.DarkCyan] = Color.DarkCyan;
            colors[(int)KnownColor.DarkGoldenrod] = Color.DarkGoldenrod;
            colors[(int)KnownColor.DarkGray] = Color.DarkGray;
            colors[(int)KnownColor.DarkGreen] = Color.DarkGreen;
            colors[(int)KnownColor.DarkKhaki] = Color.DarkKhaki;
            colors[(int)KnownColor.DarkMagenta] = Color.DarkMagenta;
            colors[(int)KnownColor.DarkOliveGreen] = Color.DarkOliveGreen;
            colors[(int)KnownColor.DarkOrange] = Color.DarkOrange;
            colors[(int)KnownColor.DarkOrchid] = Color.DarkOrchid;
            colors[(int)KnownColor.DarkRed] = Color.DarkRed;
            colors[(int)KnownColor.DarkSalmon] = Color.DarkSalmon;
            colors[(int)KnownColor.DarkSeaGreen] = Color.DarkSeaGreen;
            colors[(int)KnownColor.DarkSlateBlue] = Color.DarkSlateBlue;
            colors[(int)KnownColor.DarkSlateGray] = Color.DarkSlateGray;
            colors[(int)KnownColor.DarkTurquoise] = Color.DarkTurquoise;
            colors[(int)KnownColor.DarkViolet] = Color.DarkViolet;
            colors[(int)KnownColor.DeepPink] = Color.DeepPink;
            colors[(int)KnownColor.DeepSkyBlue] = Color.DeepSkyBlue;
            colors[(int)KnownColor.DimGray] = Color.DimGray;
            colors[(int)KnownColor.DodgerBlue] = Color.DodgerBlue;
            colors[(int)KnownColor.Firebrick] = Color.Firebrick;
            colors[(int)KnownColor.FloralWhite] = Color.FloralWhite;
            colors[(int)KnownColor.ForestGreen] = Color.ForestGreen;
            colors[(int)KnownColor.Fuchsia] = Color.Fuchsia;
            colors[(int)KnownColor.Gainsboro] = Color.Gainsboro;
            colors[(int)KnownColor.GhostWhite] = Color.GhostWhite;
            colors[(int)KnownColor.Gold] = Color.Gold;
            colors[(int)KnownColor.Goldenrod] = Color.Goldenrod;
            colors[(int)KnownColor.Gray] = Color.Gray;
            colors[(int)KnownColor.Green] = Color.Green;
            colors[(int)KnownColor.GreenYellow] = Color.GreenYellow;
            colors[(int)KnownColor.Honeydew] = Color.Honeydew;
            colors[(int)KnownColor.HotPink] = Color.HotPink;
            colors[(int)KnownColor.IndianRed] = Color.IndianRed;
            colors[(int)KnownColor.Indigo] = Color.Indigo;
            colors[(int)KnownColor.Ivory] = Color.Ivory;
            colors[(int)KnownColor.Khaki] = Color.Khaki;
            colors[(int)KnownColor.Lavender] = Color.Lavender;
            colors[(int)KnownColor.LavenderBlush] = Color.LavenderBlush;
            colors[(int)KnownColor.LawnGreen] = Color.LawnGreen;
            colors[(int)KnownColor.LemonChiffon] = Color.LemonChiffon;
            colors[(int)KnownColor.LightBlue] = Color.LightBlue;
            colors[(int)KnownColor.LightCoral] = Color.LightCoral;
            colors[(int)KnownColor.LightCyan] = Color.LightCyan;
            colors[(int)KnownColor.LightGoldenrodYellow] = Color.LightGoldenrodYellow;
            colors[(int)KnownColor.LightGray] = Color.LightGray;
            colors[(int)KnownColor.LightGreen] = Color.LightGreen;
            colors[(int)KnownColor.LightPink] = Color.LightPink;
            colors[(int)KnownColor.LightSalmon] = Color.LightSalmon;
            colors[(int)KnownColor.LightSeaGreen] = Color.LightSeaGreen;
            colors[(int)KnownColor.LightSkyBlue] = Color.LightSkyBlue;
            colors[(int)KnownColor.LightSlateGray] = Color.LightSlateGray;
            colors[(int)KnownColor.LightSteelBlue] = Color.LightSteelBlue;
            colors[(int)KnownColor.LightYellow] = Color.LightYellow;
            colors[(int)KnownColor.Lime] = Color.Lime;
            colors[(int)KnownColor.LimeGreen] = Color.LimeGreen;
            colors[(int)KnownColor.Linen] = Color.Linen;
            colors[(int)KnownColor.Magenta] = Color.Magenta;
            colors[(int)KnownColor.Maroon] = Color.Maroon;
            colors[(int)KnownColor.MediumAquamarine] = Color.MediumAquamarine;
            colors[(int)KnownColor.MediumBlue] = Color.MediumBlue;
            colors[(int)KnownColor.MediumOrchid] = Color.MediumOrchid;
            colors[(int)KnownColor.MediumPurple] = Color.MediumPurple;
            colors[(int)KnownColor.MediumSeaGreen] = Color.MediumSeaGreen;
            colors[(int)KnownColor.MediumSlateBlue] = Color.MediumSlateBlue;
            colors[(int)KnownColor.MediumSpringGreen] = Color.MediumSpringGreen;
            colors[(int)KnownColor.MediumTurquoise] = Color.MediumTurquoise;
            colors[(int)KnownColor.MediumVioletRed] = Color.MediumVioletRed;
            colors[(int)KnownColor.MidnightBlue] = Color.MidnightBlue;
            colors[(int)KnownColor.MintCream] = Color.MintCream;
            colors[(int)KnownColor.MistyRose] = Color.MistyRose;
            colors[(int)KnownColor.Moccasin] = Color.Moccasin;
            colors[(int)KnownColor.NavajoWhite] = Color.NavajoWhite;
            colors[(int)KnownColor.Navy] = Color.Navy;
            colors[(int)KnownColor.OldLace] = Color.OldLace;
            colors[(int)KnownColor.Olive] = Color.Olive;
            colors[(int)KnownColor.OliveDrab] = Color.OliveDrab;
            colors[(int)KnownColor.Orange] = Color.Orange;
            colors[(int)KnownColor.OrangeRed] = Color.OrangeRed;
            colors[(int)KnownColor.Orchid] = Color.Orchid;
            colors[(int)KnownColor.PaleGoldenrod] = Color.PaleGoldenrod;
            colors[(int)KnownColor.PaleGreen] = Color.PaleGreen;
            colors[(int)KnownColor.PaleTurquoise] = Color.PaleTurquoise;
            colors[(int)KnownColor.PaleVioletRed] = Color.PaleVioletRed;
            colors[(int)KnownColor.PapayaWhip] = Color.PapayaWhip;
            colors[(int)KnownColor.PeachPuff] = Color.PeachPuff;
            colors[(int)KnownColor.Peru] = Color.Peru;
            colors[(int)KnownColor.Pink] = Color.Pink;
            colors[(int)KnownColor.Plum] = Color.Plum;
            colors[(int)KnownColor.PowderBlue] = Color.PowderBlue;
            colors[(int)KnownColor.Purple] = Color.Purple;
            colors[(int)KnownColor.Red] = Color.Red;
            colors[(int)KnownColor.RosyBrown] = Color.RosyBrown;
            colors[(int)KnownColor.RoyalBlue] = Color.RoyalBlue;
            colors[(int)KnownColor.SaddleBrown] = Color.SaddleBrown;
            colors[(int)KnownColor.Salmon] = Color.Salmon;
            colors[(int)KnownColor.SandyBrown] = Color.SandyBrown;
            colors[(int)KnownColor.SeaGreen] = Color.SeaGreen;
            colors[(int)KnownColor.SeaShell] = Color.SeaShell;
            colors[(int)KnownColor.Sienna] = Color.Sienna;
            colors[(int)KnownColor.Silver] = Color.Silver;
            colors[(int)KnownColor.SkyBlue] = Color.SkyBlue;
            colors[(int)KnownColor.SlateBlue] = Color.SlateBlue;
            colors[(int)KnownColor.SlateGray] = Color.SlateGray;
            colors[(int)KnownColor.Snow] = Color.Snow;
            colors[(int)KnownColor.SpringGreen] = Color.SpringGreen;
            colors[(int)KnownColor.SteelBlue] = Color.SteelBlue;
            colors[(int)KnownColor.Tan] = Color.Tan;
            colors[(int)KnownColor.Teal] = Color.Teal;
            colors[(int)KnownColor.Thistle] = Color.Thistle;
            colors[(int)KnownColor.Tomato] = Color.Tomato;
            colors[(int)KnownColor.Turquoise] = Color.Turquoise;
            colors[(int)KnownColor.Violet] = Color.Violet;
            colors[(int)KnownColor.Wheat] = Color.Wheat;
            colors[(int)KnownColor.White] = Color.White;
            colors[(int)KnownColor.WhiteSmoke] = Color.WhiteSmoke;
            colors[(int)KnownColor.Yellow] = Color.Yellow;
            colors[(int)KnownColor.YellowGreen] = Color.YellowGreen;
            colors[(int)KnownColor.ButtonFace] = SystemColors.ButtonFace;
            colors[(int)KnownColor.ButtonHighlight] = SystemColors.ButtonHighlight;
            colors[(int)KnownColor.ButtonShadow] = SystemColors.ButtonShadow;
            colors[(int)KnownColor.GradientActiveCaption] = SystemColors.GradientActiveCaption;
            colors[(int)KnownColor.GradientInactiveCaption] = SystemColors.GradientInactiveCaption;
            colors[(int)KnownColor.MenuBar] = SystemColors.MenuBar;
            colors[(int)KnownColor.MenuHighlight] = SystemColors.MenuHighlight;
            colors[(int)KnownColor.RebeccaPurple] = Color.RebeccaPurple;
        }

        /// <summary>
        /// Retrieves the <see cref="Color"/> associated with
        /// the specified <see cref="KnownColor"/>.
        /// This method uses a cached collection of colors for improved performance.
        /// </summary>
        /// <param name="kc">A value from the <see cref="KnownColor"/>
        /// enumeration representing the color to retrieve.</param>
        /// <returns>The <see cref="Color"/> corresponding
        /// to the specified <paramref name="kc"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Get(KnownColor kc)
        {
            return colors[(int)kc];
        }
    }
}