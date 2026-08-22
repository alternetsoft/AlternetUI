using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies how to rotate and flip an image.
    /// </summary>
    public enum RotateFlipType
    {
        /// <summary>
        /// No rotation or flip is applied to the image. 
        /// </summary>
        RotateNoneFlipNone = 0,

        /// <summary>
        /// The image is rotated 90 degrees clockwise and no flip is applied.
        /// </summary>
        Rotate90FlipNone = 1,

        /// <summary>
        /// The image is rotated 180 degrees clockwise and no flip is applied.
        /// </summary>
        Rotate180FlipNone = 2,

        /// <summary>
        /// The image is rotated 270 degrees clockwise and no flip is applied. 
        /// </summary>
        Rotate270FlipNone = 3,

        /// <summary>
        /// The image is not rotated and is flipped horizontally.
        /// </summary>
        RotateNoneFlipX = 4,

        /// <summary>
        /// The image is rotated 90 degrees clockwise and is flipped horizontally.
        /// </summary>
        Rotate90FlipX = 5,

        /// <summary>
        /// The image is rotated 180 degrees clockwise and is flipped horizontally.
        /// </summary>
        Rotate180FlipX = 6,

        /// <summary>
        /// The image is rotated 270 degrees clockwise and is flipped horizontally.
        /// </summary>
        Rotate270FlipX = 7,

        /// <summary>
        /// The image is not rotated and is flipped vertically.
        /// </summary>
        RotateNoneFlipY = Rotate180FlipX,

        /// <summary>
        /// The image is rotated 90 degrees clockwise and is flipped vertically.
        /// </summary>
        Rotate90FlipY = Rotate270FlipX,

        /// <summary>
        /// The image is rotated 180 degrees clockwise and is flipped vertically.
        /// </summary>
        Rotate180FlipY = RotateNoneFlipX,

        /// <summary>
        /// The image is rotated 270 degrees clockwise and is flipped vertically.
        /// </summary>
        Rotate270FlipY = Rotate90FlipX,

        /// <summary>
        /// The image is not rotated and is flipped horizontally and vertically.
        /// </summary>
        RotateNoneFlipXY = Rotate180FlipNone,

        /// <summary>
        /// The image is rotated 90 degrees clockwise and is flipped horizontally and vertically.
        /// </summary>
        Rotate90FlipXY = Rotate270FlipNone,

        /// <summary>
        /// The image is rotated 180 degrees clockwise and is flipped horizontally and vertically.
        /// </summary>
        Rotate180FlipXY = RotateNoneFlipNone,

        /// <summary>
        /// The image is rotated 270 degrees clockwise and is flipped horizontally and vertically.
        /// </summary>
        Rotate270FlipXY = Rotate90FlipNone,
    }
}
