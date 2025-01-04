using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a selection range.
    /// </summary>
    /// <typeparam name="T">Type of the range value.</typeparam>
    internal readonly struct SelectionRange<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// Gets or sets the starting value of the selection range.
        /// </summary>
        /// <returns>The starting value of the range.</returns>
        public readonly T Start = default;

        /// <summary>
        /// Gets or sets the ending value of the selection range.
        /// </summary>
        /// <returns>The ending value of the range.</returns>
        public readonly T End = default;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionRange{T}"/> struct.
        /// </summary>
        public SelectionRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionRange{T}"/> struct
        /// with the specified beginning and ending values.
        /// </summary>
        /// <param name="lower">The starting value in the range.</param>
        /// <param name="upper">The ending value in the range.</param>
        public SelectionRange(T lower, T upper)
        {
            if (lower.CompareTo(upper) < 0)
            {
                Start = lower;
                End = upper;
            }
            else
            {
                Start = upper;
                End = lower;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionRange{T}" /> struct
        /// with the specified selection range.
        /// </summary>
        /// <param name="range">The existing <see cref="SelectionRange{T}" />.</param>
        public SelectionRange(SelectionRange<T> range)
        {
            Start = range.Start;
            End = range.End;
        }

        public enum ValuePosition
        {
            BelowStart,

            Start,

            Inside,

            End,

            AfterEnd,
        }

        public bool Multiple
        {
            get
            {
                return Start.CompareTo(End) != 0;
            }
        }

        public ValuePosition GetPosition(T value)
        {
            var result = value.CompareTo(Start);
            if (result < 0)
                return ValuePosition.BelowStart;
            if (result == 0)
                return ValuePosition.Start;

            result = value.CompareTo(End);

            if (result > 0)
                return ValuePosition.AfterEnd;
            if (result == 0)
                return ValuePosition.End;

            return ValuePosition.Inside;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsBeforeStart(T value)
        {
            var result = value.CompareTo(Start);
            return result < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsAfterEnd(T value)
        {
            var result = value.CompareTo(End);
            return result > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsOutside(T value)
        {
            return IsBeforeStart(value) || IsAfterEnd(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInside(T value)
        {
            return !IsOutside(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsStart(T value)
        {
            return value.CompareTo(Start) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnd(T value)
        {
            return value.CompareTo(End) == 0;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="SelectionRange{T}" />.
        /// </summary>
        /// <returns>A string that represents the current <see cref="SelectionRange{T}" />.</returns>
        public override readonly string ToString()
        {
            return "SelectionRange: Start: " + Start.ToString() + ", End: " + End.ToString();
        }
    }
}
