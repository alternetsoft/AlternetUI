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
        /// Specifies the type of pixel data over which a surface is created.
        /// </summary>
        /// <remarks>This enumeration defines the different kinds of pixel data that can be used as the
        /// basis for a surface. It is typically used to indicate the source or format of the
        /// underlying pixel data when
        /// creating or working with a surface.</remarks>
        public enum SurfaceKind
        {
            /// <summary>
            /// Surface is created over an unknown pixel source.
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Surface is created over the native bitmap pixels.
            /// </summary>
            NativeBitmap,

            /// <summary>
            /// Surface is created over the generic image pixels.
            /// </summary>
            GenericImage,

            /// <summary>
            /// Surface is created over the Skia bitmap pixels.
            /// </summary>
            SkiaBitmap,
        }

        /// <summary>
        /// Gets the type of surface represented by this instance.
        /// </summary>
        SurfaceKind Kind { get; }

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
