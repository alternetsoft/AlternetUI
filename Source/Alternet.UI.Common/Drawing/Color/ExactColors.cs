using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of system-defined colors as <see cref="LightDarkColor"/> instances.
    /// </summary>
    public partial class ExactColors
    {
        /// <summary>
        /// Gets a <see cref="LightDarkColor"/> that is transparent.
        /// </summary>
        public static readonly LightDarkColor Transparent = new LightDarkColor(Color.Transparent).SetImmutable();

        /// <summary>
        /// Gets a <see cref="LightDarkColor"/> that is empty.
        /// </summary>
        public static readonly LightDarkColor Empty = new LightDarkColor(Color.Empty).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Yellow = new LightDarkColor(KnownColor.Yellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Green = new LightDarkColor(KnownColor.Green).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Blue = new LightDarkColor(KnownColor.Blue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Red = new LightDarkColor(KnownColor.Red).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray100 = new LightDarkColor(Color.FromUint(0xFFE1E1E1)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray200 = new LightDarkColor(Color.FromUint(0xFFC8C8C8)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray300 = new LightDarkColor(Color.FromUint(0xFFACACAC)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray400 = new LightDarkColor(Color.FromUint(0xFF919191)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray500 = new LightDarkColor(Color.FromUint(0xFF6E6E6E)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray600 = new LightDarkColor(Color.FromUint(0xFF404040)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray900 = new LightDarkColor(Color.FromUint(0xFF212121)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray950 = new LightDarkColor(Color.FromUint(0xFF141414)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Black = new LightDarkColor(KnownColor.Black).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor AliceBlue = new LightDarkColor(KnownColor.AliceBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor AntiqueWhite = new LightDarkColor(KnownColor.AntiqueWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Aqua = new LightDarkColor(KnownColor.Aqua).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Aquamarine = new LightDarkColor(KnownColor.Aquamarine).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Azure = new LightDarkColor(KnownColor.Azure).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Beige = new LightDarkColor(KnownColor.Beige).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Bisque = new LightDarkColor(KnownColor.Bisque).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor BlanchedAlmond = new LightDarkColor(KnownColor.BlanchedAlmond).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor BlueViolet = new LightDarkColor(KnownColor.BlueViolet).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Brown = new LightDarkColor(KnownColor.Brown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor BurlyWood = new LightDarkColor(KnownColor.BurlyWood).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor CadetBlue = new LightDarkColor(KnownColor.CadetBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Chartreuse = new LightDarkColor(KnownColor.Chartreuse).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Chocolate = new LightDarkColor(KnownColor.Chocolate).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Coral = new LightDarkColor(KnownColor.Coral).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor CornflowerBlue = new LightDarkColor(KnownColor.CornflowerBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Cornsilk = new LightDarkColor(KnownColor.Cornsilk).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Crimson = new LightDarkColor(KnownColor.Crimson).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Cyan = new LightDarkColor(KnownColor.Cyan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkBlue = new LightDarkColor(KnownColor.DarkBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkCyan = new LightDarkColor(KnownColor.DarkCyan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkGoldenrod = new LightDarkColor(KnownColor.DarkGoldenrod).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkGray = new LightDarkColor(KnownColor.DarkGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkGreen = new LightDarkColor(KnownColor.DarkGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkKhaki = new LightDarkColor(KnownColor.DarkKhaki).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkMagenta = new LightDarkColor(KnownColor.DarkMagenta).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkOliveGreen = new LightDarkColor(KnownColor.DarkOliveGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkOrange = new LightDarkColor(KnownColor.DarkOrange).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkOrchid = new LightDarkColor(KnownColor.DarkOrchid).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkRed = new LightDarkColor(KnownColor.DarkRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkSalmon = new LightDarkColor(KnownColor.DarkSalmon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkSeaGreen = new LightDarkColor(KnownColor.DarkSeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkSlateBlue = new LightDarkColor(KnownColor.DarkSlateBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkSlateGray = new LightDarkColor(KnownColor.DarkSlateGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkTurquoise = new LightDarkColor(KnownColor.DarkTurquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DarkViolet = new LightDarkColor(KnownColor.DarkViolet).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DeepPink = new LightDarkColor(KnownColor.DeepPink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DeepSkyBlue = new LightDarkColor(KnownColor.DeepSkyBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DimGray = new LightDarkColor(KnownColor.DimGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor DodgerBlue = new LightDarkColor(KnownColor.DodgerBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Firebrick = new LightDarkColor(KnownColor.Firebrick).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor FloralWhite = new LightDarkColor(KnownColor.FloralWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor ForestGreen = new LightDarkColor(KnownColor.ForestGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Fuchsia = new LightDarkColor(KnownColor.Fuchsia).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gainsboro = new LightDarkColor(KnownColor.Gainsboro).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor GhostWhite = new LightDarkColor(KnownColor.GhostWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gold = new LightDarkColor(KnownColor.Gold).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Goldenrod = new LightDarkColor(KnownColor.Goldenrod).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Gray = new LightDarkColor(KnownColor.Gray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor GreenYellow = new LightDarkColor(KnownColor.GreenYellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Honeydew = new LightDarkColor(KnownColor.Honeydew).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor HotPink = new LightDarkColor(KnownColor.HotPink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor IndianRed = new LightDarkColor(KnownColor.IndianRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Indigo = new LightDarkColor(KnownColor.Indigo).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Ivory = new LightDarkColor(KnownColor.Ivory).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Khaki = new LightDarkColor(KnownColor.Khaki).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Lavender = new LightDarkColor(KnownColor.Lavender).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LavenderBlush = new LightDarkColor(KnownColor.LavenderBlush).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LawnGreen = new LightDarkColor(KnownColor.LawnGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LemonChiffon = new LightDarkColor(KnownColor.LemonChiffon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightBlue = new LightDarkColor(KnownColor.LightBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightCoral = new LightDarkColor(KnownColor.LightCoral).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightCyan = new LightDarkColor(KnownColor.LightCyan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightGoldenrodYellow =
            new LightDarkColor(KnownColor.LightGoldenrodYellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightGreen = new LightDarkColor(KnownColor.LightGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightGray = new LightDarkColor(KnownColor.LightGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightPink = new LightDarkColor(KnownColor.LightPink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightSalmon = new LightDarkColor(KnownColor.LightSalmon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightSeaGreen = new LightDarkColor(KnownColor.LightSeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightSkyBlue = new LightDarkColor(KnownColor.LightSkyBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightSlateGray = new LightDarkColor(KnownColor.LightSlateGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightSteelBlue = new LightDarkColor(KnownColor.LightSteelBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LightYellow = new LightDarkColor(KnownColor.LightYellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Lime = new LightDarkColor(KnownColor.Lime).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor LimeGreen = new LightDarkColor(KnownColor.LimeGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Linen = new LightDarkColor(KnownColor.Linen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Magenta = new LightDarkColor(KnownColor.Magenta).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Maroon = new LightDarkColor(KnownColor.Maroon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumAquamarine = new LightDarkColor(KnownColor.MediumAquamarine).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumBlue = new LightDarkColor(KnownColor.MediumBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumOrchid = new LightDarkColor(KnownColor.MediumOrchid).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumPurple = new LightDarkColor(KnownColor.MediumPurple).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumSeaGreen = new LightDarkColor(KnownColor.MediumSeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumSlateBlue = new LightDarkColor(KnownColor.MediumSlateBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumSpringGreen = new LightDarkColor(KnownColor.MediumSpringGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumTurquoise = new LightDarkColor(KnownColor.MediumTurquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MediumVioletRed = new LightDarkColor(KnownColor.MediumVioletRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MidnightBlue = new LightDarkColor(KnownColor.MidnightBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MintCream = new LightDarkColor(KnownColor.MintCream).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor MistyRose = new LightDarkColor(KnownColor.MistyRose).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Moccasin = new LightDarkColor(KnownColor.Moccasin).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor NavajoWhite = new LightDarkColor(KnownColor.NavajoWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Navy = new LightDarkColor(KnownColor.Navy).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor OldLace = new LightDarkColor(KnownColor.OldLace).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Olive = new LightDarkColor(KnownColor.Olive).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor OliveDrab = new LightDarkColor(KnownColor.OliveDrab).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Orange = new LightDarkColor(KnownColor.Orange).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor OrangeRed = new LightDarkColor(KnownColor.OrangeRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Orchid = new LightDarkColor(KnownColor.Orchid).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PaleGoldenrod = new LightDarkColor(KnownColor.PaleGoldenrod).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PaleGreen = new LightDarkColor(KnownColor.PaleGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PaleTurquoise = new LightDarkColor(KnownColor.PaleTurquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PaleVioletRed = new LightDarkColor(KnownColor.PaleVioletRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PapayaWhip = new LightDarkColor(KnownColor.PapayaWhip).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PeachPuff = new LightDarkColor(KnownColor.PeachPuff).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Peru = new LightDarkColor(KnownColor.Peru).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Pink = new LightDarkColor(KnownColor.Pink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Plum = new LightDarkColor(KnownColor.Plum).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor PowderBlue = new LightDarkColor(KnownColor.PowderBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Purple = new LightDarkColor(KnownColor.Purple).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor RebeccaPurple = new LightDarkColor(KnownColor.RebeccaPurple).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor RosyBrown = new LightDarkColor(KnownColor.RosyBrown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor RoyalBlue = new LightDarkColor(KnownColor.RoyalBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SaddleBrown = new LightDarkColor(KnownColor.SaddleBrown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Salmon = new LightDarkColor(KnownColor.Salmon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SandyBrown = new LightDarkColor(KnownColor.SandyBrown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SeaGreen = new LightDarkColor(KnownColor.SeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SeaShell = new LightDarkColor(KnownColor.SeaShell).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Sienna = new LightDarkColor(KnownColor.Sienna).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Silver = new LightDarkColor(KnownColor.Silver).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SkyBlue = new LightDarkColor(KnownColor.SkyBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SlateBlue = new LightDarkColor(KnownColor.SlateBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SlateGray = new LightDarkColor(KnownColor.SlateGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Snow = new LightDarkColor(KnownColor.Snow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SpringGreen = new LightDarkColor(KnownColor.SpringGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor SteelBlue = new LightDarkColor(KnownColor.SteelBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Tan = new LightDarkColor(KnownColor.Tan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Teal = new LightDarkColor(KnownColor.Teal).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Thistle = new LightDarkColor(KnownColor.Thistle).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Tomato = new LightDarkColor(KnownColor.Tomato).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Turquoise = new LightDarkColor(KnownColor.Turquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Violet = new LightDarkColor(KnownColor.Violet).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor Wheat = new LightDarkColor(KnownColor.Wheat).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor WhiteSmoke = new LightDarkColor(KnownColor.WhiteSmoke).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly LightDarkColor YellowGreen = new LightDarkColor(KnownColor.YellowGreen).SetImmutable();

        /// <summary>
        /// Gets a <see cref="LightDarkColor"/> that is white.
        /// </summary>
        public static LightDarkColor White { get; } = new LightDarkColor(Color.White).SetImmutable();
    }
}
