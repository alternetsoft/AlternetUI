using Alternet.Drawing;

namespace Alternet.Drawing
{
    /// <summary>
    /// Brushes for all the standard colors.
    /// </summary>
    /// <remarks>
    /// The <see cref="Brushes"/> class contains static read-only properties that return a <see cref="Brush"/> object of the color indicated by the property name.
    /// </remarks>
    public static class Brushes
    {
        private static Brush? transparent;
        private static Brush? aliceBlue;
        private static Brush? antiqueWhite;
        private static Brush? aqua;
        private static Brush? aquamarine;
        private static Brush? azure;
        private static Brush? beige;
        private static Brush? bisque;
        private static Brush? black;
        private static Brush? blanchedAlmond;
        private static Brush? blue;
        private static Brush? blueViolet;
        private static Brush? brown;
        private static Brush? burlyWood;
        private static Brush? cadetBlue;
        private static Brush? chartreuse;
        private static Brush? chocolate;
        private static Brush? coral;
        private static Brush? cornflowerBlue;
        private static Brush? cornsilk;
        private static Brush? crimson;
        private static Brush? cyan;
        private static Brush? darkBlue;
        private static Brush? darkCyan;
        private static Brush? darkGoldenrod;
        private static Brush? darkGray;
        private static Brush? darkGreen;
        private static Brush? darkKhaki;
        private static Brush? darkMagenta;
        private static Brush? darkOliveGreen;
        private static Brush? darkOrange;
        private static Brush? darkOrchid;
        private static Brush? darkRed;
        private static Brush? darkSalmon;
        private static Brush? darkSeaGreen;
        private static Brush? darkSlateBlue;
        private static Brush? darkSlateGray;
        private static Brush? darkTurquoise;
        private static Brush? darkViolet;
        private static Brush? deepPink;
        private static Brush? deepSkyBlue;
        private static Brush? dimGray;
        private static Brush? dodgerBlue;
        private static Brush? firebrick;
        private static Brush? floralWhite;
        private static Brush? forestGreen;
        private static Brush? fuchsia;
        private static Brush? gainsboro;
        private static Brush? ghostWhite;
        private static Brush? gold;
        private static Brush? goldenrod;
        private static Brush? gray;
        private static Brush? green;
        private static Brush? greenYellow;
        private static Brush? honeydew;
        private static Brush? hotPink;
        private static Brush? indianRed;
        private static Brush? indigo;
        private static Brush? ivory;
        private static Brush? khaki;
        private static Brush? lavender;
        private static Brush? lavenderBlush;
        private static Brush? lawnGreen;
        private static Brush? lemonChiffon;
        private static Brush? lightBlue;
        private static Brush? lightCoral;
        private static Brush? lightCyan;
        private static Brush? lightGoldenrodYellow;
        private static Brush? lightGreen;
        private static Brush? lightGray;
        private static Brush? lightPink;
        private static Brush? lightSalmon;
        private static Brush? lightSeaGreen;
        private static Brush? lightSkyBlue;
        private static Brush? lightSlateGray;
        private static Brush? lightSteelBlue;
        private static Brush? lightYellow;
        private static Brush? lime;
        private static Brush? limeGreen;
        private static Brush? linen;
        private static Brush? magenta;
        private static Brush? maroon;
        private static Brush? mediumAquamarine;
        private static Brush? mediumBlue;
        private static Brush? mediumOrchid;
        private static Brush? mediumPurple;
        private static Brush? mediumSeaGreen;
        private static Brush? mediumSlateBlue;
        private static Brush? mediumSpringGreen;
        private static Brush? mediumTurquoise;
        private static Brush? mediumVioletRed;
        private static Brush? midnightBlue;
        private static Brush? mintCream;
        private static Brush? mistyRose;
        private static Brush? moccasin;
        private static Brush? navajoWhite;
        private static Brush? navy;
        private static Brush? oldLace;
        private static Brush? olive;
        private static Brush? oliveDrab;
        private static Brush? orange;
        private static Brush? orangeRed;
        private static Brush? orchid;
        private static Brush? paleGoldenrod;
        private static Brush? paleGreen;
        private static Brush? paleTurquoise;
        private static Brush? paleVioletRed;
        private static Brush? papayaWhip;
        private static Brush? peachPuff;
        private static Brush? peru;
        private static Brush? pink;
        private static Brush? plum;
        private static Brush? powderBlue;
        private static Brush? purple;
        private static Brush? red;
        private static Brush? rosyBrown;
        private static Brush? royalBlue;
        private static Brush? saddleBrown;
        private static Brush? salmon;
        private static Brush? sandyBrown;
        private static Brush? seaGreen;
        private static Brush? seaShell;
        private static Brush? sienna;
        private static Brush? silver;
        private static Brush? skyBlue;
        private static Brush? slateBlue;
        private static Brush? slateGray;
        private static Brush? snow;
        private static Brush? springGreen;
        private static Brush? steelBlue;
        private static Brush? tan;
        private static Brush? teal;
        private static Brush? thistle;
        private static Brush? tomato;
        private static Brush? turquoise;
        private static Brush? violet;
        private static Brush? wheat;
        private static Brush? white;
        private static Brush? whiteSmoke;
        private static Brush? yellow;
        private static Brush? yellowGreen;

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Transparent => GetBrush(ref transparent, Color.Transparent);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush AliceBlue => GetBrush(ref aliceBlue, Color.AliceBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush AntiqueWhite => GetBrush(ref antiqueWhite, Color.AntiqueWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Aqua => GetBrush(ref aqua, Color.Aqua);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Aquamarine => GetBrush(ref aquamarine, Color.Aquamarine);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Azure => GetBrush(ref azure, Color.Azure);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Beige => GetBrush(ref beige, Color.Beige);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Bisque => GetBrush(ref bisque, Color.Bisque);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Black => GetBrush(ref black, Color.Black);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush BlanchedAlmond => GetBrush(ref blanchedAlmond, Color.BlanchedAlmond);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Blue => GetBrush(ref blue, Color.Blue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush BlueViolet => GetBrush(ref blueViolet, Color.BlueViolet);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Brown => GetBrush(ref brown, Color.Brown);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush BurlyWood => GetBrush(ref burlyWood, Color.BurlyWood);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush CadetBlue => GetBrush(ref cadetBlue, Color.CadetBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Chartreuse => GetBrush(ref chartreuse, Color.Chartreuse);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Chocolate => GetBrush(ref chocolate, Color.Chocolate);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Coral => GetBrush(ref coral, Color.Coral);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush CornflowerBlue => GetBrush(ref cornflowerBlue, Color.CornflowerBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Cornsilk => GetBrush(ref cornsilk, Color.Cornsilk);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Crimson => GetBrush(ref crimson, Color.Crimson);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Cyan => GetBrush(ref cyan, Color.Cyan);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkBlue => GetBrush(ref darkBlue, Color.DarkBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkCyan => GetBrush(ref darkCyan, Color.DarkCyan);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkGoldenrod => GetBrush(ref darkGoldenrod, Color.DarkGoldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkGray => GetBrush(ref darkGray, Color.DarkGray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkGreen => GetBrush(ref darkGreen, Color.DarkGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkKhaki => GetBrush(ref darkKhaki, Color.DarkKhaki);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkMagenta => GetBrush(ref darkMagenta, Color.DarkMagenta);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkOliveGreen => GetBrush(ref darkOliveGreen, Color.DarkOliveGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkOrange => GetBrush(ref darkOrange, Color.DarkOrange);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkOrchid => GetBrush(ref darkOrchid, Color.DarkOrchid);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkRed => GetBrush(ref darkRed, Color.DarkRed);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkSalmon => GetBrush(ref darkSalmon, Color.DarkSalmon);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkSeaGreen => GetBrush(ref darkSeaGreen, Color.DarkSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkSlateBlue => GetBrush(ref darkSlateBlue, Color.DarkSlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkSlateGray => GetBrush(ref darkSlateGray, Color.DarkSlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkTurquoise => GetBrush(ref darkTurquoise, Color.DarkTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DarkViolet => GetBrush(ref darkViolet, Color.DarkViolet);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DeepPink => GetBrush(ref deepPink, Color.DeepPink);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DeepSkyBlue => GetBrush(ref deepSkyBlue, Color.DeepSkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DimGray => GetBrush(ref dimGray, Color.DimGray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush DodgerBlue => GetBrush(ref dodgerBlue, Color.DodgerBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Firebrick => GetBrush(ref firebrick, Color.Firebrick);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush FloralWhite => GetBrush(ref floralWhite, Color.FloralWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush ForestGreen => GetBrush(ref forestGreen, Color.ForestGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Fuchsia => GetBrush(ref fuchsia, Color.Fuchsia);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Gainsboro => GetBrush(ref gainsboro, Color.Gainsboro);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush GhostWhite => GetBrush(ref ghostWhite, Color.GhostWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Gold => GetBrush(ref gold, Color.Gold);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Goldenrod => GetBrush(ref goldenrod, Color.Goldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Gray => GetBrush(ref gray, Color.Gray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Green => GetBrush(ref green, Color.Green);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush GreenYellow => GetBrush(ref greenYellow, Color.GreenYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Honeydew => GetBrush(ref honeydew, Color.Honeydew);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush HotPink => GetBrush(ref hotPink, Color.HotPink);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush IndianRed => GetBrush(ref indianRed, Color.IndianRed);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Indigo => GetBrush(ref indigo, Color.Indigo);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Ivory => GetBrush(ref ivory, Color.Ivory);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Khaki => GetBrush(ref khaki, Color.Khaki);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Lavender => GetBrush(ref lavender, Color.Lavender);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LavenderBlush => GetBrush(ref lavenderBlush, Color.LavenderBlush);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LawnGreen => GetBrush(ref lawnGreen, Color.LawnGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LemonChiffon => GetBrush(ref lemonChiffon, Color.LemonChiffon);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightBlue => GetBrush(ref lightBlue, Color.LightBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightCoral => GetBrush(ref lightCoral, Color.LightCoral);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightCyan => GetBrush(ref lightCyan, Color.LightCyan);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightGoldenrodYellow => GetBrush(ref lightGoldenrodYellow, Color.LightGoldenrodYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightGreen => GetBrush(ref lightGreen, Color.LightGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightGray => GetBrush(ref lightGray, Color.LightGray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightPink => GetBrush(ref lightPink, Color.LightPink);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightSalmon => GetBrush(ref lightSalmon, Color.LightSalmon);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightSeaGreen => GetBrush(ref lightSeaGreen, Color.LightSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightSkyBlue => GetBrush(ref lightSkyBlue, Color.LightSkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightSlateGray => GetBrush(ref lightSlateGray, Color.LightSlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightSteelBlue => GetBrush(ref lightSteelBlue, Color.LightSteelBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LightYellow => GetBrush(ref lightYellow, Color.LightYellow);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Lime => GetBrush(ref lime, Color.Lime);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush LimeGreen => GetBrush(ref limeGreen, Color.LimeGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Linen => GetBrush(ref linen, Color.Linen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Magenta => GetBrush(ref magenta, Color.Magenta);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Maroon => GetBrush(ref maroon, Color.Maroon);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumAquamarine => GetBrush(ref mediumAquamarine, Color.MediumAquamarine);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumBlue => GetBrush(ref mediumBlue, Color.MediumBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumOrchid => GetBrush(ref mediumOrchid, Color.MediumOrchid);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumPurple => GetBrush(ref mediumPurple, Color.MediumPurple);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumSeaGreen => GetBrush(ref mediumSeaGreen, Color.MediumSeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumSlateBlue => GetBrush(ref mediumSlateBlue, Color.MediumSlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumSpringGreen => GetBrush(ref mediumSpringGreen, Color.MediumSpringGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumTurquoise => GetBrush(ref mediumTurquoise, Color.MediumTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MediumVioletRed => GetBrush(ref mediumVioletRed, Color.MediumVioletRed);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MidnightBlue => GetBrush(ref midnightBlue, Color.MidnightBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MintCream => GetBrush(ref mintCream, Color.MintCream);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush MistyRose => GetBrush(ref mistyRose, Color.MistyRose);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Moccasin => GetBrush(ref moccasin, Color.Moccasin);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush NavajoWhite => GetBrush(ref navajoWhite, Color.NavajoWhite);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Navy => GetBrush(ref navy, Color.Navy);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush OldLace => GetBrush(ref oldLace, Color.OldLace);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Olive => GetBrush(ref olive, Color.Olive);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush OliveDrab => GetBrush(ref oliveDrab, Color.OliveDrab);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Orange => GetBrush(ref orange, Color.Orange);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush OrangeRed => GetBrush(ref orangeRed, Color.OrangeRed);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Orchid => GetBrush(ref orchid, Color.Orchid);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PaleGoldenrod => GetBrush(ref paleGoldenrod, Color.PaleGoldenrod);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PaleGreen => GetBrush(ref paleGreen, Color.PaleGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PaleTurquoise => GetBrush(ref paleTurquoise, Color.PaleTurquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PaleVioletRed => GetBrush(ref paleVioletRed, Color.PaleVioletRed);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PapayaWhip => GetBrush(ref papayaWhip, Color.PapayaWhip);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PeachPuff => GetBrush(ref peachPuff, Color.PeachPuff);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Peru => GetBrush(ref peru, Color.Peru);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Pink => GetBrush(ref pink, Color.Pink);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Plum => GetBrush(ref plum, Color.Plum);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush PowderBlue => GetBrush(ref powderBlue, Color.PowderBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Purple => GetBrush(ref purple, Color.Purple);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Red => GetBrush(ref red, Color.Red);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush RosyBrown => GetBrush(ref rosyBrown, Color.RosyBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush RoyalBlue => GetBrush(ref royalBlue, Color.RoyalBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SaddleBrown => GetBrush(ref saddleBrown, Color.SaddleBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Salmon => GetBrush(ref salmon, Color.Salmon);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SandyBrown => GetBrush(ref sandyBrown, Color.SandyBrown);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SeaGreen => GetBrush(ref seaGreen, Color.SeaGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SeaShell => GetBrush(ref seaShell, Color.SeaShell);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Sienna => GetBrush(ref sienna, Color.Sienna);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Silver => GetBrush(ref silver, Color.Silver);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SkyBlue => GetBrush(ref skyBlue, Color.SkyBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SlateBlue => GetBrush(ref slateBlue, Color.SlateBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SlateGray => GetBrush(ref slateGray, Color.SlateGray);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Snow => GetBrush(ref snow, Color.Snow);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SpringGreen => GetBrush(ref springGreen, Color.SpringGreen);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush SteelBlue => GetBrush(ref steelBlue, Color.SteelBlue);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Tan => GetBrush(ref tan, Color.Tan);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Teal => GetBrush(ref teal, Color.Teal);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Thistle => GetBrush(ref thistle, Color.Thistle);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Tomato => GetBrush(ref tomato, Color.Tomato);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Turquoise => GetBrush(ref turquoise, Color.Turquoise);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Violet => GetBrush(ref violet, Color.Violet);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Wheat => GetBrush(ref wheat, Color.Wheat);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush White => GetBrush(ref white, Color.White);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush WhiteSmoke => GetBrush(ref whiteSmoke, Color.WhiteSmoke);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush Yellow => GetBrush(ref yellow, Color.Yellow);

        /// <summary>
        /// Gets a system-defined <see cref="Brush"/> object.
        /// </summary>
        /// <remarks>
        /// A <see cref="Brush"/> object set to a system-defined color.
        /// </remarks>
        public static Brush YellowGreen => GetBrush(ref yellowGreen, Color.YellowGreen);

        private static Brush GetBrush(ref Brush? brush, Color color)
        {
            if (brush == null)
                brush = new SolidBrush(color, immutable: true);
            return brush;
        }
    }
}