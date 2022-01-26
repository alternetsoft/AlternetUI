// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents an ARGB (alpha, red, green, blue) color.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Named colors are represented by using the properties of the <see cref="Color"/> structure.
    /// </para>
    /// <para>
    /// The color of each pixel is represented as a 32-bit number: 8 bits each for alpha, red, green, and blue (ARGB).
    /// Each of the four components is a number from 0 through 255, with 0 representing no intensity and 255 representing full intensity.
    /// The alpha component specifies the transparency of the color: 0 is fully transparent, and 255 is fully opaque.
    /// To determine the alpha, red, green, or blue component of a color, use the <see cref="A"/>, <see cref="R"/>, <see cref="G"/>, or <see cref="B"/> property,
    /// respectively. You can create a custom color by using one of the <see cref="FromArgb(int)"/> method.
    /// </para>
    /// </remarks>
    [DebuggerDisplay("{NameAndARGBValue}")]
    //[Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
    //        "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [Serializable]
    [TypeConverter(typeof(ColorConverter))]
    public readonly struct Color : IEquatable<Color>
    {
        /// <summary>
        /// Represents a color that is <c>null</c>.
        /// </summary>
        public static readonly Color Empty;

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Transparent => new Color(KnownColor.Transparent);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color AliceBlue => new Color(KnownColor.AliceBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color AntiqueWhite => new Color(KnownColor.AntiqueWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Aqua => new Color(KnownColor.Aqua);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Aquamarine => new Color(KnownColor.Aquamarine);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Azure => new Color(KnownColor.Azure);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Beige => new Color(KnownColor.Beige);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Bisque => new Color(KnownColor.Bisque);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Black => new Color(KnownColor.Black);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color BlanchedAlmond => new Color(KnownColor.BlanchedAlmond);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Blue => new Color(KnownColor.Blue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color BlueViolet => new Color(KnownColor.BlueViolet);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Brown => new Color(KnownColor.Brown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color BurlyWood => new Color(KnownColor.BurlyWood);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color CadetBlue => new Color(KnownColor.CadetBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Chartreuse => new Color(KnownColor.Chartreuse);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Chocolate => new Color(KnownColor.Chocolate);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Coral => new Color(KnownColor.Coral);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color CornflowerBlue => new Color(KnownColor.CornflowerBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Cornsilk => new Color(KnownColor.Cornsilk);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Crimson => new Color(KnownColor.Crimson);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Cyan => new Color(KnownColor.Cyan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkBlue => new Color(KnownColor.DarkBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkCyan => new Color(KnownColor.DarkCyan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkGoldenrod => new Color(KnownColor.DarkGoldenrod);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkGray => new Color(KnownColor.DarkGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkGreen => new Color(KnownColor.DarkGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkKhaki => new Color(KnownColor.DarkKhaki);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkMagenta => new Color(KnownColor.DarkMagenta);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkOliveGreen => new Color(KnownColor.DarkOliveGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkOrange => new Color(KnownColor.DarkOrange);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkOrchid => new Color(KnownColor.DarkOrchid);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkRed => new Color(KnownColor.DarkRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkSalmon => new Color(KnownColor.DarkSalmon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkSeaGreen => new Color(KnownColor.DarkSeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkSlateBlue => new Color(KnownColor.DarkSlateBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkSlateGray => new Color(KnownColor.DarkSlateGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkTurquoise => new Color(KnownColor.DarkTurquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DarkViolet => new Color(KnownColor.DarkViolet);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DeepPink => new Color(KnownColor.DeepPink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DeepSkyBlue => new Color(KnownColor.DeepSkyBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DimGray => new Color(KnownColor.DimGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color DodgerBlue => new Color(KnownColor.DodgerBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Firebrick => new Color(KnownColor.Firebrick);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color FloralWhite => new Color(KnownColor.FloralWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color ForestGreen => new Color(KnownColor.ForestGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Fuchsia => new Color(KnownColor.Fuchsia);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Gainsboro => new Color(KnownColor.Gainsboro);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color GhostWhite => new Color(KnownColor.GhostWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Gold => new Color(KnownColor.Gold);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Goldenrod => new Color(KnownColor.Goldenrod);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Gray => new Color(KnownColor.Gray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Green => new Color(KnownColor.Green);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color GreenYellow => new Color(KnownColor.GreenYellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Honeydew => new Color(KnownColor.Honeydew);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color HotPink => new Color(KnownColor.HotPink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color IndianRed => new Color(KnownColor.IndianRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Indigo => new Color(KnownColor.Indigo);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Ivory => new Color(KnownColor.Ivory);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Khaki => new Color(KnownColor.Khaki);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Lavender => new Color(KnownColor.Lavender);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LavenderBlush => new Color(KnownColor.LavenderBlush);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LawnGreen => new Color(KnownColor.LawnGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LemonChiffon => new Color(KnownColor.LemonChiffon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightBlue => new Color(KnownColor.LightBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightCoral => new Color(KnownColor.LightCoral);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightCyan => new Color(KnownColor.LightCyan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightGoldenrodYellow => new Color(KnownColor.LightGoldenrodYellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightGreen => new Color(KnownColor.LightGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightGray => new Color(KnownColor.LightGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightPink => new Color(KnownColor.LightPink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightSalmon => new Color(KnownColor.LightSalmon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightSeaGreen => new Color(KnownColor.LightSeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightSkyBlue => new Color(KnownColor.LightSkyBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightSlateGray => new Color(KnownColor.LightSlateGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightSteelBlue => new Color(KnownColor.LightSteelBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LightYellow => new Color(KnownColor.LightYellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Lime => new Color(KnownColor.Lime);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color LimeGreen => new Color(KnownColor.LimeGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Linen => new Color(KnownColor.Linen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Magenta => new Color(KnownColor.Magenta);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Maroon => new Color(KnownColor.Maroon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumAquamarine => new Color(KnownColor.MediumAquamarine);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumBlue => new Color(KnownColor.MediumBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumOrchid => new Color(KnownColor.MediumOrchid);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumPurple => new Color(KnownColor.MediumPurple);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumSeaGreen => new Color(KnownColor.MediumSeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumSlateBlue => new Color(KnownColor.MediumSlateBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumSpringGreen => new Color(KnownColor.MediumSpringGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumTurquoise => new Color(KnownColor.MediumTurquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MediumVioletRed => new Color(KnownColor.MediumVioletRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MidnightBlue => new Color(KnownColor.MidnightBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MintCream => new Color(KnownColor.MintCream);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color MistyRose => new Color(KnownColor.MistyRose);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Moccasin => new Color(KnownColor.Moccasin);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color NavajoWhite => new Color(KnownColor.NavajoWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Navy => new Color(KnownColor.Navy);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color OldLace => new Color(KnownColor.OldLace);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Olive => new Color(KnownColor.Olive);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color OliveDrab => new Color(KnownColor.OliveDrab);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Orange => new Color(KnownColor.Orange);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color OrangeRed => new Color(KnownColor.OrangeRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Orchid => new Color(KnownColor.Orchid);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PaleGoldenrod => new Color(KnownColor.PaleGoldenrod);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PaleGreen => new Color(KnownColor.PaleGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PaleTurquoise => new Color(KnownColor.PaleTurquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PaleVioletRed => new Color(KnownColor.PaleVioletRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PapayaWhip => new Color(KnownColor.PapayaWhip);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PeachPuff => new Color(KnownColor.PeachPuff);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Peru => new Color(KnownColor.Peru);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Pink => new Color(KnownColor.Pink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Plum => new Color(KnownColor.Plum);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color PowderBlue => new Color(KnownColor.PowderBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Purple => new Color(KnownColor.Purple);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color RebeccaPurple => new Color(KnownColor.RebeccaPurple);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Red => new Color(KnownColor.Red);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color RosyBrown => new Color(KnownColor.RosyBrown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color RoyalBlue => new Color(KnownColor.RoyalBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SaddleBrown => new Color(KnownColor.SaddleBrown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Salmon => new Color(KnownColor.Salmon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SandyBrown => new Color(KnownColor.SandyBrown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SeaGreen => new Color(KnownColor.SeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SeaShell => new Color(KnownColor.SeaShell);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Sienna => new Color(KnownColor.Sienna);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Silver => new Color(KnownColor.Silver);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SkyBlue => new Color(KnownColor.SkyBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SlateBlue => new Color(KnownColor.SlateBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SlateGray => new Color(KnownColor.SlateGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Snow => new Color(KnownColor.Snow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SpringGreen => new Color(KnownColor.SpringGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color SteelBlue => new Color(KnownColor.SteelBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Tan => new Color(KnownColor.Tan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Teal => new Color(KnownColor.Teal);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Thistle => new Color(KnownColor.Thistle);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Tomato => new Color(KnownColor.Tomato);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Turquoise => new Color(KnownColor.Turquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Violet => new Color(KnownColor.Violet);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Wheat => new Color(KnownColor.Wheat);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color White => new Color(KnownColor.White);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color WhiteSmoke => new Color(KnownColor.WhiteSmoke);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color Yellow => new Color(KnownColor.Yellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static Color YellowGreen => new Color(KnownColor.YellowGreen);

        // NOTE : The "zero" pattern (all members being 0) must represent
        //      : "not set". This allows "Color c;" to be correct.

        private const short StateKnownColorValid = 0x0001;
        private const short StateARGBValueValid = 0x0002;
        private const short StateValueMask = StateARGBValueValid;
        private const short StateNameValid = 0x0008;
        private const long NotDefinedValue = 0;

        // Shift counts and bit masks for A, R, G, B components in ARGB mode

        internal const int ARGBAlphaShift = 24;
        internal const int ARGBRedShift = 16;
        internal const int ARGBGreenShift = 8;
        internal const int ARGBBlueShift = 0;
        internal const uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;
        internal const uint ARGBRedMask = 0xFFu << ARGBRedShift;
        internal const uint ARGBGreenMask = 0xFFu << ARGBGreenShift;
        internal const uint ARGBBlueMask = 0xFFu << ARGBBlueShift;

        // User supplied name of color. Will not be filled in if
        // we map to a "knowncolor"
        private readonly string? name; // Do not rename (binary serialization)

        // Standard 32bit sRGB (ARGB)
        private readonly long value; // Do not rename (binary serialization)

        // Ignored, unless "state" says it is valid
        private readonly short knownColor; // Do not rename (binary serialization)

        // State flags.
        private readonly short state; // Do not rename (binary serialization)

        internal Color(KnownColor knownColor)
        {
            value = 0;
            state = StateKnownColorValid;
            name = null;
            this.knownColor = unchecked((short)knownColor);
        }

        private Color(long value, short state, string? name, KnownColor knownColor)
        {
            this.value = value;
            this.state = state;
            this.name = name;
            this.knownColor = unchecked((short)knownColor);
        }

        /// <summary>
        /// Gets the red component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The red component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The color of each pixel is represented as a 32-bit number: 8 bits each for alpha, red, green, and blue (ARGB).
        /// Each of the four components is a number from 0 through 255, with 0 representing no intensity and 255
        /// representing full intensity. Likewise, <see cref="R"/> is a value from 0 to 255 with 0 representing no red and 255 representing fully red.
        /// </remarks>
        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        /// <summary>
        /// Gets the green component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The green component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The color of each pixel is represented as a 32-bit number: 8 bits each for alpha, red, green, and blue (ARGB).
        /// Each of the four components is a number from 0 through 255, with 0 representing no intensity and 255
        /// representing full intensity. Likewise, <see cref="G"/> is a value from 0 to 255 with 0 representing no green and 255 representing fully green.
        /// </remarks>
        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        /// <summary>
        /// Gets the blue component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The blue component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The color of each pixel is represented as a 32-bit number: 8 bits each for alpha, red, green, and blue (ARGB).
        /// Each of the four components is a number from 0 through 255, with 0 representing no intensity and 255
        /// representing full intensity. Likewise, <see cref="G"/> is a value from 0 to 255 with 0 representing no blue and 255 representing fully blue.
        /// </remarks>
        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        /// <summary>
        /// Gets the alpha component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The alpha component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The alpha component specifies the transparency of the color: 0 is fully transparent, and 255 is fully opaque.
        /// Likewise, an <see cref="A"/> value of 255 represents an opaque color. An <see cref="A"/> value from 1 through 254 represents a semitransparent color.
        /// The color becomes more opaque as <see cref="A"/> approaches 255.
        /// </remarks>
        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        /// <summary>
        /// Gets a value indicating whether this <see cref="Color"/> structure is a predefined color.
        /// Predefined colors are represented by the elements of the <see cref="KnownColor"/> enumeration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Color"/> was created from a predefined color by using either
        /// the <see cref="FromName"/> method or the <see cref="FromKnownColor(KnownColor)"/> method; otherwise, <c>false</c>.
        /// </value>
        public bool IsKnownColor => (state & StateKnownColorValid) != 0;

        /// <summary>
        /// Specifies whether this <see cref="Color"/> structure is uninitialized.
        /// </summary>
        /// <value>This property returns <c>true</c> if this color is uninitialized; otherwise, <c>false</c>.</value>
        public bool IsEmpty => state == 0;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Color"/> structure is a named color or a member of the <see cref="KnownColor"/> enumeration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Color"/> was created by using either the <see cref="FromName"/> method or the
        /// <see cref="FromKnownColor(KnownColor)"/> method; otherwise, <c>false</c>.
        /// </value>
        public bool IsNamedColor => ((state & StateNameValid) != 0) || IsKnownColor;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Color"/> structure is a system color.
        /// A system color is a color that is used in a Windows display element.
        /// System colors are represented by elements of the <see cref="KnownColor"/> enumeration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Color"/> was created from a system color by using either the <see cref="FromName"/> method
        /// or the <see cref="FromKnownColor(KnownColor)"/> method; otherwise, <c>false</c>.
        /// </value>
        public bool IsSystemColor => IsKnownColor && IsKnownColorSystem((KnownColor)knownColor);

        internal static bool IsKnownColorSystem(KnownColor knownColor)
            => KnownColorTable.ColorKindTable[(int)knownColor] == KnownColorTable.KnownColorKindSystem;

        // Used for the [DebuggerDisplay]. Inlining in the attribute is possible, but
        // against best practices as the current project language parses the string with
        // language specific heuristics.

        private string NameAndARGBValue => $"{{Name={Name}, ARGB=({A}, {R}, {G}, {B})}}";

        /// <summary>
        /// Gets the name of this <see cref="Color"/>.
        /// </summary>
        /// <value>The name of this <see cref="Color"/>.</value>
        /// <remarks>
        /// This method returns either the user-defined name of the color, if the color was created from a name,
        /// or the name of the known color. For custom colors, the RGB value is returned.
        /// </remarks>
        public string Name
        {
            get
            {
                if ((state & StateNameValid) != 0)
                {
                    if (name == null)
                        throw new InvalidOperationException();
                    return name;
                }

                if (IsKnownColor)
                {
                    string tablename = KnownColorNames.KnownColorToName((KnownColor)knownColor);
                    if (tablename == null)
                        throw new InvalidOperationException($"Could not find known color '{(KnownColor)knownColor}' in the KnownColorTable");

                    return tablename;
                }

                // if we reached here, just encode the value
                //
                return value.ToString("x");
            }
        }

        private long Value
        {
            get
            {
                if ((state & StateValueMask) != 0)
                {
                    return value;
                }

                // This is the only place we have system colors value exposed
                if (IsKnownColor)
                {
                    return KnownColorTable.KnownColorToArgb((KnownColor)knownColor);
                }

                return NotDefinedValue;
            }
        }

        private static void CheckByte(int value, string name)
        {
            static void ThrowOutOfByteRange(int v, string n) =>
                throw new ArgumentException(
                    string.Format(
                        "Variable {0} has invalid value {1}. Minimum allowed value is {2}, maximum is {3}.",
                        new object[] { n, v, byte.MinValue, byte.MaxValue }));

            if (unchecked((uint)value) > byte.MaxValue)
                ThrowOutOfByteRange(value, name);
        }

        private static Color FromArgb(uint argb) => new Color(argb, StateARGBValueValid, null, (KnownColor)0);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from a 32-bit ARGB value.
        /// </summary>
        /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
        /// <returns>The <see cref="Color"/> structure that this method creates.</returns>
        /// <remarks>
        /// The byte-ordering of the 32-bit ARGB value is AARRGGBB. The most significant byte (MSB), represented by AA,
        /// is the alpha component value. The second, third, and fourth bytes, represented by RR, GG, and BB,
        /// respectively, are the color components red, green, and blue, respectively.
        /// </remarks>
        public static Color FromArgb(int argb) => FromArgb(unchecked((uint)argb));

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the four ARGB component (alpha, red, green, and blue) values.
        /// Although this method allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="alpha">The alpha component. Valid values are 0 through 255.</param>
        /// <param name="red">The red component. Valid values are 0 through 255.</param>
        /// <param name="green">The green component. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component. Valid values are 0 through 255.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>To create an opaque color, set alpha to 255. To create a semitransparent color, set alpha to any value from 1 through 254.</remarks>
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)red << ARGBRedShift |
                (uint)green << ARGBGreenShift |
                (uint)blue << ARGBBlueShift
            );
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified <see cref="Color"/> structure, but with the new specified alpha value.
        /// Although this method allows a 32-bit value to be passed for the alpha value, the value is limited to 8 bits.
        /// </summary>
        /// <param name="alpha">The alpha value for the new <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <param name="baseColor">The <see cref="Color"/> from which to create the new Color.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>To create an opaque color, set alpha to 255. To create a semitransparent color, set alpha to any value from 1 through 254.</remarks>
        public static Color FromArgb(int alpha, Color baseColor)
        {
            CheckByte(alpha, nameof(alpha));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)baseColor.Value & ~ARGBAlphaMask
            );
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified 8-bit color values (red, green, and blue).
        /// The alpha value is implicitly 255 (fully opaque). Although this method allows a 32-bit value to be passed
        /// for each color component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="red">The red component value for the new <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the new <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the new <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <exception cref="ArgumentException"><c>red</c>, <c>green</c>, or <c>blue</c> is less than 0 or greater than 255.</exception>
        public static Color FromArgb(int red, int green, int blue) => FromArgb(byte.MaxValue, red, green, blue);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified predefined color.
        /// </summary>
        /// <param name="color">An element of the <see cref="KnownColor"/> enumeration.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>A predefined color is also called a known color and is represented by an element of the <see cref="KnownColor"/> enumeration.</remarks>
        public static Color FromKnownColor(KnownColor color) =>
            color <= 0 || color > KnownColor.RebeccaPurple ? FromName(color.ToString()) : new Color(color);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified name of a predefined color.
        /// </summary>
        /// <param name="name">A string that is the name of a predefined color. Valid names are the same as the names of the elements of the <see cref="KnownColor"/> enumeration.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>
        /// A predefined color is also called a known color and is represented by an element of the <see cref="KnownColor"/> enumeration.
        /// If the name parameter is not the valid name of a predefined color, the <see cref="FromName"/> method creates a <see cref="Color"/> structure
        /// that has an ARGB value of 0 (that is, all ARGB components are 0).
        /// </remarks>
        public static Color FromName(string name)
        {
            // try to get a known color first
            if (ColorTable.TryGetNamedColor(name, out Color color))
                return color;

            // otherwise treat it as a named color
            return new Color(NotDefinedValue, StateNameValid, name, (KnownColor)0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetRgbValues(out int r, out int g, out int b)
        {
            uint value = (uint)Value;
            r = (int)(value & ARGBRedMask) >> ARGBRedShift;
            g = (int)(value & ARGBGreenMask) >> ARGBGreenShift;
            b = (int)(value & ARGBBlueMask) >> ARGBBlueShift;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MinMaxRgb(out int min, out int max, int r, int g, int b)
        {
            if (r > g)
            {
                max = r;
                min = g;
            }
            else
            {
                max = g;
                min = r;
            }
            if (b > max)
            {
                max = b;
            }
            else if (b < min)
            {
                min = b;
            }
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) lightness value for this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The lightness of this <see cref="Color"/>. The lightness ranges from 0.0 through 1.0, where 0.0 represents black and 1.0 represents white.</returns>
        public double GetBrightness()
        {
            GetRgbValues(out int r, out int g, out int b);

            MinMaxRgb(out int min, out int max, r, g, b);

            return (max + min) / (byte.MaxValue * 2f);
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) hue value, in degrees, for this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The hue, in degrees, of this <see cref="Color"/>. The hue is measured in degrees, ranging from 0.0 through 360.0, in HSL color space.</returns>
        public double GetHue()
        {
            GetRgbValues(out int r, out int g, out int b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out int min, out int max, r, g, b);

            double delta = max - min;
            double hue;

            if (r == max)
                hue = (g - b) / delta;
            else if (g == max)
                hue = (b - r) / delta + 2f;
            else
                hue = (r - g) / delta + 4f;

            hue *= 60f;
            if (hue < 0f)
                hue += 360f;

            return hue;
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) saturation value for this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>
        /// The saturation of this <see cref="Color"/>. The saturation ranges from 0.0 through 1.0,
        /// where 0.0 is grayscale and 1.0 is the most saturated.
        /// </returns>
        public double GetSaturation()
        {
            GetRgbValues(out int r, out int g, out int b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out int min, out int max, r, g, b);

            int div = max + min;
            if (div > byte.MaxValue)
                div = byte.MaxValue * 2 - max - min;

            return (max - min) / (double)div;
        }

        /// <summary>
        /// Gets the 32-bit ARGB value of this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The 32-bit ARGB value of this <see cref="Color"/>.</returns>
        /// <remarks>
        /// The byte-ordering of the 32-bit ARGB value is AARRGGBB. The most significant byte (MSB), represented by AA,
        /// is the alpha component value. The second, third, and fourth bytes, represented by RR, GG, and BB,
        /// respectively, are the color components red, green, and blue, respectively.
        /// </remarks>
        public int ToArgb() => unchecked((int)Value);

        /// <summary>
        /// Gets the <see cref="KnownColor"/> value of this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>
        /// An element of the <see cref="KnownColor"/> enumeration, if the <see cref="Color"/> is created from a predefined
        /// color by using either the <see cref="FromName"/> method or the <see cref="FromKnownColor(KnownColor)"/> method; otherwise, 0.
        /// </returns>
        /// <remarks>
        /// A predefined color is also called a known color and is represented by an element of the <see cref="KnownColor"/> enumeration.
        /// When the <see cref="ToKnownColor"/> method is applied to a <see cref="Color"/> structure that is created by using the <see cref="FromArgb(int)"/> method,
        /// <see cref="ToKnownColor"/> returns 0, even if the ARGB value matches the ARGB value of a predefined color.
        /// <see cref="ToKnownColor"/> also returns 0 when it is applied to a <see cref="Color"/> structure that is created by using
        /// the <see cref="FromName"/> method with a string name that is not valid.
        /// </remarks>
        public KnownColor ToKnownColor() => (KnownColor)knownColor;

        /// <summary>
        /// Converts this <see cref="Color"/> structure to a human-readable string.
        /// </summary>
        /// <returns>
        /// A string that is the name of this <see cref="Color"/>, if the <see cref="Color"/> is created from a predefined color by using either the
        /// <see cref="FromName"/> method or the <see cref="FromKnownColor(KnownColor)"/> method;
        /// otherwise, a string that consists of the ARGB component names and their values.
        /// </returns>
        /// <remarks>
        /// A predefined color is also called a known color and is represented by an element of the <see cref="KnownColor"/> enumeration.
        /// When the <see cref="ToString"/> method is applied to a <see cref="Color"/> structure that is created by using the <see cref="FromArgb(int)"/> method,
        /// <see cref="ToString"/> returns a string that consists of the ARGB component names and their values, even if the ARGB value matches the ARGB value of a predefined color.
        /// </remarks>
        public override string ToString() =>
            IsNamedColor ? $"{nameof(Color)} [{Name}]" :
            (state & StateValueMask) != 0 ? $"{nameof(Color)} [A={A}, R={R}, G={G}, B={B}]" :
            $"{nameof(Color)} [Empty]";

        /// <summary>
        /// Tests whether two specified <see cref="Color"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left of the equality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="Color"/> structures are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color left, Color right) =>
            left.value == right.value
                && left.state == right.state
                && left.knownColor == right.knownColor
                && left.name == right.name;

        /// <summary>
        /// Tests whether two specified <see cref="Color"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left of the inequality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right of the inequality operator.</param>
        /// <returns><c>true</c> if the two <see cref="Color"/> structures are different; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Color left, Color right) => !(left == right);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Color other && Equals(other);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise, <c>false</c>.</returns>
        public bool Equals(Color other) => this == other;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            // Three cases:
            // 1. We don't have a name. All relevant data, including this fact, is in the remaining fields.
            // 2. We have a known name. The name will be the same instance of any other with the same
            // knownColor value, so we can ignore it for hashing. Note this also hashes different to
            // an unnamed color with the same ARGB value.
            // 3. Have an unknown name. Will differ from other unknown-named colors only by name, so we
            // can usefully use the names hash code alone.
            if (name != null && !IsKnownColor)
                return name.GetHashCode();

            return HashCode.Combine(value.GetHashCode(), state.GetHashCode(), knownColor.GetHashCode());
        }
    }
}
