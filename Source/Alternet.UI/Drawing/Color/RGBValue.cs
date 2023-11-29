using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [DebuggerDisplay("{AsDebugString}")]
    public struct RGBValue
    {
        /// <summary>
        /// Red component of a color.
        /// </summary>
        public byte R;

        /// <summary>
        /// Green component of a color.
        /// </summary>
        public byte G;

        /// <summary>
        /// Blue component of a color.
        /// </summary>
        public byte B;

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
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Gets color name and ARGB for the debug purposes.
        /// </summary>
        public string AsDebugString => $"{{RGB=({R}, {G}, {B})}}";

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => AsDebugString;
    }
}
