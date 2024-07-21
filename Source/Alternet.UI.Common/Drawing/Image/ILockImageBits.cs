using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to lock/unlock image pixels and
    /// to get pixel format.
    /// </summary>
    public interface ILockImageBits
    {
        /// <summary>
        /// Gets image width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets image height.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets <see cref="ImageBitsFormatKind"/> which identifies pixel format.
        /// </summary>
        ImageBitsFormatKind BitsFormat { get; }

        /// <summary>
        /// Gets type of the alpha component.
        /// </summary>
        SKAlphaType AlphaType { get; }

        /// <summary>
        /// Locks image pixels.
        /// </summary>
        IntPtr LockBits();

        /// <summary>
        /// Unlocks image pixels.
        /// </summary>
        void UnlockBits();

        /// <summary>
        /// Gets row stride.
        /// </summary>
        /// <returns></returns>
        int GetStride();
    }
}
