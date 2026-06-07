using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of images in different resolutions based on the same svg image.
    /// Images are generated from svg on demand and are cached in the set.
    /// </summary>
    public partial class SvgImageSet : ImageSet
    {
        private readonly SvgImage svgImage;
        private readonly SizeI baseSize;
        private readonly bool? isDarkTheme;
        private readonly Color? color;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified SVG image,
        /// base size and color theme.
        /// </summary>
        /// <param name="svgImage">The SVG image to use for the image set.</param>
        /// <param name="baseSize">The base size of the images in the set.</param>
        /// <param name="isDarkTheme">Indicates whether the image set is for a dark theme.</param>
        public SvgImageSet(SvgImage svgImage, SizeI baseSize, bool isDarkTheme)
        {
            this.isDarkTheme = isDarkTheme;
            this.svgImage = svgImage;
            this.baseSize = baseSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified stream with SVG data,
        /// base size and optional color.
        /// </summary>
        /// <param name="stream">The stream containing the SVG data.</param>
        /// <param name="baseSize">The base size of the images in the set.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        /// <param name="throwException">Indicates whether to throw an exception on error. Optional.</param>
        public SvgImageSet(
            Stream stream,
            SizeI baseSize,
            Color? color = null,
            bool throwException = true)
        {
            this.baseSize = baseSize;
            this.color = color;

            if (color is null)
                svgImage = new ColorSvgImage(stream, throwException);
            else
                svgImage = new MonoSvgImage(stream, throwException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified SVG image
        /// loaded from url or data string, base size, and optional color.
        /// </summary>
        /// <param name="urlOrData">The URL or data of the SVG image.</param>
        /// <param name="baseSize">The base size of the images in the set.</param>
        /// <param name="kind">The kind of SVG image data.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        /// <param name="throwException">Indicates whether to throw an exception on error. Optional.</param>
        public SvgImageSet(
            string urlOrData,
            SizeI baseSize,
            SvgImageDataKind kind = SvgImageDataKind.Url,
            Color? color = null,
            bool throwException = true)
        {
            this.baseSize = baseSize;
            this.color = color;

            if (color is null)
                svgImage = new ColorSvgImage(urlOrData, kind, throwException);
            else
                svgImage = new MonoSvgImage(urlOrData, kind, throwException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified SVG image,
        /// base size and optional color.
        /// </summary>
        /// <param name="svg">The SVG image to use for the image set.</param>
        /// <param name="baseSize">The base size of the images in the set.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        public SvgImageSet(SvgImage svg, SizeI baseSize, Color? color = null)
        {
            this.svgImage = svg;
            this.baseSize = baseSize;
            this.color = color;
        }

        /// <inheritdoc/>
        public override SizeI DefaultSize
        {
            get
            {
                return baseSize;
            }
        }

        /// <inheritdoc/>
        public override Image? GetExactImage(SizeI size)
        {
            var result = base.GetExactImage(size);

            result ??= AddSvg(svgImage, size, isDarkTheme, color);

            return result;
        }
    }
}
