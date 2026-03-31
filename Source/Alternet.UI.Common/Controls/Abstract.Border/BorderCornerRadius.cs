using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the radius of a border corner, supporting values specified as either a number or a percentage.
    /// </summary>
    /// <remarks>Use this struct to define the curvature of a border's corner in user interface elements. The
    /// radius can be expressed in device-independent pixels (dips) or as a percentage, depending on the value of the
    /// Kind property. This type is typically used in scenarios where flexible border styling is required.</remarks>
    public struct BorderCornerRadius
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BorderCornerRadius"/> struct.
        /// </summary>
        public BorderCornerRadius()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BorderCornerRadius class with the specified value and value kind.
        /// </summary>
        /// <param name="value">The coordinate value representing the corner radius. Can be null to indicate an unspecified value.</param>
        /// <param name="kind">The kind of value to use for the corner radius. Defaults to ValueKind.Number if not specified.</param>
        public BorderCornerRadius(Coord? value, ValueKind? kind = ValueKind.Absolute)
        {
            Value = value;
            Kind = kind;
        }

        /// <summary>
        /// Initializes a new instance of the BorderCornerRadius class with the specified value and percent indicator.
        /// </summary>
        /// <param name="value">The corner radius value to assign. If null, the radius is unspecified.</param>
        /// <param name="isPercent">A value indicating whether the corner radius is specified as a percentage. If null, the unit is unspecified.</param>
        public BorderCornerRadius(Coord? value, bool? isPercent)
        {
            Value = value;
            IsPercent = isPercent;
        }

        /// <summary>
        /// Gets or sets the radius of the border corner. The value can be specified in dips or as a percentage,
        /// depending on the value of <see cref="Kind"/> property.
        /// </summary>
        public Coord? Value { get; set; }

        /// <summary>
        /// Gets or sets the kind of value represented by this instance.
        /// </summary>
        public ValueKind? Kind { get; set; } = ValueKind.Absolute;

        /// <summary>
        /// Gets or sets a value indicating whether the current value is represented as a percentage.
        /// </summary>
        public bool? IsPercent
        {
            readonly get
            {
                if (Value == null)
                    return null;

                return Kind == ValueKind.Percent;
            }

            set
            {
                if (value == null)
                {
                    SetKind(null);
                }
                else
                {
                    SetKind(value.Value ? ValueKind.Percent : ValueKind.Absolute);
                }
            }
        }

        /// <summary>
        /// Specifies the type of value represented, such as a numeric value or a percentage.
        /// </summary>
        /// <remarks>Use this enumeration to indicate whether a value should be interpreted as a raw
        /// number or as a percent. This can help ensure correct formatting, validation, or calculation logic when
        /// handling values that may have different semantic meanings.</remarks>
        public enum ValueKind
        {
            /// <summary>
            /// Represents an absolute value.
            /// </summary>
            Absolute,

            /// <summary>
            /// Represents a percentage value.
            /// </summary>
            Percent,
        }

        /// <summary>
        /// Sets the kind of value represented by this instance.
        /// </summary>
        /// <param name="kind">The value kind to assign. Specify <see langword="null"/> to clear the current kind.</param>
        public void SetKind(ValueKind? kind)
        {
            this.Kind = kind;
        }

        /// <summary>
        /// Sets the radius of the border corner.
        /// </summary>
        /// <param name="value">The coordinate value to assign. Can be null to clear the current value.</param>
        public void SetValue(Coord? value)
        {
            this.Value = value;
        }
    }
}
