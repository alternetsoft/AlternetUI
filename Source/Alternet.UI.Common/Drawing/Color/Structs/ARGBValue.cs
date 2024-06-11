using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// A simple class which stores alpha, red, green and blue values as 8 bit unsigned integers
    /// in the range of 0-255.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGBValue
    {
        /// <summary>
        /// Alpha component of a color.
        /// </summary>
        public byte A;

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
    }
}