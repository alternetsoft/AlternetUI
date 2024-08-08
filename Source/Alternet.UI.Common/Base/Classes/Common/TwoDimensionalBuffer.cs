using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class TwoDimensionalBuffer<T>
    {
        private readonly T[] data;

        public T[] Data
        {
            get
            {
                return data;
            }
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public TwoDimensionalBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            data = new T[width * height];
        }

        public int GetOffset(int x, int y)
        {
            return (MathUtils.Clamp(y, 0, Height - 1) * Width) + MathUtils.Clamp(x, 0, Width - 1);
        }

        private int EnsureValidOffset(int offset)
        {
            return MathUtils.Clamp(offset, 0, data.Length);
        }

        public void SetData(int offset, T data)
        {
            this.data[EnsureValidOffset(offset)] = data;
        }

        public void SetData(int x, int y, T data)
        {
            this.data[GetOffset(x, y)] = data;
        }

        public T GetData(int offset)
        {
            return data[EnsureValidOffset(offset)];
        }

        public T GetData(int x, int y)
        {
            return data[GetOffset(x, y)];
        }
    }
}