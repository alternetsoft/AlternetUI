using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a known button image declaration with optional size and alignment.
    /// </summary>
    public struct KnownButtonImage
    {
        /// <summary>
        /// Represents a transparent button image with no visible content.
        /// </summary>
        /// <remarks>This static field provides a predefined instance of a button image
        /// that is fully transparent. It can be used as a placeholder or default value
        /// when no image is required.</remarks>
        public static readonly KnownButtonImage Transparent = new(KnownSvgImages.ImgEmpty);

        /// <summary>
        /// Represents an empty or uninitialized button image.
        /// </summary>
        /// <remarks>This static readonly field provides a default instance
        /// of the <see cref="KnownButtonImage"/> class that represents an empty
        /// or uninitialized state. It can be used as a
        /// placeholder or to indicate the absence of a specific button image.</remarks>
        public static readonly KnownButtonImage Empty = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownButtonImage"/> class.
        /// </summary>
        public KnownButtonImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownButtonImage"/> struct
        /// with the specified known button and optional size.
        /// </summary>
        /// <param name="knownButton">The known button.</param>
        /// <param name="size">The optional size of the image.</param>
        public KnownButtonImage(KnownButton knownButton, CoordValue? size = null)
        {
            KnownButton = knownButton;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownButtonImage"/> struct
        /// with the specified SVG image and optional size.
        /// </summary>
        /// <param name="svgImage">The SVG image.</param>
        /// <param name="size">The optional size of the image.</param>
        public KnownButtonImage(SvgImage? svgImage, CoordValue? size = null)
        {
            SvgImage = svgImage;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownButtonImage"/> struct
        /// with the specified known button, SVG image, and size.
        /// </summary>
        /// <param name="knownButton">The known button.</param>
        /// <param name="svgImage">The SVG image.</param>
        /// <param name="size">The size of the image.</param>
        public KnownButtonImage(
            KnownButton? knownButton,
            SvgImage? svgImage,
            CoordValue? size)
        {
            KnownButton = knownButton;
            SvgImage = svgImage;
            Size = size;
        }

        /// <summary>
        /// Gets or sets the known button value.
        /// </summary>
        public KnownButton? KnownButton { get; set; }

        /// <summary>
        /// Gets or sets the SVG image value.
        /// </summary>
        public SvgImage? SvgImage { get; set; }

        /// <summary>
        /// Gets or sets the size of the image.
        /// </summary>
        public CoordValue? Size { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the image.
        /// </summary>
        public HVAlignment? Alignment { get; set; }

        /// <summary>
        /// Implicitly converts a <see cref="KnownButton"/> to a <see cref="KnownButtonImage"/>.
        /// </summary>
        /// <param name="knownButton">The <see cref="KnownButton"/> instance to convert.</param>
        public static implicit operator KnownButtonImage(KnownButton knownButton)
        {
            return new KnownButtonImage(knownButton);
        }

        /// <summary>
        /// Defines an implicit conversion from an <see cref="SvgImage"/>
        /// to a <see cref="KnownButtonImage"/>.
        /// </summary>
        /// <param name="svgImage">The <see cref="SvgImage"/> instance to convert.
        /// Can be <see langword="null"/>.</param>
        public static implicit operator KnownButtonImage(SvgImage? svgImage)
        {
            return new KnownButtonImage(svgImage);
        }
    }
}
