using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Array of 64 bits.
    /// </summary>
    public struct BitArray64 : IEquatable<BitArray64>
    {
        /// <summary>
        /// Gets or sets bits as <see cref="ulong"/>.
        /// </summary>
        public ulong Bits;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray64"/> struct.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitArray64()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray64"/> struct.
        /// </summary>
        /// <param name="bitValue">All bits value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitArray64(bool bitValue)
        {
            if(bitValue)
                SetAllBits();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitArray64"/> struct.
        /// </summary>
        /// <param name="bits">Bits to make up the array.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitArray64(ulong bits)
        {
            Bits = bits;
        }

        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        /// <value>
        /// The length of the array. Always returns 64.
        /// </value>
        public readonly int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return 64;
            }
        }

        /// <summary>
        /// Gets or sets the bit at the specified index.
        /// </summary>
        /// <param name="index">Index of the bit to get or set.</param>
        public bool this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                AssertIndex(index);
                ulong mask = 1UL << index;
                return (Bits & mask) == mask;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                AssertIndex(index);
                ulong mask = 1UL << index;
                if (value)
                    Bits |= mask;
                else
                    Bits &= ~mask;
            }
        }

        /// <summary>
        /// Sets the specified bit to 1.
        /// </summary>
        /// <param name="index">Index of the bit to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBit(int index)
        {
            AssertIndex(index);
            ulong mask = 1ul << index;
            Bits |= mask;
        }

        /// <summary>
        /// Sets all bits to 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAllBits()
        {
            Bits = 0xFFFFFFFFFFFFFFFFUL;
        }

        /// <summary>
        /// Sets all bits to the specified value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAllBits(bool value)
        {
            if (value)
                Bits = 0xFFFFFFFFFFFFFFFFUL;
            else
                Bits = 0;
        }

        /// <summary>
        /// Sets all bits to 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAllBits()
        {
            Bits = 0;
        }

        /// <summary>
        /// Sets the specified bit to 0.
        /// </summary>
        /// <param name="index">Index of the bit to unset.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBit(int index)
        {
            AssertIndex(index);
            ulong mask = 1ul << index;
            Bits &= ~mask;
        }

        /// <summary>
        /// Gets all the bits that match a mask.
        /// </summary>
        /// <param name="mask">Mask of bits to get.</param>
        /// <returns>
        /// The bits that match the specified mask.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong GetBits(ulong mask)
        {
            return Bits & mask;
        }

        /// <summary>
        /// Sets all the bits that match a mask to 1.
        /// </summary>
        /// <param name="mask">Mask of bits to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBits(ulong mask)
        {
            Bits |= mask;
        }

        /// <summary>
        /// Sets all the bits that match a mask to 0.
        /// </summary>
        /// <param name="mask">Mask of bits to clear.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearBits(ulong mask)
        {
            Bits &= ~mask;
        }

        /// <summary>
        /// Checks if this object equals another object.
        /// </summary>
        /// <param name="obj">Object to compare with. May be null.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified object is a <see cref="BitArray64"/> and its bits
        /// are the same as this objects's bits; <c>false</c> otherwise.
        /// </returns>
        public readonly override bool Equals(object? obj)
        {
            return obj is BitArray64 array && Bits == array.Bits;
        }

        /// <summary>
        /// Check if this object equals another object.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>
        /// <c>true</c> if bits of another object
        /// are the same as this objects's bits; <c>false</c> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(BitArray64 obj)
        {
            return Bits == obj.Bits;
        }

        /// <summary>
        /// Gets the hash code of this object.
        /// </summary>
        /// <returns>
        /// The hash code of this object, which is the same as
        /// the hash code of <see cref="Bits"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return Bits.GetHashCode();
        }

        /// <summary>
        /// Gets a string of bits (contains 64 charactars, equal to 0 or 1).
        /// </summary>
        /// <returns></returns>
        public readonly string ToBitsString()
        {
            char[] chars = new char[Length];
            int i = 0;

            for (ulong num = 1ul << 63; num > 0; num >>= 1)
            {
                chars[i] = (Bits & num) != 0 ? '1' : '0';
                i++;
            }

            return new string(chars);
        }

        /// <summary>
        /// Gets a string representation of the object.
        /// </summary>
        /// <returns>
        /// A newly-allocated string representing the bits of the array.
        /// </returns>
        public readonly override string ToString()
        {
            return $"BitArray64{{{ToBitsString()}}}";
        }

        /// <summary>
        /// Asserts if the given index isn't in valid bounds.
        /// </summary>
        /// <param name="index">Index to check.</param>
        [Conditional("DEBUG")]
        public readonly void AssertIndex(int index)
        {
            Debug.Assert(
                index >= 0 && index < 64,
                "Index out of bounds: " + index);
        }
    }
}