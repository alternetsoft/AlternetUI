using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements light and dark color pair.
    /// </summary>
    public partial class LightDarkColor : IEquatable<LightDarkColor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class.
        /// </summary>
        /// <param name="light">Light color.</param>
        /// <param name="dark">Dark color.</param>
        public LightDarkColor(Color light, Color dark)
        {
            Light = light;
            Dark = dark;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightDarkColor"/> class.
        /// </summary>
        /// <param name="color">Light and dark color.</param>
        public LightDarkColor(Color color)
        {
            Light = color;
            Dark = color;
        }

        /// <summary>
        /// Gets light color.
        /// </summary>
        public virtual Color Light { get; set; }

        /// <summary>
        /// Gets dark color.
        /// </summary>
        public virtual Color Dark { get; set; }

        /// <summary>
        /// Conversion operator from <see cref="Color"/> to <see cref="LightDarkColor"/>.
        /// This operator creates <see cref="LightDarkColor"/> with the same values for
        /// the dark and light colors filled from the specified <see cref="Color"/>.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator LightDarkColor?(Color? value)
        {
            if (value is null)
                return null;
            return new(value);
        }

        /// <summary>
        /// Tests whether two specified <see cref="LightDarkColor"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="LightDarkColor"/> that is to the left
        /// of the inequality operator.</param>
        /// <param name="right">The <see cref="LightDarkColor"/> that is to the right
        /// of the inequality operator.</param>
        /// <returns><c>true</c> if the two <see cref="Color"/> structures
        /// are different; otherwise, <c>false</c>.</returns>
        public static bool operator !=(LightDarkColor? left, LightDarkColor? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Tests whether two specified <see cref="LightDarkColor"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="LightDarkColor"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="LightDarkColor"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="LightDarkColor"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(LightDarkColor? left, LightDarkColor? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Dark == right.Dark
                && left.Light == right.Light;
        }

        /// <summary>
        /// Gets dark or light color depending on the <paramref name="isDark"/>
        /// parameter value.
        /// </summary>
        /// <param name="isDark">Whether to get dark or light color.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Color Get(bool isDark)
        {
            Color result;

            if (isDark)
            {
                result = Dark;
            }
            else
            {
                result = Light;
            }

            return result;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is LightDarkColor color && Equals(color);
        }

        /// <inheritdoc/>
        public bool Equals(LightDarkColor other)
        {
            return this == other;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                Dark?.GetHashCode(),
                Light?.GetHashCode());
        }
    }
}
