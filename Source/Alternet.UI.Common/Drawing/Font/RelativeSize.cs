using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a scaled size, allowing for dynamic adjustment of
    /// sizes based on scaling factors, delta values,
    /// or other relative adjustments. This struct provides predefined
    /// sizes and methods to create custom relative sizes.
    /// </summary>
    public readonly struct RelativeSize : IEquatable<RelativeSize>
    {
        /// <summary>
        /// Defines the kind of relative size, indicating whether the value represents a scale factor,
        /// a delta value, or other adjustments to the size.
        /// </summary>
        public enum RuleKind
        {
            /// <summary>
            /// The size is specified as an absolute value, without any scaling or delta adjustments.
            /// </summary>
            Absolute,

            /// <summary>
            /// The size is scaled based on a scale factor.
            /// </summary>
            Scale,
            
            /// <summary>
            /// The size is adjusted by a delta value.
            /// </summary>
            Delta,

            /// <summary>
            /// The size is specified as a percentage of the base size.
            /// </summary>
            Percent,
        }

        /// <summary>
        /// Gets a predefined scaled size that is smaller than the default size.
        /// Scale factor is 0.60f, similar to xx-small size, typically used for less prominent elements.
        /// </summary>
        public static readonly RelativeSize XXSmall = new(RuleKind.Scale, 0.60f);

        /// <summary>
        /// Gets a predefined scaled size that is smaller than the default size.
        /// Scale factor is 0.75f, similar to x-small size, typically used for less prominent elements.
        /// </summary>
        public static readonly RelativeSize XSmall = new(RuleKind.Scale, 0.75f);

        /// <summary>
        /// Gets a predefined scaled size that is smaller than the default size.
        /// Scale factor is 0.89f, similar to small size, typically used for less prominent elements.
        /// </summary>
        public static readonly RelativeSize Small = new(RuleKind.Scale, 0.89f);

        /// <summary>
        /// Gets a predefined scaled size that is the default size.
        /// Scale factor is 1.00f, similar to medium size, typically used for standard elements.
        /// </summary>
        public static readonly RelativeSize Medium = new(RuleKind.Scale, 1.00f);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// Scale factor is 1.20f, similar to large size, typically used for more prominent elements.
        /// </summary>
        public static readonly RelativeSize Large = new(RuleKind.Scale, 1.20f);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// Scale factor is 1.50f, similar to x-large size, typically used for more prominent elements.
        /// </summary>
        public static readonly RelativeSize XLarge = new(RuleKind.Scale, 1.50f);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// Scale factor is 2.00f, similar to xx-large size, typically used for more prominent elements.
        /// </summary>
        public static readonly RelativeSize XXLarge = new(RuleKind.Scale, 2.00f);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// Scale factor is 3.00f, similar to xxx-large size, typically used for more prominent elements.
        /// </summary>
        public static readonly RelativeSize XXXLarge = new(RuleKind.Scale, 3.00f);

        /// <summary>
        /// Gets the value of the relative size. This value can represent a scale factor, a delta value,
        /// or other adjustments to the size, depending on the <see cref="Kind"/> property.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Gets the kind of the relative size. This property indicates whether the <see cref="Value"/>
        /// represents a scale factor, a delta value, or other adjustments to the size.
        /// </summary>
        public RuleKind Kind { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeSize"/> struct with the specified kind and value.
        /// </summary>
        /// <param name="kind">The kind of relative size.</param>
        /// <param name="value">The value of the relative size.</param>
        public RelativeSize(RuleKind kind, float value = 1.0f)
        {
            Kind = kind;
            Value = value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="RelativeSize"/> is equal to the current instance.
        /// </summary>
        /// <param name="left">The first <see cref="RelativeSize"/> to compare.</param>
        /// <param name="right">The second <see cref="RelativeSize"/> to compare.</param>
        /// <returns><c>true</c> if the specified <see cref="RelativeSize"/> instances are equal;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator ==(RelativeSize left, RelativeSize right)
            => left.Kind == right.Kind && left.Value == right.Value;

        /// <summary>
        /// Determines whether the specified <see cref="RelativeSize"/> is not equal to the current instance.
        /// </summary>
        /// <param name="left">The first <see cref="RelativeSize"/> to compare.</param>
        /// <param name="right">The second <see cref="RelativeSize"/> to compare.</param>
        /// <returns><c>true</c> if the specified <see cref="RelativeSize"/> instances are not equal;
        /// otherwise, <c>false</c>.</returns>
        public static bool operator !=(RelativeSize left, RelativeSize right) => !left.Equals(right);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// By default scale factor is 1.20f, similar to large size, typically used for more prominent elements.
        /// </summary>
        /// <param name="step">The scale factor to increase the size by.</param>
        /// <returns>A new <see cref="RelativeSize"/> instance with the specified scale factor.</returns>
        public static RelativeSize Larger(float step = 1.20f) => new(RuleKind.Scale, step);

        /// <summary>
        /// Gets a predefined scaled size that is smaller than the default size.
        /// By default scale factor is 0.83f, similar to small size, typically used for less prominent elements.
        /// </summary>
        /// <param name="step">The scale factor to decrease the size by.</param>
        /// <returns>A new <see cref="RelativeSize"/> instance with the specified scale factor.</returns>
        public static RelativeSize Smaller(float step = 0.83f) => new(RuleKind.Scale, step);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// Size is increased by the specified number of points, allowing for precise control over the size.
        /// </summary>
        /// <param name="deltaPoints">The number of points to increase the size by.</param>
        /// <returns>A new <see cref="RelativeSize"/> instance with the specified delta points.</returns>
        public static RelativeSize FromDelta(int deltaPoints) => new(RuleKind.Delta, deltaPoints);

        /// <summary>
        /// Gets a predefined scaled size that is larger than the default size.
        /// Size is scaled by the specified scale factor, allowing for precise control over the size.
        /// </summary>
        /// <param name="scale">The scale factor to apply to the size.</param>
        /// <returns>A new <see cref="RelativeSize"/> instance with the specified scale factor.</returns>
        public static RelativeSize FromScale(float scale) => new(RuleKind.Scale, scale);

        /// <summary>
        /// Calculates the actual size based on the specified base size and the relative size settings.
        /// </summary>
        /// <param name="kind">The kind of relative size, indicating whether the value represents a scale factor,
        /// a delta value, or other adjustments to the size.</param>
        /// <param name="value">The value representing the relative adjustment to the size.</param>
        /// <param name="baseSize">The base size to apply the relative adjustments to.</param>
        /// <returns>The calculated size after applying the relative adjustments.</returns>
        public static float GetSize(float baseSize, RuleKind kind, float value)
        {
            return kind switch
            {
                RuleKind.Absolute => value,
                RuleKind.Scale => baseSize * value,
                RuleKind.Delta => baseSize + value,
                RuleKind.Percent => baseSize * (value / 100f),
                _ => baseSize,
            };
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is RelativeSize other && this == other;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Kind, Value).GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Kind} {Value}";

        /// <summary>
        /// Calculates the actual size based on the specified base size and the relative size settings.
        /// </summary>
        /// <param name="baseSize">The base size to apply the relative adjustments to.</param>
        /// <returns>The calculated size after applying the relative adjustments.</returns>
        public float GetSize(float baseSize)
        {
            return GetSize(baseSize, Kind, Value);
        }

        /// <summary>
        /// Calculates the actual size based on the specified base size and the relative size settings.
        /// </summary>
        /// <param name="size">The base size to apply the relative adjustments to.</param>
        /// <returns>The calculated size after applying the relative adjustments.</returns>
        public SizeD GetSizeD(SizeD size)
        {
            return new SizeD(GetSize(size.Width), GetSize(size.Height));
        }

        /// <summary>
        /// Calculates the actual size based on the specified base size and the relative size settings.
        /// </summary>
        /// <param name="size">The base size to apply the relative adjustments to.</param>
        /// <returns>The calculated size after applying the relative adjustments.</returns>
        public SizeI GetSizeI(SizeI size)
        {
            return new SizeI((int)GetSize(size.Width), (int)GetSize(size.Height));
        }

        /// <summary>
        /// Determines whether the specified <see cref="RelativeSize"/> is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="RelativeSize"/> to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified <see cref="RelativeSize"/> is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public bool Equals(RelativeSize other)
        {
            return this == other;
        }
    }

}
