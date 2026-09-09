using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a scaled font size, allowing for dynamic adjustment of font sizes based on scaling factors, delta values,
    /// or other relative adjustments. This struct provides predefined font sizes and methods to create custom relative font sizes.
    /// </summary>
    public readonly struct RelativeFontSize : IEquatable<RelativeFontSize>
    {
        /// <summary>
        /// Defines the kind of relative font size, indicating whether the value represents a scale factor,
        /// a delta value, or other adjustments to the font size.
        /// </summary>
        public enum RuleKind
        {
            /// <summary>
            /// The font size is specified as an absolute value, without any scaling or delta adjustments.
            /// </summary>
            Absolute,

            /// <summary>
            /// The font size is scaled based on a scale factor.
            /// </summary>
            Scale,
            
            /// <summary>
            /// The font size is adjusted by a delta value.
            /// </summary>
            Delta,

            /// <summary>
            /// The font size is specified as a percentage of the base font size.
            /// </summary>
            Percent,
        }

        /// <summary>
        /// Gets a predefined scaled font size that is smaller than the default size.
        /// Scale factor is 0.60f, similar to xx-small size, typically used for less prominent text.
        /// </summary>
        public static readonly RelativeFontSize XXSmall = new(RuleKind.Scale, 0.60f);

        /// <summary>
        /// Gets a predefined scaled font size that is smaller than the default size.
        /// Scale factor is 0.75f, similar to x-small size, typically used for less prominent text.
        /// </summary>
        public static readonly RelativeFontSize XSmall = new(RuleKind.Scale, 0.75f);

        /// <summary>
        /// Gets a predefined scaled font size that is smaller than the default size.
        /// Scale factor is 0.89f, similar to small size, typically used for less prominent text.
        /// </summary>
        public static readonly RelativeFontSize Small = new(RuleKind.Scale, 0.89f);

        /// <summary>
        /// Gets a predefined scaled font size that is the default size.
        /// Scale factor is 1.00f, similar to medium size, typically used for standard text.
        /// </summary>
        public static readonly RelativeFontSize Medium = new(RuleKind.Scale, 1.00f);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// Scale factor is 1.20f, similar to large size, typically used for more prominent text.
        /// </summary>
        public static readonly RelativeFontSize Large = new(RuleKind.Scale, 1.20f);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// Scale factor is 1.50f, similar to x-large size, typically used for more prominent text.
        /// </summary>
        public static readonly RelativeFontSize XLarge = new(RuleKind.Scale, 1.50f);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// Scale factor is 2.00f, similar to xx-large size, typically used for more prominent text.
        /// </summary>
        public static readonly RelativeFontSize XXLarge = new(RuleKind.Scale, 2.00f);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// Scale factor is 3.00f, similar to xxx-large size, typically used for more prominent text.
        /// </summary>
        public static readonly RelativeFontSize XXXLarge = new(RuleKind.Scale, 3.00f);

        /// <summary>
        /// Gets the value of the relative font size. This value can represent a scale factor, a delta value,
        /// or other adjustments to the font size, depending on the <see cref="Kind"/> property.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Gets the kind of the relative font size. This property indicates whether the <see cref="Value"/>
        /// represents a scale factor, a delta value, or other adjustments to the font size.
        /// </summary>
        public RuleKind Kind { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeFontSize"/> struct with the specified kind and value.
        /// </summary>
        /// <param name="kind">The kind of relative font size.</param>
        /// <param name="value">The value of the relative font size.</param>
        public RelativeFontSize(RuleKind kind, float value = 1.0f)
        {
            Kind = kind;
            Value = value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="RelativeFontSize"/> is equal to the current instance.
        /// </summary>
        /// <param name="left">The first <see cref="RelativeFontSize"/> to compare.</param>
        /// <param name="right">The second <see cref="RelativeFontSize"/> to compare.</param>
        /// <returns><c>true</c> if the specified <see cref="RelativeFontSize"/> instances are equal;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator ==(RelativeFontSize left, RelativeFontSize right)
            => left.Kind == right.Kind && left.Value == right.Value;

        /// <summary>
        /// Determines whether the specified <see cref="RelativeFontSize"/> is not equal to the current instance.
        /// </summary>
        /// <param name="left">The first <see cref="RelativeFontSize"/> to compare.</param>
        /// <param name="right">The second <see cref="RelativeFontSize"/> to compare.</param>
        /// <returns><c>true</c> if the specified <see cref="RelativeFontSize"/> instances are not equal;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator !=(RelativeFontSize left, RelativeFontSize right) => !left.Equals(right);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// By default scale factor is 1.20f, similar to large size, typically used for more prominent text.
        /// </summary>
        /// <param name="step">The scale factor to increase the font size by.</param>
        /// <returns>A new <see cref="RelativeFontSize"/> instance with the specified scale factor.</returns>
        public static RelativeFontSize Larger(float step = 1.20f) => new(RuleKind.Scale, step);

        /// <summary>
        /// Gets a predefined scaled font size that is smaller than the default size.
        /// By default scale factor is 0.83f, similar to small size, typically used for less prominent text.
        /// </summary>
        /// <param name="step">The scale factor to decrease the font size by.</param>
        /// <returns>A new <see cref="RelativeFontSize"/> instance with the specified scale factor.</returns>
        public static RelativeFontSize Smaller(float step = 0.83f) => new(RuleKind.Scale, step);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// Font size is increased by the specified number of points, allowing for precise control over the font size.
        /// </summary>
        /// <param name="deltaPoints">The number of points to increase the font size by.</param>
        /// <returns>A new <see cref="RelativeFontSize"/> instance with the specified delta points.</returns>
        public static RelativeFontSize FromDelta(int deltaPoints) => new(RuleKind.Delta, deltaPoints);

        /// <summary>
        /// Gets a predefined scaled font size that is larger than the default size.
        /// Font size is scaled by the specified scale factor, allowing for precise control over the font size.
        /// </summary>
        /// <param name="scale">The scale factor to apply to the font size.</param>
        /// <returns>A new <see cref="RelativeFontSize"/> instance with the specified scale factor.</returns>
        public static RelativeFontSize FromScale(float scale) => new(RuleKind.Scale, scale);

        /// <inheritdoc/>
        override public bool Equals(object? obj) => obj is RelativeFontSize other && this == other;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Kind, Value).GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Kind} {Value}";

        /// <summary>
        /// Calculates the actual font size based on the specified base size and the relative font size settings.
        /// </summary>
        /// <param name="baseSize">The base font size to apply the relative adjustments to.</param>
        /// <returns>The calculated font size after applying the relative adjustments.</returns>
        public float GetSize(float baseSize)
        {
            return Kind switch
            {
                RuleKind.Absolute => Value,
                RuleKind.Scale => baseSize * Value,
                RuleKind.Delta => baseSize + Value,
                RuleKind.Percent => baseSize * (Value / 100f),
                _ => baseSize,
            };
        }

        /// <summary>
        /// Determines whether the specified <see cref="RelativeFontSize"/> is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="RelativeFontSize"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="RelativeFontSize"/> is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(RelativeFontSize other)
        {
            return this == other;
        }
    }

}
