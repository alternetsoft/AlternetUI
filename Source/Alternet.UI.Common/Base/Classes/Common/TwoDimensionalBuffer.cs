using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements two dimensional buffer which has width and height.
    /// Items of this buffer can be accessed using X and Y indexes.
    /// </summary>
    /// <typeparam name="TItem">Type of the item.</typeparam>
    public class TwoDimensionalBuffer<TItem> : BaseObject
    {
        private readonly TItem[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoDimensionalBuffer{T}"/> class.
        /// </summary>
        /// <param name="width">Horizontal size.</param>
        /// <param name="height">Vertical size.</param>
        public TwoDimensionalBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            data = new TItem[width * height];
        }

        /// <summary>
        /// Get an array with data.
        /// </summary>
        public TItem[] Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Gets or sets the item at the specified offset.
        /// </summary>
        /// <param name="offset">Offset of the item.</param>
        /// <returns>The item at the specified offset.</returns>
        public TItem this[int offset]
        {
            get
            {
                return data[offset];
            }

            set
            {
                data[offset] = value;
            }
        }

        /// <summary>
        /// Gets or sets the item at the specified X and Y indexes.
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <returns>The item at the specified coordinates.</returns>
        public TItem this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GetData(x, y);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                SetData(x, y, value);
            }
        }

        /// <summary>
        /// Gets horizontal size (row width, number of columns).
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets vertical size (number of rows).
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Enumerates all items in the specified row.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>An enumerable collection of items in the specified row.</returns>
        public IEnumerable<TItem> GetRowItems(int rowIndex)
        {
            for (int i = 0; i < Width; i++)
            {
                yield return this[i, rowIndex];
            }
        }

        /// <summary>
        /// Enumerates all items in the specified column.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>An enumerable collection of items in the specified column.</returns>
        public IEnumerable<TItem> GetColumnItems(int columnIndex)
        {
            for (int i = 0; i < Height; i++)
            {
                yield return this[columnIndex, i];
            }
        }

        /// <summary>
        /// Gets the item at the specified X and Y indexes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TItem> GetItems()
        {
            return data;
        }

        /// <summary>
        /// Gets the offset in data for the specified X and Y.
        /// Returns -1 if X is outside [0, Width - 1] or Y is outside [0, Height - 1].
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <returns>The offset in the data array for the specified coordinates,
        /// or -1 if the coordinates are out of bounds.</returns>
        public int GetOffset(int x, int y)
        {
            if (x < 0 || x >= Width)
                return -1;
            if (y < 0 || y >= Height)
                return -1;

            return (y * Width) + x;
        }

        /// <summary>
        /// Sets data in the cell specified with the offset.
        /// </summary>
        /// <param name="offset">Offset of the data cell.</param>
        /// <param name="data">Data to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetData(int offset, TItem data)
        {
            if(IsValidOffset(offset))
                this.data[offset] = data;
        }

        /// <summary>
        /// Gets whether the specified offset is valid.
        /// </summary>
        /// <param name="offset">Offset to validate.</param>
        /// <returns><c>true</c> if the offset is valid; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValidOffset(int offset)
        {
            if (offset < 0 || offset >= this.data.Length)
                return false;
            return true;
        }

        /// <summary>
        /// Sets data in the cell specified by X and Y coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <param name="data">Data to set.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetData(int x, int y, TItem data)
        {
            var offset = GetOffset(x, y);
            if(offset >= 0)
                this.data[offset] = data;
        }

        /// <summary>
        /// Gets data from the cell specified with the offset.
        /// </summary>
        /// <param name="offset">Offset of the data cell.</param>
        /// <returns>The data at the specified offset, or the default value if the offset is invalid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItem GetData(int offset)
        {
            if (IsValidOffset(offset))
                return data[offset];
            return default!;
        }

        /// <summary>
        /// Gets data from the cell specified with X and Y coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <returns>The data at the specified coordinates, or the default
        /// value if the coordinates are out of bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItem GetData(int x, int y)
        {
            var offset = GetOffset(x, y);
            if (offset >= 0)
                return data[offset];
            return default!;
        }
    }
}