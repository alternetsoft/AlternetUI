using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Contains row and column indexes.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct RowColumnIndex : IEquatable<RowColumnIndex>
    {
        /// <summary>
        /// Gets default value with row and column equal to zero.
        /// </summary>
        public static readonly RowColumnIndex Default = new();

        [FieldOffset(0)]
        private readonly ulong rowColumn;

        [FieldOffset(0)]
        private int row;

        [FieldOffset(4)]
        private int column;

        /// <summary>
        /// Initializes a new instance of the <see cref="RowColumnIndex"/> struct.
        /// </summary>
        public RowColumnIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowColumnIndex"/> struct.
        /// </summary>
        /// <param name="rowIndex">Row index value.</param>
        /// <param name="columnIndex">Column index value.</param>
        public RowColumnIndex(int rowIndex, int columnIndex)
        {
            row = rowIndex;
            column = columnIndex;
        }

        /// <summary>
        /// Gets row index.
        /// </summary>
        public int Row { readonly get => row; set => row = value; }

        /// <summary>
        /// Gets column index.
        /// </summary>
        public int Column { readonly get => column; set => column = value; }

        /// <summary>
        /// Creates a <see cref='RowColumnIndex'/> with the coordinates of
        /// the specified <see cref='PointI'/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator RowColumnIndex(PointI p) => new(p.X, p.Y);

        /// <summary>
        /// Implicit operator convertion from tuple with two <see cref="int"/> values
        /// to <see cref="RowColumnIndex"/>.
        /// </summary>
        /// <param name="d">New value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RowColumnIndex((int Row, int Column) d) => new(d.Row, d.Column);

        /// <summary>
        /// Compares two <see cref='RowColumnIndex'/> objects. The result
        /// specifies whether the values of the
        /// <see cref='Row'/> and
        /// <see cref='Column'/> properties of the two
        /// <see cref='RowColumnIndex'/> objects are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RowColumnIndex left, RowColumnIndex right)
            => left.rowColumn == right.rowColumn;

        /// <summary>
        /// Compares two <see cref='RowColumnIndex'/> objects.
        /// The result specifies whether the values of the
        /// <see cref='Row'/> or
        /// <see cref='Column'/> properties of the two
        /// <see cref='RowColumnIndex'/>  objects are unequal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RowColumnIndex left, RowColumnIndex right)
            => left.rowColumn != right.rowColumn;

        /// <summary>
        /// Returns an instance converted from the provided string using
        /// <see cref="App.InvariantEnglishUS"/> culture.
        /// <param name="source">String with data.</param>
        /// </summary>
        public static RowColumnIndex Parse(string source)
        {
            IFormatProvider formatProvider = App.InvariantEnglishUS;

            TokenizerHelper th = new(source, formatProvider);

            string firstToken = th.NextTokenRequired();

            var value = new RowColumnIndex(
                Convert.ToInt32(firstToken, formatProvider),
                Convert.ToInt32(th.NextTokenRequired(), formatProvider));

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        /// <summary>
        /// Specifies whether this <see cref='RowColumnIndex'/> contains
        /// the same coordinates as the specified
        /// <see cref='object'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
            obj is RowColumnIndex point && Equals(point);

        /// <summary>
        /// Indicates whether the current object is equal to another
        /// object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(RowColumnIndex other) => this == other;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override readonly int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }

        /// <summary>
        /// Converts this object to a human readable string.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = { PropNameStrings.Default.Row, PropNameStrings.Default.Column };
            int[] values = { Row, Column };

            return StringUtils.ToStringWithOrWithoutNames<int>(names, values);
        }
    }
}
