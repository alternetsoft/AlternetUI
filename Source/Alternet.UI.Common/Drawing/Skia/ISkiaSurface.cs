using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains properties and methods which help to use
    /// <see cref="SKCanvas"/> over the bitmap pixels.
    /// </summary>
    public interface ISkiaSurface : IDisposable
    {
        /// <summary>
        /// Gets image lock mode.
        /// </summary>
        ImageLockMode LockMode { get; }

        /// <summary>
        /// Gets image width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets image height.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets color type which describes how to interpret the components of a pixel.
        /// </summary>
        SKColorType ColorType { get; }

        /// <summary>
        /// Gets alpha type which describes how to interpret the alpha component of a pixel.
        /// </summary>
        SKAlphaType AlphaType { get; }

        /// <summary>
        /// Gets <see cref="SKSurface"/>.
        /// </summary>
        SKSurface? Surface { get; }

        /// <summary>
        /// Gets <see cref="SKBitmap"/>.
        /// </summary>
        SKBitmap? Bitmap { get; }

        /// <summary>
        /// Gets <see cref="SKCanvas"/> which can be used for the painting.
        /// </summary>
        SKCanvas Canvas { get; }

        /// <summary>
        /// Gets whether object is ok.
        /// </summary>
        bool IsOk { get; }
    }
}
