using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Alternet.Drawing;

/// <summary>
/// Represents a range of characters in a string, defined by a starting position and a length.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct CharacterRange : IEquatable<CharacterRange>
{
    private int first;
    private int length;

    /// <summary>
    /// Initializes a new instance of the <see cref='CharacterRange'/> class with the specified coordinates.
    /// </summary>
    public CharacterRange(int first, int length)
    {
        this.first = first;
        this.length = length;
    }

    /// <summary>
    /// Gets the first character position of this <see cref='CharacterRange'/>.
    /// </summary>
    public int First
    {
        readonly get => first;
        set => first = value;
    }

    /// <summary>
    /// Gets the length of this <see cref='CharacterRange'/>.
    /// </summary>
    public int Length
    {
        readonly get => length;
        set => length = value;
    }

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is CharacterRange other && Equals(other);
    }

    /// <summary>
    /// Indicates whether the current instance is equal to another instance of the same type.
    /// </summary>
    /// <param name="other">An instance to compare with this instance.</param>
    /// <returns>true if the current instance is equal to the other instance; otherwise, false.</returns>
    public readonly bool Equals(CharacterRange other) => First == other.First && Length == other.Length;

    /// <summary>
    /// Operartor == for <see cref='CharacterRange'/>.
    /// Indicates whether two instances of <see cref='CharacterRange'/> are equal.
    /// </summary>
    /// <param name="cr1"></param>
    /// <param name="cr2"></param>
    /// <returns></returns>
    public static bool operator ==(CharacterRange cr1, CharacterRange cr2) => cr1.Equals(cr2);

    /// <summary>
    /// Operartor != for <see cref='CharacterRange'/>.
    /// Indicates whether two instances of <see cref='CharacterRange'/> are not equal.
    /// </summary>
    /// <param name="cr1"></param>
    /// <param name="cr2"></param>
    /// <returns></returns>
    public static bool operator !=(CharacterRange cr1, CharacterRange cr2) => !cr1.Equals(cr2);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => (First, Length).GetHashCode();
}
