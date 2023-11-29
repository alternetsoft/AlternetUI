using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// A simple class which stores red, green and blue values as 8 bit unsigned integers
    /// in the range of 0-255.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBValue
    {
        /// <summary>
        /// Red component of a color.
        /// </summary>
        public byte Red;

        /// <summary>
        /// Green component of a color.
        /// </summary>
        public byte Green;

        /// <summary>
        /// Blue component of a color.
        /// </summary>
        public byte Blue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBValue"/>.
        /// </summary>
        public RGBValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBValue"/>.
        /// </summary>
        public RGBValue(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }
    }
}
