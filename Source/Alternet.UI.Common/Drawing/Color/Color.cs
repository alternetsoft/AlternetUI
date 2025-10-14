using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Alternet.UI;

using SkiaSharp;

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
    [DebuggerDisplay("{DebugString}")]
    [Serializable]
    [TypeConverter(typeof(ColorConverter))]
    public partial class Color : IEquatable<Color>
    {
        /// <summary>
        /// Shift count for Alpha component of the color.
        /// </summary>
        public static readonly int ARGBAlphaShift = 24;

        /// <summary>
        /// Shift count for Red component of the color.
        /// </summary>
        public static readonly int ARGBRedShift = 16;

        /// <summary>
        /// Shift count for Green component of the color.
        /// </summary>
        public static readonly int ARGBGreenShift = 8;

        /// <summary>
        /// Shift count for Blue component of the color.
        /// </summary>
        public static readonly int ARGBBlueShift = 0;

        /// <summary>
        /// Bit mask for Alpha component of the color.
        /// </summary>
        public static readonly uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;

        /// <summary>
        /// Bit mask for Red component of the color.
        /// </summary>
        public static readonly uint ARGBRedMask = 0xFFu << ARGBRedShift;

        /// <summary>
        /// Bit mask for Green component of the color.
        /// </summary>
        public static readonly uint ARGBGreenMask = 0xFFu << ARGBGreenShift;

        /// <summary>
        /// Bit mask for Blue component of the color.
        /// </summary>
        public static readonly uint ARGBBlueMask = 0xFFu << ARGBBlueShift;

        /// <summary>
        /// Represents an empty color.
        /// </summary>
        public static readonly Color Empty = new();

        /// <summary>
        /// Represents the maximum width, in dips, of a pen that can be cached
        /// in <see cref="GetPen"/> method.
        /// </summary>
        /// <remarks>This value is used to limit the size of pens stored in the cache to optimize
        /// performance and memory usage. Pens with a width greater
        /// than this value will not be cached by <see cref="GetPen"/> method.</remarks>
        public static readonly int MaxCachedPenWidth = 10;

        // User supplied name of color. Will not be filled in if
        // we map to a known color.
        private readonly string? name;

        private ColorStruct color;

        // Ignored, unless "state" says it is valid
#pragma warning disable
        private readonly KnownColor knownColor;
#pragma warning restore

        private readonly StateFlags state;

        private CachedResources resources = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class with the specified parameters.
        /// </summary>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(byte red, byte green, byte blue)
        {
            color.A = 255;
            color.R = red;
            color.G = green;
            color.B = blue;
            state = StateFlags.ValueValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class with the specified parameters.
        /// </summary>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        /// <param name="alpha">Alpha component of the color.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            color.A = alpha;
            color.R = red;
            color.G = green;
            color.B = blue;
            state = StateFlags.ValueValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class from
        /// the <see cref="ColorStruct"/>.
        /// </summary>
        /// <param name="value">Color specified using <see cref="ColorStruct"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(ColorStruct value)
        {
            color = value;
            state = StateFlags.ValueValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class
        /// from the <see cref="KnownColor"/>.
        /// </summary>
        /// <param name="knownColor">Color specified using <see cref="KnownColor"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(KnownColor knownColor)
        {
            color.Value = 0;
            state = StateFlags.KnownColorValid;
            this.knownColor = knownColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class from
        /// the <see cref="KnownSystemColor"/>.
        /// </summary>
        /// <param name="knownColor">Color specified using <see cref="KnownSystemColor"/> value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(KnownSystemColor knownColor)
        {
            color.Value = 0;
            state = StateFlags.KnownColorValid;
            this.knownColor = (KnownColor)knownColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class with the specified parameters.
        /// </summary>
        /// <param name="value">Value specified as <see cref="uint"/>.</param>
        /// <param name="state">State flags.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(uint value, StateFlags state)
        {
            color.Value = value;
            this.state = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class with the specified parameters.
        /// </summary>
        /// <param name="value">Value specified as <see cref="uint"/>.</param>
        /// <param name="state">State flags.</param>
        /// <param name="name">Name of the color.</param>
        /// <param name="knownColor"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color(uint value, StateFlags state, string? name, KnownColor knownColor)
        {
            color.Value = value;
            this.state = state;
            this.name = name;
            this.knownColor = knownColor;
        }

        /// <summary>
        /// Occurs when <see cref="string"/> is converted to <see cref="Color"/>.
        /// </summary>
        public static event EventHandler<ValueConvertEventArgs<string?, Color?>>? StringToColor;

        /// <summary>
        /// Occurs when <see cref="Color"/> is converted to <see cref="string"/>.
        /// </summary>
        public static event EventHandler<ValueConvertEventArgs<Color?, string?>>? ColorToString;

        /// <summary>
        /// Occurs when <see cref="Color"/> is converted to <see cref="string"/>
        /// for the display purposes.
        /// </summary>
        public static event EventHandler<ValueConvertEventArgs<Color?, string?>>? ColorToDisplayString;

        /// <summary>
        /// Enumerates color value state flags.
        /// </summary>
        [Flags]
        public enum StateFlags : short
        {
            /// <summary>
            /// Represents the absence of any specific options or flags.
            /// </summary>
            None = 0,

            /// <summary>
            /// KnownColor property is specified and valid.
            /// </summary>
            KnownColorValid = 1 << 0,

            /// <summary>
            /// Value property is valid.
            /// </summary>
            ValueValid = 1 << 1,

            /// <summary>
            /// Name property is valid.
            /// </summary>
            NameValid = 1 << 2,

            /// <summary>
            /// Represents a combination of flags indicating that both
            /// the color name and the known color value are valid.
            /// </summary>
            /// <remarks>This value is a bitwise combination of the <see cref="KnownColorValid"/> and
            /// <see cref="NameValid"/> flags. It is used to validate that both
            /// the color name and the known color value
            /// meet the required conditions.</remarks>
            NameAndKnownColorValid = KnownColorValid | NameValid,
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
        [Browsable(false)]
        public byte R
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color.R;
            }
        }

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
        [Browsable(false)]
        public byte G
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color.G;
            }
        }

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
        [Browsable(false)]
        public byte B
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color.B;
            }
        }

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
        [Browsable(false)]
        public byte A
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color.A;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if color is opaque (<see cref="A"/> is 255).
        /// </summary>
        [Browsable(false)]
        public bool IsOpaque
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return A == 255;
            }
        }

        /// <summary>
        /// Gets state flags of this object.
        /// </summary>
        [Browsable(false)]
        public StateFlags State
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return state;
            }
        }

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
        [Browsable(false)]
        public bool IsKnownColor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return state.HasFlag(StateFlags.KnownColorValid);
            }
        }

        /// <summary>
        /// Specifies whether this <see cref="Color"/> structure is uninitialized.
        /// </summary>
        /// <value>This property returns <c>true</c> if this color is uninitialized;
        /// otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return state == 0;
            }
        }

        /// <summary>
        /// Specifies whether this <see cref="Color"/> structure is initialized.
        /// </summary>
        /// <value>This property returns <c>true</c> if this color is initialized;
        /// otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public bool IsOk
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return state != 0;
            }
        }

        /// <summary>
        /// Gets <see cref="A"/> as hex <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public string AHex => A.ToString("X2");

        /// <summary>
        /// Gets the current color based on whether a dark or light color scheme is in use.
        /// For regular <see cref="Color"/> instances, it simply returns the instance itself.
        /// For <see cref="LightDarkColor"/> instances, it returns either the dark or light color.
        /// </summary>
        public virtual Color Current
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets <see cref="R"/> as hex <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public string RHex => R.ToString("X2");

        /// <summary>
        /// Gets <see cref="G"/> as hex <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public string GHex => G.ToString("X2");

        /// <summary>
        /// Gets <see cref="B"/> as hex <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public string BHex => B.ToString("X2");

        /// <summary>
        /// Gets RGB as hex <see cref="string"/> in the format #RRGGBB.
        /// </summary>
        [Browsable(false)]
        public string RGBHex
        {
            get
            {
                return $"#{RHex}{GHex}{BHex}";
            }
        }

        /// <summary>
        /// Gets RGB as web <see cref="string"/> in the format "rgb({R},{G},{B})".
        /// Fo example for the black color it will return "rgb(0,0,0)".
        /// </summary>
        [Browsable(false)]
        public string RGBWeb
        {
            get
            {
                RequireArgb();
                return $"rgb({color.R},{color.G},{color.B})";
            }
        }

        /// <summary>
        /// If color is opaque returns <see cref="RGBWeb"/>; otherwise
        /// returns "rgba({R},{G},{B}, {alpha})" string.
        /// </summary>
        /// <remarks>
        /// The alpha parameter in the result is a number between 0.0 (fully transparent)
        /// and 1.0 (not transparent at all).
        /// </remarks>
        public string ARGBWeb
        {
            get
            {
                RequireArgb();

                if (color.A == 255)
                    return $"rgb({color.R},{color.G},{color.B})";

                double a = color.A;
                a /= 255;

                var s = a.ToString("0.##");

                return $"rgba({color.R},{color.G},{color.B}, {s})";
            }
        }

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
        public bool IsNamedColor
        {
            get
            {
                return (state & StateFlags.NameAndKnownColorValid) != 0;
            }
        }

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
        public bool IsSystemColor
        {
            get
            {
                return IsKnownColor && IsKnownColorSystem(knownColor);
            }
        }

        /// <summary>
        /// Creates <see cref="SolidBrush"/> instance for this color.
        /// </summary>
        [Browsable(false)]
        public SolidBrush AsBrush
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return resources.AsBrush ??= new(this, immutable: true);
            }
        }

        /// <summary>
        /// Gets a <see cref="Pen"/> instance representing the current state of the object.
        /// </summary>
        [Browsable(false)]
        public Pen AsPen
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return resources.AsPen ??= CreatePenInstance(1);
            }
        }

        /// <summary>
        /// Gets <see cref="SKPaint"/> for this color with
        /// <see cref="SKPaintStyle.Fill"/> style.
        /// </summary>
        [Browsable(false)]
        public SKPaint AsStrokeAndFillPaint
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return resources.StrokeAndFillPaint ??= GraphicsFactory.ColorToStrokeAndFillPaint(this);
            }
        }

        /// <summary>
        /// Gets <see cref="SKPaint"/> for this color with
        /// <see cref="SKPaintStyle.Stroke"/> style.
        /// </summary>
        [Browsable(false)]
        public SKPaint AsStrokePaint
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return resources.StrokePaint ??= GraphicsFactory.ColorToStrokePaint(this);
            }
        }

        /// <summary>
        /// Gets <see cref="SKPaint"/> for this color with
        /// <see cref="SKPaintStyle.Fill"/> style.
        /// </summary>
        [Browsable(false)]
        public SKPaint AsFillPaint
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return resources.FillPaint ??= GraphicsFactory.ColorToFillPaint(this);
            }
        }

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
                if (state.HasFlag(StateFlags.NameValid))
                {
                    return name ?? string.Empty;
                }

                if (IsKnownColor)
                {
                    string tableName =
                        KnownColorNames.KnownColorToName(knownColor);
                    if (tableName != null)
                        return tableName;
                    throw new InvalidOperationException(
                        $"Could not find known color '{knownColor}'");
                }

                return color.Value.ToString("x");
            }
        }

        /// <summary>
        /// Gets the localized name of this <see cref="Color"/>.
        /// </summary>
        /// <value>The localized name of this <see cref="Color"/>.</value>
        /// <remarks>
        /// This method returns either the user-defined name of the color, if
        /// the color was created from a name,
        /// or the name of the known color. For custom colors, the RGB value
        /// is returned.
        /// </remarks>
        /// <remarks>
        /// In order to get localized name of the known color, getter of this property calls
        /// <see cref="ColorUtils.GetColorInfo(KnownColor)"/> and uses
        /// <see cref="IKnownColorInfo.LabelLocalized"/> property.
        /// </remarks>
        public string NameLocalized
        {
            get
            {
                if (IsKnownColor)
                {
                    var info = ColorUtils.GetColorInfo(knownColor);
                    var result = info.LabelLocalized;
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }

                return Name;
            }
        }

        /// <summary>
        /// Gets color value as text the debug purposes.
        /// </summary>
        [Browsable(false)]
        public virtual string DebugString
        {
            get
            {
                return NameAndARGBValue;
            }
        }

        /// <summary>
        /// Gets color name and ARGB.
        /// </summary>
        [Browsable(false)]
        public string NameAndARGBValue
        {
            get
            {
                RequireArgb();
                return $"{{Name={Name}, ARGB=({color.A}, {color.R}, {color.G}, {color.B})}}";
            }
        }

        /// <summary>
        /// Gets this color as <see cref="SKColor"/>.
        /// </summary>
        [Browsable(false)]
        public SKColor SkiaColor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color.Color;
            }
        }

        /// <summary>
        /// Gets this color as <see cref="ColorStruct"/>.
        /// </summary>
        public ColorStruct AsStruct
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color;
            }
        }

        internal uint Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                RequireArgb();
                return color.Value;
            }
        }

        /// <summary>
        /// Converts the specified <see cref='RGBValue'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(RGBValue rgb) => new(rgb.R, rgb.G, rgb.B);

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='SKColor'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SKColor(Color color)
        {
            return color?.SkiaColor ?? SKColor.Empty;
        }

        /// <summary>
        /// Converts the specified <see cref='SKColor'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(SKColor color)
        {
            return new(color);
        }

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='Brush'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Brush(Color color) => color.AsBrush;

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='Pen'/>.
        /// </summary>
        /// <remarks>
        /// Pen width is set to 1 device-independent unit.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Pen(Color color) => color.AsPen;

        /// <summary>
        /// Converts the specified <see cref='System.Drawing.Color'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(System.Drawing.Color color)
        {
            if (color.IsEmpty)
                return Color.Empty;

            var knownColor = color.ToKnownColor();

            if (knownColor == 0)
            {
                var argb = color.ToArgb();
                return FromArgb(argb);
            }
            else
            {
                return ColorCache.Get((KnownColor)knownColor);
            }
        }

        /// <summary>
        /// Converts the specified <see cref='Color'/> to a <see cref='System.Drawing.Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Drawing.Color(Color color)
        {
            if (color is null || color.IsEmpty)
                return System.Drawing.Color.Empty;

            var knownColor = color.ToKnownColor();

            if (knownColor == 0 || knownColor > KnownColor.MenuHighlight)
            {
                var argb = color.ToArgb();
                return System.Drawing.Color.FromArgb(argb);
            }
            else
            {
                return SystemDrawingColorCache.Get((System.Drawing.KnownColor)knownColor);
            }
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
        public static implicit operator RGBValue(Color color)
        {
            color.RequireArgb();
            return new(color.color.R, color.color.G, color.color.B);
        }

        /// <summary>
        /// Implicit operator conversion from tuple with three <see cref="byte"/> values
        /// to <see cref="Color"/>. Tuple values define RGB of the color.
        /// </summary>
        /// <param name="d">New color value specified as tuple with three
        /// <see cref="byte"/> values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color((byte Red, byte Green, byte Blue) d) =>
            new(d.Red, d.Green, d.Blue);

        /// <summary>
        /// Implicit operator conversion from tuple with four <see cref="byte"/> values
        /// to <see cref="Color"/>. Tuple values define ARGB of the color.
        /// </summary>
        /// <param name="d">New color value specified as tuple with
        /// four <see cref="byte"/> values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color((byte Alpha, byte Red, byte Green, byte Blue) d) =>
            new(d.Alpha, d.Red, d.Green, d.Blue);

        /// <summary>
        /// Converts the specified <see cref='string'/> to a <see cref='Color'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Color(string s)
        {
            return Color.Parse(s);
        }

        /// <summary>
        /// Tests whether two specified <see cref="Color"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="Color"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="Color"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="Color"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color? left, Color? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.AsStruct == right.AsStruct
                && left.state == right.state
                && left.knownColor == right.knownColor
                && left.name == right.name;
        }

        /// <summary>
        /// Tests whether <see cref="Color"/> and <see cref="System.Drawing.Color"/>
        /// structures are equivalent.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(System.Drawing.Color left, Color right)
        {
            return left.IsEmpty == right.IsEmpty && left.ToArgb() == right.ToArgb();
        }

        /// <summary>
        /// Tests whether <see cref="Color"/> and <see cref="System.Drawing.Color"/>
        /// structures are equivalent.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Color left, System.Drawing.Color right)
        {
            return left.IsEmpty == right.IsEmpty && left.ToArgb() == right.ToArgb();
        }

        /// <summary>
        /// Tests whether <see cref="Color"/> and <see cref="System.Drawing.Color"/>
        /// structures are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Color left, System.Drawing.Color right) => !(left == right);

        /// <summary>
        /// Tests whether <see cref="Color"/> and <see cref="System.Drawing.Color"/>
        /// structures are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(System.Drawing.Color left, Color right) => !(left == right);

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
        public static bool operator !=(Color? left, Color? right) => !(left == right);

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
        public static Color FromArgb(int argb) => new(argb);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return new(alpha, red, green, blue);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> structure from the three RGB
        /// component (red, green, and blue) values.
        /// Although this method allows a 32-bit value to be passed for each
        /// component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="red">The red component. Valid values are 0 through 255.
        /// </param>
        /// <param name="green">The green component. Valid values are 0 through 255.
        /// </param>
        /// <param name="blue">The blue component. Valid values are 0 through 255.
        /// </param>
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>This creates an opaque color (sets alpha to 255).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromArgb(int red, int green, int blue)
        {
            return FromArgb(255, red, green, blue);
        }

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
        /// <returns>The <see cref="Color"/> that this method creates.</returns>
        /// <remarks>To create an opaque color, set alpha to 255. To create
        /// a semitransparent color, set alpha to any value from 1 through 254.
        /// </remarks>
        /// <param name="rgb">RGB component</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromArgb(byte alpha, RGBValue rgb)
        {
            return new(alpha, rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Creates an opaque <see cref="Color"/> structure from the four ARGB
        /// components (255, <paramref name="red"/>, <paramref name="green"/>,
        /// and <paramref name="blue"/>) values.
        /// </summary>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromRgb(byte red, byte green, byte blue)
        {
            return new(red, green, blue);
        }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color FromArgb(byte alpha, Color baseColor)
        {
            return baseColor.WithAlpha(alpha);
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
        public static Color FromArgb(byte red, byte green, byte blue) =>
            new(255, red, green, blue);

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
        public static Color FromKnownColor(KnownColor color)
        {
            if (color <= 0 || color > KnownColor.RebeccaPurple)
            {
                return FromName(color.ToString());
            }
            else
            {
                return ColorCache.Get(color);
            }
        }

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
            var color = NamedColors.GetColorOrDefault(name, () =>
            {
                return new Color(0, StateFlags.NameValid, name, 0);
            });

            return color;
        }

        /// <summary>
        /// Returns a color from the specified string.
        /// </summary>
        public static Color Parse(string? s)
        {
            if(StringToColor is not null)
            {
                var e = new ValueConvertEventArgs<string?, Color?>(s);
                StringToColor(null, e);
                if (e.Handled)
                    return e.Result ?? Color.Empty;
            }

            return Parse(null, null, s) ?? Color.Empty;
        }

        /// <summary>
        /// Converts <see cref="string"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext" /> that provides a
        /// format context. You can use this object to get additional information about
        /// the environment from which this converter is being invoked. </param>
        /// <param name="culture">A <see cref="System.Globalization.CultureInfo" /> that
        /// specifies the culture to represent the color. </param>
        /// <param name="s">The string to convert.</param>
        /// <returns></returns>
        public static Color? Parse(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            var str = s!.Trim(' ', '(', ')');
            object? result = ColorConverter.ColorFromString(context, culture, str);
            if (result == null)
                return null;
            return (Color)result;
        }

        /// <summary>
        /// Enumerates colors defined in <see cref="KnownColor"/>.
        /// </summary>
        public static IReadOnlyList<Color> GetKnownColors()
        {
            List<Color> colors = new();

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
        /// Darkens or lightens a color, based on the specified percentage.
        /// <paramref name="alphaValue"/> of 0 would be completely black, 200 completely white
        /// an <paramref name="alphaValue"/> of 100 returns the same color.
        /// </summary>
        /// <param name="rgb">RGB Color.</param>
        /// <param name="alphaValue">Lightness value (0..200).</param>
        public static void ChangeLightness(ref RGBValue rgb, int alphaValue)
        {
            if (alphaValue == 100) return;

            // convert to normal alpha 0.0 - 1.0
            alphaValue = Math.Max(alphaValue, 0);
            alphaValue = Math.Min(alphaValue, 200);
            double alpha = ((double)(alphaValue - 100.0)) / 100.0;

            byte bg;
            if (alphaValue > 100)
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
        /// Assigns the same value to RGB of the color: 0 if <paramref name="on"/>
        /// is false, 255 otherwise.
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
        /// Creates a <see cref="LightDarkColor"/> instance with
        /// the specified color used for both light and dark themes.
        /// </summary>
        /// <param name="lightDark">The color to be used for both the light and dark themes.</param>
        /// <returns>A <see cref="LightDarkColor"/> instance where the same
        /// color is applied to both themes.</returns>
        public static LightDarkColor LightDark(Color lightDark)
        {
            return new LightDarkColor(lightDark, lightDark);
        }

        /// <summary>
        /// Creates <see cref="LightDarkColor"/> with the specified two colors used
        /// in light and dark themes.
        /// </summary>
        /// <param name="light">Color used when light theme is on.</param>
        /// <param name="dark">Color used when dark theme is on.</param>
        /// <returns></returns>
        public static LightDarkColor LightDark(Color light, Color dark)
        {
            return new LightDarkColor(light, dark);
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
        /// Calculates minimal and maximal values from <paramref name="r"/>, <paramref name="g"/>,
        /// <paramref name="b"/> parameters.
        /// </summary>
        /// <param name="min">Contains minimal value after method is called.</param>
        /// <param name="max">Contains maximal value after method is called.</param>
        /// <param name="r">Red component of the color.</param>
        /// <param name="g">Green component of the color.</param>
        /// <param name="b">Blue component of the color.</param>
        public static void MinMaxRgb(out byte min, out byte max, byte r, byte g, byte b)
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
            else
            if (b < min)
            {
                min = b;
            }
        }

        /// <summary>
        /// Enumerates colors defined in <see cref="KnownColor"/> for the specified
        /// color categories.
        /// </summary>
        public static IReadOnlyList<Color> GetKnownColors(
            KnownColorCategory[] cats,
            bool onlyVisible = true)
        {
            List<Color> colors = new();

            var items = ColorUtils.GetColorInfoItems();

            foreach (var item in items)
            {
                if (onlyVisible)
                {
                    if (!item.Visible)
                        continue;
                }

                if (!item.CategoryIs(cats))
                    continue;
                colors.Add(new Color(item.KnownColor));
            }

            colors.Sort(new ColorNameLocalizedComparer());
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
            MinMaxRgb(out var min, out var max);
            return (max + min) / (255 * 2f);
        }

        /// <summary>
        /// Returns the perceived brightness of the color, with 0 for black and 1
        /// for white.
        /// </summary>
        public double GetLuminance()
        {
            RequireArgb();
            return ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255.0;
        }

        /// <summary>
        /// Darkens or lightens a color, based on the specified percentage.
        /// <paramref name="alphaValue"/> of 0 would be completely black, 200 completely white
        /// an <paramref name="alphaValue"/> of 100 returns the same color.
        /// </summary>
        /// <param name="alphaValue">Lightness value (0..200).</param>
        public Color ChangeLightness(int alphaValue)
        {
            RGBValue rgb = this;
            ChangeLightness(ref rgb, alphaValue);
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
            GetRgbValues(out var r, out var g, out var b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out var min, out var max, r, g, b);

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
            GetRgbValues(out var r, out var g, out var b);

            if (r == g && g == b)
                return 0f;

            MinMaxRgb(out var min, out var max, r, g, b);

            int div = max + min;
            if (div > 255)
                div = (255 * 2) - max - min;

            return (max - min) / (double)div;
        }

        /// <summary>
        /// Creates <see cref="GenericImage"/> of the specified <paramref name="size"/>
        /// filled with this color.
        /// </summary>
        /// <param name="size">Size of the created image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage AsImage(SizeI size)
        {
            GenericImage image = new(size.Width, size.Height);
            image.SetRGBRect(this);
            return image;
        }

        /// <summary>
        /// Creates a new darker color from this color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Darker(float percOfDark)
        {
            return new(ControlPaint.Dark(AsStruct, percOfDark));
        }

        /// <summary>
        /// Adjusts the brightness of the color by a specified percentage.
        /// </summary>
        /// <param name="percOfLight">A percentage value indicating the amount
        /// of brightness adjustment. Positive values make the color lighter,
        /// while negative values make it darker.</param>
        /// <returns>A new <see cref="Color"/> instance that is lighter
        /// or darker based on the specified percentage.
        /// If <paramref name="percOfLight"/> is zero, the original color is returned.</returns>
        public Color LighterOrDarker(float percOfLight)
        {
            if (percOfLight < 0)
            {
                return Darker(-percOfLight);
            }
            else if (percOfLight > 0)
            {
                return Lighter(percOfLight);
            }

            return this;
        }

        /// <summary>
        /// Creates a new lighter color from this color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Lighter(float percOfLight)
        {
            return new(ControlPaint.Light(AsStruct, percOfLight));
        }

        /// <summary>
        /// Creates a new lighter color from this color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Lighter()
        {
            return new(ControlPaint.Light(AsStruct));
        }

        /// <summary>
        /// Creates a new much lighter color from this color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color LighterLighter()
        {
            return new(ControlPaint.LightLight(AsStruct));
        }

        /// <summary>
        /// Creates a new darker color from this color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color Darker()
        {
            return new(ControlPaint.Dark(AsStruct));
        }

        /// <summary>
        /// Creates a new much darker color from this color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color DarkerDarker()
        {
            return new(ControlPaint.DarkDark(AsStruct));
        }

        /// <summary>
        /// Creates <see cref="Image"/> of the specified <paramref name="size"/>
        /// filled with this color.
        /// </summary>
        /// <param name="size">Size of the created image in dips.</param>
        /// <param name="scaleFactor">Scaling factor used to convert dips to/from pixels.</param>
        /// <param name="borderColor">Border color. Optional. If not specified,
        /// default border color is used.</param>
        /// <returns></returns>
        public Image AsImageWithBorder(
            SizeD size,
            Coord scaleFactor,
            Color? borderColor = null)
        {
            borderColor ??= ComboBox.DefaultImageBorderColor;

            var graphics = SkiaUtils.CreateBitmapCanvas(size, scaleFactor, true);

            RectD rect = (PointD.Empty, size);

            RectD colorRect = DrawingUtils.DrawDoubleBorder(
                graphics,
                rect,
                Color.Empty,
                borderColor);

            graphics.FillRectangle(this.AsBrush, colorRect);

            return (Image)graphics.Bitmap!;
        }

        /// <summary>
        /// Creates <see cref="ImageSet"/> of the specified <paramref name="size"/>
        /// filled with this color.
        /// </summary>
        /// <param name="size">Size of the created image.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImageSet AsImageSet(SizeI size)
        {
            return (ImageSet)(Image)AsImage(size);
        }

        /// <summary>
        /// Retrieves a <see cref="Pen"/> instance with the specified width.
        /// </summary>
        /// <remarks>This method optimizes performance by caching <see cref="Pen"/>
        /// instances for widths
        /// up to a predefined limit. For widths exceeding the cache limit,
        /// a new <see cref="Pen"/> instance is created
        /// on each call.</remarks>
        /// <param name="width">The width of the pen to retrieve.
        /// Must be a positive integer.</param>
        /// <returns>A <see cref="Pen"/> instance with the specified width.
        /// If the width is less than or equal to a predefined
        /// maximum cached width, a cached instance may be returned;
        /// otherwise, a new instance is created.</returns>
        /// <see cref="AsPen"/>
        public virtual Pen GetPen(int width)
        {
            if (width <= 1)
                return AsPen;
            if (width <= MaxCachedPenWidth)
            {
                resources.PenCache ??= new Pen[MaxCachedPenWidth + 1];
                var pen = resources.PenCache[width];
                if (pen == null)
                {
                    pen = CreatePenInstance(width);
                    resources.PenCache[width] = pen;
                }

                return pen;
            }
            else
            {
                return CreatePenInstance(width);
            }
        }

        /// <summary>
        /// Gets this color as <see cref="Pen"/> with the specified width.
        /// This method caches pens with integer widths using <see cref="GetPen(int)"/>.
        /// Pens with non-integer widths are not cached.
        /// </summary>
        /// <param name="width">Width of the pen.</param>
        /// <remarks>
        /// The difference between this method and <see cref="GetPen(int)"/>
        /// is that this method accepts <see cref="Coord"/> values.
        /// </remarks>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Pen GetAsPen(Coord width = 1f)
        {
            if (MathUtils.IsInteger(width) && width > 0 && width <= MaxCachedPenWidth)
            {
                return GetPen((int)width);
            }

            return CreatePenInstance(width);
        }

        /// <summary>
        /// Gets this color as <see cref="Pen"/> with the specified width
        /// and <see cref="DashStyle"/>.
        /// </summary>
        /// <param name="width">Width of the pen. Optional. Default is 1.</param>
        /// <param name="dashStyle">Dash style of the pen. Optional.
        /// Default is <see cref="DashStyle.Solid"/>.</param>
        /// <returns></returns>
        public Pen GetAsPen(Coord width, DashStyle dashStyle)
        {
            if (dashStyle == DashStyle.Solid)
                return GetAsPen(width);
            return CreatePenInstance(width, dashStyle);
        }

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
            RequireArgb();
            r = color.R;
            g = color.G;
            b = color.B;
            a = color.A;
        }

        /// <summary>
        /// Returns RGB values of the <see cref="Color"/>
        /// </summary>
        /// <param name="r">Value of <see cref="R"/>.</param>
        /// <param name="g">Value of <see cref="G"/>.</param>
        /// <param name="b">Value of <see cref="B"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetRgbValues(out byte r, out byte g, out byte b)
        {
            RequireArgb();
            r = color.R;
            g = color.G;
            b = color.B;
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
        public uint AsUInt() => Value;

        /// <summary>
        /// Gets color properties for the debug purposes.
        /// </summary>
        public string ToDebugString()
        {
            RequireArgb();
            return $"{{Name={Name}, KnownColor={knownColor}, ARGB=({color.A}, {color.R}, {color.G}, {color.B}), State={state}}}";
        }

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
        public KnownColor ToKnownColor() => knownColor;

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
        /// <remarks>
        /// You can handle <see cref="ColorToString"/> in order to provide custom
        /// color to string conversion.
        /// </remarks>
        public override string? ToString()
        {
            RequireArgb();

            if (ColorToString is not null)
            {
                var e = new ValueConvertEventArgs<Color?, string?>(this);
                ColorToString(null, e);
                if (e.Handled)
                    return e.Result;
            }

            string result;

            if (IsNamedColor)
                result = $"{nameof(Color)} [{Name}]";
            else
            {
                if (state.HasFlag(StateFlags.ValueValid))
                {
                    RequireArgb();
                    result = $"{nameof(Color)} [A={color.A}, R={color.R}, G={color.G}, B={color.B}]";
                }
                else
                    result = $"{nameof(Color)} [Empty]";
            }

            return result;
        }

        /// <summary>
        /// Converts this color to display string.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// You can handle <see cref="ColorToDisplayString"/> in order to provide custom
        /// color to display string conversion.
        /// </remarks>
        public string? ToDisplayString()
        {
            if (ColorToDisplayString is not null)
            {
                var e = new ValueConvertEventArgs<Color?, string?>(this);
                ColorToDisplayString(null, e);
                if (e.Handled)
                    return e.Result;
            }

            if (IsNamedColor)
                return Name;
            if (IsOk)
                return ARGBWeb;
            else
                return string.Empty;
        }

        /// <summary>
        /// Returns a new color based on this current instance, but with the new red channel value.
        /// </summary>
        /// <param name="red">The new red component.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color WithRed(byte red)
        {
            var result = AsStruct;
            result.R = red;
            return new(result);
        }

        /// <summary>
        /// Returns a new color based on this current instance, but with the new green channel value.
        /// </summary>
        /// <param name="green">The new green component.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color WithGreen(byte green)
        {
            var result = AsStruct;
            result.G = green;
            return new(result);
        }

        /// <summary>
        /// Returns a new color based on this current instance, but with the new blue channel value.
        /// </summary>
        /// <param name="blue">The new blue component.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color WithBlue(byte blue)
        {
            var result = AsStruct;
            result.B = blue;
            return new(result);
        }

        /// <summary>
        /// Returns a new color based on this current instance, but with the new alpha channel value.
        /// </summary>
        /// <param name="alpha">The new alpha component.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Color WithAlpha(byte alpha)
        {
            var result = AsStruct;
            result.A = alpha;
            return new(result);
        }

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
        public bool Equals(Color? other)
        {
            if (other is null)
                return false;
            return this == other;
        }

        /// <summary>
        /// Gets whether color is dark.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Body of this function doesn't use logical not of <see cref="IsLight"/>,
        /// it implements completely different approach.
        /// </remarks>
        public bool IsDark()
        {
            if (IsBlack)
                return true;

            double r = color.R;
            double g = color.G;
            double b = color.B;

            // HSP equation from http://alienryderflex.com/hsp.html
            var hsp = Math.Sqrt(
              (0.299 * (r * r)) +
              (0.587 * (g * g)) +
              (0.114 * (b * b)));

            // Using the HSP value, determine whether the color is light or dark
            if (hsp > 127.5)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Gets whether color is light.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This function is suggested in
        /// <see href="https://learn.microsoft.com/en-us/uwp/api/windows.ui.color.b?view=winrt-26100"/>
        /// and it's implementation doesn't use logical not of <see cref="IsDark"/>.
        /// </remarks>
        public bool IsLight()
        {
            RequireArgb();
            var r = color.R;
            var g = color.G;
            var b = color.B;

            int v = (5 * g) + (2 * r) + b;

            return v > 8 * 128;
        }

        /// <summary>
        /// Calculates minimal and maximal values from all RGB color components.
        /// </summary>
        /// <param name="min">Contains minimal value after method is called.</param>
        /// <param name="max">Contains maximal value after method is called</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void MinMaxRgb(out byte min, out byte max)
        {
            RequireArgb();
            MinMaxRgb(out min, out max, color.R, color.G, color.B);
        }

        /// <summary>
        /// Indicates whether ARGB of this color is equal to ARGB of another color.
        /// </summary>
        /// <param name="other">A <see cref="Color"/> to compare with this color.</param>
        /// <returns><c>true</c> if ARGB of the colors are equal;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EqualARGB(Color other) => Value == other.Value;

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

            return (color, state, knownColor).GetHashCode();
        }

        /// <summary>
        /// Resets all cached resources used for rendering.
        /// </summary>
        /// <remarks>This method clears any cached brushes, pens, and paints,
        /// ensuring that subsequent
        /// rendering operations will recreate these resources as needed.
        /// Use this method to release memory or refresh
        /// resources when their state may have changed due
        /// to changes in system colors or user preferences.</remarks>
        public virtual void ResetCachedResources()
        {
            resources = new();
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Pen"/> with
        /// the specified width and default settings.
        /// </summary>
        /// <remarks>
        /// The pen is created with a solid dash style, flat line caps,
        /// mitered line joins, and is immutable.</remarks>
        /// <param name="width">The width of the pen, in device-independent units (DIPs).</param>
        /// <returns>A new <see cref="Pen"/> instance configured with the specified
        /// width and default settings.</returns>
        public virtual Pen CreatePenInstance(Coord width)
        {
            return new(
                this,
                width,
                DashStyle.Solid,
                LineCap.Flat,
                LineJoin.Miter,
                immutable: true);
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Pen"/> with the specified width, dash style, and default settings.
        /// </summary>
        /// <remarks>The created <see cref="Pen"/> is immutable and uses <see cref="LineCap.Flat"/> for
        /// line caps and <see cref="LineJoin.Miter"/> for line joins.</remarks>
        /// <param name="width">The width of the pen, represented as a <see cref="Coord"/>. Must be a positive value.</param>
        /// <param name="dash">The dash style of the pen, specified as a <see cref="DashStyle"/>.</param>
        /// <returns>A new <see cref="Pen"/> instance configured with the specified width,
        /// dash style, and default settings for
        /// line caps, joins, and immutability.</returns>
        public virtual Pen CreatePenInstance(Coord width, DashStyle dash)
        {
            return new(
                this,
                width,
                dash,
                LineCap.Flat,
                LineJoin.Miter,
                immutable: true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsKnownColorSystem(KnownColor knownColor)
             => KnownColorTable.ColorKindTable[(int)knownColor] ==
                 KnownColorTable.KnownColorKindSystem;

        /// <summary>
        /// Ensures that the color ARGB value is valid.
        /// </summary>
        /// <remarks>This method validates or enforces that the associated color value adheres
        /// to the ARGB format. It is intended to be used internally to maintain consistency
        /// in color representation. This method is called before any operations where ARGB
        /// values are accessed.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RequireArgb()
        {
            var oldColor = color;
            RequireArgb(ref color);
            if (oldColor != color)
                ResetCachedResources();
        }

        /// <summary>
        /// This method is called each time before argb value of the color
        /// is returned to the caller.
        /// </summary>
        /// <remarks>
        /// You can override this method in order to provide alternative mechanism
        /// for loading and preparing argb values of the color.
        /// </remarks>
        /// <param name="val">This output parameter must be set if argb of the color is changed.</param>
        protected virtual void RequireArgb(ref ColorStruct val)
        {
            if (state.HasFlag(StateFlags.KnownColorValid))
                val = KnownColorTable.KnownColorToArgb(knownColor);
        }

        private static void CheckByte(int value, string name)
        {
            if (unchecked((uint)value) > byte.MaxValue)
                ExceptionUtils.ThrowOutOfByteRange(value, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color FromArgb(uint argb)
        {
            return new(argb, StateFlags.ValueValid);
        }

        private struct CachedResources
        {
            public SolidBrush? AsBrush;

            public Pen? AsPen;

            public SKPaint? FillPaint;

            public SKPaint? StrokePaint;

            public SKPaint? StrokeAndFillPaint;

            public Pen?[]? PenCache;
        }

        /// <summary>
        /// Implements <see cref="IComparer{Color}"/> interface. Compares two colors
        /// using string compare of their names.
        /// </summary>
        public class ColorNameLocalizedComparer : IComparer<Color>
        {
            /// <inheritdoc/>
            public int Compare(Color? color1, Color? color2)
            {
                var name1 = color1?.NameLocalized;
                var name2 = color2?.NameLocalized;
                return string.Compare(name1, name2);
            }
        }

        /// <summary>
        /// Implements <see cref="IComparer{Color}"/> interface. Compares two colors
        /// using string compare of their names.
        /// </summary>
        public class ColorNameComparer : IComparer<Color>
        {
            /// <inheritdoc/>
            public int Compare(Color? color1, Color? color2)
            {
                var name1 = color1?.Name;
                var name2 = color2?.Name;
                return string.Compare(name1, name2);
            }
        }
    }
}