using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Alternet.Drawing
{
    /// <summary>
    /// Pens for all the standard colors.
    /// </summary>
    /// <remarks>
    /// The <see cref="Pens"/> class contains static read-only properties that
    /// return a <see cref="Pen"/> object of the color indicated by the property name,
    /// with a width of 1.
    /// </remarks>
    public static class Pens
    {
        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Transparent => GetPen(Color.Transparent);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen AliceBlue => GetPen(Color.AliceBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen AntiqueWhite => GetPen(Color.AntiqueWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Aqua => GetPen(Color.Aqua);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Aquamarine => GetPen(Color.Aquamarine);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Azure => GetPen(Color.Azure);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Beige => GetPen(Color.Beige);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Bisque => GetPen(Color.Bisque);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Black => GetPen(Color.Black);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen BlanchedAlmond => GetPen(Color.BlanchedAlmond);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Blue => GetPen(Color.Blue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen BlueViolet => GetPen(Color.BlueViolet);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Brown => GetPen(Color.Brown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen BurlyWood => GetPen(Color.BurlyWood);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen CadetBlue => GetPen(Color.CadetBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Chartreuse => GetPen(Color.Chartreuse);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Chocolate => GetPen(Color.Chocolate);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Coral => GetPen(Color.Coral);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen CornflowerBlue => GetPen(Color.CornflowerBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Cornsilk => GetPen(Color.Cornsilk);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Crimson => GetPen(Color.Crimson);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Cyan => GetPen(Color.Cyan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkBlue => GetPen(Color.DarkBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkCyan => GetPen(Color.DarkCyan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkGoldenrod => GetPen(Color.DarkGoldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkGray => GetPen(Color.DarkGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkGreen => GetPen(Color.DarkGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkKhaki => GetPen(Color.DarkKhaki);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkMagenta => GetPen(Color.DarkMagenta);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkOliveGreen => GetPen(Color.DarkOliveGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkOrange => GetPen(Color.DarkOrange);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkOrchid => GetPen(Color.DarkOrchid);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkRed => GetPen(Color.DarkRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSalmon => GetPen(Color.DarkSalmon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSeaGreen => GetPen(Color.DarkSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSlateBlue => GetPen(Color.DarkSlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSlateGray => GetPen(Color.DarkSlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkTurquoise => GetPen(Color.DarkTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkViolet => GetPen(Color.DarkViolet);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DeepPink => GetPen(Color.DeepPink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DeepSkyBlue => GetPen(Color.DeepSkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DimGray => GetPen(Color.DimGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DodgerBlue => GetPen(Color.DodgerBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Firebrick => GetPen(Color.Firebrick);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen FloralWhite => GetPen(Color.FloralWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen ForestGreen => GetPen(Color.ForestGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Fuchsia => GetPen(Color.Fuchsia);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Gainsboro => GetPen(Color.Gainsboro);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen GhostWhite => GetPen(Color.GhostWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Gold => GetPen(Color.Gold);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Goldenrod => GetPen(Color.Goldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Gray => GetPen(Color.Gray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Green => GetPen(Color.Green);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen GreenYellow => GetPen(Color.GreenYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Honeydew => GetPen(Color.Honeydew);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen HotPink => GetPen(Color.HotPink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen IndianRed => GetPen(Color.IndianRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Indigo => GetPen(Color.Indigo);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Ivory => GetPen(Color.Ivory);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Khaki => GetPen(Color.Khaki);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Lavender => GetPen(Color.Lavender);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LavenderBlush => GetPen(Color.LavenderBlush);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LawnGreen => GetPen(Color.LawnGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LemonChiffon => GetPen(Color.LemonChiffon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightBlue => GetPen(Color.LightBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightCoral => GetPen(Color.LightCoral);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightCyan => GetPen(Color.LightCyan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightGoldenrodYellow => GetPen(Color.LightGoldenrodYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightGreen => GetPen(Color.LightGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightGray => GetPen(Color.LightGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightPink => GetPen(Color.LightPink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSalmon => GetPen(Color.LightSalmon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSeaGreen => GetPen(Color.LightSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSkyBlue => GetPen(Color.LightSkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSlateGray => GetPen(Color.LightSlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSteelBlue => GetPen(Color.LightSteelBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightYellow => GetPen(Color.LightYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Lime => GetPen(Color.Lime);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LimeGreen => GetPen(Color.LimeGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Linen => GetPen(Color.Linen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Magenta => GetPen(Color.Magenta);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Maroon => GetPen(Color.Maroon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumAquamarine => GetPen(Color.MediumAquamarine);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumBlue => GetPen(Color.MediumBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumOrchid => GetPen(Color.MediumOrchid);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumPurple => GetPen(Color.MediumPurple);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumSeaGreen => GetPen(Color.MediumSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumSlateBlue => GetPen(Color.MediumSlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumSpringGreen => GetPen(Color.MediumSpringGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumTurquoise => GetPen(Color.MediumTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumVioletRed => GetPen(Color.MediumVioletRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MidnightBlue => GetPen(Color.MidnightBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MintCream => GetPen(Color.MintCream);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MistyRose => GetPen(Color.MistyRose);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Moccasin => GetPen(Color.Moccasin);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen NavajoWhite => GetPen(Color.NavajoWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Navy => GetPen(Color.Navy);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen OldLace => GetPen(Color.OldLace);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Olive => GetPen(Color.Olive);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen OliveDrab => GetPen(Color.OliveDrab);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Orange => GetPen(Color.Orange);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen OrangeRed => GetPen(Color.OrangeRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Orchid => GetPen(Color.Orchid);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleGoldenrod => GetPen(Color.PaleGoldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleGreen => GetPen(Color.PaleGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleTurquoise => GetPen(Color.PaleTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleVioletRed => GetPen(Color.PaleVioletRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PapayaWhip => GetPen(Color.PapayaWhip);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PeachPuff => GetPen(Color.PeachPuff);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Peru => GetPen(Color.Peru);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Pink => GetPen(Color.Pink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Plum => GetPen(Color.Plum);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PowderBlue => GetPen(Color.PowderBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Purple => GetPen(Color.Purple);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Red => GetPen(Color.Red);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen RosyBrown => GetPen(Color.RosyBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen RoyalBlue => GetPen(Color.RoyalBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SaddleBrown => GetPen(Color.SaddleBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Salmon => GetPen(Color.Salmon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SandyBrown => GetPen(Color.SandyBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SeaGreen => GetPen(Color.SeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SeaShell => GetPen(Color.SeaShell);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Sienna => GetPen(Color.Sienna);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Silver => GetPen(Color.Silver);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SkyBlue => GetPen(Color.SkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SlateBlue => GetPen(Color.SlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SlateGray => GetPen(Color.SlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Snow => GetPen(Color.Snow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SpringGreen => GetPen(Color.SpringGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SteelBlue => GetPen(Color.SteelBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Tan => GetPen(Color.Tan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Teal => GetPen(Color.Teal);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Thistle => GetPen(Color.Thistle);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Tomato => GetPen(Color.Tomato);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Turquoise => GetPen(Color.Turquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Violet => GetPen(Color.Violet);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Wheat => GetPen(Color.Wheat);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen White => GetPen(Color.White);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen WhiteSmoke => GetPen(Color.WhiteSmoke);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Yellow => GetPen(Color.Yellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen YellowGreen => GetPen(Color.YellowGreen);

        /// <summary>
        /// Gets a pen by its name.
        /// </summary>
        /// <exception cref="ArgumentException">The pen was not found.</exception>
        public static Pen GetPen(string name)
        {
            var b = TryGetPen(name) ?? throw new ArgumentException(
                    "Cannot find a pen with the name: " + name,
                    nameof(name));
            return b;
        }

        /// <summary>
        /// Tries to get a pen by its name.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Pen? TryGetPen(string name)
        {
            return NamedColors.GetColorOrNull(name)?.AsPen;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Pen GetPen(Color color)
        {
            return color.AsPen;
        }
    }
}