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
    /// <typeparam name="T">Type of the item.</typeparam>
    public class TwoDimensionalBuffer<T> : BaseObject
    {
        private readonly T[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoDimensionalBuffer{T}"/> class.
        /// </summary>
        /// <param name="width">Horizontal size.</param>
        /// <param name="height">Vertical size.</param>
        public TwoDimensionalBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            data = new T[width * height];
        }

        /// <summary>
        /// Get an array with data.
        /// </summary>
        public T[] Data
        {
            get
            {
                return data;
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
        /// Gets the offset in data for the specified X and Y.
        /// Returns -1 if X is outside [0, Width - 1] or Y is outside [0, Height - 1].
        /// </summary>
        /// <param name="x">X coordinate of the cell.</param>
        /// <param name="y">Y coordinate of the cell.</param>
        /// <returns></returns>
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
        public void SetData(int offset, T data)
        {
            if(IsValidOffset(offset))
                this.data[offset] = data;
        }

        /// <summary>
        /// Gets whether the specified offset is valid.
        /// </summary>
        /// <param name="offset">Offset to validate.</param>
        /// <returns></returns>
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
        public void SetData(int x, int y, T data)
        {
            var offset = GetOffset(x, y);
            if(offset >= 0)
                this.data[offset] = data;
        }

        /// <summary>
        /// Gets data from the cell specified with the offset.
        /// </summary>
        /// <param name="offset">Offset of the data cell.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetData(int offset)
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
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetData(int x, int y)
        {
            var offset = GetOffset(x, y);
            if (offset >= 0)
                return data[offset];
            return default!;
        }
    }
}