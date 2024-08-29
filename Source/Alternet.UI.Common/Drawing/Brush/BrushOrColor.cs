using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains brush or color.
    /// </summary>
    public class BrushOrColor
    {
        private object? val;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushOrColor"/> class.
        /// </summary>
        public BrushOrColor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushOrColor"/> class.
        /// </summary>
        /// <param name="alpha">Alpha component of the color.</param>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        public BrushOrColor(byte alpha, byte red, byte green, byte blue)
        {
            val = Color.FromArgb(alpha, red, green, blue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushOrColor"/> class.
        /// </summary>
        /// <param name="red">Red component of the color.</param>
        /// <param name="green">Green component of the color.</param>
        /// <param name="blue">Blue component of the color.</param>
        public BrushOrColor(byte red, byte green, byte blue)
        {
            val = Color.FromRgb(red, green, blue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushOrColor"/> class.
        /// </summary>
        /// <param name="brush">Brush value.</param>
        public BrushOrColor(Brush brush)
        {
            val = brush;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushOrColor"/> class.
        /// </summary>
        /// <param name="color">Color value.</param>
        public BrushOrColor(Color color)
        {
            val = color;
        }

        /// <summary>
        /// Gets or sets brush.
        /// </summary>
        public Brush? Brush
        {
            get
            {
                return val as Brush;
            }

            set
            {
                val = value;
            }
        }

        /// <summary>
        /// Gets or sets color.
        /// </summary>
        public Color? Color
        {
            get
            {
                return val as Color;
            }

            set
            {
                val = value;
            }
        }

        /// <summary>
        /// Gets or sets whether this object is empty (contains no brush or color).
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return val is null;
            }

            set
            {
                val = null;
            }
        }

        /// <summary>
        /// Gets whether this object contains brush.
        /// </summary>
        public bool IsBrush
        {
            get
            {
                return val is Brush;
            }
        }

        /// <summary>
        /// Gets whether this object contains color.
        /// </summary>
        public bool IsColor
        {
            get
            {
                return val is Color;
            }
        }

        /// <summary>
        /// Gets brush or color as brush.
        /// </summary>
        public Brush? AsBrush
        {
            get
            {
                var asColor = val as Color;
                var asBrush = val as Brush;

                if (asBrush is not null)
                    return asBrush;

                if (asColor is not null)
                    return asColor.AsBrush;

                return null;
            }
        }

        /// <summary>
        /// Gets brush or color as color.
        /// </summary>
        public Color? AsColor
        {
            get
            {
                var asColor = val as Color;
                var asBrush = val as Brush;

                if(asColor is not null)
                    return asColor;

                if (asBrush is not null)
                    return asBrush.AsColor;

                return null;
            }

            set
            {
                val = value;
            }
        }

        /// <summary>
        /// Implicit operator convertion from tuple with three <see cref="byte"/> values
        /// to <see cref="BrushOrColor"/>. Tuple values define RGB of the color.
        /// </summary>
        /// <param name="d">New color value specified as tuple with three <see cref="byte"/> values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BrushOrColor((byte, byte, byte) d) =>
            new(d.Item1, d.Item2, d.Item3);

        /// <summary>
        /// Implicit operator convertion from tuple with four <see cref="byte"/> values
        /// to <see cref="BrushOrColor"/>. Tuple values define ARGB of the color.
        /// </summary>
        /// <param name="d">New color value specified as tuple with four <see cref="byte"/> values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BrushOrColor((byte, byte, byte, byte) d) =>
            new(d.Item1, d.Item2, d.Item3, d.Item4);
    }
}
