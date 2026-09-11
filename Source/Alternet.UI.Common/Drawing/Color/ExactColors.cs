using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of system-defined colors as <see cref="ThemedColor"/> instances.
    /// </summary>
    public partial class ExactColors
    {
        /// <summary>
        /// Gets a <see cref="ThemedColor"/> that is transparent.
        /// </summary>
        public static readonly ThemedColor Transparent = new ThemedColor(Color.Transparent).SetImmutable();

        /// <summary>
        /// Gets a <see cref="ThemedColor"/> that is empty.
        /// </summary>
        public static readonly ThemedColor Empty = new ThemedColor(Color.Empty).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Yellow = new ThemedColor(KnownColor.Yellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Green = new ThemedColor(KnownColor.Green).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Blue = new ThemedColor(KnownColor.Blue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Red = new ThemedColor(KnownColor.Red).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray100 = new ThemedColor(Color.FromUint(0xFFE1E1E1)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray200 = new ThemedColor(Color.FromUint(0xFFC8C8C8)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray300 = new ThemedColor(Color.FromUint(0xFFACACAC)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray400 = new ThemedColor(Color.FromUint(0xFF919191)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray500 = new ThemedColor(Color.FromUint(0xFF6E6E6E)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray600 = new ThemedColor(Color.FromUint(0xFF404040)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray900 = new ThemedColor(Color.FromUint(0xFF212121)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray950 = new ThemedColor(Color.FromUint(0xFF141414)).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Black = new ThemedColor(KnownColor.Black).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor AliceBlue = new ThemedColor(KnownColor.AliceBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor AntiqueWhite = new ThemedColor(KnownColor.AntiqueWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Aqua = new ThemedColor(KnownColor.Aqua).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Aquamarine = new ThemedColor(KnownColor.Aquamarine).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Azure = new ThemedColor(KnownColor.Azure).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Beige = new ThemedColor(KnownColor.Beige).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Bisque = new ThemedColor(KnownColor.Bisque).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor BlanchedAlmond = new ThemedColor(KnownColor.BlanchedAlmond).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor BlueViolet = new ThemedColor(KnownColor.BlueViolet).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Brown = new ThemedColor(KnownColor.Brown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor BurlyWood = new ThemedColor(KnownColor.BurlyWood).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor CadetBlue = new ThemedColor(KnownColor.CadetBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Chartreuse = new ThemedColor(KnownColor.Chartreuse).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Chocolate = new ThemedColor(KnownColor.Chocolate).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Coral = new ThemedColor(KnownColor.Coral).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor CornflowerBlue = new ThemedColor(KnownColor.CornflowerBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Cornsilk = new ThemedColor(KnownColor.Cornsilk).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Crimson = new ThemedColor(KnownColor.Crimson).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Cyan = new ThemedColor(KnownColor.Cyan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkBlue = new ThemedColor(KnownColor.DarkBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkCyan = new ThemedColor(KnownColor.DarkCyan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkGoldenrod = new ThemedColor(KnownColor.DarkGoldenrod).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkGray = new ThemedColor(KnownColor.DarkGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkGreen = new ThemedColor(KnownColor.DarkGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkKhaki = new ThemedColor(KnownColor.DarkKhaki).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkMagenta = new ThemedColor(KnownColor.DarkMagenta).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkOliveGreen = new ThemedColor(KnownColor.DarkOliveGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkOrange = new ThemedColor(KnownColor.DarkOrange).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkOrchid = new ThemedColor(KnownColor.DarkOrchid).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkRed = new ThemedColor(KnownColor.DarkRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkSalmon = new ThemedColor(KnownColor.DarkSalmon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkSeaGreen = new ThemedColor(KnownColor.DarkSeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkSlateBlue = new ThemedColor(KnownColor.DarkSlateBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkSlateGray = new ThemedColor(KnownColor.DarkSlateGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkTurquoise = new ThemedColor(KnownColor.DarkTurquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DarkViolet = new ThemedColor(KnownColor.DarkViolet).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DeepPink = new ThemedColor(KnownColor.DeepPink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DeepSkyBlue = new ThemedColor(KnownColor.DeepSkyBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DimGray = new ThemedColor(KnownColor.DimGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor DodgerBlue = new ThemedColor(KnownColor.DodgerBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Firebrick = new ThemedColor(KnownColor.Firebrick).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor FloralWhite = new ThemedColor(KnownColor.FloralWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor ForestGreen = new ThemedColor(KnownColor.ForestGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Fuchsia = new ThemedColor(KnownColor.Fuchsia).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gainsboro = new ThemedColor(KnownColor.Gainsboro).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor GhostWhite = new ThemedColor(KnownColor.GhostWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gold = new ThemedColor(KnownColor.Gold).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Goldenrod = new ThemedColor(KnownColor.Goldenrod).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Gray = new ThemedColor(KnownColor.Gray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor GreenYellow = new ThemedColor(KnownColor.GreenYellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Honeydew = new ThemedColor(KnownColor.Honeydew).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor HotPink = new ThemedColor(KnownColor.HotPink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor IndianRed = new ThemedColor(KnownColor.IndianRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Indigo = new ThemedColor(KnownColor.Indigo).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Ivory = new ThemedColor(KnownColor.Ivory).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Khaki = new ThemedColor(KnownColor.Khaki).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Lavender = new ThemedColor(KnownColor.Lavender).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LavenderBlush = new ThemedColor(KnownColor.LavenderBlush).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LawnGreen = new ThemedColor(KnownColor.LawnGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LemonChiffon = new ThemedColor(KnownColor.LemonChiffon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightBlue = new ThemedColor(KnownColor.LightBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightCoral = new ThemedColor(KnownColor.LightCoral).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightCyan = new ThemedColor(KnownColor.LightCyan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightGoldenrodYellow =
            new ThemedColor(KnownColor.LightGoldenrodYellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightGreen = new ThemedColor(KnownColor.LightGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightGray = new ThemedColor(KnownColor.LightGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightPink = new ThemedColor(KnownColor.LightPink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightSalmon = new ThemedColor(KnownColor.LightSalmon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightSeaGreen = new ThemedColor(KnownColor.LightSeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightSkyBlue = new ThemedColor(KnownColor.LightSkyBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightSlateGray = new ThemedColor(KnownColor.LightSlateGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightSteelBlue = new ThemedColor(KnownColor.LightSteelBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LightYellow = new ThemedColor(KnownColor.LightYellow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Lime = new ThemedColor(KnownColor.Lime).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor LimeGreen = new ThemedColor(KnownColor.LimeGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Linen = new ThemedColor(KnownColor.Linen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Magenta = new ThemedColor(KnownColor.Magenta).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Maroon = new ThemedColor(KnownColor.Maroon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumAquamarine = new ThemedColor(KnownColor.MediumAquamarine).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumBlue = new ThemedColor(KnownColor.MediumBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumOrchid = new ThemedColor(KnownColor.MediumOrchid).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumPurple = new ThemedColor(KnownColor.MediumPurple).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumSeaGreen = new ThemedColor(KnownColor.MediumSeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumSlateBlue = new ThemedColor(KnownColor.MediumSlateBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumSpringGreen = new ThemedColor(KnownColor.MediumSpringGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumTurquoise = new ThemedColor(KnownColor.MediumTurquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MediumVioletRed = new ThemedColor(KnownColor.MediumVioletRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MidnightBlue = new ThemedColor(KnownColor.MidnightBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MintCream = new ThemedColor(KnownColor.MintCream).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor MistyRose = new ThemedColor(KnownColor.MistyRose).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Moccasin = new ThemedColor(KnownColor.Moccasin).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor NavajoWhite = new ThemedColor(KnownColor.NavajoWhite).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Navy = new ThemedColor(KnownColor.Navy).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor OldLace = new ThemedColor(KnownColor.OldLace).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Olive = new ThemedColor(KnownColor.Olive).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor OliveDrab = new ThemedColor(KnownColor.OliveDrab).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Orange = new ThemedColor(KnownColor.Orange).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor OrangeRed = new ThemedColor(KnownColor.OrangeRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Orchid = new ThemedColor(KnownColor.Orchid).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PaleGoldenrod = new ThemedColor(KnownColor.PaleGoldenrod).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PaleGreen = new ThemedColor(KnownColor.PaleGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PaleTurquoise = new ThemedColor(KnownColor.PaleTurquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PaleVioletRed = new ThemedColor(KnownColor.PaleVioletRed).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PapayaWhip = new ThemedColor(KnownColor.PapayaWhip).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PeachPuff = new ThemedColor(KnownColor.PeachPuff).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Peru = new ThemedColor(KnownColor.Peru).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Pink = new ThemedColor(KnownColor.Pink).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Plum = new ThemedColor(KnownColor.Plum).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor PowderBlue = new ThemedColor(KnownColor.PowderBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Purple = new ThemedColor(KnownColor.Purple).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor RebeccaPurple = new ThemedColor(KnownColor.RebeccaPurple).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor RosyBrown = new ThemedColor(KnownColor.RosyBrown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor RoyalBlue = new ThemedColor(KnownColor.RoyalBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SaddleBrown = new ThemedColor(KnownColor.SaddleBrown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Salmon = new ThemedColor(KnownColor.Salmon).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SandyBrown = new ThemedColor(KnownColor.SandyBrown).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SeaGreen = new ThemedColor(KnownColor.SeaGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SeaShell = new ThemedColor(KnownColor.SeaShell).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Sienna = new ThemedColor(KnownColor.Sienna).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Silver = new ThemedColor(KnownColor.Silver).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SkyBlue = new ThemedColor(KnownColor.SkyBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SlateBlue = new ThemedColor(KnownColor.SlateBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SlateGray = new ThemedColor(KnownColor.SlateGray).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Snow = new ThemedColor(KnownColor.Snow).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SpringGreen = new ThemedColor(KnownColor.SpringGreen).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor SteelBlue = new ThemedColor(KnownColor.SteelBlue).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Tan = new ThemedColor(KnownColor.Tan).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Teal = new ThemedColor(KnownColor.Teal).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Thistle = new ThemedColor(KnownColor.Thistle).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Tomato = new ThemedColor(KnownColor.Tomato).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Turquoise = new ThemedColor(KnownColor.Turquoise).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Violet = new ThemedColor(KnownColor.Violet).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor Wheat = new ThemedColor(KnownColor.Wheat).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor WhiteSmoke = new ThemedColor(KnownColor.WhiteSmoke).SetImmutable();

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly ThemedColor YellowGreen = new ThemedColor(KnownColor.YellowGreen).SetImmutable();

        /// <summary>
        /// Gets a <see cref="ThemedColor"/> that is white.
        /// </summary>
        public static ThemedColor White { get; } = new ThemedColor(Color.White).SetImmutable();
    }
}
