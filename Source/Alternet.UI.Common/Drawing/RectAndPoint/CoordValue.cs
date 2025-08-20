using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a coordinate value with an associated unit.
    /// </summary>
    public readonly struct CoordValue
    {
        /// <summary>
        /// Gets the coordinate value.
        /// </summary>
        public readonly int Value;

        /// <summary>
        /// Gets the unit of the coordinate value.
        /// </summary>
        public readonly CoordUnit Unit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordValue"/>
        /// struct with the specified value and unit.
        /// </summary>
        /// <param name="value">The coordinate value.</param>
        /// <param name="unit">The unit of the coordinate value.</param>
        public CoordValue(int value, CoordUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Converts the current coordinate value to pixels based on the specified
        /// scale factor and base value.
        /// </summary>
        /// <param name="scaleFactor">The scale factor used for converting
        /// device-independent pixels (DIPs) to pixels. This parameter is only
        /// used when the unit is <see cref="CoordUnit.Dip"/>.</param>
        /// <param name="baseValue">The base value, in pixels, used
        /// for percentage-based calculations. This parameter is only used when the
        /// unit is <see cref="CoordUnit.Percent"/>.</param>
        /// <returns>The coordinate value converted to pixels.
        /// The result depends on the current unit:
        /// <see cref="CoordUnit.Pixel"/> returns the value as-is,
        /// <see cref="CoordUnit.Dip"/> applies the scale factor,
        /// and <see cref="CoordUnit.Percent"/> calculates the percentage
        /// of the base value.</returns>
        /// <exception cref="NotSupportedException">Thrown if the current
        /// unit is not supported.</exception>
        public int ToPixels(double scaleFactor, int baseValue)
        {
            return Unit switch
            {
                CoordUnit.Pixel => Value,
                CoordUnit.Dip => GraphicsFactory.PixelFromDip(Value, scaleFactor),
                CoordUnit.Percent => (int)(((double)baseValue / 100d) * Value),
                _ => throw new NotSupportedException($"Unsupported unit: {Unit}"),
            };
        }
    }
}
