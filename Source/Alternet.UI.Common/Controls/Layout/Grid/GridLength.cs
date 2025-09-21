using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// GridLength is the type used for various length-like properties in the system,
    /// that explicitly support Star unit type. For example, "Width", "Height"
    /// properties of ColumnDefinition and RowDefinition used by Grid.
    /// </summary>
    [TypeConverter(typeof(GridLengthConverter))]
    public readonly struct GridLength : IEquatable<GridLength>
    {
        /// <summary>
        /// Represents a <see cref="GridLength"/> whose value is determined by the size properties
        /// of the content object.
        /// </summary>
        public static readonly GridLength Auto = new(1, GridUnitType.Auto);

        /// <summary>
        /// Represents a <see cref="GridLength"/> whose value is expressed as
        /// a weighted proportion of available space.
        /// </summary>
        public static readonly GridLength Star = new(1, GridUnitType.Star);

        private readonly Coord unitValue;
        private readonly GridUnitType unitType;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLength"/> struct
        /// as an absolute value in pixels.
        /// </summary>
        /// <param name="pixels">Specifies the number of
        /// 'device-independent pixels' (96 pixels-per-inch).</param>
        /// <exception cref="ArgumentException">
        /// If <c>pixels</c> parameter is <c>Coord.NaN</c>
        /// or <c>pixels</c> parameter is <c>Coord.NegativeInfinity</c>
        /// or <c>pixels</c> parameter is <c>Coord.PositiveInfinity</c>.
        /// </exception>
        public GridLength(Coord pixels)
            : this(pixels, GridUnitType.Pixel)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridLength"/> struct
        /// and specifies what kind of value it will hold.
        /// </summary>
        /// <param name="value">Value to be stored by this <see cref="GridLength"/> instance.</param>
        /// <param name="type">Type of the value to be stored by this
        /// <see cref="GridLength"/> instance.</param>
        /// <remarks>
        /// If the <c>type</c> parameter is <c>GridUnitType.Auto</c>,
        /// then passed in value is ignored and replaced with <c>1</c>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// If <c>value</c> parameter is infinity or is not a number.
        /// </exception>
        public GridLength(Coord value = 0, GridUnitType type = GridUnitType.Auto)
        {
            if (MathUtils.IsNaN(value) || Coord.IsInfinity(value))
                throw new ArgumentException();

            unitValue = (type == GridUnitType.Auto) ? 1 : value;
            unitType = type;
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="GridLength"/>
        /// holds an absolute value in pixels.
        /// </summary>
        public readonly bool IsAbsolute
        {
            get
            {
                return unitType == GridUnitType.Pixel;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="GridLength"/>
        /// holds an automatic value.
        /// </summary>
        public readonly bool IsAuto
        {
            get
            {
                return unitType == GridUnitType.Auto;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="GridLength"/> holds a weighted
        /// proportion of available space.
        /// </summary>
        public readonly bool IsStar
        {
            get
            {
                return unitType == GridUnitType.Star;
            }
        }

        /// <summary>
        /// Gets the value held by the <see cref="GridLength"/>.
        /// </summary>
        public readonly Coord Value
        {
            get
            {
                return unitValue;
            }
        }

        /// <summary>
        /// Gets the type of value held by the <see cref="GridLength"/>.
        /// </summary>
        public readonly GridUnitType GridUnitType
        {
            get
            {
                return unitType;
            }
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="GridLength"/> are equal.
        /// </summary>
        /// <param name="gl1">The first instance of <see cref="GridLength"/> to compare.</param>
        /// <param name="gl2">The second instance of <see cref="GridLength"/> to compare.</param>
        /// <returns>true if the two instances of <see cref="GridLength"/> are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(GridLength gl1, GridLength gl2)
        {
            return gl1.GridUnitType == gl2.GridUnitType
                    && gl1.Value == gl2.Value;
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="GridLength"/> are not equal.
        /// </summary>
        /// <param name="gl1">The first value to compare.</param>
        /// <param name="gl2">The second value to compare.</param>
        /// <returns>true if the two instances of <see cref="GridLength"/> are not equal;
        /// otherwise, false.</returns>
        public static bool operator !=(GridLength gl1, GridLength gl2)
        {
            return !(gl1 == gl2);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="GridLength"/>.
        /// </summary>
        /// <param name="oCompare">The object to compare with the current
        /// <see cref="GridLength"/>.</param>
        /// <returns>true if the specified object is equal to the current <see cref="GridLength"/>;
        /// otherwise, false.</returns>
        public readonly override bool Equals(object oCompare)
        {
            if (oCompare is GridLength l)
                return this == l;
            else
                return false;
        }

        /// <summary>
        /// Determines whether the specified value is equal to the current <see cref="GridLength"/>.
        /// </summary>
        /// <param name="gridLength">The value to compare with the current
        /// <see cref="GridLength"/>.</param>
        /// <returns>true if the specified value is equal to the current <see cref="GridLength"/>;
        /// otherwise, false.</returns>
        public readonly bool Equals(GridLength gridLength)
        {
            return this == gridLength;
        }

        /// <summary>
        /// Returns the hash code for the current <see cref="GridLength"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="GridLength"/>.</returns>
        public readonly override int GetHashCode()
        {
            return (int)unitValue + (int)unitType;
        }

        /// <summary>
        /// Returns a string representation of the current <see cref="GridLength"/>.
        /// </summary>
        /// <returns>A string representation of the current <see cref="GridLength"/>.</returns>
        public readonly override string ToString()
        {
            return GridLengthConverter.ToString(this, CultureInfo.InvariantCulture);
        }
    }
}
