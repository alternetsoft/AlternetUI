using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// A simple class which stores hue, saturation and value as doubles in the range 0.0-1.0.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [DebuggerDisplay("{AsDebugString}")]
    public struct HSVValue : IEquatable<HSVValue>
    {
        /// <summary>
        /// Hue component of a color.
        /// </summary>
        public double Hue;

        /// <summary>
        /// Saturation component of a color.
        /// </summary>
        public double Saturation;

        /// <summary>
        /// Value component of a color.
        /// </summary>
        public double Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="HSVValue"/>.
        /// </summary>
        public HSVValue(double h = 0.0, double s = 0.0, double v = 0.0)
        {
            this.Hue = h;
            this.Saturation = s;
            this.Value = v;
        }

        /// <summary>
        /// Gets color name and ARGB for the debug purposes.
        /// </summary>
        public readonly string AsDebugString => $"{{HSV=({Hue}, {Saturation}, {Value})}}";

        /// <summary>
        /// Tests whether two <see cref='HSVValue'/> objects are different.
        /// </summary>
        public static bool operator !=(HSVValue left, HSVValue right) => !(left == right);

        /// <summary>
        /// Tests whether two <see cref='HSVValue'/> objects are equal.
        /// </summary>
        public static bool operator ==(HSVValue left, HSVValue right) =>
            left.Hue == right.Hue && left.Saturation == right.Saturation && left.Value == right.Value;

        /// <summary>
        /// Gets the hash code for this <see cref='RectD'/>.
        /// </summary>
        public override readonly int GetHashCode() => (Hue, Saturation, Value).GetHashCode();

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString() => AsDebugString;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other; otherwise,
        /// <c>false</c>.</returns>
        public readonly bool Equals(HSVValue other) => this == other;

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='HSVValue'/> and
        /// is equal to this object
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is HSVValue value && Equals(value);
    }
}