using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Helper class which works with <see cref="SKColorFilter"/>.
    /// </summary>
    public class SkiaColorFilter
    {
        /// <summary>
        /// Array of alpha components.
        /// </summary>
        public byte[] A = new byte[256];

        /// <summary>
        /// Array of red components.
        /// </summary>
        public byte[] R = new byte[256];

        /// <summary>
        /// Array of green components.
        /// </summary>
        public byte[] G = new byte[256];

        /// <summary>
        /// Array of blue components.
        /// </summary>
        public byte[] B = new byte[256];

        /// <summary>
        /// Creates <see cref="SKColorFilter"/> using
        /// <see cref="SKColorFilter.CreateTable(byte[], byte[], byte[], byte[])"/> method.
        /// </summary>
        /// <returns></returns>
        public virtual SKColorFilter CreateTableUsingARGB()
        {
            var filter = SKColorFilter.CreateTable(A, R, G, B);
            return filter;
        }
    }
}