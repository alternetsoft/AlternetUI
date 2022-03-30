namespace Alternet.Drawing
{
    /// <summary>
    /// Pens for all the standard colors.
    /// </summary>
    /// <remarks>
    /// The <see cref="Pens"/> class contains static read-only properties that
    /// return a <see cref="Pen"/> object of the color indicated by the property name, with a width of 1.
    /// </remarks>
    public static class Pens
    {
        private static Pen? transparent;
        private static Pen? aliceBlue;
        private static Pen? antiqueWhite;
        private static Pen? aqua;
        private static Pen? aquamarine;
        private static Pen? azure;
        private static Pen? beige;
        private static Pen? bisque;
        private static Pen? black;
        private static Pen? blanchedAlmond;
        private static Pen? blue;
        private static Pen? blueViolet;
        private static Pen? brown;
        private static Pen? burlyWood;
        private static Pen? cadetBlue;
        private static Pen? chartreuse;
        private static Pen? chocolate;
        private static Pen? coral;
        private static Pen? cornflowerBlue;
        private static Pen? cornsilk;
        private static Pen? crimson;
        private static Pen? cyan;
        private static Pen? darkBlue;
        private static Pen? darkCyan;
        private static Pen? darkGoldenrod;
        private static Pen? darkGray;
        private static Pen? darkGreen;
        private static Pen? darkKhaki;
        private static Pen? darkMagenta;
        private static Pen? darkOliveGreen;
        private static Pen? darkOrange;
        private static Pen? darkOrchid;
        private static Pen? darkRed;
        private static Pen? darkSalmon;
        private static Pen? darkSeaGreen;
        private static Pen? darkSlateBlue;
        private static Pen? darkSlateGray;
        private static Pen? darkTurquoise;
        private static Pen? darkViolet;
        private static Pen? deepPink;
        private static Pen? deepSkyBlue;
        private static Pen? dimGray;
        private static Pen? dodgerBlue;
        private static Pen? firebrick;
        private static Pen? floralWhite;
        private static Pen? forestGreen;
        private static Pen? fuchsia;
        private static Pen? gainsboro;
        private static Pen? ghostWhite;
        private static Pen? gold;
        private static Pen? goldenrod;
        private static Pen? gray;
        private static Pen? green;
        private static Pen? greenYellow;
        private static Pen? honeydew;
        private static Pen? hotPink;
        private static Pen? indianRed;
        private static Pen? indigo;
        private static Pen? ivory;
        private static Pen? khaki;
        private static Pen? lavender;
        private static Pen? lavenderBlush;
        private static Pen? lawnGreen;
        private static Pen? lemonChiffon;
        private static Pen? lightBlue;
        private static Pen? lightCoral;
        private static Pen? lightCyan;
        private static Pen? lightGoldenrodYellow;
        private static Pen? lightGreen;
        private static Pen? lightGray;
        private static Pen? lightPink;
        private static Pen? lightSalmon;
        private static Pen? lightSeaGreen;
        private static Pen? lightSkyBlue;
        private static Pen? lightSlateGray;
        private static Pen? lightSteelBlue;
        private static Pen? lightYellow;
        private static Pen? lime;
        private static Pen? limeGreen;
        private static Pen? linen;
        private static Pen? magenta;
        private static Pen? maroon;
        private static Pen? mediumAquamarine;
        private static Pen? mediumBlue;
        private static Pen? mediumOrchid;
        private static Pen? mediumPurple;
        private static Pen? mediumSeaGreen;
        private static Pen? mediumSlateBlue;
        private static Pen? mediumSpringGreen;
        private static Pen? mediumTurquoise;
        private static Pen? mediumVioletRed;
        private static Pen? midnightBlue;
        private static Pen? mintCream;
        private static Pen? mistyRose;
        private static Pen? moccasin;
        private static Pen? navajoWhite;
        private static Pen? navy;
        private static Pen? oldLace;
        private static Pen? olive;
        private static Pen? oliveDrab;
        private static Pen? orange;
        private static Pen? orangeRed;
        private static Pen? orchid;
        private static Pen? paleGoldenrod;
        private static Pen? paleGreen;
        private static Pen? paleTurquoise;
        private static Pen? paleVioletRed;
        private static Pen? papayaWhip;
        private static Pen? peachPuff;
        private static Pen? peru;
        private static Pen? pink;
        private static Pen? plum;
        private static Pen? powderBlue;
        private static Pen? purple;
        private static Pen? red;
        private static Pen? rosyBrown;
        private static Pen? royalBlue;
        private static Pen? saddleBrown;
        private static Pen? salmon;
        private static Pen? sandyBrown;
        private static Pen? seaGreen;
        private static Pen? seaShell;
        private static Pen? sienna;
        private static Pen? silver;
        private static Pen? skyBlue;
        private static Pen? slateBlue;
        private static Pen? slateGray;
        private static Pen? snow;
        private static Pen? springGreen;
        private static Pen? steelBlue;
        private static Pen? tan;
        private static Pen? teal;
        private static Pen? thistle;
        private static Pen? tomato;
        private static Pen? turquoise;
        private static Pen? violet;
        private static Pen? wheat;
        private static Pen? white;
        private static Pen? whiteSmoke;
        private static Pen? yellow;
        private static Pen? yellowGreen;

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Transparent => GetPen(ref transparent, Color.Transparent);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen AliceBlue => GetPen(ref aliceBlue, Color.AliceBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen AntiqueWhite => GetPen(ref antiqueWhite, Color.AntiqueWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Aqua => GetPen(ref aqua, Color.Aqua);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Aquamarine => GetPen(ref aquamarine, Color.Aquamarine);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Azure => GetPen(ref azure, Color.Azure);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Beige => GetPen(ref beige, Color.Beige);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Bisque => GetPen(ref bisque, Color.Bisque);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Black => GetPen(ref black, Color.Black);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen BlanchedAlmond => GetPen(ref blanchedAlmond, Color.BlanchedAlmond);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Blue => GetPen(ref blue, Color.Blue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen BlueViolet => GetPen(ref blueViolet, Color.BlueViolet);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Brown => GetPen(ref brown, Color.Brown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen BurlyWood => GetPen(ref burlyWood, Color.BurlyWood);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen CadetBlue => GetPen(ref cadetBlue, Color.CadetBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Chartreuse => GetPen(ref chartreuse, Color.Chartreuse);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Chocolate => GetPen(ref chocolate, Color.Chocolate);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Coral => GetPen(ref coral, Color.Coral);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen CornflowerBlue => GetPen(ref cornflowerBlue, Color.CornflowerBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Cornsilk => GetPen(ref cornsilk, Color.Cornsilk);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Crimson => GetPen(ref crimson, Color.Crimson);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Cyan => GetPen(ref cyan, Color.Cyan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkBlue => GetPen(ref darkBlue, Color.DarkBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkCyan => GetPen(ref darkCyan, Color.DarkCyan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkGoldenrod => GetPen(ref darkGoldenrod, Color.DarkGoldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkGray => GetPen(ref darkGray, Color.DarkGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkGreen => GetPen(ref darkGreen, Color.DarkGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkKhaki => GetPen(ref darkKhaki, Color.DarkKhaki);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkMagenta => GetPen(ref darkMagenta, Color.DarkMagenta);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkOliveGreen => GetPen(ref darkOliveGreen, Color.DarkOliveGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkOrange => GetPen(ref darkOrange, Color.DarkOrange);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkOrchid => GetPen(ref darkOrchid, Color.DarkOrchid);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkRed => GetPen(ref darkRed, Color.DarkRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSalmon => GetPen(ref darkSalmon, Color.DarkSalmon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSeaGreen => GetPen(ref darkSeaGreen, Color.DarkSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSlateBlue => GetPen(ref darkSlateBlue, Color.DarkSlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkSlateGray => GetPen(ref darkSlateGray, Color.DarkSlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkTurquoise => GetPen(ref darkTurquoise, Color.DarkTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DarkViolet => GetPen(ref darkViolet, Color.DarkViolet);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DeepPink => GetPen(ref deepPink, Color.DeepPink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DeepSkyBlue => GetPen(ref deepSkyBlue, Color.DeepSkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DimGray => GetPen(ref dimGray, Color.DimGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen DodgerBlue => GetPen(ref dodgerBlue, Color.DodgerBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Firebrick => GetPen(ref firebrick, Color.Firebrick);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen FloralWhite => GetPen(ref floralWhite, Color.FloralWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen ForestGreen => GetPen(ref forestGreen, Color.ForestGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Fuchsia => GetPen(ref fuchsia, Color.Fuchsia);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Gainsboro => GetPen(ref gainsboro, Color.Gainsboro);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen GhostWhite => GetPen(ref ghostWhite, Color.GhostWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Gold => GetPen(ref gold, Color.Gold);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Goldenrod => GetPen(ref goldenrod, Color.Goldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Gray => GetPen(ref gray, Color.Gray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Green => GetPen(ref green, Color.Green);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen GreenYellow => GetPen(ref greenYellow, Color.GreenYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Honeydew => GetPen(ref honeydew, Color.Honeydew);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen HotPink => GetPen(ref hotPink, Color.HotPink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen IndianRed => GetPen(ref indianRed, Color.IndianRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Indigo => GetPen(ref indigo, Color.Indigo);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Ivory => GetPen(ref ivory, Color.Ivory);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Khaki => GetPen(ref khaki, Color.Khaki);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Lavender => GetPen(ref lavender, Color.Lavender);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LavenderBlush => GetPen(ref lavenderBlush, Color.LavenderBlush);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LawnGreen => GetPen(ref lawnGreen, Color.LawnGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LemonChiffon => GetPen(ref lemonChiffon, Color.LemonChiffon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightBlue => GetPen(ref lightBlue, Color.LightBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightCoral => GetPen(ref lightCoral, Color.LightCoral);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightCyan => GetPen(ref lightCyan, Color.LightCyan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightGoldenrodYellow => GetPen(ref lightGoldenrodYellow, Color.LightGoldenrodYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightGreen => GetPen(ref lightGreen, Color.LightGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightGray => GetPen(ref lightGray, Color.LightGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightPink => GetPen(ref lightPink, Color.LightPink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSalmon => GetPen(ref lightSalmon, Color.LightSalmon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSeaGreen => GetPen(ref lightSeaGreen, Color.LightSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSkyBlue => GetPen(ref lightSkyBlue, Color.LightSkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSlateGray => GetPen(ref lightSlateGray, Color.LightSlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightSteelBlue => GetPen(ref lightSteelBlue, Color.LightSteelBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LightYellow => GetPen(ref lightYellow, Color.LightYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Lime => GetPen(ref lime, Color.Lime);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen LimeGreen => GetPen(ref limeGreen, Color.LimeGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Linen => GetPen(ref linen, Color.Linen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Magenta => GetPen(ref magenta, Color.Magenta);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Maroon => GetPen(ref maroon, Color.Maroon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumAquamarine => GetPen(ref mediumAquamarine, Color.MediumAquamarine);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumBlue => GetPen(ref mediumBlue, Color.MediumBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumOrchid => GetPen(ref mediumOrchid, Color.MediumOrchid);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumPurple => GetPen(ref mediumPurple, Color.MediumPurple);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumSeaGreen => GetPen(ref mediumSeaGreen, Color.MediumSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumSlateBlue => GetPen(ref mediumSlateBlue, Color.MediumSlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumSpringGreen => GetPen(ref mediumSpringGreen, Color.MediumSpringGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumTurquoise => GetPen(ref mediumTurquoise, Color.MediumTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MediumVioletRed => GetPen(ref mediumVioletRed, Color.MediumVioletRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MidnightBlue => GetPen(ref midnightBlue, Color.MidnightBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MintCream => GetPen(ref mintCream, Color.MintCream);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen MistyRose => GetPen(ref mistyRose, Color.MistyRose);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Moccasin => GetPen(ref moccasin, Color.Moccasin);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen NavajoWhite => GetPen(ref navajoWhite, Color.NavajoWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Navy => GetPen(ref navy, Color.Navy);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen OldLace => GetPen(ref oldLace, Color.OldLace);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Olive => GetPen(ref olive, Color.Olive);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen OliveDrab => GetPen(ref oliveDrab, Color.OliveDrab);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Orange => GetPen(ref orange, Color.Orange);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen OrangeRed => GetPen(ref orangeRed, Color.OrangeRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Orchid => GetPen(ref orchid, Color.Orchid);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleGoldenrod => GetPen(ref paleGoldenrod, Color.PaleGoldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleGreen => GetPen(ref paleGreen, Color.PaleGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleTurquoise => GetPen(ref paleTurquoise, Color.PaleTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PaleVioletRed => GetPen(ref paleVioletRed, Color.PaleVioletRed);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PapayaWhip => GetPen(ref papayaWhip, Color.PapayaWhip);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PeachPuff => GetPen(ref peachPuff, Color.PeachPuff);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Peru => GetPen(ref peru, Color.Peru);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Pink => GetPen(ref pink, Color.Pink);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Plum => GetPen(ref plum, Color.Plum);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen PowderBlue => GetPen(ref powderBlue, Color.PowderBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Purple => GetPen(ref purple, Color.Purple);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Red => GetPen(ref red, Color.Red);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen RosyBrown => GetPen(ref rosyBrown, Color.RosyBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen RoyalBlue => GetPen(ref royalBlue, Color.RoyalBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SaddleBrown => GetPen(ref saddleBrown, Color.SaddleBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Salmon => GetPen(ref salmon, Color.Salmon);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SandyBrown => GetPen(ref sandyBrown, Color.SandyBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SeaGreen => GetPen(ref seaGreen, Color.SeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SeaShell => GetPen(ref seaShell, Color.SeaShell);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Sienna => GetPen(ref sienna, Color.Sienna);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Silver => GetPen(ref silver, Color.Silver);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SkyBlue => GetPen(ref skyBlue, Color.SkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SlateBlue => GetPen(ref slateBlue, Color.SlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SlateGray => GetPen(ref slateGray, Color.SlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Snow => GetPen(ref snow, Color.Snow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SpringGreen => GetPen(ref springGreen, Color.SpringGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen SteelBlue => GetPen(ref steelBlue, Color.SteelBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Tan => GetPen(ref tan, Color.Tan);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Teal => GetPen(ref teal, Color.Teal);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Thistle => GetPen(ref thistle, Color.Thistle);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Tomato => GetPen(ref tomato, Color.Tomato);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Turquoise => GetPen(ref turquoise, Color.Turquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Violet => GetPen(ref violet, Color.Violet);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Wheat => GetPen(ref wheat, Color.Wheat);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen White => GetPen(ref white, Color.White);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen WhiteSmoke => GetPen(ref whiteSmoke, Color.WhiteSmoke);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen Yellow => GetPen(ref yellow, Color.Yellow);

        /// <summary>
        /// Gets a system-defined <see cref="Pen"/> object with a width of 1.
        /// </summary>
        /// <remarks>
        /// A <see cref="Pen"/> object set to a system-defined color.
        /// </remarks>
        public static Pen YellowGreen => GetPen(ref yellowGreen, Color.YellowGreen);

        private static Pen GetPen(ref Pen? pen, Color color)
        {
            if (pen == null)
                pen = new Pen(color, 1, PenDashStyle.Solid, immutable: true);
            return pen;
        }
    }
}