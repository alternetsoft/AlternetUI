#nullable disable
using System;
using System.ComponentModel;
using System.Globalization;

namespace Alternet.UI
{
    /// <summary>
    /// GridLength is the type used for various length-like properties in the system,
    /// that explicitely support Star unit type. For example, "Width", "Height"
    /// properties of ColumnDefinition and RowDefinition used by Grid.
    /// </summary>
    [TypeConverter(typeof(GridLengthConverter))]
    public struct GridLength : IEquatable<GridLength>
    {
        private readonly double _unitValue;
        private readonly GridUnitType _unitType;

        /// <summary>
        /// Constructor, initializes the GridLength as absolute value in pixels.
        /// </summary>
        /// <param name="pixels">Specifies the number of 'device-independent pixels'
        /// (96 pixels-per-inch).</param>
        /// <exception cref="ArgumentException">
        /// If <c>pixels</c> parameter is <c>double.NaN</c>
        /// or <c>pixels</c> parameter is <c>double.NegativeInfinity</c>
        /// or <c>pixels</c> parameter is <c>double.PositiveInfinity</c>.
        /// </exception>
        public GridLength(double pixels)
            : this(pixels, GridUnitType.Pixel)
        {
        }

        /// <summary>
        /// Constructor, initializes the GridLength and specifies what kind of value
        /// it will hold.
        /// </summary>
        /// <param name="value">Value to be stored by this GridLength
        /// instance.</param>
        /// <param name="type">Type of the value to be stored by this GridLength
        /// instance.</param>
        /// <remarks>
        /// If the <c>type</c> parameter is <c>GridUnitType.Auto</c>,
        /// then passed in value is ignored and replaced with <c>0</c>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// If <c>value</c> parameter is <c>double.NaN</c>
        /// or <c>value</c> parameter is <c>double.NegativeInfinity</c>
        /// or <c>value</c> parameter is <c>double.PositiveInfinity</c>.
        /// </exception>
        public GridLength(double value = 0, GridUnitType type = GridUnitType.Auto)
        {
            if (DoubleUtil.IsNaN(value))
                throw new ArgumentException();

            if (double.IsInfinity(value))
                throw new ArgumentException();

            if (type != GridUnitType.Auto
                && type != GridUnitType.Pixel
                && type != GridUnitType.Star)
                throw new ArgumentException();

            _unitValue = (type == GridUnitType.Auto) ? 0 : value;
            _unitType = type;
        }

        /// <summary>
        /// Overloaded operator, compares 2 GridLength's.
        /// </summary>
        /// <param name="gl1">first GridLength to compare.</param>
        /// <param name="gl2">second GridLength to compare.</param>
        /// <returns>true if specified GridLengths have same value 
        /// and unit type.</returns>
        public static bool operator ==(GridLength gl1, GridLength gl2)
        {
            return gl1.GridUnitType == gl2.GridUnitType
                    && gl1.Value == gl2.Value;
        }

        /// <summary>
        /// Overloaded operator, compares 2 GridLength's.
        /// </summary>
        /// <param name="gl1">first GridLength to compare.</param>
        /// <param name="gl2">second GridLength to compare.</param>
        /// <returns>true if specified GridLengths have either different value or
        /// unit type.</returns>
        public static bool operator !=(GridLength gl1, GridLength gl2)
        {
            return gl1.GridUnitType != gl2.GridUnitType
                    || gl1.Value != gl2.Value;
        }

        /// <summary>
        /// Compares this instance of GridLength with another object.
        /// </summary>
        /// <param name="oCompare">Reference to an object for comparison.</param>
        /// <returns><c>true</c>if this GridLength instance has the same value
        /// and unit type as oCompare.</returns>
        public override bool Equals(object oCompare)
        {
            if (oCompare is GridLength l)
                return this == l;
            else
                return false;
        }

        /// <summary>
        /// Compares this instance of GridLength with another instance.
        /// </summary>
        /// <param name="gridLength">Grid length instance to compare.</param>
        /// <returns><c>true</c>if this GridLength instance has the same value
        /// and unit type as gridLength.</returns>
        public bool Equals(GridLength gridLength)
        {
            return this == gridLength;
        }

        /// <summary>
        /// <see cref="object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            return (int)_unitValue + (int)_unitType;
        }

        /// <summary>
        /// Returns <c>true</c> if this GridLength instance holds 
        /// an absolute (pixel) value.
        /// </summary>
        public bool IsAbsolute { get { return (_unitType == GridUnitType.Pixel); } }

        /// <summary>
        /// Returns <c>true</c> if this GridLength instance is
        /// automatic (not specified).
        /// </summary>
        public bool IsAuto { get { return (_unitType == GridUnitType.Auto); } }

        /// <summary>
        /// Returns <c>true</c> if this GridLength instance holds weighted propertion
        /// of available space.
        /// </summary>
        public bool IsStar { get { return (_unitType == GridUnitType.Star); } }

        /// <summary>
        /// Returns value part of this GridLength instance.
        /// </summary>
        public double Value { get { return ((_unitType == GridUnitType.Auto) ? 1 : _unitValue); } }

        /// <summary>
        /// Returns unit type of this GridLength instance.
        /// </summary>
        public GridUnitType GridUnitType { get { return (_unitType); } }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        public override string ToString()
        {
            return GridLengthConverter.ToString(this, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns initialized <see cref="GridLength"/> value with <see cref="GridUnitType"/>
        /// equal to <see cref="GridUnitType.Auto"/>.
        /// </summary>
        public static GridLength Auto { get; } = new(1, GridUnitType.Auto);

        /// <summary>
        /// Returns initialized <see cref="GridLength"/> value with <see cref="GridUnitType"/>
        /// equal to <see cref="GridUnitType.Star"/>.
        /// </summary>
        public static GridLength Star { get; } = new(1, GridUnitType.Star);
    }

}