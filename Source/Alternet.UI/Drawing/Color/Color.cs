// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
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
    /// Named colors are represented by using the properties of the
    /// <see cref="Color"/> structure.
    /// </para>
    /// <para>
    /// The color of each pixel is represented as a 32-bit number: 8 bits
    /// each for alpha, red, green, and blue (ARGB).
    /// Each of the four components is a number from 0 through 255, with 0
    /// representing no intensity and 255 representing full intensity.
    /// The alpha component specifies the transparency of the color: 0
    /// is fully transparent, and 255 is fully opaque.
    /// To determine the alpha, red, green, or blue component of a color,
    /// use the <see cref="A"/>, <see cref="R"/>, <see cref="G"/>,
    /// or <see cref="B"/> property,
    /// respectively. You can create a custom color by using one of the
    /// <see cref="FromArgb(int)"/> method.
    /// </para>
    /// </remarks>
    [DebuggerDisplay("{NameAndARGBValue}")]
    [Serializable]
    [TypeConverter(typeof(ColorConverter))]
    public readonly struct Color : IEquatable<Color>
    {
        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color White = new(KnownColor.White);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Transparent = new(KnownColor.Transparent);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Black = new(KnownColor.Black);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Blue = new(KnownColor.Blue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color AliceBlue = new(KnownColor.AliceBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color AntiqueWhite = new(KnownColor.AntiqueWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Aqua = new(KnownColor.Aqua);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Aquamarine = new(KnownColor.Aquamarine);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Azure = new(KnownColor.Azure);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Beige = new(KnownColor.Beige);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Bisque = new(KnownColor.Bisque);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color BlanchedAlmond = new(KnownColor.BlanchedAlmond);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color BlueViolet = new(KnownColor.BlueViolet);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Brown = new(KnownColor.Brown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color BurlyWood = new(KnownColor.BurlyWood);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color CadetBlue = new(KnownColor.CadetBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Chartreuse = new(KnownColor.Chartreuse);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Chocolate = new(KnownColor.Chocolate);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Coral = new(KnownColor.Coral);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color CornflowerBlue = new(KnownColor.CornflowerBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Cornsilk = new(KnownColor.Cornsilk);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Crimson = new(KnownColor.Crimson);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Cyan = new(KnownColor.Cyan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkBlue = new(KnownColor.DarkBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkCyan = new(KnownColor.DarkCyan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkGoldenrod = new(KnownColor.DarkGoldenrod);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkGray = new(KnownColor.DarkGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkGreen = new(KnownColor.DarkGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkKhaki = new(KnownColor.DarkKhaki);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkMagenta = new(KnownColor.DarkMagenta);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkOliveGreen = new(KnownColor.DarkOliveGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkOrange = new(KnownColor.DarkOrange);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkOrchid = new(KnownColor.DarkOrchid);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkRed = new(KnownColor.DarkRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkSalmon = new(KnownColor.DarkSalmon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkSeaGreen = new(KnownColor.DarkSeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkSlateBlue = new(KnownColor.DarkSlateBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkSlateGray = new(KnownColor.DarkSlateGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkTurquoise = new(KnownColor.DarkTurquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DarkViolet = new(KnownColor.DarkViolet);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DeepPink = new(KnownColor.DeepPink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DeepSkyBlue = new(KnownColor.DeepSkyBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DimGray = new(KnownColor.DimGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color DodgerBlue = new(KnownColor.DodgerBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Firebrick = new(KnownColor.Firebrick);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color FloralWhite = new(KnownColor.FloralWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color ForestGreen = new(KnownColor.ForestGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Fuchsia = new(KnownColor.Fuchsia);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Gainsboro = new(KnownColor.Gainsboro);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color GhostWhite = new(KnownColor.GhostWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Gold = new(KnownColor.Gold);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Goldenrod = new(KnownColor.Goldenrod);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Gray = new(KnownColor.Gray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Green = new(KnownColor.Green);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color GreenYellow = new(KnownColor.GreenYellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Honeydew = new(KnownColor.Honeydew);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color HotPink = new(KnownColor.HotPink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color IndianRed = new(KnownColor.IndianRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Indigo = new(KnownColor.Indigo);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Ivory = new(KnownColor.Ivory);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Khaki = new(KnownColor.Khaki);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Lavender = new(KnownColor.Lavender);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LavenderBlush = new(KnownColor.LavenderBlush);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LawnGreen = new(KnownColor.LawnGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LemonChiffon = new(KnownColor.LemonChiffon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightBlue = new(KnownColor.LightBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightCoral = new(KnownColor.LightCoral);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightCyan = new(KnownColor.LightCyan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightGoldenrodYellow =
            new(KnownColor.LightGoldenrodYellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightGreen = new(KnownColor.LightGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightGray = new(KnownColor.LightGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightPink = new(KnownColor.LightPink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightSalmon = new(KnownColor.LightSalmon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightSeaGreen = new(KnownColor.LightSeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightSkyBlue = new(KnownColor.LightSkyBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightSlateGray = new(KnownColor.LightSlateGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightSteelBlue = new(KnownColor.LightSteelBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LightYellow = new(KnownColor.LightYellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Lime = new(KnownColor.Lime);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color LimeGreen = new(KnownColor.LimeGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Linen = new(KnownColor.Linen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Magenta = new(KnownColor.Magenta);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Maroon = new(KnownColor.Maroon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumAquamarine = new(KnownColor.MediumAquamarine);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumBlue = new(KnownColor.MediumBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumOrchid = new(KnownColor.MediumOrchid);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumPurple = new(KnownColor.MediumPurple);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumSeaGreen = new(KnownColor.MediumSeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumSlateBlue = new(KnownColor.MediumSlateBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumSpringGreen = new(KnownColor.MediumSpringGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumTurquoise = new(KnownColor.MediumTurquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MediumVioletRed = new(KnownColor.MediumVioletRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MidnightBlue = new(KnownColor.MidnightBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MintCream = new(KnownColor.MintCream);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color MistyRose = new(KnownColor.MistyRose);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Moccasin = new(KnownColor.Moccasin);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color NavajoWhite = new(KnownColor.NavajoWhite);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Navy = new(KnownColor.Navy);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color OldLace = new(KnownColor.OldLace);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Olive = new(KnownColor.Olive);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color OliveDrab = new(KnownColor.OliveDrab);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Orange = new(KnownColor.Orange);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color OrangeRed = new(KnownColor.OrangeRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Orchid = new(KnownColor.Orchid);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PaleGoldenrod = new(KnownColor.PaleGoldenrod);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PaleGreen = new(KnownColor.PaleGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PaleTurquoise = new(KnownColor.PaleTurquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PaleVioletRed = new(KnownColor.PaleVioletRed);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PapayaWhip = new(KnownColor.PapayaWhip);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PeachPuff = new(KnownColor.PeachPuff);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Peru = new(KnownColor.Peru);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Pink = new(KnownColor.Pink);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Plum = new(KnownColor.Plum);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color PowderBlue = new(KnownColor.PowderBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Purple = new(KnownColor.Purple);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color RebeccaPurple = new(KnownColor.RebeccaPurple);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Red = new(KnownColor.Red);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color RosyBrown = new(KnownColor.RosyBrown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color RoyalBlue = new(KnownColor.RoyalBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SaddleBrown = new(KnownColor.SaddleBrown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Salmon = new(KnownColor.Salmon);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SandyBrown = new(KnownColor.SandyBrown);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SeaGreen = new(KnownColor.SeaGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SeaShell = new(KnownColor.SeaShell);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Sienna = new(KnownColor.Sienna);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Silver = new(KnownColor.Silver);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SkyBlue = new(KnownColor.SkyBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SlateBlue = new(KnownColor.SlateBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SlateGray = new(KnownColor.SlateGray);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Snow = new(KnownColor.Snow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SpringGreen = new(KnownColor.SpringGreen);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color SteelBlue = new(KnownColor.SteelBlue);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Tan = new(KnownColor.Tan);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Teal = new(KnownColor.Teal);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Thistle = new(KnownColor.Thistle);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Tomato = new(KnownColor.Tomato);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Turquoise = new(KnownColor.Turquoise);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Violet = new(KnownColor.Violet);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Wheat = new(KnownColor.Wheat);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color WhiteSmoke = new(KnownColor.WhiteSmoke);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color Yellow = new(KnownColor.Yellow);

        /// <summary>
        /// Gets a system-defined color.
        /// </summary>
        public static readonly Color YellowGreen = new(KnownColor.YellowGreen);

        /// <summary>
        /// Represents a color that is <c>null</c>.
        /// </summary>
        public static readonly Color Empty;

        // Shift counts and bit masks for A, R, G, B components in ARGB mode
        internal const int ARGBAlphaShift = 24;
        internal const int ARGBRedShift = 16;
        internal const int ARGBGreenShift = 8;
        internal const int ARGBBlueShift = 0;
        internal const uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;
        internal const uint ARGBRedMask = 0xFFu << ARGBRedShift;
        internal const uint ARGBGreenMask = 0xFFu << ARGBGreenShift;
        internal const uint ARGBBlueMask = 0xFFu << ARGBBlueShift;

        // NOTE : The "zero" pattern (all members being 0) must represent
        //      : "not set". This allows "Color c;" to be correct.
        private const short StateKnownColorValid = 0x0001;
        private const short StateARGBValueValid = 0x0002;
        private const short StateValueMask = StateARGBValueValid;
        private const short StateNameValid = 0x0008;
        private const long NotDefinedValue = 0;

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
        /// The color of each pixel is represented as a 32-bit number: 8 bits each
        /// for alpha, red, green, and blue (ARGB).
        /// Each of the four components is a number from 0 through 255,
        /// with 0 representing no intensity and 255
        /// representing full intensity. Likewise, <see cref="R"/> is a value from
        /// 0 to 255 with 0 representing no red and 255 representing fully red.
        /// </remarks>
        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        /// <summary>
        /// Gets the green component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The green component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The color of each pixel is represented as a 32-bit number: 8 bits each
        /// for alpha, red, green, and blue (ARGB).
        /// Each of the four components is a number from 0 through 255,
        /// with 0 representing no intensity and 255
        /// representing full intensity. Likewise, <see cref="G"/> is a value
        /// from 0 to 255 with 0 representing no green and 255 representing fully green.
        /// </remarks>
        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        /// <summary>
        /// Gets the blue component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The blue component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The color of each pixel is represented as a 32-bit number: 8 bits each
        /// for alpha, red, green, and blue (ARGB).
        /// Each of the four components is a number from 0 through 255, with
        /// 0 representing no intensity and 255
        /// representing full intensity. Likewise, <see cref="G"/> is a value from
        /// 0 to 255 with 0 representing no blue and 255 representing fully blue.
        /// </remarks>
        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        /// <summary>
        /// Gets the alpha component value of this <see cref="Color"/> structure.
        /// </summary>
        /// <value>The alpha component value of this <see cref="Color"/>.</value>
        /// <remarks>
        /// The alpha component specifies the transparency of the color: 0 is
        /// fully transparent, and 255 is fully opaque.
        /// Likewise, an <see cref="A"/> value of 255 represents an opaque color.
        /// An <see cref="A"/> value from 1 through 254 represents a semitransparent color.
        /// The color becomes more opaque as <see cref="A"/> approaches 255.
        /// </remarks>
        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        /// <summary>
        /// Returns <c>true</c> if color is opaque (<see cref="A"/> is 255).
        /// </summary>
        public bool IsOpaque => A == 255;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Color"/> structure is
        /// a predefined color.
        /// Predefined colors are represented by the elements of the
        /// <see cref="KnownColor"/> enumeration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Color"/> was created from a predefined
        /// color by using either
        /// the <see cref="FromName"/> method or the
        /// <see cref="FromKnownColor(KnownColor)"/> method; otherwise, <c>false</c>.
        /// </value>
        public bool IsKnownColor => (state & StateKnownColorValid) != 0;

        /// <summary>
        /// Specifies whether this <see cref="Color"/> structure is uninitialized.
        /// </summary>
        /// <value>This property returns <c>true</c> if this color is uninitialized;
        /// otherwise, <c>false</c>.</value>
        public bool IsEmpty => state == 0;

        /// <summary>
        /// Specifies whether this <see cref="Color"/> structure is initialized.
        /// </summary>
        /// <value>This property returns <c>true</c> if this color is initialized;
        /// otherwise, <c>false</c>.</value>
        public bool IsOk => state != 0;

        /// <summary>
        /// Gets <see cref="R"/> as hex <see cref="string"/>.
        /// </summary>
        public string RHex => R.ToString("X2");

        /// <summary>
        /// Gets <see cref="G"/> as hex <see cref="string"/>.
        /// </summary>
        public string GHex => G.ToString("X2");

        /// <summary>
        /// Gets <see cref="B"/> as hex <see cref="string"/>.
        /// </summary>
        public string BHex => B.ToString("X2");

        /// <summary>
        /// Gets RGB as hex <see cref="string"/> in the format #RRGGBB.
        /// </summary>
        public string RGBHex => $"#{RHex}{GHex}{BHex}";

        /// <summary>
        /// Gets <c>true</c> if this color is black.
        /// </summary>
        public bool IsBlack => EqualARGB(Color.Black);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Color"/> structure is
        /// a named color or a member of the <see cref="KnownColor"/> enumeration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Color"/> was created by using either
        /// the <see cref="FromName"/> method or the
        /// <see cref="FromKnownColor(KnownColor)"/> method; otherwise, <c>false</c>.
        /// </value>
        public bool IsNamedColor => ((state & StateNameValid) != 0) || IsKnownColor;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Color"/> structure is
        /// a system color.
        /// A system color is a color that is used in a Windows display element.
        /// System colors are represented by elements of the
        /// <see cref="KnownColor"/> enumeration.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="Color"/> was created from a system
        /// color by using either the <see cref="FromName"/> method
        /// or the <see cref="FromKnownColor(KnownColor)"/> method;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool IsSystemColor =>
            IsKnownColor && IsKnownColorSystem((KnownColor)knownColor);

        /// <summary>
        /// Creates <see cref="SolidBrush"/> instance for this color.
        /// </summary>
        [Browsable(false)]
        public SolidBrush AsBrush => new(this);

        /// <summary>
        /// Creates <see cref="Pen"/> instance for this color.
        /// </summary>
        [Browsable(false)]
        public Pen AsPen => GetAsPen(1);

        /// <summary>
        /// Gets the name of this <see cref="Color"/>.
        /// </summary>
        /// <value>The name of this <see cref="Color"/>.</value>
        /// <remarks>
        /// This method returns either the user-defined name of the color, if
        /// the color was created from a name,
        /// or the name of the known color. For custom colors, the RGB value
        /// is returned.
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
                    string tablename =
                        KnownColorNames.KnownColorToName((KnownColor)knownColor);
                    if (tablename != null)
                        return tablename;
                    throw new InvalidOperationException(
                        $"Could not find known color '{(KnownColor)knownColor}'");
                }

                // if we reached here, just encode the value
                return value.ToString("x");
            }
        }

        /// <summary>
        /// Gets color name and ARGB for the debug purposes.
        /// </summary>
        public string NameAndARGBValue =>
            $"{{Name={Name}, ARGB=({A}, {R}, {G}, {B})}}";

        internal long Value
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

        /// <summary>
        /// Converts the specified <see cref='RGBValue'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(RGBValue rgb) =>
            Color.FromRgb(rgb.R, rgb.G, rgb.B);

        /// <summary>
        /// Converts the specified <see cref='System.Drawing.Color'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(System.Drawing.Color color)
        {
            if (color.IsEmpty)
                return Color.Empty;
            var argb = color.ToArgb();
            return FromArgb(argb);
        }

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='System.Drawing.Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Color(Color color)
        {
            if (color.IsEmpty)
                return Color.Empty;
            var argb = color.ToArgb();
            return System.Drawing.Color.FromArgb(argb);
        }

        /// <summary>
        /// Converts the specified <see cref='RGBValue'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SolidBrush(Color color) => color.AsBrush;

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='RGBValue'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RGBValue(Color color) =>
            new(color.R, color.G, color.B);

        /// <summary>
        /// Converts the specified <see cref='string'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Color(string s)
            => Color.Parse(s);

        /// <summary>
        /// Tests whether two specified <see cref="Color"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="Color"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color left, Color right) =>
            left.value == right.value
                && left.state == right.state
                && left.knownColor == right.knownColor
                && left.name == right.name;

        /// <summary>
        /// Tests whether two specified <see cref="Color"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left
        /// of the inequality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right
        /// of the inequality operator.</param>
        /// <returns><c>true</c> if the two <see cref="Color"/> structures
        /// are different; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Color left, Color right) => !(left == right);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from a 32-bit ARGB value.
        /// </summary>
        /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
        /// <returns>The <see cref="Color"/> structure that this method creates.
        /// </returns>
        /// <remarks>
        /// The byte-ordering of the 32-bit ARGB value is AARRGGBB.
        /// The most significant byte (MSB), represented by AA,
        /// is the alpha component value. The second, third, and fourth bytes,
        /// represented by RR, GG, and BB,
        /// respectively, are the color components red, green, and blue, respectively.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromArgb(int argb) => FromArgb(unchecked((uint)argb));

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the four ARGB
        /// component (alpha, red, green, and blue) values.
        /// Although this method allows a 32-bit value to be passed for each
        /// component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="alpha">The alpha component. Valid values are 0 through 255.
        /// </param>
        /// <param name="red">The red component. Valid values are 0 through 255.
        /// </param>
        /// <param name="green">The green component. Valid values are 0 through 255.
        /// </param>
        /// <param name="blue">The blue component. Valid values are 0 through 255.
        /// </param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>To create an opaque color, set alpha to 255. To create
        /// a semitransparent color, set alpha to any value from 1 through 254.
        /// </remarks>
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
                (uint)blue << ARGBBlueShift);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the four ARGB
        /// component (alpha, red, green, and blue) values.
        /// </summary>
        /// <param name="alpha">The alpha component.</param>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>To create an opaque color, set alpha to 255. To create
        /// a semitransparent color, set alpha to any value from 1 through 254.
        /// </remarks>
        public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)red << ARGBRedShift |
                (uint)green << ARGBGreenShift |
                (uint)blue << ARGBBlueShift);
        }

        /// <summary>
        /// Creates an opaque <see cref="Color"/> structure from the four ARGB
        /// components (255, <paramref name="red"/>, <paramref name="green"/>,
        /// and <paramref name="blue"/>) values.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromRgb(byte red, byte green, byte blue) =>
            FromArgb((byte)255, red, green, blue);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified
        /// <see cref="Color"/> structure, but with the new specified alpha value.
        /// Although this method allows a 32-bit value to be passed for the
        /// alpha value, the value is limited to 8 bits.
        /// </summary>
        /// <param name="alpha">The alpha value for the new <see cref="Color"/>.
        /// Valid values are 0 through 255.</param>
        /// <param name="baseColor">The <see cref="Color"/> from which to create
        /// the new Color.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>To create an opaque color, set alpha to 255. To create
        /// a semitransparent color, set alpha to any value from 1 through 254.
        /// </remarks>
        public static Color FromArgb(int alpha, Color baseColor)
        {
            CheckByte(alpha, nameof(alpha));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift |
                (uint)baseColor.Value & ~ARGBAlphaMask);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified 8-bit
        /// color values (red, green, and blue).
        /// The alpha value is implicitly 255 (fully opaque). Although this
        /// method allows a 32-bit value to be passed
        /// for each color component, the value of each component is limited to
        /// 8 bits.
        /// </summary>
        /// <param name="red">The red component value for the new
        /// <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <param name="green">The green component value for the new
        /// <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the new
        /// <see cref="Color"/>. Valid values are 0 through 255.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <exception cref="ArgumentException"><c>red</c>, <c>green</c>, or
        /// <c>blue</c> is less than 0 or greater than 255.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromArgb(int red, int green, int blue) =>
            FromArgb(byte.MaxValue, red, green, blue);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified
        /// predefined color.
        /// </summary>
        /// <param name="color">An element of the
        /// <see cref="KnownColor"/> enumeration.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>A predefined color is also called a known color and is
        /// represented by an element of the <see cref="KnownColor"/> enumeration.
        /// </remarks>
        public static Color FromKnownColor(KnownColor color) =>
            color <= 0 || color > KnownColor.RebeccaPurple ?
            FromName(color.ToString()) : new Color(color);

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the specified name of
        /// a predefined color.
        /// </summary>
        /// <param name="name">A string that is the name of a predefined color.
        /// Valid names are the same as the names of the elements of the
        /// <see cref="KnownColor"/> enumeration.</param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>
        /// A predefined color is also called a known color and is represented by
        /// an element of the <see cref="KnownColor"/> enumeration.
        /// If the name parameter is not the valid name of a predefined color, the
        /// <see cref="FromName"/> method creates a <see cref="Color"/> structure
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

        /// <summary>
        /// Returns a color from the specified string.
        /// </summary>
        public static Color Parse(string s)
        {
            object? result = new ColorConverter().ConvertFromString(s);
            if (result == null)
                return Color.Empty;
            return (Color)result;
        }

        /// <summary>
        /// Enumerates colors defined in <see cref="KnownColor"/>.
        /// </summary>
        public static IReadOnlyList<Color> GetKnownColors()
        {
            List<Color> colors = [];

            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
            {
                colors.Add(new Color(knownColor));
            }

            colors.Sort(new ColorNameComparer());
            return colors;
        }

        /// <summary>
        /// Converts RGB color to <see cref="HSVValue"/>.
        /// </summary>
        /// <param name="rgb">RGB Color.</param>
        /// <returns></returns>
        public static HSVValue RGBtoHSV(RGBValue rgb)
        {
            const int RED = 0;
            const int GREEN = 1;
            const int BLUE = 2;

            double red = rgb.R / 255.0;
            double green = rgb.G / 255.0;
            double blue = rgb.B / 255.0;

            // find the min and max intensity (and remember which one was it for the
            // latter)
            double minimumRGB = red;
            if (green < minimumRGB)
                minimumRGB = green;
            if (blue < minimumRGB)
                minimumRGB = blue;

            int chMax = RED;

            double maximumRGB = red;
            if (green > maximumRGB)
            {
                chMax = GREEN;
                maximumRGB = green;
            }

            if (blue > maximumRGB)
            {
                chMax = BLUE;
                maximumRGB = blue;
            }

            double value = maximumRGB;

            double hue = 0.0, saturation;
            double deltaRGB = maximumRGB - minimumRGB;
            if (deltaRGB == 0)
            {
                // Gray has no color
                hue = 0.0;
                saturation = 0.0;
            }
            else
            {
                switch (chMax)
                {
                    case RED:
                        hue = (green - blue) / deltaRGB;
                        break;

                    case GREEN:
                        hue = 2.0 + ((blue - red) / deltaRGB);
                        break;

                    case BLUE:
                        hue = 4.0 + ((red - green) / deltaRGB);
                        break;
                }

                hue /= 6.0;

                if (hue < 0.0)
                    hue += 1.0;

                saturation = deltaRGB / maximumRGB;
            }

            return new HSVValue(hue, saturation, value);
        }

        /// <summary>
        /// Darkens or lightens a color, based on the specified percentage
        /// ialpha of 0 would be completely black, 200 completely white
        /// an ialpha of 100 returns the same color.
        /// </summary>
        /// <param name="rgb">RGB Color.</param>
        /// <param name="ialpha">Lightness value (0..200).</param>
        public static void ChangeLightness(ref RGBValue rgb, int ialpha)
        {
            if (ialpha == 100) return;

            // ialpha is 0..200 where 0 is completely black
            // and 200 is completely white and 100 is the same
            // convert that to normal alpha 0.0 - 1.0
            ialpha = Math.Max(ialpha, 0);
            ialpha = Math.Min(ialpha, 200);
            double alpha = ((double)(ialpha - 100.0)) / 100.0;

            byte bg;
            if (ialpha > 100)
            {
                // blend with white
                bg = 255;
                alpha = 1.0 - alpha;  // 0 = transparent fg; 1 = opaque fg
            }
            else
            {
                // blend with black
                bg = 0;
                alpha = 1.0 + alpha;  // 0 = transparent fg; 1 = opaque fg
            }

            rgb.R = AlphaBlend(rgb.R, bg, alpha);
            rgb.G = AlphaBlend(rgb.G, bg, alpha);
            rgb.B = AlphaBlend(rgb.B, bg, alpha);
        }

        /// <summary>
        /// Assigns the same value to RGB of the color: 0 if <paramref name="on"/> is false, 255 otherwise.
        /// </summary>
        /// <param name="rgb">Color.</param>
        /// <param name="on">Color on/off selector.</param>
        public static void MakeMono(ref RGBValue rgb, bool on)
        {
            byte v = on ? (byte)255 : (byte)0;
            rgb.R = rgb.G = rgb.B = v;
        }

        /// <summary>
        /// Makes color gray using integer arithmetic.
        /// </summary>
        /// <param name="rgb">Color.</param>
        public static void MakeGray(ref RGBValue rgb)
        {
            var v = (byte)(((rgb.B * 117UL) + (rgb.G * 601UL) + (rgb.R * 306UL)) >> 10);
            rgb.R = rgb.G = rgb.B = v;
        }

        /// <summary>
        /// Creates a grey color from rgb parameters using floating point arithmetic.
        /// </summary>
        /// <remarks>
        /// Defaults to using the standard ITU-T BT.601 when converting to YUV, where every
        /// pixel equals(R* weight_r) + (G* weight_g) + (B* weight_b).
        /// </remarks>
        /// <param name="rgb">Color.</param>
        /// <param name="weight_r">Weight of R component of a color.</param>
        /// <param name="weight_g">Weight of G component of a color.</param>
        /// <param name="weight_b">Weight of B component of a color.</param>
        public static void MakeGrey(ref RGBValue rgb, double weight_r, double weight_g, double weight_b)
        {
            double luma = (rgb.R * weight_r) + (rgb.G * weight_g) + (rgb.B * weight_b);
            var v = (byte)Math.Round(luma);
            rgb.R = rgb.G = rgb.B = v;
        }

        /// <summary>
        /// Sets a disabled (dimmed) color for specified <see cref="RGBValue"/>.
        /// </summary>
        /// <param name="rgb">Color.</param>
        /// <param name="brightness"></param>
        public static void MakeDisabled(ref RGBValue rgb, byte brightness)
        {
            rgb.R = AlphaBlend(rgb.R, brightness, 0.4);
            rgb.G = AlphaBlend(rgb.G, brightness, 0.4);
            rgb.B = AlphaBlend(rgb.B, brightness, 0.4);
        }

        /// <summary>
        /// Converts <see cref="HSVValue"/> to RGB color.
        /// </summary>
        /// <param name="hsv"></param>
        public static RGBValue HSVtoRGB(HSVValue hsv)
        {
            double red, green, blue;

            if (hsv.Saturation == 0)
            {
                // Grey
                red = hsv.Value;
                green = hsv.Value;
                blue = hsv.Value;
            }
            else
            {
                // not grey
                double hue = hsv.Hue * 6.0; // sector 0 to 5
                int i = (int)Math.Floor(hue);
                double f = hue - i; // fractional part of h
                double p = hsv.Value * (1.0 - hsv.Saturation);

                switch (i)
                {
                    case 0:
                        red = hsv.Value;
                        green = hsv.Value * (1.0 - (hsv.Saturation * (1.0 - f)));
                        blue = p;
                        break;

                    case 1:
                        red = hsv.Value * (1.0 - (hsv.Saturation * f));
                        green = hsv.Value;
                        blue = p;
                        break;

                    case 2:
                        red = p;
                        green = hsv.Value;
                        blue = hsv.Value * (1.0 - (hsv.Saturation * (1.0 - f)));
                        break;

                    case 3:
                        red = p;
                        green = hsv.Value * (1.0 - (hsv.Saturation * f));
                        blue = hsv.Value;
                        break;

                    case 4:
                        red = hsv.Value * (1.0 - (hsv.Saturation * (1.0 - f)));
                        green = p;
                        blue = hsv.Value;
                        break;

                    // case 5:
                    default:
                        red = hsv.Value;
                        green = p;
                        blue = hsv.Value * (1.0 - (hsv.Saturation * f));
                        break;
                }
            }

            return new RGBValue(
                (byte)Math.Round(red * 255.0),
                (byte)Math.Round(green * 255.0),
                (byte)Math.Round(blue * 255.0));
        }

        /// <summary>
        /// Enumerates colors defined in <see cref="KnownColor"/> for the specified
        /// color categories.
        /// </summary>
        public static IReadOnlyList<Color> GetKnownColors(params KnownColorCategory[] cats)
        {
            List<Color> colors = [];

            var items = ColorUtils.GetColorInfos();

            foreach (var item in items)
            {
                if (!item.Visible)
                    continue;
                if (!item.CategoryIs(cats))
                    continue;
                colors.Add(new Color(item.KnownColor));
            }

            colors.Sort(new ColorNameComparer());
            return colors;
        }

        /// <summary>
        /// Used by <see cref="ChangeLightness(int)"/> and
        /// <see cref="MakeDisabled(ref RGBValue, byte)"/>.
        /// </summary>
        /// <param name="fg"></param>
        /// <param name="bg"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static byte AlphaBlend(byte fg, byte bg, double alpha)
        {
            double result = bg + (alpha * (fg - bg));
            result = Math.Max(result, 0.0);
            result = Math.Min(result, 255.0);
            return (byte)result;
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) lightness value for this
        /// <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The lightness of this <see cref="Color"/>. The lightness
        /// ranges from 0.0 through 1.0, where 0.0 represents black and 1.0
        /// represents white.</returns>
        public double GetBrightness()
        {
            GetRgbValues(out int r, out int g, out int b);

            MinMaxRgb(out int min, out int max, r, g, b);

            return (max + min) / (byte.MaxValue * 2f);
        }

        /// <summary>
        /// Returns the perceived brightness of the color, with 0 for black and 1
        /// for white.
        /// </summary>
        public double GetLuminance()
        {
            return ((0.299 * R) + (0.587 * G) + (0.114 * B)) / 255.0;
        }

        /// <summary>
        /// Darkens or lightens a color, based on the specified percentage
        /// ialpha of 0 would be completely black, 200 completely white
        /// an ialpha of 100 returns the same color.
        /// </summary>
        /// <param name="ialpha">Lightness value (0..200).</param>
        public Color ChangeLightness(int ialpha)
        {
            RGBValue rgb = this;
            ChangeLightness(ref rgb, ialpha);
            return rgb;
        }

        /// <summary>
        /// Makes a disabled version of this color.
        /// </summary>
        /// <param name="brightness"></param>
        /// <returns></returns>
        public Color MakeDisabled(byte brightness)
        {
            RGBValue rgb = this;
            MakeDisabled(ref rgb, brightness);
            var result = Color.FromArgb(A, rgb.R, rgb.G, rgb.B);
            return result;
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) hue value, in degrees, for
        /// this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The hue, in degrees, of this <see cref="Color"/>. The hue
        /// is measured in degrees, ranging from 0.0 through 360.0, in HSL
        /// color space.</returns>
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
                hue = ((b - r) / delta) + 2f;
            else
                hue = ((r - g) / delta) + 4f;

            hue *= 60f;
            if (hue < 0f)
                hue += 360f;

            return hue;
        }

        /// <summary>
        /// Gets the hue-saturation-lightness (HSL) saturation value for this
        /// <see cref="Color"/> structure.
        /// </summary>
        /// <returns>
        /// The saturation of this <see cref="Color"/>. The saturation ranges
        /// from 0.0 through 1.0,
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
                div = (byte.MaxValue * 2) - max - min;

            return (max - min) / (double)div;
        }

        /// <summary>
        /// Gets this color as <see cref="Pen"/> with the specified width.
        /// </summary>
        /// <param name="width">Width of the pen.</param>
        /// <returns></returns>
        public Pen GetAsPen(double width) => new(this, width);

        /// <summary>
        /// Gets this color as <see cref="Pen"/> with the specified width
        /// and <see cref="DashStyle"/>.
        /// </summary>
        /// <param name="width">Width of the pen.</param>
        /// <param name="dashStyle">Dash style of the pen.</param>
        /// <returns></returns>
        public Pen GetAsPen(double width, DashStyle dashStyle) => new(this, width, dashStyle);

        /// <summary>
        /// Returns ARGB values of the <see cref="Color"/>
        /// </summary>
        /// <param name="a">Value of <see cref="A"/>.</param>
        /// <param name="r">Value of <see cref="R"/>.</param>
        /// <param name="g">Value of <see cref="G"/>.</param>
        /// <param name="b">Value of <see cref="B"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetArgbValues(out byte a, out byte r, out byte g, out byte b)
        {
            var value = Value;
            r = unchecked((byte)(value >> ARGBRedShift));
            g = unchecked((byte)(value >> ARGBGreenShift));
            b = unchecked((byte)(value >> ARGBBlueShift));
            a = unchecked((byte)(value >> ARGBAlphaShift));
        }

        /// <summary>
        /// Gets the 32-bit ARGB value of this <see cref="Color"/> structure.
        /// </summary>
        /// <returns>The 32-bit ARGB value of this <see cref="Color"/>.</returns>
        /// <remarks>
        /// The byte-ordering of the 32-bit ARGB value is AARRGGBB.
        /// The most significant byte (MSB), represented by AA,
        /// is the alpha component value. The second, third, and fourth
        /// bytes, represented by RR, GG, and BB,
        /// respectively, are the color components red, green, and blue,
        /// respectively.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ToArgb() => unchecked((int)Value);

        /// <inheritdoc cref="ToArgb"/>
        /// <remarks>
        /// This is similar to <see cref="ToArgb"/> but returns color as <see cref="uint"/>.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint AsUInt() => unchecked((uint)Value);

        /// <summary>
        /// Gets color properties for the debug purposes.
        /// </summary>
        public string ToDebugString() =>
            $"{{Name={Name}, KnownColor={(KnownColor)knownColor}, ARGB=({A}, {R}, {G}, {B}), State={state}}}";

        /// <summary>
        /// Gets the <see cref="KnownColor"/> value of this
        /// <see cref="Color"/> structure.
        /// </summary>
        /// <returns>
        /// An element of the <see cref="KnownColor"/> enumeration, if the
        /// <see cref="Color"/> is created from a predefined
        /// color by using either the <see cref="FromName"/> method or the
        /// <see cref="FromKnownColor(KnownColor)"/> method; otherwise, 0.
        /// </returns>
        /// <remarks>
        /// A predefined color is also called a known color and is represented
        /// by an element of the <see cref="KnownColor"/> enumeration.
        /// When the <see cref="ToKnownColor"/> method is applied to a
        /// <see cref="Color"/> structure that is created by using
        /// the <see cref="FromArgb(int)"/> method,
        /// <see cref="ToKnownColor"/> returns 0, even if the ARGB value
        /// matches the ARGB value of a predefined color.
        /// <see cref="ToKnownColor"/> also returns 0 when it is applied
        /// to a <see cref="Color"/> structure that is created by using
        /// the <see cref="FromName"/> method with a string name that is not valid.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KnownColor ToKnownColor() => (KnownColor)knownColor;

        /// <summary>
        /// Converts this <see cref="Color"/> structure to a human-readable string.
        /// </summary>
        /// <returns>
        /// A string that is the name of this <see cref="Color"/>, if the
        /// <see cref="Color"/> is created from a predefined color by using either the
        /// <see cref="FromName"/> method or the
        /// <see cref="FromKnownColor(KnownColor)"/> method;
        /// otherwise, a string that consists of the ARGB component names and
        /// their values.
        /// </returns>
        /// <remarks>
        /// A predefined color is also called a known color and is represented by
        /// an element of the <see cref="KnownColor"/> enumeration.
        /// When the <see cref="ToString"/> method is applied to
        /// a <see cref="Color"/> structure that is created by using the
        /// <see cref="FromArgb(int)"/> method,
        /// <see cref="ToString"/> returns a string that consists of the ARGB
        /// component names and their values, even if the ARGB value matches the ARGB
        /// value of a predefined color.
        /// </remarks>
        public override string ToString() =>
            IsNamedColor ? $"{nameof(Color)} [{Name}]" :
            (state & StateValueMask) != 0 ?
            $"{nameof(Color)} [A={A}, R={R}, G={G}, B={B}]" :
            $"{nameof(Color)} [Empty]";

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is Color other && Equals(other);

        /// <summary>
        /// Indicates whether the current object is equal to another object of
        /// the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Color other) => this == other;

        /// <summary>
        /// Indicates whether ARGB of this color is equal to ARGB of another color.
        /// </summary>
        /// <param name="other">A <see cref="Color"/> to compare with this color.</param>
        /// <returns><c>true</c> if ARGB of the colors are equal;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualARGB(Color other) => Value == other.Value;

        /// <summary>
        /// Creates <see cref="GenericImage"/> of the specified <paramref name="size"/>
        /// filled with this color.
        /// </summary>
        /// <param name="size">Size of the created image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage AsImage(Int32Size size)
        {
            GenericImage image = new(size.Width, size.Height);
            image.SetRGBRect(this);
            return image;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            // Three cases:
            // 1. We don't have a name. All relevant data, including this fact,
            // is in the remaining fields.
            // 2. We have a known name. The name will be the same instance of any
            // other with the same
            // knownColor value, so we can ignore it for hashing. Note this also
            // hashes different to
            // an unnamed color with the same ARGB value.
            // 3. Have an unknown name. Will differ from other unknown-named
            // colors only by name, so we
            // can usefully use the names hash code alone.
            if (name != null && !IsKnownColor)
                return name.GetHashCode();

            return HashCode.Combine(
                value.GetHashCode(),
                state.GetHashCode(),
                knownColor.GetHashCode());
        }

        internal static bool IsKnownColorSystem(KnownColor knownColor)
             => KnownColorTable.ColorKindTable[(int)knownColor] ==
                 KnownColorTable.KnownColorKindSystem;

        private static void CheckByte(int value, string name)
        {
            static void ThrowOutOfByteRange(int v, string n) =>
                throw new ArgumentException(
                    string.Format(
                        "Variable {0} has invalid value {1}." + " " +
                        "Minimum allowed value is {2}, maximum is {3}.",
                        new object[] { n, v, byte.MinValue, byte.MaxValue }));

            if (unchecked((uint)value) > byte.MaxValue)
                ThrowOutOfByteRange(value, name);
        }

        private static Color FromArgb(uint argb) =>
            new(argb, StateARGBValueValid, null, (KnownColor)0);

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
        /// Returns RGB values of the <see cref="Color"/>
        /// </summary>
        /// <param name="r">Value of <see cref="R"/>.</param>
        /// <param name="g">Value of <see cref="G"/>.</param>
        /// <param name="b">Value of <see cref="B"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void GetRgbValues(out int r, out int g, out int b)
        {
            uint value = (uint)Value;
            r = (int)(value & ARGBRedMask) >> ARGBRedShift;
            g = (int)(value & ARGBGreenMask) >> ARGBGreenShift;
            b = (int)(value & ARGBBlueMask) >> ARGBBlueShift;
        }

        internal class ColorNameComparer : IComparer<Color>
        {
            public int Compare(Color color1, Color color2)
            {
                var name1 = color1.Name;
                var name2 = color2.Name;
                return string.Compare(name1, name2);
            }
        }
    }
}